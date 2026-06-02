using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using Define.DefineConstant;

namespace FrameOfSystem3.Views.Config
{
    public partial class Config_Digital : UserControlForMainView.CustomView
	{
		public Config_Digital()
		{
			InitializeComponent();

			#region Instance
			m_InstanceOfDigitalIO		= FrameOfSystem3.Config.ConfigDigitalIO.GetInstance();
			m_InstanceOfSelectionList	= Functional.Form_SelectionList.GetInstance();
			m_InstanceOfKeyboard		= Functional.Form_Keyboard.GetInstance();
			m_InstanceOfCalculator		= Functional.Form_Calculator.GetInstance();
			m_InstanceOfMessageBox		= Functional.Form_MessageBox.GetInstance();
			#endregion

			m_btnSelectedTab			= m_btnInput;
			m_dgViewSelected			= m_dgViewInput;

            m_ViewInput.Visible = false;
            m_ViewOutPut.Visible = false;

            // 2023.09.08. by shkim. [ADD] UI 크기 유동적으로 변경
            //_tableWidth_gridContents = _tableLayoutPanel_GridView.Width;
            //_tableHeight_gridContents = _tableLayoutPanel_GridView.Height;
            _cellWidth_gridViewName = _tableLayoutPanel_GridView.ColumnStyles[0].Width;

            _cellWidth_gridContents = _tableLayoutPanel_GridView.ColumnStyles[1].Width;
            _cellHeight_gridContents = _tableLayoutPanel_GridView.RowStyles[0].Height;

            _tableLayoutPanel_GridView.ColumnStyles[0].Width = 0;
            _tableLayoutPanel_GridView.RowStyles[1].Height = 0;
            // 2023.09.08. by shkim. [END]
        }

		#region 상수
		private const int HEIGHT_OF_ROWS			= 30;

		//파라미터들의 최대, 최소값
		private readonly string MIN_OF_TARGET				= "-1";
		private readonly string MAX_OF_TARGET				= "128";
		private readonly string MIN_OF_DELAY				= "0";
		private readonly string MAX_OF_PARAM				= "999999";
		
		//GridView 칼럼 인덱스 번호
		private const int COLUMN_INDEX_OF_INDEX				= 0;
		private const int COLUMN_INDEX_OF_ENABLE			= 1;
		private const int COLUMN_INDEX_OF_NAME				= 2;
		private const int COLUMN_INDEX_OF_TAGNUMBER			= 3;
		private const int COLUMN_INDEX_OF_TARGET			= 4;
		private const int COLUMN_INDEX_OF_ONDELAY			= 5;
		private const int COLUMN_INDEX_OF_OFFDELAY			= 6;
		private const int COLUMN_INDEX_OF_REVERSE			= 7;		
		private const int COLUMN_INDEX_OF_PULSE				= 8;

		//GridView Location 인덱스 번호

		private const int INIT_LOCATION_INDEX_OF_INPUT		= 0;
		private const int CHANGED_LOCATION_INDEX_OF_INPUT	= 1;
		private const int INIT_LOCATION_INDEX_OF_OUTPUT		= 2;
		private const int CHANGED_LOCATION_INDEX_OF_OUTPUT	= 3;

        //GridView Size Index 번호
        private const int INIT_SIZE_INDEX = 0;
        private const int CHANGED_SIZE_INDEX = 1;

		// GridView Init and change Values
        Size[] arSize = new Size[] { new Size(1140, 714), new Size(1068, 357) };
        Point[] arLocationPoint = new Point[] { new Point(0, 186), new Point(72, 186), new Point(0, 186), new Point(72, 543) };

        #region GUI
        private readonly string LEFT_ARROW					= "◀";
		private readonly string RIGHT_ARROW					= "▶";
		
		private const int EXTEND_HEIGHT_X					= 380;
		#endregion

		private const int SELECT_NONE						= -1;

		private readonly Color c_clrTrue					= Color.DodgerBlue;
		private readonly Color c_clrFalse					= Color.White;

		private readonly Color c_clrInputOn					= Color.LimeGreen;
		private readonly Color c_clrOutputOn				= Color.Red;
		#endregion

		#region 변수
		private int m_nIndexOfSelectedItem			= -1;
		private int m_nIndexOfSelectedRows			= -1;

        Sys3Controls.Sys3button m_btnSelectedTab		= null;
		DataGridView m_dgViewSelected					= null;

		#region Instance
		FrameOfSystem3.Config.ConfigDigitalIO m_InstanceOfDigitalIO	= null;
		Functional.Form_MessageBox m_InstanceOfMessageBox			= null;
		Functional.Form_SelectionList m_InstanceOfSelectionList		= null;
		Functional.Form_Keyboard m_InstanceOfKeyboard				= null;
		Functional.Form_Calculator m_InstanceOfCalculator			= null;
		#endregion

        // 2023.09.08. by shkim. [ADD] UI 크기 유동적으로 변경
        float _cellWidth_gridViewName = 0;

        float _cellWidth_gridContents = 0;
        float _cellHeight_gridContents = 0;
        // 2023.09.08. by shkim. [END]

		// 2024.03.08 by junho [ADD] filtering 기능 추가
		string _filterKeyword = string.Empty;
		#endregion

		#region 상속 인터페이스
		/// <summary>
		/// 
		/// </summary>
		protected override void ProcessWhenActivation()
        {
			InitializeList(true);
			InitializeList(false);
        }
        /// <summary>
        /// 
        /// </summary>
        protected override void ProcessWhenDeactivation()
        {
        }
        /// <summary>
        /// 2020.06.05 by twkang [ADD] 타이머에 의해 작동되는 함수, Pulse 값을 갱신한다.
        /// </summary>
        public override void CallFunctionByTimer()
        {
			UpdateInputPulse();
			UpdateOutputPulse();
        }
        #endregion

		#region 내부인터페이스
		/// <summary>
		/// 2020.06.09 by twkang [ADD] DigitalIO INPUT 파라미터들의 값을 업데이트한다.
		/// </summary>
		private void InitializeList(bool bInput)
		{
			int[] arIndex		= null;

			if (false == m_InstanceOfDigitalIO.GetListOfItem(bInput, ref arIndex))
			{
				return;
			}

			if(bInput)
			{
				m_dgViewInput.Rows.Clear();
			}
			else
			{
				m_dgViewOutput.Rows.Clear();
			}

			for (int nIndex = 0, nEnd = arIndex.Length; nIndex < nEnd; ++nIndex)
			{
				AddItemOnList(bInput, arIndex[nIndex]);
			}
		}
		/// <summary>
		/// 2020.07.01 by twkang [ADD] Input 또는 Output 아이템을 GridView 에 추가한다.
		/// </summary>
		private void AddItemOnList(bool bInput, int nIndexOfItem)
		{
			DataGridView dgViewList		= bInput ? m_dgViewInput : m_dgViewOutput;

			int nIndexOfRow				= dgViewList.Rows.Count;

			// 2024.03.08 by junho [ADD] filtering 기능 추가
			string itemName = string.Empty;
			m_InstanceOfDigitalIO.GetParameter(bInput, nIndexOfItem, FrameOfSystem3.Config.ConfigDigitalIO.EN_PARAM_DIGITALIO.NAME, ref itemName);

			// 2024.06.25 by junho [MOD] 대소문자 구분하지 않도록 변경
			//if (_filterKeyword != string.Empty && false == itemName.Contains(_filterKeyword))
			if (_filterKeyword != string.Empty && itemName.IndexOf(_filterKeyword, StringComparison.OrdinalIgnoreCase) < 0)
				return;

			dgViewList.Rows.Add();
			
			bool bResult		= false;
			string strValue		= string.Empty;

			dgViewList.Rows[nIndexOfRow].Height = HEIGHT_OF_ROWS;

			dgViewList[COLUMN_INDEX_OF_INDEX, nIndexOfRow].Value		= nIndexOfItem.ToString();
			dgViewList.Rows[nIndexOfRow].Cells[COLUMN_INDEX_OF_INDEX].Style.BackColor			= Color.Silver;
			dgViewList.Rows[nIndexOfRow].Cells[COLUMN_INDEX_OF_INDEX].Style.SelectionBackColor	= Color.Silver;
			dgViewList.Rows[nIndexOfRow].Cells[COLUMN_INDEX_OF_INDEX].Style.SelectionForeColor	= Color.Black;

			bResult = false;
			m_InstanceOfDigitalIO.GetParameter(bInput, nIndexOfItem, FrameOfSystem3.Config.ConfigDigitalIO.EN_PARAM_DIGITALIO.ENABLE, ref bResult);
			dgViewList.Rows[nIndexOfRow].Cells[COLUMN_INDEX_OF_ENABLE].Style.BackColor				= bResult ? c_clrTrue : c_clrFalse;
			dgViewList.Rows[nIndexOfRow].Cells[COLUMN_INDEX_OF_ENABLE].Style.SelectionBackColor		= bResult ? c_clrTrue : c_clrFalse;

			// 2024.03.08 by junho [MOD] filtering 기능 추가
			//m_InstanceOfDigitalIO.GetParameter(bInput, nIndexOfItem, FrameOfSystem3.Config.ConfigDigitalIO.EN_PARAM_DIGITALIO.NAME, ref strValue);
			//dgViewList[COLUMN_INDEX_OF_NAME, nIndexOfRow].Value = strValue;
			dgViewList[COLUMN_INDEX_OF_NAME, nIndexOfRow].Value = itemName;

			m_InstanceOfDigitalIO.GetParameter(bInput, nIndexOfItem, FrameOfSystem3.Config.ConfigDigitalIO.EN_PARAM_DIGITALIO.TAG_NUMBER, ref strValue);
			dgViewList[COLUMN_INDEX_OF_TAGNUMBER, nIndexOfRow].Value	= strValue;

			m_InstanceOfDigitalIO.GetParameter(bInput, nIndexOfItem, FrameOfSystem3.Config.ConfigDigitalIO.EN_PARAM_DIGITALIO.TARGET, ref strValue);
			dgViewList[COLUMN_INDEX_OF_TARGET, nIndexOfRow].Value		= strValue;

			m_InstanceOfDigitalIO.GetParameter(bInput, nIndexOfItem, FrameOfSystem3.Config.ConfigDigitalIO.EN_PARAM_DIGITALIO.ONDELAY, ref strValue);
			dgViewList[COLUMN_INDEX_OF_ONDELAY, nIndexOfRow].Value		= strValue;

			m_InstanceOfDigitalIO.GetParameter(bInput, nIndexOfItem, FrameOfSystem3.Config.ConfigDigitalIO.EN_PARAM_DIGITALIO.OFFDELAY, ref strValue);
			dgViewList[COLUMN_INDEX_OF_OFFDELAY, nIndexOfRow].Value		= strValue;

			bResult = false;
			m_InstanceOfDigitalIO.GetParameter(bInput, nIndexOfItem, FrameOfSystem3.Config.ConfigDigitalIO.EN_PARAM_DIGITALIO.REVERSE, ref bResult);
			dgViewList.Rows[nIndexOfRow].Cells[COLUMN_INDEX_OF_REVERSE].Style.BackColor				= bResult ? c_clrTrue : c_clrFalse;
			dgViewList.Rows[nIndexOfRow].Cells[COLUMN_INDEX_OF_REVERSE].Style.SelectionBackColor	= bResult ? c_clrTrue : c_clrFalse;

			dgViewList.Rows[nIndexOfRow].Selected			= false;
		}
		/// <summary>
		/// 2020.06.05 by twkang [ADD] 주기적으로 Input Item 의 Pulse 값을 갱신한다.
		/// </summary>
		private void UpdateInputPulse()
		{
			string strValue		= string.Empty;
			int[] arIndex		= null;

			if (false == m_InstanceOfDigitalIO.GetListOfItem(true, ref arIndex))
				return;

			// 2024.03.13 by junho [MOD] filtering 기능으로 인한 변경
			//for(int nIndex = 0, nEnd = arIndex.Length; nIndex < nEnd; ++nIndex)
			//{
			//	if(m_InstanceOfDigitalIO.ReadItem(true, arIndex[nIndex]))
			//	{
			//		m_dgViewInput.Rows[nIndex].Cells[COLUMN_INDEX_OF_PULSE].Style.BackColor				= c_clrInputOn;
			//		m_dgViewInput.Rows[nIndex].Cells[COLUMN_INDEX_OF_PULSE].Style.SelectionBackColor	= c_clrInputOn;
			//	}
			//	else
			//	{
			//		m_dgViewInput.Rows[nIndex].Cells[COLUMN_INDEX_OF_PULSE].Style.BackColor				= c_clrFalse;
			//		m_dgViewInput.Rows[nIndex].Cells[COLUMN_INDEX_OF_PULSE].Style.SelectionBackColor	= c_clrFalse;
			//	}
			//}
			int index;
			for (int i = 0; i < m_dgViewInput.Rows.Count; ++i)
			{
				if (false == int.TryParse(m_dgViewInput[COLUMN_INDEX_OF_INDEX, i].Value.ToString(), out index))
					return;

				if(m_InstanceOfDigitalIO.ReadItem(true, arIndex[index]))
				{
					m_dgViewInput.Rows[i].Cells[COLUMN_INDEX_OF_PULSE].Style.BackColor = c_clrInputOn;
					m_dgViewInput.Rows[i].Cells[COLUMN_INDEX_OF_PULSE].Style.SelectionBackColor = c_clrInputOn;
				}
				else
				{
					m_dgViewInput.Rows[i].Cells[COLUMN_INDEX_OF_PULSE].Style.BackColor = c_clrFalse;
					m_dgViewInput.Rows[i].Cells[COLUMN_INDEX_OF_PULSE].Style.SelectionBackColor = c_clrFalse;
				}
			}
		}
		/// <summary>
		/// 2020.06.05 by twkang [ADD] 주기적으로 Output Item 의 Pulse 값을 갱신한다.
		/// </summary>
		private void UpdateOutputPulse()
		{
			string strValue		= string.Empty;
			int[] arIndex		= null;

			if (false == m_InstanceOfDigitalIO.GetListOfItem(false, ref arIndex))
				return;

			// 2024.03.13 by junho [MOD] filtering 기능으로 인한 변경
			//for (int nIndex = 0, nEnd = arIndex.Length; nIndex < nEnd; ++nIndex)
			//{
			//	if(m_InstanceOfDigitalIO.ReadItem(false, arIndex[nIndex]))
			//	{
			//		m_dgViewOutput.Rows[nIndex].Cells[COLUMN_INDEX_OF_PULSE].Style.BackColor			= c_clrOutputOn;
			//		m_dgViewOutput.Rows[nIndex].Cells[COLUMN_INDEX_OF_PULSE].Style.SelectionBackColor	= c_clrOutputOn;
			//	}
			//	else
			//	{
			//		m_dgViewOutput.Rows[nIndex].Cells[COLUMN_INDEX_OF_PULSE].Style.BackColor			= c_clrFalse;
			//		m_dgViewOutput.Rows[nIndex].Cells[COLUMN_INDEX_OF_PULSE].Style.SelectionBackColor	= c_clrFalse;
			//	}
			//}
			int index;
            for (int i = 0; i < m_dgViewOutput.Rows.Count; ++i)
			{
				if (false == int.TryParse(m_dgViewOutput[COLUMN_INDEX_OF_INDEX, i].Value.ToString(), out index))
					return;

				if (m_InstanceOfDigitalIO.ReadItem(false, arIndex[index]))
				{
					m_dgViewOutput.Rows[i].Cells[COLUMN_INDEX_OF_PULSE].Style.BackColor = c_clrOutputOn;
					m_dgViewOutput.Rows[i].Cells[COLUMN_INDEX_OF_PULSE].Style.SelectionBackColor = c_clrOutputOn;
				}
				else
				{
					m_dgViewOutput.Rows[i].Cells[COLUMN_INDEX_OF_PULSE].Style.BackColor = c_clrFalse;
					m_dgViewOutput.Rows[i].Cells[COLUMN_INDEX_OF_PULSE].Style.SelectionBackColor = c_clrFalse;
				}
			}
		}
		/// <summary>
		/// 2020.07.01 by twkang [ADD] 선택한 아이템을 지운다.
		/// </summary>
		private bool RemoveItemOnList(bool bInput)
		{
			if(SELECT_NONE == m_nIndexOfSelectedRows)
			{
				return false;
			}
			
			int nItemIndex	= int.Parse(m_dgViewSelected[COLUMN_INDEX_OF_INDEX, m_nIndexOfSelectedRows].Value.ToString());

			if(m_InstanceOfDigitalIO.RemoveItem(bInput, nItemIndex))
			{
				m_dgViewSelected.Rows.RemoveAt(m_nIndexOfSelectedRows);

				if(null != m_dgViewSelected.CurrentCell)
				{
					m_dgViewSelected.Rows[m_dgViewSelected.CurrentRow.Index].Selected		= false;
				}
				return true;
			}
			return false;
		}
		/// <summary>
		/// 2020.10.20 by twkang [ADD] 아이템 선택을 초기화한다.
		/// </summary>
		private void ResetSelectedItem()
		{
			m_nIndexOfSelectedItem			= SELECT_NONE;
			m_nIndexOfSelectedRows			= SELECT_NONE;

			m_btnRemove.Enabled		= false;

			if(m_dgViewSelected.CurrentRow != null)
			{
				m_dgViewSelected.CurrentRow.Selected	= false;
			}
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
		/// 2020.10.20 by twkang [ADD] ItemList 클릭 이벤트
		/// </summary>
		private void Click_DataGrid(object sender, EventArgs e)
        {
			Control ctrl = sender as Control;

			switch (ctrl.TabIndex)
            {
				case 0:
					m_dgViewSelected = m_dgViewInput;
					break;

				case 1:
					m_dgViewSelected = m_dgViewOutput;
					break;

				default:
					break;
            }

		}
		private void Click_ItemList(object sender, DataGridViewCellEventArgs e)
		{
			int nRowIndex	= e.RowIndex;

			if (nRowIndex < 0 || nRowIndex >= m_dgViewSelected.RowCount) { return ;}

			m_nIndexOfSelectedRows	= nRowIndex;
		
			if(int.TryParse(m_dgViewSelected[COLUMN_INDEX_OF_INDEX, nRowIndex].Value.ToString(), out m_nIndexOfSelectedItem))
			{
				m_btnRemove.Enabled	= true;
			}
		}
		/// <summary>
		/// 2020.06.05 by twkang [ADD] INPUT 아이템들을 더블클릭했을 때 발생하는 이벤트이다.
		/// </summary>
		private void DoubleClick_InputItem(object sender, DataGridViewCellEventArgs e)
		{
			int nRowindex		= e.RowIndex;
			int nColumnIndex	= e.ColumnIndex;

			if (nRowindex < 0 || nRowindex >= m_dgViewInput.RowCount) { return; }

			string strResult	= string.Empty;
			bool bResult		= false;
			int nResult			= -1;
			switch (nColumnIndex)
			{
				case COLUMN_INDEX_OF_ENABLE:
					if (m_InstanceOfSelectionList.CreateForm(SelectionList.ENABLE
						, Define.DefineEnumProject.SelectionList.EN_SELECTIONLIST.ENABLE_DISABLE
						, m_dgViewInput.Rows[nRowindex].Cells[COLUMN_INDEX_OF_ENABLE].Style.BackColor == c_clrTrue ? SelectionList.ENABLE : SelectionList.DISABLE
						, false
						, SelectionList.DISABLE))
					{
						m_InstanceOfSelectionList.GetResult(ref strResult);
						bResult = strResult.Equals(SelectionList.ENABLE);
						if(m_InstanceOfDigitalIO.SetParameter(true, m_nIndexOfSelectedItem, FrameOfSystem3.Config.ConfigDigitalIO.EN_PARAM_DIGITALIO.ENABLE, bResult))
						{
							m_dgViewInput.Rows[nRowindex].Cells[COLUMN_INDEX_OF_ENABLE].Style.BackColor = bResult ? c_clrTrue : c_clrFalse;
							m_dgViewInput.Rows[nRowindex].Cells[COLUMN_INDEX_OF_ENABLE].Style.SelectionBackColor = bResult ? c_clrTrue : c_clrFalse;
						}
					}
					break;
				case COLUMN_INDEX_OF_NAME:
					if (m_InstanceOfKeyboard.CreateForm(m_dgViewInput[COLUMN_INDEX_OF_NAME, nRowindex].Value.ToString()))
					{
						m_InstanceOfKeyboard.GetResult(ref strResult);
						if(m_InstanceOfDigitalIO.SetParameter(true, m_nIndexOfSelectedItem, FrameOfSystem3.Config.ConfigDigitalIO.EN_PARAM_DIGITALIO.NAME, strResult))
						{
							m_dgViewInput[COLUMN_INDEX_OF_NAME, nRowindex].Value		= strResult;
						}
					}
					break;
				case COLUMN_INDEX_OF_TAGNUMBER:
					if(m_InstanceOfKeyboard.CreateForm(m_dgViewInput[COLUMN_INDEX_OF_TAGNUMBER, nRowindex].Value.ToString()))
					{
						m_InstanceOfKeyboard.GetResult(ref strResult);
						if(m_InstanceOfDigitalIO.SetParameter(true, m_nIndexOfSelectedItem, FrameOfSystem3.Config.ConfigDigitalIO.EN_PARAM_DIGITALIO.TAG_NUMBER, strResult))
						{
							m_dgViewInput[COLUMN_INDEX_OF_TAGNUMBER, nRowindex].Value	= strResult;
						}
					}
					break;
				case COLUMN_INDEX_OF_TARGET:
					if (m_InstanceOfCalculator.CreateForm(m_dgViewInput[COLUMN_INDEX_OF_TARGET, nRowindex].Value.ToString(), MIN_OF_TARGET
						, MAX_OF_TARGET))
					{
						m_InstanceOfCalculator.GetResult(ref nResult);
						if(m_InstanceOfDigitalIO.SetParameter(true, m_nIndexOfSelectedItem, FrameOfSystem3.Config.ConfigDigitalIO.EN_PARAM_DIGITALIO.TARGET, nResult))
						{
							m_dgViewInput[COLUMN_INDEX_OF_TARGET, nRowindex].Value		= nResult.ToString();
						}
					}
					break;
				case COLUMN_INDEX_OF_ONDELAY:
					if (m_InstanceOfCalculator.CreateForm(m_dgViewInput[COLUMN_INDEX_OF_ONDELAY, nRowindex].Value.ToString(), MIN_OF_DELAY
						, MAX_OF_PARAM))
					{
						m_InstanceOfCalculator.GetResult(ref nResult);
						if(m_InstanceOfDigitalIO.SetParameter(true, m_nIndexOfSelectedItem, FrameOfSystem3.Config.ConfigDigitalIO.EN_PARAM_DIGITALIO.ONDELAY, nResult))
						{
							m_dgViewInput[COLUMN_INDEX_OF_ONDELAY, nRowindex].Value		= nResult.ToString();
						}
					}
					break;
				case COLUMN_INDEX_OF_OFFDELAY:
					if (m_InstanceOfCalculator.CreateForm(m_dgViewInput[COLUMN_INDEX_OF_OFFDELAY, nRowindex].Value.ToString(), MIN_OF_DELAY
						, MAX_OF_PARAM))
					{
						m_InstanceOfCalculator.GetResult(ref nResult);
						if(m_InstanceOfDigitalIO.SetParameter(true, m_nIndexOfSelectedItem, FrameOfSystem3.Config.ConfigDigitalIO.EN_PARAM_DIGITALIO.OFFDELAY, nResult))
						{
							m_dgViewInput[COLUMN_INDEX_OF_OFFDELAY, nRowindex].Value	= nResult.ToString();
						}
					}
					break;
				case COLUMN_INDEX_OF_REVERSE:
					if (m_InstanceOfSelectionList.CreateForm(m_dgViewInput.Columns[COLUMN_INDEX_OF_REVERSE].HeaderText
						, Define.DefineEnumProject.SelectionList.EN_SELECTIONLIST.TRUE_FALSE
						, m_dgViewInput.Rows[nRowindex].Cells[COLUMN_INDEX_OF_REVERSE].Style.BackColor == c_clrTrue ? SelectionList.TRUE : SelectionList.FALSE
						, false
						, SelectionList.FALSE))
					{
						m_InstanceOfSelectionList.GetResult(ref strResult);
						bResult = strResult.Equals(SelectionList.TRUE);
						if(m_InstanceOfDigitalIO.SetParameter(true, m_nIndexOfSelectedItem, FrameOfSystem3.Config.ConfigDigitalIO.EN_PARAM_DIGITALIO.REVERSE, bResult))
						{
							m_dgViewInput.Rows[nRowindex].Cells[COLUMN_INDEX_OF_REVERSE].Style.BackColor			= bResult ? c_clrTrue : c_clrFalse;
							m_dgViewInput.Rows[nRowindex].Cells[COLUMN_INDEX_OF_REVERSE].Style.SelectionBackColor	= bResult ? c_clrTrue : c_clrFalse;
						}
					}
					break;
			}

			m_dgViewInput.Rows[nRowindex].Selected = false;
		}
		/// <summary>
		/// 2020.06.05 by twkang [ADD] OUTPUT 아이템들을 더블클릭했을 때 발생하는 이벤트이다.
		/// </summary>
		private void DoubleClick_OutputItem(object sender, DataGridViewCellEventArgs e)
		{
			int nRowindex		= e.RowIndex;
			int nColumnIndex	= e.ColumnIndex;

			if (nRowindex < 0 || nRowindex >= m_dgViewOutput.RowCount) { return; }

			string strResult	= string.Empty;
			bool bResult		= false;
			int nResult			= -1;

			switch (nColumnIndex)
			{
				case COLUMN_INDEX_OF_ENABLE:
					if (m_InstanceOfSelectionList.CreateForm(SelectionList.ENABLE
						, Define.DefineEnumProject.SelectionList.EN_SELECTIONLIST.ENABLE_DISABLE
						, m_dgViewOutput.Rows[nRowindex].Cells[COLUMN_INDEX_OF_ENABLE].Style.BackColor == c_clrTrue ? SelectionList.ENABLE : SelectionList.DISABLE
						, false
						, SelectionList.DISABLE))
					{
						m_InstanceOfSelectionList.GetResult(ref strResult);
						bResult = strResult.Equals(SelectionList.ENABLE);
						if(m_InstanceOfDigitalIO.SetParameter(false, m_nIndexOfSelectedItem, FrameOfSystem3.Config.ConfigDigitalIO.EN_PARAM_DIGITALIO.ENABLE, bResult))
						{
							m_dgViewOutput.Rows[nRowindex].Cells[COLUMN_INDEX_OF_ENABLE].Style.BackColor = bResult ? c_clrTrue : c_clrFalse;
							m_dgViewOutput.Rows[nRowindex].Cells[COLUMN_INDEX_OF_ENABLE].Style.SelectionBackColor = bResult ? c_clrTrue : c_clrFalse;
						}
					}
					break;
				case COLUMN_INDEX_OF_NAME:
					if (m_InstanceOfKeyboard.CreateForm(m_dgViewOutput[COLUMN_INDEX_OF_NAME, nRowindex].Value.ToString()))
					{
						m_InstanceOfKeyboard.GetResult(ref strResult);
						if(m_InstanceOfDigitalIO.SetParameter(false, m_nIndexOfSelectedItem, FrameOfSystem3.Config.ConfigDigitalIO.EN_PARAM_DIGITALIO.NAME, strResult))
						{
							m_dgViewOutput[COLUMN_INDEX_OF_NAME, nRowindex].Value		= strResult;
						}
					}
					break;
				case COLUMN_INDEX_OF_TAGNUMBER:
					if (m_InstanceOfKeyboard.CreateForm(m_dgViewOutput[COLUMN_INDEX_OF_TAGNUMBER, nRowindex].Value.ToString()))
					{
						m_InstanceOfKeyboard.GetResult(ref strResult);
						if (m_InstanceOfDigitalIO.SetParameter(false, m_nIndexOfSelectedItem, FrameOfSystem3.Config.ConfigDigitalIO.EN_PARAM_DIGITALIO.TAG_NUMBER, strResult))
						{
							m_dgViewOutput[COLUMN_INDEX_OF_TAGNUMBER, nRowindex].Value = strResult;
						}
					}
					break;
				case COLUMN_INDEX_OF_TARGET:
					if (m_InstanceOfCalculator.CreateForm(m_dgViewOutput[COLUMN_INDEX_OF_TARGET, nRowindex].Value.ToString()
						, MIN_OF_TARGET
						, MAX_OF_TARGET))
					{
						m_InstanceOfCalculator.GetResult(ref nResult);
						if(m_InstanceOfDigitalIO.SetParameter(false, m_nIndexOfSelectedItem, FrameOfSystem3.Config.ConfigDigitalIO.EN_PARAM_DIGITALIO.TARGET, nResult))
						{
							m_dgViewOutput[COLUMN_INDEX_OF_TARGET, nRowindex].Value		= nResult.ToString();
						}
					}
					break;
				case COLUMN_INDEX_OF_ONDELAY:
					if (m_InstanceOfCalculator.CreateForm(m_dgViewOutput[COLUMN_INDEX_OF_ONDELAY, nRowindex].Value.ToString(), MIN_OF_DELAY
						, MAX_OF_PARAM))
					{
						m_InstanceOfCalculator.GetResult(ref nResult);
						if (m_InstanceOfDigitalIO.SetParameter(false, m_nIndexOfSelectedItem, FrameOfSystem3.Config.ConfigDigitalIO.EN_PARAM_DIGITALIO.ONDELAY, nResult))
						{
							m_dgViewOutput[COLUMN_INDEX_OF_ONDELAY, nRowindex].Value = nResult.ToString();
						}
					}
					break;
				case COLUMN_INDEX_OF_OFFDELAY:
					if (m_InstanceOfCalculator.CreateForm(m_dgViewOutput[COLUMN_INDEX_OF_OFFDELAY, nRowindex].Value.ToString(), MIN_OF_DELAY
						, MAX_OF_PARAM))
					{
						m_InstanceOfCalculator.GetResult(ref nResult);
						if (m_InstanceOfDigitalIO.SetParameter(false, m_nIndexOfSelectedItem, FrameOfSystem3.Config.ConfigDigitalIO.EN_PARAM_DIGITALIO.OFFDELAY, nResult))
						{
							m_dgViewOutput[COLUMN_INDEX_OF_OFFDELAY, nRowindex].Value = nResult.ToString();
						}
					}
					break;
				case COLUMN_INDEX_OF_REVERSE:
					if (m_InstanceOfSelectionList.CreateForm(m_dgViewOutput.Columns[COLUMN_INDEX_OF_REVERSE].HeaderText
						, Define.DefineEnumProject.SelectionList.EN_SELECTIONLIST.TRUE_FALSE
						, m_dgViewOutput.Rows[nRowindex].Cells[COLUMN_INDEX_OF_REVERSE].Style.BackColor == c_clrTrue ? SelectionList.TRUE : SelectionList.FALSE
						, false
						, SelectionList.FALSE))
					{
						m_InstanceOfSelectionList.GetResult(ref strResult);
						bResult = strResult.Equals(SelectionList.TRUE);
						if(m_InstanceOfDigitalIO.SetParameter(false, m_nIndexOfSelectedItem, FrameOfSystem3.Config.ConfigDigitalIO.EN_PARAM_DIGITALIO.REVERSE, bResult))
						{
							m_dgViewOutput.Rows[nRowindex].Cells[COLUMN_INDEX_OF_REVERSE].Style.BackColor			= bResult ? c_clrTrue : c_clrFalse;
							m_dgViewOutput.Rows[nRowindex].Cells[COLUMN_INDEX_OF_REVERSE].Style.SelectionBackColor	= bResult ? c_clrTrue : c_clrFalse;
						}
					}
					break;
				case COLUMN_INDEX_OF_PULSE:
					bool bOn	= m_dgViewOutput.Rows[nRowindex].Cells[COLUMN_INDEX_OF_PULSE].Style.BackColor == c_clrOutputOn;
					if(bOn)
					{
						m_InstanceOfDigitalIO.WriteOutput(false, m_nIndexOfSelectedItem);
					}
					else
					{
						// 2023.07.11 by junho [MOD] selection list 발생하지 않고 바로 ON되도록 변경
						//if (m_InstanceOfSelectionList.CreateForm(m_dgViewOutput.Columns[COLUMN_INDEX_OF_PULSE].HeaderText
						//, Define.DefineEnumProject.SelectionList.EN_SELECTIONLIST.TRUE_FALSE
						//, SelectionList.TRUE
						//, false
						//, SelectionList.FALSE))
						//{
						//	m_InstanceOfSelectionList.GetResult(ref strResult);
						//	bResult = strResult.Equals(SelectionList.TRUE);
						//	m_InstanceOfDigitalIO.WriteOutput(bResult, m_nIndexOfSelectedItem);
						//}
						m_InstanceOfDigitalIO.WriteOutput(true, m_nIndexOfSelectedItem);
					}
					break;
			}
		}
		/// <summary>
		/// 2020.06.05 by twkang [ADD] View Option 이벤트
		/// </summary>
		private void Click_ViewOption(object sender, EventArgs e)
		{
			Control ctrl = sender as Control;

			switch (ctrl.TabIndex)
			{
				case COLUMN_INDEX_OF_INDEX: //INDEX
					m_dgViewInput.Columns[COLUMN_INDEX_OF_INDEX].Visible			= m_ToggleIndex.Active;
					m_dgViewOutput.Columns[COLUMN_INDEX_OF_INDEX].Visible			= m_ToggleIndex.Active;
					m_ToggleIndex.Active											= m_dgViewInput.Columns[COLUMN_INDEX_OF_INDEX].Visible;
					break;
				case COLUMN_INDEX_OF_NAME: // NAME
					m_dgViewInput.Columns[COLUMN_INDEX_OF_NAME].Visible				= m_ToggleName.Active;
					m_dgViewOutput.Columns[COLUMN_INDEX_OF_NAME].Visible			= m_ToggleName.Active;
					m_ToggleName.Active												= m_dgViewInput.Columns[COLUMN_INDEX_OF_NAME].Visible;
					break;
				case COLUMN_INDEX_OF_TAGNUMBER:
					m_dgViewInput.Columns[COLUMN_INDEX_OF_TAGNUMBER].Visible		= m_ToggleTag.Active;
					m_dgViewOutput.Columns[COLUMN_INDEX_OF_TAGNUMBER].Visible		= m_ToggleTag.Active;
					m_ToggleTag.Active												= m_dgViewInput.Columns[COLUMN_INDEX_OF_TAGNUMBER].Visible;
					break;
				case COLUMN_INDEX_OF_TARGET: // TARGET
					m_dgViewInput.Columns[COLUMN_INDEX_OF_TARGET].Visible			= m_ToggleTarget.Active;
					m_dgViewOutput.Columns[COLUMN_INDEX_OF_TARGET].Visible			= m_ToggleTarget.Active;
					m_ToggleTarget.Active											= m_dgViewInput.Columns[COLUMN_INDEX_OF_TARGET].Visible;
					break;
				case COLUMN_INDEX_OF_ONDELAY: // ONDELAY
					m_dgViewInput.Columns[COLUMN_INDEX_OF_ONDELAY].Visible			= m_ToggleOnDelay.Active;
					m_dgViewOutput.Columns[COLUMN_INDEX_OF_ONDELAY].Visible			= m_ToggleOnDelay.Active;
					m_ToggleOnDelay.Active											= m_dgViewInput.Columns[COLUMN_INDEX_OF_ONDELAY].Visible;
					break;
				case COLUMN_INDEX_OF_OFFDELAY: // OFFDELAY
					m_dgViewInput.Columns[COLUMN_INDEX_OF_OFFDELAY].Visible			= m_ToggleOffDelay.Active;
					m_dgViewOutput.Columns[COLUMN_INDEX_OF_OFFDELAY].Visible		= m_ToggleOffDelay.Active;
					m_ToggleOffDelay.Active											= m_dgViewInput.Columns[COLUMN_INDEX_OF_OFFDELAY].Visible;
					break;
				case COLUMN_INDEX_OF_REVERSE: // REVERSE
					m_dgViewInput.Columns[COLUMN_INDEX_OF_REVERSE].Visible			= m_ToggleReverse.Active;
					m_dgViewOutput.Columns[COLUMN_INDEX_OF_REVERSE].Visible			= m_ToggleReverse.Active;
					m_ToggleReverse.Active											= m_dgViewInput.Columns[COLUMN_INDEX_OF_REVERSE].Visible;
					break;
			}
		}
		/// <summary>
		/// 2020.07.01 by twkang [ADD] Input View 버튼 클릭 이벤트
		/// </summary>
		private void Click_ViewButtons(object sender, EventArgs e)
		{
			Control ctrl	= sender as Control;
			bool bInput		= (m_dgViewInput.Name == m_dgViewSelected.Name );

			switch (ctrl.TabIndex)
			{
				case 0: // ADD
					int nItem		= m_InstanceOfDigitalIO.AddDigitalItem(bInput);					
					if(nItem != SELECT_NONE)
					{
						InitializeList(bInput);
					}
					break;
				case 1:	// REMOVE
					if(ConfirmButtonClick("REMOVE"))
					{
						RemoveItemOnList(bInput);
					}
					break;
				//case 2: // ADD NUM
				//	m_InstanceOfCalculator.CreateForm(MIN_OF_INDEX, MIN_OF_INDEX, MAX_OF_PARAM);
				//	int nIndex		= -1;
				//	m_InstanceOfCalculator.GetResult(ref nIndex);
				//	if(m_InstanceOfDigitalIO.AddDigitalItem(true, nIndex))
				//	{
				//		InitializeList(bInput);
				//	}
				//	break;
			}

			ResetSelectedItem();
		}
		/// <summary>
		/// 2020.07.20 by twkang [ADD] 방향 버튼을 눌렀을 때
		/// </summary>
		private void Click_ArrowButton(object sender, EventArgs e)
		{
			if(m_btnExtend.Text == LEFT_ARROW)
			{
                m_btnAdd.Visible = true;
                m_btnRemove.Visible = true;
				m_btnExtend.Text		= RIGHT_ARROW;
			}
			else
			{
                m_btnAdd.Visible = false;
                m_btnRemove.Visible = false;
				m_btnExtend.Text		= LEFT_ARROW;
			}

			m_groupList.Invalidate();
		}
		/// <summary>
		/// 2020.11.02 by twkang [ADD] 탭 전환 버튼을 클릭했을 경우 발생한다.
        /// 2021.05.14 by jhlee [MODIFY]
		/// </summary>
		private void Click_ChangeList(object sender, EventArgs e)
		{
			Control ctrl	= sender as Control;

			if (m_btnSelectedTab.TabIndex == ctrl.TabIndex) { return;}

			m_btnSelectedTab.ButtonClicked			= false;
			m_btnSelectedTab.GradientFirstColor		= Color.White;
			m_btnSelectedTab.GradientSecondColor	= Color.White;
			m_btnSelectedTab.MainFontColor			= Color.DarkBlue;

			///<summary>
			///2021.03.30 by jhlee [ADD] GridView Initialize
			///</summary>
            //m_dgViewInput.Size = arSize[INIT_SIZE_INDEX];
            //m_dgViewOutput.Size = arSize[INIT_SIZE_INDEX];

            //m_dgViewInput.Location = arLocationPoint[INIT_LOCATION_INDEX_OF_INPUT];
            //m_dgViewOutput.Location = arLocationPoint[INIT_LOCATION_INDEX_OF_OUTPUT];

			m_ViewInput.Visible = false;
			m_ViewOutPut.Visible = false;

            m_dgViewInput.Visible = false;
            m_dgViewOutput.Visible = false;


			switch (ctrl.TabIndex)
			{
				case 0: // INPUT
					m_dgViewSelected			= m_dgViewInput;
					m_btnSelectedTab			= m_btnInput;

                    // 2023.09.08. by shkim. [ADD] UI 크기 유동적으로 변경
                    _tableLayoutPanel_GridView.ColumnStyles[0].Width = 0;
                    _tableLayoutPanel_GridView.RowStyles[0].Height = _cellHeight_gridContents;
                    _tableLayoutPanel_GridView.RowStyles[1].Height = 0;
                    // 2023.09.08. by shkim. [END]
					break;
				case 1:	// OUTPUT
					m_dgViewSelected			= m_dgViewOutput;
					m_btnSelectedTab			= m_btnOutput;

                    // 2023.09.08. by shkim. [ADD] UI 크기 유동적으로 변경
                    _tableLayoutPanel_GridView.ColumnStyles[0].Width = 0;
                    _tableLayoutPanel_GridView.RowStyles[0].Height = 0;
                    _tableLayoutPanel_GridView.RowStyles[1].Height = _cellHeight_gridContents;
                    // 2023.09.08. by shkim. [END]
					break;

					///<summary>
					///2021.03.30 by jhlee [ADD] IO button Click event - Visible True and Resizing
					/// </summary>
				case 2:
                    m_btnSelectedTab = m_btnIO;

                    // 2023.09.08. by shkim. [MOD] UI 크기 유동적으로 변경
                    //m_dgViewInput.Size = arSize[CHANGED_SIZE_INDEX];
                    //m_dgViewOutput.Size = arSize[CHANGED_SIZE_INDEX];

                    //m_dgViewInput.Location = arLocationPoint[CHANGED_LOCATION_INDEX_OF_INPUT];
                    //m_dgViewOutput.Location = arLocationPoint[CHANGED_LOCATION_INDEX_OF_OUTPUT];

                    _tableLayoutPanel_GridView.ColumnStyles[0].Width = _cellWidth_gridViewName;
                    _tableLayoutPanel_GridView.ColumnStyles[1].Width = _cellWidth_gridContents;
                    _tableLayoutPanel_GridView.RowStyles[0].Height = _cellHeight_gridContents;
                    _tableLayoutPanel_GridView.RowStyles[1].Height = _cellHeight_gridContents;
                    // 2023.09.08. by shkim. [END]

                    m_dgViewInput.Visible = true;
                    m_dgViewOutput.Visible = true;

                    m_ViewInput.Visible = true;
                    m_ViewOutPut.Visible = true;

                    break;
			}
			m_btnSelectedTab.ButtonClicked			= true;
			m_btnSelectedTab.GradientFirstColor		= Color.DarkBlue;
			m_btnSelectedTab.GradientSecondColor	= Color.DarkBlue;
			m_btnSelectedTab.MainFontColor			= Color.White;

			m_dgViewSelected.Visible				= true;

			ResetSelectedItem();
		}
		/// <summary>
		/// 2024.03.08 by junho [ADD] item filtering 기능 추가
		/// </summary>
		private void lbl_Filtering_Click(object sender, EventArgs e)
		{
			if (false == m_InstanceOfKeyboard.CreateForm(_filterKeyword))
				return;

			m_InstanceOfKeyboard.GetResult(ref _filterKeyword);
			InitializeList(true);
			InitializeList(false);
			lbl_Filtering.Text = _filterKeyword;
		}

		#endregion
	}
}
