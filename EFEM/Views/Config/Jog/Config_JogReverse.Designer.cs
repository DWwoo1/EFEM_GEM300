namespace FrameOfSystem3.Views.Config
{
	partial class Config_JogReverse
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
			System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
			System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
			System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
			this.grid_ItemList = new Sys3Controls.Sys3DoubleBufferedDataGridView();
			this.m_JogGroupList = new Sys3Controls.Sys3GroupBox();
			this.m_groupConfiguration = new Sys3Controls.Sys3GroupBox();
			this.m_lblGroupName = new Sys3Controls.Sys3Label();
			this.btn_Remove = new Sys3Controls.Sys3button();
			this.btn_Add = new Sys3Controls.Sys3button();
			this.lbl_Name = new Sys3Controls.Sys3Label();
			this.sys3Label1 = new Sys3Controls.Sys3Label();
			this.lbl_Key = new Sys3Controls.Sys3Label();
			this.INDEX = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.KEY = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.NAME = new System.Windows.Forms.DataGridViewTextBoxColumn();
			((System.ComponentModel.ISupportInitialize)(this.grid_ItemList)).BeginInit();
			this.SuspendLayout();
			// 
			// grid_ItemList
			// 
			this.grid_ItemList.AllowUserToAddRows = false;
			this.grid_ItemList.AllowUserToDeleteRows = false;
			this.grid_ItemList.AllowUserToResizeColumns = false;
			this.grid_ItemList.AllowUserToResizeRows = false;
			this.grid_ItemList.BackgroundColor = System.Drawing.Color.White;
			this.grid_ItemList.CellBorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.Raised;
			this.grid_ItemList.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.Single;
			dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
			dataGridViewCellStyle1.BackColor = System.Drawing.Color.LightGray;
			dataGridViewCellStyle1.Font = new System.Drawing.Font("맑은 고딕", 11F, System.Drawing.FontStyle.Bold);
			dataGridViewCellStyle1.ForeColor = System.Drawing.Color.Black;
			dataGridViewCellStyle1.SelectionBackColor = System.Drawing.Color.White;
			dataGridViewCellStyle1.SelectionForeColor = System.Drawing.Color.Black;
			dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
			this.grid_ItemList.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
			this.grid_ItemList.ColumnHeadersHeight = 30;
			this.grid_ItemList.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
			this.grid_ItemList.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.INDEX,
            this.KEY,
            this.NAME});
			dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
			dataGridViewCellStyle2.BackColor = System.Drawing.SystemColors.Window;
			dataGridViewCellStyle2.Font = new System.Drawing.Font("맑은 고딕", 10F, System.Drawing.FontStyle.Bold);
			dataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.ControlText;
			dataGridViewCellStyle2.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(120)))), ((int)(((byte)(130)))), ((int)(((byte)(150)))));
			dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
			dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
			this.grid_ItemList.DefaultCellStyle = dataGridViewCellStyle2;
			this.grid_ItemList.EnableHeadersVisualStyles = false;
			this.grid_ItemList.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(120)))), ((int)(((byte)(130)))), ((int)(((byte)(150)))));
			this.grid_ItemList.Location = new System.Drawing.Point(2, 33);
			this.grid_ItemList.Margin = new System.Windows.Forms.Padding(1, 1, 0, 0);
			this.grid_ItemList.MultiSelect = false;
			this.grid_ItemList.Name = "grid_ItemList";
			this.grid_ItemList.ReadOnly = true;
			this.grid_ItemList.RowHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.None;
			dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
			dataGridViewCellStyle3.BackColor = System.Drawing.Color.White;
			dataGridViewCellStyle3.Font = new System.Drawing.Font("맑은 고딕", 11F);
			dataGridViewCellStyle3.ForeColor = System.Drawing.SystemColors.WindowText;
			dataGridViewCellStyle3.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(120)))), ((int)(((byte)(130)))), ((int)(((byte)(150)))));
			dataGridViewCellStyle3.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
			dataGridViewCellStyle3.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
			this.grid_ItemList.RowHeadersDefaultCellStyle = dataGridViewCellStyle3;
			this.grid_ItemList.RowHeadersVisible = false;
			this.grid_ItemList.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.DisableResizing;
			this.grid_ItemList.RowTemplate.Height = 30;
			this.grid_ItemList.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
			this.grid_ItemList.Size = new System.Drawing.Size(643, 662);
			this.grid_ItemList.TabIndex = 1275;
			this.grid_ItemList.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.Click_Item);
			// 
			// m_JogGroupList
			// 
			this.m_JogGroupList.BackGroundColor = System.Drawing.Color.WhiteSmoke;
			this.m_JogGroupList.EdgeBorderStroke = 2;
			this.m_JogGroupList.EdgeRadius = 2;
			this.m_JogGroupList.LabelFont = new System.Drawing.Font("맑은 고딕", 10F, System.Drawing.FontStyle.Bold);
			this.m_JogGroupList.LabelGradientColorFirst = System.Drawing.Color.FromArgb(((int)(((byte)(208)))), ((int)(((byte)(208)))), ((int)(((byte)(208)))));
			this.m_JogGroupList.LabelGradientColorSecond = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(240)))), ((int)(((byte)(240)))));
			this.m_JogGroupList.LabelHeight = 32;
			this.m_JogGroupList.LabelTextColor = System.Drawing.Color.Black;
			this.m_JogGroupList.Location = new System.Drawing.Point(0, 0);
			this.m_JogGroupList.Margin = new System.Windows.Forms.Padding(1, 1, 0, 0);
			this.m_JogGroupList.Name = "m_JogGroupList";
			this.m_JogGroupList.Size = new System.Drawing.Size(646, 697);
			this.m_JogGroupList.TabIndex = 1372;
			this.m_JogGroupList.Text = "ITEM LIST";
			this.m_JogGroupList.TextAlign = Sys3Controls.EN_TEXTALIGN.MIDDLE_CENTER;
			this.m_JogGroupList.ThemeIndex = 0;
			this.m_JogGroupList.UseLabelBorder = true;
			// 
			// m_groupConfiguration
			// 
			this.m_groupConfiguration.BackGroundColor = System.Drawing.Color.WhiteSmoke;
			this.m_groupConfiguration.EdgeBorderStroke = 2;
			this.m_groupConfiguration.EdgeRadius = 2;
			this.m_groupConfiguration.LabelFont = new System.Drawing.Font("맑은 고딕", 10F, System.Drawing.FontStyle.Bold);
			this.m_groupConfiguration.LabelGradientColorFirst = System.Drawing.Color.FromArgb(((int)(((byte)(208)))), ((int)(((byte)(208)))), ((int)(((byte)(208)))));
			this.m_groupConfiguration.LabelGradientColorSecond = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(240)))), ((int)(((byte)(240)))));
			this.m_groupConfiguration.LabelHeight = 32;
			this.m_groupConfiguration.LabelTextColor = System.Drawing.Color.Black;
			this.m_groupConfiguration.Location = new System.Drawing.Point(2, 696);
			this.m_groupConfiguration.Margin = new System.Windows.Forms.Padding(1, 1, 0, 0);
			this.m_groupConfiguration.Name = "m_groupConfiguration";
			this.m_groupConfiguration.Size = new System.Drawing.Size(643, 159);
			this.m_groupConfiguration.TabIndex = 1374;
			this.m_groupConfiguration.Text = "CONFIGURATION";
			this.m_groupConfiguration.TextAlign = Sys3Controls.EN_TEXTALIGN.MIDDLE_CENTER;
			this.m_groupConfiguration.ThemeIndex = 0;
			this.m_groupConfiguration.UseLabelBorder = true;
			// 
			// m_lblGroupName
			// 
			this.m_lblGroupName.BackGroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(200)))), ((int)(((byte)(200)))), ((int)(((byte)(220)))));
			this.m_lblGroupName.BorderStroke = 2;
			this.m_lblGroupName.BorderStyle = System.Windows.Forms.ButtonBorderStyle.Solid;
			this.m_lblGroupName.Description = "";
			this.m_lblGroupName.DisabledColor = System.Drawing.Color.DarkGray;
			this.m_lblGroupName.EdgeRadius = 1;
			this.m_lblGroupName.ImagePosition = new System.Drawing.Point(0, 0);
			this.m_lblGroupName.ImageSize = new System.Drawing.Point(0, 0);
			this.m_lblGroupName.LoadImage = null;
			this.m_lblGroupName.Location = new System.Drawing.Point(16, 791);
			this.m_lblGroupName.MainFont = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Bold);
			this.m_lblGroupName.MainFontColor = System.Drawing.Color.Black;
			this.m_lblGroupName.Margin = new System.Windows.Forms.Padding(1, 1, 0, 0);
			this.m_lblGroupName.Name = "m_lblGroupName";
			this.m_lblGroupName.Size = new System.Drawing.Size(111, 40);
			this.m_lblGroupName.SubFont = new System.Drawing.Font("맑은 고딕", 10F);
			this.m_lblGroupName.SubFontColor = System.Drawing.Color.Black;
			this.m_lblGroupName.SubText = "";
			this.m_lblGroupName.TabIndex = 1376;
			this.m_lblGroupName.Text = "NAME";
			this.m_lblGroupName.TextAlignMain = Sys3Controls.EN_TEXTALIGN.TOP_LEFT;
			this.m_lblGroupName.TextAlignSub = Sys3Controls.EN_TEXTALIGN.MIDDLE_CENTER;
			this.m_lblGroupName.TextAlignUnit = Sys3Controls.EN_TEXTALIGN.MIDDLE_CENTER;
			this.m_lblGroupName.ThemeIndex = 0;
			this.m_lblGroupName.UnitAreaRate = 40;
			this.m_lblGroupName.UnitFont = new System.Drawing.Font("맑은 고딕", 10F, System.Drawing.FontStyle.Bold);
			this.m_lblGroupName.UnitFontColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
			this.m_lblGroupName.UnitPositionVertical = false;
			this.m_lblGroupName.UnitText = "";
			this.m_lblGroupName.UseBorder = true;
			this.m_lblGroupName.UseEdgeRadius = false;
			this.m_lblGroupName.UseImage = false;
			this.m_lblGroupName.UseSubFont = false;
			this.m_lblGroupName.UseUnitFont = false;
			// 
			// btn_Remove
			// 
			this.btn_Remove.BorderWidth = 2;
			this.btn_Remove.ButtonClicked = false;
			this.btn_Remove.ClickedEmphasizeTextColor = System.Drawing.Color.White;
			this.btn_Remove.CustomClickedGradientFirstColor = System.Drawing.Color.White;
			this.btn_Remove.CustomClickedGradientSecondColor = System.Drawing.Color.White;
			this.btn_Remove.Description = "";
			this.btn_Remove.DisabledColor = System.Drawing.Color.DarkGray;
			this.btn_Remove.EdgeRadius = 5;
			this.btn_Remove.GradientAngle = 70F;
			this.btn_Remove.GradientFirstColor = System.Drawing.Color.White;
			this.btn_Remove.GradientSecondColor = System.Drawing.Color.FromArgb(((int)(((byte)(170)))), ((int)(((byte)(176)))), ((int)(((byte)(183)))));
			this.btn_Remove.HoverEmphasizeCustomColor = System.Drawing.Color.White;
			this.btn_Remove.ImagePosition = new System.Drawing.Point(15, 15);
			this.btn_Remove.ImageSize = new System.Drawing.Point(30, 30);
			this.btn_Remove.LoadImage = global::FrameOfSystem3.Properties.Resources.REMOVE;
			this.btn_Remove.Location = new System.Drawing.Point(491, 791);
			this.btn_Remove.MainFont = new System.Drawing.Font("맑은 고딕", 13F, System.Drawing.FontStyle.Bold);
			this.btn_Remove.MainFontColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(36)))), ((int)(((byte)(0)))));
			this.btn_Remove.Margin = new System.Windows.Forms.Padding(1, 1, 0, 0);
			this.btn_Remove.Name = "btn_Remove";
			this.btn_Remove.Size = new System.Drawing.Size(142, 55);
			this.btn_Remove.SubFont = new System.Drawing.Font("맑은 고딕", 8F, System.Drawing.FontStyle.Bold);
			this.btn_Remove.SubFontColor = System.Drawing.Color.DarkBlue;
			this.btn_Remove.SubText = "STATUS";
			this.btn_Remove.TabIndex = 1;
			this.btn_Remove.Text = "REMOVE";
			this.btn_Remove.TextAlignMain = Sys3Controls.EN_TEXTALIGN.MIDDLE_RIGHT;
			this.btn_Remove.TextAlignSub = Sys3Controls.EN_TEXTALIGN.TOP_LEFT;
			this.btn_Remove.ThemeIndex = 0;
			this.btn_Remove.UseBorder = true;
			this.btn_Remove.UseClickedEmphasizeTextColor = false;
			this.btn_Remove.UseCustomizeClickedColor = false;
			this.btn_Remove.UseEdge = false;
			this.btn_Remove.UseHoverEmphasizeCustomColor = false;
			this.btn_Remove.UseImage = true;
			this.btn_Remove.UserHoverEmpahsize = false;
			this.btn_Remove.UseSubFont = false;
			this.btn_Remove.Click += new System.EventHandler(this.Click_AddOrRemove);
			// 
			// btn_Add
			// 
			this.btn_Add.BorderWidth = 2;
			this.btn_Add.ButtonClicked = false;
			this.btn_Add.ClickedEmphasizeTextColor = System.Drawing.Color.White;
			this.btn_Add.CustomClickedGradientFirstColor = System.Drawing.Color.White;
			this.btn_Add.CustomClickedGradientSecondColor = System.Drawing.Color.White;
			this.btn_Add.Description = "";
			this.btn_Add.DisabledColor = System.Drawing.Color.DarkGray;
			this.btn_Add.EdgeRadius = 5;
			this.btn_Add.GradientAngle = 70F;
			this.btn_Add.GradientFirstColor = System.Drawing.Color.White;
			this.btn_Add.GradientSecondColor = System.Drawing.Color.FromArgb(((int)(((byte)(170)))), ((int)(((byte)(176)))), ((int)(((byte)(183)))));
			this.btn_Add.HoverEmphasizeCustomColor = System.Drawing.Color.White;
			this.btn_Add.ImagePosition = new System.Drawing.Point(15, 15);
			this.btn_Add.ImageSize = new System.Drawing.Point(30, 30);
			this.btn_Add.LoadImage = global::FrameOfSystem3.Properties.Resources.CONFIG_ADD3;
			this.btn_Add.Location = new System.Drawing.Point(491, 735);
			this.btn_Add.MainFont = new System.Drawing.Font("맑은 고딕", 13F, System.Drawing.FontStyle.Bold);
			this.btn_Add.MainFontColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(36)))), ((int)(((byte)(0)))));
			this.btn_Add.Margin = new System.Windows.Forms.Padding(1, 1, 0, 0);
			this.btn_Add.Name = "btn_Add";
			this.btn_Add.Size = new System.Drawing.Size(142, 55);
			this.btn_Add.SubFont = new System.Drawing.Font("맑은 고딕", 8F, System.Drawing.FontStyle.Bold);
			this.btn_Add.SubFontColor = System.Drawing.Color.DarkBlue;
			this.btn_Add.SubText = "STATUS";
			this.btn_Add.TabIndex = 0;
			this.btn_Add.Text = "ADD";
			this.btn_Add.TextAlignMain = Sys3Controls.EN_TEXTALIGN.MIDDLE_RIGHT;
			this.btn_Add.TextAlignSub = Sys3Controls.EN_TEXTALIGN.TOP_LEFT;
			this.btn_Add.ThemeIndex = 0;
			this.btn_Add.UseBorder = true;
			this.btn_Add.UseClickedEmphasizeTextColor = false;
			this.btn_Add.UseCustomizeClickedColor = false;
			this.btn_Add.UseEdge = false;
			this.btn_Add.UseHoverEmphasizeCustomColor = false;
			this.btn_Add.UseImage = true;
			this.btn_Add.UserHoverEmpahsize = false;
			this.btn_Add.UseSubFont = false;
			this.btn_Add.Click += new System.EventHandler(this.Click_AddOrRemove);
			// 
			// lbl_Name
			// 
			this.lbl_Name.BackGroundColor = System.Drawing.Color.White;
			this.lbl_Name.BorderStroke = 2;
			this.lbl_Name.BorderStyle = System.Windows.Forms.ButtonBorderStyle.Solid;
			this.lbl_Name.Description = "";
			this.lbl_Name.DisabledColor = System.Drawing.Color.LightGray;
			this.lbl_Name.EdgeRadius = 1;
			this.lbl_Name.Enabled = false;
			this.lbl_Name.ImagePosition = new System.Drawing.Point(0, 0);
			this.lbl_Name.ImageSize = new System.Drawing.Point(0, 0);
			this.lbl_Name.LoadImage = null;
			this.lbl_Name.Location = new System.Drawing.Point(128, 791);
			this.lbl_Name.MainFont = new System.Drawing.Font("맑은 고딕", 11F, System.Drawing.FontStyle.Bold);
			this.lbl_Name.MainFontColor = System.Drawing.Color.Black;
			this.lbl_Name.Margin = new System.Windows.Forms.Padding(1, 1, 0, 0);
			this.lbl_Name.Name = "lbl_Name";
			this.lbl_Name.Size = new System.Drawing.Size(362, 40);
			this.lbl_Name.SubFont = new System.Drawing.Font("맑은 고딕", 10F, System.Drawing.FontStyle.Bold);
			this.lbl_Name.SubFontColor = System.Drawing.Color.Black;
			this.lbl_Name.SubText = "[ -1 ]";
			this.lbl_Name.TabIndex = 0;
			this.lbl_Name.TextAlignMain = Sys3Controls.EN_TEXTALIGN.BOTTOM_RIGHT;
			this.lbl_Name.TextAlignSub = Sys3Controls.EN_TEXTALIGN.TOP_LEFT;
			this.lbl_Name.TextAlignUnit = Sys3Controls.EN_TEXTALIGN.MIDDLE_CENTER;
			this.lbl_Name.ThemeIndex = 0;
			this.lbl_Name.UnitAreaRate = 40;
			this.lbl_Name.UnitFont = new System.Drawing.Font("맑은 고딕", 10F, System.Drawing.FontStyle.Bold);
			this.lbl_Name.UnitFontColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
			this.lbl_Name.UnitPositionVertical = false;
			this.lbl_Name.UnitText = "";
			this.lbl_Name.UseBorder = true;
			this.lbl_Name.UseEdgeRadius = false;
			this.lbl_Name.UseImage = false;
			this.lbl_Name.UseSubFont = false;
			this.lbl_Name.UseUnitFont = false;
			this.lbl_Name.Click += new System.EventHandler(this.Click_Labels);
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
			this.sys3Label1.Location = new System.Drawing.Point(16, 750);
			this.sys3Label1.MainFont = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Bold);
			this.sys3Label1.MainFontColor = System.Drawing.Color.Black;
			this.sys3Label1.Margin = new System.Windows.Forms.Padding(1, 1, 0, 0);
			this.sys3Label1.Name = "sys3Label1";
			this.sys3Label1.Size = new System.Drawing.Size(111, 40);
			this.sys3Label1.SubFont = new System.Drawing.Font("맑은 고딕", 10F);
			this.sys3Label1.SubFontColor = System.Drawing.Color.Black;
			this.sys3Label1.SubText = "";
			this.sys3Label1.TabIndex = 1376;
			this.sys3Label1.Text = "KEY";
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
			// lbl_Key
			// 
			this.lbl_Key.BackGroundColor = System.Drawing.Color.White;
			this.lbl_Key.BorderStroke = 2;
			this.lbl_Key.BorderStyle = System.Windows.Forms.ButtonBorderStyle.Solid;
			this.lbl_Key.Description = "";
			this.lbl_Key.DisabledColor = System.Drawing.Color.LightGray;
			this.lbl_Key.EdgeRadius = 1;
			this.lbl_Key.Enabled = false;
			this.lbl_Key.ImagePosition = new System.Drawing.Point(0, 0);
			this.lbl_Key.ImageSize = new System.Drawing.Point(0, 0);
			this.lbl_Key.LoadImage = null;
			this.lbl_Key.Location = new System.Drawing.Point(128, 750);
			this.lbl_Key.MainFont = new System.Drawing.Font("맑은 고딕", 11F, System.Drawing.FontStyle.Bold);
			this.lbl_Key.MainFontColor = System.Drawing.Color.Black;
			this.lbl_Key.Margin = new System.Windows.Forms.Padding(1, 1, 0, 0);
			this.lbl_Key.Name = "lbl_Key";
			this.lbl_Key.Size = new System.Drawing.Size(362, 40);
			this.lbl_Key.SubFont = new System.Drawing.Font("맑은 고딕", 10F, System.Drawing.FontStyle.Bold);
			this.lbl_Key.SubFontColor = System.Drawing.Color.Black;
			this.lbl_Key.SubText = "[ -1 ]";
			this.lbl_Key.TabIndex = 0;
			this.lbl_Key.TextAlignMain = Sys3Controls.EN_TEXTALIGN.BOTTOM_RIGHT;
			this.lbl_Key.TextAlignSub = Sys3Controls.EN_TEXTALIGN.TOP_LEFT;
			this.lbl_Key.TextAlignUnit = Sys3Controls.EN_TEXTALIGN.MIDDLE_CENTER;
			this.lbl_Key.ThemeIndex = 0;
			this.lbl_Key.UnitAreaRate = 40;
			this.lbl_Key.UnitFont = new System.Drawing.Font("맑은 고딕", 10F, System.Drawing.FontStyle.Bold);
			this.lbl_Key.UnitFontColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
			this.lbl_Key.UnitPositionVertical = false;
			this.lbl_Key.UnitText = "";
			this.lbl_Key.UseBorder = true;
			this.lbl_Key.UseEdgeRadius = false;
			this.lbl_Key.UseImage = false;
			this.lbl_Key.UseSubFont = false;
			this.lbl_Key.UseUnitFont = false;
			this.lbl_Key.Click += new System.EventHandler(this.Click_Labels);
			// 
			// INDEX
			// 
			this.INDEX.HeaderText = "INDEX";
			this.INDEX.Name = "INDEX";
			this.INDEX.ReadOnly = true;
			this.INDEX.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
			this.INDEX.Width = 80;
			// 
			// KEY
			// 
			this.KEY.HeaderText = "AXIS No.";
			this.KEY.MaxInputLength = 20;
			this.KEY.Name = "KEY";
			this.KEY.ReadOnly = true;
			this.KEY.Resizable = System.Windows.Forms.DataGridViewTriState.False;
			this.KEY.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
			this.KEY.Width = 80;
			// 
			// NAME
			// 
			this.NAME.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
			this.NAME.HeaderText = "AXIS_NAME";
			this.NAME.Name = "NAME";
			this.NAME.ReadOnly = true;
			this.NAME.Resizable = System.Windows.Forms.DataGridViewTriState.False;
			this.NAME.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
			// 
			// Config_JogReverse
			// 
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
			this.Controls.Add(this.lbl_Key);
			this.Controls.Add(this.lbl_Name);
			this.Controls.Add(this.btn_Remove);
			this.Controls.Add(this.btn_Add);
			this.Controls.Add(this.sys3Label1);
			this.Controls.Add(this.m_lblGroupName);
			this.Controls.Add(this.grid_ItemList);
			this.Controls.Add(this.m_JogGroupList);
			this.Controls.Add(this.m_groupConfiguration);
			this.Name = "Config_JogReverse";
			this.Size = new System.Drawing.Size(646, 855);
			((System.ComponentModel.ISupportInitialize)(this.grid_ItemList)).EndInit();
			this.ResumeLayout(false);

		}

		#endregion

		private Sys3Controls.Sys3DoubleBufferedDataGridView grid_ItemList;
		private Sys3Controls.Sys3GroupBox m_JogGroupList;
		private Sys3Controls.Sys3GroupBox m_groupConfiguration;
		private Sys3Controls.Sys3Label m_lblGroupName;
		private Sys3Controls.Sys3button btn_Remove;
		private Sys3Controls.Sys3button btn_Add;
		private Sys3Controls.Sys3Label lbl_Name;
		private Sys3Controls.Sys3Label sys3Label1;
		private Sys3Controls.Sys3Label lbl_Key;
		private System.Windows.Forms.DataGridViewTextBoxColumn INDEX;
		private System.Windows.Forms.DataGridViewTextBoxColumn KEY;
		private System.Windows.Forms.DataGridViewTextBoxColumn NAME;
	}
}
