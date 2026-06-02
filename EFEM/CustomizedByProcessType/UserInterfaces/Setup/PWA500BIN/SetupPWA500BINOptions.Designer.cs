
namespace EFEM.CustomizedByProcessType.UserInterface.Setup.PWA500BIN
{
    partial class SetupPWA500BINOptions
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
            this.customParameterLabel1 = new FrameOfSystem3.Component.CustomParameterLabel();
            this.lblHandlingWaitTime = new FrameOfSystem3.Component.CustomParameterLabel();
            this.ToggleUseLowerArm = new FrameOfSystem3.Component.CustomParameterToggleButton();
            this.ToggleUseUpperArm = new FrameOfSystem3.Component.CustomParameterToggleButton();
            this.sys3Label5 = new Sys3Controls.Sys3Label();
            this.sys3Label2 = new Sys3Controls.Sys3Label();
            this.sys3Label3 = new Sys3Controls.Sys3Label();
            this.sys3Label4 = new Sys3Controls.Sys3Label();
            this.sys3Label1 = new Sys3Controls.Sys3Label();
            this.toggleUseCycleMode = new FrameOfSystem3.Component.CustomParameterToggleButton();
            this.gbTitle = new Sys3Controls.Sys3GroupBoxContainer();
            this.tableLayoutPanel1.SuspendLayout();
            this.gbTitle.SuspendLayout();
            this.SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 2;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 70.19749F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 29.80251F));
            this.tableLayoutPanel1.Controls.Add(this.customParameterLabel1, 1, 4);
            this.tableLayoutPanel1.Controls.Add(this.lblHandlingWaitTime, 1, 3);
            this.tableLayoutPanel1.Controls.Add(this.ToggleUseLowerArm, 1, 2);
            this.tableLayoutPanel1.Controls.Add(this.ToggleUseUpperArm, 1, 1);
            this.tableLayoutPanel1.Controls.Add(this.sys3Label5, 0, 4);
            this.tableLayoutPanel1.Controls.Add(this.sys3Label2, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.sys3Label3, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.sys3Label4, 0, 2);
            this.tableLayoutPanel1.Controls.Add(this.sys3Label1, 0, 3);
            this.tableLayoutPanel1.Controls.Add(this.toggleUseCycleMode, 1, 0);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(3, 32);
            this.tableLayoutPanel1.Margin = new System.Windows.Forms.Padding(0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 6;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 16.66667F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 16.66667F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 16.66667F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 16.66667F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 16.66667F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 16.66667F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(548, 279);
            this.tableLayoutPanel1.TabIndex = 2;
            // 
            // customParameterLabel1
            // 
            this.customParameterLabel1.AssociatedMap = null;
            this.customParameterLabel1.BackGroundColor = System.Drawing.Color.White;
            this.customParameterLabel1.BorderStroke = 2;
            this.customParameterLabel1.BorderStyle = System.Windows.Forms.ButtonBorderStyle.Solid;
            this.customParameterLabel1.Description = "";
            this.customParameterLabel1.DisabledColor = System.Drawing.Color.DarkGray;
            this.customParameterLabel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.customParameterLabel1.EdgeRadius = 1;
            this.customParameterLabel1.ImagePosition = new System.Drawing.Point(0, 0);
            this.customParameterLabel1.ImageSize = new System.Drawing.Point(0, 0);
            this.customParameterLabel1.LoadImage = null;
            this.customParameterLabel1.Location = new System.Drawing.Point(389, 189);
            this.customParameterLabel1.MainFont = new System.Drawing.Font("맑은 고딕", 11F, System.Drawing.FontStyle.Bold);
            this.customParameterLabel1.MainFontColor = System.Drawing.Color.Black;
            this.customParameterLabel1.Margin = new System.Windows.Forms.Padding(5);
            this.customParameterLabel1.Modifiable = true;
            this.customParameterLabel1.Name = "customParameterLabel1";
            this.customParameterLabel1.NeedRemakeMap = false;
            this.customParameterLabel1.ParameterChangeDefaultColor = System.Drawing.Color.Black;
            this.customParameterLabel1.ParameterChangeWaitColor = System.Drawing.Color.Red;
            this.customParameterLabel1.ParameterDefaultValue = "";
            this.customParameterLabel1.ParameterIndex = 0;
            this.customParameterLabel1.ParameterMAX = "";
            this.customParameterLabel1.ParameterMIN = "";
            this.customParameterLabel1.ParameterName = "BinWaferStepId";
            this.customParameterLabel1.ParameterSettingType = FrameOfSystem3.Component.EN_LABEL_PARAMETER_TYPE.KEYBOARD;
            this.customParameterLabel1.ParameterStorage = "";
            this.customParameterLabel1.ParameterStored = false;
            this.customParameterLabel1.ParameterType = FrameOfSystem3.Recipe.EN_RECIPE_TYPE.EQUIPMENT;
            this.customParameterLabel1.ParameterUNIT = "";
            this.customParameterLabel1.SelectionList = Define.DefineEnumProject.SelectionList.EN_SELECTIONLIST.NONE;
            this.customParameterLabel1.Size = new System.Drawing.Size(154, 36);
            this.customParameterLabel1.SubFont = new System.Drawing.Font("맑은 고딕", 8F, System.Drawing.FontStyle.Bold);
            this.customParameterLabel1.SubFontColor = System.Drawing.Color.Gray;
            this.customParameterLabel1.SubText = "";
            this.customParameterLabel1.TabIndex = 21158;
            this.customParameterLabel1.Tag = "";
            this.customParameterLabel1.TaskName = "LoadPort1";
            this.customParameterLabel1.Text = "--";
            this.customParameterLabel1.TextAlignMain = Sys3Controls.EN_TEXTALIGN.MIDDLE_CENTER;
            this.customParameterLabel1.TextAlignSub = Sys3Controls.EN_TEXTALIGN.TOP_LEFT;
            this.customParameterLabel1.TextAlignUnit = Sys3Controls.EN_TEXTALIGN.MIDDLE_RIGHT;
            this.customParameterLabel1.ThemeIndex = 0;
            this.customParameterLabel1.UnitAreaRate = 40;
            this.customParameterLabel1.UnitFont = new System.Drawing.Font("맑은 고딕", 10F, System.Drawing.FontStyle.Bold);
            this.customParameterLabel1.UnitFontColor = System.Drawing.Color.Red;
            this.customParameterLabel1.UnitPositionVertical = false;
            this.customParameterLabel1.UnitText = "--";
            this.customParameterLabel1.UseBorder = true;
            this.customParameterLabel1.UseEdgeRadius = false;
            this.customParameterLabel1.UseImage = false;
            this.customParameterLabel1.UseParameterChangeConfirm = true;
            this.customParameterLabel1.UseSubFont = true;
            this.customParameterLabel1.UseUnitFont = false;
            // 
            // lblHandlingWaitTime
            // 
            this.lblHandlingWaitTime.AssociatedMap = null;
            this.lblHandlingWaitTime.BackGroundColor = System.Drawing.Color.White;
            this.lblHandlingWaitTime.BorderStroke = 2;
            this.lblHandlingWaitTime.BorderStyle = System.Windows.Forms.ButtonBorderStyle.Solid;
            this.lblHandlingWaitTime.Description = "";
            this.lblHandlingWaitTime.DisabledColor = System.Drawing.Color.DarkGray;
            this.lblHandlingWaitTime.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblHandlingWaitTime.EdgeRadius = 1;
            this.lblHandlingWaitTime.ImagePosition = new System.Drawing.Point(0, 0);
            this.lblHandlingWaitTime.ImageSize = new System.Drawing.Point(0, 0);
            this.lblHandlingWaitTime.LoadImage = null;
            this.lblHandlingWaitTime.Location = new System.Drawing.Point(389, 143);
            this.lblHandlingWaitTime.MainFont = new System.Drawing.Font("맑은 고딕", 11F, System.Drawing.FontStyle.Bold);
            this.lblHandlingWaitTime.MainFontColor = System.Drawing.Color.Black;
            this.lblHandlingWaitTime.Margin = new System.Windows.Forms.Padding(5);
            this.lblHandlingWaitTime.Modifiable = true;
            this.lblHandlingWaitTime.Name = "lblHandlingWaitTime";
            this.lblHandlingWaitTime.NeedRemakeMap = false;
            this.lblHandlingWaitTime.ParameterChangeDefaultColor = System.Drawing.Color.Black;
            this.lblHandlingWaitTime.ParameterChangeWaitColor = System.Drawing.Color.Red;
            this.lblHandlingWaitTime.ParameterDefaultValue = "";
            this.lblHandlingWaitTime.ParameterIndex = 0;
            this.lblHandlingWaitTime.ParameterMAX = "600000";
            this.lblHandlingWaitTime.ParameterMIN = "10000";
            this.lblHandlingWaitTime.ParameterName = "HandlingWaitTime";
            this.lblHandlingWaitTime.ParameterSettingType = FrameOfSystem3.Component.EN_LABEL_PARAMETER_TYPE.CALCULATE_UINT;
            this.lblHandlingWaitTime.ParameterStorage = "";
            this.lblHandlingWaitTime.ParameterStored = false;
            this.lblHandlingWaitTime.ParameterType = FrameOfSystem3.Recipe.EN_RECIPE_TYPE.EQUIPMENT;
            this.lblHandlingWaitTime.ParameterUNIT = "ms";
            this.lblHandlingWaitTime.SelectionList = Define.DefineEnumProject.SelectionList.EN_SELECTIONLIST.NONE;
            this.lblHandlingWaitTime.Size = new System.Drawing.Size(154, 36);
            this.lblHandlingWaitTime.SubFont = new System.Drawing.Font("맑은 고딕", 8F, System.Drawing.FontStyle.Bold);
            this.lblHandlingWaitTime.SubFontColor = System.Drawing.Color.Gray;
            this.lblHandlingWaitTime.SubText = "";
            this.lblHandlingWaitTime.TabIndex = 21156;
            this.lblHandlingWaitTime.Tag = "";
            this.lblHandlingWaitTime.TaskName = "LoadPort1";
            this.lblHandlingWaitTime.Text = "--";
            this.lblHandlingWaitTime.TextAlignMain = Sys3Controls.EN_TEXTALIGN.MIDDLE_RIGHT;
            this.lblHandlingWaitTime.TextAlignSub = Sys3Controls.EN_TEXTALIGN.TOP_LEFT;
            this.lblHandlingWaitTime.TextAlignUnit = Sys3Controls.EN_TEXTALIGN.MIDDLE_RIGHT;
            this.lblHandlingWaitTime.ThemeIndex = 0;
            this.lblHandlingWaitTime.UnitAreaRate = 40;
            this.lblHandlingWaitTime.UnitFont = new System.Drawing.Font("맑은 고딕", 10F, System.Drawing.FontStyle.Bold);
            this.lblHandlingWaitTime.UnitFontColor = System.Drawing.Color.Red;
            this.lblHandlingWaitTime.UnitPositionVertical = false;
            this.lblHandlingWaitTime.UnitText = "--";
            this.lblHandlingWaitTime.UseBorder = true;
            this.lblHandlingWaitTime.UseEdgeRadius = false;
            this.lblHandlingWaitTime.UseImage = false;
            this.lblHandlingWaitTime.UseParameterChangeConfirm = true;
            this.lblHandlingWaitTime.UseSubFont = true;
            this.lblHandlingWaitTime.UseUnitFont = true;
            // 
            // ToggleUseLowerArm
            // 
            this.ToggleUseLowerArm.Active = false;
            this.ToggleUseLowerArm.ActiveColorFirst = System.Drawing.Color.RoyalBlue;
            this.ToggleUseLowerArm.ActiveColorSecond = System.Drawing.Color.DodgerBlue;
            this.ToggleUseLowerArm.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.ToggleUseLowerArm.AssociatedMap = null;
            this.ToggleUseLowerArm.Location = new System.Drawing.Point(394, 95);
            this.ToggleUseLowerArm.Margin = new System.Windows.Forms.Padding(10, 1, 0, 0);
            this.ToggleUseLowerArm.Name = "ToggleUseLowerArm";
            this.ToggleUseLowerArm.NeedRemakeMap = false;
            this.ToggleUseLowerArm.NormalColorFirst = System.Drawing.Color.DarkGray;
            this.ToggleUseLowerArm.NormalColorSecond = System.Drawing.Color.Silver;
            this.ToggleUseLowerArm.ParameterChangeDefaultActiveColorFirst = System.Drawing.Color.RoyalBlue;
            this.ToggleUseLowerArm.ParameterChangeDefaultActiveColorSecond = System.Drawing.Color.DodgerBlue;
            this.ToggleUseLowerArm.ParameterChangeDefaultNormalColorFirst = System.Drawing.Color.DarkGray;
            this.ToggleUseLowerArm.ParameterChangeDefaultNormalColorSecond = System.Drawing.Color.Silver;
            this.ToggleUseLowerArm.ParameterChangeWaitColorFirst = System.Drawing.Color.DarkRed;
            this.ToggleUseLowerArm.ParameterChangeWaitColorSecond = System.Drawing.Color.Firebrick;
            this.ToggleUseLowerArm.ParameterIndex = 0;
            this.ToggleUseLowerArm.ParameterName = "UseRobotLowerArm";
            this.ToggleUseLowerArm.ParameterStorage = "False";
            this.ToggleUseLowerArm.ParameterStored = false;
            this.ToggleUseLowerArm.ParameterType = FrameOfSystem3.Recipe.EN_RECIPE_TYPE.EQUIPMENT;
            this.ToggleUseLowerArm.Size = new System.Drawing.Size(80, 40);
            this.ToggleUseLowerArm.TabIndex = 21154;
            this.ToggleUseLowerArm.TaskName = "Sample";
            this.ToggleUseLowerArm.UseParameterChangeConfirm = true;
            // 
            // ToggleUseUpperArm
            // 
            this.ToggleUseUpperArm.Active = false;
            this.ToggleUseUpperArm.ActiveColorFirst = System.Drawing.Color.RoyalBlue;
            this.ToggleUseUpperArm.ActiveColorSecond = System.Drawing.Color.DodgerBlue;
            this.ToggleUseUpperArm.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.ToggleUseUpperArm.AssociatedMap = null;
            this.ToggleUseUpperArm.Location = new System.Drawing.Point(394, 49);
            this.ToggleUseUpperArm.Margin = new System.Windows.Forms.Padding(10, 1, 0, 0);
            this.ToggleUseUpperArm.Name = "ToggleUseUpperArm";
            this.ToggleUseUpperArm.NeedRemakeMap = false;
            this.ToggleUseUpperArm.NormalColorFirst = System.Drawing.Color.DarkGray;
            this.ToggleUseUpperArm.NormalColorSecond = System.Drawing.Color.Silver;
            this.ToggleUseUpperArm.ParameterChangeDefaultActiveColorFirst = System.Drawing.Color.RoyalBlue;
            this.ToggleUseUpperArm.ParameterChangeDefaultActiveColorSecond = System.Drawing.Color.DodgerBlue;
            this.ToggleUseUpperArm.ParameterChangeDefaultNormalColorFirst = System.Drawing.Color.DarkGray;
            this.ToggleUseUpperArm.ParameterChangeDefaultNormalColorSecond = System.Drawing.Color.Silver;
            this.ToggleUseUpperArm.ParameterChangeWaitColorFirst = System.Drawing.Color.DarkRed;
            this.ToggleUseUpperArm.ParameterChangeWaitColorSecond = System.Drawing.Color.Firebrick;
            this.ToggleUseUpperArm.ParameterIndex = 0;
            this.ToggleUseUpperArm.ParameterName = "UseRobotUpperArm";
            this.ToggleUseUpperArm.ParameterStorage = "False";
            this.ToggleUseUpperArm.ParameterStored = false;
            this.ToggleUseUpperArm.ParameterType = FrameOfSystem3.Recipe.EN_RECIPE_TYPE.EQUIPMENT;
            this.ToggleUseUpperArm.Size = new System.Drawing.Size(80, 40);
            this.ToggleUseUpperArm.TabIndex = 21152;
            this.ToggleUseUpperArm.TaskName = "Sample";
            this.ToggleUseUpperArm.UseParameterChangeConfirm = true;
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
            this.sys3Label5.Location = new System.Drawing.Point(5, 189);
            this.sys3Label5.MainFont = new System.Drawing.Font("맑은 고딕", 11F, System.Drawing.FontStyle.Bold);
            this.sys3Label5.MainFontColor = System.Drawing.Color.Black;
            this.sys3Label5.Margin = new System.Windows.Forms.Padding(5);
            this.sys3Label5.Name = "sys3Label5";
            this.sys3Label5.Size = new System.Drawing.Size(374, 36);
            this.sys3Label5.SubFont = new System.Drawing.Font("맑은 고딕", 10F, System.Drawing.FontStyle.Bold);
            this.sys3Label5.SubFontColor = System.Drawing.Color.Gray;
            this.sys3Label5.SubText = "";
            this.sys3Label5.TabIndex = 21158;
            this.sys3Label5.Text = "BIN WAFER STEP ID";
            this.sys3Label5.TextAlignMain = Sys3Controls.EN_TEXTALIGN.MIDDLE_RIGHT;
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
            // sys3Label2
            // 
            this.sys3Label2.BackGroundColor = System.Drawing.Color.DarkGray;
            this.sys3Label2.BorderStroke = 2;
            this.sys3Label2.BorderStyle = System.Windows.Forms.ButtonBorderStyle.Solid;
            this.sys3Label2.Description = "";
            this.sys3Label2.DisabledColor = System.Drawing.Color.DarkGray;
            this.sys3Label2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.sys3Label2.EdgeRadius = 1;
            this.sys3Label2.ImagePosition = new System.Drawing.Point(0, 0);
            this.sys3Label2.ImageSize = new System.Drawing.Point(0, 0);
            this.sys3Label2.LoadImage = null;
            this.sys3Label2.Location = new System.Drawing.Point(5, 5);
            this.sys3Label2.MainFont = new System.Drawing.Font("맑은 고딕", 11F, System.Drawing.FontStyle.Bold);
            this.sys3Label2.MainFontColor = System.Drawing.Color.Black;
            this.sys3Label2.Margin = new System.Windows.Forms.Padding(5);
            this.sys3Label2.Name = "sys3Label2";
            this.sys3Label2.Size = new System.Drawing.Size(374, 36);
            this.sys3Label2.SubFont = new System.Drawing.Font("맑은 고딕", 10F, System.Drawing.FontStyle.Bold);
            this.sys3Label2.SubFontColor = System.Drawing.Color.Gray;
            this.sys3Label2.SubText = "";
            this.sys3Label2.TabIndex = 21148;
            this.sys3Label2.Text = "USE WAFER CYCLE MODE";
            this.sys3Label2.TextAlignMain = Sys3Controls.EN_TEXTALIGN.MIDDLE_RIGHT;
            this.sys3Label2.TextAlignSub = Sys3Controls.EN_TEXTALIGN.TOP_LEFT;
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
            this.sys3Label2.UseSubFont = true;
            this.sys3Label2.UseUnitFont = false;
            // 
            // sys3Label3
            // 
            this.sys3Label3.BackGroundColor = System.Drawing.Color.DarkGray;
            this.sys3Label3.BorderStroke = 2;
            this.sys3Label3.BorderStyle = System.Windows.Forms.ButtonBorderStyle.Solid;
            this.sys3Label3.Description = "";
            this.sys3Label3.DisabledColor = System.Drawing.Color.DarkGray;
            this.sys3Label3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.sys3Label3.EdgeRadius = 1;
            this.sys3Label3.ImagePosition = new System.Drawing.Point(0, 0);
            this.sys3Label3.ImageSize = new System.Drawing.Point(0, 0);
            this.sys3Label3.LoadImage = null;
            this.sys3Label3.Location = new System.Drawing.Point(5, 51);
            this.sys3Label3.MainFont = new System.Drawing.Font("맑은 고딕", 11F, System.Drawing.FontStyle.Bold);
            this.sys3Label3.MainFontColor = System.Drawing.Color.Black;
            this.sys3Label3.Margin = new System.Windows.Forms.Padding(5);
            this.sys3Label3.Name = "sys3Label3";
            this.sys3Label3.Size = new System.Drawing.Size(374, 36);
            this.sys3Label3.SubFont = new System.Drawing.Font("맑은 고딕", 10F, System.Drawing.FontStyle.Bold);
            this.sys3Label3.SubFontColor = System.Drawing.Color.Gray;
            this.sys3Label3.SubText = "";
            this.sys3Label3.TabIndex = 21152;
            this.sys3Label3.Text = "USE UPPER ARM";
            this.sys3Label3.TextAlignMain = Sys3Controls.EN_TEXTALIGN.MIDDLE_RIGHT;
            this.sys3Label3.TextAlignSub = Sys3Controls.EN_TEXTALIGN.TOP_LEFT;
            this.sys3Label3.TextAlignUnit = Sys3Controls.EN_TEXTALIGN.MIDDLE_CENTER;
            this.sys3Label3.ThemeIndex = 0;
            this.sys3Label3.UnitAreaRate = 30;
            this.sys3Label3.UnitFont = new System.Drawing.Font("맑은 고딕", 10F, System.Drawing.FontStyle.Bold);
            this.sys3Label3.UnitFontColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.sys3Label3.UnitPositionVertical = false;
            this.sys3Label3.UnitText = "";
            this.sys3Label3.UseBorder = true;
            this.sys3Label3.UseEdgeRadius = false;
            this.sys3Label3.UseImage = false;
            this.sys3Label3.UseSubFont = true;
            this.sys3Label3.UseUnitFont = false;
            // 
            // sys3Label4
            // 
            this.sys3Label4.BackGroundColor = System.Drawing.Color.DarkGray;
            this.sys3Label4.BorderStroke = 2;
            this.sys3Label4.BorderStyle = System.Windows.Forms.ButtonBorderStyle.Solid;
            this.sys3Label4.Description = "";
            this.sys3Label4.DisabledColor = System.Drawing.Color.DarkGray;
            this.sys3Label4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.sys3Label4.EdgeRadius = 1;
            this.sys3Label4.ImagePosition = new System.Drawing.Point(0, 0);
            this.sys3Label4.ImageSize = new System.Drawing.Point(0, 0);
            this.sys3Label4.LoadImage = null;
            this.sys3Label4.Location = new System.Drawing.Point(5, 97);
            this.sys3Label4.MainFont = new System.Drawing.Font("맑은 고딕", 11F, System.Drawing.FontStyle.Bold);
            this.sys3Label4.MainFontColor = System.Drawing.Color.Black;
            this.sys3Label4.Margin = new System.Windows.Forms.Padding(5);
            this.sys3Label4.Name = "sys3Label4";
            this.sys3Label4.Size = new System.Drawing.Size(374, 36);
            this.sys3Label4.SubFont = new System.Drawing.Font("맑은 고딕", 10F, System.Drawing.FontStyle.Bold);
            this.sys3Label4.SubFontColor = System.Drawing.Color.Gray;
            this.sys3Label4.SubText = "";
            this.sys3Label4.TabIndex = 21154;
            this.sys3Label4.Text = "USE LOWER ARM";
            this.sys3Label4.TextAlignMain = Sys3Controls.EN_TEXTALIGN.MIDDLE_RIGHT;
            this.sys3Label4.TextAlignSub = Sys3Controls.EN_TEXTALIGN.TOP_LEFT;
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
            this.sys3Label4.UseSubFont = true;
            this.sys3Label4.UseUnitFont = false;
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
            this.sys3Label1.Location = new System.Drawing.Point(5, 143);
            this.sys3Label1.MainFont = new System.Drawing.Font("맑은 고딕", 11F, System.Drawing.FontStyle.Bold);
            this.sys3Label1.MainFontColor = System.Drawing.Color.Black;
            this.sys3Label1.Margin = new System.Windows.Forms.Padding(5);
            this.sys3Label1.Name = "sys3Label1";
            this.sys3Label1.Size = new System.Drawing.Size(374, 36);
            this.sys3Label1.SubFont = new System.Drawing.Font("맑은 고딕", 10F, System.Drawing.FontStyle.Bold);
            this.sys3Label1.SubFontColor = System.Drawing.Color.Gray;
            this.sys3Label1.SubText = "";
            this.sys3Label1.TabIndex = 21156;
            this.sys3Label1.Text = "AMHS HANDLING EVENT INTERVAL";
            this.sys3Label1.TextAlignMain = Sys3Controls.EN_TEXTALIGN.MIDDLE_RIGHT;
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
            // toggleUseCycleMode
            // 
            this.toggleUseCycleMode.Active = false;
            this.toggleUseCycleMode.ActiveColorFirst = System.Drawing.Color.RoyalBlue;
            this.toggleUseCycleMode.ActiveColorSecond = System.Drawing.Color.DodgerBlue;
            this.toggleUseCycleMode.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.toggleUseCycleMode.AssociatedMap = null;
            this.toggleUseCycleMode.Location = new System.Drawing.Point(394, 3);
            this.toggleUseCycleMode.Margin = new System.Windows.Forms.Padding(10, 1, 0, 0);
            this.toggleUseCycleMode.Name = "toggleUseCycleMode";
            this.toggleUseCycleMode.NeedRemakeMap = false;
            this.toggleUseCycleMode.NormalColorFirst = System.Drawing.Color.DarkGray;
            this.toggleUseCycleMode.NormalColorSecond = System.Drawing.Color.Silver;
            this.toggleUseCycleMode.ParameterChangeDefaultActiveColorFirst = System.Drawing.Color.RoyalBlue;
            this.toggleUseCycleMode.ParameterChangeDefaultActiveColorSecond = System.Drawing.Color.DodgerBlue;
            this.toggleUseCycleMode.ParameterChangeDefaultNormalColorFirst = System.Drawing.Color.DarkGray;
            this.toggleUseCycleMode.ParameterChangeDefaultNormalColorSecond = System.Drawing.Color.Silver;
            this.toggleUseCycleMode.ParameterChangeWaitColorFirst = System.Drawing.Color.DarkRed;
            this.toggleUseCycleMode.ParameterChangeWaitColorSecond = System.Drawing.Color.Firebrick;
            this.toggleUseCycleMode.ParameterIndex = 0;
            this.toggleUseCycleMode.ParameterName = "UseCycleMode";
            this.toggleUseCycleMode.ParameterStorage = "False";
            this.toggleUseCycleMode.ParameterStored = false;
            this.toggleUseCycleMode.ParameterType = FrameOfSystem3.Recipe.EN_RECIPE_TYPE.COMMON;
            this.toggleUseCycleMode.Size = new System.Drawing.Size(80, 40);
            this.toggleUseCycleMode.TabIndex = 21148;
            this.toggleUseCycleMode.TaskName = "Sample";
            this.toggleUseCycleMode.UseParameterChangeConfirm = true;
            this.toggleUseCycleMode.Click += new System.EventHandler(this.ToggleClicked);
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
            this.gbTitle.Padding = new System.Windows.Forms.Padding(3, 18, 3, 3);
            this.gbTitle.Size = new System.Drawing.Size(554, 314);
            this.gbTitle.TabIndex = 21136;
            this.gbTitle.TabStop = false;
            this.gbTitle.TextAlign = Sys3Controls.EN_TEXTALIGN.MIDDLE_CENTER;
            this.gbTitle.ThemeIndex = 0;
            this.gbTitle.UseLabelBorder = true;
            this.gbTitle.UseTitle = true;
            // 
            // SetupPWA500BINOptions
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.Controls.Add(this.gbTitle);
            this.Margin = new System.Windows.Forms.Padding(0);
            this.Name = "SetupPWA500BINOptions";
            this.Size = new System.Drawing.Size(554, 314);
            this.Tag = "";
            this.tableLayoutPanel1.ResumeLayout(false);
            this.gbTitle.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private FrameOfSystem3.Component.CustomParameterLabel customParameterLabel1;
        private FrameOfSystem3.Component.CustomParameterLabel lblHandlingWaitTime;
        private FrameOfSystem3.Component.CustomParameterToggleButton ToggleUseLowerArm;
        private FrameOfSystem3.Component.CustomParameterToggleButton ToggleUseUpperArm;
        private Sys3Controls.Sys3Label sys3Label5;
        private Sys3Controls.Sys3Label sys3Label2;
        private Sys3Controls.Sys3Label sys3Label3;
        private Sys3Controls.Sys3Label sys3Label4;
        private Sys3Controls.Sys3Label sys3Label1;
        private FrameOfSystem3.Component.CustomParameterToggleButton toggleUseCycleMode;
        private Sys3Controls.Sys3GroupBoxContainer gbTitle;
    }
}
