namespace FrameOfSystem3.Views.Setup
{
	partial class Setup_LoadPort
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
            this.panel1 = new System.Windows.Forms.Panel();
            this.btn_ParameterUndo = new Sys3Controls.Sys3button();
            this.btn_DecideParameterAll = new Sys3Controls.Sys3button();
            this.m_tabLP6 = new Sys3Controls.Sys3button();
            this.m_tabLP5 = new Sys3Controls.Sys3button();
            this.m_tabLP4 = new Sys3Controls.Sys3button();
            this.m_tabLP3 = new Sys3Controls.Sys3button();
            this.m_tabLP2 = new Sys3Controls.Sys3button();
            this.m_tabLP1 = new Sys3Controls.Sys3button();
            this.panelSubView = new System.Windows.Forms.Panel();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.btn_ParameterUndo);
            this.panel1.Controls.Add(this.btn_DecideParameterAll);
            this.panel1.Controls.Add(this.m_tabLP6);
            this.panel1.Controls.Add(this.m_tabLP5);
            this.panel1.Controls.Add(this.m_tabLP4);
            this.panel1.Controls.Add(this.m_tabLP3);
            this.panel1.Controls.Add(this.m_tabLP2);
            this.panel1.Controls.Add(this.m_tabLP1);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1140, 49);
            this.panel1.TabIndex = 20537;
            // 
            // btn_ParameterUndo
            // 
            this.btn_ParameterUndo.BorderWidth = 2;
            this.btn_ParameterUndo.ButtonClicked = false;
            this.btn_ParameterUndo.ClickedEmphasizeTextColor = System.Drawing.Color.White;
            this.btn_ParameterUndo.CustomClickedGradientFirstColor = System.Drawing.Color.BlanchedAlmond;
            this.btn_ParameterUndo.CustomClickedGradientSecondColor = System.Drawing.Color.Gold;
            this.btn_ParameterUndo.Description = "";
            this.btn_ParameterUndo.DisabledColor = System.Drawing.Color.DarkGray;
            this.btn_ParameterUndo.Dock = System.Windows.Forms.DockStyle.Right;
            this.btn_ParameterUndo.EdgeRadius = 5;
            this.btn_ParameterUndo.GradientAngle = 60F;
            this.btn_ParameterUndo.GradientFirstColor = System.Drawing.Color.DeepSkyBlue;
            this.btn_ParameterUndo.GradientSecondColor = System.Drawing.Color.SteelBlue;
            this.btn_ParameterUndo.HoverEmphasizeCustomColor = System.Drawing.Color.Firebrick;
            this.btn_ParameterUndo.ImagePosition = new System.Drawing.Point(8, 7);
            this.btn_ParameterUndo.ImageSize = new System.Drawing.Point(35, 35);
            this.btn_ParameterUndo.LoadImage = global::FrameOfSystem3.Properties.Resources.undo_52px;
            this.btn_ParameterUndo.Location = new System.Drawing.Point(1042, 0);
            this.btn_ParameterUndo.MainFont = new System.Drawing.Font("맑은 고딕", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btn_ParameterUndo.MainFontColor = System.Drawing.Color.White;
            this.btn_ParameterUndo.Margin = new System.Windows.Forms.Padding(1, 1, 0, 0);
            this.btn_ParameterUndo.Name = "btn_ParameterUndo";
            this.btn_ParameterUndo.Size = new System.Drawing.Size(49, 49);
            this.btn_ParameterUndo.SubFont = new System.Drawing.Font("맑은 고딕", 10F);
            this.btn_ParameterUndo.SubFontColor = System.Drawing.Color.Black;
            this.btn_ParameterUndo.SubText = "";
            this.btn_ParameterUndo.TabIndex = 20836;
            this.btn_ParameterUndo.Tag = "";
            this.btn_ParameterUndo.TextAlignMain = Sys3Controls.EN_TEXTALIGN.MIDDLE_CENTER;
            this.btn_ParameterUndo.TextAlignSub = Sys3Controls.EN_TEXTALIGN.MIDDLE_CENTER;
            this.btn_ParameterUndo.ThemeIndex = 0;
            this.btn_ParameterUndo.UseBorder = true;
            this.btn_ParameterUndo.UseClickedEmphasizeTextColor = false;
            this.btn_ParameterUndo.UseCustomizeClickedColor = true;
            this.btn_ParameterUndo.UseEdge = true;
            this.btn_ParameterUndo.UseHoverEmphasizeCustomColor = true;
            this.btn_ParameterUndo.UseImage = true;
            this.btn_ParameterUndo.UserHoverEmpahsize = true;
            this.btn_ParameterUndo.UseSubFont = false;
            this.btn_ParameterUndo.Click += new System.EventHandler(this.ClickParameterUndo);
            // 
            // btn_DecideParameterAll
            // 
            this.btn_DecideParameterAll.BorderWidth = 2;
            this.btn_DecideParameterAll.ButtonClicked = false;
            this.btn_DecideParameterAll.ClickedEmphasizeTextColor = System.Drawing.Color.White;
            this.btn_DecideParameterAll.CustomClickedGradientFirstColor = System.Drawing.Color.BlanchedAlmond;
            this.btn_DecideParameterAll.CustomClickedGradientSecondColor = System.Drawing.Color.Gold;
            this.btn_DecideParameterAll.Description = "";
            this.btn_DecideParameterAll.DisabledColor = System.Drawing.Color.DarkGray;
            this.btn_DecideParameterAll.Dock = System.Windows.Forms.DockStyle.Right;
            this.btn_DecideParameterAll.EdgeRadius = 5;
            this.btn_DecideParameterAll.GradientAngle = 60F;
            this.btn_DecideParameterAll.GradientFirstColor = System.Drawing.Color.DeepSkyBlue;
            this.btn_DecideParameterAll.GradientSecondColor = System.Drawing.Color.SteelBlue;
            this.btn_DecideParameterAll.HoverEmphasizeCustomColor = System.Drawing.Color.Firebrick;
            this.btn_DecideParameterAll.ImagePosition = new System.Drawing.Point(8, 7);
            this.btn_DecideParameterAll.ImageSize = new System.Drawing.Point(35, 35);
            this.btn_DecideParameterAll.LoadImage = global::FrameOfSystem3.Properties.Resources.save_52px;
            this.btn_DecideParameterAll.Location = new System.Drawing.Point(1091, 0);
            this.btn_DecideParameterAll.MainFont = new System.Drawing.Font("맑은 고딕", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btn_DecideParameterAll.MainFontColor = System.Drawing.Color.White;
            this.btn_DecideParameterAll.Margin = new System.Windows.Forms.Padding(1, 1, 0, 0);
            this.btn_DecideParameterAll.Name = "btn_DecideParameterAll";
            this.btn_DecideParameterAll.Size = new System.Drawing.Size(49, 49);
            this.btn_DecideParameterAll.SubFont = new System.Drawing.Font("맑은 고딕", 10F);
            this.btn_DecideParameterAll.SubFontColor = System.Drawing.Color.Black;
            this.btn_DecideParameterAll.SubText = "";
            this.btn_DecideParameterAll.TabIndex = 20837;
            this.btn_DecideParameterAll.Tag = "";
            this.btn_DecideParameterAll.TextAlignMain = Sys3Controls.EN_TEXTALIGN.MIDDLE_CENTER;
            this.btn_DecideParameterAll.TextAlignSub = Sys3Controls.EN_TEXTALIGN.MIDDLE_CENTER;
            this.btn_DecideParameterAll.ThemeIndex = 0;
            this.btn_DecideParameterAll.UseBorder = true;
            this.btn_DecideParameterAll.UseClickedEmphasizeTextColor = false;
            this.btn_DecideParameterAll.UseCustomizeClickedColor = true;
            this.btn_DecideParameterAll.UseEdge = true;
            this.btn_DecideParameterAll.UseHoverEmphasizeCustomColor = true;
            this.btn_DecideParameterAll.UseImage = true;
            this.btn_DecideParameterAll.UserHoverEmpahsize = true;
            this.btn_DecideParameterAll.UseSubFont = false;
            this.btn_DecideParameterAll.Click += new System.EventHandler(this.ClickParameterSave);
            // 
            // m_tabLP6
            // 
            this.m_tabLP6.BorderWidth = 5;
            this.m_tabLP6.ButtonClicked = false;
            this.m_tabLP6.ClickedEmphasizeTextColor = System.Drawing.Color.White;
            this.m_tabLP6.CustomClickedGradientFirstColor = System.Drawing.Color.Thistle;
            this.m_tabLP6.CustomClickedGradientSecondColor = System.Drawing.Color.PaleVioletRed;
            this.m_tabLP6.Description = "";
            this.m_tabLP6.DisabledColor = System.Drawing.Color.DarkGray;
            this.m_tabLP6.Dock = System.Windows.Forms.DockStyle.Left;
            this.m_tabLP6.EdgeRadius = 1;
            this.m_tabLP6.GradientAngle = 60F;
            this.m_tabLP6.GradientFirstColor = System.Drawing.Color.WhiteSmoke;
            this.m_tabLP6.GradientSecondColor = System.Drawing.Color.Gray;
            this.m_tabLP6.HoverEmphasizeCustomColor = System.Drawing.Color.Firebrick;
            this.m_tabLP6.ImagePosition = new System.Drawing.Point(37, 25);
            this.m_tabLP6.ImageSize = new System.Drawing.Point(30, 30);
            this.m_tabLP6.LoadImage = null;
            this.m_tabLP6.Location = new System.Drawing.Point(750, 0);
            this.m_tabLP6.MainFont = new System.Drawing.Font("맑은 고딕", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.m_tabLP6.MainFontColor = System.Drawing.Color.White;
            this.m_tabLP6.Margin = new System.Windows.Forms.Padding(1, 1, 0, 0);
            this.m_tabLP6.Name = "m_tabLP6";
            this.m_tabLP6.Size = new System.Drawing.Size(150, 49);
            this.m_tabLP6.SubFont = new System.Drawing.Font("맑은 고딕", 10F);
            this.m_tabLP6.SubFontColor = System.Drawing.Color.Black;
            this.m_tabLP6.SubText = "";
            this.m_tabLP6.TabIndex = 20835;
            this.m_tabLP6.Text = "LP6";
            this.m_tabLP6.TextAlignMain = Sys3Controls.EN_TEXTALIGN.MIDDLE_CENTER;
            this.m_tabLP6.TextAlignSub = Sys3Controls.EN_TEXTALIGN.TOP_LEFT;
            this.m_tabLP6.ThemeIndex = 0;
            this.m_tabLP6.UseBorder = true;
            this.m_tabLP6.UseClickedEmphasizeTextColor = false;
            this.m_tabLP6.UseCustomizeClickedColor = true;
            this.m_tabLP6.UseEdge = true;
            this.m_tabLP6.UseHoverEmphasizeCustomColor = true;
            this.m_tabLP6.UseImage = true;
            this.m_tabLP6.UserHoverEmpahsize = true;
            this.m_tabLP6.UseSubFont = true;
            this.m_tabLP6.Click += new System.EventHandler(this.Click_TabButton);
            // 
            // m_tabLP5
            // 
            this.m_tabLP5.BorderWidth = 5;
            this.m_tabLP5.ButtonClicked = false;
            this.m_tabLP5.ClickedEmphasizeTextColor = System.Drawing.Color.White;
            this.m_tabLP5.CustomClickedGradientFirstColor = System.Drawing.Color.Thistle;
            this.m_tabLP5.CustomClickedGradientSecondColor = System.Drawing.Color.PaleVioletRed;
            this.m_tabLP5.Description = "";
            this.m_tabLP5.DisabledColor = System.Drawing.Color.DarkGray;
            this.m_tabLP5.Dock = System.Windows.Forms.DockStyle.Left;
            this.m_tabLP5.EdgeRadius = 1;
            this.m_tabLP5.GradientAngle = 60F;
            this.m_tabLP5.GradientFirstColor = System.Drawing.Color.WhiteSmoke;
            this.m_tabLP5.GradientSecondColor = System.Drawing.Color.Gray;
            this.m_tabLP5.HoverEmphasizeCustomColor = System.Drawing.Color.Firebrick;
            this.m_tabLP5.ImagePosition = new System.Drawing.Point(37, 25);
            this.m_tabLP5.ImageSize = new System.Drawing.Point(30, 30);
            this.m_tabLP5.LoadImage = null;
            this.m_tabLP5.Location = new System.Drawing.Point(600, 0);
            this.m_tabLP5.MainFont = new System.Drawing.Font("맑은 고딕", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.m_tabLP5.MainFontColor = System.Drawing.Color.White;
            this.m_tabLP5.Margin = new System.Windows.Forms.Padding(1, 1, 0, 0);
            this.m_tabLP5.Name = "m_tabLP5";
            this.m_tabLP5.Size = new System.Drawing.Size(150, 49);
            this.m_tabLP5.SubFont = new System.Drawing.Font("맑은 고딕", 10F);
            this.m_tabLP5.SubFontColor = System.Drawing.Color.Black;
            this.m_tabLP5.SubText = "";
            this.m_tabLP5.TabIndex = 20834;
            this.m_tabLP5.Text = "LP5";
            this.m_tabLP5.TextAlignMain = Sys3Controls.EN_TEXTALIGN.MIDDLE_CENTER;
            this.m_tabLP5.TextAlignSub = Sys3Controls.EN_TEXTALIGN.TOP_LEFT;
            this.m_tabLP5.ThemeIndex = 0;
            this.m_tabLP5.UseBorder = true;
            this.m_tabLP5.UseClickedEmphasizeTextColor = false;
            this.m_tabLP5.UseCustomizeClickedColor = true;
            this.m_tabLP5.UseEdge = true;
            this.m_tabLP5.UseHoverEmphasizeCustomColor = true;
            this.m_tabLP5.UseImage = true;
            this.m_tabLP5.UserHoverEmpahsize = true;
            this.m_tabLP5.UseSubFont = true;
            this.m_tabLP5.Click += new System.EventHandler(this.Click_TabButton);
            // 
            // m_tabLP4
            // 
            this.m_tabLP4.BorderWidth = 5;
            this.m_tabLP4.ButtonClicked = false;
            this.m_tabLP4.ClickedEmphasizeTextColor = System.Drawing.Color.White;
            this.m_tabLP4.CustomClickedGradientFirstColor = System.Drawing.Color.Thistle;
            this.m_tabLP4.CustomClickedGradientSecondColor = System.Drawing.Color.PaleVioletRed;
            this.m_tabLP4.Description = "";
            this.m_tabLP4.DisabledColor = System.Drawing.Color.DarkGray;
            this.m_tabLP4.Dock = System.Windows.Forms.DockStyle.Left;
            this.m_tabLP4.EdgeRadius = 1;
            this.m_tabLP4.GradientAngle = 60F;
            this.m_tabLP4.GradientFirstColor = System.Drawing.Color.WhiteSmoke;
            this.m_tabLP4.GradientSecondColor = System.Drawing.Color.Gray;
            this.m_tabLP4.HoverEmphasizeCustomColor = System.Drawing.Color.Firebrick;
            this.m_tabLP4.ImagePosition = new System.Drawing.Point(37, 25);
            this.m_tabLP4.ImageSize = new System.Drawing.Point(30, 30);
            this.m_tabLP4.LoadImage = null;
            this.m_tabLP4.Location = new System.Drawing.Point(450, 0);
            this.m_tabLP4.MainFont = new System.Drawing.Font("맑은 고딕", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.m_tabLP4.MainFontColor = System.Drawing.Color.White;
            this.m_tabLP4.Margin = new System.Windows.Forms.Padding(1, 1, 0, 0);
            this.m_tabLP4.Name = "m_tabLP4";
            this.m_tabLP4.Size = new System.Drawing.Size(150, 49);
            this.m_tabLP4.SubFont = new System.Drawing.Font("맑은 고딕", 10F);
            this.m_tabLP4.SubFontColor = System.Drawing.Color.Black;
            this.m_tabLP4.SubText = "";
            this.m_tabLP4.TabIndex = 20833;
            this.m_tabLP4.Text = "LP4";
            this.m_tabLP4.TextAlignMain = Sys3Controls.EN_TEXTALIGN.MIDDLE_CENTER;
            this.m_tabLP4.TextAlignSub = Sys3Controls.EN_TEXTALIGN.TOP_LEFT;
            this.m_tabLP4.ThemeIndex = 0;
            this.m_tabLP4.UseBorder = true;
            this.m_tabLP4.UseClickedEmphasizeTextColor = false;
            this.m_tabLP4.UseCustomizeClickedColor = true;
            this.m_tabLP4.UseEdge = true;
            this.m_tabLP4.UseHoverEmphasizeCustomColor = true;
            this.m_tabLP4.UseImage = true;
            this.m_tabLP4.UserHoverEmpahsize = true;
            this.m_tabLP4.UseSubFont = true;
            this.m_tabLP4.Click += new System.EventHandler(this.Click_TabButton);
            // 
            // m_tabLP3
            // 
            this.m_tabLP3.BorderWidth = 5;
            this.m_tabLP3.ButtonClicked = false;
            this.m_tabLP3.ClickedEmphasizeTextColor = System.Drawing.Color.White;
            this.m_tabLP3.CustomClickedGradientFirstColor = System.Drawing.Color.Thistle;
            this.m_tabLP3.CustomClickedGradientSecondColor = System.Drawing.Color.PaleVioletRed;
            this.m_tabLP3.Description = "";
            this.m_tabLP3.DisabledColor = System.Drawing.Color.DarkGray;
            this.m_tabLP3.Dock = System.Windows.Forms.DockStyle.Left;
            this.m_tabLP3.EdgeRadius = 1;
            this.m_tabLP3.GradientAngle = 60F;
            this.m_tabLP3.GradientFirstColor = System.Drawing.Color.WhiteSmoke;
            this.m_tabLP3.GradientSecondColor = System.Drawing.Color.Gray;
            this.m_tabLP3.HoverEmphasizeCustomColor = System.Drawing.Color.Firebrick;
            this.m_tabLP3.ImagePosition = new System.Drawing.Point(37, 25);
            this.m_tabLP3.ImageSize = new System.Drawing.Point(30, 30);
            this.m_tabLP3.LoadImage = null;
            this.m_tabLP3.Location = new System.Drawing.Point(300, 0);
            this.m_tabLP3.MainFont = new System.Drawing.Font("맑은 고딕", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.m_tabLP3.MainFontColor = System.Drawing.Color.White;
            this.m_tabLP3.Margin = new System.Windows.Forms.Padding(1, 1, 0, 0);
            this.m_tabLP3.Name = "m_tabLP3";
            this.m_tabLP3.Size = new System.Drawing.Size(150, 49);
            this.m_tabLP3.SubFont = new System.Drawing.Font("맑은 고딕", 10F);
            this.m_tabLP3.SubFontColor = System.Drawing.Color.Black;
            this.m_tabLP3.SubText = "";
            this.m_tabLP3.TabIndex = 20832;
            this.m_tabLP3.Text = "LP3";
            this.m_tabLP3.TextAlignMain = Sys3Controls.EN_TEXTALIGN.MIDDLE_CENTER;
            this.m_tabLP3.TextAlignSub = Sys3Controls.EN_TEXTALIGN.TOP_LEFT;
            this.m_tabLP3.ThemeIndex = 0;
            this.m_tabLP3.UseBorder = true;
            this.m_tabLP3.UseClickedEmphasizeTextColor = false;
            this.m_tabLP3.UseCustomizeClickedColor = true;
            this.m_tabLP3.UseEdge = true;
            this.m_tabLP3.UseHoverEmphasizeCustomColor = true;
            this.m_tabLP3.UseImage = true;
            this.m_tabLP3.UserHoverEmpahsize = true;
            this.m_tabLP3.UseSubFont = true;
            this.m_tabLP3.Click += new System.EventHandler(this.Click_TabButton);
            // 
            // m_tabLP2
            // 
            this.m_tabLP2.BorderWidth = 5;
            this.m_tabLP2.ButtonClicked = false;
            this.m_tabLP2.ClickedEmphasizeTextColor = System.Drawing.Color.White;
            this.m_tabLP2.CustomClickedGradientFirstColor = System.Drawing.Color.Thistle;
            this.m_tabLP2.CustomClickedGradientSecondColor = System.Drawing.Color.PaleVioletRed;
            this.m_tabLP2.Description = "";
            this.m_tabLP2.DisabledColor = System.Drawing.Color.DarkGray;
            this.m_tabLP2.Dock = System.Windows.Forms.DockStyle.Left;
            this.m_tabLP2.EdgeRadius = 1;
            this.m_tabLP2.GradientAngle = 60F;
            this.m_tabLP2.GradientFirstColor = System.Drawing.Color.WhiteSmoke;
            this.m_tabLP2.GradientSecondColor = System.Drawing.Color.Gray;
            this.m_tabLP2.HoverEmphasizeCustomColor = System.Drawing.Color.Firebrick;
            this.m_tabLP2.ImagePosition = new System.Drawing.Point(37, 25);
            this.m_tabLP2.ImageSize = new System.Drawing.Point(30, 30);
            this.m_tabLP2.LoadImage = null;
            this.m_tabLP2.Location = new System.Drawing.Point(150, 0);
            this.m_tabLP2.MainFont = new System.Drawing.Font("맑은 고딕", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.m_tabLP2.MainFontColor = System.Drawing.Color.White;
            this.m_tabLP2.Margin = new System.Windows.Forms.Padding(1, 1, 0, 0);
            this.m_tabLP2.Name = "m_tabLP2";
            this.m_tabLP2.Size = new System.Drawing.Size(150, 49);
            this.m_tabLP2.SubFont = new System.Drawing.Font("맑은 고딕", 10F);
            this.m_tabLP2.SubFontColor = System.Drawing.Color.Black;
            this.m_tabLP2.SubText = "";
            this.m_tabLP2.TabIndex = 20831;
            this.m_tabLP2.Tag = "LOADPORT_2";
            this.m_tabLP2.Text = "LP2";
            this.m_tabLP2.TextAlignMain = Sys3Controls.EN_TEXTALIGN.MIDDLE_CENTER;
            this.m_tabLP2.TextAlignSub = Sys3Controls.EN_TEXTALIGN.TOP_LEFT;
            this.m_tabLP2.ThemeIndex = 0;
            this.m_tabLP2.UseBorder = true;
            this.m_tabLP2.UseClickedEmphasizeTextColor = false;
            this.m_tabLP2.UseCustomizeClickedColor = true;
            this.m_tabLP2.UseEdge = true;
            this.m_tabLP2.UseHoverEmphasizeCustomColor = true;
            this.m_tabLP2.UseImage = true;
            this.m_tabLP2.UserHoverEmpahsize = true;
            this.m_tabLP2.UseSubFont = true;
            this.m_tabLP2.Click += new System.EventHandler(this.Click_TabButton);
            // 
            // m_tabLP1
            // 
            this.m_tabLP1.BorderWidth = 5;
            this.m_tabLP1.ButtonClicked = false;
            this.m_tabLP1.ClickedEmphasizeTextColor = System.Drawing.Color.White;
            this.m_tabLP1.CustomClickedGradientFirstColor = System.Drawing.Color.Thistle;
            this.m_tabLP1.CustomClickedGradientSecondColor = System.Drawing.Color.PaleVioletRed;
            this.m_tabLP1.Description = "";
            this.m_tabLP1.DisabledColor = System.Drawing.Color.DarkGray;
            this.m_tabLP1.Dock = System.Windows.Forms.DockStyle.Left;
            this.m_tabLP1.EdgeRadius = 1;
            this.m_tabLP1.GradientAngle = 60F;
            this.m_tabLP1.GradientFirstColor = System.Drawing.Color.WhiteSmoke;
            this.m_tabLP1.GradientSecondColor = System.Drawing.Color.Gray;
            this.m_tabLP1.HoverEmphasizeCustomColor = System.Drawing.Color.Firebrick;
            this.m_tabLP1.ImagePosition = new System.Drawing.Point(37, 25);
            this.m_tabLP1.ImageSize = new System.Drawing.Point(30, 30);
            this.m_tabLP1.LoadImage = null;
            this.m_tabLP1.Location = new System.Drawing.Point(0, 0);
            this.m_tabLP1.MainFont = new System.Drawing.Font("맑은 고딕", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.m_tabLP1.MainFontColor = System.Drawing.Color.White;
            this.m_tabLP1.Margin = new System.Windows.Forms.Padding(1, 1, 0, 0);
            this.m_tabLP1.Name = "m_tabLP1";
            this.m_tabLP1.Size = new System.Drawing.Size(150, 49);
            this.m_tabLP1.SubFont = new System.Drawing.Font("맑은 고딕", 10F);
            this.m_tabLP1.SubFontColor = System.Drawing.Color.Black;
            this.m_tabLP1.SubText = "";
            this.m_tabLP1.TabIndex = 20535;
            this.m_tabLP1.Tag = "LOADPORT_1";
            this.m_tabLP1.Text = "LP1";
            this.m_tabLP1.TextAlignMain = Sys3Controls.EN_TEXTALIGN.MIDDLE_CENTER;
            this.m_tabLP1.TextAlignSub = Sys3Controls.EN_TEXTALIGN.TOP_LEFT;
            this.m_tabLP1.ThemeIndex = 0;
            this.m_tabLP1.UseBorder = true;
            this.m_tabLP1.UseClickedEmphasizeTextColor = false;
            this.m_tabLP1.UseCustomizeClickedColor = true;
            this.m_tabLP1.UseEdge = true;
            this.m_tabLP1.UseHoverEmphasizeCustomColor = true;
            this.m_tabLP1.UseImage = true;
            this.m_tabLP1.UserHoverEmpahsize = true;
            this.m_tabLP1.UseSubFont = true;
            this.m_tabLP1.Click += new System.EventHandler(this.Click_TabButton);
            // 
            // panelSubView
            // 
            this.panelSubView.AutoScroll = true;
            this.panelSubView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelSubView.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.panelSubView.Location = new System.Drawing.Point(0, 49);
            this.panelSubView.Margin = new System.Windows.Forms.Padding(1, 1, 0, 0);
            this.panelSubView.Name = "panelSubView";
            this.panelSubView.Size = new System.Drawing.Size(1140, 851);
            this.panelSubView.TabIndex = 20538;
            // 
            // Setup_LoadPort
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.Controls.Add(this.panelSubView);
            this.Controls.Add(this.panel1);
            this.Margin = new System.Windows.Forms.Padding(0);
            this.Name = "Setup_LoadPort";
            this.Size = new System.Drawing.Size(1140, 900);
            this.panel1.ResumeLayout(false);
            this.ResumeLayout(false);

		}
        #endregion
        private System.Windows.Forms.Panel panel1;
        private Sys3Controls.Sys3button m_tabLP2;
        private Sys3Controls.Sys3button m_tabLP1;
        private Sys3Controls.Sys3button m_tabLP6;
        private Sys3Controls.Sys3button m_tabLP5;
        private Sys3Controls.Sys3button m_tabLP4;
        private Sys3Controls.Sys3button m_tabLP3;
        private System.Windows.Forms.Panel panelSubView;
        private Sys3Controls.Sys3button btn_ParameterUndo;
        private Sys3Controls.Sys3button btn_DecideParameterAll;
    }
}
