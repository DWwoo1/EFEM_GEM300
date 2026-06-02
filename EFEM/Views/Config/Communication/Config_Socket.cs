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
    public partial class Config_Socket : UserControlForMainView.CustomView
    {
        public Config_Socket()
        {
            InitializeComponent();

			m_InstaceOfSoket				= FrameOfSystem3.Config.ConfigSocket.GetInstance();
			m_InstanceOfCalculator			= Functional.Form_Calculator.GetInstance();
			m_InstanceOfKeyboard			= Functional.Form_Keyboard.GetInstance();
			m_InstanceOfSelectionList		= Functional.Form_SelectionList.GetInstance();

			#region Mapping
			m_DicOfProtocolParam.Clear();
			foreach(PROTOCOL_TYPE en in Enum.GetValues(typeof(PROTOCOL_TYPE)))
			{
				m_DicOfProtocolParam.Add((int)en, en.ToString());
			}
			#endregion

			_callbackSocketLog = new Socket_.DelegateWritingSocketLog(WritingSocketLog);
			m_InstaceOfSoket.SetLogFunction(ref _callbackSocketLog);

        }

		#region 열거형
		private enum PROTOCOL_TYPE
		{
			UDP			= 0,
			TCP_CLIENT	= 1,
			TCP_SERVER	= 2,
		}
		#endregion

		#region 상수
		const int HEIGHT_OF_ROWS					= 30;

		readonly string	MIN_OF_PARAM				= "0";
		readonly string MAX_OF_PARAM				= "999999";

		#region COLUMN_INDEX
		const int COLUMN_INDEX_OF_INDEX				= 0;
		const int COLUMN_INDEX_OF_ENABLE			= 1;
		const int COLUMN_INDEX_OF_NAME				= 2;
		const int COLUMN_INDEX_OF_PROTOCOL_TYPE		= 3;
		const int COLUMN_INDEX_OF_PROT				= 4;
		const int COLUMN_INDEX_OF_TARGET_PORT		= 5;
		const int COLUMN_INDEX_OF_STATE				= 6;
		#endregion

		const int SELECT_NONE						= -1;

		readonly Color c_clrTrue					= Color.DodgerBlue;
		readonly Color c_clrFalse					= Color.White;
		const int MAX_DISPLAY_LOG_COUNT						= 1000;
		#endregion

		#region 변수
		private int m_nIndexOfSelectedRows					= -1;
		private int m_nIndexOfSelectedItem					= -1;

		private int[] m_nArrOfItem							= null;

		private string m_strPreState						= string.Empty;

		private Dictionary<int, string> m_DicOfProtocolParam	= new Dictionary<int,string>();

		Functional.Form_Calculator m_InstanceOfCalculator			= null;
		Functional.Form_SelectionList m_InstanceOfSelectionList		= null;
		Functional.Form_Keyboard m_InstanceOfKeyboard				= null;
		FrameOfSystem3.Config.ConfigSocket m_InstaceOfSoket				= null;

        Socket_.DelegateWritingSocketLog _callbackSocketLog = null;
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
				m_dgViewSocket.Rows[m_nIndexOfSelectedRows].Selected	= true;
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
            if (false == m_InstaceOfSoket.GetListOfItem(ref m_nArrOfItem))
			{
				return;
			}

			UpdateState();
			// CheckMessage();
        }
        #endregion

		#region 내부인터페이스
		/// <summary>
		/// 2020.06.15 by twkang [ADD] 아이템의 파라미터들로 GridView 를 업데이트한다.
		/// </summary>
		private void UpdateGrid()
		{
			if (false == m_InstaceOfSoket.GetListOfItem(ref m_nArrOfItem))
			{
				return;
			}

			int nCountOfItem = m_nArrOfItem.Length;

			m_dgViewSocket.Rows.Clear();

			for (int i = 0; i < nCountOfItem; ++i)
			{
				AddItemOnGrid(m_nArrOfItem[i]);
			}
		}
		/// <summary>
		/// 2020.07.15 by twkang [ADD] 아이템을 GridView 에 추가한다.
		/// </summary>
		private void AddItemOnGrid(int nIndexOfItem)
		{
			int nIndexOfRow		= m_dgViewSocket.Rows.Count;

			m_dgViewSocket.Rows.Add();

			int nValue			= -1;
			bool bEnable		= false;
			string strValue		= string.Empty;

			m_dgViewSocket.Rows[nIndexOfRow].Height						= HEIGHT_OF_ROWS;
			m_dgViewSocket[COLUMN_INDEX_OF_INDEX, nIndexOfRow].Value	= nIndexOfItem.ToString();

			m_dgViewSocket.Rows[nIndexOfRow].Cells[COLUMN_INDEX_OF_INDEX].Style.BackColor			= Color.Silver;
			m_dgViewSocket.Rows[nIndexOfRow].Cells[COLUMN_INDEX_OF_INDEX].Style.SelectionBackColor	= Color.Silver;
			m_dgViewSocket.Rows[nIndexOfRow].Cells[COLUMN_INDEX_OF_INDEX].Style.SelectionForeColor	= Color.Black;

			m_InstaceOfSoket.GetParameter(nIndexOfItem, ConfigSocket.EN_PARAM_SOCKET.ENABLE, ref bEnable);
			m_dgViewSocket.Rows[nIndexOfRow].Cells[COLUMN_INDEX_OF_ENABLE].Style.BackColor				= bEnable ? c_clrTrue : c_clrFalse;
			m_dgViewSocket.Rows[nIndexOfRow].Cells[COLUMN_INDEX_OF_ENABLE].Style.SelectionBackColor		= bEnable ? c_clrTrue : c_clrFalse;

			if (false == m_InstaceOfSoket.GetParameter(nIndexOfItem, ConfigSocket.EN_PARAM_SOCKET.NAME, ref strValue))
			{
				strValue = string.Empty;
			}
			m_dgViewSocket[COLUMN_INDEX_OF_NAME, nIndexOfRow].Value = strValue;

			if(m_InstaceOfSoket.GetParameter(nIndexOfItem, ConfigSocket.EN_PARAM_SOCKET.PROTOCOL_MODE, ref nValue)
				|| m_DicOfProtocolParam.ContainsKey(nValue))
			{
				m_dgViewSocket[COLUMN_INDEX_OF_PROTOCOL_TYPE, nIndexOfRow].Value = m_DicOfProtocolParam[nValue];
			}

			if (false == m_InstaceOfSoket.GetParameter(nIndexOfItem, ConfigSocket.EN_PARAM_SOCKET.PORT, ref strValue))
			{
				strValue = string.Empty;
			}
			m_dgViewSocket[COLUMN_INDEX_OF_PROT, nIndexOfRow].Value = strValue;

			if (false == m_InstaceOfSoket.GetParameter(nIndexOfItem, ConfigSocket.EN_PARAM_SOCKET.TARGET_PORT, ref strValue))
			{
				strValue = string.Empty;
			}
			m_dgViewSocket[COLUMN_INDEX_OF_TARGET_PORT, nIndexOfRow].Value = strValue;

			m_dgViewSocket.Rows[nIndexOfRow].Selected		= false;
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

			if(m_InstaceOfSoket.RemoveItem(m_nIndexOfSelectedItem))
			{
				m_dgViewSocket.Rows.RemoveAt(m_nIndexOfSelectedRows);
				if(null != m_dgViewSocket.CurrentCell)
				{
					m_dgViewSocket.Rows[m_dgViewSocket.CurrentRow.Index].Selected	= false;
				}

				return true;
			}
			return false;
		}

		void WritingSocketLog(string socketItemID, Socket_.PROTOCOL_TYPE protocolType, bool isSendLog, string message)
		{
			if (m_listMessage.InvokeRequired)
			{
				Socket_.DelegateWritingSocketLog d = new Socket_.DelegateWritingSocketLog(WritingSocketLog);
				m_listMessage.BeginInvoke(d, new object[] { socketItemID, protocolType, isSendLog, message });
			}
			else
			{
				if (isSendLog)
				{
					message = message.Insert(0, socketItemID + " Send Message : ");
				}
				else
				{
					message = message.Insert(0, socketItemID + " Receive Message : ");
				}

				m_listMessage.Items.Add(message);

				if (m_listMessage.Items.Count > MAX_DISPLAY_LOG_COUNT)
				{
					m_listMessage.Items.RemoveAt(0);
				}
			}
		}

        /// <summary>
        /// 2020.06.15 by twkang [ADD] ENABLE, STATE 를 업데이트한다.
        /// </summary>
        /// 
        private void UpdateState()
		{
			string strState		= string.Empty;

			for(int nIndex = 0, nEnd = m_dgViewSocket.Rows.Count; nIndex < nEnd; ++nIndex)
			{
				int nIndexOfItem	= int.Parse(m_dgViewSocket[COLUMN_INDEX_OF_INDEX, nIndex].Value.ToString());

				strState			= m_InstaceOfSoket.GetState(nIndexOfItem).ToString();
				m_dgViewSocket[COLUMN_INDEX_OF_STATE, nIndex].Value = strState;

				if(nIndexOfItem == m_nIndexOfSelectedItem)
				{
					m_labelState.Text = strState;

					if (strState != m_strPreState)
					{
						SetActiveAllControls(ref m_nIndexOfSelectedItem);
					}
				}
			}
		}
		/// <summary>
		/// 2020.06.15 by twkang [ADD] 라벨들의 값을 채운다.
		/// </summary>
		private void SetConfigurationLabels(ref int nSelectedIndex, ref int nRowIndex)
		{
			string strValue					= string.Empty;

			m_labelIndex.Text = m_dgViewSocket[COLUMN_INDEX_OF_INDEX, nRowIndex].Value.ToString();

			m_labelProtocolType.Text		= m_dgViewSocket[COLUMN_INDEX_OF_PROTOCOL_TYPE, nRowIndex].Value.ToString();
			m_labelPort.Text				= m_dgViewSocket[COLUMN_INDEX_OF_PROT, nRowIndex].Value.ToString();
			m_labelTargetPort.Text			= m_dgViewSocket[COLUMN_INDEX_OF_TARGET_PORT, nRowIndex].Value.ToString();
			m_labelName.Text				= m_dgViewSocket[COLUMN_INDEX_OF_NAME, nRowIndex].Value.ToString();

			m_InstaceOfSoket.GetParameter(nSelectedIndex, ConfigSocket.EN_PARAM_SOCKET.TARGET_IP, ref strValue);
			m_labelServerIP.Text			= strValue;
			
			m_InstaceOfSoket.GetParameter(nSelectedIndex, ConfigSocket.EN_PARAM_SOCKET.RETRY_INTERVAL, ref strValue);
			m_labelRetryInterval.Text		= strValue;
			
			m_InstaceOfSoket.GetParameter(nSelectedIndex, ConfigSocket.EN_PARAM_SOCKET.RECEIVE_TIMEOUT, ref strValue);
			m_labelReceiveTimeout.Text		= strValue;

            int nLogType                    = 0;

            m_InstaceOfSoket.GetParameter(nSelectedIndex, ConfigSocket.EN_PARAM_SOCKET.LOG_TYPE, ref nLogType);
            m_labelLogType.Text             = m_InstaceOfSoket.ConvertIntToStringLogType(nLogType);

			SetActiveAllControls(ref nSelectedIndex);
		}
		/// <summary>
		/// 2020.06.15 by twkang [ADD] 사용 유무 및 연결 유무에 따른 컨트롤 상태를 정의한다.
		/// </summary>
		private void SetActiveAllControls(ref int nSelectedIndex)
		{
			bool bEnable	= false;
			m_InstaceOfSoket.GetParameter(nSelectedIndex, ConfigSocket.EN_PARAM_SOCKET.ENABLE, ref bEnable);

			if(bEnable == false)
			{
				SetActiveControls(false);
				SetActiveSocketControls(false);
				SetActiveMessageControls(false);
			}
			else
			{
				ConfigSocket.EN_SOCKET_STATE enState = m_InstaceOfSoket.GetState(nSelectedIndex);

				bool bControl           = true;
				bool bSocketControl     = true;
				bool bMessageContrl     = true;

				m_strPreState	= enState.ToString();

				switch(enState)
				{
					case ConfigSocket.EN_SOCKET_STATE.WAITING_CONNECTION:
						bControl		= false;
						bMessageContrl	= false;
						break;
					case ConfigSocket.EN_SOCKET_STATE.CONNECTED:
					case ConfigSocket.EN_SOCKET_STATE.TIMEOUT_RECEIVE:
					case ConfigSocket.EN_SOCKET_STATE.WAITING_RECEIVE:
						bControl		= false;
						break;
					default:
						bMessageContrl	= false;
						bSocketControl	= false;
						break;
				}
				SetActiveControls(bControl);
				SetActiveSocketControls(bSocketControl);
				SetActiveMessageControls(bMessageContrl);

				m_btnEnable.Enabled         = false;
			}
		}
		/// <summary>
		/// 2020.06.15 by twkang [ADD] 사용 유무 및 연결 유무에 따라 컨트롤 상태를 바꿈
		/// </summary>
		private void SetActiveControls(bool bEnable)
		{
			m_btnEnable.Enabled				= !bEnable;
			m_btnDisable.Enabled			= bEnable;

			m_labelProtocolType.Enabled		= bEnable;
			m_labelName.Enabled				= bEnable;
			m_labelServerIP.Enabled			= bEnable;

			m_labelPort.Enabled				= bEnable;
			m_labelTargetPort.Enabled		= bEnable;
			m_labelRetryInterval.Enabled	= bEnable;
			m_labelReceiveTimeout.Enabled	= bEnable;

            m_labelLogType.Enabled          = bEnable;

			m_btnRemove.Enabled				= bEnable;

		}
		/// <summary>
		/// 2020.06.15 by twkang [ADD] 연결 상태에 따른 접속 상태 설정
		/// </summary>
		private void SetActiveSocketControls(bool bConnect)
		{
			m_btnConnect.Enabled = !bConnect;
			m_btnDisconnect.Enabled = bConnect;
		}
		/// <summary>
		/// 2020.06.15 by twkang [ADD] 연결 상태에 따른 메시지 입력 상태 설정
		/// </summary>
		private void SetActiveMessageControls(bool bEnable)
		{
			m_labelMessage.Enabled = bEnable;
			m_btnSend.Enabled = bEnable;
			m_btnClearMessage.Enabled = bEnable;
		}
		/// <summary>
		/// 2020.06.15 by twkang [ADD] 선택된 아이템을 초기화한다.
		/// </summary>
		private void ResetSelectedItem()
		{
			m_nIndexOfSelectedItem		= -1;
			m_nIndexOfSelectedRows		= -1;

			m_labelIndex.Text			= "";
			m_labelState.Text			= "--";

			m_labelProtocolType.Text	= "--";

			m_labelPort.Text			= "--";
			m_labelServerIP.Text		= "--";

			m_labelTargetPort.Text		= "--";

			m_labelReceiveTimeout.Text	= "--";
			m_labelRetryInterval.Text	= "--";

			SetActiveControls(false);
			SetActiveSocketControls(false);
			SetActiveMessageControls(false);

			m_btnEnable.Enabled			= false;
			m_btnConnect.Enabled		= false;

			m_labelMessage.Text			= "--";
			m_listMessage.Items.Clear();
		}
		#endregion

		#region UI이벤트
		/// <summary>
		/// 2020.06.15 by twkang [ADD] 그리드 뷰 더블 클릭 이벤트
		/// </summary>
		private void Click_Dgview(object sender, DataGridViewCellEventArgs e)
		{
			int nRowIndex   = e.RowIndex;

			if (nRowIndex < 0 || nRowIndex >= m_dgViewSocket.RowCount) { return; }

			if (nRowIndex == m_nIndexOfSelectedRows) return;

			int nIndex	= int.Parse(m_dgViewSocket[COLUMN_INDEX_OF_INDEX, nRowIndex].Value.ToString());

			m_nIndexOfSelectedRows = nRowIndex;
			m_nIndexOfSelectedItem = nIndex;

			SetConfigurationLabels(ref nIndex, ref nRowIndex);
		}
		/// <summary>
		/// 2020.06.15 by twkang [ADD] Enable, Disable 버튼 클릭 이벤트
		/// </summary>
		private void Click_EnableOrDisable(object sender, EventArgs e)
		{
			Control ctrl	= sender as Control;

			bool bEnable	= false;

			switch(ctrl.TabIndex)
			{
				case 0: //ENABLE
					bEnable		= true;
					break;
				case 1: //DISABLE
					break;
			}
			m_InstaceOfSoket.SetParameter(m_nIndexOfSelectedItem, ConfigSocket.EN_PARAM_SOCKET.ENABLE, bEnable);
			m_dgViewSocket.Rows[m_nIndexOfSelectedRows].Cells[COLUMN_INDEX_OF_ENABLE].Style.BackColor			= bEnable ? c_clrTrue : c_clrFalse;
			m_dgViewSocket.Rows[m_nIndexOfSelectedRows].Cells[COLUMN_INDEX_OF_ENABLE].Style.SelectionBackColor	= bEnable ? c_clrTrue : c_clrFalse;

			SetActiveAllControls(ref m_nIndexOfSelectedItem);
		}
		/// <summary>
		/// 2020.06.15 by twkang [ADD] ADD, REMOVE 버튼 클릭 이벤트
		/// </summary>
		private void Click_AddOrRemove(object sender, EventArgs e)
		{
			Control ctrl = sender as Control;

			switch(ctrl.TabIndex)
			{
				case 0: //ADD
					int nIndexOfItem = m_InstaceOfSoket.AddSocketItem();
					if(SELECT_NONE != nIndexOfItem)
					{
						AddItemOnGrid(nIndexOfItem);
					}					
					break;
				case 1: //REMOVE
					RemoveItemOnGrid();
					break;
			}
			ResetSelectedItem();
		}
		/// <summary>
		/// 2020.06.15 by twkang [ADD] 연결/연결해제 버튼 클릭 이벤트
		/// </summary>
		private void Click_ConnectOrDisconnect(object sender, EventArgs e)
		{
			Control ctrl = sender as Control;

			switch(ctrl.TabIndex)
			{
				case 0: // CONNECT
					m_InstaceOfSoket.Connect(ref m_nIndexOfSelectedItem);
					break;
				case 1: // DISCONNECT
					m_InstaceOfSoket.DisConnect(ref m_nIndexOfSelectedItem);
					break;
			}			
		}
		/// <summary>
		/// 2020.06.15 by twkang [ADD] 메시지창 (입력, 보내기, 초기화) 클릭 이벤트
		/// </summary>
		private void Click_SendOrClearMessage(object sender, EventArgs e)
		{
			Control ctrl		= sender as Control;
			string strResult	= string.Empty;

			switch(ctrl.TabIndex)
			{
				case 0: // 문자입력
					if(m_InstanceOfKeyboard.CreateForm(m_labelMessage.Text))
					{
						m_InstanceOfKeyboard.GetResult(ref strResult);

						m_labelMessage.Text = strResult;
					}
					break;
				case 1: // SEND 버튼
					if(m_InstaceOfSoket.send(m_nIndexOfSelectedItem, m_labelMessage.Text))
					{
// 						string strData	= m_nIndexOfSelectedItem.ToString() + " Send Message : " + m_labelMessage.Text;
// 						m_listMessage.Items.Add(strData);
						m_labelMessage.ResetText();
					}
					break;
				case 2: // Clear Message 버튼
					m_listMessage.Items.Clear();
					break;
			}
		}
		/// <summary>
		/// 2020.06.15 by twkang [ADD] 설정 라벨들을 클릭 했을 경우
		/// </summary>
		private void Click_Configuration(object sender, EventArgs e)
		{
			Control ctrl = sender as Control;

			string strResult	= string.Empty;
			int nResult			= -1;

			switch(ctrl.TabIndex)
			{
				case 0: // NAME
					if(m_InstanceOfKeyboard.CreateForm(m_labelName.Text))
					{
						m_InstanceOfKeyboard.GetResult(ref strResult);
						if(m_InstaceOfSoket.SetParameter(m_nIndexOfSelectedItem, ConfigSocket.EN_PARAM_SOCKET.NAME, strResult))
						{
							m_dgViewSocket[COLUMN_INDEX_OF_NAME, m_nIndexOfSelectedRows].Value	= strResult;
							m_labelName.Text	= strResult;

						}
					}
					break;
				case 1:	// PROTOCOL TYPE
					if(m_InstanceOfSelectionList.CreateForm(m_lblProtocolType.Text
						, Define.DefineEnumProject.SelectionList.EN_SELECTIONLIST.SOCKET_PROTOCOL_TYPE
						, m_labelProtocolType.Text))
					{
						m_InstanceOfSelectionList.GetResult(ref strResult);
						if(m_InstaceOfSoket.SetParameter(m_nIndexOfSelectedItem, ConfigSocket.EN_PARAM_SOCKET.PROTOCOL_MODE, strResult))
						{
							m_dgViewSocket[COLUMN_INDEX_OF_PROTOCOL_TYPE, m_nIndexOfSelectedRows].Value	= strResult;
							m_labelProtocolType.Text	= strResult;
						}
					}
					break;	
				case 2:	// TARGET IP
					if(m_InstanceOfKeyboard.CreateForm(m_labelServerIP.Text))
					{
						m_InstanceOfKeyboard.GetResult(ref strResult);
						if(m_InstaceOfSoket.SetParameter(m_nIndexOfSelectedItem, ConfigSocket.EN_PARAM_SOCKET.TARGET_IP, strResult))
						{
							m_labelServerIP.Text	= strResult;
						}
					}
					break;
				case 3:	// PORT
					if(m_InstanceOfCalculator.CreateForm(m_labelPort.Text, MIN_OF_PARAM, MAX_OF_PARAM))
					{
						m_InstanceOfCalculator.GetResult(ref nResult);
						strResult	= nResult.ToString();
						if(m_InstaceOfSoket.SetParameter(m_nIndexOfSelectedItem, ConfigSocket.EN_PARAM_SOCKET.PORT, strResult))
						{
							m_dgViewSocket[COLUMN_INDEX_OF_PROT, m_nIndexOfSelectedRows].Value	= strResult;
							m_labelPort.Text	= strResult;
						}
					}
					break;
				case 4:	// TARGET PORT
					if(m_InstanceOfCalculator.CreateForm(m_labelTargetPort.Text, MIN_OF_PARAM, MAX_OF_PARAM))
					{
						m_InstanceOfCalculator.GetResult(ref nResult);
						strResult	= nResult.ToString();
						if(m_InstaceOfSoket.SetParameter(m_nIndexOfSelectedItem, ConfigSocket.EN_PARAM_SOCKET.TARGET_PORT, strResult))
						{
							m_dgViewSocket[COLUMN_INDEX_OF_TARGET_PORT, m_nIndexOfSelectedRows].Value	= strResult;
							m_labelTargetPort.Text	= strResult;
						}
					}
					break;
				case 5:	// RETRY INTERVAL
					if(m_InstanceOfCalculator.CreateForm(m_labelRetryInterval.Text, MIN_OF_PARAM, MAX_OF_PARAM))
					{
						m_InstanceOfCalculator.GetResult(ref nResult);
						strResult	=  nResult.ToString();
						if(m_InstaceOfSoket.SetParameter(m_nIndexOfSelectedItem, ConfigSocket.EN_PARAM_SOCKET.RETRY_INTERVAL, strResult))
						{
							m_labelRetryInterval.Text	= strResult;
						}
					}
					break;
				case 6:	// TIMEOUT
					if(m_InstanceOfCalculator.CreateForm(m_labelReceiveTimeout.Text, MIN_OF_PARAM, MAX_OF_PARAM))
					{
						m_InstanceOfCalculator.GetResult(ref nResult);
						strResult	= nResult.ToString();
						if(m_InstaceOfSoket.SetParameter(m_nIndexOfSelectedItem, ConfigSocket.EN_PARAM_SOCKET.RECEIVE_TIMEOUT, strResult))
						{
							m_labelReceiveTimeout.Text	= strResult;
						}
					}
					break;

                case 7: // LOG TYPE
                    if (m_InstanceOfSelectionList.CreateForm(m_lblLogType.Text
                        , Define.DefineEnumProject.SelectionList.EN_SELECTIONLIST.SOCKET_LOG_TYPE
                        , m_labelLogType.Text))
                    {
                        m_InstanceOfSelectionList.GetResult(ref strResult);

                        if (m_InstaceOfSoket.SetParameter(m_nIndexOfSelectedItem, ConfigSocket.EN_PARAM_SOCKET.LOG_TYPE, strResult))
                        {
                            m_labelLogType.Text    = strResult;
                        }
                    }
                    break;
			}
		}
		#endregion
    }
}
