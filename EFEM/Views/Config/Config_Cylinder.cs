using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using System.Diagnostics;
using System.Threading;

using Define.DefineConstant;

namespace FrameOfSystem3.Views.Config
{
    public partial class Config_Cylinder : UserControlForMainView.CustomView
	{
		public Config_Cylinder()
		{
			InitializeComponent();

			#region Instance
			m_InstanceOfDigitalIO = FrameOfSystem3.Config.ConfigDigitalIO.GetInstance();
			m_InstanceOfCylinder = FrameOfSystem3.Config.ConfigCylinder.GetInstance();
			m_InstanceOfSelectionList = Functional.Form_SelectionList.GetInstance();
			m_InstanceOfKeyboard = Functional.Form_Keyboard.GetInstance();
			m_InstanceOfCalculator = Functional.Form_Calculator.GetInstance();
			m_InstanceOfMessageBox = Functional.Form_MessageBox.GetInstance();
			#endregion

			m_tabClicked				= m_tabForward;

			#region MappingTable
			m_DicForMonitoringMode.Clear();
			foreach (MONITORING_MODE en in Enum.GetValues(typeof(MONITORING_MODE)))
			{
				m_DicForMonitoringMode.Add((int)en, en.ToString());
			}
			#endregion
		}

		#region 상수
		private const int HEIGHT_OF_ROWS				= 30;

		#region COLUMN_INDEX
		private const int COLUMN_INDEX_OF_INDEX			= 0;
		private const int COLUMN_INDEX_OF_NAME			= 1;
		private const int COLUMN_INDEX_OF_MONITORING	= 2;
		private const int COLUMN_INDEX_OF_STATE			= 3;
		private const int COLUMN_INDEX_OF_FWIN			= 4;
		private const int COLUMN_INDEX_OF_FWINPULSE		= 5;
		private const int COLUMN_INDEX_OF_FWOUT			= 6;
		private const int COLUMN_INDEX_OF_FWOUTPULSE	= 7;
		private const int COLUMN_INDEX_OF_BWIN			= 8;
		private const int COLUMN_INDEX_OF_BWINPULSE		= 9;
		private const int COLUMN_INDEX_OF_BWOUT			= 10;
		private const int COLUMN_INDEX_OF_BWOUTPULSE	= 11;

		private const int COLUMN_INDEX_OF_IO_VALUE		= 2;
		#endregion

		private const int SELECT_NONE					= -1;

		private readonly string MAX_OF_PARAM			= "999999";
		private readonly string MIN_OF_PARAM			= "0";

		// 미사용?
		//private readonly string LEFT_ARROW				= "◀";
		//private readonly string RIGHT_ARROW				= "▶";

		private readonly Color c_clrTrue			= Color.DodgerBlue;
		private readonly Color c_clrFalse			= Color.White;		
		#endregion

		#region 열거형
		private enum MONITORING_MODE
		{
			ENABLE = 0,
			DISABLE_INPUT = 1,
			DISABLE_OUTPUT = 2,
			DISABLE = 3,
		}
		private enum EN_VIEW_TAB
		{
			FORWARD,
			BACKWARD,
		}
		#endregion

		#region 변수
		private int m_nIndexOfSelectedRows								= SELECT_NONE;
		private	int m_nIndexOfSelectedItem								= SELECT_NONE;

		private EN_VIEW_TAB m_enTab										= EN_VIEW_TAB.FORWARD;
		private Sys3Controls.Sys3button m_tabClicked					= null;
		Dictionary<int, string> m_DicForMonitoringMode					= new Dictionary<int, string>();

		// 포워드 백워드 메시지 OK확인여부
		private bool m_bNeedToConfirm									= true;

		#region Instance
		FrameOfSystem3.Config.ConfigDigitalIO m_InstanceOfDigitalIO			= null;
		FrameOfSystem3.Config.ConfigCylinder m_InstanceOfCylinder			= null;
		Functional.Form_SelectionList m_InstanceOfSelectionList				= null;
		Functional.Form_Keyboard m_InstanceOfKeyboard						= null;
		Functional.Form_Calculator m_InstanceOfCalculator					= null;
		Functional.Form_MessageBox m_InstanceOfMessageBox					= null;
		#endregion

		// 2024.03.08 by junho [ADD] filtering 기능 추가
		string _filterKeyword = string.Empty;
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
				m_dgViewCylinder.Rows[m_nIndexOfSelectedRows].Selected		= true;
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
			UpdateCylinderList();
        }
        #endregion

		#region 내부인터페이스
		/// <summary>
		/// 2020.06.09 by twkang [ADD] Cylinder Item 들의 파라미터 값을 업데이트한다.
		/// </summary>
		private void UpdateForm()
		{
			int[] arIndex		= null;

			if (false == m_InstanceOfCylinder.GetListOfItem(ref arIndex))
			{
				return;
			}

			int nCountOfItem = arIndex.Length;

			m_dgViewCylinder.Rows.Clear();

			for(int i = 0; i < nCountOfItem; ++i)
			{
				AddItemOnGrid(arIndex[i]);
			}
		}
		/// <summary>
		/// 2020.07.13 by twkang [ADD] 아이템을 GridView 에 추가한다.
		/// </summary>
		private void AddItemOnGrid(int nIndexOfItem)
		{
			int nIndexOfRow		= m_dgViewCylinder.Rows.Count;

			// 2024.03.08 by junho [ADD] filtering 기능 추가
			string itemName = string.Empty;
			if (false == m_InstanceOfCylinder.GetParameter(nIndexOfItem, FrameOfSystem3.Config.ConfigCylinder.EN_PARAM_CYLINDER.NAME, ref itemName))
				itemName = string.Empty;

			// 2024.06.25 by junho [MOD] 대소문자 구분하지 않도록 변경
			//if (_filterKeyword != string.Empty && false == itemName.Contains(_filterKeyword))
			if (_filterKeyword != string.Empty && itemName.IndexOf(_filterKeyword, StringComparison.OrdinalIgnoreCase) < 0)
				return;

			m_dgViewCylinder.Rows.Add();

			string strValue		= string.Empty;
			int nValue			= -1;

			m_dgViewCylinder.Rows[nIndexOfRow].Height						= HEIGHT_OF_ROWS;
			m_dgViewCylinder[COLUMN_INDEX_OF_INDEX, nIndexOfRow].Value		= nIndexOfItem.ToString();

			m_dgViewCylinder.Rows[nIndexOfRow].Cells[COLUMN_INDEX_OF_INDEX].Style.BackColor				= Color.Silver;
			m_dgViewCylinder.Rows[nIndexOfRow].Cells[COLUMN_INDEX_OF_INDEX].Style.SelectionBackColor	= Color.Silver;
			m_dgViewCylinder.Rows[nIndexOfRow].Cells[COLUMN_INDEX_OF_INDEX].Style.SelectionForeColor	= Color.Black;

			// 2024.03.08 by junho [MOD] filtering 기능 추가
			//if (false == m_InstanceOfCylinder.GetParameter(nIndexOfItem, FrameOfSystem3.Config.ConfigCylinder.EN_PARAM_CYLINDER.NAME, ref strValue))
			//{
			//	strValue = Define.DefineConstant.Common.NONE;
			//}
			//m_dgViewCylinder[COLUMN_INDEX_OF_NAME, nIndexOfRow].Value		= strValue;
			m_dgViewCylinder[COLUMN_INDEX_OF_NAME, nIndexOfRow].Value = itemName;

			if (m_InstanceOfCylinder.GetParameter(nIndexOfItem, FrameOfSystem3.Config.ConfigCylinder.EN_PARAM_CYLINDER.MONITORING, ref nValue))
			{
				if (m_DicForMonitoringMode.ContainsKey(nValue))
				{
					strValue = m_DicForMonitoringMode[nValue];
				}
				else
				{
					strValue = Define.DefineConstant.Common.NONE;
				}
			}
			m_dgViewCylinder[COLUMN_INDEX_OF_MONITORING, nIndexOfRow].Value = strValue;
			m_dgViewCylinder.Rows[nIndexOfRow].Selected			= false;
		}
		/// <summary>
		/// 2020.07.13 by twkang [ADD] 선택한 아이템을 지운다.
		/// </summary>
		private bool RemoveItemOnGrid()
		{
			if(null == m_dgViewCylinder.CurrentCell)
			{
				return false;
			}

			int nRowIndex	= m_dgViewCylinder.CurrentCell.RowIndex;
			int nItemIndex	= int.Parse(m_dgViewCylinder[COLUMN_INDEX_OF_INDEX, nRowIndex].Value.ToString());

			if(m_InstanceOfCylinder.RemoveItem(nItemIndex))
			{
				m_dgViewCylinder.Rows.RemoveAt(nRowIndex);

				if(null != m_dgViewCylinder.CurrentCell)
				{
					m_dgViewCylinder.Rows[m_dgViewCylinder.CurrentRow.Index].Selected	= false;
				}
				return true;
			}
			return false;
		}
		/// <summary>
		/// 2020.06.09 by twkang [ADD] 주기적으로 Cylinder 의 특정 Parameter 값을 갱신한다.
		/// </summary>
		private void UpdateCylinderList()
		{
			int[] arIndex	= null;

			if (false == m_InstanceOfCylinder.GetListOfItem(ref arIndex))
			{
				return;
			}

			int nCountOfItem	= arIndex.Length;

			// 2024.03.13 by junho [MOD] filtering 기능으로 인한 변경
			#region 기존 소스
			//for (int i = 0; i < nCountOfItem; ++i)
			//{
			//	int[] arFWInput = null;
			//	int[] arFWOutput = null;
			//	int[] arBWInput = null;
			//	int[] arBWOutput = null;

			//	int nFWInputCount = 0;
			//	int nFWOutputCount = 0;
			//	int nBWInputCount = 0;
			//	int nBWOutputCount = 0;

			//	string strState = string.Empty;
			//	string strFWInput = string.Empty;
			//	string strFWOutput = string.Empty;
			//	string strBWInput = string.Empty;
			//	string strBWOutput = string.Empty;

			//	bool bResult = false;

			//	m_InstanceOfCylinder.GetCylinderState(arIndex[i], ref strState);
			//	m_dgViewCylinder[COLUMN_INDEX_OF_STATE, i].Value = strState;

			//	#region GridView
			//	if (m_InstanceOfCylinder.GetParameter(arIndex[i], FrameOfSystem3.Config.ConfigCylinder.EN_PARAM_LIST_IO.FORWARD_IN, ref nFWInputCount, ref arFWInput))
			//	{
			//		bResult = true;
			//		for (int nIndex = 0; nIndex < nFWInputCount; ++nIndex)
			//		{
			//			bResult &= m_InstanceOfDigitalIO.ReadItem(true, arFWInput[nIndex]);
			//		}
			//		m_dgViewCylinder[COLUMN_INDEX_OF_FWIN, i].Value = string.Join(", ", arFWInput);
			//		m_dgViewCylinder.Rows[i].Cells[COLUMN_INDEX_OF_FWINPULSE].Style.BackColor = bResult ? c_clrTrue : c_clrFalse;
			//		m_dgViewCylinder.Rows[i].Cells[COLUMN_INDEX_OF_FWINPULSE].Style.SelectionBackColor = bResult ? c_clrTrue : c_clrFalse;
			//	}
			//	else
			//	{
			//		m_dgViewCylinder[COLUMN_INDEX_OF_FWIN, i].Value = string.Empty;
			//		m_dgViewCylinder.Rows[i].Cells[COLUMN_INDEX_OF_FWINPULSE].Style.BackColor = bResult ? c_clrTrue : c_clrFalse;
			//		m_dgViewCylinder.Rows[i].Cells[COLUMN_INDEX_OF_FWINPULSE].Style.SelectionBackColor = bResult ? c_clrTrue : c_clrFalse;
			//	}

			//	if (m_InstanceOfCylinder.GetParameter(arIndex[i], FrameOfSystem3.Config.ConfigCylinder.EN_PARAM_LIST_IO.FORWARD_OUT, ref nFWOutputCount, ref arFWOutput))
			//	{
			//		bResult = true;
			//		for (int nIndex = 0; nIndex < nFWOutputCount; ++nIndex)
			//		{
			//			bResult &= m_InstanceOfDigitalIO.ReadItem(false, arFWOutput[nIndex]);
			//		}
			//		m_dgViewCylinder[COLUMN_INDEX_OF_FWOUT, i].Value = string.Join(", ", arFWOutput);
			//		m_dgViewCylinder.Rows[i].Cells[COLUMN_INDEX_OF_FWOUTPULSE].Style.BackColor = bResult ? c_clrTrue : c_clrFalse;
			//		m_dgViewCylinder.Rows[i].Cells[COLUMN_INDEX_OF_FWOUTPULSE].Style.SelectionBackColor = bResult ? c_clrTrue : c_clrFalse;
			//	}
			//	else
			//	{
			//		m_dgViewCylinder[COLUMN_INDEX_OF_FWOUT, i].Value = string.Empty;
			//		m_dgViewCylinder.Rows[i].Cells[COLUMN_INDEX_OF_FWOUTPULSE].Style.BackColor = c_clrFalse;
			//		m_dgViewCylinder.Rows[i].Cells[COLUMN_INDEX_OF_FWOUTPULSE].Style.SelectionBackColor = c_clrFalse;
			//	}

			//	if (m_InstanceOfCylinder.GetParameter(arIndex[i], FrameOfSystem3.Config.ConfigCylinder.EN_PARAM_LIST_IO.BACKWARD_IN, ref nBWInputCount, ref arBWInput))
			//	{
			//		bResult = true;
			//		for (int nIndex = 0; nIndex < nBWInputCount; ++nIndex)
			//		{
			//			bResult &= m_InstanceOfDigitalIO.ReadItem(true, arBWInput[nIndex]);
			//		}
			//		m_dgViewCylinder[COLUMN_INDEX_OF_BWIN, i].Value = string.Join(", ", arBWInput);
			//		m_dgViewCylinder.Rows[i].Cells[COLUMN_INDEX_OF_BWINPULSE].Style.BackColor = bResult ? c_clrTrue : c_clrFalse;
			//		m_dgViewCylinder.Rows[i].Cells[COLUMN_INDEX_OF_BWINPULSE].Style.SelectionBackColor = bResult ? c_clrTrue : c_clrFalse;
			//	}
			//	else
			//	{
			//		m_dgViewCylinder[COLUMN_INDEX_OF_BWIN, i].Value = string.Empty;
			//		m_dgViewCylinder.Rows[i].Cells[COLUMN_INDEX_OF_BWINPULSE].Style.BackColor = c_clrFalse;
			//		m_dgViewCylinder.Rows[i].Cells[COLUMN_INDEX_OF_BWINPULSE].Style.SelectionBackColor = c_clrFalse;
			//	}

			//	if (m_InstanceOfCylinder.GetParameter(arIndex[i], FrameOfSystem3.Config.ConfigCylinder.EN_PARAM_LIST_IO.BACKWARD_OUT, ref nBWOutputCount, ref arBWOutput))
			//	{
			//		bResult = true;
			//		for (int nIndex = 0; nIndex < nBWOutputCount; ++nIndex)
			//		{
			//			bResult &= m_InstanceOfDigitalIO.ReadItem(false, arBWOutput[nIndex]);
			//		}
			//		m_dgViewCylinder[COLUMN_INDEX_OF_BWOUT, i].Value = string.Join(",", arBWOutput);
			//		m_dgViewCylinder.Rows[i].Cells[COLUMN_INDEX_OF_BWOUTPULSE].Style.BackColor = bResult ? c_clrTrue : c_clrFalse;
			//		m_dgViewCylinder.Rows[i].Cells[COLUMN_INDEX_OF_BWOUTPULSE].Style.SelectionBackColor = bResult ? c_clrTrue : c_clrFalse;
			//	}
			//	else
			//	{
			//		m_dgViewCylinder[COLUMN_INDEX_OF_BWOUT, i].Value = string.Empty;
			//		m_dgViewCylinder.Rows[i].Cells[COLUMN_INDEX_OF_BWOUTPULSE].Style.BackColor = c_clrFalse;
			//		m_dgViewCylinder.Rows[i].Cells[COLUMN_INDEX_OF_BWOUTPULSE].Style.SelectionBackColor = c_clrFalse;
			//	}
			//	#endregion

			//	if (m_nIndexOfSelectedRows == i)
			//	{
			//		m_labelState.Text = strState;

			//		switch (m_enTab)
			//		{
			//			case EN_VIEW_TAB.FORWARD:

			//				for (int nindex = 0, nEnd = nFWInputCount; nindex < nEnd; ++nindex)
			//				{
			//					bool bValue = m_InstanceOfDigitalIO.ReadItem(true, arFWInput[nindex]);
			//					m_dgViewINPUT.Rows[nindex].Cells[COLUMN_INDEX_OF_IO_VALUE].Style.BackColor = bValue ? c_clrTrue : c_clrFalse;
			//				}


			//				for (int nindex = 0, nEnd = nFWOutputCount; nindex < nEnd; ++nindex)
			//				{
			//					bool bValue = m_InstanceOfDigitalIO.ReadItem(false, arFWOutput[nindex]);
			//					m_dgViewOutput.Rows[nindex].Cells[COLUMN_INDEX_OF_IO_VALUE].Style.BackColor = bValue ? c_clrTrue : c_clrFalse;
			//				}

			//				break;
			//			case EN_VIEW_TAB.BACKWARD:

			//				for (int nindex = 0, nEnd = nBWInputCount; nindex < nEnd; ++nindex)
			//				{
			//					bool bValue = m_InstanceOfDigitalIO.ReadItem(true, arBWInput[nindex]);
			//					m_dgViewINPUT.Rows[nindex].Cells[COLUMN_INDEX_OF_IO_VALUE].Style.BackColor = bValue ? c_clrTrue : c_clrFalse;
			//				}


			//				for (int nindex = 0, nEnd = nBWOutputCount; nindex < nEnd; ++nindex)
			//				{
			//					bool bValue = m_InstanceOfDigitalIO.ReadItem(false, arBWOutput[nindex]);
			//					m_dgViewOutput.Rows[nindex].Cells[COLUMN_INDEX_OF_IO_VALUE].Style.BackColor = bValue ? c_clrTrue : c_clrFalse;
			//				}

			//				break;
			//		}
			//	}
			//}
			#endregion /기존 소스
			int itemIndex;
			for (int row = 0; row < m_dgViewCylinder.Rows.Count; ++row)
			{
				if (false == int.TryParse(m_dgViewCylinder[COLUMN_INDEX_OF_INDEX, row].Value.ToString(), out itemIndex))
					return;

				int[] arFWInput			= null;
				int[] arFWOutput		= null;
				int[] arBWInput			= null;
				int[] arBWOutput		= null;

				int nFWInputCount				= 0;
				int nFWOutputCount				= 0;
				int nBWInputCount				= 0;
				int nBWOutputCount				= 0;

				string strState			= string.Empty;
				string strFWInput		= string.Empty;
				string strFWOutput		= string.Empty;
				string strBWInput		= string.Empty;
				string strBWOutput		= string.Empty;

				bool bResult			= false;

				m_InstanceOfCylinder.GetCylinderState(arIndex[itemIndex], ref strState);
				m_dgViewCylinder[COLUMN_INDEX_OF_STATE, row].Value = strState;

				#region GridView
				if (m_InstanceOfCylinder.GetParameter(arIndex[itemIndex], FrameOfSystem3.Config.ConfigCylinder.EN_PARAM_LIST_IO.FORWARD_IN, ref nFWInputCount, ref arFWInput))
				{
					bResult = true;
					for (int nIndex = 0; nIndex < nFWInputCount; ++nIndex)
					{
						bResult &= m_InstanceOfDigitalIO.ReadItem(true, arFWInput[nIndex]);
					}
					m_dgViewCylinder[COLUMN_INDEX_OF_FWIN, row].Value		= string.Join(", ", arFWInput);
					m_dgViewCylinder.Rows[row].Cells[COLUMN_INDEX_OF_FWINPULSE].Style.BackColor = bResult ? c_clrTrue : c_clrFalse;
					m_dgViewCylinder.Rows[row].Cells[COLUMN_INDEX_OF_FWINPULSE].Style.SelectionBackColor = bResult ? c_clrTrue : c_clrFalse;
				}
				else
				{
					m_dgViewCylinder[COLUMN_INDEX_OF_FWIN, row].Value		= string.Empty;
					m_dgViewCylinder.Rows[row].Cells[COLUMN_INDEX_OF_FWINPULSE].Style.BackColor			= bResult ? c_clrTrue : c_clrFalse;
					m_dgViewCylinder.Rows[row].Cells[COLUMN_INDEX_OF_FWINPULSE].Style.SelectionBackColor	= bResult ? c_clrTrue : c_clrFalse;
				}

				if (m_InstanceOfCylinder.GetParameter(arIndex[itemIndex], FrameOfSystem3.Config.ConfigCylinder.EN_PARAM_LIST_IO.FORWARD_OUT, ref nFWOutputCount, ref arFWOutput))
				{
					bResult = true;
					for (int nIndex = 0; nIndex < nFWOutputCount; ++nIndex)
					{
						bResult &= m_InstanceOfDigitalIO.ReadItem(false, arFWOutput[nIndex]);
					}
					m_dgViewCylinder[COLUMN_INDEX_OF_FWOUT, row].Value = string.Join(", ", arFWOutput);
					m_dgViewCylinder.Rows[row].Cells[COLUMN_INDEX_OF_FWOUTPULSE].Style.BackColor			= bResult ? c_clrTrue : c_clrFalse;
					m_dgViewCylinder.Rows[row].Cells[COLUMN_INDEX_OF_FWOUTPULSE].Style.SelectionBackColor	= bResult ? c_clrTrue : c_clrFalse;
				}
				else
				{
					m_dgViewCylinder[COLUMN_INDEX_OF_FWOUT, row].Value	= string.Empty;
					m_dgViewCylinder.Rows[row].Cells[COLUMN_INDEX_OF_FWOUTPULSE].Style.BackColor			= c_clrFalse;
					m_dgViewCylinder.Rows[row].Cells[COLUMN_INDEX_OF_FWOUTPULSE].Style.SelectionBackColor	= c_clrFalse;
				}

				if (m_InstanceOfCylinder.GetParameter(arIndex[itemIndex], FrameOfSystem3.Config.ConfigCylinder.EN_PARAM_LIST_IO.BACKWARD_IN, ref nBWInputCount, ref arBWInput))
				{
					bResult = true;
					for (int nIndex = 0; nIndex < nBWInputCount; ++nIndex)
					{
						bResult &= m_InstanceOfDigitalIO.ReadItem(true, arBWInput[nIndex]);
					}
					m_dgViewCylinder[COLUMN_INDEX_OF_BWIN, row].Value		= string.Join(", ", arBWInput);
					m_dgViewCylinder.Rows[row].Cells[COLUMN_INDEX_OF_BWINPULSE].Style.BackColor			= bResult ? c_clrTrue : c_clrFalse;
					m_dgViewCylinder.Rows[row].Cells[COLUMN_INDEX_OF_BWINPULSE].Style.SelectionBackColor	= bResult ? c_clrTrue : c_clrFalse;
				}
				else
				{
					m_dgViewCylinder[COLUMN_INDEX_OF_BWIN, row].Value		= string.Empty;
					m_dgViewCylinder.Rows[row].Cells[COLUMN_INDEX_OF_BWINPULSE].Style.BackColor			= c_clrFalse;
					m_dgViewCylinder.Rows[row].Cells[COLUMN_INDEX_OF_BWINPULSE].Style.SelectionBackColor	= c_clrFalse;
				}

				if (m_InstanceOfCylinder.GetParameter(arIndex[itemIndex], FrameOfSystem3.Config.ConfigCylinder.EN_PARAM_LIST_IO.BACKWARD_OUT, ref nBWOutputCount, ref arBWOutput))
				{
					bResult = true;
					for (int nIndex = 0; nIndex < nBWOutputCount; ++nIndex)
					{
						bResult &= m_InstanceOfDigitalIO.ReadItem(false, arBWOutput[nIndex]);
					}
					m_dgViewCylinder[COLUMN_INDEX_OF_BWOUT, row].Value	= string.Join(",", arBWOutput);
					m_dgViewCylinder.Rows[row].Cells[COLUMN_INDEX_OF_BWOUTPULSE].Style.BackColor				= bResult ? c_clrTrue : c_clrFalse;
					m_dgViewCylinder.Rows[row].Cells[COLUMN_INDEX_OF_BWOUTPULSE].Style.SelectionBackColor		= bResult ? c_clrTrue : c_clrFalse;
				}
				else
				{
					m_dgViewCylinder[COLUMN_INDEX_OF_BWOUT, row].Value	= string.Empty;
					m_dgViewCylinder.Rows[row].Cells[COLUMN_INDEX_OF_BWOUTPULSE].Style.BackColor				= c_clrFalse;
					m_dgViewCylinder.Rows[row].Cells[COLUMN_INDEX_OF_BWOUTPULSE].Style.SelectionBackColor		= c_clrFalse;
				}
				#endregion
				
				if(m_nIndexOfSelectedRows == row)
				{
					m_labelState.Text			= strState;

					switch(m_enTab)
					{
						case EN_VIEW_TAB.FORWARD:
							
							for (int nindex = 0, nEnd = nFWInputCount; nindex < nEnd; ++nindex)
							{
								bool bValue = m_InstanceOfDigitalIO.ReadItem(true, arFWInput[nindex]);
								m_dgViewINPUT.Rows[nindex].Cells[COLUMN_INDEX_OF_IO_VALUE].Style.BackColor = bValue ? c_clrTrue : c_clrFalse;
							}
							
							
							for (int nindex = 0, nEnd = nFWOutputCount; nindex < nEnd; ++nindex)
							{
								bool bValue = m_InstanceOfDigitalIO.ReadItem(false, arFWOutput[nindex]);
								m_dgViewOutput.Rows[nindex].Cells[COLUMN_INDEX_OF_IO_VALUE].Style.BackColor = bValue ? c_clrTrue : c_clrFalse;
							}
							
							break;
						case EN_VIEW_TAB.BACKWARD:
							
							for (int nindex = 0, nEnd = nBWInputCount; nindex < nEnd; ++nindex)
							{
								bool bValue = m_InstanceOfDigitalIO.ReadItem(true, arBWInput[nindex]);
								m_dgViewINPUT.Rows[nindex].Cells[COLUMN_INDEX_OF_IO_VALUE].Style.BackColor = bValue ? c_clrTrue : c_clrFalse;
							}
							
							
							for (int nindex = 0, nEnd = nBWOutputCount; nindex < nEnd; ++nindex)
							{
								bool bValue = m_InstanceOfDigitalIO.ReadItem(false, arBWOutput[nindex]);
								m_dgViewOutput.Rows[nindex].Cells[COLUMN_INDEX_OF_IO_VALUE].Style.BackColor = bValue ? c_clrTrue : c_clrFalse;
							}
							
							break;
					}
				}
			}
		}
		/// <summary>
		/// 2020.06.10 by twkang [ADD] 라벨들의 값을 설정한다.
		/// </summary>
		private void SetConfigurationLabels(ref int nItemOfIndex, ref int nRowIndex)
		{
			if (nItemOfIndex == SELECT_NONE || nRowIndex == SELECT_NONE) { return; }

			m_labelKey.Text				= m_dgViewCylinder[COLUMN_INDEX_OF_INDEX, nRowIndex].Value.ToString();
			m_labelName.Text			= m_dgViewCylinder[COLUMN_INDEX_OF_NAME, nRowIndex].Value.ToString();
			m_labelMonitoring.Text		= m_dgViewCylinder[COLUMN_INDEX_OF_MONITORING, nRowIndex].Value.ToString();
			
			string strResult		= string.Empty;

			bool bUseOrMode = false;

			if (m_enTab == EN_VIEW_TAB.FORWARD)
			{
				m_lblDelay.Text		= "FORWARD DELAY";
				m_lblTimeout.Text	= "FORWARD TIMEOUT";

				if (false == m_InstanceOfCylinder.GetParameter(nItemOfIndex, FrameOfSystem3.Config.ConfigCylinder.EN_PARAM_CYLINDER.FORWARD_DELAY, ref strResult))
				{
					strResult = Define.DefineConstant.Common.NONE;
				}
				m_labelDelay.Text = strResult;

				if (false == m_InstanceOfCylinder.GetParameter(nItemOfIndex, FrameOfSystem3.Config.ConfigCylinder.EN_PARAM_CYLINDER.FORWARD_TIMEOUT, ref strResult))
				{
					strResult = Define.DefineConstant.Common.NONE;
				}
				m_labelTimeout.Text = strResult;

				// 2025.03.17. by shkim. [ADD] Cylinder Or Mode 추가
				if (false == m_InstanceOfCylinder.GetParameter(nItemOfIndex, FrameOfSystem3.Config.ConfigCylinder.EN_PARAM_CYLINDER.FORWARD_CHECK_LOGIC_OR, ref bUseOrMode))
				{
					m_radioUseORMode.Active = false;
				}
				else
				{
					m_radioUseORMode.Active = bUseOrMode;
				}
				// 2025.03.17. by shkim. [END]
			}
			else
			{
				m_lblDelay.Text		= "BACKWARD DELAY";
				m_lblTimeout.Text	= "BACKWARD TIMEOUT";

				if (false == m_InstanceOfCylinder.GetParameter(nItemOfIndex, FrameOfSystem3.Config.ConfigCylinder.EN_PARAM_CYLINDER.BACKWARD_DELAY, ref strResult))
				{
					strResult = Define.DefineConstant.Common.NONE;
				}
				m_labelDelay.Text = strResult;

				if (false == m_InstanceOfCylinder.GetParameter(nItemOfIndex, FrameOfSystem3.Config.ConfigCylinder.EN_PARAM_CYLINDER.BACKWARD_TIMEOUT, ref strResult))
				{
					strResult = Define.DefineConstant.Common.NONE;
				}
				m_labelTimeout.Text = strResult;

				// 2025.03.17. by shkim. [ADD] Cylinder Or Mode 추가
				if (false == m_InstanceOfCylinder.GetParameter(nItemOfIndex, FrameOfSystem3.Config.ConfigCylinder.EN_PARAM_CYLINDER.BACKWARD_CHECK_LOGIC_OR, ref bUseOrMode))
				{
					m_radioUseORMode.Active = false;
				}
				else
				{
					m_radioUseORMode.Active = bUseOrMode;
				}
				// 2025.03.17. by shkim. [END]
			}

			AddIOItemList(ref nItemOfIndex);

			bool bEnabled			= false;

			m_InstanceOfCylinder.GetParameter(nItemOfIndex, FrameOfSystem3.Config.ConfigCylinder.EN_PARAM_CYLINDER.ENABLE, ref bEnabled);
			SetActiveControls(bEnabled);
			SetRepeatControls();
		}
		/// <summary>
		/// 
		/// </summary>
		private void AddIOItemList(ref int nItemOfIndex)
		{
			m_dgViewINPUT.Rows.Clear();
			m_dgViewOutput.Rows.Clear();

			int nInputCount		= 0;
			int[] arInputItem	= null;

			int nOutputCount	= 0;
			int[] arOutputItem	= null;
			
			switch (m_enTab)
			{
 				case EN_VIEW_TAB.BACKWARD:
					m_InstanceOfCylinder.GetParameter(nItemOfIndex, FrameOfSystem3.Config.ConfigCylinder.EN_PARAM_LIST_IO.BACKWARD_IN, ref nInputCount, ref arInputItem);
					m_InstanceOfCylinder.GetParameter(nItemOfIndex, FrameOfSystem3.Config.ConfigCylinder.EN_PARAM_LIST_IO.BACKWARD_OUT, ref nOutputCount, ref arOutputItem);
					break;
				case EN_VIEW_TAB.FORWARD:
					m_InstanceOfCylinder.GetParameter(nItemOfIndex, FrameOfSystem3.Config.ConfigCylinder.EN_PARAM_LIST_IO.FORWARD_IN, ref nInputCount, ref arInputItem);
					m_InstanceOfCylinder.GetParameter(nItemOfIndex, FrameOfSystem3.Config.ConfigCylinder.EN_PARAM_LIST_IO.FORWARD_OUT, ref nOutputCount, ref arOutputItem);
					break;
			}

			for (int nIndex = 0; nIndex < nInputCount; ++nIndex)
			{
				m_dgViewINPUT.Rows.Add();

				string strName	= string.Empty;
				m_InstanceOfDigitalIO.GetParameter(true, arInputItem[nIndex], FrameOfSystem3.Config.ConfigDigitalIO.EN_PARAM_DIGITALIO.NAME, ref strName);
				m_dgViewINPUT[COLUMN_INDEX_OF_INDEX, nIndex].Value		= arInputItem[nIndex];
				m_dgViewINPUT[COLUMN_INDEX_OF_NAME, nIndex].Value		= strName;
				m_dgViewINPUT.Rows[nIndex].Selected						= false;
			}

			for (int nIndex = 0; nIndex < nOutputCount; ++nIndex)
			{
				m_dgViewOutput.Rows.Add();

				string strName	= string.Empty;
				m_InstanceOfDigitalIO.GetParameter(false, arOutputItem[nIndex], FrameOfSystem3.Config.ConfigDigitalIO.EN_PARAM_DIGITALIO.NAME, ref strName);
				m_dgViewOutput[COLUMN_INDEX_OF_INDEX, nIndex].Value		= arOutputItem[nIndex];
				m_dgViewOutput[COLUMN_INDEX_OF_NAME, nIndex].Value		= strName;
				m_dgViewOutput.Rows[nIndex].Selected						= false;
			}
		}
		/// <summary>
		/// 2020.06.10 by twkang [ADD] 컨트롤들의 활성, 비활성 상태를 설정한다.
		/// </summary>
		private void SetActiveControls(bool bEnabled)
		{
			m_btnEnable.Enabled				= !bEnabled;
			m_btnDisable.Enabled			= bEnabled;

			m_btnRemove.Enabled				= bEnabled;

			m_labelTimeout.Enabled			= bEnabled;
			m_labelName.Enabled				= bEnabled;
			m_labelMonitoring.Enabled		= bEnabled;

			m_labelDelay.Enabled			= bEnabled;
			
			m_btnRepeat.Enabled				= bEnabled;
			m_btnForward.Enabled			= bEnabled;
			m_btnBackward.Enabled			= bEnabled;
		}
		private void SetRepeatControls()
		{
			bool bRepeat					= m_InstanceOfCylinder.IsRepeat(m_nIndexOfSelectedItem);

			m_ledRepeat.Active				= bRepeat;

			m_labelName.Enabled				= !bRepeat;
			m_labelMonitoring.Enabled		= !bRepeat;
			m_labelTimeout.Enabled			= !bRepeat;
			m_labelDelay.Enabled			= !bRepeat;

			m_btnAddInput.Enabled			= !bRepeat;
			m_btnAddOutput.Enabled			= !bRepeat;

			m_btnBackward.Enabled			= !bRepeat;
			m_btnForward.Enabled			= !bRepeat;

		}
		/// <summary>
		/// 2020.06.10 by twkang [ADD] 라벨 상태와, 값을 초기화한다.
		/// </summary>
		private void ResetSelectedItem()
		{
			m_nIndexOfSelectedRows			= SELECT_NONE;
			m_nIndexOfSelectedItem			= SELECT_NONE;

            m_labelKey.Text					= "";
            m_labelState.Text				= "--";
			m_labelTimeout.Text				= "--";
            m_labelName.Text				= "--";
            m_labelMonitoring.Text			= "--";
            m_labelDelay.Text				= "--";

            m_dgViewINPUT.Rows.Clear();
			m_dgViewOutput.Rows.Clear();

            SetActiveControls(false);

			m_ledRepeat.Active				= false;
            m_btnEnable.Enabled				= false;
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
		/// 2021.07.28 by twkang [ADD] 버튼 클릭 확인
		/// </summary>
		private bool ConfirmButtonClick(string strActionName)
		{
			return m_InstanceOfMessageBox.ShowMessage(string.Format("Action : [{0}], This is a confirmation message", strActionName));
		}
		#endregion

		#region UI이벤트
		/// <summary>
		/// 2020.06.11 by twkang [ADD] Grid 셀을 했을 경우
		/// </summary>
		private void Click_Item(object sender, DataGridViewCellEventArgs e)
		{
			int nRowindex		= e.RowIndex;
			int nColumnIndex	= e.ColumnIndex;

			if (nRowindex < 0 || nRowindex >= m_dgViewCylinder.RowCount) { return; }

			int nIndex	= int.Parse(m_dgViewCylinder[COLUMN_INDEX_OF_INDEX, nRowindex].Value.ToString());

			m_nIndexOfSelectedRows		= nRowindex;
			m_nIndexOfSelectedItem		= nIndex;

			m_bNeedToConfirm			= true;

			SetConfigurationLabels(ref nIndex, ref nRowindex);				
		}
		/// <summary>
		/// 2020.06.11 by twkang [ADD] SelectionList 가 필요한 라벨을 클릭했을 때
		/// </summary>
		private void Click_Monitoring(object sender, EventArgs e)
		{
			string strValue		= string.Empty;
			string strPreValue	= string.Empty;
			
			if(m_InstanceOfSelectionList.CreateForm(m_lblMonitoring.Text, Define.DefineEnumProject.SelectionList.EN_SELECTIONLIST.CYLINDER_MONITORING_MODE
				, m_labelMonitoring.Text))
			{
				m_InstanceOfSelectionList.GetResult(ref strValue);
				if(m_InstanceOfCylinder.SetParameter(m_nIndexOfSelectedItem, FrameOfSystem3.Config.ConfigCylinder.EN_PARAM_CYLINDER.MONITORING, strValue))
				{
					m_dgViewCylinder[COLUMN_INDEX_OF_MONITORING, m_nIndexOfSelectedRows].Value	= strValue;
					m_labelMonitoring.Text	= strValue;
				}
            }
		}
		/// <summary>
		/// 2020.06.11 by twkang [ADD] ENABLE 또는 DISABLE 버튼 클릭 이벤트
		/// </summary>
		private void Click_EnableOrDisable(object sender, EventArgs e)
		{
			if(m_nIndexOfSelectedItem == SELECT_NONE)
			{
				return;
			}

			Control ctrl = sender as Control;

			switch(ctrl.TabIndex)
			{
				case 0: // ENABLE
					m_InstanceOfCylinder.SetParameter(m_nIndexOfSelectedItem, FrameOfSystem3.Config.ConfigCylinder.EN_PARAM_CYLINDER.ENABLE, true);
					break;
				case 1: // DISABLE
					m_InstanceOfCylinder.SetParameter(m_nIndexOfSelectedItem, FrameOfSystem3.Config.ConfigCylinder.EN_PARAM_CYLINDER.ENABLE, false);
					break;
			}
			SetConfigurationLabels(ref m_nIndexOfSelectedItem, ref m_nIndexOfSelectedRows);
		}
		/// <summary>
		/// 2020.06.11 by twkang [ADD] 숫자 입력이 필요한 라벨을 클릭했을 때
		/// </summary>
		private void Click_ValueOfNumber(object sender, EventArgs e)
		{
			Control ctrl	= sender as Control;

			int nResult		= -1;

			switch(ctrl.TabIndex)
			{
				case 0: // DELAY
					if(m_InstanceOfCalculator.CreateForm(m_labelDelay.Text
						, MIN_OF_PARAM
						, MAX_OF_PARAM))
					{
						m_InstanceOfCalculator.GetResult(ref nResult);

						if(m_enTab == EN_VIEW_TAB.FORWARD)
						{
							if (m_InstanceOfCylinder.SetParameter(m_nIndexOfSelectedItem, FrameOfSystem3.Config.ConfigCylinder.EN_PARAM_CYLINDER.FORWARD_DELAY, nResult))
							{
								m_labelDelay.Text = nResult.ToString();
							}
						}
						else
						{
							if (m_InstanceOfCylinder.SetParameter(m_nIndexOfSelectedItem, FrameOfSystem3.Config.ConfigCylinder.EN_PARAM_CYLINDER.BACKWARD_DELAY, nResult))
							{
								m_labelDelay.Text = nResult.ToString();
							}
						}
					}
					break;
				case 1:	// TIMEOUT
					if(m_InstanceOfCalculator.CreateForm(m_labelTimeout.Text
						, MIN_OF_PARAM
						, MAX_OF_PARAM))
					{
						m_InstanceOfCalculator.GetResult(ref nResult);

						if(m_enTab == EN_VIEW_TAB.FORWARD)
						{
							if (m_InstanceOfCylinder.SetParameter(m_nIndexOfSelectedItem, FrameOfSystem3.Config.ConfigCylinder.EN_PARAM_CYLINDER.FORWARD_TIMEOUT, nResult))
							{
								m_labelTimeout.Text = nResult.ToString();
							}
						}
						else
						{
							if (m_InstanceOfCylinder.SetParameter(m_nIndexOfSelectedItem, FrameOfSystem3.Config.ConfigCylinder.EN_PARAM_CYLINDER.BACKWARD_TIMEOUT, nResult))
							{
								m_labelTimeout.Text = nResult.ToString();
							}
						}
					}
					break;
			}
		}
		/// <summary>
		/// 2020.06.11 by twkang [ADD] 키보드 입력이 필요한 라벨을 클릭했을 때
		/// </summary>
		private void Click_ValueOfCharacter(object sender, EventArgs e)
		{
			string strResult	= string.Empty;

			if(m_InstanceOfKeyboard.CreateForm(m_labelName.Text))
			{
				m_InstanceOfKeyboard.GetResult(ref strResult);
				if(m_InstanceOfCylinder.SetParameter(m_nIndexOfSelectedItem
					, FrameOfSystem3.Config.ConfigCylinder.EN_PARAM_CYLINDER.NAME
					, strResult))
				{
					m_labelName.Text	= strResult;
					m_dgViewCylinder[COLUMN_INDEX_OF_NAME, m_nIndexOfSelectedRows].Value	= strResult;
				}
			}
		}
		/// <summary>
		/// 2020.06.11 by twkang [ADD] ADD, REMOVE 버튼을 눌렀을 때
		/// </summary>
		private void Click_AddOrRemove(object sender, EventArgs e)
		{
			Control ctrl	= sender as Control;
			
			switch(ctrl.TabIndex)
			{
				case 0: // ADD
					int nItem		= m_InstanceOfCylinder.AddCylinderItem();
					if(SELECT_NONE != nItem)
					{
						AddItemOnGrid(nItem);
					}					
					break;
				case 1: // REMOVE
					if(m_InstanceOfCylinder.IsRepeat(m_nIndexOfSelectedItem))
					{
						m_InstanceOfCylinder.SetRepeat(m_nIndexOfSelectedItem);
					}
					
					if(ConfirmButtonClick("REMOVE"))
					{
						RemoveItemOnGrid();
					}
					break;
			}
			ResetSelectedItem();
		}
		/// <summary>
		/// 2020.11.13 by twkang [ADD]
		/// </summary>
		private void Click_InputListButton(object sender, EventArgs e)
		{
			int[] arDigitalIndex	= null;
			string[] arDigitalName	= null;

			m_InstanceOfDigitalIO.GetListOfItem(true, ref arDigitalIndex);
			m_InstanceOfDigitalIO.GetListOfName(true, ref arDigitalName);

			string[] arPreValue	= new string[m_dgViewINPUT.Rows.Count];

			for(int nIndex = 0, nEnd = m_dgViewINPUT.Rows.Count; nIndex < nEnd; ++nIndex)
			{
				arPreValue[nIndex]	= m_dgViewINPUT[COLUMN_INDEX_OF_NAME, nIndex].Value.ToString();
			}

			int[] arSelected	= null;

			if(m_InstanceOfSelectionList.CreateForm(string.Empty ,arDigitalName, arDigitalIndex, arPreValue, true))
			{
				m_InstanceOfSelectionList.GetResult(ref arSelected);

				switch (m_enTab)
				{
					case EN_VIEW_TAB.FORWARD:
						m_InstanceOfCylinder.SetParameter(m_nIndexOfSelectedItem, FrameOfSystem3.Config.ConfigCylinder.EN_PARAM_LIST_IO.FORWARD_IN, arSelected.Length, ref arSelected);
						break;
					case EN_VIEW_TAB.BACKWARD:
						m_InstanceOfCylinder.SetParameter(m_nIndexOfSelectedItem, FrameOfSystem3.Config.ConfigCylinder.EN_PARAM_LIST_IO.BACKWARD_IN, arSelected.Length, ref arSelected);
						break;
				}

				AddIOItemList(ref m_nIndexOfSelectedItem);
			}
		}
		/// <summary>
		/// 2020.11.13 by twkang [ADD]
		/// </summary>
		private void Click_OutputListButton(object sender, EventArgs e)
		{
			int[] arDigitalIndex	= null;
			string[] arDigitalName	= null;

			m_InstanceOfDigitalIO.GetListOfItem(false, ref arDigitalIndex);
			m_InstanceOfDigitalIO.GetListOfName(false, ref arDigitalName);

			string[] arPreValue	= new string[m_dgViewOutput.Rows.Count];

			for(int nIndex = 0, nEnd = m_dgViewOutput.Rows.Count; nIndex < nEnd; ++nIndex)
			{
				arPreValue[nIndex]	= m_dgViewOutput[COLUMN_INDEX_OF_NAME, nIndex].Value.ToString();
			}

			int[] arSelected	= null;

			if(m_InstanceOfSelectionList.CreateForm(string.Empty ,arDigitalName, arDigitalIndex, arPreValue, true))
			{
				m_InstanceOfSelectionList.GetResult(ref arSelected);

				switch (m_enTab)
				{
					case EN_VIEW_TAB.FORWARD:
						m_InstanceOfCylinder.SetParameter(m_nIndexOfSelectedItem, FrameOfSystem3.Config.ConfigCylinder.EN_PARAM_LIST_IO.FORWARD_OUT, arSelected.Length, ref arSelected);
						break;
					case EN_VIEW_TAB.BACKWARD:
						m_InstanceOfCylinder.SetParameter(m_nIndexOfSelectedItem, FrameOfSystem3.Config.ConfigCylinder.EN_PARAM_LIST_IO.BACKWARD_OUT, arSelected.Length, ref arSelected);
						break;
				}

				AddIOItemList(ref m_nIndexOfSelectedItem);
			}
		}
		/// <summary>
		/// 2020.11.13 by twkang [ADD] 
		/// </summary>
		private void Click_TabLabel(object sender, EventArgs e)
		{
			Control ctrl	= sender as Control;

			if (m_tabClicked.TabIndex == ctrl.TabIndex) { return; }

			m_tabClicked.GradientFirstColor		= Color.White;
			m_tabClicked.GradientSecondColor	= Color.White;
			m_tabClicked.MainFontColor			= Color.DarkBlue;
			m_tabClicked.ButtonClicked			= false;

			switch(ctrl.TabIndex)
			{
				case 0:
					m_tabClicked				= m_tabForward;
					m_enTab						= EN_VIEW_TAB.FORWARD;
					break;
				case 1:
					m_tabClicked				= m_tabBackward;
					m_enTab						= EN_VIEW_TAB.BACKWARD;
					break;
			}

			m_tabClicked.ButtonClicked			= true;
			m_tabClicked.GradientFirstColor		= Color.DarkBlue;
			m_tabClicked.GradientSecondColor	= Color.DarkBlue;
			m_tabClicked.MainFontColor			= Color.White;

			SetConfigurationLabels(ref m_nIndexOfSelectedItem, ref m_nIndexOfSelectedRows);
		}
		/// <summary>
		/// 2020.06.11 by twkang [ADD] ACTION 관련 버튼을 눌렀을 때
		/// </summary>
		private void Click_Action(object sender, EventArgs e)
		{
			Control ctrl	= sender as Control;

			string strState	= null;

			m_InstanceOfCylinder.GetCylinderState(m_nIndexOfSelectedItem, ref strState);

			if (m_bNeedToConfirm)
			{ 
				if(false == ConfirmButtonClick(ctrl.Text))
					return;

				m_bNeedToConfirm		= false;
			}

			switch(ctrl.TabIndex)
			{
				case 0: // REPEAT
					if(strState != Define.DefineConstant.Common.NONE)
					{
						m_InstanceOfCylinder.SetRepeat(m_nIndexOfSelectedItem);
						SetRepeatControls();
					}					
					break;
				case 1:	// FORWARD
					m_InstanceOfCylinder.MoveForward(m_nIndexOfSelectedItem);
					break;
				case 2:	// BACKWARD
					m_InstanceOfCylinder.MoveBackward(m_nIndexOfSelectedItem);
					break;
			}
		}
		#endregion

		private void lbl_Filtering_Click(object sender, EventArgs e)
		{
			if (false == m_InstanceOfKeyboard.CreateForm(_filterKeyword))
				return;

			m_InstanceOfKeyboard.GetResult(ref _filterKeyword);
			UpdateForm();
			lbl_Filtering.Text = _filterKeyword;
		}

		void Clicked_OrMode(object sender, EventArgs e)
		{
			if (m_nIndexOfSelectedItem == SELECT_NONE)
			{
				return;
			}

			switch (m_tabClicked.TabIndex)
			{
				case 0:
					if (m_InstanceOfCylinder.SetParameter(m_nIndexOfSelectedItem, FrameOfSystem3.Config.ConfigCylinder.EN_PARAM_CYLINDER.FORWARD_CHECK_LOGIC_OR, m_radioUseORMode.Active))
					{
						
					}
					break;
				case 1:
					if (m_InstanceOfCylinder.SetParameter(m_nIndexOfSelectedItem, FrameOfSystem3.Config.ConfigCylinder.EN_PARAM_CYLINDER.BACKWARD_CHECK_LOGIC_OR, m_radioUseORMode.Active))
					{

					}
					break;
			}
		}
	}
}
