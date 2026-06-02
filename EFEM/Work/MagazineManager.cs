using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

using Define.DefineEnumProject.Map;
using FrameOfSystem3.Work.MapManager;

namespace FrameOfSystem3.Work.Information.Magazine
{
	public class MagazineManager : Work.MapManager.MapManager
	{
		#region singleton
		private MagazineManager()
			: base("Magazine", EN_SUBJECT_STATUS.Empty.ToString(), false, true)
		{
			Recovery();
		}
		private static MagazineManager _instance = null;
		public static MagazineManager Instance
		{
			get
			{
				if (_instance == null)
					_instance = new MagazineManager();

				return _instance;
			}
		}
		private void Recovery()
		{
			Size size = MapSize;
			for(int m = 0; m < size.Width; ++m)
			{
				for(int s = 0; s < size.Height; ++s)
				{
					var index = new Point(m,s);
					SetStatus(index, GetSlotStatus(m, s), false);
					SetText(index, s.ToString(), false);
				}
			}
			// update 필요 없음 (map link도 안되어 있을 시점)
		}
		#endregion /singleton

		#region property
		public delegate void DeleSelectedSlot(int magazineNo, int slotNo);
		public static event DeleSelectedSlot OnSelectedSlot = null;
		public enum ESlotData
		{
			Status,
			SubjectId,
		}
		#endregion /property

		#region interface
		public void SetColor(EN_SUBJECT_STATUS status, Color color)
		{
			base.SetColor(status.ToString(), color);
		}
		public Color GetColor(EN_SUBJECT_STATUS status)
		{
			return base.GetColor(status.ToString());
		}
		public void Reset(int magazineNo, int slotNo)
		{
			RemoveNode(GroupDefine.MagazineData);
			Reset(new Size(magazineNo, slotNo), false, false);
			for (int m = 0; m < magazineNo; ++m)
			{
				for(int s = 0; s < slotNo; ++s)
				{
					InsertSubject(m, s, EN_SUBJECT_STATUS.Empty, string.Empty, false);
				}
			}
			Save();
			UpdateUi();
		}
		public void SetEditMode(bool isEditMode)
		{
			if (isEditMode)
			{
				_viewManager.SetClickType(Views.MapManager.MapViewManager.EClickType.StatusChange);
			}
			else
			{
				_viewManager.SetClickType(Views.MapManager.MapViewManager.EClickType.NotifyClicked);
			}
		}

		public bool SetEnables(params bool[] options)
		{
			if (options.Length != MapSize.Width)	// MapSize.Width : magazine count
				return false;

			bool result = true;
			for (int i = 0; i < options.Length; ++i)
			{
				result &= SetValue(string.Format("Magazine_{0}", i.ToString())
					, options[i].ToString(), GroupDefine.MagazineData, "Enabled");
			}

			Save();
			return result;
		}
		public bool GetEnable(int magazineNo)
		{
			return GetValue(string.Format("Magazine_{0}", magazineNo.ToString())
				, false, GroupDefine.MagazineData, "Enabled");
		}

		public bool CheckSlotCount(int slotCount)
		{
			return MapSize.Height == slotCount;
		}
		public void SetSelectedStatus(EN_SUBJECT_STATUS status)
		{
			SelectedEditStatus = status.ToString();
		}
		public EN_SUBJECT_STATUS GetSelectedStatus()
		{
			EN_SUBJECT_STATUS result;
			if (false == Enum.TryParse(SelectedEditStatus, out result))
				return EN_SUBJECT_STATUS.Empty;

			return result;
		}

		public bool SetSlotStatus(int magazineNo, int slotNo, EN_SUBJECT_STATUS status, bool isSave = true)
		{
			if(SetSlotData(magazineNo, slotNo, ESlotData.Status.ToString(), status.ToString(), isSave))
			{
				var index = new Point(magazineNo, slotNo);
				SetStatus(index, status.ToString(), isSave);
				SetText(index, slotNo.ToString(), isSave);
				return true;
			}
			return false;
		}
		public string GetSlotStatus(int magazineNo, int slotNo)
		{
			return GetSlotData(magazineNo, slotNo, ESlotData.Status.ToString(), string.Empty);
		}

		public bool SetSubjectId(int magazineNo, int slotNo, string subjectId, bool isSave = true)
		{
			return SetSlotData(magazineNo, slotNo, ESlotData.SubjectId.ToString(), subjectId, isSave);
		}
		public string GetSubjectId(int magazineNo, int slotNo)
		{
			return GetSlotData(magazineNo, slotNo, ESlotData.SubjectId.ToString()
				, string.Empty);
		}

		public bool InsertSubject(int magazineNo, int slotNo, EN_SUBJECT_STATUS status, string subjectId, bool isSave = true)
		{
			if (false == SizeCheck(magazineNo, slotNo))
				return false;

			if (false == SetSlotStatus(magazineNo, slotNo, status, false)) return false;
			if (false == SetSubjectId(magazineNo, slotNo, subjectId, false)) return false;

			if (isSave)
			{
				Save();
				UpdateUi();
			}

			return true;
		}
		public bool FindStatus(EN_SUBJECT_STATUS status, out int magazineNo, out int slotNo)
		{
			for (int magazine = 0; magazine < MapSize.Width; ++magazine)
			{
				if (false == GetEnable(magazine))
					continue;

				for (int slot = 0; slot < MapSize.Height; ++slot)
				{
					string readStatus = GetSlotStatus(magazine, slot);
					if (readStatus == status.ToString())
					{
						magazineNo = magazine;
						slotNo = slot;
						return true;
					}
				}
			}
			magazineNo = slotNo = -1;
			return false;
		}
		#endregion /interface

		#region method
		protected override void ClickedStatusChange(Queue<Point> cells)
		{
			if (SelectedEditStatus == string.Empty)
				return;

			if (false == Task.TaskOperator.GetInstance().IsIdleMode())
				return;

			foreach(var cell in cells)
			{
				SetSlotStatus(cell.X, cell.Y, GetSelectedStatus(), false);
			}

			Save();
			UpdateUi();
		}
		protected override void SelectedCells(Queue<Point> cells)
		{
			if (OnSelectedSlot == null)
				return;

			var index = cells.Last();
			OnSelectedSlot(index.X, index.Y);
		}

		private string GetSlotData(int magazineNo, int slotNo, string key, string defaultValue)
		{
			if (false == SizeCheck(magazineNo, slotNo))
				return defaultValue;

			return GetValue(key, defaultValue, GroupDefine.MapData(magazineNo, slotNo));
		}
		private bool SetSlotData(int magazineNo, int slotNo, string key, string value, bool isSave = true)
		{
			if (false == SizeCheck(magazineNo, slotNo))
				return false;

			bool r = SetValue(key, value, GroupDefine.MapData(magazineNo, slotNo));

			//if (isSave)
				Save();

			return r;
		}
		#endregion /method

		#region inner class
		static class GroupDefine
		{
			public static readonly string MagazineData = "MagazineData";
			public static string[] MapData(int magazineNo, int slotNo)
			{
				return new string[]
				{ 
					MagazineData,
					string.Format("Magazine_{0}", magazineNo.ToString()),
					string.Format("Slot_{0}", slotNo.ToString())
				};
			}
		}
		#endregion /inner class
	}
}
