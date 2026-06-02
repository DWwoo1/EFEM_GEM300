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
    public partial class Config_Trigger : UserControlForMainView.CustomView
    {
        public Config_Trigger()
        {			
			InitializeComponent();

			#region Instance
			m_InstanceOfDigitalIO				= FrameOfSystem3.Config.ConfigDigitalIO.GetInstance();
			m_InstanceOfTrigger					= FrameOfSystem3.Config.ConfigTrigger.GetInstance();
			m_InstanceOfCalculator				= Functional.Form_Calculator.GetInstance();
			m_InstanceOfKeyborad				= Functional.Form_Keyboard.GetInstance();
			m_InstanceOfSelectionList			= Functional.Form_SelectionList.GetInstance();
			m_InstanceOfMessageBox				= Functional.Form_MessageBox.GetInstance();
			#endregion
        }

		#region 상수
		private const int HEIGHT_OF_ROWS						= 30;

		private readonly string MIN_OF_PARAM					= "0";
		private readonly string MAX_OF_PARAM					= "999999";

		#region COLUMN_INDEX
		private const int COLUMN_INDEX_OF_INDEX					= 0;
		private const int COLUMN_INDEX_OF_ENABLE				= 1;
		private const int COLUMN_INDEX_OF_NAME					= 2;
		private const int COLUMN_INDEX_OF_TARGET				= 3;
		private const int COLUMN_INDEX_OF_TARGET_ACT			= 4;
		private const int COLUMN_INDEX_OF_CONDITION				= 5;
		private const int COLUMN_INDEX_OF_LED1					= 6;
		private const int COLUMN_INDEX_OF_BLINKCONDITION		= 7;
		private const int COLUMN_INDEX_OF_LED2					= 8;
		#endregion

		private const int SELECT_NONE							= -1;

		private readonly Color c_clrTrue						= Color.DodgerBlue;
		private readonly Color c_clrFalse						= Color.White;		

		#endregion 

		#region 변수
		private int m_nIndexOfSelectedRows				= -1;
		private int m_nIndexOfSelectedItem				= -1;
		
		private int[] m_nArrOfItem						= null;

		#region Insatnce
		Functional.Form_Calculator m_InstanceOfCalculator			= null;
		Functional.Form_Keyboard m_InstanceOfKeyborad				= null;
		Functional.Form_SelectionList m_InstanceOfSelectionList		= null;
		Functional.Form_MessageBox m_InstanceOfMessageBox			= null;

		FrameOfSystem3.Config.ConfigTrigger m_InstanceOfTrigger			= null;
		FrameOfSystem3.Config.ConfigDigitalIO m_InstanceOfDigitalIO		= null;
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
				m_dgViewInterLock.Rows[m_nIndexOfSelectedRows].Selected		= true;
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
			if (false == m_InstanceOfTrigger.GetListOfItem(ref m_nArrOfItem))
			{
				return;
			}

			int nCountOfItem = m_nArrOfItem.Length;

			m_dgViewInterLock.Rows.Clear();

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
			if (false == m_InstanceOfTrigger.GetListOfItem(ref m_nArrOfItem))
			{
				return;
			}

			int nCountOfItem	= m_nArrOfItem.Length;

			int nTarget			= -1;
			string strTarget	= string.Empty;
			bool bResult		= false;

			for(int i = 0; i < nCountOfItem; i++)
			{
				m_InstanceOfTrigger.GetParameter(m_nArrOfItem[i], FrameOfSystem3.Config.ConfigTrigger.EN_PARAM_TRIGGER.TARGET, ref nTarget);
				m_dgViewInterLock[COLUMN_INDEX_OF_TARGET, i].Value		= nTarget.ToString();

				bResult			= m_InstanceOfDigitalIO.ReadItem(false, nTarget);
				m_dgViewInterLock.Rows[i].Cells[COLUMN_INDEX_OF_TARGET_ACT].Style.BackColor				= bResult ? c_clrTrue : c_clrFalse;
				m_dgViewInterLock.Rows[i].Cells[COLUMN_INDEX_OF_TARGET_ACT].Style.SelectionBackColor	= bResult ? c_clrTrue : c_clrFalse;

				bResult			= m_InstanceOfTrigger.CheckConditionActivation(m_nArrOfItem[i]);
				m_dgViewInterLock.Rows[i].Cells[COLUMN_INDEX_OF_LED1].Style.BackColor			= bResult ? c_clrTrue : c_clrFalse;
				m_dgViewInterLock.Rows[i].Cells[COLUMN_INDEX_OF_LED1].Style.SelectionBackColor	= bResult ? c_clrTrue : c_clrFalse;

				bResult			= m_InstanceOfTrigger.CheckConditionBlinking(m_nArrOfItem[i]);
				m_dgViewInterLock.Rows[i].Cells[COLUMN_INDEX_OF_LED2].Style.BackColor			= bResult ? c_clrTrue : c_clrFalse;
				m_dgViewInterLock.Rows[i].Cells[COLUMN_INDEX_OF_LED2].Style.SelectionBackColor	= bResult ? c_clrTrue : c_clrFalse;

				if(m_nIndexOfSelectedRows == i)
				{
					m_InstanceOfDigitalIO.GetParameter(false, nTarget, FrameOfSystem3.Config.ConfigDigitalIO.EN_PARAM_DIGITALIO.NAME, ref strTarget);
					m_labelTarget.Text		= strTarget;
					m_labelTarget.SubText	= "[ " + nTarget.ToString() + " ]";
					m_ledIO.Active			= m_InstanceOfDigitalIO.ReadItem(false , nTarget);
				}				
			}
		}
		/// <summary>
		/// 2020.07.14 by twkang [ADD] 새로운 아이템을 Grid 에 추가한다.
		/// </summary>
		private void AddItemOnGrid(int nIndexOfItem)
		{
			int nIndexOfRow		= m_dgViewInterLock.Rows.Count;

			m_dgViewInterLock.Rows.Add();

			string strValue		= string.Empty;
			bool bResult		= false;

			m_dgViewInterLock.Rows[nIndexOfRow].Height = HEIGHT_OF_ROWS;
			m_dgViewInterLock[COLUMN_INDEX_OF_INDEX, nIndexOfRow].Value = nIndexOfItem.ToString();
			m_dgViewInterLock.Rows[nIndexOfRow].Cells[COLUMN_INDEX_OF_INDEX].Style.BackColor = Color.Silver;
			m_dgViewInterLock.Rows[nIndexOfRow].Cells[COLUMN_INDEX_OF_INDEX].Style.SelectionBackColor = Color.Silver;
			m_dgViewInterLock.Rows[nIndexOfRow].Cells[COLUMN_INDEX_OF_INDEX].Style.SelectionForeColor = Color.Black;

			m_InstanceOfTrigger.GetParameter(nIndexOfItem, FrameOfSystem3.Config.ConfigTrigger.EN_PARAM_TRIGGER.ENABLE, ref bResult);
			m_dgViewInterLock.Rows[nIndexOfRow].Cells[COLUMN_INDEX_OF_ENABLE].Style.BackColor = bResult ? c_clrTrue : c_clrFalse;
			m_dgViewInterLock.Rows[nIndexOfRow].Cells[COLUMN_INDEX_OF_ENABLE].Style.SelectionBackColor = bResult ? c_clrTrue : c_clrFalse;

			if (false == m_InstanceOfTrigger.GetParameter(nIndexOfItem, FrameOfSystem3.Config.ConfigTrigger.EN_PARAM_TRIGGER.NAME, ref strValue))
			{
				strValue = string.Empty;
			}
			m_dgViewInterLock[COLUMN_INDEX_OF_NAME, nIndexOfRow].Value = strValue;

			if (false == m_InstanceOfTrigger.GetParameter(nIndexOfItem, FrameOfSystem3.Config.ConfigTrigger.EN_PARAM_TRIGGER.TARGET, ref strValue))
			{
				strValue = string.Empty;
			}
			m_dgViewInterLock[COLUMN_INDEX_OF_TARGET, nIndexOfRow].Value = strValue;

			if (false == m_InstanceOfTrigger.GetParameter(nIndexOfItem, FrameOfSystem3.Config.ConfigTrigger.EN_PARAM_TRIGGER.CONDITION_EQUIPMENT, ref strValue))
			{
				strValue = string.Empty;
			}
			m_dgViewInterLock[COLUMN_INDEX_OF_CONDITION, nIndexOfRow].Value = strValue;

			if (false == m_InstanceOfTrigger.GetParameter(nIndexOfItem, FrameOfSystem3.Config.ConfigTrigger.EN_PARAM_TRIGGER.BLINK_CONDITION_EQUIPMENT, ref strValue))
			{
				strValue = string.Empty;
			}
			m_dgViewInterLock[COLUMN_INDEX_OF_BLINKCONDITION, nIndexOfRow].Value = strValue;	

			m_dgViewInterLock.Rows[nIndexOfRow].Selected		= false;
		}
		/// <summary>
		/// 2020.07.15 by twkang [ADD] 선택한 아이템을 지운다.
		/// </summary>
		private bool RemoveItemOnGrid()
		{
			if(SELECT_NONE == m_nIndexOfSelectedRows)
			{
				return false;
			}

			if(m_InstanceOfTrigger.RemoveItem(m_nIndexOfSelectedItem))
			{
				m_dgViewInterLock.Rows.RemoveAt(m_nIndexOfSelectedRows);
				if(null != m_dgViewInterLock.CurrentCell)
				{
					m_dgViewInterLock.Rows[m_dgViewInterLock.CurrentRow.Index].Selected		= false;
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
			string strValue						= string.Empty;

			m_labelKey.Text						= m_dgViewInterLock[COLUMN_INDEX_OF_INDEX, nRowIndex].Value.ToString();			
			m_labelName.Text					= m_dgViewInterLock[COLUMN_INDEX_OF_NAME, nRowIndex].Value.ToString();
            
            m_InstanceOfTrigger.GetParameter(m_nIndexOfSelectedItem, FrameOfSystem3.Config.ConfigTrigger.EN_PARAM_TRIGGER.CHECK_INTERVAL, ref strValue);
            m_labelCheckInterval.Text           = strValue;

			m_InstanceOfTrigger.GetParameter(m_nIndexOfSelectedItem, FrameOfSystem3.Config.ConfigTrigger.EN_PARAM_TRIGGER.INTERVAL, ref strValue);
			m_labelBlinkInterval.Text			= strValue;

			m_labelCondition.Text				= m_dgViewInterLock[COLUMN_INDEX_OF_CONDITION, nRowIndex].Value.ToString();

			m_InstanceOfTrigger.GetParameter(m_nIndexOfSelectedItem, FrameOfSystem3.Config.ConfigTrigger.EN_PARAM_TRIGGER.CONDITION_ALARM, ref strValue);
			m_labelConditionAlarm.Text			= strValue;

			m_labelBlinkCondition.Text			= m_dgViewInterLock[COLUMN_INDEX_OF_BLINKCONDITION, nRowIndex].Value.ToString();
			
			m_InstanceOfTrigger.GetParameter(m_nIndexOfSelectedItem, FrameOfSystem3.Config.ConfigTrigger.EN_PARAM_TRIGGER.BLINK_CONDITION_ALARM, ref strValue);
			m_labelBlinkConditionAlarm.Text		= strValue;

			bool bEnable		= false;
			m_InstanceOfTrigger.GetParameter(m_nIndexOfSelectedItem, FrameOfSystem3.Config.ConfigTrigger.EN_PARAM_TRIGGER.ENABLE, ref bEnable);

			SetActiveControls(bEnable);
			
		}
		/// <summary>
		/// 2020.06.18 by twkang [ADD] 버튼들의 활성화 상태를 설정한다.
		/// </summary>
		private void SetActiveControls(bool bEnable)
		{
			m_btnEnable.Enabled					= !bEnable;
            m_btnDisable.Enabled				= bEnable;

            m_btnRemove.Enabled					= bEnable;

            m_labelBlinkInterval.Enabled        = bEnable;
            m_labelCheckInterval.Enabled        = bEnable;

            m_labelName.Enabled                 = bEnable;
            m_labelTarget.Enabled               = bEnable;
            m_labelCondition.Enabled            = bEnable;
            m_labelConditionAlarm.Enabled       = bEnable;
            m_labelBlinkCondition.Enabled       = bEnable;
            m_labelBlinkConditionAlarm.Enabled  = bEnable;
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
		/// 2020.06.18 by twkang [ADD] 선택 아이템, 라벨을 초기화한다.
		/// </summary>
		private void ResetSelectedItem()
		{
			m_nIndexOfSelectedItem			= -1;
			m_nIndexOfSelectedRows			= -1;

			m_labelKey.Text					= "";
			m_labelName.Text				= "--";
			m_labelBlinkInterval.Text		= "--";

			m_labelTarget.SubText			= "[ -1 ]";
			m_labelTarget.Text				= "--";

			m_labelCondition.Text			= "--";
			m_labelConditionAlarm.Text		= "--";

			m_labelBlinkCondition.Text		= "--";
			m_labelBlinkConditionAlarm.Text	= "--";

			SetActiveControls(false);

			m_btnEnable.Enabled = false;
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

			if (nRowIndex < 0 || nRowIndex >= m_dgViewInterLock.RowCount) { return; }

			if (nRowIndex == m_nIndexOfSelectedRows) return;

			m_nIndexOfSelectedItem = int.Parse(m_dgViewInterLock[COLUMN_INDEX_OF_INDEX, nRowIndex].Value.ToString());

			m_nIndexOfSelectedRows = nRowIndex;

			SetConfigurationLabels(ref nRowIndex);
		}
		/// <summary>
		/// 2020.06.18 by twkang [ADD] ENABLE, DISABLE 버튼 클릭 이벤트
		/// </summary>
		private void Click_EnableOrDisable(object sender, EventArgs e)
		{
			bool bEnable		= false;

			if(m_InstanceOfTrigger.GetParameter(m_nIndexOfSelectedItem, FrameOfSystem3.Config.ConfigTrigger.EN_PARAM_TRIGGER.ENABLE, ref bEnable))
			{
				if(m_InstanceOfTrigger.SetParameter(m_nIndexOfSelectedItem, FrameOfSystem3.Config.ConfigTrigger.EN_PARAM_TRIGGER.ENABLE, !bEnable))
				{
					m_dgViewInterLock.Rows[m_nIndexOfSelectedRows].Cells[COLUMN_INDEX_OF_ENABLE].Style.BackColor			= !bEnable ? c_clrTrue : c_clrFalse;
					m_dgViewInterLock.Rows[m_nIndexOfSelectedRows].Cells[COLUMN_INDEX_OF_ENABLE].Style.SelectionBackColor	= !bEnable ? c_clrTrue : c_clrFalse;
					SetActiveControls(!bEnable);
				}								
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
					int nIndexOfItem = m_InstanceOfTrigger.AddTriggerItem();
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
					if(m_InstanceOfKeyborad.CreateForm(m_labelName.Text))
					{
						m_InstanceOfKeyborad.GetResult(ref strResult);
						if(m_InstanceOfTrigger.SetParameter(m_nIndexOfSelectedItem, FrameOfSystem3.Config.ConfigTrigger.EN_PARAM_TRIGGER.NAME, strResult))
						{
							m_labelName.Text	= strResult;
							m_dgViewInterLock[COLUMN_INDEX_OF_NAME, m_nIndexOfSelectedRows].Value	= strResult;
						}
					}
					break;
				case 1:	// TARGET
					m_InstanceOfDigitalIO.GetListOfItem(false, ref nArrIndex);
					m_InstanceOfDigitalIO.GetListOfName(false, ref strArrName);
					if(m_InstanceOfSelectionList.CreateForm(m_lblTarget.Text, strArrName, nArrIndex, RemoveBrakets(m_labelTarget.SubText)))
					{
						m_InstanceOfSelectionList.GetResult(ref nValue);
						m_InstanceOfTrigger.SetParameter(m_nIndexOfSelectedItem, FrameOfSystem3.Config.ConfigTrigger.EN_PARAM_TRIGGER.TARGET, nValue);
					}
					break;
				case 2:	// CONDITION EQUIP
					if(m_InstanceOfSelectionList.CreateForm(m_lblCondition.SubText, Define.DefineEnumProject.SelectionList.EN_SELECTIONLIST.EQUIPMENT_STATE, m_labelCondition.Text, true))
					{
						m_InstanceOfSelectionList.GetResult(ref strResult);
						if(m_InstanceOfTrigger.SetParameter(m_nIndexOfSelectedItem, FrameOfSystem3.Config.ConfigTrigger.EN_PARAM_TRIGGER.CONDITION_EQUIPMENT, strResult))
						{
							m_labelCondition.Text	= strResult;
							m_dgViewInterLock[COLUMN_INDEX_OF_CONDITION, m_nIndexOfSelectedRows].Value	= strResult;
						}
					}
					break;
				case 3:	// CONDITION ALARM
					if(m_InstanceOfSelectionList.CreateForm(m_labelConditionAlarm.SubText, Define.DefineEnumProject.SelectionList.EN_SELECTIONLIST.ALARM_STATE, m_labelConditionAlarm.Text, true))
					{
						m_InstanceOfSelectionList.GetResult(ref strResult);
						if(m_InstanceOfTrigger.SetParameter(m_nIndexOfSelectedItem, FrameOfSystem3.Config.ConfigTrigger.EN_PARAM_TRIGGER.CONDITION_ALARM, strResult))
						{
							m_labelConditionAlarm.Text	= strResult;							
						}
					}
					break;
				case 4:	// BLINK CONDITION EQUIP
					if(m_InstanceOfSelectionList.CreateForm(m_labelBlinkCondition.SubText, Define.DefineEnumProject.SelectionList.EN_SELECTIONLIST.EQUIPMENT_STATE, m_labelBlinkCondition.Text, true))
					{
						m_InstanceOfSelectionList.GetResult(ref strResult);
						if(m_InstanceOfTrigger.SetParameter(m_nIndexOfSelectedItem, FrameOfSystem3.Config.ConfigTrigger.EN_PARAM_TRIGGER.BLINK_CONDITION_EQUIPMENT, strResult))
						{
							m_labelBlinkCondition.Text	= strResult;
							m_dgViewInterLock[COLUMN_INDEX_OF_BLINKCONDITION, m_nIndexOfSelectedRows].Value	= strResult;
						}
					}
					break;
				case 5:	// BLINK CONDITION ALARM
					if (m_InstanceOfSelectionList.CreateForm(m_labelBlinkConditionAlarm.SubText, Define.DefineEnumProject.SelectionList.EN_SELECTIONLIST.ALARM_STATE, m_labelBlinkConditionAlarm.Text, true))
					{
						m_InstanceOfSelectionList.GetResult(ref strResult);
						if (m_InstanceOfTrigger.SetParameter(m_nIndexOfSelectedItem, FrameOfSystem3.Config.ConfigTrigger.EN_PARAM_TRIGGER.BLINK_CONDITION_ALARM, strResult))
						{
							m_labelBlinkConditionAlarm.Text = strResult;
						}
					}
					break;
			}
		}
		/// <summary>
		/// 2020.06.19 by twkang [ADD] 블링크 인터벌 라벨 클릭 이벤트
		/// </summary>
		private void Click_BlinkInterval(object sender, EventArgs e)
		{
			if (m_nIndexOfSelectedItem < 0) { return; }

            Sys3Controls.Sys3Label label     = sender as Sys3Controls.Sys3Label;

			int nResult		= -1;

			if(m_InstanceOfCalculator.CreateForm(m_labelBlinkInterval.Text, MIN_OF_PARAM, MAX_OF_PARAM))
			{
				m_InstanceOfCalculator.GetResult(ref nResult);

				if(m_InstanceOfTrigger.SetParameter(m_nIndexOfSelectedItem
                    , label.TabIndex == 0 ? FrameOfSystem3.Config.ConfigTrigger.EN_PARAM_TRIGGER.CHECK_INTERVAL
                    : FrameOfSystem3.Config.ConfigTrigger.EN_PARAM_TRIGGER.INTERVAL
                    , nResult))
				{
					label.Text          = nResult.ToString();					
				}
			}
		}
		#endregion
	}
}
