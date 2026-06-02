
namespace FrameOfSystem3.Views.Operation.SubPanelSummary.LoadPortSummary
{
    partial class SummaryLoadPortState_Slot
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
            this.mapLoadPortSlot = new Sys3Controls.Sys3Map();
            this.tableLayoutPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 1;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel1.Controls.Add(this.mapLoadPortSlot, 0, 0);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Margin = new System.Windows.Forms.Padding(0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 1;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(170, 142);
            this.tableLayoutPanel1.TabIndex = 21105;
            // 
            // mapLoadPortSlot
            // 
            this.mapLoadPortSlot.BackColor = System.Drawing.SystemColors.Control;
            this.mapLoadPortSlot.BackGroundColor = System.Drawing.Color.DarkGray;
            this.mapLoadPortSlot.CellPadding = new System.Drawing.Size(0, 0);
            this.mapLoadPortSlot.ClickOnlyOne = false;
            this.mapLoadPortSlot.ColorDisabled = System.Drawing.Color.DimGray;
            this.mapLoadPortSlot.ColorSelected = System.Drawing.Color.Gainsboro;
            this.mapLoadPortSlot.Dock = System.Windows.Forms.DockStyle.Fill;
            this.mapLoadPortSlot.FontRatioToCellHight = 0.5D;
            this.mapLoadPortSlot.GroupInUnitMapSize = new System.Drawing.Size(1, 1);
            this.mapLoadPortSlot.GroupMapSize = new System.Drawing.Size(1, 1);
            this.mapLoadPortSlot.GroupPadding = new System.Drawing.Size(0, 0);
            this.mapLoadPortSlot.Location = new System.Drawing.Point(0, 0);
            this.mapLoadPortSlot.MainFont = new System.Drawing.Font("맑은 고딕", 10F);
            this.mapLoadPortSlot.MainFontColor = System.Drawing.Color.Black;
            this.mapLoadPortSlot.MapSize = new System.Drawing.Size(1, 25);
            this.mapLoadPortSlot.Margin = new System.Windows.Forms.Padding(0);
            this.mapLoadPortSlot.Name = "mapLoadPortSlot";
            this.mapLoadPortSlot.Size = new System.Drawing.Size(170, 142);
            this.mapLoadPortSlot.TabIndex = 21103;
            this.mapLoadPortSlot.Text = "sys3Map1";
            this.mapLoadPortSlot.UseClick = true;
            this.mapLoadPortSlot.UseComplementaryFontColor = false;
            this.mapLoadPortSlot.UseDynamicFontSizeAccordingToCellHeight = false;
            this.mapLoadPortSlot.WorkUnitView = false;
            // 
            // SummaryLoadPortState_Slot
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.Controls.Add(this.tableLayoutPanel1);
            this.Margin = new System.Windows.Forms.Padding(0);
            this.Name = "SummaryLoadPortState_Slot";
            this.Size = new System.Drawing.Size(170, 142);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private Sys3Controls.Sys3Map mapLoadPortSlot;
    }
}
