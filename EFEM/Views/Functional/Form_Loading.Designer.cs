namespace FrameOfSystem3.Views.Functional
{
	partial class Form_Loading
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
			this.m_btnAdd = new Sys3Controls.Sys3button();
			this.m_btnRemove = new Sys3Controls.Sys3button();
			this.SuspendLayout();
			// 
			// m_btnAdd
			// 
			this.m_btnAdd.BorderWidth = 1;
			this.m_btnAdd.ButtonClicked = false;
			this.m_btnAdd.DisabledColor = System.Drawing.Color.DarkGray;
			this.m_btnAdd.EdgeRadius = 5;
			this.m_btnAdd.GradientAngle = 70F;
			this.m_btnAdd.GradientFirstColor = System.Drawing.Color.White;
			this.m_btnAdd.GradientSecondColor = System.Drawing.Color.FromArgb(((int)(((byte)(170)))), ((int)(((byte)(176)))), ((int)(((byte)(183)))));
			this.m_btnAdd.ImagePosition = new System.Drawing.Point(7, 7);
			this.m_btnAdd.ImageSize = new System.Drawing.Point(30, 30);
			this.m_btnAdd.LoadImage = global::FrameOfSystem3.Properties.Resources.CONFIG_ADD3;
			this.m_btnAdd.Location = new System.Drawing.Point(188, 244);
			this.m_btnAdd.MainFont = new System.Drawing.Font("맑은 고딕", 13F, System.Drawing.FontStyle.Bold);
			this.m_btnAdd.MainFontColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(36)))), ((int)(((byte)(0)))));
			this.m_btnAdd.Name = "m_btnAdd";
			this.m_btnAdd.Size = new System.Drawing.Size(126, 77);
			this.m_btnAdd.SubFont = new System.Drawing.Font("맑은 고딕", 8F, System.Drawing.FontStyle.Bold);
			this.m_btnAdd.SubFontColor = System.Drawing.Color.DarkBlue;
			this.m_btnAdd.SubText = "STATUS";
			this.m_btnAdd.TabIndex = 0;
			this.m_btnAdd.Text = "ADD";
			this.m_btnAdd.TextAlignMain = Sys3Controls.EN_TEXTALIGN.MIDDLE_RIGHT;
			this.m_btnAdd.TextAlignSub = Sys3Controls.EN_TEXTALIGN.TOP_LEFT;
			this.m_btnAdd.UseBorder = false;
			this.m_btnAdd.UseEdge = false;
			this.m_btnAdd.UseImage = true;
			this.m_btnAdd.UseSubFont = false;
			this.m_btnAdd.Click += new System.EventHandler(this.Click_AddOrRemove);
			// 
			// m_btnRemove
			// 
			this.m_btnRemove.BorderWidth = 1;
			this.m_btnRemove.ButtonClicked = false;
			this.m_btnRemove.DisabledColor = System.Drawing.Color.DarkGray;
			this.m_btnRemove.EdgeRadius = 5;
			this.m_btnRemove.Enabled = false;
			this.m_btnRemove.GradientAngle = 70F;
			this.m_btnRemove.GradientFirstColor = System.Drawing.Color.White;
			this.m_btnRemove.GradientSecondColor = System.Drawing.Color.FromArgb(((int)(((byte)(170)))), ((int)(((byte)(176)))), ((int)(((byte)(183)))));
			this.m_btnRemove.ImagePosition = new System.Drawing.Point(7, 7);
			this.m_btnRemove.ImageSize = new System.Drawing.Point(30, 30);
			this.m_btnRemove.LoadImage = global::FrameOfSystem3.Properties.Resources.REMOVE;
			this.m_btnRemove.Location = new System.Drawing.Point(312, 244);
			this.m_btnRemove.MainFont = new System.Drawing.Font("맑은 고딕", 13F, System.Drawing.FontStyle.Bold);
			this.m_btnRemove.MainFontColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(36)))), ((int)(((byte)(0)))));
			this.m_btnRemove.Name = "m_btnRemove";
			this.m_btnRemove.Size = new System.Drawing.Size(128, 77);
			this.m_btnRemove.SubFont = new System.Drawing.Font("맑은 고딕", 8F, System.Drawing.FontStyle.Bold);
			this.m_btnRemove.SubFontColor = System.Drawing.Color.DarkBlue;
			this.m_btnRemove.SubText = "STATUS";
			this.m_btnRemove.TabIndex = 1;
			this.m_btnRemove.Text = "REMOVE";
			this.m_btnRemove.TextAlignMain = Sys3Controls.EN_TEXTALIGN.MIDDLE_RIGHT;
			this.m_btnRemove.TextAlignSub = Sys3Controls.EN_TEXTALIGN.TOP_LEFT;
			this.m_btnRemove.UseBorder = false;
			this.m_btnRemove.UseEdge = false;
			this.m_btnRemove.UseImage = true;
			this.m_btnRemove.UseSubFont = false;
			this.m_btnRemove.Click += new System.EventHandler(this.Click_AddOrRemove);
			// 
			// Form_Loading
			// 
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
			this.ClientSize = new System.Drawing.Size(664, 581);
			this.Controls.Add(this.m_btnAdd);
			this.Controls.Add(this.m_btnRemove);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
			this.Name = "Form_Loading";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			this.Text = "Form_Loading";
			this.ResumeLayout(false);

		}

		#endregion

		private Sys3Controls.Sys3button m_btnAdd;
		private Sys3Controls.Sys3button m_btnRemove;
	}
}