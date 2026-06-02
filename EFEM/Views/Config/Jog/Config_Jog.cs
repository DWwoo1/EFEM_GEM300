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
	public partial class Config_Jog : UserControlForMainView.CustomView
	{
		public Config_Jog()
		{
			InitializeComponent();

			#region Instance
			m_InstanceOfJog											= FrameOfSystem3.Config.ConfigJog.GetInstance();
			m_InstanceOfMotion										= FrameOfSystem3.Config.ConfigMotion.GetInstance();
			m_InstanceOfKeyboard									= Functional.Form_Keyboard.GetInstance();
			m_InstanceOfSelectionList								= Functional.Form_SelectionList.GetInstance();
			m_InstanceOfMessageBox									= Functional.Form_MessageBox.GetInstance();
			#endregion
		}

		#region 상수
		private const int HEIGHT_OF_ROWS							= 30;
		private const int SELECT_NONE								= -1;

		#region ENUM
		private enum EN_COLUMN_INDEX
		{
			INDEX = 0,
			GROUP_NAME,
			AXIS_X,
			ENABLE_X,
			AXIS_Y,
			ENABLE_Y,
			AXIS_EX_X,
			ENABLE_EX_X,
			AXIS_EX_Y,
			ENABLE_EX_Y,
		}
		#endregion /ENUM

		private readonly Color c_clrTrue						= Color.DodgerBlue;
		private readonly Color c_clrFalse						= Color.White;		
		#endregion

		#region 변수

		#region Instance
		FrameOfSystem3.Config.ConfigMotion m_InstanceOfMotion		= null;
		FrameOfSystem3.Config.ConfigJog m_InstanceOfJog				= null;
		Functional.Form_Keyboard m_InstanceOfKeyboard				= null;
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
				m_dgViewJog.Rows[m_nIndexOfSelectedRows].Selected	= true;
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
			if(false == m_InstanceOfJog.GetListOfItem(ref m_nArrOfItem))
			{
				return;
			}

			int nCountOfItem		= m_nArrOfItem.Length;

			m_dgViewJog.Rows.Clear();

			for(int nIndex = 0; nIndex < nCountOfItem; ++nIndex)
			{
				AddItemOnGrid(m_nArrOfItem[nIndex]);
			}

		}	
		/// <summary>
		/// 2018.06.29 by yjlee [ADD] 괄호를 제거하고 정수로 캐스팅하여 반환한다.
		/// </summary>
		private int RemoveBrakets(string strNumber)
		{
			string strValue = strNumber.Remove(0, 2);

			int nValue = -1;

			try
			{
				nValue = int.Parse(strValue.Remove(strValue.Length - 2, 2));
			}
			catch (FormatException e)
			{
				System.Diagnostics.Debug.WriteLine(e.Message);

				return -1;
			}

			return nValue;
		}
		/// <summary>
		/// 2020.07.16 by twkang [ADD] 아이템을 GridView 에 추가한다.
		/// </summary>
		private void AddItemOnGrid(int nIndexOfItem)
		{
			int nIndexOfRow		= m_dgViewJog.Rows.Count;

			m_dgViewJog.Rows.Add();

			string strValue		= string.Empty;
			bool bResult		= false;

			m_dgViewJog.Rows[nIndexOfRow].Height					= HEIGHT_OF_ROWS;
			m_dgViewJog[(int)EN_COLUMN_INDEX.INDEX, nIndexOfRow].Value = nIndexOfItem.ToString();
			m_dgViewJog.Rows[nIndexOfRow].Cells[(int)EN_COLUMN_INDEX.INDEX].Style.BackColor = Color.Silver;
			m_dgViewJog.Rows[nIndexOfRow].Cells[(int)EN_COLUMN_INDEX.INDEX].Style.SelectionBackColor = Color.Silver;
			m_dgViewJog.Rows[nIndexOfRow].Cells[(int)EN_COLUMN_INDEX.INDEX].Style.SelectionForeColor = Color.Black;

			if(false == m_InstanceOfJog.GetParameter(nIndexOfItem, FrameOfSystem3.Config.ConfigJog.EN_PARAM_JOG.NAME, ref strValue))
			{
				strValue	= string.Empty;
			}
			m_dgViewJog[(int)EN_COLUMN_INDEX.GROUP_NAME, nIndexOfRow].Value = strValue;

			if (false == m_InstanceOfJog.GetParameter(nIndexOfItem, FrameOfSystem3.Config.ConfigJog.EN_PARAM_JOG.INDEX_LEFT_RIGHT, ref strValue))
			{
				strValue = string.Empty;
			}
			m_dgViewJog[(int)EN_COLUMN_INDEX.AXIS_X, nIndexOfRow].Value = strValue;

			if (false == m_InstanceOfJog.GetParameter(nIndexOfItem, FrameOfSystem3.Config.ConfigJog.EN_PARAM_JOG.INDEX_UP_DOWN, ref strValue))
			{
				strValue = string.Empty;
			}
			m_dgViewJog[(int)EN_COLUMN_INDEX.AXIS_Y, nIndexOfRow].Value = strValue;

			// 2022.11.01 by junho [ADD] extra axis
			if (false == m_InstanceOfJog.GetParameter(nIndexOfItem, FrameOfSystem3.Config.ConfigJog.EN_PARAM_JOG.INDEX_EXTRA_LEFT_RIGHT, ref strValue))
			{
				strValue = string.Empty;
			}
			m_dgViewJog[(int)EN_COLUMN_INDEX.AXIS_EX_X, nIndexOfRow].Value = strValue;

			if (false == m_InstanceOfJog.GetParameter(nIndexOfItem, FrameOfSystem3.Config.ConfigJog.EN_PARAM_JOG.INDEX_EXTRA_UP_DOWN, ref strValue))
			{
				strValue = string.Empty;
			}
			m_dgViewJog[(int)EN_COLUMN_INDEX.AXIS_EX_Y, nIndexOfRow].Value = strValue;

			if (false == m_InstanceOfJog.GetParameter(nIndexOfItem, FrameOfSystem3.Config.ConfigJog.EN_PARAM_JOG.ENABLE_LEFT_RIGHT, ref bResult)) bResult = false;
			m_dgViewJog.Rows[nIndexOfRow].Cells[(int)EN_COLUMN_INDEX.ENABLE_X].Style.BackColor = bResult ? c_clrTrue : c_clrFalse;
			m_dgViewJog.Rows[nIndexOfRow].Cells[(int)EN_COLUMN_INDEX.ENABLE_X].Style.SelectionBackColor = bResult ? c_clrTrue : c_clrFalse;

			if (false ==m_InstanceOfJog.GetParameter(nIndexOfItem, FrameOfSystem3.Config.ConfigJog.EN_PARAM_JOG.ENABLE_UP_DOWN, ref bResult)) bResult = false;
			m_dgViewJog.Rows[nIndexOfRow].Cells[(int)EN_COLUMN_INDEX.ENABLE_Y].Style.BackColor = bResult ? c_clrTrue : c_clrFalse;
			m_dgViewJog.Rows[nIndexOfRow].Cells[(int)EN_COLUMN_INDEX.ENABLE_Y].Style.SelectionBackColor = bResult ? c_clrTrue : c_clrFalse;

			// 2022.11.01 by junho [ADD] extra axis
			if (false ==m_InstanceOfJog.GetParameter(nIndexOfItem, FrameOfSystem3.Config.ConfigJog.EN_PARAM_JOG.ENABLE_EXTRA_LEFT_RIGHT, ref bResult)) bResult = false;
			m_dgViewJog.Rows[nIndexOfRow].Cells[(int)EN_COLUMN_INDEX.ENABLE_EX_X].Style.BackColor = bResult ? c_clrTrue : c_clrFalse;
			m_dgViewJog.Rows[nIndexOfRow].Cells[(int)EN_COLUMN_INDEX.ENABLE_EX_X].Style.SelectionBackColor = bResult ? c_clrTrue : c_clrFalse;

			if (false == m_InstanceOfJog.GetParameter(nIndexOfItem, FrameOfSystem3.Config.ConfigJog.EN_PARAM_JOG.ENABLE_EXTRA_UP_DOWN, ref bResult)) bResult = false;
			m_dgViewJog.Rows[nIndexOfRow].Cells[(int)EN_COLUMN_INDEX.ENABLE_EX_Y].Style.BackColor = bResult ? c_clrTrue : c_clrFalse;
			m_dgViewJog.Rows[nIndexOfRow].Cells[(int)EN_COLUMN_INDEX.ENABLE_EX_Y].Style.SelectionBackColor = bResult ? c_clrTrue : c_clrFalse;

			m_dgViewJog.Rows[nIndexOfRow].Selected		= false;
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

			if(m_InstanceOfJog.RemoveJogItem(m_nIndexOfSelectedItem))
			{
				m_dgViewJog.Rows.RemoveAt(m_nIndexOfSelectedRows);

				if(null != m_dgViewJog.CurrentCell)
				{
					m_dgViewJog.Rows[m_dgViewJog.CurrentRow.Index].Selected	= false;
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

			m_labelIndex.Text					= string.Empty;
			m_labelGroupName.Text				= string.Empty;

			m_labelAxis1.Text					= "--";
			m_labelAxis1.SubText				= "[ -1 ]";

			m_labelAxis2.Text					= "--";
			m_labelAxis2.SubText				= "[ -1 ]";

			m_radioUseAxisRL.Active				= false;
			m_radioUseAxisUD.Active				= false;

			m_labelExtraAxis1.Text = "--";
			m_labelExtraAxis1.SubText = "[ -1 ]";

			m_labelExtraAxis2.Text = "--";
			m_labelExtraAxis2.SubText = "[ -1 ]";

			m_radioUseExtraAxisRL.Active = false;
			m_radioUseExtraAxisUD.Active = false;

			SetActiveControls(false);
		}
		/// <summary>
		/// 2020.08.12 by twkang [ADD] 버튼들의 활성화 상태를 설정한다.
		/// </summary>
		private void SetActiveControls(bool bEnable)
		{
			m_radioUseAxisRL.Enabled	= bEnable;
			m_radioUseAxisUD.Enabled	= bEnable;
			
			m_labelAxis1.Enabled		= m_radioUseAxisRL.Active;
			m_labelAxis2.Enabled		= m_radioUseAxisUD.Active;

			m_radioUseExtraAxisRL.Enabled = bEnable;
			m_radioUseExtraAxisUD.Enabled = bEnable;

			m_labelExtraAxis1.Enabled = m_radioUseExtraAxisRL.Active;
			m_labelExtraAxis2.Enabled = m_radioUseExtraAxisUD.Active;

			m_labelGroupName.Enabled	= bEnable;

			m_btnRemove.Enabled			= bEnable;
		}
		/// <summary>
		/// 2020.07.16 by twkang [ADD] 라벨들의 값을 설정한다.
		/// </summary>
		private void SetConfigurationLabels()
		{
			string strValue				= string.Empty;
			int nItemNum				= -1;
			bool bResult				= false;

			m_labelIndex.Text = m_dgViewJog[(int)EN_COLUMN_INDEX.INDEX, m_nIndexOfSelectedRows].Value.ToString();
			m_labelGroupName.Text = m_dgViewJog[(int)EN_COLUMN_INDEX.GROUP_NAME, m_nIndexOfSelectedRows].Value.ToString();


			if (int.TryParse(m_dgViewJog[(int)EN_COLUMN_INDEX.AXIS_X, m_nIndexOfSelectedRows].Value.ToString(), out nItemNum))
			{
				if(false == m_InstanceOfMotion.GetMotionParameter(nItemNum, FrameOfSystem3.Config.ConfigMotion.EN_PARAM_MOTION.NAME, ref strValue))
				{
					strValue	= string.Empty;
				}
				m_labelAxis1.SubText = string.Format("[ {0} ]", nItemNum);
				m_labelAxis1.Text = strValue;
			}
			else
			{
				m_labelAxis1.Text = "--";
				m_labelAxis1.SubText = "[ -1 ]";
			}
			if (int.TryParse(m_dgViewJog[(int)EN_COLUMN_INDEX.AXIS_Y, m_nIndexOfSelectedRows].Value.ToString(), out nItemNum))
			{
				if(false == m_InstanceOfMotion.GetMotionParameter(nItemNum, FrameOfSystem3.Config.ConfigMotion.EN_PARAM_MOTION.NAME, ref strValue))
				{
					strValue	= string.Empty;
				}
				m_labelAxis2.SubText = string.Format("[ {0} ]", nItemNum);
				m_labelAxis2.Text = strValue;
			}
			else
			{
				m_labelAxis2.Text = "--";
				m_labelAxis2.SubText = "[ -1 ]";
			}

			// 2022.11.01 by junho [ADD] extra axis
			if (int.TryParse(m_dgViewJog[(int)EN_COLUMN_INDEX.AXIS_EX_X, m_nIndexOfSelectedRows].Value.ToString(), out nItemNum))
			{
				if (false == m_InstanceOfMotion.GetMotionParameter(nItemNum, FrameOfSystem3.Config.ConfigMotion.EN_PARAM_MOTION.NAME, ref strValue))
				{
					strValue = string.Empty;
				}
				m_labelExtraAxis1.SubText = string.Format("[ {0} ]", nItemNum);
				m_labelExtraAxis1.Text = strValue;
			}
			else
			{
				m_labelExtraAxis1.Text = "--";
				m_labelExtraAxis1.SubText = "[ -1 ]";
			}
			if (int.TryParse(m_dgViewJog[(int)EN_COLUMN_INDEX.AXIS_EX_Y, m_nIndexOfSelectedRows].Value.ToString(), out nItemNum))
			{
				if (false == m_InstanceOfMotion.GetMotionParameter(nItemNum, FrameOfSystem3.Config.ConfigMotion.EN_PARAM_MOTION.NAME, ref strValue))
				{
					strValue = string.Empty;
				}
				m_labelExtraAxis2.SubText = string.Format("[ {0} ]", nItemNum);
				m_labelExtraAxis2.Text = strValue;
			}
			else
			{
				m_labelExtraAxis2.Text = "--";
				m_labelExtraAxis2.SubText = "[ -1 ]";
			}


			if (false == m_InstanceOfJog.GetParameter(m_nIndexOfSelectedItem, FrameOfSystem3.Config.ConfigJog.EN_PARAM_JOG.ENABLE_LEFT_RIGHT, ref bResult))
			{
				bResult = false;
			}
			m_radioUseAxisRL.Active = bResult;

			if (false == m_InstanceOfJog.GetParameter(m_nIndexOfSelectedItem, FrameOfSystem3.Config.ConfigJog.EN_PARAM_JOG.ENABLE_UP_DOWN, ref bResult))
			{
				bResult = false;
			}
			m_radioUseAxisUD.Active = bResult;

			// 2022.11.01 by junho [ADD] extra axis
			if (false == m_InstanceOfJog.GetParameter(m_nIndexOfSelectedItem, FrameOfSystem3.Config.ConfigJog.EN_PARAM_JOG.ENABLE_EXTRA_LEFT_RIGHT, ref bResult))
			{
				bResult = false;
			}
			m_radioUseExtraAxisRL.Active = bResult;

			if (false == m_InstanceOfJog.GetParameter(m_nIndexOfSelectedItem, FrameOfSystem3.Config.ConfigJog.EN_PARAM_JOG.ENABLE_EXTRA_UP_DOWN, ref bResult))
			{
				bResult = false;
			}
			m_radioUseExtraAxisUD.Active		= bResult;

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

			if (nRowindex < 0 || nRowindex >= m_dgViewJog.RowCount) { return; }

			int nIndex = int.Parse(m_dgViewJog[(int)EN_COLUMN_INDEX.INDEX, nRowindex].Value.ToString());

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
				case 0: // GROUP NAME
					if(m_InstanceOfKeyboard.CreateForm(m_labelGroupName.Text))
					{
						m_InstanceOfKeyboard.GetResult(ref strValue);
						if(m_InstanceOfJog.SetParameter(m_nIndexOfSelectedItem, FrameOfSystem3.Config.ConfigJog.EN_PARAM_JOG.NAME, strValue))
						{
							m_labelGroupName.Text	= strValue;
							m_dgViewJog[(int)EN_COLUMN_INDEX.GROUP_NAME, m_nIndexOfSelectedRows].Value = strValue;
						}
					}
					break;
				case 1:	// AXIS RIGHT,LEFT
					m_InstanceOfMotion.GetListOfItem(ref nArrIndex);
					m_InstanceOfMotion.GetListOfName(ref strArrName);
					if(m_InstanceOfSelectionList.CreateForm(m_lblAxis1.Text, strArrName, nArrIndex, RemoveBrakets(m_labelAxis1.SubText)))
					{
						m_InstanceOfSelectionList.GetResult(ref nValue);
						if(SELECT_NONE != nValue)
						{
							m_InstanceOfMotion.GetMotionParameter(nValue, FrameOfSystem3.Config.ConfigMotion.EN_PARAM_MOTION.NAME, ref strValue);
						}
						
						if(m_InstanceOfJog.SetParameter(m_nIndexOfSelectedItem, FrameOfSystem3.Config.ConfigJog.EN_PARAM_JOG.INDEX_LEFT_RIGHT, nValue))
						{
							m_labelAxis1.Text		= strValue;
							m_labelAxis1.SubText	= string.Format("[ {0} ]", nValue);
							m_dgViewJog[(int)EN_COLUMN_INDEX.AXIS_X, m_nIndexOfSelectedRows].Value = nValue.ToString();
						}
					}
					break;
				case 2:	// AXIS UP, DOWN
					m_InstanceOfMotion.GetListOfItem(ref nArrIndex);
					m_InstanceOfMotion.GetListOfName(ref strArrName);
					if(m_InstanceOfSelectionList.CreateForm(m_lblAxis2.Text, strArrName, nArrIndex, RemoveBrakets(m_labelAxis2.SubText), false))
					{
						m_InstanceOfSelectionList.GetResult(ref nValue);
						if (m_InstanceOfMotion.GetMotionParameter(nValue, FrameOfSystem3.Config.ConfigMotion.EN_PARAM_MOTION.NAME, ref strValue)
							&& m_InstanceOfJog.SetParameter(m_nIndexOfSelectedItem, FrameOfSystem3.Config.ConfigJog.EN_PARAM_JOG.INDEX_UP_DOWN, nValue))
						{
							m_labelAxis2.Text = strValue;
							m_labelAxis2.SubText = string.Format("[ {0} ]", nValue);
							m_dgViewJog[(int)EN_COLUMN_INDEX.AXIS_Y, m_nIndexOfSelectedRows].Value = nValue.ToString();
						}
					}
					break;
				// 2022.11.01 by junho [ADD] extra axis
				case 3:	// EXTRA AXIS RIGHT,LEFT
					m_InstanceOfMotion.GetListOfItem(ref nArrIndex);
					m_InstanceOfMotion.GetListOfName(ref strArrName);
					if (m_InstanceOfSelectionList.CreateForm(m_lblExtraAxis1.Text, strArrName, nArrIndex, RemoveBrakets(m_labelExtraAxis1.SubText)))
					{
						m_InstanceOfSelectionList.GetResult(ref nValue);
						if (SELECT_NONE != nValue)
						{
							m_InstanceOfMotion.GetMotionParameter(nValue, FrameOfSystem3.Config.ConfigMotion.EN_PARAM_MOTION.NAME, ref strValue);
						}

						if (m_InstanceOfJog.SetParameter(m_nIndexOfSelectedItem, FrameOfSystem3.Config.ConfigJog.EN_PARAM_JOG.INDEX_EXTRA_LEFT_RIGHT, nValue))
						{
							m_labelExtraAxis1.Text = strValue;
							m_labelExtraAxis1.SubText = string.Format("[ {0} ]", nValue);
							m_dgViewJog[(int)EN_COLUMN_INDEX.AXIS_EX_X, m_nIndexOfSelectedRows].Value = nValue.ToString();
						}
					}
					break;
				case 4:	// EXTRA AXIS UP, DOWN
					m_InstanceOfMotion.GetListOfItem(ref nArrIndex);
					m_InstanceOfMotion.GetListOfName(ref strArrName);
					if (m_InstanceOfSelectionList.CreateForm(m_lblExtraAxis2.Text, strArrName, nArrIndex, RemoveBrakets(m_labelExtraAxis2.SubText), false))
					{
						m_InstanceOfSelectionList.GetResult(ref nValue);
						if (m_InstanceOfMotion.GetMotionParameter(nValue, FrameOfSystem3.Config.ConfigMotion.EN_PARAM_MOTION.NAME, ref strValue)
							&& m_InstanceOfJog.SetParameter(m_nIndexOfSelectedItem, FrameOfSystem3.Config.ConfigJog.EN_PARAM_JOG.INDEX_EXTRA_UP_DOWN, nValue))
						{
							m_labelExtraAxis2.Text = strValue;
							m_labelExtraAxis2.SubText = string.Format("[ {0} ]", nValue);
							m_dgViewJog[(int)EN_COLUMN_INDEX.AXIS_EX_Y, m_nIndexOfSelectedRows].Value = nValue.ToString();
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
					int nIndexOfItem		= m_InstanceOfJog.AddJogItem();
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
		/// <summary>
		/// 2020.07.16 by twkang [ADD] Axis Option 버튼 클릭 이벤트
		/// </summary>
		private void Click_UseOption(object sender, EventArgs e)
		{
			Control ctrl	= sender as Control;
			bool bEnable	= false;
			switch(ctrl.TabIndex)
			{
				case 0: // Right, Left
					bEnable					= m_radioUseAxisRL.Active;
					m_labelAxis1.Enabled	= bEnable;

					if(m_InstanceOfJog.SetParameter(m_nIndexOfSelectedItem, FrameOfSystem3.Config.ConfigJog.EN_PARAM_JOG.ENABLE_LEFT_RIGHT, bEnable))
					{
						m_dgViewJog.Rows[m_nIndexOfSelectedRows].Cells[(int)EN_COLUMN_INDEX.ENABLE_X].Style.BackColor			= bEnable ? c_clrTrue : c_clrFalse;
						m_dgViewJog.Rows[m_nIndexOfSelectedRows].Cells[(int)EN_COLUMN_INDEX.ENABLE_X].Style.SelectionBackColor = bEnable ? c_clrTrue : c_clrFalse;
					}
					break;
				case 1: // Up, Down
					bEnable					= m_radioUseAxisUD.Active;
					m_labelAxis2.Enabled	= bEnable;

					if (m_InstanceOfJog.SetParameter(m_nIndexOfSelectedItem, FrameOfSystem3.Config.ConfigJog.EN_PARAM_JOG.ENABLE_UP_DOWN, bEnable))
					{
						m_dgViewJog.Rows[m_nIndexOfSelectedRows].Cells[(int)EN_COLUMN_INDEX.ENABLE_Y].Style.BackColor = bEnable ? c_clrTrue : c_clrFalse;
						m_dgViewJog.Rows[m_nIndexOfSelectedRows].Cells[(int)EN_COLUMN_INDEX.ENABLE_Y].Style.SelectionBackColor = bEnable ? c_clrTrue : c_clrFalse;
					}
					break;
				case 2: // Extra Right, Left
					bEnable = m_radioUseExtraAxisRL.Active;
					m_labelExtraAxis1.Enabled = bEnable;

					if (m_InstanceOfJog.SetParameter(m_nIndexOfSelectedItem, FrameOfSystem3.Config.ConfigJog.EN_PARAM_JOG.ENABLE_EXTRA_LEFT_RIGHT, bEnable))
					{
						m_dgViewJog.Rows[m_nIndexOfSelectedRows].Cells[(int)EN_COLUMN_INDEX.ENABLE_EX_X].Style.BackColor = bEnable ? c_clrTrue : c_clrFalse;
						m_dgViewJog.Rows[m_nIndexOfSelectedRows].Cells[(int)EN_COLUMN_INDEX.ENABLE_EX_X].Style.SelectionBackColor = bEnable ? c_clrTrue : c_clrFalse;
					}
					break;
				case 3: // Extra Up, Down
					bEnable = m_radioUseExtraAxisUD.Active;
					m_labelExtraAxis2.Enabled = bEnable;

					if (m_InstanceOfJog.SetParameter(m_nIndexOfSelectedItem, FrameOfSystem3.Config.ConfigJog.EN_PARAM_JOG.ENABLE_EXTRA_UP_DOWN, bEnable))
					{
						m_dgViewJog.Rows[m_nIndexOfSelectedRows].Cells[(int)EN_COLUMN_INDEX.ENABLE_EX_Y].Style.BackColor = bEnable ? c_clrTrue : c_clrFalse;
						m_dgViewJog.Rows[m_nIndexOfSelectedRows].Cells[(int)EN_COLUMN_INDEX.ENABLE_EX_Y].Style.SelectionBackColor = bEnable ? c_clrTrue : c_clrFalse;
					}
					break;
			}
		}
		#endregion

	}
}
