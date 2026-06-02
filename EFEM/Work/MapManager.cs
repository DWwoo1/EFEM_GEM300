using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

using XmlWriter_;

using FrameOfSystem3.Views.MapManager;

namespace FrameOfSystem3.Work.MapManager
{
	abstract public class MapManager : XmlWriter
	{
		public MapManager(string mapName, string defaultStatus, bool reverseViewX, bool reverseViewY)
			: base(string.Format("{0}_MapManager", mapName), Define.DefineConstant.FilePath.FILEPATH_RECOVERY)
		{
			Load();
			_viewManager = new MapViewManager(ClickedStatusChange, reverseViewX, reverseViewY);
			_viewManager.RegisterClickEvent(SelectedCells);

			def_Status = defaultStatus;
			def_Color = Color.White;

			Reset(false, false);
		}

		#region field

		#region define
		readonly string KeyR = "Red";
		readonly string KeyG = "Green";
		readonly string KeyB = "Blue";

		readonly string def_Status;
		readonly Color def_Color;
		#endregion /define

		protected MapViewManager _viewManager = null;
		protected Dictionary<Point, MapData> _dataList = new Dictionary<Point, MapData>();

		private string _selectedEditStatus = string.Empty;
		#endregion /field

		#region property
		protected string SelectedEditStatus
		{
			get { return _selectedEditStatus; }
			set { _selectedEditStatus = value; }
		}
		#endregion /property

		#region interface
		public void AddMapControl(Sys3Controls.Sys3Map mapControl, bool isClickEventBlock)
		{
			_viewManager.AddMap(mapControl, isClickEventBlock);
		}
		public void RemoveMapControl(Sys3Controls.Sys3Map mapControl)
		{
			_viewManager.RemoveMap(mapControl);
		}
		
		protected void Reset(Size size, bool isSave = true, bool isUpdateUi = true)
		{
			SetValue("Width", size.Width, GroupDefine.MapSize);
			SetValue("Height", size.Height, GroupDefine.MapSize);

			_dataList.Clear();
			for(int y = 0; y < size.Height; ++y)
			{
				for(int x = 0; x<size.Width; ++x)
				{
					int n = (size.Width * y) + x;
					var pt = new Point(x, y);
					_dataList.Add(pt, new MapData(n, pt));
					_dataList[pt].Status = def_Status;
				}
			}
			if(isSave) Save();
			if(isUpdateUi) UpdateUi();
		}
		protected void Reset(bool isSave = true, bool isUpdateUi = true)
		{
			Reset(MapSize, isSave, isUpdateUi);
		}
		
		public Size MapSize
		{
			get
			{
				int width = GetValue("Width", 1, GroupDefine.MapSize);
				int height = GetValue("Height", 1, GroupDefine.MapSize);
				return new Size(width, height);
			}
		}
		public bool SetColor(string status, Color color)
		{
			bool r = true;
			r &= SetValue(KeyR, (int)color.R, GroupDefine.Color(status));
			r &= SetValue(KeyG, (int)color.G, GroupDefine.Color(status));
			r &= SetValue(KeyB, (int)color.B, GroupDefine.Color(status));

			if(r) Save();
			return r;
		}
		public Color GetColor(string status)
		{
			return Color.FromArgb(R(status), G(status), B(status));
		}

		public void UpdateUi()
		{
			List<Sys3Controls.CellData> cellDatas = new List<Sys3Controls.CellData>();

			foreach (var data in _dataList.Values)
			{
				cellDatas.Add(new Sys3Controls.CellData(
					data.Index, GetColor(data.Status), data.Text));
			}

			_viewManager.UpdateUi(MapSize, cellDatas);
		}
		#endregion /interface

		#region method
		protected bool SetStatus(Dictionary<Point, string> datas)
		{
			foreach (var kvp in datas)
			{
				if (false == SetStatus(kvp.Key, kvp.Value, false)) return false;
			}

			UpdateUi();
			return true;
		}
		protected bool SetStatus(Point index, string status, bool isUpdate = true)
		{
			if (false == _dataList.ContainsKey(index))
				return false;

			_dataList[index].Status = status;

			if (isUpdate)
				UpdateUi();

			return true;
		}
		protected bool SetText(Dictionary<Point, string> datas)
		{
			foreach (var kvp in datas)
			{
				if (false == SetText(kvp.Key, kvp.Value, false)) return false;
			}

			UpdateUi();
			return true;
		}
		protected bool SetText(Point index, string text, bool isUpdate = true)
		{
			if (false == _dataList.ContainsKey(index))
				return false;

			_dataList[index].Text = text;

			if (isUpdate)
				UpdateUi();

			return true;
		}

		protected abstract void ClickedStatusChange(Queue<Point> cells);
		
		/// <summary>
		/// Status 변경 모드일 때 변경 가능한 조건
		/// </summary>
		protected abstract void SelectedCells(Queue<Point> cells);
		protected bool SizeCheck(Point index)
		{
			if (SizeCheck(index.X, index.Y))
				return false;

			return true;
		}
		protected bool SizeCheck(int width, int height)
		{
			if (MapSize.Width <= width || MapSize.Height <= height)
				return false;

			return true;
		}

		#region color
		private int R(string status)
		{
			return GetValue(KeyR, (int)def_Color.R, GroupDefine.Color(status));
		}
		private int G(string status)
		{
			return GetValue(KeyG, (int)def_Color.G, GroupDefine.Color(status));
		}
		private int B(string status)
		{
			return GetValue(KeyB, (int)def_Color.B, GroupDefine.Color(status));
		}
		#endregion /color

		#endregion /method

		#region inner class
		public class MapData
		{
			string _status = string.Empty;
			int _number = -1;
			Point _index = new Point(-1, -1);
			string _text = string.Empty;

			public MapData(int number, Point index)
			{
				_number = number;
				_index = index;
			}

			public string Status
			{
				get { return _status; }
				set { _status = value; }
			}
			public string Text
			{
				get { return _text; }
				set { _text = value; }
			}
			public int Number
			{
				get { return _number; }
				set { _number = value; }
			}
			public Point Index
			{
				get { return _index; }
				set { _index = value; }
			}
		}
		static class GroupDefine
		{
			public static readonly string Base = "Base";
			public static readonly string[] MapSize = new string[] { Base, "MapSize" };

			public static string[] Color(string status)
			{
				return new string[] { Base, "Color", status };
			}
		}
		#endregion /inner class
	}
}
