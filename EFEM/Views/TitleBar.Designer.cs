namespace FrameOfSystem3.Views
{
    partial class TitleBar
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
			this.components = new System.ComponentModel.Container();
			this.m_labelMachineName = new Sys3Controls.Sys3Label();
			this.m_labelRecipeName = new Sys3Controls.Sys3Label();
			this.lblVersion = new Sys3Controls.Sys3Label();
			this.m_labelTimer = new Sys3Controls.Sys3Label();
			this.m_labelUserID = new Sys3Controls.Sys3Label();
			this.m_labelUserAuthority = new Sys3Controls.Sys3Label();
			this.m_labelMachineState = new Sys3Controls.Sys3Label();
			this.sys3Label1 = new Sys3Controls.Sys3Label();
			this.sys3Label2 = new Sys3Controls.Sys3Label();
			this.sys3Label3 = new Sys3Controls.Sys3Label();
			this.sys3Label4 = new Sys3Controls.Sys3Label();
			this.sys3Label5 = new Sys3Controls.Sys3Label();
			this.tt_FullVersion = new System.Windows.Forms.ToolTip(this.components);
			this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
			this.sys3Label7 = new Sys3Controls.Sys3Label();
			this.lbl_CommState = new Sys3Controls.Sys3Label();
			this.tableLayoutPanel1.SuspendLayout();
			this.SuspendLayout();
			// 
			// m_labelMachineName
			// 
			this.m_labelMachineName.BackGroundColor = System.Drawing.Color.Transparent;
			this.m_labelMachineName.BorderStroke = 1;
			this.m_labelMachineName.BorderStyle = System.Windows.Forms.ButtonBorderStyle.Inset;
			this.m_labelMachineName.Description = "";
			this.m_labelMachineName.DisabledColor = System.Drawing.Color.DarkGray;
			this.m_labelMachineName.Dock = System.Windows.Forms.DockStyle.Fill;
			this.m_labelMachineName.EdgeRadius = 1;
			this.m_labelMachineName.ImagePosition = new System.Drawing.Point(0, 0);
			this.m_labelMachineName.ImageSize = new System.Drawing.Point(0, 0);
			this.m_labelMachineName.LoadImage = null;
			this.m_labelMachineName.Location = new System.Drawing.Point(3, 3);
			this.m_labelMachineName.MainFont = new System.Drawing.Font("맑은 고딕", 20.25F, System.Drawing.FontStyle.Bold);
			this.m_labelMachineName.MainFontColor = System.Drawing.Color.White;
			this.m_labelMachineName.Margin = new System.Windows.Forms.Padding(0);
			this.m_labelMachineName.Name = "m_labelMachineName";
			this.tableLayoutPanel1.SetRowSpan(this.m_labelMachineName, 2);
			this.m_labelMachineName.Size = new System.Drawing.Size(283, 60);
			this.m_labelMachineName.SubFont = new System.Drawing.Font("맑은 고딕", 10F);
			this.m_labelMachineName.SubFontColor = System.Drawing.Color.Black;
			this.m_labelMachineName.SubText = "";
			this.m_labelMachineName.TabIndex = 1386;
			this.m_labelMachineName.Text = "Machine Name";
			this.m_labelMachineName.TextAlignMain = Sys3Controls.EN_TEXTALIGN.MIDDLE_CENTER;
			this.m_labelMachineName.TextAlignSub = Sys3Controls.EN_TEXTALIGN.MIDDLE_CENTER;
			this.m_labelMachineName.TextAlignUnit = Sys3Controls.EN_TEXTALIGN.MIDDLE_CENTER;
			this.m_labelMachineName.ThemeIndex = 0;
			this.m_labelMachineName.UnitAreaRate = 40;
			this.m_labelMachineName.UnitFont = new System.Drawing.Font("맑은 고딕", 10F, System.Drawing.FontStyle.Bold);
			this.m_labelMachineName.UnitFontColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
			this.m_labelMachineName.UnitPositionVertical = false;
			this.m_labelMachineName.UnitText = "";
			this.m_labelMachineName.UseBorder = true;
			this.m_labelMachineName.UseEdgeRadius = false;
			this.m_labelMachineName.UseImage = false;
			this.m_labelMachineName.UseSubFont = false;
			this.m_labelMachineName.UseUnitFont = false;
			// 
			// m_labelRecipeName
			// 
			this.m_labelRecipeName.BackGroundColor = System.Drawing.Color.Transparent;
			this.m_labelRecipeName.BorderStroke = 1;
			this.m_labelRecipeName.BorderStyle = System.Windows.Forms.ButtonBorderStyle.Inset;
			this.tableLayoutPanel1.SetColumnSpan(this.m_labelRecipeName, 3);
			this.m_labelRecipeName.Description = "";
			this.m_labelRecipeName.DisabledColor = System.Drawing.Color.DarkGray;
			this.m_labelRecipeName.Dock = System.Windows.Forms.DockStyle.Fill;
			this.m_labelRecipeName.EdgeRadius = 1;
			this.m_labelRecipeName.ImagePosition = new System.Drawing.Point(0, 0);
			this.m_labelRecipeName.ImageSize = new System.Drawing.Point(0, 0);
			this.m_labelRecipeName.LoadImage = null;
			this.m_labelRecipeName.Location = new System.Drawing.Point(369, 6);
			this.m_labelRecipeName.MainFont = new System.Drawing.Font("맑은 고딕", 10F, System.Drawing.FontStyle.Bold);
			this.m_labelRecipeName.MainFontColor = System.Drawing.Color.White;
			this.m_labelRecipeName.Name = "m_labelRecipeName";
			this.m_labelRecipeName.Size = new System.Drawing.Size(343, 24);
			this.m_labelRecipeName.SubFont = new System.Drawing.Font("맑은 고딕", 10F);
			this.m_labelRecipeName.SubFontColor = System.Drawing.Color.Black;
			this.m_labelRecipeName.SubText = "";
			this.m_labelRecipeName.TabIndex = 1387;
			this.m_labelRecipeName.Text = "RECIPE";
			this.m_labelRecipeName.TextAlignMain = Sys3Controls.EN_TEXTALIGN.MIDDLE_RIGHT;
			this.m_labelRecipeName.TextAlignSub = Sys3Controls.EN_TEXTALIGN.MIDDLE_CENTER;
			this.m_labelRecipeName.TextAlignUnit = Sys3Controls.EN_TEXTALIGN.MIDDLE_CENTER;
			this.m_labelRecipeName.ThemeIndex = 0;
			this.m_labelRecipeName.UnitAreaRate = 40;
			this.m_labelRecipeName.UnitFont = new System.Drawing.Font("맑은 고딕", 10F, System.Drawing.FontStyle.Bold);
			this.m_labelRecipeName.UnitFontColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
			this.m_labelRecipeName.UnitPositionVertical = false;
			this.m_labelRecipeName.UnitText = "";
			this.m_labelRecipeName.UseBorder = true;
			this.m_labelRecipeName.UseEdgeRadius = false;
			this.m_labelRecipeName.UseImage = false;
			this.m_labelRecipeName.UseSubFont = false;
			this.m_labelRecipeName.UseUnitFont = false;
			// 
			// lblVersion
			// 
			this.lblVersion.BackGroundColor = System.Drawing.Color.Transparent;
			this.lblVersion.BorderStroke = 1;
			this.lblVersion.BorderStyle = System.Windows.Forms.ButtonBorderStyle.Inset;
			this.lblVersion.Description = "";
			this.lblVersion.DisabledColor = System.Drawing.Color.DarkGray;
			this.lblVersion.Dock = System.Windows.Forms.DockStyle.Fill;
			this.lblVersion.EdgeRadius = 1;
			this.lblVersion.ImagePosition = new System.Drawing.Point(0, 0);
			this.lblVersion.ImageSize = new System.Drawing.Point(0, 0);
			this.lblVersion.LoadImage = null;
			this.lblVersion.Location = new System.Drawing.Point(369, 36);
			this.lblVersion.MainFont = new System.Drawing.Font("맑은 고딕", 10F, System.Drawing.FontStyle.Bold);
			this.lblVersion.MainFontColor = System.Drawing.Color.White;
			this.lblVersion.Name = "lblVersion";
			this.lblVersion.Size = new System.Drawing.Size(109, 24);
			this.lblVersion.SubFont = new System.Drawing.Font("맑은 고딕", 10F);
			this.lblVersion.SubFontColor = System.Drawing.Color.Black;
			this.lblVersion.SubText = "";
			this.lblVersion.TabIndex = 1387;
			this.lblVersion.Text = "10.10.10.10";
			this.lblVersion.TextAlignMain = Sys3Controls.EN_TEXTALIGN.MIDDLE_RIGHT;
			this.lblVersion.TextAlignSub = Sys3Controls.EN_TEXTALIGN.MIDDLE_CENTER;
			this.lblVersion.TextAlignUnit = Sys3Controls.EN_TEXTALIGN.MIDDLE_CENTER;
			this.lblVersion.ThemeIndex = 0;
			this.lblVersion.UnitAreaRate = 40;
			this.lblVersion.UnitFont = new System.Drawing.Font("맑은 고딕", 10F, System.Drawing.FontStyle.Bold);
			this.lblVersion.UnitFontColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
			this.lblVersion.UnitPositionVertical = false;
			this.lblVersion.UnitText = "";
			this.lblVersion.UseBorder = true;
			this.lblVersion.UseEdgeRadius = false;
			this.lblVersion.UseImage = false;
			this.lblVersion.UseSubFont = false;
			this.lblVersion.UseUnitFont = false;
			// 
			// m_labelTimer
			// 
			this.m_labelTimer.BackGroundColor = System.Drawing.Color.Transparent;
			this.m_labelTimer.BorderStroke = 1;
			this.m_labelTimer.BorderStyle = System.Windows.Forms.ButtonBorderStyle.Inset;
			this.m_labelTimer.Description = "";
			this.m_labelTimer.DisabledColor = System.Drawing.Color.DarkGray;
			this.m_labelTimer.Dock = System.Windows.Forms.DockStyle.Fill;
			this.m_labelTimer.EdgeRadius = 1;
			this.m_labelTimer.ImagePosition = new System.Drawing.Point(0, 0);
			this.m_labelTimer.ImageSize = new System.Drawing.Point(0, 0);
			this.m_labelTimer.LoadImage = null;
			this.m_labelTimer.Location = new System.Drawing.Point(534, 36);
			this.m_labelTimer.MainFont = new System.Drawing.Font("맑은 고딕", 10F, System.Drawing.FontStyle.Bold);
			this.m_labelTimer.MainFontColor = System.Drawing.Color.White;
			this.m_labelTimer.Name = "m_labelTimer";
			this.m_labelTimer.Size = new System.Drawing.Size(178, 24);
			this.m_labelTimer.SubFont = new System.Drawing.Font("맑은 고딕", 10F);
			this.m_labelTimer.SubFontColor = System.Drawing.Color.Black;
			this.m_labelTimer.SubText = "";
			this.m_labelTimer.TabIndex = 1387;
			this.m_labelTimer.Text = "yyyy-MM-dd HH:mm:ss";
			this.m_labelTimer.TextAlignMain = Sys3Controls.EN_TEXTALIGN.MIDDLE_RIGHT;
			this.m_labelTimer.TextAlignSub = Sys3Controls.EN_TEXTALIGN.MIDDLE_CENTER;
			this.m_labelTimer.TextAlignUnit = Sys3Controls.EN_TEXTALIGN.MIDDLE_CENTER;
			this.m_labelTimer.ThemeIndex = 0;
			this.m_labelTimer.UnitAreaRate = 40;
			this.m_labelTimer.UnitFont = new System.Drawing.Font("맑은 고딕", 10F, System.Drawing.FontStyle.Bold);
			this.m_labelTimer.UnitFontColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
			this.m_labelTimer.UnitPositionVertical = false;
			this.m_labelTimer.UnitText = "";
			this.m_labelTimer.UseBorder = true;
			this.m_labelTimer.UseEdgeRadius = false;
			this.m_labelTimer.UseImage = false;
			this.m_labelTimer.UseSubFont = false;
			this.m_labelTimer.UseUnitFont = false;
			// 
			// m_labelUserID
			// 
			this.m_labelUserID.BackGroundColor = System.Drawing.Color.Transparent;
			this.m_labelUserID.BorderStroke = 1;
			this.m_labelUserID.BorderStyle = System.Windows.Forms.ButtonBorderStyle.Inset;
			this.m_labelUserID.Description = "";
			this.m_labelUserID.DisabledColor = System.Drawing.Color.DarkGray;
			this.m_labelUserID.Dock = System.Windows.Forms.DockStyle.Fill;
			this.m_labelUserID.EdgeRadius = 1;
			this.m_labelUserID.ImagePosition = new System.Drawing.Point(0, 0);
			this.m_labelUserID.ImageSize = new System.Drawing.Point(0, 0);
			this.m_labelUserID.LoadImage = null;
			this.m_labelUserID.Location = new System.Drawing.Point(808, 36);
			this.m_labelUserID.MainFont = new System.Drawing.Font("맑은 고딕", 10F, System.Drawing.FontStyle.Bold);
			this.m_labelUserID.MainFontColor = System.Drawing.Color.White;
			this.m_labelUserID.Name = "m_labelUserID";
			this.m_labelUserID.Size = new System.Drawing.Size(94, 24);
			this.m_labelUserID.SubFont = new System.Drawing.Font("맑은 고딕", 10F);
			this.m_labelUserID.SubFontColor = System.Drawing.Color.Black;
			this.m_labelUserID.SubText = "";
			this.m_labelUserID.TabIndex = 1388;
			this.m_labelUserID.Text = "Operator";
			this.m_labelUserID.TextAlignMain = Sys3Controls.EN_TEXTALIGN.MIDDLE_RIGHT;
			this.m_labelUserID.TextAlignSub = Sys3Controls.EN_TEXTALIGN.MIDDLE_CENTER;
			this.m_labelUserID.TextAlignUnit = Sys3Controls.EN_TEXTALIGN.MIDDLE_CENTER;
			this.m_labelUserID.ThemeIndex = 0;
			this.m_labelUserID.UnitAreaRate = 40;
			this.m_labelUserID.UnitFont = new System.Drawing.Font("맑은 고딕", 10F, System.Drawing.FontStyle.Bold);
			this.m_labelUserID.UnitFontColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
			this.m_labelUserID.UnitPositionVertical = false;
			this.m_labelUserID.UnitText = "";
			this.m_labelUserID.UseBorder = true;
			this.m_labelUserID.UseEdgeRadius = false;
			this.m_labelUserID.UseImage = false;
			this.m_labelUserID.UseSubFont = false;
			this.m_labelUserID.UseUnitFont = false;
			// 
			// m_labelUserAuthority
			// 
			this.m_labelUserAuthority.BackGroundColor = System.Drawing.Color.Transparent;
			this.m_labelUserAuthority.BorderStroke = 1;
			this.m_labelUserAuthority.BorderStyle = System.Windows.Forms.ButtonBorderStyle.Inset;
			this.m_labelUserAuthority.Description = "";
			this.m_labelUserAuthority.DisabledColor = System.Drawing.Color.DarkGray;
			this.m_labelUserAuthority.Dock = System.Windows.Forms.DockStyle.Fill;
			this.m_labelUserAuthority.EdgeRadius = 1;
			this.m_labelUserAuthority.ImagePosition = new System.Drawing.Point(0, 0);
			this.m_labelUserAuthority.ImageSize = new System.Drawing.Point(0, 0);
			this.m_labelUserAuthority.LoadImage = null;
			this.m_labelUserAuthority.Location = new System.Drawing.Point(808, 6);
			this.m_labelUserAuthority.MainFont = new System.Drawing.Font("맑은 고딕", 10F, System.Drawing.FontStyle.Bold);
			this.m_labelUserAuthority.MainFontColor = System.Drawing.Color.White;
			this.m_labelUserAuthority.Name = "m_labelUserAuthority";
			this.m_labelUserAuthority.Size = new System.Drawing.Size(94, 24);
			this.m_labelUserAuthority.SubFont = new System.Drawing.Font("맑은 고딕", 10F);
			this.m_labelUserAuthority.SubFontColor = System.Drawing.Color.Black;
			this.m_labelUserAuthority.SubText = "";
			this.m_labelUserAuthority.TabIndex = 1389;
			this.m_labelUserAuthority.Text = "Operator";
			this.m_labelUserAuthority.TextAlignMain = Sys3Controls.EN_TEXTALIGN.MIDDLE_RIGHT;
			this.m_labelUserAuthority.TextAlignSub = Sys3Controls.EN_TEXTALIGN.MIDDLE_CENTER;
			this.m_labelUserAuthority.TextAlignUnit = Sys3Controls.EN_TEXTALIGN.MIDDLE_CENTER;
			this.m_labelUserAuthority.ThemeIndex = 0;
			this.m_labelUserAuthority.UnitAreaRate = 40;
			this.m_labelUserAuthority.UnitFont = new System.Drawing.Font("맑은 고딕", 10F, System.Drawing.FontStyle.Bold);
			this.m_labelUserAuthority.UnitFontColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
			this.m_labelUserAuthority.UnitPositionVertical = false;
			this.m_labelUserAuthority.UnitText = "";
			this.m_labelUserAuthority.UseBorder = true;
			this.m_labelUserAuthority.UseEdgeRadius = false;
			this.m_labelUserAuthority.UseImage = false;
			this.m_labelUserAuthority.UseSubFont = false;
			this.m_labelUserAuthority.UseUnitFont = false;
			// 
			// m_labelMachineState
			// 
			this.m_labelMachineState.BackGroundColor = System.Drawing.Color.DarkOrange;
			this.m_labelMachineState.BorderStroke = 1;
			this.m_labelMachineState.BorderStyle = System.Windows.Forms.ButtonBorderStyle.Inset;
			this.m_labelMachineState.Description = "";
			this.m_labelMachineState.DisabledColor = System.Drawing.Color.DarkGray;
			this.m_labelMachineState.Dock = System.Windows.Forms.DockStyle.Fill;
			this.m_labelMachineState.EdgeRadius = 1;
			this.m_labelMachineState.ImagePosition = new System.Drawing.Point(0, 0);
			this.m_labelMachineState.ImageSize = new System.Drawing.Point(0, 0);
			this.m_labelMachineState.LoadImage = null;
			this.m_labelMachineState.Location = new System.Drawing.Point(908, 6);
			this.m_labelMachineState.MainFont = new System.Drawing.Font("맑은 고딕", 20.25F, System.Drawing.FontStyle.Bold);
			this.m_labelMachineState.MainFontColor = System.Drawing.Color.White;
			this.m_labelMachineState.Name = "m_labelMachineState";
			this.tableLayoutPanel1.SetRowSpan(this.m_labelMachineState, 2);
			this.m_labelMachineState.Size = new System.Drawing.Size(224, 54);
			this.m_labelMachineState.SubFont = new System.Drawing.Font("맑은 고딕", 10F);
			this.m_labelMachineState.SubFontColor = System.Drawing.Color.Black;
			this.m_labelMachineState.SubText = "";
			this.m_labelMachineState.TabIndex = 1390;
			this.m_labelMachineState.Text = "STOP";
			this.m_labelMachineState.TextAlignMain = Sys3Controls.EN_TEXTALIGN.MIDDLE_CENTER;
			this.m_labelMachineState.TextAlignSub = Sys3Controls.EN_TEXTALIGN.MIDDLE_CENTER;
			this.m_labelMachineState.TextAlignUnit = Sys3Controls.EN_TEXTALIGN.MIDDLE_CENTER;
			this.m_labelMachineState.ThemeIndex = 0;
			this.m_labelMachineState.UnitAreaRate = 40;
			this.m_labelMachineState.UnitFont = new System.Drawing.Font("맑은 고딕", 10F, System.Drawing.FontStyle.Bold);
			this.m_labelMachineState.UnitFontColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
			this.m_labelMachineState.UnitPositionVertical = false;
			this.m_labelMachineState.UnitText = "";
			this.m_labelMachineState.UseBorder = true;
			this.m_labelMachineState.UseEdgeRadius = false;
			this.m_labelMachineState.UseImage = false;
			this.m_labelMachineState.UseSubFont = false;
			this.m_labelMachineState.UseUnitFont = false;
			// 
			// sys3Label1
			// 
			this.sys3Label1.BackGroundColor = System.Drawing.Color.Transparent;
			this.sys3Label1.BorderStroke = 1;
			this.sys3Label1.BorderStyle = System.Windows.Forms.ButtonBorderStyle.Inset;
			this.sys3Label1.Description = "";
			this.sys3Label1.DisabledColor = System.Drawing.Color.DarkGray;
			this.sys3Label1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.sys3Label1.EdgeRadius = 1;
			this.sys3Label1.ImagePosition = new System.Drawing.Point(0, 0);
			this.sys3Label1.ImageSize = new System.Drawing.Point(0, 0);
			this.sys3Label1.LoadImage = null;
			this.sys3Label1.Location = new System.Drawing.Point(289, 6);
			this.sys3Label1.MainFont = new System.Drawing.Font("맑은 고딕", 9F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))));
			this.sys3Label1.MainFontColor = System.Drawing.Color.White;
			this.sys3Label1.Name = "sys3Label1";
			this.sys3Label1.Size = new System.Drawing.Size(74, 24);
			this.sys3Label1.SubFont = new System.Drawing.Font("맑은 고딕", 10F);
			this.sys3Label1.SubFontColor = System.Drawing.Color.Black;
			this.sys3Label1.SubText = "";
			this.sys3Label1.TabIndex = 1391;
			this.sys3Label1.Text = "RECIPE";
			this.sys3Label1.TextAlignMain = Sys3Controls.EN_TEXTALIGN.MIDDLE_RIGHT;
			this.sys3Label1.TextAlignSub = Sys3Controls.EN_TEXTALIGN.MIDDLE_CENTER;
			this.sys3Label1.TextAlignUnit = Sys3Controls.EN_TEXTALIGN.MIDDLE_CENTER;
			this.sys3Label1.ThemeIndex = 0;
			this.sys3Label1.UnitAreaRate = 40;
			this.sys3Label1.UnitFont = new System.Drawing.Font("맑은 고딕", 10F, System.Drawing.FontStyle.Bold);
			this.sys3Label1.UnitFontColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
			this.sys3Label1.UnitPositionVertical = false;
			this.sys3Label1.UnitText = "";
			this.sys3Label1.UseBorder = false;
			this.sys3Label1.UseEdgeRadius = true;
			this.sys3Label1.UseImage = false;
			this.sys3Label1.UseSubFont = false;
			this.sys3Label1.UseUnitFont = false;
			// 
			// sys3Label2
			// 
			this.sys3Label2.BackGroundColor = System.Drawing.Color.Transparent;
			this.sys3Label2.BorderStroke = 1;
			this.sys3Label2.BorderStyle = System.Windows.Forms.ButtonBorderStyle.Inset;
			this.sys3Label2.Description = "";
			this.sys3Label2.DisabledColor = System.Drawing.Color.DarkGray;
			this.sys3Label2.Dock = System.Windows.Forms.DockStyle.Fill;
			this.sys3Label2.EdgeRadius = 1;
			this.sys3Label2.ImagePosition = new System.Drawing.Point(0, 0);
			this.sys3Label2.ImageSize = new System.Drawing.Point(0, 0);
			this.sys3Label2.LoadImage = null;
			this.sys3Label2.Location = new System.Drawing.Point(289, 36);
			this.sys3Label2.MainFont = new System.Drawing.Font("맑은 고딕", 9F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))));
			this.sys3Label2.MainFontColor = System.Drawing.Color.White;
			this.sys3Label2.Name = "sys3Label2";
			this.sys3Label2.Size = new System.Drawing.Size(74, 24);
			this.sys3Label2.SubFont = new System.Drawing.Font("맑은 고딕", 10F);
			this.sys3Label2.SubFontColor = System.Drawing.Color.Black;
			this.sys3Label2.SubText = "";
			this.sys3Label2.TabIndex = 1391;
			this.sys3Label2.Text = "VERSION";
			this.sys3Label2.TextAlignMain = Sys3Controls.EN_TEXTALIGN.MIDDLE_RIGHT;
			this.sys3Label2.TextAlignSub = Sys3Controls.EN_TEXTALIGN.MIDDLE_CENTER;
			this.sys3Label2.TextAlignUnit = Sys3Controls.EN_TEXTALIGN.MIDDLE_CENTER;
			this.sys3Label2.ThemeIndex = 0;
			this.sys3Label2.UnitAreaRate = 40;
			this.sys3Label2.UnitFont = new System.Drawing.Font("맑은 고딕", 10F, System.Drawing.FontStyle.Bold);
			this.sys3Label2.UnitFontColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
			this.sys3Label2.UnitPositionVertical = false;
			this.sys3Label2.UnitText = "";
			this.sys3Label2.UseBorder = false;
			this.sys3Label2.UseEdgeRadius = true;
			this.sys3Label2.UseImage = false;
			this.sys3Label2.UseSubFont = false;
			this.sys3Label2.UseUnitFont = false;
			// 
			// sys3Label3
			// 
			this.sys3Label3.BackGroundColor = System.Drawing.Color.Transparent;
			this.sys3Label3.BorderStroke = 1;
			this.sys3Label3.BorderStyle = System.Windows.Forms.ButtonBorderStyle.Inset;
			this.sys3Label3.Description = "";
			this.sys3Label3.DisabledColor = System.Drawing.Color.DarkGray;
			this.sys3Label3.Dock = System.Windows.Forms.DockStyle.Fill;
			this.sys3Label3.EdgeRadius = 1;
			this.sys3Label3.ImagePosition = new System.Drawing.Point(0, 0);
			this.sys3Label3.ImageSize = new System.Drawing.Point(0, 0);
			this.sys3Label3.LoadImage = null;
			this.sys3Label3.Location = new System.Drawing.Point(484, 36);
			this.sys3Label3.MainFont = new System.Drawing.Font("맑은 고딕", 9F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))));
			this.sys3Label3.MainFontColor = System.Drawing.Color.White;
			this.sys3Label3.Name = "sys3Label3";
			this.sys3Label3.Size = new System.Drawing.Size(44, 24);
			this.sys3Label3.SubFont = new System.Drawing.Font("맑은 고딕", 10F);
			this.sys3Label3.SubFontColor = System.Drawing.Color.Black;
			this.sys3Label3.SubText = "";
			this.sys3Label3.TabIndex = 1391;
			this.sys3Label3.Text = "TIME";
			this.sys3Label3.TextAlignMain = Sys3Controls.EN_TEXTALIGN.MIDDLE_RIGHT;
			this.sys3Label3.TextAlignSub = Sys3Controls.EN_TEXTALIGN.MIDDLE_CENTER;
			this.sys3Label3.TextAlignUnit = Sys3Controls.EN_TEXTALIGN.MIDDLE_CENTER;
			this.sys3Label3.ThemeIndex = 0;
			this.sys3Label3.UnitAreaRate = 40;
			this.sys3Label3.UnitFont = new System.Drawing.Font("맑은 고딕", 10F, System.Drawing.FontStyle.Bold);
			this.sys3Label3.UnitFontColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
			this.sys3Label3.UnitPositionVertical = false;
			this.sys3Label3.UnitText = "";
			this.sys3Label3.UseBorder = false;
			this.sys3Label3.UseEdgeRadius = true;
			this.sys3Label3.UseImage = false;
			this.sys3Label3.UseSubFont = false;
			this.sys3Label3.UseUnitFont = false;
			// 
			// sys3Label4
			// 
			this.sys3Label4.BackGroundColor = System.Drawing.Color.Transparent;
			this.sys3Label4.BorderStroke = 1;
			this.sys3Label4.BorderStyle = System.Windows.Forms.ButtonBorderStyle.Inset;
			this.sys3Label4.Description = "";
			this.sys3Label4.DisabledColor = System.Drawing.Color.DarkGray;
			this.sys3Label4.Dock = System.Windows.Forms.DockStyle.Fill;
			this.sys3Label4.EdgeRadius = 1;
			this.sys3Label4.ImagePosition = new System.Drawing.Point(0, 0);
			this.sys3Label4.ImageSize = new System.Drawing.Point(0, 0);
			this.sys3Label4.LoadImage = null;
			this.sys3Label4.Location = new System.Drawing.Point(718, 6);
			this.sys3Label4.MainFont = new System.Drawing.Font("맑은 고딕", 9F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))));
			this.sys3Label4.MainFontColor = System.Drawing.Color.White;
			this.sys3Label4.Name = "sys3Label4";
			this.sys3Label4.Size = new System.Drawing.Size(84, 24);
			this.sys3Label4.SubFont = new System.Drawing.Font("맑은 고딕", 10F);
			this.sys3Label4.SubFontColor = System.Drawing.Color.Black;
			this.sys3Label4.SubText = "";
			this.sys3Label4.TabIndex = 1391;
			this.sys3Label4.Text = "AUTHORITY";
			this.sys3Label4.TextAlignMain = Sys3Controls.EN_TEXTALIGN.MIDDLE_RIGHT;
			this.sys3Label4.TextAlignSub = Sys3Controls.EN_TEXTALIGN.MIDDLE_CENTER;
			this.sys3Label4.TextAlignUnit = Sys3Controls.EN_TEXTALIGN.MIDDLE_CENTER;
			this.sys3Label4.ThemeIndex = 0;
			this.sys3Label4.UnitAreaRate = 40;
			this.sys3Label4.UnitFont = new System.Drawing.Font("맑은 고딕", 10F, System.Drawing.FontStyle.Bold);
			this.sys3Label4.UnitFontColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
			this.sys3Label4.UnitPositionVertical = false;
			this.sys3Label4.UnitText = "";
			this.sys3Label4.UseBorder = false;
			this.sys3Label4.UseEdgeRadius = true;
			this.sys3Label4.UseImage = false;
			this.sys3Label4.UseSubFont = false;
			this.sys3Label4.UseUnitFont = false;
			// 
			// sys3Label5
			// 
			this.sys3Label5.BackGroundColor = System.Drawing.Color.Transparent;
			this.sys3Label5.BorderStroke = 1;
			this.sys3Label5.BorderStyle = System.Windows.Forms.ButtonBorderStyle.Inset;
			this.sys3Label5.Description = "";
			this.sys3Label5.DisabledColor = System.Drawing.Color.DarkGray;
			this.sys3Label5.Dock = System.Windows.Forms.DockStyle.Fill;
			this.sys3Label5.EdgeRadius = 1;
			this.sys3Label5.ImagePosition = new System.Drawing.Point(0, 0);
			this.sys3Label5.ImageSize = new System.Drawing.Point(0, 0);
			this.sys3Label5.LoadImage = null;
			this.sys3Label5.Location = new System.Drawing.Point(718, 36);
			this.sys3Label5.MainFont = new System.Drawing.Font("맑은 고딕", 9F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))));
			this.sys3Label5.MainFontColor = System.Drawing.Color.White;
			this.sys3Label5.Name = "sys3Label5";
			this.sys3Label5.Size = new System.Drawing.Size(84, 24);
			this.sys3Label5.SubFont = new System.Drawing.Font("맑은 고딕", 10F);
			this.sys3Label5.SubFontColor = System.Drawing.Color.Black;
			this.sys3Label5.SubText = "";
			this.sys3Label5.TabIndex = 1391;
			this.sys3Label5.Text = "USER";
			this.sys3Label5.TextAlignMain = Sys3Controls.EN_TEXTALIGN.MIDDLE_RIGHT;
			this.sys3Label5.TextAlignSub = Sys3Controls.EN_TEXTALIGN.MIDDLE_CENTER;
			this.sys3Label5.TextAlignUnit = Sys3Controls.EN_TEXTALIGN.MIDDLE_CENTER;
			this.sys3Label5.ThemeIndex = 0;
			this.sys3Label5.UnitAreaRate = 40;
			this.sys3Label5.UnitFont = new System.Drawing.Font("맑은 고딕", 10F, System.Drawing.FontStyle.Bold);
			this.sys3Label5.UnitFontColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
			this.sys3Label5.UnitPositionVertical = false;
			this.sys3Label5.UnitText = "";
			this.sys3Label5.UseBorder = false;
			this.sys3Label5.UseEdgeRadius = true;
			this.sys3Label5.UseImage = false;
			this.sys3Label5.UseSubFont = false;
			this.sys3Label5.UseUnitFont = false;
			// 
			// tableLayoutPanel1
			// 
			this.tableLayoutPanel1.ColumnCount = 9;
			this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 34.83715F));
			this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 80F));
			this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 14.17884F));
			this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 50F));
			this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 22.65687F));
			this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 90F));
			this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 100F));
			this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 28.32715F));
			this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 140F));
			this.tableLayoutPanel1.Controls.Add(this.sys3Label7, 8, 0);
			this.tableLayoutPanel1.Controls.Add(this.lbl_CommState, 8, 1);
			this.tableLayoutPanel1.Controls.Add(this.m_labelMachineName, 0, 0);
			this.tableLayoutPanel1.Controls.Add(this.m_labelMachineState, 7, 0);
			this.tableLayoutPanel1.Controls.Add(this.sys3Label5, 5, 1);
			this.tableLayoutPanel1.Controls.Add(this.m_labelUserID, 6, 1);
			this.tableLayoutPanel1.Controls.Add(this.m_labelUserAuthority, 6, 0);
			this.tableLayoutPanel1.Controls.Add(this.sys3Label1, 1, 0);
			this.tableLayoutPanel1.Controls.Add(this.sys3Label4, 5, 0);
			this.tableLayoutPanel1.Controls.Add(this.m_labelRecipeName, 2, 0);
			this.tableLayoutPanel1.Controls.Add(this.sys3Label3, 3, 1);
			this.tableLayoutPanel1.Controls.Add(this.sys3Label2, 1, 1);
			this.tableLayoutPanel1.Controls.Add(this.lblVersion, 2, 1);
			this.tableLayoutPanel1.Controls.Add(this.m_labelTimer, 4, 1);
			this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
			this.tableLayoutPanel1.Name = "tableLayoutPanel1";
			this.tableLayoutPanel1.Padding = new System.Windows.Forms.Padding(3);
			this.tableLayoutPanel1.RowCount = 2;
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
			this.tableLayoutPanel1.Size = new System.Drawing.Size(1280, 66);
			this.tableLayoutPanel1.TabIndex = 1392;
			// 
			// sys3Label7
			// 
			this.sys3Label7.BackGroundColor = System.Drawing.Color.Transparent;
			this.sys3Label7.BorderStroke = 1;
			this.sys3Label7.BorderStyle = System.Windows.Forms.ButtonBorderStyle.Inset;
			this.sys3Label7.Description = "";
			this.sys3Label7.DisabledColor = System.Drawing.Color.DarkGray;
			this.sys3Label7.Dock = System.Windows.Forms.DockStyle.Fill;
			this.sys3Label7.EdgeRadius = 1;
			this.sys3Label7.ImagePosition = new System.Drawing.Point(0, 0);
			this.sys3Label7.ImageSize = new System.Drawing.Point(0, 0);
			this.sys3Label7.LoadImage = null;
			this.sys3Label7.Location = new System.Drawing.Point(1138, 6);
			this.sys3Label7.MainFont = new System.Drawing.Font("맑은 고딕", 9F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))));
			this.sys3Label7.MainFontColor = System.Drawing.Color.White;
			this.sys3Label7.Name = "sys3Label7";
			this.sys3Label7.Size = new System.Drawing.Size(136, 24);
			this.sys3Label7.SubFont = new System.Drawing.Font("맑은 고딕", 10F);
			this.sys3Label7.SubFontColor = System.Drawing.Color.Black;
			this.sys3Label7.SubText = "";
			this.sys3Label7.TabIndex = 1395;
			this.sys3Label7.Text = "COMM STATE";
			this.sys3Label7.TextAlignMain = Sys3Controls.EN_TEXTALIGN.MIDDLE_RIGHT;
			this.sys3Label7.TextAlignSub = Sys3Controls.EN_TEXTALIGN.MIDDLE_CENTER;
			this.sys3Label7.TextAlignUnit = Sys3Controls.EN_TEXTALIGN.MIDDLE_CENTER;
			this.sys3Label7.ThemeIndex = 0;
			this.sys3Label7.UnitAreaRate = 40;
			this.sys3Label7.UnitFont = new System.Drawing.Font("맑은 고딕", 10F, System.Drawing.FontStyle.Bold);
			this.sys3Label7.UnitFontColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
			this.sys3Label7.UnitPositionVertical = false;
			this.sys3Label7.UnitText = "";
			this.sys3Label7.UseBorder = false;
			this.sys3Label7.UseEdgeRadius = true;
			this.sys3Label7.UseImage = false;
			this.sys3Label7.UseSubFont = false;
			this.sys3Label7.UseUnitFont = false;
			// 
			// lbl_CommState
			// 
			this.lbl_CommState.BackGroundColor = System.Drawing.Color.LimeGreen;
			this.lbl_CommState.BorderStroke = 1;
			this.lbl_CommState.BorderStyle = System.Windows.Forms.ButtonBorderStyle.Inset;
			this.lbl_CommState.Description = "";
			this.lbl_CommState.DisabledColor = System.Drawing.Color.DarkGray;
			this.lbl_CommState.Dock = System.Windows.Forms.DockStyle.Fill;
			this.lbl_CommState.EdgeRadius = 1;
			this.lbl_CommState.ImagePosition = new System.Drawing.Point(0, 0);
			this.lbl_CommState.ImageSize = new System.Drawing.Point(0, 0);
			this.lbl_CommState.LoadImage = null;
			this.lbl_CommState.Location = new System.Drawing.Point(1138, 36);
			this.lbl_CommState.MainFont = new System.Drawing.Font("맑은 고딕", 10F, System.Drawing.FontStyle.Bold);
			this.lbl_CommState.MainFontColor = System.Drawing.Color.White;
			this.lbl_CommState.Name = "lbl_CommState";
			this.lbl_CommState.Size = new System.Drawing.Size(136, 24);
			this.lbl_CommState.SubFont = new System.Drawing.Font("맑은 고딕", 10F);
			this.lbl_CommState.SubFontColor = System.Drawing.Color.Black;
			this.lbl_CommState.SubText = "";
			this.lbl_CommState.TabIndex = 1394;
			this.lbl_CommState.Text = "COMMUNICATING";
			this.lbl_CommState.TextAlignMain = Sys3Controls.EN_TEXTALIGN.MIDDLE_RIGHT;
			this.lbl_CommState.TextAlignSub = Sys3Controls.EN_TEXTALIGN.MIDDLE_CENTER;
			this.lbl_CommState.TextAlignUnit = Sys3Controls.EN_TEXTALIGN.MIDDLE_CENTER;
			this.lbl_CommState.ThemeIndex = 0;
			this.lbl_CommState.UnitAreaRate = 40;
			this.lbl_CommState.UnitFont = new System.Drawing.Font("맑은 고딕", 10F, System.Drawing.FontStyle.Bold);
			this.lbl_CommState.UnitFontColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
			this.lbl_CommState.UnitPositionVertical = false;
			this.lbl_CommState.UnitText = "";
			this.lbl_CommState.UseBorder = true;
			this.lbl_CommState.UseEdgeRadius = false;
			this.lbl_CommState.UseImage = false;
			this.lbl_CommState.UseSubFont = false;
			this.lbl_CommState.UseUnitFont = false;
			// 
			// TitleBar
			// 
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
			this.BackColor = System.Drawing.Color.Gray;
			this.Controls.Add(this.tableLayoutPanel1);
			this.Margin = new System.Windows.Forms.Padding(0);
			this.Name = "TitleBar";
			this.Size = new System.Drawing.Size(1280, 66);
			this.tableLayoutPanel1.ResumeLayout(false);
			this.ResumeLayout(false);

        }

        #endregion

		private Sys3Controls.Sys3Label m_labelMachineName;
		private Sys3Controls.Sys3Label m_labelRecipeName;
		private Sys3Controls.Sys3Label lblVersion;
		private Sys3Controls.Sys3Label m_labelTimer;
		private Sys3Controls.Sys3Label m_labelUserID;
		private Sys3Controls.Sys3Label m_labelUserAuthority;
		private Sys3Controls.Sys3Label m_labelMachineState;
		private Sys3Controls.Sys3Label sys3Label1;
		private Sys3Controls.Sys3Label sys3Label2;
		private Sys3Controls.Sys3Label sys3Label3;
		private Sys3Controls.Sys3Label sys3Label4;
		private Sys3Controls.Sys3Label sys3Label5;
        private System.Windows.Forms.ToolTip tt_FullVersion;
		private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
		private Sys3Controls.Sys3Label sys3Label7;
		private Sys3Controls.Sys3Label lbl_CommState;

    }
}
