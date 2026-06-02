namespace FrameOfSystem3.Views.Functional
{
	partial class Form_AlarmMessage
	{
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		protected override void Dispose(bool disposing)
		{
			if (disposing && (components != null))
			{
				components.Dispose();
			}
			base.Dispose(disposing);
		}

		#region Windows Form Designer generated code

		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
            this.m_listWaiting = new System.Windows.Forms.ListBox();
            this.m_groupTitle = new Sys3Controls.Sys3ColorGroupBox();
            this.m_lblAlarmCode = new Sys3Controls.Sys3Label();
            this.m_lblAlarmMessage = new Sys3Controls.Sys3Label();
            this.m_lblAlarmList = new Sys3Controls.Sys3Label();
            this.m_labelAlarmCode = new Sys3Controls.Sys3Label();
            this.m_labelAlarmMessage = new Sys3Controls.Sys3Label();
            this.m_btnPass = new Sys3Controls.Sys3button();
            this.m_btnSkip = new Sys3Controls.Sys3button();
            this.m_btnResetOrAbort = new Sys3Controls.Sys3button();
            this.m_btnBuzzerOffOrOk = new Sys3Controls.Sys3button();
            this.m_labelTime = new Sys3Controls.Sys3Label();
            this.m_lblTime = new Sys3Controls.Sys3Label();
            this.m_lblElapsed = new Sys3Controls.Sys3Label();
            this.m_labelElapsed = new Sys3Controls.Sys3Label();
            this.m_lblSequenceNum = new Sys3Controls.Sys3Label();
            this.m_lblActionName = new Sys3Controls.Sys3Label();
            this.m_labelActionName = new Sys3Controls.Sys3Label();
            this.m_labelSequenceNumber = new Sys3Controls.Sys3Label();
            this.m_lblAlarmSolution = new Sys3Controls.Sys3Label();
            this.m_labelSolution = new Sys3Controls.Sys3Label();
            this.SuspendLayout();
            // 
            // m_listWaiting
            // 
            this.m_listWaiting.BackColor = System.Drawing.Color.WhiteSmoke;
            this.m_listWaiting.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.m_listWaiting.Font = new System.Drawing.Font("맑은 고딕", 10F, System.Drawing.FontStyle.Bold);
            this.m_listWaiting.FormattingEnabled = true;
            this.m_listWaiting.ItemHeight = 17;
            this.m_listWaiting.Location = new System.Drawing.Point(186, 296);
            this.m_listWaiting.Name = "m_listWaiting";
            this.m_listWaiting.Size = new System.Drawing.Size(259, 70);
            this.m_listWaiting.TabIndex = 1296;
            // 
            // m_groupTitle
            // 
            this.m_groupTitle.BackGroundColor = System.Drawing.Color.WhiteSmoke;
            this.m_groupTitle.Dock = System.Windows.Forms.DockStyle.Fill;
            this.m_groupTitle.EdgeBorderStroke = 2;
            this.m_groupTitle.EdgeBorderStyle = System.Windows.Forms.ButtonBorderStyle.Solid;
            this.m_groupTitle.EdgeColor = System.Drawing.Color.FromArgb(((int)(((byte)(225)))), ((int)(((byte)(135)))), ((int)(((byte)(0)))));
            this.m_groupTitle.EdgeStroke = 25F;
            this.m_groupTitle.LabelBorderStroke = 2;
            this.m_groupTitle.LabelBorderStyle = System.Windows.Forms.ButtonBorderStyle.Solid;
            this.m_groupTitle.LabelFont = new System.Drawing.Font("맑은 고딕", 12F, System.Drawing.FontStyle.Bold);
            this.m_groupTitle.LabelGradientColorFirst = System.Drawing.Color.FromArgb(((int)(((byte)(103)))), ((int)(((byte)(155)))), ((int)(((byte)(217)))));
            this.m_groupTitle.LabelGradientColorSecond = System.Drawing.Color.FromArgb(((int)(((byte)(83)))), ((int)(((byte)(125)))), ((int)(((byte)(187)))));
            this.m_groupTitle.LabelHeight = 1;
            this.m_groupTitle.LabelTextColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.m_groupTitle.Location = new System.Drawing.Point(0, 0);
            this.m_groupTitle.Name = "m_groupTitle";
            this.m_groupTitle.Size = new System.Drawing.Size(758, 443);
            this.m_groupTitle.TabIndex = 1297;
            this.m_groupTitle.Text = "STATUS";
            this.m_groupTitle.TextAlign = Sys3Controls.EN_TEXTALIGN.MIDDLE_CENTER;
            this.m_groupTitle.UseEdgeBorder = true;
            this.m_groupTitle.UseLabelBorder = true;
            // 
            // m_lblAlarmCode
            // 
            this.m_lblAlarmCode.BackGroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(225)))), ((int)(((byte)(135)))), ((int)(((byte)(0)))));
            this.m_lblAlarmCode.BorderStroke = 2;
            this.m_lblAlarmCode.BorderStyle = System.Windows.Forms.ButtonBorderStyle.Solid;
            this.m_lblAlarmCode.Description = "";
            this.m_lblAlarmCode.DisabledColor = System.Drawing.Color.DarkGray;
            this.m_lblAlarmCode.EdgeRadius = 1;
            this.m_lblAlarmCode.ImagePosition = new System.Drawing.Point(0, 0);
            this.m_lblAlarmCode.ImageSize = new System.Drawing.Point(0, 0);
            this.m_lblAlarmCode.LoadImage = null;
            this.m_lblAlarmCode.Location = new System.Drawing.Point(21, 21);
            this.m_lblAlarmCode.MainFont = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Bold);
            this.m_lblAlarmCode.MainFontColor = System.Drawing.Color.WhiteSmoke;
            this.m_lblAlarmCode.Name = "m_lblAlarmCode";
            this.m_lblAlarmCode.Size = new System.Drawing.Size(161, 34);
            this.m_lblAlarmCode.SubFont = new System.Drawing.Font("맑은 고딕", 10F);
            this.m_lblAlarmCode.SubFontColor = System.Drawing.Color.Black;
            this.m_lblAlarmCode.SubText = "";
            this.m_lblAlarmCode.TabIndex = 1388;
            this.m_lblAlarmCode.Text = "ALARM CODE";
            this.m_lblAlarmCode.TextAlignMain = Sys3Controls.EN_TEXTALIGN.MIDDLE_CENTER;
            this.m_lblAlarmCode.TextAlignSub = Sys3Controls.EN_TEXTALIGN.MIDDLE_CENTER;
            this.m_lblAlarmCode.TextAlignUnit = Sys3Controls.EN_TEXTALIGN.MIDDLE_CENTER;
            this.m_lblAlarmCode.ThemeIndex = 0;
            this.m_lblAlarmCode.UnitAreaRate = 40;
            this.m_lblAlarmCode.UnitFont = new System.Drawing.Font("맑은 고딕", 10F, System.Drawing.FontStyle.Bold);
            this.m_lblAlarmCode.UnitFontColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.m_lblAlarmCode.UnitPositionVertical = false;
            this.m_lblAlarmCode.UnitText = "";
            this.m_lblAlarmCode.UseBorder = true;
            this.m_lblAlarmCode.UseEdgeRadius = false;
            this.m_lblAlarmCode.UseImage = false;
            this.m_lblAlarmCode.UseSubFont = false;
            this.m_lblAlarmCode.UseUnitFont = false;
            // 
            // m_lblAlarmMessage
            // 
            this.m_lblAlarmMessage.BackGroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(225)))), ((int)(((byte)(135)))), ((int)(((byte)(0)))));
            this.m_lblAlarmMessage.BorderStroke = 2;
            this.m_lblAlarmMessage.BorderStyle = System.Windows.Forms.ButtonBorderStyle.Solid;
            this.m_lblAlarmMessage.Description = "";
            this.m_lblAlarmMessage.DisabledColor = System.Drawing.Color.DarkGray;
            this.m_lblAlarmMessage.EdgeRadius = 1;
            this.m_lblAlarmMessage.ImagePosition = new System.Drawing.Point(0, 0);
            this.m_lblAlarmMessage.ImageSize = new System.Drawing.Point(0, 0);
            this.m_lblAlarmMessage.LoadImage = null;
            this.m_lblAlarmMessage.Location = new System.Drawing.Point(21, 94);
            this.m_lblAlarmMessage.MainFont = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Bold);
            this.m_lblAlarmMessage.MainFontColor = System.Drawing.Color.WhiteSmoke;
            this.m_lblAlarmMessage.Name = "m_lblAlarmMessage";
            this.m_lblAlarmMessage.Size = new System.Drawing.Size(161, 34);
            this.m_lblAlarmMessage.SubFont = new System.Drawing.Font("맑은 고딕", 10F);
            this.m_lblAlarmMessage.SubFontColor = System.Drawing.Color.Black;
            this.m_lblAlarmMessage.SubText = "";
            this.m_lblAlarmMessage.TabIndex = 1388;
            this.m_lblAlarmMessage.Text = "ALARM MESSAGE";
            this.m_lblAlarmMessage.TextAlignMain = Sys3Controls.EN_TEXTALIGN.MIDDLE_CENTER;
            this.m_lblAlarmMessage.TextAlignSub = Sys3Controls.EN_TEXTALIGN.MIDDLE_CENTER;
            this.m_lblAlarmMessage.TextAlignUnit = Sys3Controls.EN_TEXTALIGN.MIDDLE_CENTER;
            this.m_lblAlarmMessage.ThemeIndex = 0;
            this.m_lblAlarmMessage.UnitAreaRate = 40;
            this.m_lblAlarmMessage.UnitFont = new System.Drawing.Font("맑은 고딕", 10F, System.Drawing.FontStyle.Bold);
            this.m_lblAlarmMessage.UnitFontColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.m_lblAlarmMessage.UnitPositionVertical = false;
            this.m_lblAlarmMessage.UnitText = "";
            this.m_lblAlarmMessage.UseBorder = true;
            this.m_lblAlarmMessage.UseEdgeRadius = false;
            this.m_lblAlarmMessage.UseImage = false;
            this.m_lblAlarmMessage.UseSubFont = false;
            this.m_lblAlarmMessage.UseUnitFont = false;
            // 
            // m_lblAlarmList
            // 
            this.m_lblAlarmList.BackGroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(225)))), ((int)(((byte)(135)))), ((int)(((byte)(0)))));
            this.m_lblAlarmList.BorderStroke = 2;
            this.m_lblAlarmList.BorderStyle = System.Windows.Forms.ButtonBorderStyle.Solid;
            this.m_lblAlarmList.Description = "";
            this.m_lblAlarmList.DisabledColor = System.Drawing.Color.DarkGray;
            this.m_lblAlarmList.EdgeRadius = 1;
            this.m_lblAlarmList.ImagePosition = new System.Drawing.Point(0, 0);
            this.m_lblAlarmList.ImageSize = new System.Drawing.Point(0, 0);
            this.m_lblAlarmList.LoadImage = null;
            this.m_lblAlarmList.Location = new System.Drawing.Point(21, 296);
            this.m_lblAlarmList.MainFont = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Bold);
            this.m_lblAlarmList.MainFontColor = System.Drawing.Color.WhiteSmoke;
            this.m_lblAlarmList.Name = "m_lblAlarmList";
            this.m_lblAlarmList.Size = new System.Drawing.Size(161, 34);
            this.m_lblAlarmList.SubFont = new System.Drawing.Font("맑은 고딕", 10F);
            this.m_lblAlarmList.SubFontColor = System.Drawing.Color.Black;
            this.m_lblAlarmList.SubText = "";
            this.m_lblAlarmList.TabIndex = 1388;
            this.m_lblAlarmList.Text = "ALARM LIST";
            this.m_lblAlarmList.TextAlignMain = Sys3Controls.EN_TEXTALIGN.MIDDLE_CENTER;
            this.m_lblAlarmList.TextAlignSub = Sys3Controls.EN_TEXTALIGN.MIDDLE_CENTER;
            this.m_lblAlarmList.TextAlignUnit = Sys3Controls.EN_TEXTALIGN.MIDDLE_CENTER;
            this.m_lblAlarmList.ThemeIndex = 0;
            this.m_lblAlarmList.UnitAreaRate = 40;
            this.m_lblAlarmList.UnitFont = new System.Drawing.Font("맑은 고딕", 10F, System.Drawing.FontStyle.Bold);
            this.m_lblAlarmList.UnitFontColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.m_lblAlarmList.UnitPositionVertical = false;
            this.m_lblAlarmList.UnitText = "";
            this.m_lblAlarmList.UseBorder = true;
            this.m_lblAlarmList.UseEdgeRadius = false;
            this.m_lblAlarmList.UseImage = false;
            this.m_lblAlarmList.UseSubFont = false;
            this.m_lblAlarmList.UseUnitFont = false;
            // 
            // m_labelAlarmCode
            // 
            this.m_labelAlarmCode.BackGroundColor = System.Drawing.Color.WhiteSmoke;
            this.m_labelAlarmCode.BorderStroke = 2;
            this.m_labelAlarmCode.BorderStyle = System.Windows.Forms.ButtonBorderStyle.Solid;
            this.m_labelAlarmCode.Description = "";
            this.m_labelAlarmCode.DisabledColor = System.Drawing.Color.Silver;
            this.m_labelAlarmCode.EdgeRadius = 1;
            this.m_labelAlarmCode.ImagePosition = new System.Drawing.Point(0, 0);
            this.m_labelAlarmCode.ImageSize = new System.Drawing.Point(0, 0);
            this.m_labelAlarmCode.LoadImage = null;
            this.m_labelAlarmCode.Location = new System.Drawing.Point(185, 21);
            this.m_labelAlarmCode.MainFont = new System.Drawing.Font("맑은 고딕", 16F, System.Drawing.FontStyle.Bold);
            this.m_labelAlarmCode.MainFontColor = System.Drawing.Color.Black;
            this.m_labelAlarmCode.Name = "m_labelAlarmCode";
            this.m_labelAlarmCode.Size = new System.Drawing.Size(261, 71);
            this.m_labelAlarmCode.SubFont = new System.Drawing.Font("맑은 고딕", 10F);
            this.m_labelAlarmCode.SubFontColor = System.Drawing.Color.Black;
            this.m_labelAlarmCode.SubText = "";
            this.m_labelAlarmCode.TabIndex = 1389;
            this.m_labelAlarmCode.Text = "000000";
            this.m_labelAlarmCode.TextAlignMain = Sys3Controls.EN_TEXTALIGN.MIDDLE_CENTER;
            this.m_labelAlarmCode.TextAlignSub = Sys3Controls.EN_TEXTALIGN.MIDDLE_CENTER;
            this.m_labelAlarmCode.TextAlignUnit = Sys3Controls.EN_TEXTALIGN.MIDDLE_CENTER;
            this.m_labelAlarmCode.ThemeIndex = 0;
            this.m_labelAlarmCode.UnitAreaRate = 40;
            this.m_labelAlarmCode.UnitFont = new System.Drawing.Font("맑은 고딕", 10F, System.Drawing.FontStyle.Bold);
            this.m_labelAlarmCode.UnitFontColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.m_labelAlarmCode.UnitPositionVertical = false;
            this.m_labelAlarmCode.UnitText = "";
            this.m_labelAlarmCode.UseBorder = true;
            this.m_labelAlarmCode.UseEdgeRadius = false;
            this.m_labelAlarmCode.UseImage = false;
            this.m_labelAlarmCode.UseSubFont = false;
            this.m_labelAlarmCode.UseUnitFont = false;
            // 
            // m_labelAlarmMessage
            // 
            this.m_labelAlarmMessage.BackGroundColor = System.Drawing.Color.WhiteSmoke;
            this.m_labelAlarmMessage.BorderStroke = 2;
            this.m_labelAlarmMessage.BorderStyle = System.Windows.Forms.ButtonBorderStyle.Solid;
            this.m_labelAlarmMessage.Description = "";
            this.m_labelAlarmMessage.DisabledColor = System.Drawing.Color.Silver;
            this.m_labelAlarmMessage.EdgeRadius = 1;
            this.m_labelAlarmMessage.ImagePosition = new System.Drawing.Point(0, 0);
            this.m_labelAlarmMessage.ImageSize = new System.Drawing.Point(0, 0);
            this.m_labelAlarmMessage.LoadImage = null;
            this.m_labelAlarmMessage.Location = new System.Drawing.Point(184, 94);
            this.m_labelAlarmMessage.MainFont = new System.Drawing.Font("맑은 고딕", 16F, System.Drawing.FontStyle.Bold);
            this.m_labelAlarmMessage.MainFontColor = System.Drawing.Color.Black;
            this.m_labelAlarmMessage.Name = "m_labelAlarmMessage";
            this.m_labelAlarmMessage.Size = new System.Drawing.Size(560, 99);
            this.m_labelAlarmMessage.SubFont = new System.Drawing.Font("맑은 고딕", 10F);
            this.m_labelAlarmMessage.SubFontColor = System.Drawing.Color.Black;
            this.m_labelAlarmMessage.SubText = "";
            this.m_labelAlarmMessage.TabIndex = 1389;
            this.m_labelAlarmMessage.Text = "AN UNKNOWN ERROR OCCUR.";
            this.m_labelAlarmMessage.TextAlignMain = Sys3Controls.EN_TEXTALIGN.MIDDLE_CENTER;
            this.m_labelAlarmMessage.TextAlignSub = Sys3Controls.EN_TEXTALIGN.MIDDLE_CENTER;
            this.m_labelAlarmMessage.TextAlignUnit = Sys3Controls.EN_TEXTALIGN.MIDDLE_CENTER;
            this.m_labelAlarmMessage.ThemeIndex = 0;
            this.m_labelAlarmMessage.UnitAreaRate = 40;
            this.m_labelAlarmMessage.UnitFont = new System.Drawing.Font("맑은 고딕", 10F, System.Drawing.FontStyle.Bold);
            this.m_labelAlarmMessage.UnitFontColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.m_labelAlarmMessage.UnitPositionVertical = false;
            this.m_labelAlarmMessage.UnitText = "";
            this.m_labelAlarmMessage.UseBorder = true;
            this.m_labelAlarmMessage.UseEdgeRadius = false;
            this.m_labelAlarmMessage.UseImage = false;
            this.m_labelAlarmMessage.UseSubFont = false;
            this.m_labelAlarmMessage.UseUnitFont = false;
            // 
            // m_btnPass
            // 
            this.m_btnPass.BorderWidth = 2;
            this.m_btnPass.ButtonClicked = false;
            this.m_btnPass.ClickedEmphasizeTextColor = System.Drawing.Color.White;
            this.m_btnPass.CustomClickedGradientFirstColor = System.Drawing.Color.White;
            this.m_btnPass.CustomClickedGradientSecondColor = System.Drawing.Color.White;
            this.m_btnPass.Description = "";
            this.m_btnPass.DisabledColor = System.Drawing.Color.DarkGray;
            this.m_btnPass.EdgeRadius = 5;
            this.m_btnPass.GradientAngle = 70F;
            this.m_btnPass.GradientFirstColor = System.Drawing.Color.White;
            this.m_btnPass.GradientSecondColor = System.Drawing.Color.FromArgb(((int)(((byte)(170)))), ((int)(((byte)(176)))), ((int)(((byte)(183)))));
            this.m_btnPass.HoverEmphasizeCustomColor = System.Drawing.Color.White;
            this.m_btnPass.ImagePosition = new System.Drawing.Point(7, 7);
            this.m_btnPass.ImageSize = new System.Drawing.Point(30, 30);
            this.m_btnPass.LoadImage = global::FrameOfSystem3.Properties.Resources.CONFIG_ADD3;
            this.m_btnPass.Location = new System.Drawing.Point(21, 371);
            this.m_btnPass.MainFont = new System.Drawing.Font("맑은 고딕", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.m_btnPass.MainFontColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(36)))), ((int)(((byte)(0)))));
            this.m_btnPass.Name = "m_btnPass";
            this.m_btnPass.Size = new System.Drawing.Size(161, 58);
            this.m_btnPass.SubFont = new System.Drawing.Font("맑은 고딕", 8F, System.Drawing.FontStyle.Bold);
            this.m_btnPass.SubFontColor = System.Drawing.Color.DarkBlue;
            this.m_btnPass.SubText = "STATUS";
            this.m_btnPass.TabIndex = 0;
            this.m_btnPass.Text = "RETRY/COMPLETE";
            this.m_btnPass.TextAlignMain = Sys3Controls.EN_TEXTALIGN.MIDDLE_CENTER;
            this.m_btnPass.TextAlignSub = Sys3Controls.EN_TEXTALIGN.TOP_LEFT;
            this.m_btnPass.ThemeIndex = 0;
            this.m_btnPass.UseBorder = true;
            this.m_btnPass.UseClickedEmphasizeTextColor = false;
            this.m_btnPass.UseCustomizeClickedColor = false;
            this.m_btnPass.UseEdge = true;
            this.m_btnPass.UseHoverEmphasizeCustomColor = false;
            this.m_btnPass.UseImage = false;
            this.m_btnPass.UserHoverEmpahsize = false;
            this.m_btnPass.UseSubFont = false;
            this.m_btnPass.Click += new System.EventHandler(this.Click_Buttons);
            // 
            // m_btnSkip
            // 
            this.m_btnSkip.BorderWidth = 2;
            this.m_btnSkip.ButtonClicked = false;
            this.m_btnSkip.ClickedEmphasizeTextColor = System.Drawing.Color.White;
            this.m_btnSkip.CustomClickedGradientFirstColor = System.Drawing.Color.White;
            this.m_btnSkip.CustomClickedGradientSecondColor = System.Drawing.Color.White;
            this.m_btnSkip.Description = "";
            this.m_btnSkip.DisabledColor = System.Drawing.Color.DarkGray;
            this.m_btnSkip.EdgeRadius = 5;
            this.m_btnSkip.GradientAngle = 70F;
            this.m_btnSkip.GradientFirstColor = System.Drawing.Color.White;
            this.m_btnSkip.GradientSecondColor = System.Drawing.Color.FromArgb(((int)(((byte)(170)))), ((int)(((byte)(176)))), ((int)(((byte)(183)))));
            this.m_btnSkip.HoverEmphasizeCustomColor = System.Drawing.Color.White;
            this.m_btnSkip.ImagePosition = new System.Drawing.Point(7, 7);
            this.m_btnSkip.ImageSize = new System.Drawing.Point(30, 30);
            this.m_btnSkip.LoadImage = global::FrameOfSystem3.Properties.Resources.CONFIG_ADD3;
            this.m_btnSkip.Location = new System.Drawing.Point(208, 371);
            this.m_btnSkip.MainFont = new System.Drawing.Font("맑은 고딕", 13F, System.Drawing.FontStyle.Bold);
            this.m_btnSkip.MainFontColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(36)))), ((int)(((byte)(0)))));
            this.m_btnSkip.Name = "m_btnSkip";
            this.m_btnSkip.Size = new System.Drawing.Size(161, 58);
            this.m_btnSkip.SubFont = new System.Drawing.Font("맑은 고딕", 8F, System.Drawing.FontStyle.Bold);
            this.m_btnSkip.SubFontColor = System.Drawing.Color.DarkBlue;
            this.m_btnSkip.SubText = "STATUS";
            this.m_btnSkip.TabIndex = 1;
            this.m_btnSkip.Text = "SKIP";
            this.m_btnSkip.TextAlignMain = Sys3Controls.EN_TEXTALIGN.MIDDLE_CENTER;
            this.m_btnSkip.TextAlignSub = Sys3Controls.EN_TEXTALIGN.TOP_LEFT;
            this.m_btnSkip.ThemeIndex = 0;
            this.m_btnSkip.UseBorder = true;
            this.m_btnSkip.UseClickedEmphasizeTextColor = false;
            this.m_btnSkip.UseCustomizeClickedColor = false;
            this.m_btnSkip.UseEdge = true;
            this.m_btnSkip.UseHoverEmphasizeCustomColor = false;
            this.m_btnSkip.UseImage = false;
            this.m_btnSkip.UserHoverEmpahsize = false;
            this.m_btnSkip.UseSubFont = false;
            this.m_btnSkip.Click += new System.EventHandler(this.Click_Buttons);
            // 
            // m_btnResetOrAbort
            // 
            this.m_btnResetOrAbort.BorderWidth = 2;
            this.m_btnResetOrAbort.ButtonClicked = false;
            this.m_btnResetOrAbort.ClickedEmphasizeTextColor = System.Drawing.Color.White;
            this.m_btnResetOrAbort.CustomClickedGradientFirstColor = System.Drawing.Color.White;
            this.m_btnResetOrAbort.CustomClickedGradientSecondColor = System.Drawing.Color.White;
            this.m_btnResetOrAbort.Description = "";
            this.m_btnResetOrAbort.DisabledColor = System.Drawing.Color.DarkGray;
            this.m_btnResetOrAbort.EdgeRadius = 5;
            this.m_btnResetOrAbort.GradientAngle = 70F;
            this.m_btnResetOrAbort.GradientFirstColor = System.Drawing.Color.White;
            this.m_btnResetOrAbort.GradientSecondColor = System.Drawing.Color.FromArgb(((int)(((byte)(170)))), ((int)(((byte)(176)))), ((int)(((byte)(183)))));
            this.m_btnResetOrAbort.HoverEmphasizeCustomColor = System.Drawing.Color.White;
            this.m_btnResetOrAbort.ImagePosition = new System.Drawing.Point(7, 7);
            this.m_btnResetOrAbort.ImageSize = new System.Drawing.Point(30, 30);
            this.m_btnResetOrAbort.LoadImage = global::FrameOfSystem3.Properties.Resources.CONFIG_ADD3;
            this.m_btnResetOrAbort.Location = new System.Drawing.Point(395, 371);
            this.m_btnResetOrAbort.MainFont = new System.Drawing.Font("맑은 고딕", 13F, System.Drawing.FontStyle.Bold);
            this.m_btnResetOrAbort.MainFontColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(36)))), ((int)(((byte)(0)))));
            this.m_btnResetOrAbort.Name = "m_btnResetOrAbort";
            this.m_btnResetOrAbort.Size = new System.Drawing.Size(161, 58);
            this.m_btnResetOrAbort.SubFont = new System.Drawing.Font("맑은 고딕", 8F, System.Drawing.FontStyle.Bold);
            this.m_btnResetOrAbort.SubFontColor = System.Drawing.Color.DarkBlue;
            this.m_btnResetOrAbort.SubText = "STATUS";
            this.m_btnResetOrAbort.TabIndex = 2;
            this.m_btnResetOrAbort.Text = "RESET";
            this.m_btnResetOrAbort.TextAlignMain = Sys3Controls.EN_TEXTALIGN.MIDDLE_CENTER;
            this.m_btnResetOrAbort.TextAlignSub = Sys3Controls.EN_TEXTALIGN.TOP_LEFT;
            this.m_btnResetOrAbort.ThemeIndex = 0;
            this.m_btnResetOrAbort.UseBorder = true;
            this.m_btnResetOrAbort.UseClickedEmphasizeTextColor = false;
            this.m_btnResetOrAbort.UseCustomizeClickedColor = false;
            this.m_btnResetOrAbort.UseEdge = true;
            this.m_btnResetOrAbort.UseHoverEmphasizeCustomColor = false;
            this.m_btnResetOrAbort.UseImage = false;
            this.m_btnResetOrAbort.UserHoverEmpahsize = false;
            this.m_btnResetOrAbort.UseSubFont = false;
            this.m_btnResetOrAbort.Click += new System.EventHandler(this.Click_Buttons);
            // 
            // m_btnBuzzerOffOrOk
            // 
            this.m_btnBuzzerOffOrOk.BorderWidth = 2;
            this.m_btnBuzzerOffOrOk.ButtonClicked = false;
            this.m_btnBuzzerOffOrOk.ClickedEmphasizeTextColor = System.Drawing.Color.White;
            this.m_btnBuzzerOffOrOk.CustomClickedGradientFirstColor = System.Drawing.Color.White;
            this.m_btnBuzzerOffOrOk.CustomClickedGradientSecondColor = System.Drawing.Color.White;
            this.m_btnBuzzerOffOrOk.Description = "";
            this.m_btnBuzzerOffOrOk.DisabledColor = System.Drawing.Color.DarkGray;
            this.m_btnBuzzerOffOrOk.EdgeRadius = 5;
            this.m_btnBuzzerOffOrOk.GradientAngle = 70F;
            this.m_btnBuzzerOffOrOk.GradientFirstColor = System.Drawing.Color.White;
            this.m_btnBuzzerOffOrOk.GradientSecondColor = System.Drawing.Color.FromArgb(((int)(((byte)(170)))), ((int)(((byte)(176)))), ((int)(((byte)(183)))));
            this.m_btnBuzzerOffOrOk.HoverEmphasizeCustomColor = System.Drawing.Color.White;
            this.m_btnBuzzerOffOrOk.ImagePosition = new System.Drawing.Point(7, 7);
            this.m_btnBuzzerOffOrOk.ImageSize = new System.Drawing.Point(30, 30);
            this.m_btnBuzzerOffOrOk.LoadImage = global::FrameOfSystem3.Properties.Resources.CONFIG_ADD3;
            this.m_btnBuzzerOffOrOk.Location = new System.Drawing.Point(582, 371);
            this.m_btnBuzzerOffOrOk.MainFont = new System.Drawing.Font("맑은 고딕", 13F, System.Drawing.FontStyle.Bold);
            this.m_btnBuzzerOffOrOk.MainFontColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(36)))), ((int)(((byte)(0)))));
            this.m_btnBuzzerOffOrOk.Name = "m_btnBuzzerOffOrOk";
            this.m_btnBuzzerOffOrOk.Size = new System.Drawing.Size(161, 58);
            this.m_btnBuzzerOffOrOk.SubFont = new System.Drawing.Font("맑은 고딕", 8F, System.Drawing.FontStyle.Bold);
            this.m_btnBuzzerOffOrOk.SubFontColor = System.Drawing.Color.DarkBlue;
            this.m_btnBuzzerOffOrOk.SubText = "STATUS";
            this.m_btnBuzzerOffOrOk.TabIndex = 3;
            this.m_btnBuzzerOffOrOk.Text = "BUZZER OFF";
            this.m_btnBuzzerOffOrOk.TextAlignMain = Sys3Controls.EN_TEXTALIGN.MIDDLE_CENTER;
            this.m_btnBuzzerOffOrOk.TextAlignSub = Sys3Controls.EN_TEXTALIGN.TOP_LEFT;
            this.m_btnBuzzerOffOrOk.ThemeIndex = 0;
            this.m_btnBuzzerOffOrOk.UseBorder = true;
            this.m_btnBuzzerOffOrOk.UseClickedEmphasizeTextColor = false;
            this.m_btnBuzzerOffOrOk.UseCustomizeClickedColor = false;
            this.m_btnBuzzerOffOrOk.UseEdge = true;
            this.m_btnBuzzerOffOrOk.UseHoverEmphasizeCustomColor = false;
            this.m_btnBuzzerOffOrOk.UseImage = false;
            this.m_btnBuzzerOffOrOk.UserHoverEmpahsize = false;
            this.m_btnBuzzerOffOrOk.UseSubFont = false;
            this.m_btnBuzzerOffOrOk.Click += new System.EventHandler(this.Click_Buttons);
            // 
            // m_labelTime
            // 
            this.m_labelTime.BackGroundColor = System.Drawing.Color.WhiteSmoke;
            this.m_labelTime.BorderStroke = 2;
            this.m_labelTime.BorderStyle = System.Windows.Forms.ButtonBorderStyle.Solid;
            this.m_labelTime.Description = "";
            this.m_labelTime.DisabledColor = System.Drawing.Color.Silver;
            this.m_labelTime.EdgeRadius = 1;
            this.m_labelTime.ImagePosition = new System.Drawing.Point(0, 0);
            this.m_labelTime.ImageSize = new System.Drawing.Point(0, 0);
            this.m_labelTime.LoadImage = null;
            this.m_labelTime.Location = new System.Drawing.Point(562, 21);
            this.m_labelTime.MainFont = new System.Drawing.Font("맑은 고딕", 12F, System.Drawing.FontStyle.Bold);
            this.m_labelTime.MainFontColor = System.Drawing.Color.Black;
            this.m_labelTime.Name = "m_labelTime";
            this.m_labelTime.Size = new System.Drawing.Size(182, 34);
            this.m_labelTime.SubFont = new System.Drawing.Font("맑은 고딕", 10F);
            this.m_labelTime.SubFontColor = System.Drawing.Color.Black;
            this.m_labelTime.SubText = "";
            this.m_labelTime.TabIndex = 1390;
            this.m_labelTime.Text = "0000-00-00 00:00:00";
            this.m_labelTime.TextAlignMain = Sys3Controls.EN_TEXTALIGN.MIDDLE_CENTER;
            this.m_labelTime.TextAlignSub = Sys3Controls.EN_TEXTALIGN.MIDDLE_CENTER;
            this.m_labelTime.TextAlignUnit = Sys3Controls.EN_TEXTALIGN.MIDDLE_CENTER;
            this.m_labelTime.ThemeIndex = 0;
            this.m_labelTime.UnitAreaRate = 40;
            this.m_labelTime.UnitFont = new System.Drawing.Font("맑은 고딕", 10F, System.Drawing.FontStyle.Bold);
            this.m_labelTime.UnitFontColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.m_labelTime.UnitPositionVertical = false;
            this.m_labelTime.UnitText = "";
            this.m_labelTime.UseBorder = true;
            this.m_labelTime.UseEdgeRadius = false;
            this.m_labelTime.UseImage = false;
            this.m_labelTime.UseSubFont = false;
            this.m_labelTime.UseUnitFont = false;
            // 
            // m_lblTime
            // 
            this.m_lblTime.BackGroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(225)))), ((int)(((byte)(135)))), ((int)(((byte)(0)))));
            this.m_lblTime.BorderStroke = 2;
            this.m_lblTime.BorderStyle = System.Windows.Forms.ButtonBorderStyle.Solid;
            this.m_lblTime.Description = "";
            this.m_lblTime.DisabledColor = System.Drawing.Color.DarkGray;
            this.m_lblTime.EdgeRadius = 1;
            this.m_lblTime.ImagePosition = new System.Drawing.Point(0, 0);
            this.m_lblTime.ImageSize = new System.Drawing.Point(0, 0);
            this.m_lblTime.LoadImage = null;
            this.m_lblTime.Location = new System.Drawing.Point(449, 21);
            this.m_lblTime.MainFont = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Bold);
            this.m_lblTime.MainFontColor = System.Drawing.Color.WhiteSmoke;
            this.m_lblTime.Name = "m_lblTime";
            this.m_lblTime.Size = new System.Drawing.Size(111, 34);
            this.m_lblTime.SubFont = new System.Drawing.Font("맑은 고딕", 10F);
            this.m_lblTime.SubFontColor = System.Drawing.Color.Black;
            this.m_lblTime.SubText = "";
            this.m_lblTime.TabIndex = 1388;
            this.m_lblTime.Text = "OCCUR TIME";
            this.m_lblTime.TextAlignMain = Sys3Controls.EN_TEXTALIGN.MIDDLE_CENTER;
            this.m_lblTime.TextAlignSub = Sys3Controls.EN_TEXTALIGN.MIDDLE_CENTER;
            this.m_lblTime.TextAlignUnit = Sys3Controls.EN_TEXTALIGN.MIDDLE_CENTER;
            this.m_lblTime.ThemeIndex = 0;
            this.m_lblTime.UnitAreaRate = 40;
            this.m_lblTime.UnitFont = new System.Drawing.Font("맑은 고딕", 10F, System.Drawing.FontStyle.Bold);
            this.m_lblTime.UnitFontColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.m_lblTime.UnitPositionVertical = false;
            this.m_lblTime.UnitText = "";
            this.m_lblTime.UseBorder = true;
            this.m_lblTime.UseEdgeRadius = false;
            this.m_lblTime.UseImage = false;
            this.m_lblTime.UseSubFont = false;
            this.m_lblTime.UseUnitFont = false;
            // 
            // m_lblElapsed
            // 
            this.m_lblElapsed.BackGroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(225)))), ((int)(((byte)(135)))), ((int)(((byte)(0)))));
            this.m_lblElapsed.BorderStroke = 2;
            this.m_lblElapsed.BorderStyle = System.Windows.Forms.ButtonBorderStyle.Solid;
            this.m_lblElapsed.Description = "";
            this.m_lblElapsed.DisabledColor = System.Drawing.Color.DarkGray;
            this.m_lblElapsed.EdgeRadius = 1;
            this.m_lblElapsed.ImagePosition = new System.Drawing.Point(0, 0);
            this.m_lblElapsed.ImageSize = new System.Drawing.Point(0, 0);
            this.m_lblElapsed.LoadImage = null;
            this.m_lblElapsed.Location = new System.Drawing.Point(449, 58);
            this.m_lblElapsed.MainFont = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Bold);
            this.m_lblElapsed.MainFontColor = System.Drawing.Color.WhiteSmoke;
            this.m_lblElapsed.Name = "m_lblElapsed";
            this.m_lblElapsed.Size = new System.Drawing.Size(111, 34);
            this.m_lblElapsed.SubFont = new System.Drawing.Font("맑은 고딕", 10F);
            this.m_lblElapsed.SubFontColor = System.Drawing.Color.Black;
            this.m_lblElapsed.SubText = "";
            this.m_lblElapsed.TabIndex = 1388;
            this.m_lblElapsed.Text = "ELAPSED TIME";
            this.m_lblElapsed.TextAlignMain = Sys3Controls.EN_TEXTALIGN.MIDDLE_CENTER;
            this.m_lblElapsed.TextAlignSub = Sys3Controls.EN_TEXTALIGN.MIDDLE_CENTER;
            this.m_lblElapsed.TextAlignUnit = Sys3Controls.EN_TEXTALIGN.MIDDLE_CENTER;
            this.m_lblElapsed.ThemeIndex = 0;
            this.m_lblElapsed.UnitAreaRate = 40;
            this.m_lblElapsed.UnitFont = new System.Drawing.Font("맑은 고딕", 10F, System.Drawing.FontStyle.Bold);
            this.m_lblElapsed.UnitFontColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.m_lblElapsed.UnitPositionVertical = false;
            this.m_lblElapsed.UnitText = "";
            this.m_lblElapsed.UseBorder = true;
            this.m_lblElapsed.UseEdgeRadius = false;
            this.m_lblElapsed.UseImage = false;
            this.m_lblElapsed.UseSubFont = false;
            this.m_lblElapsed.UseUnitFont = false;
            // 
            // m_labelElapsed
            // 
            this.m_labelElapsed.BackGroundColor = System.Drawing.Color.WhiteSmoke;
            this.m_labelElapsed.BorderStroke = 2;
            this.m_labelElapsed.BorderStyle = System.Windows.Forms.ButtonBorderStyle.Solid;
            this.m_labelElapsed.Description = "";
            this.m_labelElapsed.DisabledColor = System.Drawing.Color.Silver;
            this.m_labelElapsed.EdgeRadius = 1;
            this.m_labelElapsed.ImagePosition = new System.Drawing.Point(0, 0);
            this.m_labelElapsed.ImageSize = new System.Drawing.Point(0, 0);
            this.m_labelElapsed.LoadImage = null;
            this.m_labelElapsed.Location = new System.Drawing.Point(562, 58);
            this.m_labelElapsed.MainFont = new System.Drawing.Font("맑은 고딕", 12F, System.Drawing.FontStyle.Bold);
            this.m_labelElapsed.MainFontColor = System.Drawing.Color.Black;
            this.m_labelElapsed.Name = "m_labelElapsed";
            this.m_labelElapsed.Size = new System.Drawing.Size(182, 34);
            this.m_labelElapsed.SubFont = new System.Drawing.Font("맑은 고딕", 10F);
            this.m_labelElapsed.SubFontColor = System.Drawing.Color.Black;
            this.m_labelElapsed.SubText = "";
            this.m_labelElapsed.TabIndex = 1390;
            this.m_labelElapsed.Text = "00:00:00";
            this.m_labelElapsed.TextAlignMain = Sys3Controls.EN_TEXTALIGN.MIDDLE_CENTER;
            this.m_labelElapsed.TextAlignSub = Sys3Controls.EN_TEXTALIGN.MIDDLE_CENTER;
            this.m_labelElapsed.TextAlignUnit = Sys3Controls.EN_TEXTALIGN.MIDDLE_CENTER;
            this.m_labelElapsed.ThemeIndex = 0;
            this.m_labelElapsed.UnitAreaRate = 40;
            this.m_labelElapsed.UnitFont = new System.Drawing.Font("맑은 고딕", 10F, System.Drawing.FontStyle.Bold);
            this.m_labelElapsed.UnitFontColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.m_labelElapsed.UnitPositionVertical = false;
            this.m_labelElapsed.UnitText = "";
            this.m_labelElapsed.UseBorder = true;
            this.m_labelElapsed.UseEdgeRadius = false;
            this.m_labelElapsed.UseImage = false;
            this.m_labelElapsed.UseSubFont = false;
            this.m_labelElapsed.UseUnitFont = false;
            // 
            // m_lblSequenceNum
            // 
            this.m_lblSequenceNum.BackGroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(225)))), ((int)(((byte)(135)))), ((int)(((byte)(0)))));
            this.m_lblSequenceNum.BorderStroke = 2;
            this.m_lblSequenceNum.BorderStyle = System.Windows.Forms.ButtonBorderStyle.Solid;
            this.m_lblSequenceNum.Description = "";
            this.m_lblSequenceNum.DisabledColor = System.Drawing.Color.DarkGray;
            this.m_lblSequenceNum.EdgeRadius = 1;
            this.m_lblSequenceNum.ImagePosition = new System.Drawing.Point(0, 0);
            this.m_lblSequenceNum.ImageSize = new System.Drawing.Point(0, 0);
            this.m_lblSequenceNum.LoadImage = null;
            this.m_lblSequenceNum.Location = new System.Drawing.Point(449, 295);
            this.m_lblSequenceNum.MainFont = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Bold);
            this.m_lblSequenceNum.MainFontColor = System.Drawing.Color.WhiteSmoke;
            this.m_lblSequenceNum.Name = "m_lblSequenceNum";
            this.m_lblSequenceNum.Size = new System.Drawing.Size(111, 34);
            this.m_lblSequenceNum.SubFont = new System.Drawing.Font("맑은 고딕", 10F);
            this.m_lblSequenceNum.SubFontColor = System.Drawing.Color.Black;
            this.m_lblSequenceNum.SubText = "";
            this.m_lblSequenceNum.TabIndex = 1391;
            this.m_lblSequenceNum.Text = "SEQUENCE NUM";
            this.m_lblSequenceNum.TextAlignMain = Sys3Controls.EN_TEXTALIGN.MIDDLE_CENTER;
            this.m_lblSequenceNum.TextAlignSub = Sys3Controls.EN_TEXTALIGN.MIDDLE_CENTER;
            this.m_lblSequenceNum.TextAlignUnit = Sys3Controls.EN_TEXTALIGN.MIDDLE_CENTER;
            this.m_lblSequenceNum.ThemeIndex = 0;
            this.m_lblSequenceNum.UnitAreaRate = 40;
            this.m_lblSequenceNum.UnitFont = new System.Drawing.Font("맑은 고딕", 10F, System.Drawing.FontStyle.Bold);
            this.m_lblSequenceNum.UnitFontColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.m_lblSequenceNum.UnitPositionVertical = false;
            this.m_lblSequenceNum.UnitText = "";
            this.m_lblSequenceNum.UseBorder = true;
            this.m_lblSequenceNum.UseEdgeRadius = false;
            this.m_lblSequenceNum.UseImage = false;
            this.m_lblSequenceNum.UseSubFont = false;
            this.m_lblSequenceNum.UseUnitFont = false;
            // 
            // m_lblActionName
            // 
            this.m_lblActionName.BackGroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(225)))), ((int)(((byte)(135)))), ((int)(((byte)(0)))));
            this.m_lblActionName.BorderStroke = 2;
            this.m_lblActionName.BorderStyle = System.Windows.Forms.ButtonBorderStyle.Solid;
            this.m_lblActionName.Description = "";
            this.m_lblActionName.DisabledColor = System.Drawing.Color.DarkGray;
            this.m_lblActionName.EdgeRadius = 1;
            this.m_lblActionName.ImagePosition = new System.Drawing.Point(0, 0);
            this.m_lblActionName.ImageSize = new System.Drawing.Point(0, 0);
            this.m_lblActionName.LoadImage = null;
            this.m_lblActionName.Location = new System.Drawing.Point(449, 331);
            this.m_lblActionName.MainFont = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Bold);
            this.m_lblActionName.MainFontColor = System.Drawing.Color.WhiteSmoke;
            this.m_lblActionName.Name = "m_lblActionName";
            this.m_lblActionName.Size = new System.Drawing.Size(111, 34);
            this.m_lblActionName.SubFont = new System.Drawing.Font("맑은 고딕", 10F);
            this.m_lblActionName.SubFontColor = System.Drawing.Color.Black;
            this.m_lblActionName.SubText = "";
            this.m_lblActionName.TabIndex = 1392;
            this.m_lblActionName.Text = "ACTION NAME";
            this.m_lblActionName.TextAlignMain = Sys3Controls.EN_TEXTALIGN.MIDDLE_CENTER;
            this.m_lblActionName.TextAlignSub = Sys3Controls.EN_TEXTALIGN.MIDDLE_CENTER;
            this.m_lblActionName.TextAlignUnit = Sys3Controls.EN_TEXTALIGN.MIDDLE_CENTER;
            this.m_lblActionName.ThemeIndex = 0;
            this.m_lblActionName.UnitAreaRate = 40;
            this.m_lblActionName.UnitFont = new System.Drawing.Font("맑은 고딕", 10F, System.Drawing.FontStyle.Bold);
            this.m_lblActionName.UnitFontColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.m_lblActionName.UnitPositionVertical = false;
            this.m_lblActionName.UnitText = "";
            this.m_lblActionName.UseBorder = true;
            this.m_lblActionName.UseEdgeRadius = false;
            this.m_lblActionName.UseImage = false;
            this.m_lblActionName.UseSubFont = false;
            this.m_lblActionName.UseUnitFont = false;
            // 
            // m_labelActionName
            // 
            this.m_labelActionName.BackGroundColor = System.Drawing.Color.WhiteSmoke;
            this.m_labelActionName.BorderStroke = 2;
            this.m_labelActionName.BorderStyle = System.Windows.Forms.ButtonBorderStyle.Solid;
            this.m_labelActionName.Description = "";
            this.m_labelActionName.DisabledColor = System.Drawing.Color.Silver;
            this.m_labelActionName.EdgeRadius = 1;
            this.m_labelActionName.ImagePosition = new System.Drawing.Point(0, 0);
            this.m_labelActionName.ImageSize = new System.Drawing.Point(0, 0);
            this.m_labelActionName.LoadImage = null;
            this.m_labelActionName.Location = new System.Drawing.Point(562, 331);
            this.m_labelActionName.MainFont = new System.Drawing.Font("맑은 고딕", 10F, System.Drawing.FontStyle.Bold);
            this.m_labelActionName.MainFontColor = System.Drawing.Color.Black;
            this.m_labelActionName.Name = "m_labelActionName";
            this.m_labelActionName.Size = new System.Drawing.Size(182, 34);
            this.m_labelActionName.SubFont = new System.Drawing.Font("맑은 고딕", 10F);
            this.m_labelActionName.SubFontColor = System.Drawing.Color.Black;
            this.m_labelActionName.SubText = "";
            this.m_labelActionName.TabIndex = 1393;
            this.m_labelActionName.Text = "--";
            this.m_labelActionName.TextAlignMain = Sys3Controls.EN_TEXTALIGN.MIDDLE_CENTER;
            this.m_labelActionName.TextAlignSub = Sys3Controls.EN_TEXTALIGN.MIDDLE_CENTER;
            this.m_labelActionName.TextAlignUnit = Sys3Controls.EN_TEXTALIGN.MIDDLE_CENTER;
            this.m_labelActionName.ThemeIndex = 0;
            this.m_labelActionName.UnitAreaRate = 40;
            this.m_labelActionName.UnitFont = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Bold);
            this.m_labelActionName.UnitFontColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.m_labelActionName.UnitPositionVertical = false;
            this.m_labelActionName.UnitText = "";
            this.m_labelActionName.UseBorder = true;
            this.m_labelActionName.UseEdgeRadius = false;
            this.m_labelActionName.UseImage = false;
            this.m_labelActionName.UseSubFont = false;
            this.m_labelActionName.UseUnitFont = false;
            // 
            // m_labelSequenceNumber
            // 
            this.m_labelSequenceNumber.BackGroundColor = System.Drawing.Color.WhiteSmoke;
            this.m_labelSequenceNumber.BorderStroke = 2;
            this.m_labelSequenceNumber.BorderStyle = System.Windows.Forms.ButtonBorderStyle.Solid;
            this.m_labelSequenceNumber.Description = "";
            this.m_labelSequenceNumber.DisabledColor = System.Drawing.Color.Silver;
            this.m_labelSequenceNumber.EdgeRadius = 1;
            this.m_labelSequenceNumber.ImagePosition = new System.Drawing.Point(0, 0);
            this.m_labelSequenceNumber.ImageSize = new System.Drawing.Point(0, 0);
            this.m_labelSequenceNumber.LoadImage = null;
            this.m_labelSequenceNumber.Location = new System.Drawing.Point(562, 295);
            this.m_labelSequenceNumber.MainFont = new System.Drawing.Font("맑은 고딕", 12F, System.Drawing.FontStyle.Bold);
            this.m_labelSequenceNumber.MainFontColor = System.Drawing.Color.Black;
            this.m_labelSequenceNumber.Name = "m_labelSequenceNumber";
            this.m_labelSequenceNumber.Size = new System.Drawing.Size(182, 34);
            this.m_labelSequenceNumber.SubFont = new System.Drawing.Font("맑은 고딕", 10F);
            this.m_labelSequenceNumber.SubFontColor = System.Drawing.Color.Black;
            this.m_labelSequenceNumber.SubText = "";
            this.m_labelSequenceNumber.TabIndex = 1394;
            this.m_labelSequenceNumber.Text = "00";
            this.m_labelSequenceNumber.TextAlignMain = Sys3Controls.EN_TEXTALIGN.MIDDLE_CENTER;
            this.m_labelSequenceNumber.TextAlignSub = Sys3Controls.EN_TEXTALIGN.MIDDLE_CENTER;
            this.m_labelSequenceNumber.TextAlignUnit = Sys3Controls.EN_TEXTALIGN.MIDDLE_CENTER;
            this.m_labelSequenceNumber.ThemeIndex = 0;
            this.m_labelSequenceNumber.UnitAreaRate = 40;
            this.m_labelSequenceNumber.UnitFont = new System.Drawing.Font("맑은 고딕", 10F, System.Drawing.FontStyle.Bold);
            this.m_labelSequenceNumber.UnitFontColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.m_labelSequenceNumber.UnitPositionVertical = false;
            this.m_labelSequenceNumber.UnitText = "";
            this.m_labelSequenceNumber.UseBorder = true;
            this.m_labelSequenceNumber.UseEdgeRadius = false;
            this.m_labelSequenceNumber.UseImage = false;
            this.m_labelSequenceNumber.UseSubFont = false;
            this.m_labelSequenceNumber.UseUnitFont = false;
            // 
            // m_lblAlarmSolution
            // 
            this.m_lblAlarmSolution.BackGroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(225)))), ((int)(((byte)(135)))), ((int)(((byte)(0)))));
            this.m_lblAlarmSolution.BorderStroke = 2;
            this.m_lblAlarmSolution.BorderStyle = System.Windows.Forms.ButtonBorderStyle.Solid;
            this.m_lblAlarmSolution.Description = "";
            this.m_lblAlarmSolution.DisabledColor = System.Drawing.Color.DarkGray;
            this.m_lblAlarmSolution.EdgeRadius = 1;
            this.m_lblAlarmSolution.ImagePosition = new System.Drawing.Point(0, 0);
            this.m_lblAlarmSolution.ImageSize = new System.Drawing.Point(0, 0);
            this.m_lblAlarmSolution.LoadImage = null;
            this.m_lblAlarmSolution.Location = new System.Drawing.Point(21, 195);
            this.m_lblAlarmSolution.MainFont = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Bold);
            this.m_lblAlarmSolution.MainFontColor = System.Drawing.Color.WhiteSmoke;
            this.m_lblAlarmSolution.Name = "m_lblAlarmSolution";
            this.m_lblAlarmSolution.Size = new System.Drawing.Size(161, 34);
            this.m_lblAlarmSolution.SubFont = new System.Drawing.Font("맑은 고딕", 10F);
            this.m_lblAlarmSolution.SubFontColor = System.Drawing.Color.Black;
            this.m_lblAlarmSolution.SubText = "";
            this.m_lblAlarmSolution.TabIndex = 1388;
            this.m_lblAlarmSolution.Text = "SOLUTION";
            this.m_lblAlarmSolution.TextAlignMain = Sys3Controls.EN_TEXTALIGN.MIDDLE_CENTER;
            this.m_lblAlarmSolution.TextAlignSub = Sys3Controls.EN_TEXTALIGN.MIDDLE_CENTER;
            this.m_lblAlarmSolution.TextAlignUnit = Sys3Controls.EN_TEXTALIGN.MIDDLE_CENTER;
            this.m_lblAlarmSolution.ThemeIndex = 0;
            this.m_lblAlarmSolution.UnitAreaRate = 40;
            this.m_lblAlarmSolution.UnitFont = new System.Drawing.Font("맑은 고딕", 10F, System.Drawing.FontStyle.Bold);
            this.m_lblAlarmSolution.UnitFontColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.m_lblAlarmSolution.UnitPositionVertical = false;
            this.m_lblAlarmSolution.UnitText = "";
            this.m_lblAlarmSolution.UseBorder = true;
            this.m_lblAlarmSolution.UseEdgeRadius = false;
            this.m_lblAlarmSolution.UseImage = false;
            this.m_lblAlarmSolution.UseSubFont = false;
            this.m_lblAlarmSolution.UseUnitFont = false;
            // 
            // m_labelSolution
            // 
            this.m_labelSolution.BackGroundColor = System.Drawing.Color.WhiteSmoke;
            this.m_labelSolution.BorderStroke = 2;
            this.m_labelSolution.BorderStyle = System.Windows.Forms.ButtonBorderStyle.Solid;
            this.m_labelSolution.Description = "";
            this.m_labelSolution.DisabledColor = System.Drawing.Color.Silver;
            this.m_labelSolution.EdgeRadius = 1;
            this.m_labelSolution.ImagePosition = new System.Drawing.Point(0, 0);
            this.m_labelSolution.ImageSize = new System.Drawing.Point(0, 0);
            this.m_labelSolution.LoadImage = null;
            this.m_labelSolution.Location = new System.Drawing.Point(184, 195);
            this.m_labelSolution.MainFont = new System.Drawing.Font("맑은 고딕", 16F, System.Drawing.FontStyle.Bold);
            this.m_labelSolution.MainFontColor = System.Drawing.Color.Black;
            this.m_labelSolution.Name = "m_labelSolution";
            this.m_labelSolution.Size = new System.Drawing.Size(560, 99);
            this.m_labelSolution.SubFont = new System.Drawing.Font("맑은 고딕", 10F);
            this.m_labelSolution.SubFontColor = System.Drawing.Color.Black;
            this.m_labelSolution.SubText = "";
            this.m_labelSolution.TabIndex = 1389;
            this.m_labelSolution.Text = "--";
            this.m_labelSolution.TextAlignMain = Sys3Controls.EN_TEXTALIGN.MIDDLE_CENTER;
            this.m_labelSolution.TextAlignSub = Sys3Controls.EN_TEXTALIGN.MIDDLE_CENTER;
            this.m_labelSolution.TextAlignUnit = Sys3Controls.EN_TEXTALIGN.MIDDLE_CENTER;
            this.m_labelSolution.ThemeIndex = 0;
            this.m_labelSolution.UnitAreaRate = 40;
            this.m_labelSolution.UnitFont = new System.Drawing.Font("맑은 고딕", 10F, System.Drawing.FontStyle.Bold);
            this.m_labelSolution.UnitFontColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.m_labelSolution.UnitPositionVertical = false;
            this.m_labelSolution.UnitText = "";
            this.m_labelSolution.UseBorder = true;
            this.m_labelSolution.UseEdgeRadius = false;
            this.m_labelSolution.UseImage = false;
            this.m_labelSolution.UseSubFont = false;
            this.m_labelSolution.UseUnitFont = false;
            // 
            // Form_AlarmMessage
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.BackColor = System.Drawing.Color.WhiteSmoke;
            this.CausesValidation = false;
            this.ClientSize = new System.Drawing.Size(758, 443);
            this.ControlBox = false;
            this.Controls.Add(this.m_labelActionName);
            this.Controls.Add(this.m_labelSequenceNumber);
            this.Controls.Add(this.m_lblActionName);
            this.Controls.Add(this.m_lblSequenceNum);
            this.Controls.Add(this.m_labelElapsed);
            this.Controls.Add(this.m_labelTime);
            this.Controls.Add(this.m_btnBuzzerOffOrOk);
            this.Controls.Add(this.m_btnResetOrAbort);
            this.Controls.Add(this.m_btnSkip);
            this.Controls.Add(this.m_btnPass);
            this.Controls.Add(this.m_labelSolution);
            this.Controls.Add(this.m_labelAlarmMessage);
            this.Controls.Add(this.m_labelAlarmCode);
            this.Controls.Add(this.m_lblAlarmList);
            this.Controls.Add(this.m_lblAlarmSolution);
            this.Controls.Add(this.m_lblAlarmMessage);
            this.Controls.Add(this.m_lblElapsed);
            this.Controls.Add(this.m_lblTime);
            this.Controls.Add(this.m_lblAlarmCode);
            this.Controls.Add(this.m_listWaiting);
            this.Controls.Add(this.m_groupTitle);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.KeyPreview = true;
            this.Location = new System.Drawing.Point(230, 400);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "Form_AlarmMessage";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            this.Text = " ";
            this.TopMost = true;
            this.TransparencyKey = System.Drawing.Color.White;
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.Form_AlarmMessage_KeyDown);
            this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.ListBox m_listWaiting;
		private Sys3Controls.Sys3ColorGroupBox m_groupTitle;
		private Sys3Controls.Sys3Label m_lblAlarmCode;
		private Sys3Controls.Sys3Label m_lblAlarmMessage;
		private Sys3Controls.Sys3Label m_lblAlarmList;
		private Sys3Controls.Sys3Label m_labelAlarmCode;
		private Sys3Controls.Sys3Label m_labelAlarmMessage;
		private Sys3Controls.Sys3button m_btnPass;
		private Sys3Controls.Sys3button m_btnSkip;
		private Sys3Controls.Sys3button m_btnResetOrAbort;
		private Sys3Controls.Sys3button m_btnBuzzerOffOrOk;
		private Sys3Controls.Sys3Label m_labelTime;
		private Sys3Controls.Sys3Label m_lblTime;
		private Sys3Controls.Sys3Label m_lblElapsed;
		private Sys3Controls.Sys3Label m_labelElapsed;
		private Sys3Controls.Sys3Label m_lblSequenceNum;
		private Sys3Controls.Sys3Label m_lblActionName;
		private Sys3Controls.Sys3Label m_labelActionName;
		private Sys3Controls.Sys3Label m_labelSequenceNumber;
		private Sys3Controls.Sys3Label m_labelSolution;
		private Sys3Controls.Sys3Label m_lblAlarmSolution;


	}
}
