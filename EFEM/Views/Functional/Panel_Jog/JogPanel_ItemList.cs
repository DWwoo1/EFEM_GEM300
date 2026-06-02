using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using FrameOfSystem3.Component;

namespace FrameOfSystem3.Views.Functional.Jog
{
	public partial class JogPanel_ItemList : UserControl
	{
		#region <CONSTRUCTOR>
		public JogPanel_ItemList(Form_Jog parentsInstance)
		{
			InitializeComponent();

			_parents = parentsInstance;
		}
		#endregion </CONSTRUCTOR>

		#region <FILED>
		Form_Jog _parents = null;
		#endregion </FILED>

		#region <ENUM>
		private enum EN_COLUMN
		{
			INDEX = 0,
			NAME = 1,
			X,
			Y,
		}
		#endregion </ENUM>

		#region <UI EVENT>
		private void Click_Cell(object sender, DataGridViewCellEventArgs e)
		{
			int nRowIndex = e.RowIndex;
			if (nRowIndex < 0 || nRowIndex >= grid_ItemList.RowCount) { return; }

			int index = int.Parse(grid_ItemList[(int)EN_COLUMN.INDEX, nRowIndex].Value.ToString());
			_parents.SelectJogItem(index);
		}
		/// <summary>
		/// 2020.01.21 by twkang [ADD] Form Key Down Event 에 Data Grid View 가 적용받지 않도록 추가
		/// </summary>
		private void KeyDown_Grid(object sender, KeyEventArgs e)
		{
			if (e.KeyData == Keys.Down || e.KeyData == Keys.Up)
			{
				e.Handled = true;
			}
		}
		#endregion </UI EVENT>

		#region <INTERFACE>
		public void UpdateJogItemOnGrid(Dictionary<int, string> itemList)
		{
			grid_ItemList.Rows.Clear();

			foreach(var kvp in itemList)
			{
				int row = grid_ItemList.Rows.Count;
				grid_ItemList.Rows.Add();
				grid_ItemList[(int)EN_COLUMN.INDEX, row].Value = kvp.Key.ToString();
				grid_ItemList.Rows[row].Cells[(int)EN_COLUMN.INDEX].Style.BackColor = Color.Silver;
				grid_ItemList.Rows[row].Cells[(int)EN_COLUMN.INDEX].Style.SelectionBackColor = Color.Silver;
				grid_ItemList.Rows[row].Cells[(int)EN_COLUMN.INDEX].Style.SelectionForeColor = Color.Black;

				grid_ItemList[(int)EN_COLUMN.NAME, row].Value = kvp.Value;
				grid_ItemList[(int)EN_COLUMN.X, row].Value = "0.000";
				grid_ItemList[(int)EN_COLUMN.Y, row].Value = "0.000";
				grid_ItemList.Rows[row].Selected = false;
			}
		}
		public void UpdatePosition(Dictionary<int, DPointXY> itemList)
		{
			for (int row = 0; row < grid_ItemList.Rows.Count; ++row)
			{
				int key;
				if (false == int.TryParse(grid_ItemList[(int)EN_COLUMN.INDEX, row].Value.ToString(), out key)) continue;
				if (false == itemList.ContainsKey(key)) continue;

				grid_ItemList[(int)EN_COLUMN.X, row].Value = itemList[key].x.ToString("F3");
				grid_ItemList[(int)EN_COLUMN.Y, row].Value = itemList[key].y.ToString("F3");
			}
		}
		#endregion </INTERFACE>
	}
}
