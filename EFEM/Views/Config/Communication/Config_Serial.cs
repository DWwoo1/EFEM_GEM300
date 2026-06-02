using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using FrameOfSystem3.Config;

namespace FrameOfSystem3.Views.Config
{
    public partial class Config_Serial : UserControlForMainView.CustomView
    {
		// 2022.03.26 by shkim. [ADD] 토큰을 적용한 타겟 정의
        private enum EN_TOKEN_TYPE
        {
            SEND_START = 0,
            SEND_END = 1,
            RECEIVE_START = 2,
            RECEIVE_END = 3,
        }
        public Config_Serial()
        {
            InitializeComponent();
			
			m_InstanceOfSerial					= FrameOfSystem3.Config.ConfigSerial.GetInstance();
			m_InstanceOfSelectionList			= Functional.Form_SelectionList.GetInstance();
			m_InstanceOfKeyboard				= Functional.Form_Keyboard.GetInstance();
			m_InstanceOfCalculator				= Functional.Form_Calculator.GetInstance();
			m_InstanceOfMessageBox				= Functional.Form_MessageBox.GetInstance();

			#region mapping
			m_DicForParity.Clear();
			foreach(FrameOfSystem3.Config.ConfigSerial.SERIAL_PARITY en in Enum.GetValues(typeof(FrameOfSystem3.Config.ConfigSerial.SERIAL_PARITY)))
			{
				m_DicForParity.Add((int)en, en.ToString());
			}
			
			m_DicForSTOPBIT.Clear();
			foreach(FrameOfSystem3.Config.ConfigSerial.EN_SERIAL_STOPBIT en in Enum.GetValues(typeof(FrameOfSystem3.Config.ConfigSerial.EN_SERIAL_STOPBIT)))
			{
				m_DicForSTOPBIT.Add((int)en, en.ToString());
			}
			
			m_DicForDatabit.Clear();
			foreach(FrameOfSystem3.Config.ConfigSerial.EN_SERIAL_BYTESIZE en in Enum.GetValues(typeof(FrameOfSystem3.Config.ConfigSerial.EN_SERIAL_BYTESIZE)))
			{
				m_DicForDatabit.Add((int)en, en.ToString());
			}
			
			m_DicForBaudrate.Clear();
			foreach(FrameOfSystem3.Config.ConfigSerial.EN_SERIAL_BAUDRATE en in Enum.GetValues(typeof(FrameOfSystem3.Config.ConfigSerial.EN_SERIAL_BAUDRATE)))
			{
				m_DicForBaudrate.Add((int)en, en.ToString());
			}

			// 2022.03.26. by shkim. [ADD] 토큰 라벨 인덱스
            m_labelSendStart.TabIndex = (int)EN_TOKEN_TYPE.SEND_START;
            m_labelSendEnd.TabIndex = (int)EN_TOKEN_TYPE.SEND_END;
            m_labelReceiveStart.TabIndex = (int)EN_TOKEN_TYPE.RECEIVE_START;
            m_labelReceiveEnd.TabIndex = (int)EN_TOKEN_TYPE.RECEIVE_END;

			#endregion

            m_dgWriteSerial = new Serial_.DelegateWritingSerialLog(WriteSerialLog);
            Serial_.Serial.GetInstance().SetLogFunction(ref m_dgWriteSerial);
        }

        Serial_.DelegateWritingSerialLog m_dgWriteSerial = null;

		#region 상수
		const int HEIGHT_OF_ROWS				= 30;

		readonly string PORT_COM				= "COM";

		readonly string MIN_OF_TIMEOUT			= "0";
		readonly string MAX_OF_TIMEOUT			= "999999";

		#region COLUMN_INDEX
		const int COLUMN_INDEX_OF_INDEX			= 0;
		const int COLUMN_INDEX_OF_ENABLE		= 1;
		const int COLUMN_INDEX_OF_NAME			= 2;
		const int COLUMN_INDEX_OF_PORTNAME		= 3;
		const int COLUMN_INDEX_OF_BAUDRATE		= 4;
		const int COLUMN_INDEX_OF_DATABIT		= 5;
		const int COLUMN_INDEX_OF_STOPBIT		= 6;
		const int COLUMN_INDEX_OF_PARITY		= 7;
		const int COLUMN_INDEX_OF_STATE			= 8;
		#endregion

		const int SELECT_NONE					= -1;

		readonly Color c_clrTrue				= Color.DodgerBlue;
		readonly Color c_clrFalse				= Color.White;		
		const int MAX_DISPLAY_LOG_COUNT = 1000;
		#endregion

		#region 변수
		private int m_nIndexOfSelectedRows				= -1;
		private int m_nIndexOfSelectedItem				= -1;

		private string m_strPreState					= string.Empty;

		private int[] m_nArrOfItem						= null;

		Dictionary<int, string> m_DicForDatabit						= new Dictionary<int,string>();
		Dictionary<int, string> m_DicForBaudrate					= new Dictionary<int,string>();
		Dictionary<int, string> m_DicForSTOPBIT						= new Dictionary<int,string>();
		Dictionary<int, string> m_DicForParity						= new Dictionary<int,string>();

		Functional.Form_Calculator m_InstanceOfCalculator			= null;
		Functional.Form_SelectionList m_InstanceOfSelectionList		= null;
		Functional.Form_Keyboard m_InstanceOfKeyboard				= null;
		FrameOfSystem3.Config.ConfigSerial m_InstanceOfSerial				= null;
		Functional.Form_MessageBox m_InstanceOfMessageBox			= null;
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
				m_dgViewSerial.Rows[m_nIndexOfSelectedRows].Selected		= true;
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
			if (false == m_InstanceOfSerial.GetListOfItem(ref m_nArrOfItem))
			{
				return;
			}

			UpdateState();
            // 2022.04.04. by shkim [DEL] Serial Log 이벤트로 변경
			// CheckMessage();
        }
        #endregion

		#region 내부인터페이스
		/// <summary>
		/// 2020.06.16 by twkang [ADD] 아이템의 파라미터들로 GridView 를 업데이트한다.
		/// </summary>
		private void UpdateGrid()
		{
			if(false == m_InstanceOfSerial.GetListOfItem(ref m_nArrOfItem))
			{
				return;
			}

			int nCountOfItem	= m_nArrOfItem.Length;

			m_dgViewSerial.Rows.Clear();
			
			for(int i = 0; i < nCountOfItem; ++i)
			{
				AddItemOnGrid(m_nArrOfItem[i]);
			}
		}
		/// <summary>
		/// 2020.06.16 by twkang [ADD] ENABLE, STATE 를 업데이트한다.
		/// </summary>
		private void UpdateState()
		{
			string strState		= string.Empty;

			for(int nIndex = 0, nEnd = m_dgViewSerial.Rows.Count; nIndex < nEnd; ++nIndex)
			{
				int nIndexOfItem		= int.Parse(m_dgViewSerial[COLUMN_INDEX_OF_INDEX, nIndex].Value.ToString());

				strState = m_InstanceOfSerial.GetState(nIndexOfItem).ToString();
				m_dgViewSerial[COLUMN_INDEX_OF_STATE, nIndex].Value	= strState;

				if(m_nIndexOfSelectedRows == nIndexOfItem)
				{
					m_labelState.Text	= strState;

					if(strState != m_strPreState)
					{
						SetActiveAllControls(ref m_nIndexOfSelectedItem);
					}
				}

			}
		}

		/// <summary>
		/// 2020.07.15 by twkang [ADD] 아이템을 GridView 에 추가한다.
		/// </summary>
		private void AddItemOnGrid(int nIndexOfItem)
		{
			int nIndexOfRow		= m_dgViewSerial.Rows.Count;

			m_dgViewSerial.Rows.Add();

			string strValue		= string.Empty;
			int nValue			= -1;
			bool bEnable		= false;

			m_dgViewSerial.Rows[nIndexOfRow].Height						= HEIGHT_OF_ROWS;
			m_dgViewSerial[COLUMN_INDEX_OF_INDEX, nIndexOfRow].Value	= nIndexOfItem;

			m_dgViewSerial.Rows[nIndexOfRow].Cells[COLUMN_INDEX_OF_INDEX].Style.BackColor			= Color.Silver;
			m_dgViewSerial.Rows[nIndexOfRow].Cells[COLUMN_INDEX_OF_INDEX].Style.SelectionBackColor	= Color.Silver;
			m_dgViewSerial.Rows[nIndexOfRow].Cells[COLUMN_INDEX_OF_INDEX].Style.SelectionForeColor	= Color.Black;

			m_InstanceOfSerial.GetParameter(nIndexOfItem, ConfigSerial.EN_PARAM_SERIAL.ENABLE, ref bEnable);
			m_dgViewSerial.Rows[nIndexOfRow].Cells[COLUMN_INDEX_OF_ENABLE].Style.BackColor			= bEnable ? c_clrTrue : c_clrFalse;
			m_dgViewSerial.Rows[nIndexOfRow].Cells[COLUMN_INDEX_OF_ENABLE].Style.SelectionBackColor	= bEnable ? c_clrTrue : c_clrFalse;

			if (false == m_InstanceOfSerial.GetParameter(nIndexOfItem, FrameOfSystem3.Config.ConfigSerial.EN_PARAM_SERIAL.NAME, ref strValue))
			{
				strValue = string.Empty;
			}
			m_dgViewSerial[COLUMN_INDEX_OF_NAME, nIndexOfRow].Value = strValue;

			if (false == m_InstanceOfSerial.GetParameter(nIndexOfItem, FrameOfSystem3.Config.ConfigSerial.EN_PARAM_SERIAL.PORT, ref strValue))
			{
				strValue = PORT_COM + SELECT_NONE.ToString();
			}
			else
			{
				strValue = PORT_COM + strValue;
			}
			m_dgViewSerial[COLUMN_INDEX_OF_PORTNAME, nIndexOfRow].Value = strValue;

			if (m_InstanceOfSerial.GetParameter(nIndexOfItem, FrameOfSystem3.Config.ConfigSerial.EN_PARAM_SERIAL.BAUDRATE, ref nValue))
			{
				strValue = m_DicForBaudrate.ContainsKey(nValue) ? m_DicForBaudrate[nValue] : Define.DefineConstant.Common.NONE;
				m_dgViewSerial[COLUMN_INDEX_OF_BAUDRATE, nIndexOfRow].Value = strValue;
			}

			if (m_InstanceOfSerial.GetParameter(nIndexOfItem, FrameOfSystem3.Config.ConfigSerial.EN_PARAM_SERIAL.DATABIT, ref nValue))
			{
				strValue = m_DicForDatabit.ContainsKey(nValue) ? m_DicForDatabit[nValue] : Define.DefineConstant.Common.NONE;
				m_dgViewSerial[COLUMN_INDEX_OF_DATABIT, nIndexOfRow].Value = strValue;
			}

			if (m_InstanceOfSerial.GetParameter(nIndexOfItem, FrameOfSystem3.Config.ConfigSerial.EN_PARAM_SERIAL.STOPBIT, ref nValue))
			{
				strValue = m_DicForSTOPBIT.ContainsKey(nValue) ? m_DicForSTOPBIT[nValue] : Define.DefineConstant.Common.NONE;
				m_dgViewSerial[COLUMN_INDEX_OF_STOPBIT, nIndexOfRow].Value = strValue;
			}


			if (m_InstanceOfSerial.GetParameter(nIndexOfItem, FrameOfSystem3.Config.ConfigSerial.EN_PARAM_SERIAL.PARITYBIT, ref nValue))
			{
				strValue = m_DicForParity.ContainsKey(nValue) ? m_DicForParity[nValue] : Define.DefineConstant.Common.NONE;
				m_dgViewSerial[COLUMN_INDEX_OF_PARITY, nIndexOfRow].Value = strValue;
			}

			m_dgViewSerial.Rows[nIndexOfRow].Selected			= false;
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

			if(m_InstanceOfSerial.RemoveItem(m_nIndexOfSelectedItem))
			{
				m_dgViewSerial.Rows.RemoveAt(m_nIndexOfSelectedRows);
				if(null != m_dgViewSerial.CurrentCell)
				{
					m_dgViewSerial.Rows[m_dgViewSerial.CurrentRow.Index].Selected	= false;
				}
				return true;
			}
			return false;
		}
		/// <summary>
		/// 2020.06.16 by twkang [ADD] 라벨들의 값을 채운다.
		/// </summary>
		private void SetConfigurationLabels(ref int nSelectedIndex, ref int nRowIndex)
		{
			string strValue			= string.Empty;

			m_labelIndex.Text		= m_dgViewSerial[COLUMN_INDEX_OF_INDEX, nRowIndex].Value.ToString();

			m_labelPortName.Text	= m_dgViewSerial[COLUMN_INDEX_OF_PORTNAME, nRowIndex].Value.ToString();
			m_labelName.Text		= m_dgViewSerial[COLUMN_INDEX_OF_NAME, nRowIndex].Value.ToString();
			m_labelBaudRate.Text	= m_dgViewSerial[COLUMN_INDEX_OF_BAUDRATE, nRowIndex].Value.ToString();
			m_labelDataBit.Text		= m_dgViewSerial[COLUMN_INDEX_OF_DATABIT, nRowIndex].Value.ToString();
			m_labelStopBit.Text		= m_dgViewSerial[COLUMN_INDEX_OF_STOPBIT, nRowIndex].Value.ToString();
			m_labelParityBit.Text	= m_dgViewSerial[COLUMN_INDEX_OF_PARITY, nRowIndex].Value.ToString();
			
			m_InstanceOfSerial.GetParameter(nSelectedIndex, FrameOfSystem3.Config.ConfigSerial.EN_PARAM_SERIAL.RECEIVE_TIMEOUT, ref strValue);
			m_labelReceiveTimeout.Text	= strValue;

            int nLogType            = 0;

            m_InstanceOfSerial.GetParameter(nSelectedIndex, FrameOfSystem3.Config.ConfigSerial.EN_PARAM_SERIAL.LOG_TYPE, ref nLogType);
            m_labelLogType.Text     = m_InstanceOfSerial.ConvertIntToStringLogType(nLogType);

            // 2022.03.26. by shkim. [ADD] 토큰 라벨에 값 설정
            SetTokenLabels(nSelectedIndex);

			SetActiveAllControls(ref nSelectedIndex);

		}
		/// <summary>
		/// 2020.06.16 by twkang [ADD] 모든 컨트롤들의 활성상태를 설정한다.
		/// </summary>
		private void SetActiveAllControls(ref int nIndexOfItem)
		{
			bool bEnable	= false;
			m_InstanceOfSerial.GetParameter(nIndexOfItem, FrameOfSystem3.Config.ConfigSerial.EN_PARAM_SERIAL.ENABLE, ref bEnable);

			if(false == bEnable)
			{
				SetActiveConfigControls(false);
				SetActiveMessageControls(false);
				SetActiveSerialControls(false);

				m_btnOpen.Enabled		= false;
			}
			else
			{
				FrameOfSystem3.Config.ConfigSerial.EN_SERIAL_STATE enState       = m_InstanceOfSerial.GetState(nIndexOfItem);

				bool bConfig    = true;
				bool bSerial    = false;
				bool bMessage   = false;

				m_strPreState		= enState.ToString();

				switch(enState)
				{
					case FrameOfSystem3.Config.ConfigSerial.EN_SERIAL_STATE.OPENED:
					case FrameOfSystem3.Config.ConfigSerial.EN_SERIAL_STATE.TIMEOUT_RECEIVE:
					case FrameOfSystem3.Config.ConfigSerial.EN_SERIAL_STATE.WAITING_RECEIVE:
						bConfig		= false;
						bSerial		= true;
						bMessage	= true;
						break;
				}

				SetActiveConfigControls(bConfig);
				SetActiveMessageControls(bMessage);
				SetActiveSerialControls(bSerial);

				m_btnEnable.Enabled	= false;
			}

		}
		/// <summary>
		/// 2020.06.16 by twkang [ADD] 설정 컨트롤들의 활성 상태를 설정한다.
		/// </summary>
		private void SetActiveConfigControls(bool bEnable)
		{
			m_btnEnable.Enabled				= !bEnable;
			m_btnDisable.Enabled			= bEnable;

			m_labelPortName.Enabled			= bEnable;
			m_labelBaudRate.Enabled			= bEnable;
			m_labelDataBit.Enabled			= bEnable;
			m_labelStopBit.Enabled			= bEnable;
			m_labelParityBit.Enabled		= bEnable;

			m_labelName.Enabled				= bEnable;
			m_labelReceiveTimeout.Enabled	= bEnable;

            m_labelLogType.Enabled          = bEnable;

			m_btnRemove.Enabled				= bEnable;

            // 2022.03.25. by shkim. [ADD] 토큰 관련 라벨 설정
            m_labelSendStart.Enabled = bEnable;
            m_labelSendEnd.Enabled = bEnable;
            m_labelReceiveStart.Enabled = bEnable;
            m_labelReceiveEnd.Enabled = bEnable;
		}
		/// <summary>
		/// 2020.06.16 by twkang [ADD] 메시지 컨트롤들의 활성 상태를 설정한다.
		/// </summary>
		private void SetActiveMessageControls(bool bEnable)
		{
			m_labelMessage.Enabled		= bEnable;
			m_btnSend.Enabled			= bEnable;
			m_btnClearMessage.Enabled	= bEnable;
		}
		/// <summary>
		/// 2020.06.16 by twkang [ADD] 시리얼 컨트롤들의 활성 상태를 설정한다.
		/// </summary>
        private void SetActiveSerialControls(bool bOpen)
		{
			m_btnOpen.Enabled			= !bOpen;
			m_btnClose.Enabled			= bOpen;
		}
		/// <summary>
		/// 2020.06.16 by twkang [ADD] 컨트롤들의 값과 상태를 초기화한다.
		/// </summary>
		private void ResetSelectedItem()
		{
			m_nIndexOfSelectedItem			= -1;
			m_nIndexOfSelectedRows			= -1;

			m_labelIndex.Text				= "";
			m_labelState.Text				= "--";

			m_labelPortName.Text			= "--";
			m_labelBaudRate.Text			= "--";
			m_labelDataBit.Text				= "--";
			m_labelStopBit.Text				= "--";
			m_labelParityBit.Text			= "--";
			m_labelName.Text				= "--";
			m_labelReceiveTimeout.Text		= "--";

			m_labelMessage.Text				= "";
			m_listMessage.Items.Clear();

			SetActiveConfigControls(false);
			SetActiveSerialControls(false);
			SetActiveMessageControls(false);

			m_btnEnable.Enabled = false;
			m_btnOpen.Enabled = false;

		}

        // 2022.03.26. by shkim [ADD] 토큰 관련 라벨 값 설정
        private void SetTokenLabels(int nSelectedIndex)
        {
            string strTokens = string.Empty;
            m_InstanceOfSerial.GetParameter(nSelectedIndex, ConfigSerial.EN_PARAM_SERIAL.SEND_START_CMD_CHARACTER, ref strTokens);
            m_labelSendStart.Text = (strTokens == "-1") ? "" : strTokens;

            m_InstanceOfSerial.GetParameter(nSelectedIndex, ConfigSerial.EN_PARAM_SERIAL.SEND_END_CMD_CHARACTER, ref strTokens);
            m_labelSendEnd.Text = (strTokens == "-1") ? "" : strTokens;

            m_InstanceOfSerial.GetParameter(nSelectedIndex, ConfigSerial.EN_PARAM_SERIAL.RECEIVE_START_CMD_CHARACTER, ref strTokens);
            m_labelReceiveStart.Text = (strTokens == "-1") ? "" : strTokens;
            
            m_InstanceOfSerial.GetParameter(nSelectedIndex, ConfigSerial.EN_PARAM_SERIAL.RECEIVE_END_CMD_CHARACTER, ref strTokens);
            m_labelReceiveEnd.Text = (strTokens == "-1") ? "" : strTokens;
        }

		// 2024.11.27. by shkim [ADD] 로그 Leak 발생 위험 개선
		// 2022.04.04. by shkim [ADD] Serial Log 이벤트로 변경
		private void WriteSerialLog(string strID, bool bSend, string strData)
		{
			if (this.InvokeRequired)
			{
				Serial_.DelegateWritingSerialLog d = new Serial_.DelegateWritingSerialLog(WriteSerialLog);
				this.BeginInvoke(d, new object[] { strID, bSend, strData });
			}
			else
			{
				if (false == m_labelName.Text.Equals(strID)) return;

				if (false == bSend)
				{
					strData = string.Format("{0} Read Message : {1}", strID, strData);
				}
				else
				{
					strData = string.Format("{0} Write Message : {1}", strID, strData);
				}

				m_listMessage.Items.Add(strData);

				if (m_listMessage.Items.Count > MAX_DISPLAY_LOG_COUNT)
				{
					m_listMessage.Items.RemoveAt(0);
				}
			}
		}

		#endregion

		#region UI 이벤트
		/// <summary>
		/// 2020.06.15 by twkang [ADD] 그리드 뷰 더블 클릭 이벤트
		/// </summary>
		private void Click_Dgview(object sender, DataGridViewCellEventArgs e)
		{
			int nRowIndex   = e.RowIndex;

			if (nRowIndex < 0 || nRowIndex >= m_dgViewSerial.RowCount) { return; }

			if (nRowIndex == m_nIndexOfSelectedRows) return;

			int nIndex	= int.Parse(m_dgViewSerial[COLUMN_INDEX_OF_INDEX, nRowIndex].Value.ToString());

			m_nIndexOfSelectedRows = nRowIndex;
			m_nIndexOfSelectedItem = nIndex;

			SetConfigurationLabels(ref nIndex, ref nRowIndex);
		}
		/// <summary>
		/// 2020.06.16 by twkang [ADD] ENABLE, DISABLE 버튼 클릭 이벤트
		/// </summary>
		private void Click_EnableOrDisable(object sender, EventArgs e)
		{
			Control ctrl	= sender as Control;

			bool bEnable	= false;

			switch(ctrl.TabIndex)
			{
				case 0: // ENABLE
					bEnable			= true;
					break;
				case 1:	// DISABLE
					break;
			}
			m_InstanceOfSerial.SetParameter(m_nIndexOfSelectedItem, ConfigSerial.EN_PARAM_SERIAL.ENABLE, bEnable);
			m_dgViewSerial.Rows[m_nIndexOfSelectedRows].Cells[COLUMN_INDEX_OF_ENABLE].Style.BackColor			= bEnable ? c_clrTrue : c_clrFalse;
			m_dgViewSerial.Rows[m_nIndexOfSelectedRows].Cells[COLUMN_INDEX_OF_ENABLE].Style.SelectionBackColor	= bEnable ? c_clrTrue : c_clrFalse;

			SetActiveAllControls(ref m_nIndexOfSelectedItem);
		}
		/// <summary>
		/// 2020.06.16 by twkang [ADD] ADD, REMOVE 버튼 클릭 이벤트
		/// </summary>
		private void Click_AddOrRemove(object sender, EventArgs e)
		{
			Control ctrl	= sender as Control;
			
			switch(ctrl.TabIndex)
			{
				case 0: // ADD
					int nItemNum = m_InstanceOfSerial.AddSerialItem();
					if(SELECT_NONE != nItemNum)
					{
						AddItemOnGrid(nItemNum);
					}
					break;
				case 1:	// REMOVE
					RemoveItemOnGrid();
					break;
			}
			ResetSelectedItem();
		}
		/// <summary>
		/// 2020.06.16 by twkang [ADD] OPEN, CLOSE 버튼 클릭 이벤트
		/// </summary>
		private void Click_OpenOrClose(object sender, EventArgs e)
		{
			Control ctrl	= sender as Control;

			switch(ctrl.TabIndex)
			{
				case 0: // OPEN
					if(false == m_InstanceOfSerial.Open(m_nIndexOfSelectedItem))
					{
						m_InstanceOfMessageBox.ShowMessage("FAIL");
					}
					break;
				case 1:	// CLOSE
					m_InstanceOfSerial.Close(m_nIndexOfSelectedItem);
					break;
			}
		}
		/// <summary>
		/// 2020.06.16 by twkang [ADD] 파라미터 설정관련 컨트롤 클릭 시
		/// </summary>
		private void Click_Configuration(object sender, EventArgs e)
		{
			Control ctrl		= sender as Control;
			
			string strResult		= string.Empty;
			int nResult				= -1;
			switch(ctrl.TabIndex)
			{
				case 0: // PORT NAME
					string[] strArrPortName	= null;
					if(m_InstanceOfSerial.GetListOfPortName(ref strArrPortName))
					{
						if(m_InstanceOfSelectionList.CreateForm(m_lblPortName.Text, strArrPortName, m_labelPortName.Text))
						{
							m_InstanceOfSelectionList.GetResult(ref strResult);
							
							if(m_InstanceOfSerial.SetParameter(m_nIndexOfSelectedItem, FrameOfSystem3.Config.ConfigSerial.EN_PARAM_SERIAL.PORT, strResult.Remove(0, 3)))
							{
								m_dgViewSerial[COLUMN_INDEX_OF_PORTNAME, m_nIndexOfSelectedRows].Value = strResult;
								m_labelPortName.Text = strResult;
							}							
						}
					}
					else
					{
						m_InstanceOfMessageBox.ShowMessage("There is no available COM Port in the system!");
					}
					break;
				case 1: // DATA BIT
					if(m_InstanceOfSelectionList.CreateForm(m_lblDataBit.Text
						, Define.DefineEnumProject.SelectionList.EN_SELECTIONLIST.SERIAL_DATA_BIT
						, m_labelDataBit.Text))
					{
						m_InstanceOfSelectionList.GetResult(ref strResult);
						
						if(m_InstanceOfSerial.SetParameter(m_nIndexOfSelectedItem, FrameOfSystem3.Config.ConfigSerial.EN_PARAM_SERIAL.DATABIT, strResult))
						{
							m_dgViewSerial[COLUMN_INDEX_OF_DATABIT, m_nIndexOfSelectedRows].Value	= strResult;
							m_labelDataBit.Text		= strResult;
						}
					}
					break;
				case 2:	// BAUDRATE
					if(m_InstanceOfSelectionList.CreateForm(m_lblBaudRate.Text
						, Define.DefineEnumProject.SelectionList.EN_SELECTIONLIST.SERIAL_BAUDRATE
						, m_labelBaudRate.Text))
					{
						m_InstanceOfSelectionList.GetResult(ref strResult);
						if(m_InstanceOfSerial.SetParameter(m_nIndexOfSelectedItem, FrameOfSystem3.Config.ConfigSerial.EN_PARAM_SERIAL.BAUDRATE, strResult))
						{
							m_dgViewSerial[COLUMN_INDEX_OF_BAUDRATE, m_nIndexOfSelectedRows].Value	= strResult;
							m_labelBaudRate.Text		= strResult;
						}
					}
					break;
				case 3:	// STOP BIT
					if(m_InstanceOfSelectionList.CreateForm(m_lblStopBit.Text
						, Define.DefineEnumProject.SelectionList.EN_SELECTIONLIST.SERIAL_STOPBIT
						, m_labelStopBit.Text))
					{
						m_InstanceOfSelectionList.GetResult(ref strResult);
						if(m_InstanceOfSerial.SetParameter(m_nIndexOfSelectedItem, FrameOfSystem3.Config.ConfigSerial.EN_PARAM_SERIAL.STOPBIT, strResult))
						{
							m_dgViewSerial[COLUMN_INDEX_OF_STOPBIT, m_nIndexOfSelectedRows].Value	= strResult;
							m_labelStopBit.Text			= strResult;
						}
					}
					break;
				case 4:	// PARITY BIT
					if(m_InstanceOfSelectionList.CreateForm(m_lblParityBit.Text
						, Define.DefineEnumProject.SelectionList.EN_SELECTIONLIST.SERIAL_PARITY
						, m_labelParityBit.Text))
					{
						m_InstanceOfSelectionList.GetResult(ref strResult);
						if(m_InstanceOfSerial.SetParameter(m_nIndexOfSelectedItem, FrameOfSystem3.Config.ConfigSerial.EN_PARAM_SERIAL.PARITYBIT, strResult))
						{
							m_dgViewSerial[COLUMN_INDEX_OF_PARITY, m_nIndexOfSelectedRows].Value	= strResult;
							m_labelParityBit.Text		= strResult;
						}
					}
					break;
				case 5:	// NAME
					if(m_InstanceOfKeyboard.CreateForm(m_labelName.Text))
					{
						m_InstanceOfKeyboard.GetResult(ref strResult);
						if(m_InstanceOfSerial.SetParameter(m_nIndexOfSelectedItem, FrameOfSystem3.Config.ConfigSerial.EN_PARAM_SERIAL.NAME, strResult))
						{
							m_dgViewSerial[COLUMN_INDEX_OF_NAME, m_nIndexOfSelectedRows].Value		= strResult;
							m_labelName.Text			= strResult;
						}
					}
					break;
				case 6:	// TIMEOUT
					if(m_InstanceOfCalculator.CreateForm(m_labelReceiveTimeout.Text, MIN_OF_TIMEOUT, MAX_OF_TIMEOUT))
					{
						m_InstanceOfCalculator.GetResult(ref nResult);
						strResult			= nResult.ToString();
						if(m_InstanceOfSerial.SetParameter(m_nIndexOfSelectedItem, FrameOfSystem3.Config.ConfigSerial.EN_PARAM_SERIAL.RECEIVE_TIMEOUT, strResult))
						{
							m_labelReceiveTimeout.Text		= strResult;
						}
					}
					break;

                case 7: // LOG TYPE
                    if (m_InstanceOfSelectionList.CreateForm(m_lblLogType.Text
                        , Define.DefineEnumProject.SelectionList.EN_SELECTIONLIST.SERIAL_LOG_TYPE
                        , m_labelLogType.Text))
                    {
                        m_InstanceOfSelectionList.GetResult(ref strResult);

                        if (m_InstanceOfSerial.SetParameter(m_nIndexOfSelectedItem, FrameOfSystem3.Config.ConfigSerial.EN_PARAM_SERIAL.LOG_TYPE, strResult))
                        {
                            m_labelLogType.Text             = strResult;
                        }
                    }
                    break;
			}
		}
		/// <summary>
		/// 2020.06.17 by twkang [ADD] 보내기/메시지창 초기화 버튼 클릭 이벤트
		/// </summary>
		private void Click_SendOrClearMessage(object sender, EventArgs e)
		{
			Control ctrl		= sender as Control;
			string strResult	= string.Empty;

			switch(ctrl.TabIndex)
			{
				case 0: // 메시지 입력
					if(m_InstanceOfKeyboard.CreateForm(m_labelMessage.Text))
					{
						m_InstanceOfKeyboard.GetResult(ref strResult);
						m_labelMessage.Text	= strResult;
					}
					break;
				case 1:	// 메시지 보내기
					if(m_InstanceOfSerial.Write(m_nIndexOfSelectedItem, m_labelMessage.Text))
					{
// 						string strData = string.Format("{0} Write Message : {1}", m_nIndexOfSelectedItem.ToString(), m_labelMessage.Text);
// 						m_listMessage.Items.Add(strData);
						m_labelMessage.ResetText();
					}
					break;
				case 2:	// 메시지창 클리어
					m_listMessage.Items.Clear();
					break;
			}
				
		}

        // 2022.03.26 by shkim [ADD] 설정하기 원하는 곳에 토큰 설정
        private void Click_TokenSetting(object sender, EventArgs e)
        {
            if(sender is Sys3Controls.Sys3Label)
            {
                Sys3Controls.Sys3Label clickedLabel = sender as Sys3Controls.Sys3Label;
                //if(clickedLabel.TabIndex == null) return;

                string[] arCharacterName = Enum.GetNames(typeof(Serial_.COM_CONTROL_CHARACTER));
                var arrayValues = Enum.GetValues(typeof(Serial_.COM_CONTROL_CHARACTER));
                int[] arIntValues = arrayValues.Cast<int>().ToArray();
                string[] arPreCharacterNames = null;

                int[] selectedArray = null;

				if (false == m_InstanceOfSelectionList.CreateForm(string.Empty, arCharacterName, arIntValues, arPreCharacterNames, true))
					return;

				m_InstanceOfSelectionList.GetResult(ref selectedArray, true);
                switch(clickedLabel.TabIndex)
                {
                    case (int)EN_TOKEN_TYPE.SEND_START:
                        m_InstanceOfSerial.SetParameter(m_nIndexOfSelectedItem, ConfigSerial.EN_PARAM_SERIAL.SEND_START_CMD_CHARACTER, ref selectedArray);
                        break;
                    case (int)EN_TOKEN_TYPE.SEND_END:
                        m_InstanceOfSerial.SetParameter(m_nIndexOfSelectedItem, ConfigSerial.EN_PARAM_SERIAL.SEND_END_CMD_CHARACTER, ref selectedArray);
                        break;
                    case (int)EN_TOKEN_TYPE.RECEIVE_START:
                        m_InstanceOfSerial.SetParameter(m_nIndexOfSelectedItem, ConfigSerial.EN_PARAM_SERIAL.RECEIVE_START_CMD_CHARACTER, ref selectedArray);
                        break;
                    case (int)EN_TOKEN_TYPE.RECEIVE_END:
                        m_InstanceOfSerial.SetParameter(m_nIndexOfSelectedItem, ConfigSerial.EN_PARAM_SERIAL.RECEIVE_END_CMD_CHARACTER, ref selectedArray);
                        break;
                }
                SetTokenLabels(m_nIndexOfSelectedItem);
                // UI 업데이트
            }
        }
		#endregion

    }
}
