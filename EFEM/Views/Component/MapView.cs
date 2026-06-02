using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using FrameOfSystem3.Work;
using Define;
using Define.DefineEnumProject.Map;

namespace FrameOfSystem3
{
	public enum EN_DISPLAY_CENTER
	{
		BASE,
		UNIT,
	}
	public enum EN_MAPVIEW_MODE
	{
		VEIW,
		EDIT,
		MOVE,
	}

	public partial class MapView : UserControl
	{
		#region 변수
		// private CustomMapLabel[] m_lblMapUnits = null;

		#region For Drawing
		private Bitmap m_MapBitMap;
		private Graphics m_MapDC;
		private Graphics m_DrawDC;
		Pen pen = new Pen(Color.Gray);

		private Dictionary<EN_UNIT_STATUS, Brush> m_dicofStatusBackColor = new Dictionary<EN_UNIT_STATUS, Brush>();
		private Dictionary<EN_UNIT_STATUS, Brush> m_dicofStatusFontColor = new Dictionary<EN_UNIT_STATUS, Brush>();
		#endregion

		private EN_DISPLAY_CENTER m_enDisplayCenter = EN_DISPLAY_CENTER.BASE;
		private EN_MAPVIEW_MODE m_enMapViewMode = EN_MAPVIEW_MODE.VEIW;

		private bool m_bDrawCross = false;

		private int TotalCount;
		private int MaxX = 1;
		private int MaxY = 1;
		int DisplayAreaX, DisplayAreaY;
		private int Display_OffsetX = 0;
		private int Display_OffsetY = 0;
		private int View_CenterX = 0, View_CenterY = 0;
		private int Display_Chip_SizeX = 30;
		private int Display_Chip_SizeY = 30;

		int UnitCountX = 1;
		int UnitCountY = 1;
		int UnitCountInGroupX = 1;
		int UnitCountInGroupY = 1;
		int GroupCountX = 1;
		int GroupCountY = 1;

		private int m_nSelectedIndex = 1;
		private List<int> m_lstSelectedIndexForEdit = new List<int>();

		private Dictionary<int, EN_UNIT_STATUS> m_dicOfStatus = new Dictionary<int, EN_UNIT_STATUS>();
        private Dictionary<int[], EN_UNIT_STATUS> m_dicOfGroup = new Dictionary<int[], EN_UNIT_STATUS>();
		private int Display_Chip_Size
		{
			get
			{
				int Value = Display_Chip_SizeX < Display_Chip_SizeY ? Display_Chip_SizeX : Display_Chip_SizeY;
				return Value;
			}
			set
			{
				Display_Chip_SizeX = value;
				Display_Chip_SizeY = value;
			}
		}

		#region ForDrag
		double DragStartX = 0;
		double DragStartY = 0;
		#endregion
		#endregion

		#region 속성
		public int nSelectedIndex { get { return m_nSelectedIndex; } set { m_nSelectedIndex = value; } }
		public EN_MAPVIEW_MODE enMapViewMode { get { return m_enMapViewMode; } set { m_enMapViewMode = value; } }
		public List<int> lstSelectedIndexForEdit { get { return m_lstSelectedIndexForEdit; } set { m_lstSelectedIndexForEdit = value; } }
		#endregion

		#region 외부 event
		public delegate void MouseUpDelegate(bool bRight);
		public event MouseUpDelegate MouseUpEvent;
		#endregion
		public MapView()
		{
			InitializeComponent();
			m_DrawDC = null;
			m_MapBitMap = null;
			m_MapDC = null;
			MapPanel.Size = this.Size;

			m_DrawDC = MapPanel.CreateGraphics();
			m_MapBitMap = new Bitmap(MapPanel.ClientRectangle.Width, MapPanel.ClientRectangle.Height, m_DrawDC);
			m_MapDC = Graphics.FromImage(m_MapBitMap);

			SetBackColorList(m_dicofStatusBackColor);
			SetFontColorList(m_dicofStatusFontColor);

			this.MapPanel.MouseWheel += new System.Windows.Forms.MouseEventHandler(this.Map_MouseWheel);

		}

		#region 외부 인터페이스
		public void InitControl(int nUnitCountX, int nUnitCountY, int nGroupCountX = 1, int nGruopCountY = 1)
		{
			MapPanel.Size = this.Size;

			m_DrawDC = MapPanel.CreateGraphics();
			m_MapBitMap = new Bitmap(MapPanel.ClientRectangle.Width, MapPanel.ClientRectangle.Height, m_DrawDC);
			m_MapDC = Graphics.FromImage(m_MapBitMap);

			InitUnitComponent(nUnitCountX, nUnitCountY, nGroupCountX, nGruopCountY);
		}

		/// <summary>
		/// 2019.03.11 by ssh [ADD] 각 Unit 에 Parameter 값을 부여하여 색 표시한다.
		/// </summary>
		public void SetCellByParameter()
		{
			DrawUnitComponent();
		}

		public void SetScalePlus()
		{

			View_CenterX = m_nSelectedIndex % MaxX;
			View_CenterY = -(int)(m_nSelectedIndex / MaxX + 1);
			if (View_CenterX == 0)
			{
				View_CenterX = MaxX;
				View_CenterY += 1;
			}

			m_enDisplayCenter = EN_DISPLAY_CENTER.UNIT;

			Display_Chip_SizeX += 2;
			Display_Chip_SizeY += 2; // (2 * Display_Chip_SizeY / Display_Chip_SizeX);
			DrawUnitComponent();
		}
		public void SetScaleMinus()
		{
            if (Display_Chip_Size < 5)
                return;
			View_CenterX = m_nSelectedIndex % MaxX;
			View_CenterY = -(int)(m_nSelectedIndex / MaxX + 1);
			if (View_CenterX == 0)
			{
				View_CenterX = MaxX;
				View_CenterY += 1;
			}

			m_enDisplayCenter = EN_DISPLAY_CENTER.UNIT;

			Display_Chip_SizeX -= 2;
			Display_Chip_SizeY -= 2; // (2 * Display_Chip_SizeY / Display_Chip_SizeX);
			DrawUnitComponent();
		}

		public void SetStatus(Dictionary<int, EN_UNIT_STATUS> dicOfStatus)
		{
			m_dicOfStatus = dicOfStatus;
			DrawUnitComponent();
		}

        public void SetGroupStatus(Dictionary<int[], EN_UNIT_STATUS> dicOfStatus)
		{
			m_dicOfGroup = dicOfStatus;
		}
		#endregion


		#region 내부 인터페이스
		#region UI Draw 관련
		private void InitUnitComponent(int nUnitCountX, int nUnitCountY, int nGroupCountX = 1, int nGruopCountY = 1)
		{
			GroupCountX = nGroupCountX;
			GroupCountY = nGruopCountY;
			UnitCountInGroupX = nUnitCountX;
			UnitCountInGroupY = nUnitCountY;
			UnitCountX = UnitCountInGroupX * GroupCountX;
			UnitCountY = UnitCountInGroupY * GroupCountY;
			TotalCount = UnitCountX * UnitCountY;

			MaxX = UnitCountX;
			MaxY = UnitCountY;
			if (MaxX == 0)
				return;
			Display_Chip_SizeX = MapPanel.ClientRectangle.Width / MaxX;
			if (MaxY == 0)
				return;
			Display_Chip_SizeY = MapPanel.ClientRectangle.Height / MaxY;

            if (Display_Chip_SizeX < 5)
                Display_Chip_SizeX = 5;

            if (Display_Chip_SizeY < 5)
                Display_Chip_SizeY = 5;

			m_enDisplayCenter = EN_DISPLAY_CENTER.BASE;
			View_CenterX = MaxX / 2 + 1;
			View_CenterY = -MaxY / 2 - 1;
		}
		private void DrawUnitComponent()
		{
			try
			{
				SetDisplayOffset();

				m_MapDC.FillRectangle(Brushes.Black, 0, 0, MapPanel.ClientRectangle.Width, MapPanel.ClientRectangle.Height);

				for (int iy = 5001; iy <= 5000 + MaxY; iy++)
				{
					for (int ix = 5001; ix <= 5000 + MaxX; ix++)
					{
						Wafer_Chip_Draw(ix, iy);
					}
				}

				#region draw cross
				if (m_bDrawCross)
				{
					pen.Color = Color.Orange;

					m_MapDC.DrawLine(pen, MapPanel.ClientRectangle.Width / 2, 0
						, MapPanel.ClientRectangle.Width / 2, MapPanel.ClientRectangle.Height);

					m_MapDC.DrawLine(pen, 0, MapPanel.ClientRectangle.Height / 2,
										MapPanel.ClientRectangle.Width, MapPanel.ClientRectangle.Height / 2);
				}
				#endregion

				DrawGruopRect();

				Wafer_Display_Refresh();
			}
			catch
			{
			}
		}
		private void Wafer_Chip_Draw(int x, int y)
		{
			Brush nBrush = Brushes.Silver;
			Brush nTextBrush = Brushes.Black;
			pen.Color = Color.Orange;

			string strLabel = string.Empty;
			EN_UNIT_STATUS enStatus;

            int nIndex = ((y - 5001) * MaxX + (x - 5000));
			int MarginX = 1;
			int MarginY = 1;
			int StartMarginX = 1;
			int StartMarginY = 1;

			if (m_dicOfStatus.ContainsKey(nIndex))
				enStatus = m_dicOfStatus[nIndex];
			else
				enStatus = EN_UNIT_STATUS.WAIT;

			nBrush = m_dicofStatusBackColor[enStatus];
			nTextBrush = m_dicofStatusFontColor[enStatus];
			strLabel = nIndex.ToString();

			DisplayAreaX = (x - 5000) * Display_Chip_SizeX + Display_OffsetX;
            DisplayAreaY = ((y - 5000)) * Display_Chip_SizeY + Display_OffsetY;

            if ((x - 5000) % UnitCountInGroupX == 0) MarginX = 4;
            if ((x - 5000) % UnitCountInGroupX == 1) StartMarginX = 4;
            if ((y - 5000) % UnitCountInGroupY == 0) MarginY = 4;
            if ((y - 5000) % UnitCountInGroupY == 1) StartMarginY = 4;

			float FontSize = 20;
			bool bNumView = true;
			if (Display_Chip_Size <= 15)
				bNumView = false;
			if (bNumView)
			{
				if (Display_Chip_Size <= 50)
					FontSize = 20 * Display_Chip_Size / 50;
			}

            StringFormat stringFormat = new StringFormat();

            stringFormat.Alignment = StringAlignment.Center;

            stringFormat.LineAlignment = StringAlignment.Center;

			if (DisplayAreaX > Display_Chip_SizeX * -1 && DisplayAreaX < MapPanel.ClientRectangle.Width
				&& DisplayAreaY > Display_Chip_SizeY * -1 && DisplayAreaY < MapPanel.ClientRectangle.Height)
			{
				m_MapDC.FillRectangle(nBrush, DisplayAreaX + StartMarginX, DisplayAreaY + StartMarginY, Display_Chip_SizeX - StartMarginX - MarginX, Display_Chip_SizeY - StartMarginY - MarginY);

				if (bNumView)
					m_MapDC.DrawString(strLabel, new Font("Arial", FontSize), nTextBrush,
                                         DisplayAreaX + (Display_Chip_SizeX / 2)/* - (FontSize / 2) - nTextOffset*/, DisplayAreaY + (Display_Chip_SizeY / 2)/* - (FontSize / 2)*/, stringFormat);

			}
		}
		private void SetDisplayOffset()
		{
			switch (m_enDisplayCenter)
			{
				case EN_DISPLAY_CENTER.BASE:
					bool bOddX = MaxX % 2 == 1 ? true : false;
					bool bOddY = MaxY % 2 == 1 ? true : false;

					Display_OffsetX = (MapPanel.ClientRectangle.Width / 2) - (View_CenterX * Display_Chip_SizeX);
					if (bOddX)
						Display_OffsetX -= Display_Chip_SizeX / 2;

					Display_OffsetY = (MapPanel.ClientRectangle.Height / 2) + (View_CenterY * Display_Chip_SizeY);
					if (bOddY)
						Display_OffsetY -= Display_Chip_SizeY / 2;
					break;
				case EN_DISPLAY_CENTER.UNIT:
					Display_OffsetX = (MapPanel.ClientRectangle.Width / 2) - (View_CenterX * Display_Chip_SizeX);
					Display_OffsetX -= Display_Chip_SizeX / 2;

					Display_OffsetY = (MapPanel.ClientRectangle.Height / 2) + (View_CenterY * Display_Chip_SizeY);
					Display_OffsetY -= Display_Chip_SizeY / 2;
					break;
			}


		}

		private void DrawGruopRect()
		{
			pen.Width = 2;

			if(null == m_dicOfGroup) return;

			foreach (var Item in m_dicOfGroup)
			{
				int nLT = Item.Key.Min();
				int nRB = Item.Key.Max();
				switch (Item.Value)
				{
                    case EN_UNIT_STATUS.SKIP:
						pen.Color = Color.Red;
						break;
					case EN_UNIT_STATUS.WAIT:
						pen.Color = Color.Orange;
						break;
					default:
						pen.Color = Color.Blue;
						break;
				}
				IPointXY LTIndex = new IPointXY();
				LTIndex.x = ((nLT - 1) % UnitCountX) + 1;
				LTIndex.y = ((nLT - 1) / UnitCountX) + 1;
				IPointXY RBIndex = new IPointXY();
				RBIndex.x = ((nRB - 1) % UnitCountX) + 1;
				RBIndex.y = ((nRB - 1) / UnitCountX) + 1;

				DisplayAreaX = LTIndex.x * Display_Chip_SizeX + Display_OffsetX;
				DisplayAreaY = LTIndex.y * Display_Chip_SizeY + Display_OffsetY;

				int MarginX = 1;
				int MarginY = 1;
				int StartMarginX = 1;
				int StartMarginY = 1;

				if ((RBIndex.x) % UnitCountInGroupX == 0) MarginX = 4;
				if (LTIndex.x % UnitCountInGroupX == 1) StartMarginX = 4;
				if ((RBIndex.y) % UnitCountInGroupY == 0) MarginY = 4;
				if (LTIndex.y % UnitCountInGroupY == 1) StartMarginY = 4;

				if (DisplayAreaX > (Display_Chip_SizeX * ((RBIndex.x - LTIndex.x) + 1)) * -1 && DisplayAreaX < MapPanel.ClientRectangle.Width
					&& DisplayAreaY > (Display_Chip_SizeY * ((RBIndex.y - LTIndex.y) + 1)) * -1 && DisplayAreaY < MapPanel.ClientRectangle.Height)
				{
					m_MapDC.DrawRectangle(pen, DisplayAreaX + StartMarginX, DisplayAreaY + StartMarginY, (Display_Chip_SizeX * ((RBIndex.x - LTIndex.x) + 1)) - MarginX - StartMarginX, (Display_Chip_SizeY * ((RBIndex.y - LTIndex.y) + 1)) - MarginY - StartMarginY);
				}
			}
		}
		private void Wafer_Display_Refresh()
		{
			m_DrawDC.DrawImageUnscaled(m_MapBitMap, 0, 0);
		}

		#endregion

		#region Color
		private void SetBackColorList(Dictionary<EN_UNIT_STATUS, Brush> dicofColorList)
		{
			if (!dicofColorList.ContainsKey(EN_UNIT_STATUS.ALIGN))
				dicofColorList.Add(EN_UNIT_STATUS.ALIGN, new SolidBrush(Color.DeepSkyBlue));
			if (!dicofColorList.ContainsKey(EN_UNIT_STATUS.DONE))
				dicofColorList.Add(EN_UNIT_STATUS.DONE, new SolidBrush(Color.Blue));
			if (!dicofColorList.ContainsKey(EN_UNIT_STATUS.EMPTY))
				dicofColorList.Add(EN_UNIT_STATUS.EMPTY, new SolidBrush(Color.Black));
			if (!dicofColorList.ContainsKey(EN_UNIT_STATUS.FAIL))
				dicofColorList.Add(EN_UNIT_STATUS.FAIL, new SolidBrush(Color.Black));
			if (!dicofColorList.ContainsKey(EN_UNIT_STATUS.IDCODE))
				dicofColorList.Add(EN_UNIT_STATUS.IDCODE, new SolidBrush(Color.LightGreen));
			if (!dicofColorList.ContainsKey(EN_UNIT_STATUS.NONE))
				dicofColorList.Add(EN_UNIT_STATUS.NONE, new SolidBrush(Color.White));
			if (!dicofColorList.ContainsKey(EN_UNIT_STATUS.REFERENCE))
				dicofColorList.Add(EN_UNIT_STATUS.REFERENCE, new SolidBrush(Color.Yellow));
			if (!dicofColorList.ContainsKey(EN_UNIT_STATUS.REJECT))
				dicofColorList.Add(EN_UNIT_STATUS.REJECT, new SolidBrush(Color.Black));
			if (!dicofColorList.ContainsKey(EN_UNIT_STATUS.SKIP))
				dicofColorList.Add(EN_UNIT_STATUS.SKIP, new SolidBrush(Color.OrangeRed));
			if (!dicofColorList.ContainsKey(EN_UNIT_STATUS.STACK))
				dicofColorList.Add(EN_UNIT_STATUS.STACK, new SolidBrush(Color.Black));
			if (!dicofColorList.ContainsKey(EN_UNIT_STATUS.WAIT))
				dicofColorList.Add(EN_UNIT_STATUS.WAIT, new SolidBrush(Color.Green));
			if (!dicofColorList.ContainsKey(EN_UNIT_STATUS.INSP))
				dicofColorList.Add(EN_UNIT_STATUS.INSP, new SolidBrush(Color.CornflowerBlue));
		}
		private void SetFontColorList(Dictionary<EN_UNIT_STATUS, Brush> dicofColorList)
		{
			if (!dicofColorList.ContainsKey(EN_UNIT_STATUS.ALIGN))
				dicofColorList.Add(EN_UNIT_STATUS.ALIGN, new SolidBrush(Color.White));
			if (!dicofColorList.ContainsKey(EN_UNIT_STATUS.DONE))
				dicofColorList.Add(EN_UNIT_STATUS.DONE, new SolidBrush(Color.White));
			if (!dicofColorList.ContainsKey(EN_UNIT_STATUS.EMPTY))
				dicofColorList.Add(EN_UNIT_STATUS.EMPTY, new SolidBrush(Color.Black));
			if (!dicofColorList.ContainsKey(EN_UNIT_STATUS.FAIL))
				dicofColorList.Add(EN_UNIT_STATUS.FAIL, new SolidBrush(Color.White));
			if (!dicofColorList.ContainsKey(EN_UNIT_STATUS.IDCODE))
				dicofColorList.Add(EN_UNIT_STATUS.IDCODE, new SolidBrush(Color.White));
			if (!dicofColorList.ContainsKey(EN_UNIT_STATUS.NONE))
				dicofColorList.Add(EN_UNIT_STATUS.NONE, new SolidBrush(Color.White));
			if (!dicofColorList.ContainsKey(EN_UNIT_STATUS.REFERENCE))
				dicofColorList.Add(EN_UNIT_STATUS.REFERENCE, new SolidBrush(Color.White));
			if (!dicofColorList.ContainsKey(EN_UNIT_STATUS.REJECT))
				dicofColorList.Add(EN_UNIT_STATUS.REJECT, new SolidBrush(Color.White));
			if (!dicofColorList.ContainsKey(EN_UNIT_STATUS.SKIP))
				dicofColorList.Add(EN_UNIT_STATUS.SKIP, new SolidBrush(Color.White));
			if (!dicofColorList.ContainsKey(EN_UNIT_STATUS.STACK))
				dicofColorList.Add(EN_UNIT_STATUS.STACK, new SolidBrush(Color.White));
			if (!dicofColorList.ContainsKey(EN_UNIT_STATUS.WAIT))
				dicofColorList.Add(EN_UNIT_STATUS.WAIT, new SolidBrush(Color.White));
			if (!dicofColorList.ContainsKey(EN_UNIT_STATUS.INSP))
				dicofColorList.Add(EN_UNIT_STATUS.INSP, new SolidBrush(Color.White));
		}
		#endregion
		#endregion

		#region UI 이벤트
		private void Map_MouseWheel(object sender, System.Windows.Forms.MouseEventArgs e)
		{
			double X = ((e.X - Display_OffsetX) / Display_Chip_SizeX);
			double Y = ((e.Y - Display_OffsetY - (Display_Chip_SizeY)) / Display_Chip_SizeY);
			if (X > 0 && Y < MaxY && X <= MaxX && Y >= 0)
			{
				View_CenterX = (int)X;
				View_CenterY = -(int)(Y + 1);
			}
			m_enDisplayCenter = EN_DISPLAY_CENTER.UNIT;
			if (e.Delta / 120 > 0)
			{
				Display_Chip_SizeX += 2;
				Display_Chip_SizeY += 2; // (2 * Display_Chip_SizeY / Display_Chip_SizeX);
				DrawUnitComponent();
			}
			else
			{
				if (Display_Chip_Size >= 5)
				{
					Display_Chip_SizeX -= 2;
					Display_Chip_SizeY -= 2; // (2 * Display_Chip_SizeY / Display_Chip_SizeX);
				}
				DrawUnitComponent();
			}
		}

		private void MapPanel_Paint(object sender, PaintEventArgs e)
		{
			Wafer_Display_Refresh();
		}

		#region 클릭 관련
		private void MapPanel_MouseDown(object sender, MouseEventArgs e)
		{
			DragStartX = ((e.X - Display_OffsetX) / Display_Chip_SizeX);
			DragStartY = ((e.Y - Display_OffsetY - (Display_Chip_SizeY)) / Display_Chip_SizeY);
			m_lstSelectedIndexForEdit.Clear();
		}
		private void MapPanel_MouseUp(object sender, MouseEventArgs e)
		{
			double X = ((e.X - Display_OffsetX) / Display_Chip_SizeX);
			double Y = ((e.Y - Display_OffsetY - (Display_Chip_SizeY)) / Display_Chip_SizeY);
			int nIndex = -1;
			if (X > 0 && Y < MaxY && X <= MaxX && Y >= 0)
			{
				nIndex = ((int)Y * MaxX + (int)X);
			}

			if (e.Button == MouseButtons.Left)
			{

				if (nIndex == -1)
					return;
				m_nSelectedIndex = nIndex;

				double startX, endX;
				double startY, endY;
				if (DragStartX < X)
				{
					startX = DragStartX;
					endX = X;
				}
				else
				{
					startX = X;
					endX = DragStartX;
				}
				if (DragStartY < Y)
				{
					startY = DragStartY;
					endY = Y;
				}
				else
				{
					startY = Y;
					endY = DragStartY;
				}

				for (double i = startX; i <= endX; i++)
				{
					for (double j = startY; j <= endY; j++)
					{
						if (X > 0 && Y < MaxY && X <= MaxX && Y >= 0)
						{
							nIndex = ((int)j * MaxX + (int)i);
							m_lstSelectedIndexForEdit.Add(nIndex);
						}
					}
				}


				if (MouseUpEvent != null)
					MouseUpEvent(false);
			}

			if (e.Button == MouseButtons.Right)
			{
				if (nIndex == -1)
					return;
				m_nSelectedIndex = nIndex;
				if (MouseUpEvent != null)
					MouseUpEvent(true);
			}

		}
		#endregion
		#endregion
	}
}
