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
    public partial class Config_Interrupt : UserControlForMainView.CustomView
    {
        public Config_Interrupt()
        {
            InitializeComponent();

			#region Instance
			m_InstanceOfDigitalIO				= FrameOfSystem3.Config.ConfigDigitalIO.GetInstance();
			m_InstanceOfInterrupt				= FrameOfSystem3.Config.ConfigInterrupt.GetInstance();
			m_InstanceOfCalculator				= Functional.Form_Calculator.GetInstance();
			m_InstanceOfKeyboard				= Functional.Form_Keyboard.GetInstance();
			m_InstanceOfSelectionList			= Functional.Form_SelectionList.GetInstance();
			m_InstanceOfMessageBox				= Functional.Form_MessageBox.GetInstance();
			#endregion

			#region Mapping
			m_DicForActionParam.Clear();
			foreach(INTERRUPT_ACTION en in Enum.GetValues(typeof(INTERRUPT_ACTION)))
			{
				m_DicForActionParam.Add((int)en, en.ToString());
			}
			#endregion
		}
		#region 열거형
		private enum INTERRUPT_ACTION
		{
			START	= 0,
			STOP	= 1,
			RESET	= 2,
			ALARM	= 3,
			DIGITAL_OUT	= 4,
		}
		#endregion

		#region 상수
		private const int HEIGHT_OF_ROWS					= 30;

		private readonly string MIN_OF_PARAM				= "0";
		private readonly string MAX_OF_PARAM				= "999999";

		#region COLUMN_INDEX
		private const int COLUMN_INDEX_OF_INDEX				= 0;
		private const int COLUMN_INDEX_OF_ENABLE			= 1;
		private const int COLUMN_INDEX_OF_NAME				= 2;
		private const int COLUMN_INDEX_OF_TARGET			= 3;
		private const int COLUMN_INDEX_OF_TARGET_ACT		= 4;
		private const int COLUMN_INDEX_OF_ACTION			= 5;
		private const int COLUMN_INDEX_OF_CODE				= 6;
		private const int COLUMN_INDEX_OF_OUTPUT			= 7;
		private const int COLUMN_INDEX_OF_CONDITION			= 8;
		private const int COLUMN_INDEX_OF_CONDITION_ACT		= 9;
		#endregion

		private const int SELECT_NONE						= -1;

		private readonly Color c_clrTrue					= Color.DodgerBlue;
		private readonly Color c_clrFalse				    = Color.White;		
		#endregion

		#region 변수
		private int m_nIndexOfSelectedRows					= -1;
		private int m_nIndexOfSelectedItem					= -1;

		private int[] m_nArrOfItem							= null;

		Dictionary<int, string> m_DicForActionParam			= new Dictionary<int,string>();

		#region Instance
		Functional.Form_MessageBox m_InstanceOfMessageBox					= null;
		Functional.Form_Keyboard m_InstanceOfKeyboard						= null;
		Functional.Form_Calculator m_InstanceOfCalculator					= null;
		Functional.Form_SelectionList m_InstanceOfSelectionList				= null;
		FrameOfSystem3.Config.ConfigDigitalIO m_InstanceOfDigitalIO			= null;
		FrameOfSystem3.Config.ConfigInterrupt m_InstanceOfInterrupt			= null;
		#endregion
		
		#endregion

		#region 상속 인터페이스
		/// <summary>
        /// 
        /// </summary>
        protected override void ProcessWhenActivation()
        {
			UpdateGrid();
			if(SELECT_NONE != m_nIndexOfSelectedRows)
			{
				m_dgViewInterrupt.Rows[m_nIndexOfSelectedRows].Selected	= true;
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
			UpdateState();
        }
        #endregion

		#region 내부인터페이스
		/// <summary>
		/// 2020.06.18 by twkang [ADD] 아이템의 파라미터들로 GridView 를 업데이트한다.
		/// </summary>
		private void UpdateGrid()
		{
			if(false == m_InstanceOfInterrupt.GetListOfItem(ref m_nArrOfItem))
			{
				return;
			}

			int nCountOfItem = m_nArrOfItem.Length;

			m_dgViewInterrupt.Rows.Clear();

			for(int i = 0; i < nCountOfItem; ++i)
			{
				AddItemOnGrid(m_nArrOfItem[i]);
			}
		}
		/// <summary>
		/// 2020.06.18 by twkang [ADD] Target State 를 갱신한다.
		/// </summary>
		private void UpdateState()
		{
			if (false == m_InstanceOfInterrupt.GetListOfItem(ref m_nArrOfItem))
			{
				return;
			}

			int nCountOfItem	= m_nArrOfItem.Length;

			for(int i = 0; i < nCountOfItem; i++)
			{
				bool bResult			= false;
				string strTarget		= string.Empty;
				int nTarget				= -1;

				m_InstanceOfInterrupt.GetParameter(m_nArrOfItem[i], FrameOfSystem3.Config.ConfigInterrupt.EN_PARAM_INTERRUPT.ENABLE, ref bResult);
				m_dgViewInterrupt.Rows[i].Cells[COLUMN_INDEX_OF_ENABLE].Style.BackColor				= bResult ? c_clrTrue : c_clrFalse;
				m_dgViewInterrupt.Rows[i].Cells[COLUMN_INDEX_OF_ENABLE].Style.SelectionBackColor	= bResult ? c_clrTrue : c_clrFalse;

				m_InstanceOfInterrupt.GetParameter(m_nArrOfItem[i], FrameOfSystem3.Config.ConfigInterrupt.EN_PARAM_INTERRUPT.TARGET, ref nTarget);
				m_dgViewInterrupt[COLUMN_INDEX_OF_TARGET, i].Value		= nTarget.ToString();

				bResult	= m_InstanceOfDigitalIO.ReadItem(true, nTarget);
				m_dgViewInterrupt.Rows[i].Cells[COLUMN_INDEX_OF_TARGET_ACT].Style.BackColor				= bResult ? c_clrTrue : c_clrFalse;
				m_dgViewInterrupt.Rows[i].Cells[COLUMN_INDEX_OF_TARGET_ACT].Style.SelectionBackColor	= bResult ? c_clrTrue : c_clrFalse;

				bResult = m_InstanceOfInterrupt.CheckConditionActivation(m_nArrOfItem[i]);
				m_dgViewInterrupt.Rows[i].Cells[COLUMN_INDEX_OF_CONDITION_ACT].Style.BackColor			= bResult ? c_clrTrue : c_clrFalse;
				m_dgViewInterrupt.Rows[i].Cells[COLUMN_INDEX_OF_CONDITION_ACT].Style.SelectionBackColor	= bResult ? c_clrTrue : c_clrFalse;

				if(m_nIndexOfSelectedRows == i)
				{
					m_InstanceOfDigitalIO.GetParameter(true, nTarget, FrameOfSystem3.Config.ConfigDigitalIO.EN_PARAM_DIGITALIO.NAME, ref strTarget);
					m_labelInput.Text		= strTarget;
					m_labelInput.SubText	= "[ " + nTarget.ToString() + " ]";
					m_ledInput.Active		= m_InstanceOfDigitalIO.ReadItem(true, nTarget);
				}

                if (m_nIndexOfSelectedRows == i)
                {
                    int nDOIndex = 0;
                    m_InstanceOfInterrupt.GetParameter(m_nArrOfItem[i], FrameOfSystem3.Config.ConfigInterrupt.EN_PARAM_INTERRUPT.DO_INDEX, ref nDOIndex);

                    m_InstanceOfDigitalIO.GetParameter(false, nDOIndex, FrameOfSystem3.Config.ConfigDigitalIO.EN_PARAM_DIGITALIO.NAME, ref strTarget);
                    m_labelDO.Text = strTarget;
                    m_labelDO.SubText = "[ " + nDOIndex.ToString() + " ]";
                }
			}
		}
		/// <summary>
		/// 2020.07.14 by twkang [ADD] 아이템을 
		/// </summary>
		private void AddItemOnGrid(int nIndexOfItem)
		{ 
			int nIndexOfRow		= m_dgViewInterrupt.Rows.Count;

			m_dgViewInterrupt.Rows.Add();

			string strValue		= string.Empty;
			int nResult			= -1;

			m_dgViewInterrupt.Rows[nIndexOfRow].Height = HEIGHT_OF_ROWS;
			m_dgViewInterrupt[COLUMN_INDEX_OF_INDEX, nIndexOfRow].Value = nIndexOfItem.ToString();
			m_dgViewInterrupt.Rows[nIndexOfRow].Cells[COLUMN_INDEX_OF_INDEX].Style.BackColor = Color.Silver;
			m_dgViewInterrupt.Rows[nIndexOfRow].Cells[COLUMN_INDEX_OF_INDEX].Style.SelectionBackColor = Color.Silver;
			m_dgViewInterrupt.Rows[nIndexOfRow].Cells[COLUMN_INDEX_OF_INDEX].Style.SelectionForeColor = Color.Black;

			if (false == m_InstanceOfInterrupt.GetParameter(nIndexOfItem, FrameOfSystem3.Config.ConfigInterrupt.EN_PARAM_INTERRUPT.NAME, ref strValue))
			{
				strValue = string.Empty;
			}
			m_dgViewInterrupt[COLUMN_INDEX_OF_NAME, nIndexOfRow].Value = strValue;

			m_dgViewInterrupt[COLUMN_INDEX_OF_ACTION, nIndexOfRow].Value = string.Empty;
			if (m_InstanceOfInterrupt.GetParameter(nIndexOfItem, FrameOfSystem3.Config.ConfigInterrupt.EN_PARAM_INTERRUPT.ACTION, ref nResult)
				|| m_DicForActionParam.ContainsKey(nResult))
			{
				m_dgViewInterrupt[COLUMN_INDEX_OF_ACTION, nIndexOfRow].Value = m_DicForActionParam[nResult];
			}

			if (false == m_InstanceOfInterrupt.GetParameter(nIndexOfItem, FrameOfSystem3.Config.ConfigInterrupt.EN_PARAM_INTERRUPT.CODE_ALARM, ref strValue))
			{
				strValue = string.Empty;
			}
			m_dgViewInterrupt[COLUMN_INDEX_OF_CODE, nIndexOfRow].Value = strValue;

            if (false == m_InstanceOfInterrupt.GetParameter(nIndexOfItem, FrameOfSystem3.Config.ConfigInterrupt.EN_PARAM_INTERRUPT.DO_INDEX, ref strValue))
            {
                strValue = string.Empty;
            }
            m_dgViewInterrupt[COLUMN_INDEX_OF_OUTPUT, nIndexOfRow].Value = strValue;

			if (false == m_InstanceOfInterrupt.GetParameter(nIndexOfItem, FrameOfSystem3.Config.ConfigInterrupt.EN_PARAM_INTERRUPT.CONDITION, ref strValue))
			{
				strValue = string.Empty;
			}
			m_dgViewInterrupt[COLUMN_INDEX_OF_CONDITION, nIndexOfRow].Value = strValue;
			m_dgViewInterrupt.Rows[nIndexOfRow].Selected	= false;
		}
		/// <summary>
		/// 2020.07.14 by twkang [ADD] 선택한 아이템을 지운다.
		/// </summary>
		/// <returns></returns>
		private bool RemoveItemOnGrid()
		{
			if(SELECT_NONE == m_nIndexOfSelectedRows)
			{
				return false;
			}

			if(m_InstanceOfInterrupt.RemoveItem(m_nIndexOfSelectedItem))
			{
				m_dgViewInterrupt.Rows.RemoveAt(m_nIndexOfSelectedRows);

				if(null != m_dgViewInterrupt.CurrentCell)
				{
					m_dgViewInterrupt.Rows[m_dgViewInterrupt.CurrentRow.Index].Selected	= false;
				}
				return true;
			}
			return false;
		}
		/// <summary>
		/// 2020.06.18 by twkang [ADD] 라벨들의 값을 설정한다.
		/// </summary>
		private void SetConfigurationLabels(ref int nRowIndex)
		{
			m_labelKey.Text			= m_nIndexOfSelectedItem.ToString();
			m_labelName.Text        = m_dgViewInterrupt[COLUMN_INDEX_OF_NAME, nRowIndex].Value.ToString();

            m_labelAction.Text      = m_dgViewInterrupt[COLUMN_INDEX_OF_ACTION, nRowIndex].Value.ToString();
            m_labelAlarmCode.Text   = m_dgViewInterrupt[COLUMN_INDEX_OF_CODE, nRowIndex].Value.ToString();
            m_labelCondition.Text   = m_dgViewInterrupt[COLUMN_INDEX_OF_CONDITION, nRowIndex].Value.ToString();

			bool bEnable			= false;

			m_InstanceOfInterrupt.GetParameter(m_nIndexOfSelectedItem, FrameOfSystem3.Config.ConfigInterrupt.EN_PARAM_INTERRUPT.ENABLE, ref bEnable);

			SetActiveControls(bEnable);
		}
		/// <summary>ㅌ
		/// 2020.06.18. by twkang [ADD] 아이템의 활성 상태에 따른 버튼의 상태
		/// </summary>
		private void SetActiveControls(bool bValue)
		{
			m_btnEnable.Enabled         = !bValue;
            m_btnDisable.Enabled        = bValue;

            m_btnRemove.Enabled         = bValue;

            m_labelName.Enabled         = bValue;
            m_labelInput.Enabled        = bValue;
            m_labelAction.Enabled       = bValue;
            m_labelCondition.Enabled    = bValue;
            m_labelAlarmCode.Enabled    = bValue;
            m_labelDO.Enabled           = bValue;
		}
		/// <summary>
		/// 2020.06.18 by twkang [ADD] 선택 아이템, 라벨을 초기화한다.
		/// </summary>
		private void ResetSelectedItem()
		{
			m_nIndexOfSelectedItem		= -1;
			m_nIndexOfSelectedRows		= -1;
            m_labelKey.Text			    = "";

            m_labelName.Text			= "--";
            m_labelAction.Text			= "--";
            m_labelInput.Text			= "--";
            m_labelInput.SubText		= "[ -1 ]";
            m_labelAction.Text			= "--";
            m_labelCondition.Text		= "--";
            m_labelDO.Text              = "--";

            m_ledInput.Active			= false;

            SetActiveControls(false);

            m_btnEnable.Enabled			 = false;
		}
		/// <summary>
		/// 2020.06.18 by twkang [ADD] 괄호를 제거하고 정수로 캐스팅하여 반환한다.
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
		/// 2020.06.18 by twkang [ADD] 그리드 뷰 클릭 이벤트
		/// </summary>
		private void Click_Dgview(object sender, DataGridViewCellEventArgs e)
		{
			int nRowIndex   = e.RowIndex;

			if (nRowIndex < 0 || nRowIndex >= m_dgViewInterrupt.RowCount) { return; }

			if (nRowIndex == m_nIndexOfSelectedRows) return;

			m_nIndexOfSelectedItem	= int.Parse(m_dgViewInterrupt[COLUMN_INDEX_OF_INDEX, nRowIndex].Value.ToString());

			m_nIndexOfSelectedRows	= nRowIndex;
			
			SetConfigurationLabels(ref nRowIndex);
		}
		/// <summary>
		/// 2020.06.18 by twkang [ADD] ENABLE, DISABLE 버튼 클릭 이벤트
		/// </summary>
		private void Click_EnableOrDisable(object sender, EventArgs e)
		{
			bool bEnable	= false;
			
			if(m_InstanceOfInterrupt.GetParameter(m_nIndexOfSelectedItem, FrameOfSystem3.Config.ConfigInterrupt.EN_PARAM_INTERRUPT.ENABLE, ref bEnable))
			{
				m_InstanceOfInterrupt.SetParameter(m_nIndexOfSelectedItem, FrameOfSystem3.Config.ConfigInterrupt.EN_PARAM_INTERRUPT.ENABLE, !bEnable);

				SetActiveControls(!bEnable);
			}

		}
		/// <summary>
		/// 2020.06.18 by twkang [ADD] ADD, REMOVE 버튼 클릭 이벤트
		/// </summary>
		private void Click_AddOrRemove(object sender, EventArgs e)
		{
			Control ctrl		= sender as Control;

			switch(ctrl.TabIndex)
			{
				case 0: // ADD					
					int nItem	= m_InstanceOfInterrupt.AddInterruptItem();
					if(SELECT_NONE != nItem)
					{
						AddItemOnGrid(nItem);
					}
					break;
				case 1:	// REMOVE
					if(ConfirmButtonClick("REMOVE"))
					{
						RemoveItemOnGrid();
					}
					break;
			}
			ResetSelectedItem();
		}
		/// <summary>
		/// 2020.06.18 by twkang [ADD] 설정 라벨들 클릭 이벤트
		/// </summary>
		private void Click_Configuration(object sender, EventArgs e)
		{
			Control ctrl			= sender as Control;

			string strResult		= string.Empty;
			int nValue				= -1;

			string[] strArrName		= null;
			int[] nArrIndex			= null;
			switch(ctrl.TabIndex)
			{
				case 0: // NAME
					if(m_InstanceOfKeyboard.CreateForm(m_labelName.Text))
					{
						m_InstanceOfKeyboard.GetResult(ref strResult);
						if(m_InstanceOfInterrupt.SetParameter(m_nIndexOfSelectedItem, FrameOfSystem3.Config.ConfigInterrupt.EN_PARAM_INTERRUPT.NAME, strResult))
						{
							m_labelName.Text	= strResult;
							m_dgViewInterrupt[COLUMN_INDEX_OF_NAME, m_nIndexOfSelectedRows].Value	= strResult;
						}
					}
					break;
				case 1:	// INPUT TARGET
					if(false == m_InstanceOfDigitalIO.GetListOfItem(true, ref nArrIndex)
						|| false == m_InstanceOfDigitalIO.GetListOfName(true, ref strArrName))
					{
						return;
					}
					if(m_InstanceOfSelectionList.CreateForm(m_lblInput.Text, strArrName, nArrIndex, RemoveBrakets(m_labelInput.SubText)))
					{
						m_InstanceOfSelectionList.GetResult(ref nValue);
						m_InstanceOfInterrupt.SetParameter(m_nIndexOfSelectedItem, FrameOfSystem3.Config.ConfigInterrupt.EN_PARAM_INTERRUPT.TARGET, nValue);
					}
					break;
				case 2:	// CONDITION
					if(m_InstanceOfSelectionList.CreateForm(m_lblCondition.Text, Define.DefineEnumProject.SelectionList.EN_SELECTIONLIST.EQUIPMENT_STATE
						, m_labelCondition.Text, true))
					{
						m_InstanceOfSelectionList.GetResult(ref strResult);
						if(m_InstanceOfInterrupt.SetParameter(m_nIndexOfSelectedItem, FrameOfSystem3.Config.ConfigInterrupt.EN_PARAM_INTERRUPT.CONDITION, strResult))
						{
							m_labelCondition.Text	= strResult;
							m_dgViewInterrupt[COLUMN_INDEX_OF_CONDITION, m_nIndexOfSelectedRows].Value	= strResult;
						}
					}
					break;
				case 3:	// ALARM CODE
					if(m_InstanceOfCalculator.CreateForm(m_labelAlarmCode.Text, MIN_OF_PARAM, MAX_OF_PARAM))
					{
						m_InstanceOfCalculator.GetResult(ref nValue);
						strResult		= nValue.ToString();
						if(m_InstanceOfInterrupt.SetParameter(m_nIndexOfSelectedItem, FrameOfSystem3.Config.ConfigInterrupt.EN_PARAM_INTERRUPT.CODE_ALARM, strResult))
						{
							m_labelAlarmCode.Text = strResult;
							m_dgViewInterrupt[COLUMN_INDEX_OF_CODE, m_nIndexOfSelectedRows].Value	= strResult;
						}
					}
					break;
				case 4:	// ACTION
                    if (m_InstanceOfSelectionList.CreateForm(m_lblAction.Text, Define.DefineEnumProject.SelectionList.EN_SELECTIONLIST.INTERRUPT_ACTION, m_labelAction.Text))
                    {
                        m_InstanceOfSelectionList.GetResult(ref strResult);
                        if (m_InstanceOfInterrupt.SetParameter(m_nIndexOfSelectedItem, FrameOfSystem3.Config.ConfigInterrupt.EN_PARAM_INTERRUPT.ACTION, strResult))
                        {
                            m_labelAction.Text = strResult;
                            m_dgViewInterrupt[COLUMN_INDEX_OF_ACTION, m_nIndexOfSelectedRows].Value = strResult;
                        }
                    }
					break;
                case 5:	// INPUT TARGET
                    if (false == m_InstanceOfDigitalIO.GetListOfItem(false, ref nArrIndex)
                        || false == m_InstanceOfDigitalIO.GetListOfName(false, ref strArrName))
                    {
                        return;
                    }
                    if (m_InstanceOfSelectionList.CreateForm(m_labelDO.Text, strArrName, nArrIndex, RemoveBrakets(m_labelDO.SubText)))
                    {
                        m_InstanceOfSelectionList.GetResult(ref nValue);
                        m_InstanceOfInterrupt.SetParameter(m_nIndexOfSelectedItem, FrameOfSystem3.Config.ConfigInterrupt.EN_PARAM_INTERRUPT.DO_INDEX, nValue);
                    }
                    break;
			}
		}
		#endregion
	}
}
