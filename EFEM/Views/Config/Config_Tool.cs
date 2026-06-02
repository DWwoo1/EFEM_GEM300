using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using FrameOfSystem3.Tool;

namespace FrameOfSystem3.Views.Config
{
    public partial class Config_Tool : UserControlForMainView.CustomView
    {
        #region CONSTANT

        private const int DGVIEW_TOOL_INDEX = 0;
        private const int DGVIEW_TOOL_NAME = 1;

        private const int DGVIEW_ITEM_NAME = 0;
        private const int DGVIEW_ITEM_VALUE = 1;

        private const int SELECT_NONE = -1;
        private const int STARTING_INDEX = 0;

        private readonly Color c_clrTrue = Color.DodgerBlue;
        private readonly Color c_clrFalse = Color.White;
        #endregion

        #region CONSTRUCTOR
        public Config_Tool()
        {
            InitializeComponent();

			_calculator				= Functional.Form_Calculator.GetInstance();
			_keyboard				= Functional.Form_Keyboard.GetInstance();
			_selectionList			= Functional.Form_SelectionList.GetInstance();
			_messageBox				= Functional.Form_MessageBox.GetInstance();
            _dateTimeSelector       = Functional.Form_DateTimeSelector.GetInstance();

            m_ConfigTool = FrameOfSystem3.Config.ConfigTool.GetInstance();
		}
        #endregion

		#region FIELD
		private int m_nSelectedToolListRow = -1;
		private int m_nSelectedToolListIndex = -1;

		Functional.Form_MessageBox _messageBox					= null;
		Functional.Form_Keyboard _keyboard						= null;
		Functional.Form_Calculator _calculator					= null;
		Functional.Form_SelectionList _selectionList			= null;
        Functional.Form_DateTimeSelector _dateTimeSelector      = null;

        FrameOfSystem3.Config.ConfigTool m_ConfigTool = null;              
		#endregion

		#region OVERRIDE
        protected override void ProcessWhenActivation()
        {
            UpdateToolList();

			if(SELECT_NONE != m_nSelectedToolListRow)
			{
                m_dgViewToolList.Rows[m_nSelectedToolListRow].Selected = true;
			}
        }
        protected override void ProcessWhenDeactivation()
        {
        }
        public override void CallFunctionByTimer()
        {
            if (m_nSelectedToolListIndex.Equals(SELECT_NONE))
                return;

            UpdateMonitoringData(m_nSelectedToolListIndex);
        }
        #endregion

		#region METHOD
        private void UpdateToolList()
        {
            m_dgViewToolList.Rows.Clear();

            string[] arToolList = null;

            if (m_ConfigTool.GetToolList(ref arToolList))
            {
                if (arToolList.Length <= 0)
                    return;

                for (int nIndex = 0, nEnd = arToolList.Length; nIndex < nEnd; ++nIndex)
                {
                    string sIndex = arToolList[nIndex];

                    m_dgViewToolList.Rows.Add();

                    m_dgViewToolList[DGVIEW_TOOL_INDEX, nIndex].Value = sIndex;
                    
                    string sItemValue = string.Empty;

                    m_ConfigTool.GetToolName(sIndex, ref sItemValue);

                    m_dgViewToolList[DGVIEW_TOOL_NAME, nIndex].Value = sItemValue;
                }

                m_nSelectedToolListIndex = int.Parse(arToolList[STARTING_INDEX].ToString());

                m_nSelectedToolListRow = STARTING_INDEX;

                UpdateItemList(m_nSelectedToolListIndex);
            }
        }
        private void UpdateItemList(int nSelectedIndex)
        {
            // ITEM LIST
            m_dgViewItemList.Rows.Clear();

            string[] arItemNames = null;
            string[] arItemValues = null;

            m_ConfigTool.GetItemsValue(nSelectedIndex.ToString(), ref arItemNames, ref arItemValues);

            for (int nIndex = 0, nEnd = arItemValues.Length; nIndex < nEnd; ++nIndex)
            {
                m_dgViewItemList.Rows.Add();

                m_dgViewItemList[DGVIEW_ITEM_NAME, nIndex].Value = arItemNames[nIndex];
                m_dgViewItemList[DGVIEW_ITEM_VALUE, nIndex].Value = arItemValues[nIndex];
            }

            // MONITORING DATA
            m_dgViewMonitoringData.Rows.Clear();

            string[] arMonitoringNames = null;
            string[] arMonitoringValues = null;

            m_ConfigTool.GetMonitoringData(nSelectedIndex.ToString(), ref arMonitoringNames, ref arMonitoringValues);

            for (int nIndex = 0, nEnd = arMonitoringValues.Length; nIndex < nEnd; ++nIndex)
            {
                m_dgViewMonitoringData.Rows.Add();

                m_dgViewMonitoringData[DGVIEW_ITEM_NAME, nIndex].Value = arMonitoringNames[nIndex];
                m_dgViewMonitoringData[DGVIEW_ITEM_VALUE, nIndex].Value = arMonitoringValues[nIndex];
            }

            bool isEnable;
            m_ConfigTool.GetEnable(nSelectedIndex, out isEnable);
            btn_Enable.Enabled = false == isEnable;
            btn_Disable.Enabled = isEnable;
        }
        private void UpdateMonitoringData(int nSelectedIndex)
        {
            string[] arMonitoringNames = null;
            string[] arMonitoringValues = null;

            m_ConfigTool.GetMonitoringData(nSelectedIndex.ToString(), ref arMonitoringNames, ref arMonitoringValues);

            for (int nIndex = 0, nEnd = arMonitoringValues.Length; nIndex < nEnd; ++nIndex)
            {
                m_dgViewMonitoringData[DGVIEW_ITEM_NAME, nIndex].Value = arMonitoringNames[nIndex];

				if(nIndex == 6)
					m_dgViewMonitoringData[DGVIEW_ITEM_VALUE, nIndex].Value = arMonitoringValues[nIndex].Substring(0, 8);
				else
					m_dgViewMonitoringData[DGVIEW_ITEM_VALUE, nIndex].Value = arMonitoringValues[nIndex];
            }
        }
		#endregion

		#region UI EVENT
        private void Click_dgViewToolList(object sender, DataGridViewCellEventArgs e)
        {
            int nRowIndex = e.RowIndex;

            if (nRowIndex < 0 || nRowIndex >= m_dgViewToolList.RowCount)
                return;

            if (m_nSelectedToolListRow.Equals(nRowIndex)) 
                return;

            m_nSelectedToolListIndex = int.Parse(m_dgViewToolList[DGVIEW_TOOL_INDEX, nRowIndex].Value.ToString());

            m_nSelectedToolListRow = nRowIndex;

            UpdateItemList(m_nSelectedToolListIndex);
        }
		private void Click_Configuration(object sender, DataGridViewCellEventArgs e)
		{
			if (m_nSelectedToolListIndex < 0)
				return;

			if (e.RowIndex < 0 || e.RowIndex >= m_dgViewItemList.RowCount)
				return;

            if (false == Enum.IsDefined(typeof(EN_TOOL_PARAM), e.RowIndex))
                return;

            var param = (EN_TOOL_PARAM)e.RowIndex;
            string newValue = string.Empty;
            string oldValue = string.Empty;

            if (false == m_ConfigTool.LoadItem(m_nSelectedToolListIndex.ToString(), param.ToString(), ref oldValue))
                return;

            switch (param)
			{
				case EN_TOOL_PARAM.NAME:
                    if (false == _keyboard.CreateForm(param.ToString()))
                        return;

                    _keyboard.GetResult(ref newValue);
					break;
				case EN_TOOL_PARAM.ENABLE:
				case EN_TOOL_PARAM.STARTUP:
				case EN_TOOL_PARAM.STANDSTILL:
				case EN_TOOL_PARAM.EVENT_EXCHANGE:
					if (false == _selectionList.CreateForm(param.ToString(), Define.DefineEnumProject.SelectionList.EN_SELECTIONLIST.TRUE_FALSE, oldValue))
                        return;

					_selectionList.GetResult(ref newValue);
					break;

				case EN_TOOL_PARAM.USAGE_TYPE:
					if (false == _selectionList.CreateForm(param.ToString(), Enum.GetNames(typeof(EN_USAGE_TYPE)), oldValue))
						return;

					_selectionList.GetResult(ref newValue);
					break;
				case EN_TOOL_PARAM.WORK_END:
					if (false == _selectionList.CreateForm(param.ToString(), Enum.GetNames(typeof(EN_WORK_END)), oldValue))
						return;

					_selectionList.GetResult(ref newValue);
					break;
				case EN_TOOL_PARAM.NOTICE_LIMIT_COUNT:
				case EN_TOOL_PARAM.NOTICE_ALARM_CODE:
				case EN_TOOL_PARAM.NOTICE_INTERVAL_COUNT:
				case EN_TOOL_PARAM.WARNING_LIMIT_COUNT:
				case EN_TOOL_PARAM.WARNING_ALARM_CODE:
				case EN_TOOL_PARAM.WARNING_INTERVAL_COUNT:
                    if (false == _calculator.CreateForm(oldValue, "1", "10000000", "", param.ToString()))
                        return;

                    _calculator.GetResult(ref newValue);
					break;
				case EN_TOOL_PARAM.NOTICE_LIMIT_TIME:
				case EN_TOOL_PARAM.NOTICE_INTERVAL_TIME:
				case EN_TOOL_PARAM.WARNING_LIMIT_TIME:
				case EN_TOOL_PARAM.WARNING_INTERVAL_TIME:
                    {
                        TimeSpan spOld;
                        if (false == TimeSpan.TryParse(oldValue, out spOld)) spOld = TimeSpan.Zero;
                        if (false == _dateTimeSelector.CreateForm(spOld, Functional.Form_DateTimeSelector.EShowType.Hour, param.ToString()))
                            return;

                        _dateTimeSelector.GetResult(ref newValue);
                    }
					break;
				default: return;
			}

			if (string.IsNullOrWhiteSpace(newValue))
                return;

            if (false == _messageBox.ShowMessage(string.Format("Do you change value?\n{0} : {1} > {2}", param.ToString(), oldValue, newValue)))
                return;

            m_ConfigTool.SaveItem(m_nSelectedToolListIndex, param.ToString(), newValue);
			UpdateItemList(m_nSelectedToolListIndex);
		}
		private void Click_dgViewMonitoringData(object sender, DataGridViewCellEventArgs e)
		{
            if (e.RowIndex != 0) return;    // tool id만 설정 가능하도록

            string strValue = string.Empty;
            if (false == m_ConfigTool.GetToolId(m_nSelectedToolListIndex, ref strValue))
                return;

            if (false == _keyboard.CreateForm(strValue, 200, false, "New tool ID"))
                return;

            _keyboard.GetResult(ref strValue);
            m_ConfigTool.SetToolId(m_nSelectedToolListIndex, strValue);
		}

		private void Click_Reset(object sender, EventArgs e)
		{
			if (m_nSelectedToolListIndex < 0) return;

			m_ConfigTool.ResetItem(m_nSelectedToolListIndex);
		}
        private void Click_EnableDisable(object sender, EventArgs e)
		{
            if (m_nSelectedToolListIndex < 0)
                return;

            if (sender == btn_Enable)
            {
                m_ConfigTool.SetEnable(m_nSelectedToolListIndex, true);
            }
            else if (sender == btn_Disable)
            {
				m_ConfigTool.SetEnable(m_nSelectedToolListIndex, false);
			}
			else return;

			UpdateItemList(m_nSelectedToolListIndex);
		}
		#endregion

	}
}
