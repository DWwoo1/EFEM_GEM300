namespace FrameOfSystem3.Views.Functional
{
    partial class Form_MessageBox
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
            this._panel_Button = new System.Windows.Forms.TableLayoutPanel();
            this.m_btnOK = new Sys3Controls.Sys3button();
            this.m_btnCancel = new Sys3Controls.Sys3button();
            this.m_TitleBar = new Sys3Controls.Sys3GroupBoxContainer();
            this._panel_Main = new System.Windows.Forms.TableLayoutPanel();
            this._panel_Message = new System.Windows.Forms.Panel();
            this.m_labelMessage = new Sys3Controls.Sys3Label();
            this._panel_Button.SuspendLayout();
            this.m_TitleBar.SuspendLayout();
            this._panel_Main.SuspendLayout();
            this._panel_Message.SuspendLayout();
            this.SuspendLayout();
            // 
            // _panel_Button
            // 
            this._panel_Button.ColumnCount = 5;
            this._panel_Button.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this._panel_Button.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 136F));
            this._panel_Button.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 5F));
            this._panel_Button.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 136F));
            this._panel_Button.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this._panel_Button.Controls.Add(this.m_btnOK, 1, 1);
            this._panel_Button.Controls.Add(this.m_btnCancel, 3, 1);
            this._panel_Button.Dock = System.Windows.Forms.DockStyle.Fill;
            this._panel_Button.Location = new System.Drawing.Point(3, 97);
            this._panel_Button.Name = "_panel_Button";
            this._panel_Button.Padding = new System.Windows.Forms.Padding(1);
            this._panel_Button.RowCount = 3;
            this._panel_Button.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 5F));
            this._panel_Button.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this._panel_Button.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 5F));
            this._panel_Button.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this._panel_Button.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this._panel_Button.Size = new System.Drawing.Size(538, 57);
            this._panel_Button.TabIndex = 9;
            // 
            // m_btnOK
            // 
            this.m_btnOK.BorderWidth = 3;
            this.m_btnOK.ButtonClicked = false;
            this.m_btnOK.ClickedEmphasizeTextColor = System.Drawing.Color.White;
            this.m_btnOK.CustomClickedGradientFirstColor = System.Drawing.Color.BlanchedAlmond;
            this.m_btnOK.CustomClickedGradientSecondColor = System.Drawing.Color.Gold;
            this.m_btnOK.Description = "";
            this.m_btnOK.DisabledColor = System.Drawing.Color.DarkGray;
            this.m_btnOK.Dock = System.Windows.Forms.DockStyle.Fill;
            this.m_btnOK.EdgeRadius = 5;
            this.m_btnOK.GradientAngle = 70F;
            this.m_btnOK.GradientFirstColor = System.Drawing.Color.White;
            this.m_btnOK.GradientSecondColor = System.Drawing.Color.LightSlateGray;
            this.m_btnOK.HoverEmphasizeCustomColor = System.Drawing.Color.Firebrick;
            this.m_btnOK.ImagePosition = new System.Drawing.Point(10, 10);
            this.m_btnOK.ImageSize = new System.Drawing.Point(30, 30);
            this.m_btnOK.LoadImage = global::FrameOfSystem3.Properties.Resources.CLEAR_BLACK;
            this.m_btnOK.Location = new System.Drawing.Point(133, 9);
            this.m_btnOK.MainFont = new System.Drawing.Font("맑은 고딕", 11F, System.Drawing.FontStyle.Bold);
            this.m_btnOK.MainFontColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(36)))), ((int)(((byte)(0)))));
            this.m_btnOK.Name = "m_btnOK";
            this.m_btnOK.Size = new System.Drawing.Size(130, 39);
            this.m_btnOK.SubFont = new System.Drawing.Font("맑은 고딕", 8F, System.Drawing.FontStyle.Bold);
            this.m_btnOK.SubFontColor = System.Drawing.Color.DarkBlue;
            this.m_btnOK.SubText = "STATUS";
            this.m_btnOK.TabIndex = 0;
            this.m_btnOK.Text = "OK";
            this.m_btnOK.TextAlignMain = Sys3Controls.EN_TEXTALIGN.MIDDLE_CENTER;
            this.m_btnOK.TextAlignSub = Sys3Controls.EN_TEXTALIGN.TOP_LEFT;
            this.m_btnOK.ThemeIndex = 0;
            this.m_btnOK.UseBorder = true;
            this.m_btnOK.UseClickedEmphasizeTextColor = false;
            this.m_btnOK.UseCustomizeClickedColor = true;
            this.m_btnOK.UseEdge = true;
            this.m_btnOK.UseHoverEmphasizeCustomColor = true;
            this.m_btnOK.UseImage = false;
            this.m_btnOK.UserHoverEmpahsize = true;
            this.m_btnOK.UseSubFont = false;
            this.m_btnOK.Click += new System.EventHandler(this.Click_OkorCancel);
            // 
            // m_btnCancel
            // 
            this.m_btnCancel.BorderWidth = 3;
            this.m_btnCancel.ButtonClicked = false;
            this.m_btnCancel.ClickedEmphasizeTextColor = System.Drawing.Color.White;
            this.m_btnCancel.CustomClickedGradientFirstColor = System.Drawing.Color.BlanchedAlmond;
            this.m_btnCancel.CustomClickedGradientSecondColor = System.Drawing.Color.Gold;
            this.m_btnCancel.Description = "";
            this.m_btnCancel.DisabledColor = System.Drawing.Color.DarkGray;
            this.m_btnCancel.EdgeRadius = 5;
            this.m_btnCancel.GradientAngle = 70F;
            this.m_btnCancel.GradientFirstColor = System.Drawing.Color.White;
            this.m_btnCancel.GradientSecondColor = System.Drawing.Color.LightSlateGray;
            this.m_btnCancel.HoverEmphasizeCustomColor = System.Drawing.Color.Firebrick;
            this.m_btnCancel.ImagePosition = new System.Drawing.Point(10, 10);
            this.m_btnCancel.ImageSize = new System.Drawing.Point(30, 30);
            this.m_btnCancel.LoadImage = global::FrameOfSystem3.Properties.Resources.CLEAR_BLACK;
            this.m_btnCancel.Location = new System.Drawing.Point(274, 9);
            this.m_btnCancel.MainFont = new System.Drawing.Font("맑은 고딕", 11F, System.Drawing.FontStyle.Bold);
            this.m_btnCancel.MainFontColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(36)))), ((int)(((byte)(0)))));
            this.m_btnCancel.Name = "m_btnCancel";
            this.m_btnCancel.Size = new System.Drawing.Size(130, 39);
            this.m_btnCancel.SubFont = new System.Drawing.Font("맑은 고딕", 8F, System.Drawing.FontStyle.Bold);
            this.m_btnCancel.SubFontColor = System.Drawing.Color.DarkBlue;
            this.m_btnCancel.SubText = "STATUS";
            this.m_btnCancel.TabIndex = 1;
            this.m_btnCancel.Text = "CANCEL";
            this.m_btnCancel.TextAlignMain = Sys3Controls.EN_TEXTALIGN.MIDDLE_CENTER;
            this.m_btnCancel.TextAlignSub = Sys3Controls.EN_TEXTALIGN.TOP_LEFT;
            this.m_btnCancel.ThemeIndex = 0;
            this.m_btnCancel.UseBorder = true;
            this.m_btnCancel.UseClickedEmphasizeTextColor = false;
            this.m_btnCancel.UseCustomizeClickedColor = true;
            this.m_btnCancel.UseEdge = true;
            this.m_btnCancel.UseHoverEmphasizeCustomColor = true;
            this.m_btnCancel.UseImage = false;
            this.m_btnCancel.UserHoverEmpahsize = true;
            this.m_btnCancel.UseSubFont = false;
            this.m_btnCancel.Click += new System.EventHandler(this.Click_OkorCancel);
            // 
            // m_TitleBar
            // 
            this.m_TitleBar.BackGroundColor = System.Drawing.Color.WhiteSmoke;
            this.m_TitleBar.Controls.Add(this._panel_Main);
            this.m_TitleBar.Dock = System.Windows.Forms.DockStyle.Fill;
            this.m_TitleBar.EdgeBorderStroke = 2;
            this.m_TitleBar.EdgeRadius = 2;
            this.m_TitleBar.LabelFont = new System.Drawing.Font("맑은 고딕", 10F, System.Drawing.FontStyle.Bold);
            this.m_TitleBar.LabelGradientColorFirst = System.Drawing.Color.FromArgb(((int)(((byte)(208)))), ((int)(((byte)(208)))), ((int)(((byte)(208)))));
            this.m_TitleBar.LabelGradientColorSecond = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(240)))), ((int)(((byte)(240)))));
            this.m_TitleBar.LabelHeight = 35;
            this.m_TitleBar.LabelTextColor = System.Drawing.Color.Black;
            this.m_TitleBar.Location = new System.Drawing.Point(0, 0);
            this.m_TitleBar.Margin = new System.Windows.Forms.Padding(0);
            this.m_TitleBar.Name = "m_TitleBar";
            this.m_TitleBar.Size = new System.Drawing.Size(550, 200);
            this.m_TitleBar.TabIndex = 10;
            this.m_TitleBar.TabStop = false;
            this.m_TitleBar.Text = "Confirmation Message";
            this.m_TitleBar.TextAlign = Sys3Controls.EN_TEXTALIGN.MIDDLE_CENTER;
            this.m_TitleBar.ThemeIndex = 0;
            this.m_TitleBar.UseLabelBorder = true;
            this.m_TitleBar.UseTitle = true;
            this.m_TitleBar.MouseDown += new System.Windows.Forms.MouseEventHandler(this.MouseDown_Title);
            this.m_TitleBar.MouseUp += new System.Windows.Forms.MouseEventHandler(this.MouseUp_Title);
            this.m_TitleBar.MouseMove += new System.Windows.Forms.MouseEventHandler(this.MouseMove_Title);
            // 
            // _panel_Main
            // 
            this._panel_Main.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this._panel_Main.ColumnCount = 1;
            this._panel_Main.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this._panel_Main.Controls.Add(this._panel_Message, 0, 0);
            this._panel_Main.Controls.Add(this._panel_Button, 0, 1);
            this._panel_Main.Location = new System.Drawing.Point(3, 37);
            this._panel_Main.Name = "_panel_Main";
            this._panel_Main.RowCount = 2;
            this._panel_Main.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this._panel_Main.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 63F));
            this._panel_Main.Size = new System.Drawing.Size(544, 157);
            this._panel_Main.TabIndex = 11;
            // 
            // _panel_Message
            // 
            this._panel_Message.AutoScroll = true;
            this._panel_Message.Controls.Add(this.m_labelMessage);
            this._panel_Message.Dock = System.Windows.Forms.DockStyle.Fill;
            this._panel_Message.Location = new System.Drawing.Point(0, 0);
            this._panel_Message.Margin = new System.Windows.Forms.Padding(0);
            this._panel_Message.Name = "_panel_Message";
            this._panel_Message.Size = new System.Drawing.Size(544, 94);
            this._panel_Message.TabIndex = 11;
            // 
            // m_labelMessage
            // 
            this.m_labelMessage.BackGroundColor = System.Drawing.Color.Transparent;
            this.m_labelMessage.BorderStroke = 2;
            this.m_labelMessage.BorderStyle = System.Windows.Forms.ButtonBorderStyle.Solid;
            this.m_labelMessage.Description = "";
            this.m_labelMessage.DisabledColor = System.Drawing.Color.DarkGray;
            this.m_labelMessage.Dock = System.Windows.Forms.DockStyle.Fill;
            this.m_labelMessage.EdgeRadius = 1;
            this.m_labelMessage.ImagePosition = new System.Drawing.Point(0, 0);
            this.m_labelMessage.ImageSize = new System.Drawing.Point(0, 0);
            this.m_labelMessage.LoadImage = null;
            this.m_labelMessage.Location = new System.Drawing.Point(0, 0);
            this.m_labelMessage.MainFont = new System.Drawing.Font("맑은 고딕", 12F, System.Drawing.FontStyle.Bold);
            this.m_labelMessage.MainFontColor = System.Drawing.Color.Black;
            this.m_labelMessage.Name = "m_labelMessage";
            this.m_labelMessage.Size = new System.Drawing.Size(544, 94);
            this.m_labelMessage.SubFont = new System.Drawing.Font("맑은 고딕", 10F);
            this.m_labelMessage.SubFontColor = System.Drawing.Color.Black;
            this.m_labelMessage.SubText = "";
            this.m_labelMessage.TabIndex = 7;
            this.m_labelMessage.Text = "This is message form for responding Ok or Cancel.";
            this.m_labelMessage.TextAlignMain = Sys3Controls.EN_TEXTALIGN.MIDDLE_CENTER;
            this.m_labelMessage.TextAlignSub = Sys3Controls.EN_TEXTALIGN.MIDDLE_CENTER;
            this.m_labelMessage.TextAlignUnit = Sys3Controls.EN_TEXTALIGN.MIDDLE_CENTER;
            this.m_labelMessage.ThemeIndex = 0;
            this.m_labelMessage.UnitAreaRate = 40;
            this.m_labelMessage.UnitFont = new System.Drawing.Font("맑은 고딕", 10F, System.Drawing.FontStyle.Bold);
            this.m_labelMessage.UnitFontColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.m_labelMessage.UnitPositionVertical = false;
            this.m_labelMessage.UnitText = "";
            this.m_labelMessage.UseBorder = false;
            this.m_labelMessage.UseEdgeRadius = true;
            this.m_labelMessage.UseImage = false;
            this.m_labelMessage.UseSubFont = false;
            this.m_labelMessage.UseUnitFont = false;
            // 
            // Form_MessageBox
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.BackColor = System.Drawing.Color.WhiteSmoke;
            this.ClientSize = new System.Drawing.Size(550, 200);
            this.ControlBox = false;
            this.Controls.Add(this.m_TitleBar);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.KeyPreview = true;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "Form_MessageBox";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.Text = "`";
            this.TopMost = true;
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.Form_MessageBox_KeyDown);
            this._panel_Button.ResumeLayout(false);
            this.m_TitleBar.ResumeLayout(false);
            this._panel_Main.ResumeLayout(false);
            this._panel_Message.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

		private System.Windows.Forms.TableLayoutPanel _panel_Button;
		private Sys3Controls.Sys3button m_btnOK;
		private Sys3Controls.Sys3button m_btnCancel;
		private Sys3Controls.Sys3GroupBoxContainer m_TitleBar;
		private Sys3Controls.Sys3Label m_labelMessage;
		private System.Windows.Forms.TableLayoutPanel _panel_Main;
		private System.Windows.Forms.Panel _panel_Message;
	}
}