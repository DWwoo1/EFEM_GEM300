
namespace FrameOfSystem3.Views.EFEM_Simulator
{
    partial class EFEMSimulator_AtmRobot
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
            this.btnPlaceWafer = new Sys3Controls.Sys3button();
            this.btnPickWafer = new Sys3Controls.Sys3button();
            this.SuspendLayout();
            // 
            // btnPlaceWafer
            // 
            this.btnPlaceWafer.BorderWidth = 3;
            this.btnPlaceWafer.ButtonClicked = false;
            this.btnPlaceWafer.ClickedEmphasizeTextColor = System.Drawing.Color.White;
            this.btnPlaceWafer.CustomClickedGradientFirstColor = System.Drawing.Color.White;
            this.btnPlaceWafer.CustomClickedGradientSecondColor = System.Drawing.Color.White;
            this.btnPlaceWafer.Description = "";
            this.btnPlaceWafer.DisabledColor = System.Drawing.Color.DarkGray;
            this.btnPlaceWafer.EdgeRadius = 1;
            this.btnPlaceWafer.GradientAngle = 90F;
            this.btnPlaceWafer.GradientFirstColor = System.Drawing.Color.DimGray;
            this.btnPlaceWafer.GradientSecondColor = System.Drawing.Color.DarkGray;
            this.btnPlaceWafer.HoverEmphasizeCustomColor = System.Drawing.Color.White;
            this.btnPlaceWafer.ImagePosition = new System.Drawing.Point(78, 10);
            this.btnPlaceWafer.ImageSize = new System.Drawing.Point(30, 30);
            this.btnPlaceWafer.LoadImage = null;
            this.btnPlaceWafer.Location = new System.Drawing.Point(480, 54);
            this.btnPlaceWafer.MainFont = new System.Drawing.Font("맑은 고딕", 11F, System.Drawing.FontStyle.Bold);
            this.btnPlaceWafer.MainFontColor = System.Drawing.Color.White;
            this.btnPlaceWafer.Name = "btnPlaceWafer";
            this.btnPlaceWafer.Size = new System.Drawing.Size(127, 53);
            this.btnPlaceWafer.SubFont = new System.Drawing.Font("맑은 고딕", 10F);
            this.btnPlaceWafer.SubFontColor = System.Drawing.Color.Black;
            this.btnPlaceWafer.SubText = "";
            this.btnPlaceWafer.TabIndex = 21151;
            this.btnPlaceWafer.Text = "Place Wafer";
            this.btnPlaceWafer.TextAlignMain = Sys3Controls.EN_TEXTALIGN.MIDDLE_CENTER;
            this.btnPlaceWafer.TextAlignSub = Sys3Controls.EN_TEXTALIGN.MIDDLE_CENTER;
            this.btnPlaceWafer.ThemeIndex = 0;
            this.btnPlaceWafer.UseBorder = true;
            this.btnPlaceWafer.UseClickedEmphasizeTextColor = false;
            this.btnPlaceWafer.UseCustomizeClickedColor = false;
            this.btnPlaceWafer.UseEdge = true;
            this.btnPlaceWafer.UseHoverEmphasizeCustomColor = false;
            this.btnPlaceWafer.UseImage = false;
            this.btnPlaceWafer.UserHoverEmpahsize = false;
            this.btnPlaceWafer.UseSubFont = false;
            this.btnPlaceWafer.Click += new System.EventHandler(this.BtnPlaceWaferClicked);
            // 
            // btnPickWafer
            // 
            this.btnPickWafer.BorderWidth = 3;
            this.btnPickWafer.ButtonClicked = false;
            this.btnPickWafer.ClickedEmphasizeTextColor = System.Drawing.Color.White;
            this.btnPickWafer.CustomClickedGradientFirstColor = System.Drawing.Color.White;
            this.btnPickWafer.CustomClickedGradientSecondColor = System.Drawing.Color.White;
            this.btnPickWafer.Description = "";
            this.btnPickWafer.DisabledColor = System.Drawing.Color.DarkGray;
            this.btnPickWafer.EdgeRadius = 1;
            this.btnPickWafer.GradientAngle = 90F;
            this.btnPickWafer.GradientFirstColor = System.Drawing.Color.DimGray;
            this.btnPickWafer.GradientSecondColor = System.Drawing.Color.DarkGray;
            this.btnPickWafer.HoverEmphasizeCustomColor = System.Drawing.Color.White;
            this.btnPickWafer.ImagePosition = new System.Drawing.Point(78, 10);
            this.btnPickWafer.ImageSize = new System.Drawing.Point(30, 30);
            this.btnPickWafer.LoadImage = null;
            this.btnPickWafer.Location = new System.Drawing.Point(316, 54);
            this.btnPickWafer.MainFont = new System.Drawing.Font("맑은 고딕", 11F, System.Drawing.FontStyle.Bold);
            this.btnPickWafer.MainFontColor = System.Drawing.Color.White;
            this.btnPickWafer.Name = "btnPickWafer";
            this.btnPickWafer.Size = new System.Drawing.Size(127, 53);
            this.btnPickWafer.SubFont = new System.Drawing.Font("맑은 고딕", 10F);
            this.btnPickWafer.SubFontColor = System.Drawing.Color.Black;
            this.btnPickWafer.SubText = "";
            this.btnPickWafer.TabIndex = 21150;
            this.btnPickWafer.Text = "Pick Wafer";
            this.btnPickWafer.TextAlignMain = Sys3Controls.EN_TEXTALIGN.MIDDLE_CENTER;
            this.btnPickWafer.TextAlignSub = Sys3Controls.EN_TEXTALIGN.MIDDLE_CENTER;
            this.btnPickWafer.ThemeIndex = 0;
            this.btnPickWafer.UseBorder = true;
            this.btnPickWafer.UseClickedEmphasizeTextColor = false;
            this.btnPickWafer.UseCustomizeClickedColor = false;
            this.btnPickWafer.UseEdge = true;
            this.btnPickWafer.UseHoverEmphasizeCustomColor = false;
            this.btnPickWafer.UseImage = false;
            this.btnPickWafer.UserHoverEmpahsize = false;
            this.btnPickWafer.UseSubFont = false;
            this.btnPickWafer.Click += new System.EventHandler(this.BtnPickWaferClicked);
            // 
            // EFEMSimulator_AtmRobot
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.Controls.Add(this.btnPlaceWafer);
            this.Controls.Add(this.btnPickWafer);
            this.Name = "EFEMSimulator_AtmRobot";
            this.Size = new System.Drawing.Size(643, 409);
            this.ResumeLayout(false);

        }

        #endregion
        private Sys3Controls.Sys3button btnPlaceWafer;
        private Sys3Controls.Sys3button btnPickWafer;
    }
}
