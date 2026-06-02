namespace FrameOfSystem3.Views.Setup.Options
{
	partial class SetupOptionEquipment
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
            this.tgl_Sample = new FrameOfSystem3.Component.CustomParameterToggleButton();
            this.sys3Label1 = new Sys3Controls.Sys3Label();
            this.lblMachineLanguage = new Sys3Controls.Sys3Label();
            this.sys3Label6 = new Sys3Controls.Sys3Label();
            this.sys3Label2 = new Sys3Controls.Sys3Label();
            this.customParameterLabel2 = new FrameOfSystem3.Component.CustomParameterLabel();
            this.sys3Label3 = new Sys3Controls.Sys3Label();
            this.customParameterLabel3 = new FrameOfSystem3.Component.CustomParameterLabel();
            this.customParameterToggleButton1 = new FrameOfSystem3.Component.CustomParameterToggleButton();
            this.sys3Label4 = new Sys3Controls.Sys3Label();
            this.SuspendLayout();
            // 
            // tgl_Sample
            // 
            this.tgl_Sample.Active = false;
            this.tgl_Sample.ActiveColorFirst = System.Drawing.Color.RoyalBlue;
            this.tgl_Sample.ActiveColorSecond = System.Drawing.Color.DodgerBlue;
            this.tgl_Sample.AssociatedMap = null;
            this.tgl_Sample.Location = new System.Drawing.Point(341, 6);
            this.tgl_Sample.Margin = new System.Windows.Forms.Padding(1, 1, 0, 0);
            this.tgl_Sample.Name = "tgl_Sample";
            this.tgl_Sample.NeedRemakeMap = false;
            this.tgl_Sample.NormalColorFirst = System.Drawing.Color.DarkGray;
            this.tgl_Sample.NormalColorSecond = System.Drawing.Color.Silver;
            this.tgl_Sample.ParameterChangeDefaultActiveColorFirst = System.Drawing.Color.RoyalBlue;
            this.tgl_Sample.ParameterChangeDefaultActiveColorSecond = System.Drawing.Color.DodgerBlue;
            this.tgl_Sample.ParameterChangeDefaultNormalColorFirst = System.Drawing.Color.DarkGray;
            this.tgl_Sample.ParameterChangeDefaultNormalColorSecond = System.Drawing.Color.Silver;
            this.tgl_Sample.ParameterChangeWaitColorFirst = System.Drawing.Color.DarkRed;
            this.tgl_Sample.ParameterChangeWaitColorSecond = System.Drawing.Color.Firebrick;
            this.tgl_Sample.ParameterIndex = 0;
            this.tgl_Sample.ParameterName = "Use_Vision";
            this.tgl_Sample.ParameterStorage = "False";
            this.tgl_Sample.ParameterStored = false;
            this.tgl_Sample.ParameterType = FrameOfSystem3.Recipe.EN_RECIPE_TYPE.EQUIPMENT;
            this.tgl_Sample.Size = new System.Drawing.Size(60, 30);
            this.tgl_Sample.TabIndex = 20665;
            this.tgl_Sample.TaskName = "Device";
            this.tgl_Sample.UseParameterChangeConfirm = true;
            // 
            // sys3Label1
            // 
            this.sys3Label1.BackGroundColor = System.Drawing.Color.DarkGray;
            this.sys3Label1.BorderStroke = 2;
            this.sys3Label1.BorderStyle = System.Windows.Forms.ButtonBorderStyle.Solid;
            this.sys3Label1.Description = "";
            this.sys3Label1.DisabledColor = System.Drawing.Color.DarkGray;
            this.sys3Label1.EdgeRadius = 1;
            this.sys3Label1.ImagePosition = new System.Drawing.Point(0, 0);
            this.sys3Label1.ImageSize = new System.Drawing.Point(0, 0);
            this.sys3Label1.LoadImage = null;
            this.sys3Label1.Location = new System.Drawing.Point(6, 6);
            this.sys3Label1.MainFont = new System.Drawing.Font("맑은 고딕", 11F, System.Drawing.FontStyle.Bold);
            this.sys3Label1.MainFontColor = System.Drawing.Color.Black;
            this.sys3Label1.Margin = new System.Windows.Forms.Padding(1, 1, 0, 0);
            this.sys3Label1.Name = "sys3Label1";
            this.sys3Label1.Size = new System.Drawing.Size(334, 30);
            this.sys3Label1.SubFont = new System.Drawing.Font("맑은 고딕", 10F, System.Drawing.FontStyle.Bold);
            this.sys3Label1.SubFontColor = System.Drawing.Color.DarkRed;
            this.sys3Label1.SubText = "";
            this.sys3Label1.TabIndex = 20666;
            this.sys3Label1.Tag = "";
            this.sys3Label1.Text = "USE VISION";
            this.sys3Label1.TextAlignMain = Sys3Controls.EN_TEXTALIGN.MIDDLE_RIGHT;
            this.sys3Label1.TextAlignSub = Sys3Controls.EN_TEXTALIGN.TOP_LEFT;
            this.sys3Label1.TextAlignUnit = Sys3Controls.EN_TEXTALIGN.MIDDLE_CENTER;
            this.sys3Label1.ThemeIndex = 0;
            this.sys3Label1.UnitAreaRate = 40;
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
            // lblMachineLanguage
            // 
            this.lblMachineLanguage.BackGroundColor = System.Drawing.Color.White;
            this.lblMachineLanguage.BorderStroke = 2;
            this.lblMachineLanguage.BorderStyle = System.Windows.Forms.ButtonBorderStyle.Solid;
            this.lblMachineLanguage.Description = "";
            this.lblMachineLanguage.DisabledColor = System.Drawing.Color.DarkGray;
            this.lblMachineLanguage.EdgeRadius = 1;
            this.lblMachineLanguage.ImagePosition = new System.Drawing.Point(0, 0);
            this.lblMachineLanguage.ImageSize = new System.Drawing.Point(0, 0);
            this.lblMachineLanguage.LoadImage = null;
            this.lblMachineLanguage.Location = new System.Drawing.Point(341, 37);
            this.lblMachineLanguage.MainFont = new System.Drawing.Font("맑은 고딕", 11F, System.Drawing.FontStyle.Bold);
            this.lblMachineLanguage.MainFontColor = System.Drawing.Color.Black;
            this.lblMachineLanguage.Margin = new System.Windows.Forms.Padding(1, 1, 0, 0);
            this.lblMachineLanguage.Name = "lblMachineLanguage";
            this.lblMachineLanguage.Size = new System.Drawing.Size(154, 30);
            this.lblMachineLanguage.SubFont = new System.Drawing.Font("맑은 고딕", 8F, System.Drawing.FontStyle.Bold);
            this.lblMachineLanguage.SubFontColor = System.Drawing.Color.Gray;
            this.lblMachineLanguage.SubText = "";
            this.lblMachineLanguage.TabIndex = 20671;
            this.lblMachineLanguage.Tag = "";
            this.lblMachineLanguage.Text = "--";
            this.lblMachineLanguage.TextAlignMain = Sys3Controls.EN_TEXTALIGN.MIDDLE_RIGHT;
            this.lblMachineLanguage.TextAlignSub = Sys3Controls.EN_TEXTALIGN.TOP_LEFT;
            this.lblMachineLanguage.TextAlignUnit = Sys3Controls.EN_TEXTALIGN.MIDDLE_RIGHT;
            this.lblMachineLanguage.ThemeIndex = 0;
            this.lblMachineLanguage.UnitAreaRate = 40;
            this.lblMachineLanguage.UnitFont = new System.Drawing.Font("맑은 고딕", 10F, System.Drawing.FontStyle.Bold);
            this.lblMachineLanguage.UnitFontColor = System.Drawing.Color.Red;
            this.lblMachineLanguage.UnitPositionVertical = false;
            this.lblMachineLanguage.UnitText = "--";
            this.lblMachineLanguage.UseBorder = true;
            this.lblMachineLanguage.UseEdgeRadius = false;
            this.lblMachineLanguage.UseImage = false;            
            this.lblMachineLanguage.UseSubFont = true;
            this.lblMachineLanguage.UseUnitFont = false;
            this.lblMachineLanguage.Click += new System.EventHandler(this.BtnButtonClicked);
            // 
            // sys3Label6
            // 
            this.sys3Label6.BackGroundColor = System.Drawing.Color.DarkGray;
            this.sys3Label6.BorderStroke = 2;
            this.sys3Label6.BorderStyle = System.Windows.Forms.ButtonBorderStyle.Solid;
            this.sys3Label6.Description = "";
            this.sys3Label6.DisabledColor = System.Drawing.Color.DarkGray;
            this.sys3Label6.EdgeRadius = 1;
            this.sys3Label6.ImagePosition = new System.Drawing.Point(0, 0);
            this.sys3Label6.ImageSize = new System.Drawing.Point(0, 0);
            this.sys3Label6.LoadImage = null;
            this.sys3Label6.Location = new System.Drawing.Point(6, 37);
            this.sys3Label6.MainFont = new System.Drawing.Font("맑은 고딕", 11F, System.Drawing.FontStyle.Bold);
            this.sys3Label6.MainFontColor = System.Drawing.Color.Black;
            this.sys3Label6.Margin = new System.Windows.Forms.Padding(1, 1, 0, 0);
            this.sys3Label6.Name = "sys3Label6";
            this.sys3Label6.Size = new System.Drawing.Size(334, 30);
            this.sys3Label6.SubFont = new System.Drawing.Font("맑은 고딕", 10F, System.Drawing.FontStyle.Bold);
            this.sys3Label6.SubFontColor = System.Drawing.Color.DarkRed;
            this.sys3Label6.SubText = "";
            this.sys3Label6.TabIndex = 20670;
            this.sys3Label6.Tag = "";
            this.sys3Label6.Text = "MACHINE LANGUAGE";
            this.sys3Label6.TextAlignMain = Sys3Controls.EN_TEXTALIGN.MIDDLE_RIGHT;
            this.sys3Label6.TextAlignSub = Sys3Controls.EN_TEXTALIGN.TOP_LEFT;
            this.sys3Label6.TextAlignUnit = Sys3Controls.EN_TEXTALIGN.MIDDLE_CENTER;
            this.sys3Label6.ThemeIndex = 0;
            this.sys3Label6.UnitAreaRate = 40;
            this.sys3Label6.UnitFont = new System.Drawing.Font("맑은 고딕", 10F, System.Drawing.FontStyle.Bold);
            this.sys3Label6.UnitFontColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.sys3Label6.UnitPositionVertical = false;
            this.sys3Label6.UnitText = "";
            this.sys3Label6.UseBorder = true;
            this.sys3Label6.UseEdgeRadius = false;
            this.sys3Label6.UseImage = false;
            this.sys3Label6.UseSubFont = true;
            this.sys3Label6.UseUnitFont = false;
            // 
            // sys3Label2
            // 
            this.sys3Label2.BackGroundColor = System.Drawing.Color.DarkGray;
            this.sys3Label2.BorderStroke = 2;
            this.sys3Label2.BorderStyle = System.Windows.Forms.ButtonBorderStyle.Solid;
            this.sys3Label2.Description = "";
            this.sys3Label2.DisabledColor = System.Drawing.Color.DarkGray;
            this.sys3Label2.EdgeRadius = 1;
            this.sys3Label2.ImagePosition = new System.Drawing.Point(0, 0);
            this.sys3Label2.ImageSize = new System.Drawing.Point(0, 0);
            this.sys3Label2.LoadImage = null;
            this.sys3Label2.Location = new System.Drawing.Point(6, 68);
            this.sys3Label2.MainFont = new System.Drawing.Font("맑은 고딕", 11F, System.Drawing.FontStyle.Bold);
            this.sys3Label2.MainFontColor = System.Drawing.Color.Black;
            this.sys3Label2.Margin = new System.Windows.Forms.Padding(1, 1, 0, 0);
            this.sys3Label2.Name = "sys3Label2";
            this.sys3Label2.Size = new System.Drawing.Size(334, 30);
            this.sys3Label2.SubFont = new System.Drawing.Font("맑은 고딕", 10F, System.Drawing.FontStyle.Bold);
            this.sys3Label2.SubFontColor = System.Drawing.Color.DarkRed;
            this.sys3Label2.SubText = "";
            this.sys3Label2.TabIndex = 20670;
            this.sys3Label2.Tag = "";
            this.sys3Label2.Text = "MACHINE NAME";
            this.sys3Label2.TextAlignMain = Sys3Controls.EN_TEXTALIGN.MIDDLE_RIGHT;
            this.sys3Label2.TextAlignSub = Sys3Controls.EN_TEXTALIGN.TOP_LEFT;
            this.sys3Label2.TextAlignUnit = Sys3Controls.EN_TEXTALIGN.MIDDLE_CENTER;
            this.sys3Label2.ThemeIndex = 0;
            this.sys3Label2.UnitAreaRate = 40;
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
            // customParameterLabel2
            // 
            this.customParameterLabel2.AssociatedMap = null;
            this.customParameterLabel2.BackGroundColor = System.Drawing.Color.White;
            this.customParameterLabel2.BorderStroke = 2;
            this.customParameterLabel2.BorderStyle = System.Windows.Forms.ButtonBorderStyle.Solid;
            this.customParameterLabel2.Description = "";
            this.customParameterLabel2.DisabledColor = System.Drawing.Color.DarkGray;
            this.customParameterLabel2.EdgeRadius = 1;
            this.customParameterLabel2.ImagePosition = new System.Drawing.Point(0, 0);
            this.customParameterLabel2.ImageSize = new System.Drawing.Point(0, 0);
            this.customParameterLabel2.LoadImage = null;
            this.customParameterLabel2.Location = new System.Drawing.Point(341, 68);
            this.customParameterLabel2.MainFont = new System.Drawing.Font("맑은 고딕", 11F, System.Drawing.FontStyle.Bold);
            this.customParameterLabel2.MainFontColor = System.Drawing.Color.Black;
            this.customParameterLabel2.Margin = new System.Windows.Forms.Padding(1, 1, 0, 0);
            this.customParameterLabel2.Modifiable = true;
            this.customParameterLabel2.Name = "customParameterLabel2";
            this.customParameterLabel2.NeedRemakeMap = false;
            this.customParameterLabel2.ParameterChangeDefaultColor = System.Drawing.Color.Black;
            this.customParameterLabel2.ParameterChangeWaitColor = System.Drawing.Color.Red;
            this.customParameterLabel2.ParameterDefaultValue = "";
            this.customParameterLabel2.ParameterIndex = 0;
            this.customParameterLabel2.ParameterMAX = "1";
            this.customParameterLabel2.ParameterMIN = "0";
            this.customParameterLabel2.ParameterName = "MachineName";
            this.customParameterLabel2.ParameterSettingType = FrameOfSystem3.Component.EN_LABEL_PARAMETER_TYPE.KEYBOARD;
            this.customParameterLabel2.ParameterStorage = "";
            this.customParameterLabel2.ParameterStored = false;
            this.customParameterLabel2.ParameterType = FrameOfSystem3.Recipe.EN_RECIPE_TYPE.EQUIPMENT;
            this.customParameterLabel2.ParameterUNIT = "";
            this.customParameterLabel2.SelectionList = Define.DefineEnumProject.SelectionList.EN_SELECTIONLIST.LANGUAGE;
            this.customParameterLabel2.Size = new System.Drawing.Size(154, 30);
            this.customParameterLabel2.SubFont = new System.Drawing.Font("맑은 고딕", 8F, System.Drawing.FontStyle.Bold);
            this.customParameterLabel2.SubFontColor = System.Drawing.Color.Gray;
            this.customParameterLabel2.SubText = "";
            this.customParameterLabel2.TabIndex = 20671;
            this.customParameterLabel2.Tag = "";
            this.customParameterLabel2.TaskName = "Sample";
            this.customParameterLabel2.Text = "--";
            this.customParameterLabel2.TextAlignMain = Sys3Controls.EN_TEXTALIGN.MIDDLE_RIGHT;
            this.customParameterLabel2.TextAlignSub = Sys3Controls.EN_TEXTALIGN.TOP_LEFT;
            this.customParameterLabel2.TextAlignUnit = Sys3Controls.EN_TEXTALIGN.MIDDLE_RIGHT;
            this.customParameterLabel2.ThemeIndex = 0;
            this.customParameterLabel2.UnitAreaRate = 40;
            this.customParameterLabel2.UnitFont = new System.Drawing.Font("맑은 고딕", 10F, System.Drawing.FontStyle.Bold);
            this.customParameterLabel2.UnitFontColor = System.Drawing.Color.Red;
            this.customParameterLabel2.UnitPositionVertical = false;
            this.customParameterLabel2.UnitText = "--";
            this.customParameterLabel2.UseBorder = true;
            this.customParameterLabel2.UseEdgeRadius = false;
            this.customParameterLabel2.UseImage = false;
            this.customParameterLabel2.UseParameterChangeConfirm = true;
            this.customParameterLabel2.UseSubFont = true;
            this.customParameterLabel2.UseUnitFont = false;
            // 
            // sys3Label3
            // 
            this.sys3Label3.BackGroundColor = System.Drawing.Color.DarkGray;
            this.sys3Label3.BorderStroke = 2;
            this.sys3Label3.BorderStyle = System.Windows.Forms.ButtonBorderStyle.Solid;
            this.sys3Label3.Description = "";
            this.sys3Label3.DisabledColor = System.Drawing.Color.DarkGray;
            this.sys3Label3.EdgeRadius = 1;
            this.sys3Label3.ImagePosition = new System.Drawing.Point(0, 0);
            this.sys3Label3.ImageSize = new System.Drawing.Point(0, 0);
            this.sys3Label3.LoadImage = null;
            this.sys3Label3.Location = new System.Drawing.Point(6, 99);
            this.sys3Label3.MainFont = new System.Drawing.Font("맑은 고딕", 11F, System.Drawing.FontStyle.Bold);
            this.sys3Label3.MainFontColor = System.Drawing.Color.Black;
            this.sys3Label3.Margin = new System.Windows.Forms.Padding(1, 1, 0, 0);
            this.sys3Label3.Name = "sys3Label3";
            this.sys3Label3.Size = new System.Drawing.Size(334, 30);
            this.sys3Label3.SubFont = new System.Drawing.Font("맑은 고딕", 10F, System.Drawing.FontStyle.Bold);
            this.sys3Label3.SubFontColor = System.Drawing.Color.DarkRed;
            this.sys3Label3.SubText = "";
            this.sys3Label3.TabIndex = 20670;
            this.sys3Label3.Tag = "";
            this.sys3Label3.Text = "CAMERA COUNT";
            this.sys3Label3.TextAlignMain = Sys3Controls.EN_TEXTALIGN.MIDDLE_RIGHT;
            this.sys3Label3.TextAlignSub = Sys3Controls.EN_TEXTALIGN.TOP_LEFT;
            this.sys3Label3.TextAlignUnit = Sys3Controls.EN_TEXTALIGN.MIDDLE_CENTER;
            this.sys3Label3.ThemeIndex = 0;
            this.sys3Label3.UnitAreaRate = 40;
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
            // customParameterLabel3
            // 
            this.customParameterLabel3.AssociatedMap = null;
            this.customParameterLabel3.BackGroundColor = System.Drawing.Color.White;
            this.customParameterLabel3.BorderStroke = 2;
            this.customParameterLabel3.BorderStyle = System.Windows.Forms.ButtonBorderStyle.Solid;
            this.customParameterLabel3.Description = "";
            this.customParameterLabel3.DisabledColor = System.Drawing.Color.DarkGray;
            this.customParameterLabel3.EdgeRadius = 1;
            this.customParameterLabel3.ImagePosition = new System.Drawing.Point(0, 0);
            this.customParameterLabel3.ImageSize = new System.Drawing.Point(0, 0);
            this.customParameterLabel3.LoadImage = null;
            this.customParameterLabel3.Location = new System.Drawing.Point(341, 99);
            this.customParameterLabel3.MainFont = new System.Drawing.Font("맑은 고딕", 11F, System.Drawing.FontStyle.Bold);
            this.customParameterLabel3.MainFontColor = System.Drawing.Color.Black;
            this.customParameterLabel3.Margin = new System.Windows.Forms.Padding(1, 1, 0, 0);
            this.customParameterLabel3.Modifiable = true;
            this.customParameterLabel3.Name = "customParameterLabel3";
            this.customParameterLabel3.NeedRemakeMap = false;
            this.customParameterLabel3.ParameterChangeDefaultColor = System.Drawing.Color.Black;
            this.customParameterLabel3.ParameterChangeWaitColor = System.Drawing.Color.Red;
            this.customParameterLabel3.ParameterDefaultValue = "";
            this.customParameterLabel3.ParameterIndex = 0;
            this.customParameterLabel3.ParameterMAX = "100";
            this.customParameterLabel3.ParameterMIN = "0";
            this.customParameterLabel3.ParameterName = "Vision_CameraCount";
            this.customParameterLabel3.ParameterSettingType = FrameOfSystem3.Component.EN_LABEL_PARAMETER_TYPE.CALCULATE_DOUBLE;
            this.customParameterLabel3.ParameterStorage = "";
            this.customParameterLabel3.ParameterStored = false;
            this.customParameterLabel3.ParameterType = FrameOfSystem3.Recipe.EN_RECIPE_TYPE.EQUIPMENT;
            this.customParameterLabel3.ParameterUNIT = "count";
            this.customParameterLabel3.SelectionList = Define.DefineEnumProject.SelectionList.EN_SELECTIONLIST.LANGUAGE;
            this.customParameterLabel3.Size = new System.Drawing.Size(154, 30);
            this.customParameterLabel3.SubFont = new System.Drawing.Font("맑은 고딕", 8F, System.Drawing.FontStyle.Bold);
            this.customParameterLabel3.SubFontColor = System.Drawing.Color.Gray;
            this.customParameterLabel3.SubText = "";
            this.customParameterLabel3.TabIndex = 20671;
            this.customParameterLabel3.Tag = "";
            this.customParameterLabel3.TaskName = "Sample";
            this.customParameterLabel3.Text = "--";
            this.customParameterLabel3.TextAlignMain = Sys3Controls.EN_TEXTALIGN.MIDDLE_RIGHT;
            this.customParameterLabel3.TextAlignSub = Sys3Controls.EN_TEXTALIGN.TOP_LEFT;
            this.customParameterLabel3.TextAlignUnit = Sys3Controls.EN_TEXTALIGN.MIDDLE_RIGHT;
            this.customParameterLabel3.ThemeIndex = 0;
            this.customParameterLabel3.UnitAreaRate = 40;
            this.customParameterLabel3.UnitFont = new System.Drawing.Font("맑은 고딕", 10F, System.Drawing.FontStyle.Bold);
            this.customParameterLabel3.UnitFontColor = System.Drawing.Color.Red;
            this.customParameterLabel3.UnitPositionVertical = false;
            this.customParameterLabel3.UnitText = "--";
            this.customParameterLabel3.UseBorder = true;
            this.customParameterLabel3.UseEdgeRadius = false;
            this.customParameterLabel3.UseImage = false;
            this.customParameterLabel3.UseParameterChangeConfirm = true;
            this.customParameterLabel3.UseSubFont = true;
            this.customParameterLabel3.UseUnitFont = false;
            // 
            // customParameterToggleButton1
            // 
            this.customParameterToggleButton1.Active = false;
            this.customParameterToggleButton1.ActiveColorFirst = System.Drawing.Color.RoyalBlue;
            this.customParameterToggleButton1.ActiveColorSecond = System.Drawing.Color.DodgerBlue;
            this.customParameterToggleButton1.AssociatedMap = null;
            this.customParameterToggleButton1.Location = new System.Drawing.Point(341, 130);
            this.customParameterToggleButton1.Margin = new System.Windows.Forms.Padding(1, 1, 0, 0);
            this.customParameterToggleButton1.Name = "customParameterToggleButton1";
            this.customParameterToggleButton1.NeedRemakeMap = false;
            this.customParameterToggleButton1.NormalColorFirst = System.Drawing.Color.DarkGray;
            this.customParameterToggleButton1.NormalColorSecond = System.Drawing.Color.Silver;
            this.customParameterToggleButton1.ParameterChangeDefaultActiveColorFirst = System.Drawing.Color.RoyalBlue;
            this.customParameterToggleButton1.ParameterChangeDefaultActiveColorSecond = System.Drawing.Color.DodgerBlue;
            this.customParameterToggleButton1.ParameterChangeDefaultNormalColorFirst = System.Drawing.Color.DarkGray;
            this.customParameterToggleButton1.ParameterChangeDefaultNormalColorSecond = System.Drawing.Color.Silver;
            this.customParameterToggleButton1.ParameterChangeWaitColorFirst = System.Drawing.Color.DarkRed;
            this.customParameterToggleButton1.ParameterChangeWaitColorSecond = System.Drawing.Color.Firebrick;
            this.customParameterToggleButton1.ParameterIndex = 0;
            this.customParameterToggleButton1.ParameterName = "UnlockParameterChange";
            this.customParameterToggleButton1.ParameterStorage = "False";
            this.customParameterToggleButton1.ParameterStored = false;
            this.customParameterToggleButton1.ParameterType = FrameOfSystem3.Recipe.EN_RECIPE_TYPE.EQUIPMENT;
            this.customParameterToggleButton1.Size = new System.Drawing.Size(60, 30);
            this.customParameterToggleButton1.TabIndex = 20665;
            this.customParameterToggleButton1.TaskName = "Device";
            this.customParameterToggleButton1.UseParameterChangeConfirm = true;
            // 
            // sys3Label4
            // 
            this.sys3Label4.BackGroundColor = System.Drawing.Color.DarkGray;
            this.sys3Label4.BorderStroke = 2;
            this.sys3Label4.BorderStyle = System.Windows.Forms.ButtonBorderStyle.Solid;
            this.sys3Label4.Description = "";
            this.sys3Label4.DisabledColor = System.Drawing.Color.DarkGray;
            this.sys3Label4.EdgeRadius = 1;
            this.sys3Label4.ImagePosition = new System.Drawing.Point(0, 0);
            this.sys3Label4.ImageSize = new System.Drawing.Point(0, 0);
            this.sys3Label4.LoadImage = null;
            this.sys3Label4.Location = new System.Drawing.Point(6, 130);
            this.sys3Label4.MainFont = new System.Drawing.Font("맑은 고딕", 11F, System.Drawing.FontStyle.Bold);
            this.sys3Label4.MainFontColor = System.Drawing.Color.Black;
            this.sys3Label4.Margin = new System.Windows.Forms.Padding(1, 1, 0, 0);
            this.sys3Label4.Name = "sys3Label4";
            this.sys3Label4.Size = new System.Drawing.Size(334, 30);
            this.sys3Label4.SubFont = new System.Drawing.Font("맑은 고딕", 10F, System.Drawing.FontStyle.Bold);
            this.sys3Label4.SubFontColor = System.Drawing.Color.DarkRed;
            this.sys3Label4.SubText = "";
            this.sys3Label4.TabIndex = 20666;
            this.sys3Label4.Tag = "";
            this.sys3Label4.Text = "UNLOCK PARAMETER CHANGE";
            this.sys3Label4.TextAlignMain = Sys3Controls.EN_TEXTALIGN.MIDDLE_RIGHT;
            this.sys3Label4.TextAlignSub = Sys3Controls.EN_TEXTALIGN.TOP_LEFT;
            this.sys3Label4.TextAlignUnit = Sys3Controls.EN_TEXTALIGN.MIDDLE_CENTER;
            this.sys3Label4.ThemeIndex = 0;
            this.sys3Label4.UnitAreaRate = 40;
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
            // SetupOptionEquipment
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.Controls.Add(this.customParameterLabel3);
            this.Controls.Add(this.customParameterLabel2);
            this.Controls.Add(this.lblMachineLanguage);
            this.Controls.Add(this.sys3Label3);
            this.Controls.Add(this.sys3Label2);
            this.Controls.Add(this.sys3Label6);
            this.Controls.Add(this.sys3Label4);
            this.Controls.Add(this.customParameterToggleButton1);
            this.Controls.Add(this.sys3Label1);
            this.Controls.Add(this.tgl_Sample);
            this.Name = "SetupOptionEquipment";
            this.Padding = new System.Windows.Forms.Padding(5);
            this.Size = new System.Drawing.Size(1140, 165);
            this.Tag = "EQUIPMENT";
            this.ResumeLayout(false);

		}

		#endregion

		private Component.CustomParameterToggleButton tgl_Sample;
		private Sys3Controls.Sys3Label sys3Label1;
		private Sys3Controls.Sys3Label lblMachineLanguage;
		private Sys3Controls.Sys3Label sys3Label6;
		private Sys3Controls.Sys3Label sys3Label2;
		private Component.CustomParameterLabel customParameterLabel2;
		private Sys3Controls.Sys3Label sys3Label3;
		private Component.CustomParameterLabel customParameterLabel3;
		private Component.CustomParameterToggleButton customParameterToggleButton1;
		private Sys3Controls.Sys3Label sys3Label4;


	}
}
