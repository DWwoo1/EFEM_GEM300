using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FrameOfSystem3.Views.Config.MotionPanel
{
    public partial class MotorGantry : UserControlForMainView.CustomView
    {
        public MotorGantry()
        {
            InitializeComponent();

			m_InstanceOfMotion			= FrameOfSystem3.Config.ConfigMotion.GetInstance();
			m_InstanceOfSelectionList	= Functional.Form_SelectionList.GetInstance();
        }

		#region 상수
		private readonly string DEFALUT_INDEX					= "[ -1 ]";
		private readonly string DEFALUT_VALUE					= "--";
		private const int SELECT_NONE							= -1;

		#endregion

		#region 변수
		private int m_nIndexOfSelectedItem						= SELECT_NONE;
		
		Functional.Form_SelectionList m_InstanceOfSelectionList	= null;
		FrameOfSystem3.Config.ConfigMotion m_InstanceOfMotion	= null;
		#endregion

		#region 외부인터페이스
		/// <summary>
		/// 2020.06.25 by twkang [ADD] 컨트롤들의 활성 유무를 설정한다.
		/// </summary>
		public void SetActiveControls(bool bEnable)
		{
			m_ToggleEnableGantry.Enabled	= bEnable;
			m_btnResetGantry.Enabled		= bEnable;
			if(bEnable)
			{
				SetEnableItemLabel(m_ToggleEnableGantry.Active);
			}
			else
			{
				SetEnableItemLabel(false);
			}
		}
		/// <summary>
		/// 2020.06.25 by twkang [ADD] 컨트롤들의 값을 설정한다.
		/// </summary>
		public void SetValuesToControls(int nIndexOfItem)
		{
			m_nIndexOfSelectedItem	= nIndexOfItem;
			
			bool bResult			= false;
			int nResult				= -1;
			string strValue			= string.Empty;

			if(m_InstanceOfMotion.GetGantryParameter(nIndexOfItem, FrameOfSystem3.Config.ConfigMotion.EN_PARAM_GANTRY.ENABLE, ref bResult))
			{
				m_ToggleEnableGantry.Active	= bResult;
			}

			if(m_InstanceOfMotion.GetGantryParameter(nIndexOfItem, FrameOfSystem3.Config.ConfigMotion.EN_PARAM_GANTRY.INDEX_MASTER, ref nResult))
			{
				if(m_InstanceOfMotion.GetMotionParameter(nResult, FrameOfSystem3.Config.ConfigMotion.EN_PARAM_MOTION.NAME, ref strValue))
				{
					m_labelMasterItem.SubText	= "[ " + nResult + " ]";
					m_labelMasterItem.Text		= strValue;
				}
				else
				{
					m_labelMasterItem.SubText	= DEFALUT_INDEX;
					m_labelMasterItem.Text		= DEFALUT_VALUE;
				}
				m_ledMaster.Active	= nIndexOfItem == nResult;
			}

			if(m_InstanceOfMotion.GetGantryParameter(nIndexOfItem, FrameOfSystem3.Config.ConfigMotion.EN_PARAM_GANTRY.INDEX_SLAVE, ref nResult))
			{
				if(m_InstanceOfMotion.GetMotionParameter(nResult, FrameOfSystem3.Config.ConfigMotion.EN_PARAM_MOTION.NAME, ref strValue))
				{
					m_labelSlaveItem.SubText	= "[ " + nResult + " ]";
					m_labelSlaveItem.Text		= strValue;
				}
				else
				{
					m_labelSlaveItem.SubText	= DEFALUT_INDEX;
					m_labelSlaveItem.Text		= DEFALUT_VALUE;
				}
				m_ledSlave.Active	= nIndexOfItem == nResult;
			}

			strValue		= DEFALUT_VALUE;
			if(m_InstanceOfMotion.GetGantryParameter(nIndexOfItem, FrameOfSystem3.Config.ConfigMotion.EN_PARAM_GANTRY.REVERSE_SLAVE_DIR, ref bResult))
			{
				strValue	= bResult ? Define.DefineConstant.SelectionList.ENABLE : Define.DefineConstant.SelectionList.DISABLE;
			}
			m_labelSlaveInverse.Text		= strValue;
		}
		#endregion

		#region 내부인터페이스
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
		/// 
		/// </summary>
		private void SetEnableItemLabel(bool bEnable)
		{
			if(m_ledSlave.Active)
			{
				m_labelSlaveItem.Enabled	= false;
				m_labelSlaveInverse.Enabled	= false;
			}
			else
			{
				m_labelSlaveItem.Enabled	= bEnable;
				m_labelSlaveInverse.Enabled = bEnable;
			}
		}
		/// <summary>
		/// 
		/// </summary>
		private void SetSlaveItem(int nIndexOfSlave)
		{
			int nIndexOfSlavedAxis	= RemoveBrakets(m_labelSlaveItem.SubText);
			if(nIndexOfSlavedAxis != SELECT_NONE)
			{
				m_InstanceOfMotion.SetGantryParameter(nIndexOfSlavedAxis, FrameOfSystem3.Config.ConfigMotion.EN_PARAM_GANTRY.ENABLE, false);
				m_InstanceOfMotion.SetGantryParameter(nIndexOfSlavedAxis, FrameOfSystem3.Config.ConfigMotion.EN_PARAM_GANTRY.INDEX_MASTER, SELECT_NONE);
				m_InstanceOfMotion.SetGantryParameter(nIndexOfSlavedAxis, FrameOfSystem3.Config.ConfigMotion.EN_PARAM_GANTRY.INDEX_SLAVE, SELECT_NONE);
			}

			m_InstanceOfMotion.SetGantryParameter(m_nIndexOfSelectedItem, FrameOfSystem3.Config.ConfigMotion.EN_PARAM_GANTRY.INDEX_MASTER, m_nIndexOfSelectedItem);
			m_InstanceOfMotion.SetGantryParameter(m_nIndexOfSelectedItem, FrameOfSystem3.Config.ConfigMotion.EN_PARAM_GANTRY.INDEX_SLAVE, nIndexOfSlave);
			m_InstanceOfMotion.SetGantryParameter(nIndexOfSlave, FrameOfSystem3.Config.ConfigMotion.EN_PARAM_GANTRY.ENABLE, true);
			m_InstanceOfMotion.SetGantryParameter(nIndexOfSlave, FrameOfSystem3.Config.ConfigMotion.EN_PARAM_GANTRY.INDEX_MASTER, m_nIndexOfSelectedItem);
			m_InstanceOfMotion.SetGantryParameter(nIndexOfSlave, FrameOfSystem3.Config.ConfigMotion.EN_PARAM_GANTRY.INDEX_SLAVE, nIndexOfSlave);
		}
		#endregion

		#region UI이벤트
		/// <summary>
		/// 2020.06.25 by twkang [ADD] 
		/// </summary>
		private void Click_Configuration(object sender, EventArgs e)
		{
			Control ctrl		= sender as Control;
			
			bool bResult			= false;
			string strValue			= string.Empty;
			string[] strArrName		= null;
			int[] nArrIndex			= null;
			int nSelectedIndex		= -1;

			switch(ctrl.TabIndex)
			{
				case 0: // ENABLE GANTRY
					m_InstanceOfMotion.SetGantryParameter(m_nIndexOfSelectedItem, FrameOfSystem3.Config.ConfigMotion.EN_PARAM_GANTRY.ENABLE, m_ToggleEnableGantry.Active);
					break;
				case 1:	// MASTER ITEM
					//m_InstanceOfMotion.GetListOfItem(ref nArrIndex);
					//m_InstanceOfMotion.GetListOfName(ref strArrName);
					//if(m_InstanceOfSelectionList.CreateForm(m_labelMasterItem.Text, strArrName, nArrIndex, RemoveBrakets(m_labelMasterItem.SubText)))
					//{
					//	m_InstanceOfSelectionList.GetResult(ref nSelectedIndex);
					//	m_InstanceOfSelectionList.GetResult(ref strValue);

					//	if(nSelectedIndex != m_nIndexOfSelectedItem)
					//	{
					//		m_InstanceOfMotion.SetGantryParameter(m_nIndexOfSelectedItem, FrameOfSystem3.Config.ConfigMotion.EN_PARAM_GANTRY.INDEX_MASTER, nSelectedIndex);
					//		m_InstanceOfMotion.SetGantryParameter(m_nIndexOfSelectedItem, FrameOfSystem3.Config.ConfigMotion.EN_PARAM_GANTRY.INDEX_SLAVE, m_nIndexOfSelectedItem);
					//	}

					//}
					break;
				case 2:	// SLAVE ITEM
					m_InstanceOfMotion.GetListOfItem(ref nArrIndex);
					m_InstanceOfMotion.GetListOfName(ref strArrName);
					if(m_InstanceOfSelectionList.CreateForm(m_lblSlaveItem.Text, strArrName, nArrIndex, RemoveBrakets(m_labelSlaveItem.SubText)))
					{
						m_InstanceOfSelectionList.GetResult(ref nSelectedIndex);
						m_InstanceOfSelectionList.GetResult(ref strValue);

						if(nSelectedIndex != m_nIndexOfSelectedItem)
						{
							SetSlaveItem(nSelectedIndex);
						}
					}
					break;
				case 3:	// INVERSE SLAVE DIRECTION
					if(m_InstanceOfSelectionList.CreateForm(m_labelSlaveInverse.Text, Define.DefineEnumProject.SelectionList.EN_SELECTIONLIST.ENABLE_DISABLE, m_labelSlaveInverse.Text))
					{
						m_InstanceOfSelectionList.GetResult(ref strValue);
						bResult	= strValue.Equals(Define.DefineConstant.SelectionList.ENABLE);
						m_InstanceOfMotion.SetGantryParameter(m_nIndexOfSelectedItem, FrameOfSystem3.Config.ConfigMotion.EN_PARAM_GANTRY.REVERSE_SLAVE_DIR, bResult);
						
					}
					break;
				case 4: // RESET GANTRY
					m_InstanceOfMotion.SetGantryParameter(m_nIndexOfSelectedItem, FrameOfSystem3.Config.ConfigMotion.EN_PARAM_GANTRY.ENABLE, false);
					m_InstanceOfMotion.SetGantryParameter(m_nIndexOfSelectedItem, FrameOfSystem3.Config.ConfigMotion.EN_PARAM_GANTRY.INDEX_MASTER, SELECT_NONE);
					m_InstanceOfMotion.SetGantryParameter(m_nIndexOfSelectedItem, FrameOfSystem3.Config.ConfigMotion.EN_PARAM_GANTRY.INDEX_SLAVE, SELECT_NONE);
					m_InstanceOfMotion.SetGantryParameter(m_nIndexOfSelectedItem, FrameOfSystem3.Config.ConfigMotion.EN_PARAM_GANTRY.REVERSE_SLAVE_DIR, false);
					break;
			}
			SetValuesToControls(m_nIndexOfSelectedItem);
			SetEnableItemLabel(m_ToggleEnableGantry.Active);
		}
		#endregion
    }
}
