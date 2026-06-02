
namespace FrameOfSystem3.Views.EFEM_Simulator
{
    partial class EFEMSimulator_LoarPort
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
            this.btnCarrierRemoved = new Sys3Controls.Sys3button();
            this.btnCarrierPlaced = new Sys3Controls.Sys3button();
            this.lblLoadPort = new Sys3Controls.Sys3Label();
            this.lblLoadPortSelection = new Sys3Controls.Sys3Label();
            this.sys3Label1 = new Sys3Controls.Sys3Label();
            this.lblTransferState = new Sys3Controls.Sys3Label();
            this.sys3Label3 = new Sys3Controls.Sys3Label();
            this.lblIdState = new Sys3Controls.Sys3Label();
            this.sys3Label5 = new Sys3Controls.Sys3Label();
            this.lblSlotMapState = new Sys3Controls.Sys3Label();
            this.btnUnloadButton = new Sys3Controls.Sys3button();
            this.btnLoadButton = new Sys3Controls.Sys3button();
            this.btnLoadCarrierAll = new Sys3Controls.Sys3button();
            this.btnRemoveCarrierAll = new Sys3Controls.Sys3button();
            this.btnUnloadCarrierAll = new Sys3Controls.Sys3button();
            this.btnClearLoadPortInfo = new Sys3Controls.Sys3button();
            this.btnClearPMInfo = new Sys3Controls.Sys3button();
            this.btnClearRobotInfo = new Sys3Controls.Sys3button();
            this.SuspendLayout();
            // 
            // btnCarrierRemoved
            // 
            this.btnCarrierRemoved.BorderWidth = 3;
            this.btnCarrierRemoved.ButtonClicked = false;
            this.btnCarrierRemoved.ClickedEmphasizeTextColor = System.Drawing.Color.White;
            this.btnCarrierRemoved.CustomClickedGradientFirstColor = System.Drawing.Color.White;
            this.btnCarrierRemoved.CustomClickedGradientSecondColor = System.Drawing.Color.White;
            this.btnCarrierRemoved.Description = "";
            this.btnCarrierRemoved.DisabledColor = System.Drawing.Color.DarkGray;
            this.btnCarrierRemoved.EdgeRadius = 1;
            this.btnCarrierRemoved.GradientAngle = 90F;
            this.btnCarrierRemoved.GradientFirstColor = System.Drawing.Color.DimGray;
            this.btnCarrierRemoved.GradientSecondColor = System.Drawing.Color.DarkGray;
            this.btnCarrierRemoved.HoverEmphasizeCustomColor = System.Drawing.Color.White;
            this.btnCarrierRemoved.ImagePosition = new System.Drawing.Point(78, 10);
            this.btnCarrierRemoved.ImageSize = new System.Drawing.Point(30, 30);
            this.btnCarrierRemoved.LoadImage = null;
            this.btnCarrierRemoved.Location = new System.Drawing.Point(144, 55);
            this.btnCarrierRemoved.MainFont = new System.Drawing.Font("맑은 고딕", 11F, System.Drawing.FontStyle.Bold);
            this.btnCarrierRemoved.MainFontColor = System.Drawing.Color.White;
            this.btnCarrierRemoved.Name = "btnCarrierRemoved";
            this.btnCarrierRemoved.Size = new System.Drawing.Size(127, 53);
            this.btnCarrierRemoved.SubFont = new System.Drawing.Font("맑은 고딕", 10F);
            this.btnCarrierRemoved.SubFontColor = System.Drawing.Color.Black;
            this.btnCarrierRemoved.SubText = "";
            this.btnCarrierRemoved.TabIndex = 1486;
            this.btnCarrierRemoved.Text = "Remove Carrier";
            this.btnCarrierRemoved.TextAlignMain = Sys3Controls.EN_TEXTALIGN.MIDDLE_CENTER;
            this.btnCarrierRemoved.TextAlignSub = Sys3Controls.EN_TEXTALIGN.MIDDLE_CENTER;
            this.btnCarrierRemoved.ThemeIndex = 0;
            this.btnCarrierRemoved.UseBorder = true;
            this.btnCarrierRemoved.UseClickedEmphasizeTextColor = false;
            this.btnCarrierRemoved.UseCustomizeClickedColor = false;
            this.btnCarrierRemoved.UseEdge = true;
            this.btnCarrierRemoved.UseHoverEmphasizeCustomColor = false;
            this.btnCarrierRemoved.UseImage = false;
            this.btnCarrierRemoved.UserHoverEmpahsize = false;
            this.btnCarrierRemoved.UseSubFont = false;
            this.btnCarrierRemoved.Click += new System.EventHandler(this.BtnCarrierPresenceClicked);
            // 
            // btnCarrierPlaced
            // 
            this.btnCarrierPlaced.BorderWidth = 3;
            this.btnCarrierPlaced.ButtonClicked = false;
            this.btnCarrierPlaced.ClickedEmphasizeTextColor = System.Drawing.Color.White;
            this.btnCarrierPlaced.CustomClickedGradientFirstColor = System.Drawing.Color.White;
            this.btnCarrierPlaced.CustomClickedGradientSecondColor = System.Drawing.Color.White;
            this.btnCarrierPlaced.Description = "";
            this.btnCarrierPlaced.DisabledColor = System.Drawing.Color.DarkGray;
            this.btnCarrierPlaced.EdgeRadius = 1;
            this.btnCarrierPlaced.GradientAngle = 90F;
            this.btnCarrierPlaced.GradientFirstColor = System.Drawing.Color.DimGray;
            this.btnCarrierPlaced.GradientSecondColor = System.Drawing.Color.DarkGray;
            this.btnCarrierPlaced.HoverEmphasizeCustomColor = System.Drawing.Color.White;
            this.btnCarrierPlaced.ImagePosition = new System.Drawing.Point(78, 10);
            this.btnCarrierPlaced.ImageSize = new System.Drawing.Point(30, 30);
            this.btnCarrierPlaced.LoadImage = null;
            this.btnCarrierPlaced.Location = new System.Drawing.Point(10, 55);
            this.btnCarrierPlaced.MainFont = new System.Drawing.Font("맑은 고딕", 11F, System.Drawing.FontStyle.Bold);
            this.btnCarrierPlaced.MainFontColor = System.Drawing.Color.White;
            this.btnCarrierPlaced.Name = "btnCarrierPlaced";
            this.btnCarrierPlaced.Size = new System.Drawing.Size(127, 53);
            this.btnCarrierPlaced.SubFont = new System.Drawing.Font("맑은 고딕", 10F);
            this.btnCarrierPlaced.SubFontColor = System.Drawing.Color.Black;
            this.btnCarrierPlaced.SubText = "";
            this.btnCarrierPlaced.TabIndex = 1485;
            this.btnCarrierPlaced.Text = "Place Carrier";
            this.btnCarrierPlaced.TextAlignMain = Sys3Controls.EN_TEXTALIGN.MIDDLE_CENTER;
            this.btnCarrierPlaced.TextAlignSub = Sys3Controls.EN_TEXTALIGN.MIDDLE_CENTER;
            this.btnCarrierPlaced.ThemeIndex = 0;
            this.btnCarrierPlaced.UseBorder = true;
            this.btnCarrierPlaced.UseClickedEmphasizeTextColor = false;
            this.btnCarrierPlaced.UseCustomizeClickedColor = false;
            this.btnCarrierPlaced.UseEdge = true;
            this.btnCarrierPlaced.UseHoverEmphasizeCustomColor = false;
            this.btnCarrierPlaced.UseImage = false;
            this.btnCarrierPlaced.UserHoverEmpahsize = false;
            this.btnCarrierPlaced.UseSubFont = false;
            this.btnCarrierPlaced.Click += new System.EventHandler(this.BtnCarrierPresenceClicked);
            // 
            // lblLoadPort
            // 
            this.lblLoadPort.BackGroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(200)))), ((int)(((byte)(200)))), ((int)(((byte)(220)))));
            this.lblLoadPort.BorderStroke = 2;
            this.lblLoadPort.BorderStyle = System.Windows.Forms.ButtonBorderStyle.Solid;
            this.lblLoadPort.Description = "";
            this.lblLoadPort.DisabledColor = System.Drawing.Color.DarkGray;
            this.lblLoadPort.EdgeRadius = 1;
            this.lblLoadPort.ImagePosition = new System.Drawing.Point(0, 0);
            this.lblLoadPort.ImageSize = new System.Drawing.Point(0, 0);
            this.lblLoadPort.LoadImage = null;
            this.lblLoadPort.Location = new System.Drawing.Point(10, 11);
            this.lblLoadPort.MainFont = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Bold);
            this.lblLoadPort.MainFontColor = System.Drawing.Color.Black;
            this.lblLoadPort.Margin = new System.Windows.Forms.Padding(1);
            this.lblLoadPort.Name = "lblLoadPort";
            this.lblLoadPort.Size = new System.Drawing.Size(80, 40);
            this.lblLoadPort.SubFont = new System.Drawing.Font("맑은 고딕", 10F);
            this.lblLoadPort.SubFontColor = System.Drawing.Color.Black;
            this.lblLoadPort.SubText = "";
            this.lblLoadPort.TabIndex = 21143;
            this.lblLoadPort.Text = "LOADPORT";
            this.lblLoadPort.TextAlignMain = Sys3Controls.EN_TEXTALIGN.TOP_LEFT;
            this.lblLoadPort.TextAlignSub = Sys3Controls.EN_TEXTALIGN.MIDDLE_CENTER;
            this.lblLoadPort.TextAlignUnit = Sys3Controls.EN_TEXTALIGN.MIDDLE_CENTER;
            this.lblLoadPort.ThemeIndex = 0;
            this.lblLoadPort.UnitAreaRate = 40;
            this.lblLoadPort.UnitFont = new System.Drawing.Font("맑은 고딕", 10F, System.Drawing.FontStyle.Bold);
            this.lblLoadPort.UnitFontColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.lblLoadPort.UnitPositionVertical = false;
            this.lblLoadPort.UnitText = "";
            this.lblLoadPort.UseBorder = true;
            this.lblLoadPort.UseEdgeRadius = false;
            this.lblLoadPort.UseImage = false;
            this.lblLoadPort.UseSubFont = false;
            this.lblLoadPort.UseUnitFont = false;
            // 
            // lblLoadPortSelection
            // 
            this.lblLoadPortSelection.BackGroundColor = System.Drawing.Color.White;
            this.lblLoadPortSelection.BorderStroke = 2;
            this.lblLoadPortSelection.BorderStyle = System.Windows.Forms.ButtonBorderStyle.Solid;
            this.lblLoadPortSelection.Description = "";
            this.lblLoadPortSelection.DisabledColor = System.Drawing.Color.LightGray;
            this.lblLoadPortSelection.EdgeRadius = 1;
            this.lblLoadPortSelection.ImagePosition = new System.Drawing.Point(0, 0);
            this.lblLoadPortSelection.ImageSize = new System.Drawing.Point(0, 0);
            this.lblLoadPortSelection.LoadImage = null;
            this.lblLoadPortSelection.Location = new System.Drawing.Point(92, 11);
            this.lblLoadPortSelection.MainFont = new System.Drawing.Font("맑은 고딕", 11F, System.Drawing.FontStyle.Bold);
            this.lblLoadPortSelection.MainFontColor = System.Drawing.Color.Black;
            this.lblLoadPortSelection.Margin = new System.Windows.Forms.Padding(1);
            this.lblLoadPortSelection.Name = "lblLoadPortSelection";
            this.lblLoadPortSelection.Size = new System.Drawing.Size(179, 40);
            this.lblLoadPortSelection.SubFont = new System.Drawing.Font("맑은 고딕", 10F);
            this.lblLoadPortSelection.SubFontColor = System.Drawing.Color.Black;
            this.lblLoadPortSelection.SubText = "";
            this.lblLoadPortSelection.TabIndex = 21142;
            this.lblLoadPortSelection.Text = "--";
            this.lblLoadPortSelection.TextAlignMain = Sys3Controls.EN_TEXTALIGN.BOTTOM_RIGHT;
            this.lblLoadPortSelection.TextAlignSub = Sys3Controls.EN_TEXTALIGN.MIDDLE_CENTER;
            this.lblLoadPortSelection.TextAlignUnit = Sys3Controls.EN_TEXTALIGN.MIDDLE_CENTER;
            this.lblLoadPortSelection.ThemeIndex = 0;
            this.lblLoadPortSelection.UnitAreaRate = 40;
            this.lblLoadPortSelection.UnitFont = new System.Drawing.Font("맑은 고딕", 10F, System.Drawing.FontStyle.Bold);
            this.lblLoadPortSelection.UnitFontColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.lblLoadPortSelection.UnitPositionVertical = false;
            this.lblLoadPortSelection.UnitText = "";
            this.lblLoadPortSelection.UseBorder = true;
            this.lblLoadPortSelection.UseEdgeRadius = false;
            this.lblLoadPortSelection.UseImage = false;
            this.lblLoadPortSelection.UseSubFont = false;
            this.lblLoadPortSelection.UseUnitFont = false;
            this.lblLoadPortSelection.Click += new System.EventHandler(this.LblLoadPortSlectionClicked);
            // 
            // sys3Label1
            // 
            this.sys3Label1.BackGroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(200)))), ((int)(((byte)(200)))), ((int)(((byte)(220)))));
            this.sys3Label1.BorderStroke = 2;
            this.sys3Label1.BorderStyle = System.Windows.Forms.ButtonBorderStyle.Solid;
            this.sys3Label1.Description = "";
            this.sys3Label1.DisabledColor = System.Drawing.Color.DarkGray;
            this.sys3Label1.EdgeRadius = 1;
            this.sys3Label1.ImagePosition = new System.Drawing.Point(0, 0);
            this.sys3Label1.ImageSize = new System.Drawing.Point(0, 0);
            this.sys3Label1.LoadImage = null;
            this.sys3Label1.Location = new System.Drawing.Point(10, 112);
            this.sys3Label1.MainFont = new System.Drawing.Font("맑은 고딕", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.sys3Label1.MainFontColor = System.Drawing.Color.Black;
            this.sys3Label1.Margin = new System.Windows.Forms.Padding(1);
            this.sys3Label1.Name = "sys3Label1";
            this.sys3Label1.Size = new System.Drawing.Size(80, 40);
            this.sys3Label1.SubFont = new System.Drawing.Font("맑은 고딕", 10F);
            this.sys3Label1.SubFontColor = System.Drawing.Color.Black;
            this.sys3Label1.SubText = "";
            this.sys3Label1.TabIndex = 21145;
            this.sys3Label1.Text = "TRANSFER STATE";
            this.sys3Label1.TextAlignMain = Sys3Controls.EN_TEXTALIGN.TOP_LEFT;
            this.sys3Label1.TextAlignSub = Sys3Controls.EN_TEXTALIGN.MIDDLE_CENTER;
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
            this.sys3Label1.UseSubFont = false;
            this.sys3Label1.UseUnitFont = false;
            // 
            // lblTransferState
            // 
            this.lblTransferState.BackGroundColor = System.Drawing.Color.White;
            this.lblTransferState.BorderStroke = 2;
            this.lblTransferState.BorderStyle = System.Windows.Forms.ButtonBorderStyle.Solid;
            this.lblTransferState.Description = "";
            this.lblTransferState.DisabledColor = System.Drawing.Color.LightGray;
            this.lblTransferState.EdgeRadius = 1;
            this.lblTransferState.ImagePosition = new System.Drawing.Point(0, 0);
            this.lblTransferState.ImageSize = new System.Drawing.Point(0, 0);
            this.lblTransferState.LoadImage = null;
            this.lblTransferState.Location = new System.Drawing.Point(92, 112);
            this.lblTransferState.MainFont = new System.Drawing.Font("맑은 고딕", 11F, System.Drawing.FontStyle.Bold);
            this.lblTransferState.MainFontColor = System.Drawing.Color.Black;
            this.lblTransferState.Margin = new System.Windows.Forms.Padding(1);
            this.lblTransferState.Name = "lblTransferState";
            this.lblTransferState.Size = new System.Drawing.Size(179, 40);
            this.lblTransferState.SubFont = new System.Drawing.Font("맑은 고딕", 10F);
            this.lblTransferState.SubFontColor = System.Drawing.Color.Black;
            this.lblTransferState.SubText = "";
            this.lblTransferState.TabIndex = 21144;
            this.lblTransferState.Text = "--";
            this.lblTransferState.TextAlignMain = Sys3Controls.EN_TEXTALIGN.BOTTOM_RIGHT;
            this.lblTransferState.TextAlignSub = Sys3Controls.EN_TEXTALIGN.MIDDLE_CENTER;
            this.lblTransferState.TextAlignUnit = Sys3Controls.EN_TEXTALIGN.MIDDLE_CENTER;
            this.lblTransferState.ThemeIndex = 0;
            this.lblTransferState.UnitAreaRate = 40;
            this.lblTransferState.UnitFont = new System.Drawing.Font("맑은 고딕", 10F, System.Drawing.FontStyle.Bold);
            this.lblTransferState.UnitFontColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.lblTransferState.UnitPositionVertical = false;
            this.lblTransferState.UnitText = "";
            this.lblTransferState.UseBorder = true;
            this.lblTransferState.UseEdgeRadius = false;
            this.lblTransferState.UseImage = false;
            this.lblTransferState.UseSubFont = false;
            this.lblTransferState.UseUnitFont = false;
            // 
            // sys3Label3
            // 
            this.sys3Label3.BackGroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(200)))), ((int)(((byte)(200)))), ((int)(((byte)(220)))));
            this.sys3Label3.BorderStroke = 2;
            this.sys3Label3.BorderStyle = System.Windows.Forms.ButtonBorderStyle.Solid;
            this.sys3Label3.Description = "";
            this.sys3Label3.DisabledColor = System.Drawing.Color.DarkGray;
            this.sys3Label3.EdgeRadius = 1;
            this.sys3Label3.ImagePosition = new System.Drawing.Point(0, 0);
            this.sys3Label3.ImageSize = new System.Drawing.Point(0, 0);
            this.sys3Label3.LoadImage = null;
            this.sys3Label3.Location = new System.Drawing.Point(10, 154);
            this.sys3Label3.MainFont = new System.Drawing.Font("맑은 고딕", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.sys3Label3.MainFontColor = System.Drawing.Color.Black;
            this.sys3Label3.Margin = new System.Windows.Forms.Padding(1);
            this.sys3Label3.Name = "sys3Label3";
            this.sys3Label3.Size = new System.Drawing.Size(80, 40);
            this.sys3Label3.SubFont = new System.Drawing.Font("맑은 고딕", 10F);
            this.sys3Label3.SubFontColor = System.Drawing.Color.Black;
            this.sys3Label3.SubText = "";
            this.sys3Label3.TabIndex = 21147;
            this.sys3Label3.Text = "CARRIER ID STATE";
            this.sys3Label3.TextAlignMain = Sys3Controls.EN_TEXTALIGN.TOP_LEFT;
            this.sys3Label3.TextAlignSub = Sys3Controls.EN_TEXTALIGN.MIDDLE_CENTER;
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
            this.sys3Label3.UseSubFont = false;
            this.sys3Label3.UseUnitFont = false;
            // 
            // lblIdState
            // 
            this.lblIdState.BackGroundColor = System.Drawing.Color.White;
            this.lblIdState.BorderStroke = 2;
            this.lblIdState.BorderStyle = System.Windows.Forms.ButtonBorderStyle.Solid;
            this.lblIdState.Description = "";
            this.lblIdState.DisabledColor = System.Drawing.Color.LightGray;
            this.lblIdState.EdgeRadius = 1;
            this.lblIdState.ImagePosition = new System.Drawing.Point(0, 0);
            this.lblIdState.ImageSize = new System.Drawing.Point(0, 0);
            this.lblIdState.LoadImage = null;
            this.lblIdState.Location = new System.Drawing.Point(92, 154);
            this.lblIdState.MainFont = new System.Drawing.Font("맑은 고딕", 11F, System.Drawing.FontStyle.Bold);
            this.lblIdState.MainFontColor = System.Drawing.Color.Black;
            this.lblIdState.Margin = new System.Windows.Forms.Padding(1);
            this.lblIdState.Name = "lblIdState";
            this.lblIdState.Size = new System.Drawing.Size(179, 40);
            this.lblIdState.SubFont = new System.Drawing.Font("맑은 고딕", 10F);
            this.lblIdState.SubFontColor = System.Drawing.Color.Black;
            this.lblIdState.SubText = "";
            this.lblIdState.TabIndex = 21146;
            this.lblIdState.Text = "--";
            this.lblIdState.TextAlignMain = Sys3Controls.EN_TEXTALIGN.BOTTOM_RIGHT;
            this.lblIdState.TextAlignSub = Sys3Controls.EN_TEXTALIGN.MIDDLE_CENTER;
            this.lblIdState.TextAlignUnit = Sys3Controls.EN_TEXTALIGN.MIDDLE_CENTER;
            this.lblIdState.ThemeIndex = 0;
            this.lblIdState.UnitAreaRate = 40;
            this.lblIdState.UnitFont = new System.Drawing.Font("맑은 고딕", 10F, System.Drawing.FontStyle.Bold);
            this.lblIdState.UnitFontColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.lblIdState.UnitPositionVertical = false;
            this.lblIdState.UnitText = "";
            this.lblIdState.UseBorder = true;
            this.lblIdState.UseEdgeRadius = false;
            this.lblIdState.UseImage = false;
            this.lblIdState.UseSubFont = false;
            this.lblIdState.UseUnitFont = false;
            // 
            // sys3Label5
            // 
            this.sys3Label5.BackGroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(200)))), ((int)(((byte)(200)))), ((int)(((byte)(220)))));
            this.sys3Label5.BorderStroke = 2;
            this.sys3Label5.BorderStyle = System.Windows.Forms.ButtonBorderStyle.Solid;
            this.sys3Label5.Description = "";
            this.sys3Label5.DisabledColor = System.Drawing.Color.DarkGray;
            this.sys3Label5.EdgeRadius = 1;
            this.sys3Label5.ImagePosition = new System.Drawing.Point(0, 0);
            this.sys3Label5.ImageSize = new System.Drawing.Point(0, 0);
            this.sys3Label5.LoadImage = null;
            this.sys3Label5.Location = new System.Drawing.Point(10, 196);
            this.sys3Label5.MainFont = new System.Drawing.Font("맑은 고딕", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.sys3Label5.MainFontColor = System.Drawing.Color.Black;
            this.sys3Label5.Margin = new System.Windows.Forms.Padding(1);
            this.sys3Label5.Name = "sys3Label5";
            this.sys3Label5.Size = new System.Drawing.Size(80, 40);
            this.sys3Label5.SubFont = new System.Drawing.Font("맑은 고딕", 10F);
            this.sys3Label5.SubFontColor = System.Drawing.Color.Black;
            this.sys3Label5.SubText = "";
            this.sys3Label5.TabIndex = 21149;
            this.sys3Label5.Text = "CARRIER SLOT STATE";
            this.sys3Label5.TextAlignMain = Sys3Controls.EN_TEXTALIGN.TOP_LEFT;
            this.sys3Label5.TextAlignSub = Sys3Controls.EN_TEXTALIGN.MIDDLE_CENTER;
            this.sys3Label5.TextAlignUnit = Sys3Controls.EN_TEXTALIGN.MIDDLE_CENTER;
            this.sys3Label5.ThemeIndex = 0;
            this.sys3Label5.UnitAreaRate = 40;
            this.sys3Label5.UnitFont = new System.Drawing.Font("맑은 고딕", 10F, System.Drawing.FontStyle.Bold);
            this.sys3Label5.UnitFontColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.sys3Label5.UnitPositionVertical = false;
            this.sys3Label5.UnitText = "";
            this.sys3Label5.UseBorder = true;
            this.sys3Label5.UseEdgeRadius = false;
            this.sys3Label5.UseImage = false;
            this.sys3Label5.UseSubFont = false;
            this.sys3Label5.UseUnitFont = false;
            // 
            // lblSlotMapState
            // 
            this.lblSlotMapState.BackGroundColor = System.Drawing.Color.White;
            this.lblSlotMapState.BorderStroke = 2;
            this.lblSlotMapState.BorderStyle = System.Windows.Forms.ButtonBorderStyle.Solid;
            this.lblSlotMapState.Description = "";
            this.lblSlotMapState.DisabledColor = System.Drawing.Color.LightGray;
            this.lblSlotMapState.EdgeRadius = 1;
            this.lblSlotMapState.ImagePosition = new System.Drawing.Point(0, 0);
            this.lblSlotMapState.ImageSize = new System.Drawing.Point(0, 0);
            this.lblSlotMapState.LoadImage = null;
            this.lblSlotMapState.Location = new System.Drawing.Point(92, 196);
            this.lblSlotMapState.MainFont = new System.Drawing.Font("맑은 고딕", 11F, System.Drawing.FontStyle.Bold);
            this.lblSlotMapState.MainFontColor = System.Drawing.Color.Black;
            this.lblSlotMapState.Margin = new System.Windows.Forms.Padding(1);
            this.lblSlotMapState.Name = "lblSlotMapState";
            this.lblSlotMapState.Size = new System.Drawing.Size(179, 40);
            this.lblSlotMapState.SubFont = new System.Drawing.Font("맑은 고딕", 10F);
            this.lblSlotMapState.SubFontColor = System.Drawing.Color.Black;
            this.lblSlotMapState.SubText = "";
            this.lblSlotMapState.TabIndex = 21148;
            this.lblSlotMapState.Text = "--";
            this.lblSlotMapState.TextAlignMain = Sys3Controls.EN_TEXTALIGN.BOTTOM_RIGHT;
            this.lblSlotMapState.TextAlignSub = Sys3Controls.EN_TEXTALIGN.MIDDLE_CENTER;
            this.lblSlotMapState.TextAlignUnit = Sys3Controls.EN_TEXTALIGN.MIDDLE_CENTER;
            this.lblSlotMapState.ThemeIndex = 0;
            this.lblSlotMapState.UnitAreaRate = 40;
            this.lblSlotMapState.UnitFont = new System.Drawing.Font("맑은 고딕", 10F, System.Drawing.FontStyle.Bold);
            this.lblSlotMapState.UnitFontColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.lblSlotMapState.UnitPositionVertical = false;
            this.lblSlotMapState.UnitText = "";
            this.lblSlotMapState.UseBorder = true;
            this.lblSlotMapState.UseEdgeRadius = false;
            this.lblSlotMapState.UseImage = false;
            this.lblSlotMapState.UseSubFont = false;
            this.lblSlotMapState.UseUnitFont = false;
            // 
            // btnUnloadButton
            // 
            this.btnUnloadButton.BorderWidth = 3;
            this.btnUnloadButton.ButtonClicked = false;
            this.btnUnloadButton.ClickedEmphasizeTextColor = System.Drawing.Color.White;
            this.btnUnloadButton.CustomClickedGradientFirstColor = System.Drawing.Color.White;
            this.btnUnloadButton.CustomClickedGradientSecondColor = System.Drawing.Color.White;
            this.btnUnloadButton.Description = "";
            this.btnUnloadButton.DisabledColor = System.Drawing.Color.DarkGray;
            this.btnUnloadButton.EdgeRadius = 1;
            this.btnUnloadButton.GradientAngle = 90F;
            this.btnUnloadButton.GradientFirstColor = System.Drawing.Color.DimGray;
            this.btnUnloadButton.GradientSecondColor = System.Drawing.Color.DarkGray;
            this.btnUnloadButton.HoverEmphasizeCustomColor = System.Drawing.Color.White;
            this.btnUnloadButton.ImagePosition = new System.Drawing.Point(78, 10);
            this.btnUnloadButton.ImageSize = new System.Drawing.Point(30, 30);
            this.btnUnloadButton.LoadImage = null;
            this.btnUnloadButton.Location = new System.Drawing.Point(144, 240);
            this.btnUnloadButton.MainFont = new System.Drawing.Font("맑은 고딕", 11F, System.Drawing.FontStyle.Bold);
            this.btnUnloadButton.MainFontColor = System.Drawing.Color.White;
            this.btnUnloadButton.Name = "btnUnloadButton";
            this.btnUnloadButton.Size = new System.Drawing.Size(127, 53);
            this.btnUnloadButton.SubFont = new System.Drawing.Font("맑은 고딕", 10F);
            this.btnUnloadButton.SubFontColor = System.Drawing.Color.Black;
            this.btnUnloadButton.SubText = "";
            this.btnUnloadButton.TabIndex = 21151;
            this.btnUnloadButton.Text = "Unload Carrier";
            this.btnUnloadButton.TextAlignMain = Sys3Controls.EN_TEXTALIGN.MIDDLE_CENTER;
            this.btnUnloadButton.TextAlignSub = Sys3Controls.EN_TEXTALIGN.MIDDLE_CENTER;
            this.btnUnloadButton.ThemeIndex = 0;
            this.btnUnloadButton.UseBorder = true;
            this.btnUnloadButton.UseClickedEmphasizeTextColor = false;
            this.btnUnloadButton.UseCustomizeClickedColor = false;
            this.btnUnloadButton.UseEdge = true;
            this.btnUnloadButton.UseHoverEmphasizeCustomColor = false;
            this.btnUnloadButton.UseImage = false;
            this.btnUnloadButton.UserHoverEmpahsize = false;
            this.btnUnloadButton.UseSubFont = false;
            this.btnUnloadButton.Click += new System.EventHandler(this.BtnMechanicalButtonClicked);
            // 
            // btnLoadButton
            // 
            this.btnLoadButton.BorderWidth = 3;
            this.btnLoadButton.ButtonClicked = false;
            this.btnLoadButton.ClickedEmphasizeTextColor = System.Drawing.Color.White;
            this.btnLoadButton.CustomClickedGradientFirstColor = System.Drawing.Color.White;
            this.btnLoadButton.CustomClickedGradientSecondColor = System.Drawing.Color.White;
            this.btnLoadButton.Description = "";
            this.btnLoadButton.DisabledColor = System.Drawing.Color.DarkGray;
            this.btnLoadButton.EdgeRadius = 1;
            this.btnLoadButton.GradientAngle = 90F;
            this.btnLoadButton.GradientFirstColor = System.Drawing.Color.DimGray;
            this.btnLoadButton.GradientSecondColor = System.Drawing.Color.DarkGray;
            this.btnLoadButton.HoverEmphasizeCustomColor = System.Drawing.Color.White;
            this.btnLoadButton.ImagePosition = new System.Drawing.Point(78, 10);
            this.btnLoadButton.ImageSize = new System.Drawing.Point(30, 30);
            this.btnLoadButton.LoadImage = null;
            this.btnLoadButton.Location = new System.Drawing.Point(10, 240);
            this.btnLoadButton.MainFont = new System.Drawing.Font("맑은 고딕", 11F, System.Drawing.FontStyle.Bold);
            this.btnLoadButton.MainFontColor = System.Drawing.Color.White;
            this.btnLoadButton.Name = "btnLoadButton";
            this.btnLoadButton.Size = new System.Drawing.Size(127, 53);
            this.btnLoadButton.SubFont = new System.Drawing.Font("맑은 고딕", 10F);
            this.btnLoadButton.SubFontColor = System.Drawing.Color.Black;
            this.btnLoadButton.SubText = "";
            this.btnLoadButton.TabIndex = 21150;
            this.btnLoadButton.Text = "Load Carrier";
            this.btnLoadButton.TextAlignMain = Sys3Controls.EN_TEXTALIGN.MIDDLE_CENTER;
            this.btnLoadButton.TextAlignSub = Sys3Controls.EN_TEXTALIGN.MIDDLE_CENTER;
            this.btnLoadButton.ThemeIndex = 0;
            this.btnLoadButton.UseBorder = true;
            this.btnLoadButton.UseClickedEmphasizeTextColor = false;
            this.btnLoadButton.UseCustomizeClickedColor = false;
            this.btnLoadButton.UseEdge = true;
            this.btnLoadButton.UseHoverEmphasizeCustomColor = false;
            this.btnLoadButton.UseImage = false;
            this.btnLoadButton.UserHoverEmpahsize = false;
            this.btnLoadButton.UseSubFont = false;
            this.btnLoadButton.Click += new System.EventHandler(this.BtnMechanicalButtonClicked);
            // 
            // btnLoadCarrierAll
            // 
            this.btnLoadCarrierAll.BorderWidth = 3;
            this.btnLoadCarrierAll.ButtonClicked = false;
            this.btnLoadCarrierAll.ClickedEmphasizeTextColor = System.Drawing.Color.White;
            this.btnLoadCarrierAll.CustomClickedGradientFirstColor = System.Drawing.Color.White;
            this.btnLoadCarrierAll.CustomClickedGradientSecondColor = System.Drawing.Color.White;
            this.btnLoadCarrierAll.Description = "";
            this.btnLoadCarrierAll.DisabledColor = System.Drawing.Color.DarkGray;
            this.btnLoadCarrierAll.EdgeRadius = 1;
            this.btnLoadCarrierAll.GradientAngle = 90F;
            this.btnLoadCarrierAll.GradientFirstColor = System.Drawing.Color.DimGray;
            this.btnLoadCarrierAll.GradientSecondColor = System.Drawing.Color.DarkGray;
            this.btnLoadCarrierAll.HoverEmphasizeCustomColor = System.Drawing.Color.White;
            this.btnLoadCarrierAll.ImagePosition = new System.Drawing.Point(78, 10);
            this.btnLoadCarrierAll.ImageSize = new System.Drawing.Point(30, 30);
            this.btnLoadCarrierAll.LoadImage = null;
            this.btnLoadCarrierAll.Location = new System.Drawing.Point(10, 299);
            this.btnLoadCarrierAll.MainFont = new System.Drawing.Font("맑은 고딕", 11F, System.Drawing.FontStyle.Bold);
            this.btnLoadCarrierAll.MainFontColor = System.Drawing.Color.White;
            this.btnLoadCarrierAll.Name = "btnLoadCarrierAll";
            this.btnLoadCarrierAll.Size = new System.Drawing.Size(127, 53);
            this.btnLoadCarrierAll.SubFont = new System.Drawing.Font("맑은 고딕", 10F);
            this.btnLoadCarrierAll.SubFontColor = System.Drawing.Color.Black;
            this.btnLoadCarrierAll.SubText = "";
            this.btnLoadCarrierAll.TabIndex = 21152;
            this.btnLoadCarrierAll.Text = "Load Carrier All";
            this.btnLoadCarrierAll.TextAlignMain = Sys3Controls.EN_TEXTALIGN.MIDDLE_CENTER;
            this.btnLoadCarrierAll.TextAlignSub = Sys3Controls.EN_TEXTALIGN.MIDDLE_CENTER;
            this.btnLoadCarrierAll.ThemeIndex = 0;
            this.btnLoadCarrierAll.UseBorder = true;
            this.btnLoadCarrierAll.UseClickedEmphasizeTextColor = false;
            this.btnLoadCarrierAll.UseCustomizeClickedColor = false;
            this.btnLoadCarrierAll.UseEdge = true;
            this.btnLoadCarrierAll.UseHoverEmphasizeCustomColor = false;
            this.btnLoadCarrierAll.UseImage = false;
            this.btnLoadCarrierAll.UserHoverEmpahsize = false;
            this.btnLoadCarrierAll.UseSubFont = false;
            this.btnLoadCarrierAll.Click += new System.EventHandler(this.BtnMechanicalButtonClicked);
            // 
            // btnRemoveCarrierAll
            // 
            this.btnRemoveCarrierAll.BorderWidth = 3;
            this.btnRemoveCarrierAll.ButtonClicked = false;
            this.btnRemoveCarrierAll.ClickedEmphasizeTextColor = System.Drawing.Color.White;
            this.btnRemoveCarrierAll.CustomClickedGradientFirstColor = System.Drawing.Color.White;
            this.btnRemoveCarrierAll.CustomClickedGradientSecondColor = System.Drawing.Color.White;
            this.btnRemoveCarrierAll.Description = "";
            this.btnRemoveCarrierAll.DisabledColor = System.Drawing.Color.DarkGray;
            this.btnRemoveCarrierAll.EdgeRadius = 1;
            this.btnRemoveCarrierAll.GradientAngle = 90F;
            this.btnRemoveCarrierAll.GradientFirstColor = System.Drawing.Color.DimGray;
            this.btnRemoveCarrierAll.GradientSecondColor = System.Drawing.Color.DarkGray;
            this.btnRemoveCarrierAll.HoverEmphasizeCustomColor = System.Drawing.Color.White;
            this.btnRemoveCarrierAll.ImagePosition = new System.Drawing.Point(78, 10);
            this.btnRemoveCarrierAll.ImageSize = new System.Drawing.Point(30, 30);
            this.btnRemoveCarrierAll.LoadImage = null;
            this.btnRemoveCarrierAll.Location = new System.Drawing.Point(277, 55);
            this.btnRemoveCarrierAll.MainFont = new System.Drawing.Font("맑은 고딕", 11F, System.Drawing.FontStyle.Bold);
            this.btnRemoveCarrierAll.MainFontColor = System.Drawing.Color.White;
            this.btnRemoveCarrierAll.Name = "btnRemoveCarrierAll";
            this.btnRemoveCarrierAll.Size = new System.Drawing.Size(127, 53);
            this.btnRemoveCarrierAll.SubFont = new System.Drawing.Font("맑은 고딕", 10F);
            this.btnRemoveCarrierAll.SubFontColor = System.Drawing.Color.Black;
            this.btnRemoveCarrierAll.SubText = "";
            this.btnRemoveCarrierAll.TabIndex = 21153;
            this.btnRemoveCarrierAll.Text = "Remove Carrier All";
            this.btnRemoveCarrierAll.TextAlignMain = Sys3Controls.EN_TEXTALIGN.MIDDLE_CENTER;
            this.btnRemoveCarrierAll.TextAlignSub = Sys3Controls.EN_TEXTALIGN.MIDDLE_CENTER;
            this.btnRemoveCarrierAll.ThemeIndex = 0;
            this.btnRemoveCarrierAll.UseBorder = true;
            this.btnRemoveCarrierAll.UseClickedEmphasizeTextColor = false;
            this.btnRemoveCarrierAll.UseCustomizeClickedColor = false;
            this.btnRemoveCarrierAll.UseEdge = true;
            this.btnRemoveCarrierAll.UseHoverEmphasizeCustomColor = false;
            this.btnRemoveCarrierAll.UseImage = false;
            this.btnRemoveCarrierAll.UserHoverEmpahsize = false;
            this.btnRemoveCarrierAll.UseSubFont = false;
            this.btnRemoveCarrierAll.Click += new System.EventHandler(this.BtnCarrierPresenceClicked);
            // 
            // btnUnloadCarrierAll
            // 
            this.btnUnloadCarrierAll.BorderWidth = 3;
            this.btnUnloadCarrierAll.ButtonClicked = false;
            this.btnUnloadCarrierAll.ClickedEmphasizeTextColor = System.Drawing.Color.White;
            this.btnUnloadCarrierAll.CustomClickedGradientFirstColor = System.Drawing.Color.White;
            this.btnUnloadCarrierAll.CustomClickedGradientSecondColor = System.Drawing.Color.White;
            this.btnUnloadCarrierAll.Description = "";
            this.btnUnloadCarrierAll.DisabledColor = System.Drawing.Color.DarkGray;
            this.btnUnloadCarrierAll.EdgeRadius = 1;
            this.btnUnloadCarrierAll.GradientAngle = 90F;
            this.btnUnloadCarrierAll.GradientFirstColor = System.Drawing.Color.DimGray;
            this.btnUnloadCarrierAll.GradientSecondColor = System.Drawing.Color.DarkGray;
            this.btnUnloadCarrierAll.HoverEmphasizeCustomColor = System.Drawing.Color.White;
            this.btnUnloadCarrierAll.ImagePosition = new System.Drawing.Point(78, 10);
            this.btnUnloadCarrierAll.ImageSize = new System.Drawing.Point(30, 30);
            this.btnUnloadCarrierAll.LoadImage = null;
            this.btnUnloadCarrierAll.Location = new System.Drawing.Point(144, 299);
            this.btnUnloadCarrierAll.MainFont = new System.Drawing.Font("맑은 고딕", 11F, System.Drawing.FontStyle.Bold);
            this.btnUnloadCarrierAll.MainFontColor = System.Drawing.Color.White;
            this.btnUnloadCarrierAll.Name = "btnUnloadCarrierAll";
            this.btnUnloadCarrierAll.Size = new System.Drawing.Size(127, 53);
            this.btnUnloadCarrierAll.SubFont = new System.Drawing.Font("맑은 고딕", 10F);
            this.btnUnloadCarrierAll.SubFontColor = System.Drawing.Color.Black;
            this.btnUnloadCarrierAll.SubText = "";
            this.btnUnloadCarrierAll.TabIndex = 21154;
            this.btnUnloadCarrierAll.Text = "Unload Carrier All";
            this.btnUnloadCarrierAll.TextAlignMain = Sys3Controls.EN_TEXTALIGN.MIDDLE_CENTER;
            this.btnUnloadCarrierAll.TextAlignSub = Sys3Controls.EN_TEXTALIGN.MIDDLE_CENTER;
            this.btnUnloadCarrierAll.ThemeIndex = 0;
            this.btnUnloadCarrierAll.UseBorder = true;
            this.btnUnloadCarrierAll.UseClickedEmphasizeTextColor = false;
            this.btnUnloadCarrierAll.UseCustomizeClickedColor = false;
            this.btnUnloadCarrierAll.UseEdge = true;
            this.btnUnloadCarrierAll.UseHoverEmphasizeCustomColor = false;
            this.btnUnloadCarrierAll.UseImage = false;
            this.btnUnloadCarrierAll.UserHoverEmpahsize = false;
            this.btnUnloadCarrierAll.UseSubFont = false;
            this.btnUnloadCarrierAll.Click += new System.EventHandler(this.BtnMechanicalButtonClicked);
            // 
            // btnClearLoadPortInfo
            // 
            this.btnClearLoadPortInfo.BorderWidth = 3;
            this.btnClearLoadPortInfo.ButtonClicked = false;
            this.btnClearLoadPortInfo.ClickedEmphasizeTextColor = System.Drawing.Color.White;
            this.btnClearLoadPortInfo.CustomClickedGradientFirstColor = System.Drawing.Color.White;
            this.btnClearLoadPortInfo.CustomClickedGradientSecondColor = System.Drawing.Color.White;
            this.btnClearLoadPortInfo.Description = "";
            this.btnClearLoadPortInfo.DisabledColor = System.Drawing.Color.DarkGray;
            this.btnClearLoadPortInfo.EdgeRadius = 1;
            this.btnClearLoadPortInfo.GradientAngle = 90F;
            this.btnClearLoadPortInfo.GradientFirstColor = System.Drawing.Color.DimGray;
            this.btnClearLoadPortInfo.GradientSecondColor = System.Drawing.Color.DarkGray;
            this.btnClearLoadPortInfo.HoverEmphasizeCustomColor = System.Drawing.Color.White;
            this.btnClearLoadPortInfo.ImagePosition = new System.Drawing.Point(78, 10);
            this.btnClearLoadPortInfo.ImageSize = new System.Drawing.Point(30, 30);
            this.btnClearLoadPortInfo.LoadImage = null;
            this.btnClearLoadPortInfo.Location = new System.Drawing.Point(504, 55);
            this.btnClearLoadPortInfo.MainFont = new System.Drawing.Font("맑은 고딕", 11F, System.Drawing.FontStyle.Bold);
            this.btnClearLoadPortInfo.MainFontColor = System.Drawing.Color.White;
            this.btnClearLoadPortInfo.Name = "btnClearLoadPortInfo";
            this.btnClearLoadPortInfo.Size = new System.Drawing.Size(127, 53);
            this.btnClearLoadPortInfo.SubFont = new System.Drawing.Font("맑은 고딕", 10F);
            this.btnClearLoadPortInfo.SubFontColor = System.Drawing.Color.Black;
            this.btnClearLoadPortInfo.SubText = "";
            this.btnClearLoadPortInfo.TabIndex = 21155;
            this.btnClearLoadPortInfo.Text = "Clear Info At LoadPort";
            this.btnClearLoadPortInfo.TextAlignMain = Sys3Controls.EN_TEXTALIGN.MIDDLE_CENTER;
            this.btnClearLoadPortInfo.TextAlignSub = Sys3Controls.EN_TEXTALIGN.MIDDLE_CENTER;
            this.btnClearLoadPortInfo.ThemeIndex = 0;
            this.btnClearLoadPortInfo.UseBorder = true;
            this.btnClearLoadPortInfo.UseClickedEmphasizeTextColor = false;
            this.btnClearLoadPortInfo.UseCustomizeClickedColor = false;
            this.btnClearLoadPortInfo.UseEdge = true;
            this.btnClearLoadPortInfo.UseHoverEmphasizeCustomColor = false;
            this.btnClearLoadPortInfo.UseImage = false;
            this.btnClearLoadPortInfo.UserHoverEmpahsize = false;
            this.btnClearLoadPortInfo.UseSubFont = false;
            this.btnClearLoadPortInfo.Click += new System.EventHandler(this.ClearInfos);
            // 
            // btnClearPMInfo
            // 
            this.btnClearPMInfo.BorderWidth = 3;
            this.btnClearPMInfo.ButtonClicked = false;
            this.btnClearPMInfo.ClickedEmphasizeTextColor = System.Drawing.Color.White;
            this.btnClearPMInfo.CustomClickedGradientFirstColor = System.Drawing.Color.White;
            this.btnClearPMInfo.CustomClickedGradientSecondColor = System.Drawing.Color.White;
            this.btnClearPMInfo.Description = "";
            this.btnClearPMInfo.DisabledColor = System.Drawing.Color.DarkGray;
            this.btnClearPMInfo.EdgeRadius = 1;
            this.btnClearPMInfo.GradientAngle = 90F;
            this.btnClearPMInfo.GradientFirstColor = System.Drawing.Color.DimGray;
            this.btnClearPMInfo.GradientSecondColor = System.Drawing.Color.DarkGray;
            this.btnClearPMInfo.HoverEmphasizeCustomColor = System.Drawing.Color.White;
            this.btnClearPMInfo.ImagePosition = new System.Drawing.Point(78, 10);
            this.btnClearPMInfo.ImageSize = new System.Drawing.Point(30, 30);
            this.btnClearPMInfo.LoadImage = null;
            this.btnClearPMInfo.Location = new System.Drawing.Point(504, 112);
            this.btnClearPMInfo.MainFont = new System.Drawing.Font("맑은 고딕", 11F, System.Drawing.FontStyle.Bold);
            this.btnClearPMInfo.MainFontColor = System.Drawing.Color.White;
            this.btnClearPMInfo.Name = "btnClearPMInfo";
            this.btnClearPMInfo.Size = new System.Drawing.Size(127, 53);
            this.btnClearPMInfo.SubFont = new System.Drawing.Font("맑은 고딕", 10F);
            this.btnClearPMInfo.SubFontColor = System.Drawing.Color.Black;
            this.btnClearPMInfo.SubText = "";
            this.btnClearPMInfo.TabIndex = 21156;
            this.btnClearPMInfo.Text = "Clear Info At PM";
            this.btnClearPMInfo.TextAlignMain = Sys3Controls.EN_TEXTALIGN.MIDDLE_CENTER;
            this.btnClearPMInfo.TextAlignSub = Sys3Controls.EN_TEXTALIGN.MIDDLE_CENTER;
            this.btnClearPMInfo.ThemeIndex = 0;
            this.btnClearPMInfo.UseBorder = true;
            this.btnClearPMInfo.UseClickedEmphasizeTextColor = false;
            this.btnClearPMInfo.UseCustomizeClickedColor = false;
            this.btnClearPMInfo.UseEdge = true;
            this.btnClearPMInfo.UseHoverEmphasizeCustomColor = false;
            this.btnClearPMInfo.UseImage = false;
            this.btnClearPMInfo.UserHoverEmpahsize = false;
            this.btnClearPMInfo.UseSubFont = false;
            this.btnClearPMInfo.Click += new System.EventHandler(this.ClearInfos);
            // 
            // btnClearRobotInfo
            // 
            this.btnClearRobotInfo.BorderWidth = 3;
            this.btnClearRobotInfo.ButtonClicked = false;
            this.btnClearRobotInfo.ClickedEmphasizeTextColor = System.Drawing.Color.White;
            this.btnClearRobotInfo.CustomClickedGradientFirstColor = System.Drawing.Color.White;
            this.btnClearRobotInfo.CustomClickedGradientSecondColor = System.Drawing.Color.White;
            this.btnClearRobotInfo.Description = "";
            this.btnClearRobotInfo.DisabledColor = System.Drawing.Color.DarkGray;
            this.btnClearRobotInfo.EdgeRadius = 1;
            this.btnClearRobotInfo.GradientAngle = 90F;
            this.btnClearRobotInfo.GradientFirstColor = System.Drawing.Color.DimGray;
            this.btnClearRobotInfo.GradientSecondColor = System.Drawing.Color.DarkGray;
            this.btnClearRobotInfo.HoverEmphasizeCustomColor = System.Drawing.Color.White;
            this.btnClearRobotInfo.ImagePosition = new System.Drawing.Point(78, 10);
            this.btnClearRobotInfo.ImageSize = new System.Drawing.Point(30, 30);
            this.btnClearRobotInfo.LoadImage = null;
            this.btnClearRobotInfo.Location = new System.Drawing.Point(504, 171);
            this.btnClearRobotInfo.MainFont = new System.Drawing.Font("맑은 고딕", 11F, System.Drawing.FontStyle.Bold);
            this.btnClearRobotInfo.MainFontColor = System.Drawing.Color.White;
            this.btnClearRobotInfo.Name = "btnClearRobotInfo";
            this.btnClearRobotInfo.Size = new System.Drawing.Size(127, 53);
            this.btnClearRobotInfo.SubFont = new System.Drawing.Font("맑은 고딕", 10F);
            this.btnClearRobotInfo.SubFontColor = System.Drawing.Color.Black;
            this.btnClearRobotInfo.SubText = "";
            this.btnClearRobotInfo.TabIndex = 21157;
            this.btnClearRobotInfo.Text = "Clear Info At Arms";
            this.btnClearRobotInfo.TextAlignMain = Sys3Controls.EN_TEXTALIGN.MIDDLE_CENTER;
            this.btnClearRobotInfo.TextAlignSub = Sys3Controls.EN_TEXTALIGN.MIDDLE_CENTER;
            this.btnClearRobotInfo.ThemeIndex = 0;
            this.btnClearRobotInfo.UseBorder = true;
            this.btnClearRobotInfo.UseClickedEmphasizeTextColor = false;
            this.btnClearRobotInfo.UseCustomizeClickedColor = false;
            this.btnClearRobotInfo.UseEdge = true;
            this.btnClearRobotInfo.UseHoverEmphasizeCustomColor = false;
            this.btnClearRobotInfo.UseImage = false;
            this.btnClearRobotInfo.UserHoverEmpahsize = false;
            this.btnClearRobotInfo.UseSubFont = false;
            this.btnClearRobotInfo.Click += new System.EventHandler(this.ClearInfos);
            // 
            // EFEMSimulator_LoarPort
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.Controls.Add(this.btnClearRobotInfo);
            this.Controls.Add(this.btnClearPMInfo);
            this.Controls.Add(this.btnClearLoadPortInfo);
            this.Controls.Add(this.btnUnloadCarrierAll);
            this.Controls.Add(this.btnRemoveCarrierAll);
            this.Controls.Add(this.btnLoadCarrierAll);
            this.Controls.Add(this.btnUnloadButton);
            this.Controls.Add(this.btnLoadButton);
            this.Controls.Add(this.sys3Label5);
            this.Controls.Add(this.lblSlotMapState);
            this.Controls.Add(this.sys3Label3);
            this.Controls.Add(this.lblIdState);
            this.Controls.Add(this.sys3Label1);
            this.Controls.Add(this.lblTransferState);
            this.Controls.Add(this.lblLoadPort);
            this.Controls.Add(this.lblLoadPortSelection);
            this.Controls.Add(this.btnCarrierRemoved);
            this.Controls.Add(this.btnCarrierPlaced);
            this.Name = "EFEMSimulator_LoarPort";
            this.Size = new System.Drawing.Size(643, 409);
            this.ResumeLayout(false);

        }

        #endregion

        private Sys3Controls.Sys3button btnCarrierRemoved;
        private Sys3Controls.Sys3button btnCarrierPlaced;
        private Sys3Controls.Sys3Label lblLoadPort;
        private Sys3Controls.Sys3Label lblLoadPortSelection;
        private Sys3Controls.Sys3Label sys3Label1;
        private Sys3Controls.Sys3Label lblTransferState;
        private Sys3Controls.Sys3Label sys3Label3;
        private Sys3Controls.Sys3Label lblIdState;
        private Sys3Controls.Sys3Label sys3Label5;
        private Sys3Controls.Sys3Label lblSlotMapState;
        private Sys3Controls.Sys3button btnUnloadButton;
        private Sys3Controls.Sys3button btnLoadButton;
        private Sys3Controls.Sys3button btnLoadCarrierAll;
        private Sys3Controls.Sys3button btnRemoveCarrierAll;
        private Sys3Controls.Sys3button btnUnloadCarrierAll;
        private Sys3Controls.Sys3button btnClearLoadPortInfo;
        private Sys3Controls.Sys3button btnClearPMInfo;
        private Sys3Controls.Sys3button btnClearRobotInfo;
    }
}
