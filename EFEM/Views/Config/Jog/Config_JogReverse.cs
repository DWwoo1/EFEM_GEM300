using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FrameOfSystem3.Views.Config
{
	public partial class Config_JogReverse : UserControlForMainView.CustomView
	{
		public Config_JogReverse()
		{
			InitializeComponent();

			#region Instance
			m_InstanceOfJog											= FrameOfSystem3.Config.ConfigJog.GetInstance();
			m_InstanceOfMotion										= FrameOfSystem3.Config.ConfigMotion.GetInstance();
			m_InstanceOfSelectionList								= Functional.Form_SelectionList.GetInstance();
			m_InstanceOfMessageBox									= Functional.Form_MessageBox.GetInstance();
			#endregion
		}

		#region 상수
		private const int HEIGHT_OF_ROWS							= 30;
		private const int SELECT_NONE								= -1;

		private readonly Color c_clrTrue						= Color.DodgerBlue;
		private readonly Color c_clrFalse						= Color.White;		
		#endregion
		private enum EN_COLUMN_INDEX
		{
			INDEX = 0,
			AXIS_NO,
			AXIS_NAME,
		}


		#region 변수

		#region Instance
		FrameOfSystem3.Config.ConfigMotion m_InstanceOfMotion		= null;
		FrameOfSystem3.Config.ConfigJog m_InstanceOfJog				= null;
		Functional.Form_SelectionList m_InstanceOfSelectionList		= null;
		Functional.Form_MessageBox m_InstanceOfMessageBox			= null;
		#endregion

		private int[] m_nArrOfItem									= null;

		private int m_nIndexOfSelectedItem							= -1;
		private int m_nIndexOfSelectedRows							= -1;
		#endregion

		#region 상속 인터페이스
		/// <summary>
		/// 
		/// </summary>
		protected override void ProcessWhenActivation()
		{
			UpdateForm();
			if(SELECT_NONE != m_nIndexOfSelectedRows)
			{
				grid_ItemList.Rows[m_nIndexOfSelectedRows].Selected	= true;
			}
		}
		/// <summary>
		/// 
		/// </summary>
		protected override void ProcessWhenDeactivation()
		{

		}
		/// <summary>
		/// 
		/// </summary>
		public override void CallFunctionByTimer()
		{
			
		}
		#endregion

		#region 내부인터페이스
		/// <summary>
		/// 2020.08.12 by twkang [ADD] 아이템의 파라미터로 GridView 를 업데이트한다.
		/// </summary>
		private void UpdateForm()
		{
			if(false == m_InstanceOfJog.GetListOfReverseItem(ref m_nArrOfItem))
				return;

			grid_ItemList.Rows.Clear();
			for(int i = 0; i < m_nArrOfItem.Length; ++i)
			{
				AddItemOnGrid(m_nArrOfItem[i]);
			}
		}
		/// <summary>
		/// 2020.07.16 by twkang [ADD] 아이템을 GridView 에 추가한다.
		/// </summary>
		private void AddItemOnGrid(int nIndexOfItem)
		{
			int nIndexOfRow		= grid_ItemList.Rows.Count;
			string strValue		= string.Empty;

			grid_ItemList.Rows.Add();
			grid_ItemList.Rows[nIndexOfRow].Height					= HEIGHT_OF_ROWS;
			grid_ItemList[(int)EN_COLUMN_INDEX.INDEX, nIndexOfRow].Value	= nIndexOfItem.ToString();
			grid_ItemList.Rows[nIndexOfRow].Cells[(int)EN_COLUMN_INDEX.INDEX].Style.BackColor = Color.Silver;
			grid_ItemList.Rows[nIndexOfRow].Cells[(int)EN_COLUMN_INDEX.INDEX].Style.SelectionBackColor = Color.Silver;
			grid_ItemList.Rows[nIndexOfRow].Cells[(int)EN_COLUMN_INDEX.INDEX].Style.SelectionForeColor = Color.Black;

			if(false == m_InstanceOfJog.GetParameterReverse(nIndexOfItem, FrameOfSystem3.Config.ConfigJog.EN_PARAM_JOG_REVERSE.NAME, ref strValue))
			{
				strValue	= string.Empty;
			}
			grid_ItemList[(int)EN_COLUMN_INDEX.AXIS_NAME, nIndexOfRow].Value = strValue;

			if (false == m_InstanceOfJog.GetParameterReverse(nIndexOfItem, FrameOfSystem3.Config.ConfigJog.EN_PARAM_JOG_REVERSE.KEY, ref strValue))
			{
				strValue = string.Empty;
			}
			grid_ItemList[(int)EN_COLUMN_INDEX.AXIS_NO, nIndexOfRow].Value		= strValue;
			grid_ItemList.Rows[nIndexOfRow].Selected		= false;
		}
		/// <summary>
		/// 2020.07.16 by twkang [ADD] 선택한 아이템을 지운다.
		/// </summary>
		private bool RemoveItemOnGrid()
		{
			if(SELECT_NONE == m_nIndexOfSelectedRows)
			{
				return false;
			}

			if(m_InstanceOfJog.RemoveJogReverseItem(m_nIndexOfSelectedItem))
			{
				grid_ItemList.Rows.RemoveAt(m_nIndexOfSelectedRows);

				if(null != grid_ItemList.CurrentCell)
				{
					grid_ItemList.Rows[grid_ItemList.CurrentRow.Index].Selected	= false;
				}
				return true;
			}

			return false;
		}
		/// <summary>
		/// 2020.08.12 by twkang [ADD] 선택 아이템, 라벨을 초기화한다.
		/// </summary>
		private void ResetControlValue()
		{
			m_nIndexOfSelectedItem				= -1;
			m_nIndexOfSelectedRows				= -1;

			lbl_Name.Text = string.Empty;
			lbl_Key.Text = "--";

			SetActiveControls(false);
		}
		/// <summary>
		/// 2020.08.12 by twkang [ADD] 버튼들의 활성화 상태를 설정한다.
		/// </summary>
		private void SetActiveControls(bool bEnable)
		{
			lbl_Name.Enabled = bEnable;
			lbl_Key.Enabled = bEnable;
			btn_Remove.Enabled			= bEnable;
		}
		/// <summary>
		/// 2020.07.16 by twkang [ADD] 라벨들의 값을 설정한다.
		/// </summary>
		private void SetConfigurationLabels()
		{
			string strValue				= string.Empty;

			lbl_Key.Text = grid_ItemList[(int)EN_COLUMN_INDEX.AXIS_NO, m_nIndexOfSelectedRows].Value.ToString();
			lbl_Name.Text = grid_ItemList[(int)EN_COLUMN_INDEX.AXIS_NAME, m_nIndexOfSelectedRows].Value.ToString();

			SetActiveControls(true);
		}
		/// <summary>
		/// 2021.07.28 by twkang [ADD] 버튼 클릭 확인
		/// </summary>
		private bool ConfirmButtonClick(string strActionName)
		{
			return m_InstanceOfMessageBox.ShowMessage(string.Format("Action : [{0}], This is a confirmation message", strActionName));
		}
		#endregion

		#region UI 이벤트
		/// <summary>
		/// 2020.07.16 by twkang [ADD] Grid 셀을 더블클릭 했을 경우
		/// </summary>
		private void Click_Item(object sender, DataGridViewCellEventArgs e)
		{
			int nRowindex		= e.RowIndex;
			int nColumnIndex	= e.ColumnIndex;

			if (nRowindex < 0 || nRowindex >= grid_ItemList.RowCount) { return; }

			int nIndex	= int.Parse(grid_ItemList[(int)EN_COLUMN_INDEX.INDEX, nRowindex].Value.ToString());

			m_nIndexOfSelectedRows = nRowindex;
			m_nIndexOfSelectedItem = nIndex;

			SetConfigurationLabels();

		}
		/// <summary>
		/// 2020.07.16 by twkang [ADD] Configuration 라벨 클릭 이벤트
		/// </summary>
		private void Click_Labels(object sender, EventArgs e)
		{
			Control ctrl			= sender as Control;

			string[] strArrName		= null;
			int[] nArrIndex			= null;
			int nValue				= -1;
			string strValue			= string.Empty;

			switch(ctrl.TabIndex)
			{
				case 0:
					m_InstanceOfMotion.GetListOfItem(ref nArrIndex);
					m_InstanceOfMotion.GetListOfName(ref strArrName);
					if (m_InstanceOfSelectionList.CreateForm(lbl_Name.Text, strArrName, nArrIndex, int.Parse(lbl_Key.Text)))
					{
						m_InstanceOfSelectionList.GetResult(ref nValue);
						if(SELECT_NONE != nValue)
						{
							m_InstanceOfMotion.GetMotionParameter(nValue, FrameOfSystem3.Config.ConfigMotion.EN_PARAM_MOTION.NAME, ref strValue);
						}
						
						if(m_InstanceOfJog.SetParameterReverse(m_nIndexOfSelectedItem, FrameOfSystem3.Config.ConfigJog.EN_PARAM_JOG_REVERSE.KEY, nValue)
							&& m_InstanceOfJog.SetParameterReverse(m_nIndexOfSelectedItem, FrameOfSystem3.Config.ConfigJog.EN_PARAM_JOG_REVERSE.NAME, strValue))
						{
							lbl_Key.Text = nValue.ToString();
							grid_ItemList[(int)EN_COLUMN_INDEX.AXIS_NO, m_nIndexOfSelectedRows].Value = nValue.ToString();
							lbl_Name.Text = strValue;
							grid_ItemList[(int)EN_COLUMN_INDEX.AXIS_NAME, m_nIndexOfSelectedRows].Value = strValue.ToString();
						}
					}
					break;
			}
		}
		/// <summary>
		/// 2020.07.16 by twkang [ADD] ADD 또는 REMOVE 버튼 클릭 이벤트
		/// </summary>
		private void Click_AddOrRemove(object sender, EventArgs e)
		{
			Control ctrl	= sender as Control;

			switch(ctrl.TabIndex)
			{
				case 0:	// ADD
					int nIndexOfItem		= m_InstanceOfJog.AddJogReverseItem();
					if(nIndexOfItem != SELECT_NONE)
					{
						AddItemOnGrid(nIndexOfItem);
					}
					break;
				case 1: // REMOVE
					if(ConfirmButtonClick("REMOVE"))
					{
						 RemoveItemOnGrid();
					}
					break;
			}
			ResetControlValue();
		}
		#endregion

	}
}
