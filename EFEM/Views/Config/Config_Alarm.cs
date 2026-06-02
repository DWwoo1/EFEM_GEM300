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
    public partial class Config_Alarm : UserControlForMainView.CustomView
    {
        public Config_Alarm()
        {
            InitializeComponent();

			#region Instance
			m_InstanceOfAlarm				= FrameOfSystem3.Config.ConfigAlarm.GetInstance();
			m_InstanceOfLanguage			= FrameOfSystem3.Config.ConfigLanguage.GetInstance();
			m_InstanceOfDIO					= FrameOfSystem3.Config.ConfigDigitalIO.GetInstance();

			m_InstanceOfMessageBox			= Functional.Form_MessageBox.GetInstance();
			m_InstanceOfSelectionList		= Functional.Form_SelectionList.GetInstance();
			m_InstanceOfCalculator			= Functional.Form_Calculator.GetInstance();
			#endregion
			
		}

		#region 열거형
		#endregion

		#region 상수
		private const int HEIGHT_OF_ROWS				= 30;

		private readonly string MIN_OF_PARAM			= "0";
		private readonly string MAX_OF_PARAM			= "999999";

		#region Column
		private readonly string ASCENDING_ICON			= " ▲";
		private readonly string DESCENDING_ICON			= " ▼";

		private readonly string COLUMN_TEXT_INDEX		= "INDEX";
		private readonly string COLUMN_TEXT_ALARM_CODE	= "ALARM CODE";
		private readonly string COLUMN_TEXT_GRADE		= "GRADE";
		private readonly string COLUMN_TEXT_MESSAGE		= "MESSAGE";
		private readonly string COLUMN_TEXT_SOLUTION	= "SOLUTION";

		#region COLUMN_INDEX
		private const int COLUMN_INDEX_OF_INDEX			= 0;
		private const int COLUMN_INDEX_OF_ALARMCODE		= 1;
		private const int COLUMN_INDEX_OF_GRADE			= 2;
		private const int COLUMN_INDEX_OF_MESSAGE_CODE	= 3;
		private const int COLUMN_INDEX_OF_SOLUTION		= 4;
		private const int COLUMN_INDEX_OF_MESSAGE		= 5;
		private const int COLUMN_INDEX_OF_COUNT			= 6;
		#endregion

		#endregion

		private const int SELECT_NONE					= -1;
		
		private const string SubContent					= "Sub content";
		#endregion

		#region 변수

		#region Instance
		FrameOfSystem3.Config.ConfigAlarm m_InstanceOfAlarm			= null;
		FrameOfSystem3.Config.ConfigLanguage m_InstanceOfLanguage	= null;
		FrameOfSystem3.Config.ConfigDigitalIO m_InstanceOfDIO		= null;

		Functional.Form_Calculator m_InstanceOfCalculator			= null;
		Functional.Form_SelectionList m_InstanceOfSelectionList		= null;
		Functional.Form_MessageBox m_InstanceOfMessageBox			= null;		
		#endregion

		private int m_nCountOfItem						= 0;
		private int m_nIndexOfSelectedRows				= -1;
		private int m_nIndexOfSelectedItem				= -1;

		private int[] m_nArrIndexOfItem					= null;
		#endregion

		#region 상속 인터페이스
		/// <summary>
        /// 
        /// </summary>
        protected override void ProcessWhenActivation()
        {
            InitializeForm();
			if(SELECT_NONE != m_nIndexOfSelectedRows)
			{
				m_dgViewAlarm.Rows[m_nIndexOfSelectedRows].Selected		= true;
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
		/// 2020.06.22 by twkang [ADD] Alarm 아이템들의 값을 갱신한다.
		/// </summary>
		private void InitializeForm()
		{
			if(false == m_InstanceOfAlarm.GetListOfItem(ref m_nArrIndexOfItem))
			{
				return;
			}
			
			m_nCountOfItem	= m_nArrIndexOfItem.Length;

			m_dgViewAlarm.Rows.Clear();

			for(int nIndex = 0; nIndex < m_nCountOfItem; ++nIndex)
			{
				AddItemOnGrid(m_nArrIndexOfItem[nIndex]);
			}
			
			UpdateColumnSortUI(COLUMN_INDEX_OF_INDEX, true);
		}
		/// <summary>
		/// 2020.10.07 by twkang [ADD] 아이템을 GridView 에 추가한다.
		/// </summary>
		private void AddItemOnGrid(int nIndex)
		{
			int nIndexOfRow			= m_dgViewAlarm.Rows.Count;
			string strValue			= string.Empty;
			int alarmCode = -1;

			m_dgViewAlarm.Rows.Add();

			m_dgViewAlarm.Rows[nIndexOfRow].Height						= HEIGHT_OF_ROWS;
			m_dgViewAlarm[COLUMN_INDEX_OF_INDEX, nIndexOfRow].Value		= nIndex.ToString();
			m_dgViewAlarm.Rows[nIndexOfRow].Cells[COLUMN_INDEX_OF_INDEX].Style.BackColor			= Color.Silver;
			m_dgViewAlarm.Rows[nIndexOfRow].Cells[COLUMN_INDEX_OF_INDEX].Style.SelectionBackColor	= Color.Silver;
			m_dgViewAlarm.Rows[nIndexOfRow].Cells[COLUMN_INDEX_OF_INDEX].Style.SelectionForeColor	= Color.Black;

			if (false == m_InstanceOfAlarm.GetParameter(nIndex, FrameOfSystem3.Config.ConfigAlarm.EN_PARAM_ALARM.ALARMCODE, ref alarmCode))
			{
				alarmCode = -1;
			}
			m_dgViewAlarm[COLUMN_INDEX_OF_ALARMCODE, nIndexOfRow].Value = alarmCode.ToString();

			m_dgViewAlarm[COLUMN_INDEX_OF_GRADE, nIndexOfRow].Value = string.Empty;
			if (false == m_InstanceOfAlarm.GetParameter(nIndex, FrameOfSystem3.Config.ConfigAlarm.EN_PARAM_ALARM.GRADE, ref strValue))
			{
				strValue = Define.DefineConstant.Common.NONE;
			}
			m_dgViewAlarm[COLUMN_INDEX_OF_GRADE, nIndexOfRow].Value = strValue;

			if (false == m_InstanceOfAlarm.GetParameter(nIndex, FrameOfSystem3.Config.ConfigAlarm.EN_PARAM_ALARM.MESSAGECODE, ref strValue))
			{
				strValue = string.Empty;
			}
			m_dgViewAlarm[COLUMN_INDEX_OF_MESSAGE_CODE, nIndexOfRow].Value = strValue;

			int nIndexOfItem	= -1;
			if (int.TryParse(strValue, out nIndexOfItem))
			{
				// 2025.07.08. jhlim [MOD] 현재 언어로 번역해보고 실패 -> 영어로 재 번역 -> 공백
                var currentLanguage = m_InstanceOfLanguage.GetLangauge();
                if (false == m_InstanceOfLanguage.GetParameter(FrameOfSystem3.Config.ConfigLanguage.EN_TYPE_FORMAT.SENTENCE, nIndexOfItem, currentLanguage, ref strValue))
                {
                    var fallbackLanguage = FrameOfSystem3.Config.ConfigLanguage.EN_PARAM_LANGUAGE.ENGLISH;

                    if (currentLanguage.Equals(fallbackLanguage) ||
                        false == m_InstanceOfLanguage.GetParameter(FrameOfSystem3.Config.ConfigLanguage.EN_TYPE_FORMAT.SENTENCE, nIndexOfItem, fallbackLanguage, ref strValue))
                    {
                        strValue = string.Empty;
                    }
                }
				//if (false == m_InstanceOfLanguage.GetParameter(FrameOfSystem3.Config.ConfigLanguage.EN_TYPE_FORMAT.SENTENCE, nIndexOfItem, FrameOfSystem3.Config.ConfigLanguage.EN_PARAM_LANGUAGE.ENGLISH, ref strValue))
				//{
				//	strValue = string.Empty;
				//}
				// 2025.07.08. jhlim [END]

				// 2023.05.25 by junho [MOD] token 변환하여 표시하도록 변경
				//m_dgViewAlarm[COLUMN_INDEX_OF_MESSAGE, nIndexOfRow].Value = strValue;
				m_dgViewAlarm[COLUMN_INDEX_OF_MESSAGE, nIndexOfRow].Value = ReplaceTokenOfAlarmMessage(alarmCode, strValue);
			}

			if (false == m_InstanceOfAlarm.GetParameter(nIndex, FrameOfSystem3.Config.ConfigAlarm.EN_PARAM_ALARM.SOLUTIONCODE, ref strValue))
			{
				strValue = string.Empty;
			}
			m_dgViewAlarm[COLUMN_INDEX_OF_SOLUTION, nIndexOfRow].Value = strValue;

			m_dgViewAlarm.Rows[nIndexOfRow].Selected		= false;
		}
		/// <summary>
		/// 2020.06.22 by twkang [ADD] 컨트롤들에 값을 채운다.
		/// </summary>
		private	void SetControlValues(int nRowIndex)
		{
			m_labelIndex.Text			= m_dgViewAlarm[COLUMN_INDEX_OF_INDEX, nRowIndex].Value.ToString();

			m_labelAlarmCode.Text		= m_dgViewAlarm[COLUMN_INDEX_OF_ALARMCODE, nRowIndex].Value.ToString();
			m_labelAlarmGrade.Text		= m_dgViewAlarm[COLUMN_INDEX_OF_GRADE, nRowIndex].Value.ToString();
			m_labelMessageCode.Text		= m_dgViewAlarm[COLUMN_INDEX_OF_MESSAGE_CODE, nRowIndex].Value.ToString();
			m_labelSolutionCode.Text	= m_dgViewAlarm[COLUMN_INDEX_OF_SOLUTION, nRowIndex].Value.ToString();		
			
			int nBuzzerIndex	= -1;
			string strIOName	= "--";

			m_InstanceOfAlarm.GetParameter(m_nIndexOfSelectedItem, FrameOfSystem3.Config.ConfigAlarm.EN_PARAM_ALARM.OUTPUT_BUZZER, ref nBuzzerIndex);
			m_InstanceOfDIO.GetParameter(false, nBuzzerIndex, FrameOfSystem3.Config.ConfigDigitalIO.EN_PARAM_DIGITALIO.NAME, ref strIOName);

			m_labelBuzzerIO.SubText		= string.Format("[ {0} ]", nBuzzerIndex);
			m_labelBuzzerIO.Text		= strIOName;
		}
		/// <summary>
		/// 2020.06.22 by twkang [ADD] 컨트롤들의 활성화 여부를 설정한다.
		/// </summary>
		private void SetActiveControls(bool bEnable)
		{
			m_labelAlarmCode.Enabled	= bEnable;
			m_labelAlarmGrade.Enabled	= bEnable;
			m_labelMessageCode.Enabled	= bEnable;
			m_labelSolutionCode.Enabled = bEnable;
			m_labelBuzzerIO.Enabled		= bEnable;

			m_btnRemove.Enabled			= bEnable;
		}
		/// <summary>
		/// 2020.06.22 by twkang [ADD] 모든 컨트롤들을 초기화한다.
		/// </summary>
		private void ResetControls()
		{
			m_nIndexOfSelectedItem		= -1;
			m_nIndexOfSelectedRows		= -1;
			
			SetActiveControls(false);

			m_labelIndex.Text			= "";

			m_labelAlarmCode.Text		= "--";
			m_labelAlarmGrade.Text		= "--";
			m_labelMessageCode.Text		= "--";
			m_labelSolutionCode.Text	= "--";			

			m_labelBuzzerIO.Text		= "--";
			m_labelBuzzerIO.SubText		= "[ -1 ]";

			EnterDefalutColumnText();
		}

        /// <summary>
        /// 2020.12.03 by yjlee [ADD] Sort the content by the column.
        /// </summary>
        private void SortContent(ref int nColumnIndex)
        {
            ListSortDirection sortDir       = m_dgViewAlarm.SortOrder == SortOrder.Ascending ? ListSortDirection.Descending : ListSortDirection.Ascending;

            m_dgViewAlarm.Sort(m_dgViewAlarm.Columns[nColumnIndex], sortDir);

			UpdateColumnSortUI(nColumnIndex, sortDir == ListSortDirection.Ascending);

            m_dgViewAlarm.SortCompare += new DataGridViewSortCompareEventHandler(SortCompareByAlarmCode);
        }
        /// <summary>
        /// 2020.12.03 by yjlee [ADD] Sort the content by the alarm code.
        /// </summary>
        private void SortCompareByAlarmCode(object sender, DataGridViewSortCompareEventArgs e)
        {
			if(null == e.CellValue1 || null == e.CellValue2)
				return;

            string strValueFirst    = e.CellValue1.ToString();
            string strValueSecond   = e.CellValue2.ToString();

            int nValueFirst         = 0;
            int nValueSecond        = 0;

            if(int.TryParse(strValueFirst, out nValueFirst)
                && int.TryParse(strValueSecond, out nValueSecond))
            {
                e.SortResult        = nValueFirst.CompareTo(nValueSecond);
            }
            else
            {
                e.SortResult        = string.Compare(strValueFirst, strValueSecond);
            }

            e.Handled       = true;
        }
		/// <summary>
		/// 2021.05.12 by twkang [ADD] GUI DataGridView Column 모양 업데이트
		/// </summary>
		private void UpdateColumnSortUI(int nColumnIndex, bool bAscending)
		{
			string strSorting	= bAscending ? ASCENDING_ICON : DESCENDING_ICON;

			EnterDefalutColumnText();

			m_dgViewAlarm.Columns[nColumnIndex].HeaderText += strSorting;
		}
		/// <summary>
		/// 2021.05.12 by twkang [ADD} 
		/// </summary>
		private void EnterDefalutColumnText()
		{
			m_dgViewAlarm.Columns[COLUMN_INDEX_OF_INDEX].HeaderText			= COLUMN_TEXT_INDEX;
			m_dgViewAlarm.Columns[COLUMN_INDEX_OF_ALARMCODE].HeaderText		= COLUMN_TEXT_ALARM_CODE;
			m_dgViewAlarm.Columns[COLUMN_INDEX_OF_GRADE].HeaderText			= COLUMN_TEXT_GRADE;
			m_dgViewAlarm.Columns[COLUMN_INDEX_OF_MESSAGE_CODE].HeaderText	= COLUMN_TEXT_MESSAGE;
			m_dgViewAlarm.Columns[COLUMN_INDEX_OF_SOLUTION].HeaderText		= COLUMN_TEXT_SOLUTION;
			m_dgViewAlarm.Columns[COLUMN_INDEX_OF_MESSAGE].HeaderText		= COLUMN_TEXT_MESSAGE;
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

		private string ReplaceTokenOfAlarmMessage(int alarmCode, string originMessage)
		{
			if (alarmCode < 1000) return originMessage;

			int taskNo = (int)(alarmCode / 10000) - 1;
			if(taskNo < 0) return originMessage;

			if (false == Enum.IsDefined(typeof(Define.DefineEnumProject.Task.EN_TASK_LIST), taskNo))
				return originMessage;


			string token = "$T";
			List<int> indexes = new List<int>();
			int index = 0;
			while ((index = originMessage.IndexOf(token, index)) != -1)
			{
				indexes.Add(index);
				index += token.Length;
			}

			if (indexes.Count <= 0) return originMessage;


			List<string> replaces = new List<string>();
			replaces.Add(((Define.DefineEnumProject.Task.EN_TASK_LIST)taskNo).ToString());		// Task Name

			Func<int, string> GetDeviceName = (code) =>
			{
				// 2024.08.28. jhlim [ADD] 1이상 10000미만의 코드 범위는 디바이스 알람 범위가 아니다.
				int codeAndTypeOnly = code % 10000;
				bool needCheckingDevice = (codeAndTypeOnly > 0 && codeAndTypeOnly < 10000) ? false : true;
				if (false == needCheckingDevice)
					return SubContent;
				// 2024.08.28. jhlim [END]

				int alarmNo = code % 100;
				TaskDevice_.TYPE_DEVICE deviceType;
				if (alarmNo >= 10 && alarmNo < 30) deviceType = TaskDevice_.TYPE_DEVICE.MOTION;
				else if (alarmNo >= 30 && alarmNo < 40) deviceType = TaskDevice_.TYPE_DEVICE.CYLINDER;
				else if (alarmNo >= 40 && alarmNo < 45) deviceType = TaskDevice_.TYPE_DEVICE.DIGITAL_INPUT;
				else if (alarmNo >= 45 && alarmNo < 50) deviceType = TaskDevice_.TYPE_DEVICE.DIGITAL_OUTPUT;
				else if (alarmNo >= 50 && alarmNo < 55) deviceType = TaskDevice_.TYPE_DEVICE.ANALOG_INPUT;
				else if (alarmNo >= 55 && alarmNo < 60) deviceType = TaskDevice_.TYPE_DEVICE.ANALOG_OUTPUT;
				else return SubContent;

				string deviceName = string.Empty;
				int deviceNo = (alarmCode / 100) % 100;
				TaskDevice_.TaskDevice.GetInstance().GetDeviceTargetName(replaces[0], deviceType, deviceNo, ref deviceName);
				return deviceName;
			};

			replaces.Add(GetDeviceName(alarmCode));

			string result = originMessage;
			foreach(string replace in replaces)
			{
				index = originMessage.IndexOf(token);
				if (index == -1) break;

				result = originMessage.Substring(0, index);
				result += replace;
				result += originMessage.Substring(index+token.Length);
				originMessage = result;
			}

			return result;
		}
		#endregion

		#region UI이벤트
		/// <summary>
		/// 2020.06.22 by twkang [ADD] 버튼 클릭 이벤트
		/// </summary>
		private void Click_Buttons(object sender, EventArgs e)
		{
			Control ctrl	= sender as Control;

			switch(ctrl.TabIndex)
			{
				case 0: // ADD
					int nIndex	= m_InstanceOfAlarm.AddAlarmItem();
					if(SELECT_NONE != nIndex)
					{
						AddItemOnGrid(nIndex);
					}
					break;
				case 1: // REMOVE
					if(ConfirmButtonClick("REMOVE"))
					{
						m_InstanceOfAlarm.RemoveItem(m_nIndexOfSelectedItem);
						m_dgViewAlarm.Rows.RemoveAt(m_nIndexOfSelectedRows);
					}
					
					if(null != m_dgViewAlarm.CurrentCell)
					{
						m_dgViewAlarm.Rows[m_dgViewAlarm.CurrentRow.Index].Selected	= false;
					}
					break;
			}
			ResetControls();
		}
		/// <summary>
		/// 2020.06.22 by twkang [ADD] 그리드 뷰 클릭 이벤트
        /// 2020.12.03 by yjlee [ADD] Sorting
		/// </summary>
		private void Click_Dgview(object sender, DataGridViewCellEventArgs e)
		{
			int nRowIndex       = e.RowIndex;
            int nColumnIndex    = e.ColumnIndex;

            if(nRowIndex < 0)
            {
                SortContent(ref nColumnIndex);
            }
			else if (nRowIndex < m_dgViewAlarm.RowCount)
            {
                if (nRowIndex == m_nIndexOfSelectedRows) { return; }

                m_nIndexOfSelectedRows = nRowIndex;

                m_nIndexOfSelectedItem = int.Parse(m_dgViewAlarm[COLUMN_INDEX_OF_INDEX, nRowIndex].Value.ToString());

                SetControlValues(nRowIndex);
                SetActiveControls(true);
            }
		}
		/// <summary>
		/// 2020.06.22 by twkang [ADD] 설정 라벨 클릭 이벤트
		/// </summary>
		private void Click_Configuration(object sender, EventArgs e)
		{
			Control ctrl		= sender as Control;

			string strValue		= string.Empty;
			int nResult			= -1;

			switch(ctrl.TabIndex)
			{
				case 0: // ALARM CODE
					if(m_InstanceOfCalculator.CreateForm(m_labelAlarmCode.Text, MIN_OF_PARAM, MAX_OF_PARAM))
					{
						m_InstanceOfCalculator.GetResult(ref nResult);
						strValue		= nResult.ToString();
						if(m_InstanceOfAlarm.SetParameter(m_nIndexOfSelectedItem, FrameOfSystem3.Config.ConfigAlarm.EN_PARAM_ALARM.ALARMCODE, strValue))
						{
							m_labelAlarmCode.Text	= strValue;
							m_dgViewAlarm[COLUMN_INDEX_OF_ALARMCODE, m_nIndexOfSelectedRows].Value = strValue;
						}
					}
					break;
				case 1: // ALARM GRADE
					if(m_InstanceOfSelectionList.CreateForm(m_lblAlarmGrade.Text, Define.DefineEnumProject.SelectionList.EN_SELECTIONLIST.ALARM_STATE, m_labelAlarmGrade.Text))
					{
						m_InstanceOfSelectionList.GetResult(ref strValue);
						if(m_InstanceOfAlarm.SetParameter(m_nIndexOfSelectedItem, FrameOfSystem3.Config.ConfigAlarm.EN_PARAM_ALARM.GRADE, strValue))
						{
							m_labelAlarmGrade.Text		= strValue;
							m_dgViewAlarm[COLUMN_INDEX_OF_GRADE, m_nIndexOfSelectedRows].Value		= strValue;
						}
					}
					break;
				case 2:	// SOLUTION CODE
					if(m_InstanceOfCalculator.CreateForm(m_labelSolutionCode.Text, MIN_OF_PARAM, MAX_OF_PARAM))
					{
						m_InstanceOfCalculator.GetResult(ref nResult);
						strValue		= nResult.ToString();
						if(m_InstanceOfAlarm.SetParameter(m_nIndexOfSelectedItem, FrameOfSystem3.Config.ConfigAlarm.EN_PARAM_ALARM.SOLUTIONCODE, strValue))
						{
							m_labelSolutionCode.Text	= strValue;
							m_dgViewAlarm[COLUMN_INDEX_OF_SOLUTION, m_nIndexOfSelectedRows].Value = strValue;
						}
					}
					break;
				case 3:	// MESSAGE CODE
					if(m_InstanceOfCalculator.CreateForm(m_labelMessageCode.Text, MIN_OF_PARAM, MAX_OF_PARAM))
					{
						m_InstanceOfCalculator.GetResult(ref nResult);
						if(m_InstanceOfAlarm.SetParameter(m_nIndexOfSelectedItem, FrameOfSystem3.Config.ConfigAlarm.EN_PARAM_ALARM.MESSAGECODE, nResult))
						{
							m_labelMessageCode.Text	= nResult.ToString();
							m_dgViewAlarm[COLUMN_INDEX_OF_MESSAGE_CODE, m_nIndexOfSelectedRows].Value = nResult.ToString();

							// 2025.07.08. jhlim [MOD] 현재 언어로 번역해보고 실패 -> 영어로 재 번역 -> 공백
							var currentLanguage = m_InstanceOfLanguage.GetLangauge();
                            if (false == m_InstanceOfLanguage.GetParameter(FrameOfSystem3.Config.ConfigLanguage.EN_TYPE_FORMAT.SENTENCE, nResult, currentLanguage, ref strValue))
                            {
                                var fallbackLanguage = FrameOfSystem3.Config.ConfigLanguage.EN_PARAM_LANGUAGE.ENGLISH;

                                if (currentLanguage.Equals(fallbackLanguage) ||
                                    false == m_InstanceOfLanguage.GetParameter(FrameOfSystem3.Config.ConfigLanguage.EN_TYPE_FORMAT.SENTENCE, nResult, fallbackLanguage, ref strValue))
                                {
                                    strValue = string.Empty;
                                }
                            }
							//m_InstanceOfLanguage.GetParameter(FrameOfSystem3.Config.ConfigLanguage.EN_TYPE_FORMAT.SENTENCE, nResult, FrameOfSystem3.Config.ConfigLanguage.EN_PARAM_LANGUAGE.ENGLISH, ref strValue);						
							// 2025.07.08. jhlim [END]

							// 2023.05.25 by junho [MOD] token 변환하여 표시하도록 변경
							//m_dgViewAlarm[COLUMN_INDEX_OF_MESSAGE, m_nIndexOfSelectedRows].Value = strValue;
							int alarmCode = -1;
							if (false == m_InstanceOfAlarm.GetParameter(m_nIndexOfSelectedItem, FrameOfSystem3.Config.ConfigAlarm.EN_PARAM_ALARM.ALARMCODE, ref alarmCode))
								alarmCode = -1;

							m_dgViewAlarm[COLUMN_INDEX_OF_MESSAGE, m_nIndexOfSelectedRows].Value = ReplaceTokenOfAlarmMessage(alarmCode, strValue);
						}
					}					
					break;
				case 4: // Output Buzzer
					string[] arItem			= null;
					string strItemName		= string.Empty;

					m_InstanceOfDIO.GetListOfName(false, ref arItem);

					int nPreValue			= RemoveBrakets(m_labelBuzzerIO.SubText);

					m_InstanceOfDIO.GetParameter(false, nPreValue, FrameOfSystem3.Config.ConfigDigitalIO.EN_PARAM_DIGITALIO.NAME, ref strItemName);

					if(m_InstanceOfSelectionList.CreateForm("BUZZER INDEX", arItem, strItemName))
					{
						m_InstanceOfSelectionList.GetResult(ref nResult);
						m_InstanceOfSelectionList.GetResult(ref strItemName);

						if(m_InstanceOfAlarm.SetParameter(m_nIndexOfSelectedItem, FrameOfSystem3.Config.ConfigAlarm.EN_PARAM_ALARM.OUTPUT_BUZZER, nResult))
						{
							m_labelBuzzerIO.SubText		= string.Format("[ {0} ]", nResult);
							m_labelBuzzerIO.Text		= strItemName;
						}
					}
					break;
			}
		}
		private void Click_Export(object sender, EventArgs e)
		{
			FrameOfSystem3.Functional.FunctionsETC.CsvExportToGridView(m_dgViewAlarm);
		}
		private void Click_BuzzerDiffuse(object sender, EventArgs e)
		{
			int buzzerIoNo = -1;
			if (false == m_InstanceOfAlarm.GetParameter(m_nIndexOfSelectedItem, FrameOfSystem3.Config.ConfigAlarm.EN_PARAM_ALARM.OUTPUT_BUZZER, ref buzzerIoNo)
				|| buzzerIoNo < 0)
				return;

			for (int nIndex = 0; nIndex < m_nArrIndexOfItem.Length; ++nIndex)
			{
				m_InstanceOfAlarm.SetParameter(m_nArrIndexOfItem[nIndex], FrameOfSystem3.Config.ConfigAlarm.EN_PARAM_ALARM.OUTPUT_BUZZER, buzzerIoNo);
			}
		}
		#endregion
	}
}