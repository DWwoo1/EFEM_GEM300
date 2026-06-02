using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

using Sys3Controls;

namespace FrameOfSystem3.Views.MapManager
{
	public class MapViewManager
	{
		#region constructor
		public MapViewManager(DeleClickedIndex deleConfirmStatusChange
			, bool reverseViewX = false, bool reverseViewY = false)
		{
			_deleClickCallBack = new Sys3Map.DelegateGettingCellCoordinatesWithMap(Click_Cell);

			_reverseViewX = reverseViewX;
			_reverseViewY = reverseViewY;

			OnRequestStatusChange += deleConfirmStatusChange;
		}
		#endregion /constructor

		#region field
		Sys3Map.DelegateGettingCellCoordinatesWithMap _deleClickCallBack = null;

		Size _mapSize = new Size(1, 1);
		Dictionary<Sys3Map, MapViewData> _mapList = new Dictionary<Sys3Map, MapViewData>();
		bool _reverseViewX, _reverseViewY;	// 굳이 property로 빼진 않음

		EClickType _clickType = EClickType.None;
		#endregion /field

		#region enum
		public enum EClickType
		{
			None,
			StatusChange,
			NotifyClicked,
		}
		#endregion /enum

		#region interface
		public void AddMap(Sys3Map map, bool isShowOnly)
		{
			if (_mapList.ContainsKey(map))
				return;

			_mapList.Add(map, new MapViewData());
			_mapList[map].IsShowOnly = isShowOnly;
			if(false == isShowOnly)
				map.SetCallbackFunctionForGettingCell(ref _deleClickCallBack);
		}
		public void RemoveMap(Sys3Map map)
		{
			if (_mapList.ContainsKey(map))
				_mapList.Remove(map);
		}
		public void UpdateUi(Size size, List<CellData> cellDatas)
		{
			_mapSize = size;

			for (int i = 0; i < cellDatas.Count; ++i )
			{
				cellDatas[i].Index = ApplyReverse(cellDatas[i].Index);
			}

			foreach (var map in _mapList.Keys)
			{
				if (map.MapSize != _mapSize)
					map.MapSize = _mapSize;

				map.SetCellColorAndText(cellDatas);
			}
		}
		public void SetClickType(EClickType type)
		{
			_clickType = type;
			ResetHightlight();
			SetClickOnlyOne(_clickType == EClickType.NotifyClicked);
		}
		public void ResetHightlight()
		{
			foreach (var map in _mapList.Keys)
			{
				map.SetMultiCellHighlighted(-1, -1, -1, -1);
			}
		}
		public void SetClickOnlyOne(bool isEnable)
		{
			foreach (var map in _mapList.Keys)
			{
				map.ClickOnlyOne = isEnable;
			}
		}

		#region delegate / event
		public delegate void DeleClickedIndex(Queue<Point> cellIndex);
		protected event DeleClickedIndex OnClickedIndex = null;
		public void RegisterClickEvent(DeleClickedIndex dele)
		{
			OnClickedIndex = dele;	// click event를 처리하는 곳은 한군데이어야 하므로 +=하지 않는다.
		}
		public void RevocationClickEvent()
		{
			OnClickedIndex = null;
		}

		protected static event DeleClickedIndex OnRequestStatusChange = null;
		#endregion /delegate / event

		#endregion /interface

		#region method
		private void Click_Cell(Sys3Map informedMap, Queue<Point> cells)
		{
			if (null == cells || cells.Count < 1) { return; }

			if (false == _mapList.ContainsKey(informedMap) || _mapList[informedMap].IsShowOnly)
				return;

			// reverse 하기 전에 먼저 보내야함.
			if(_clickType == EClickType.NotifyClicked)
					informedMap.SetMultiCellHighlighted(cells);

			ApplyReverse(ref cells);

			switch (_clickType)
			{
				case EClickType.None: return;
				case EClickType.StatusChange:
					if (OnRequestStatusChange == null)
						return;

					OnRequestStatusChange(cells);
					break;

				case EClickType.NotifyClicked:
					if (OnClickedIndex == null)
						return;

					OnClickedIndex(cells);
					break;
			}
		}

		private void ApplyReverse(ref Queue<Point> cells)
		{
			Queue<Point> newPoints = new Queue<Point>();
			foreach(var pt in cells)
			{
				newPoints.Enqueue(ApplyReverse(pt));
			}
			cells = newPoints;
		}
		private void ApplyReverse(ref Point index)
		{
			if (_reverseViewX)
			{
				index.X = _mapSize.Width - 1 - index.X;
			}
			if (_reverseViewY)
			{
				index.Y = _mapSize.Height - 1 - index.Y;
			}
		}
		private Point ApplyReverse(Point index)
		{
			if (_reverseViewX)
			{
				index.X = _mapSize.Width - 1 - index.X;
			}
			if (_reverseViewY)
			{
				index.Y = _mapSize.Height - 1 - index.Y;
			}
			return index;
		}
		#endregion /method

		#region inner class
		class MapViewData
		{
			public MapViewData()
			{
				IsShowOnly = false;
			}

			public bool IsShowOnly { get; set; }
		}
		#endregion /inner class
	}
}
