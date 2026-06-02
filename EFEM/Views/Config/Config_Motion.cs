using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using System.Threading;

namespace FrameOfSystem3.Views.Config
{
	extern alias MotionInstance;

    public partial class Config_Motion : UserControlForMainView.CustomView
	{
        public Config_Motion()
        {
            InitializeComponent();
			
            InitializePanel();

			#region Instance
			m_InstanceOfMotion					= FrameOfSystem3.Config.ConfigMotion.GetInstance();
			m_InstanceOfCalculator				= Functional.Form_Calculator.GetInstance();
			m_InstanceOfKeyboard				= Functional.Form_Keyboard.GetInstance();
			m_InstancOfSelectionList			= Functional.Form_SelectionList.GetInstance();
			m_InstanceOfProgressBar				= Functional.Form_ProgressBar.GetInstance();
			m_InstanceOfMessageBox				= Functional.Form_MessageBox.GetInstance();
			#endregion

            m_GroupBox.Visible = false;
			m_ProgressBar.Visible = false;

            // 2023.09.07. by shkim. [ADD] Frame Size 가변
            bool isActivated = true;

            m_btnAdd.Visible = !isActivated;
            m_btnRemove.Visible = !isActivated;

            m_btnExtend.Text = isActivated ? LEFT_ARROW : RIGHT_ARROW;

            _configRowHeightBackup = _tableLayoutPanel_Main.RowStyles[2].Height;
            // 2023.09.07. by shkim. [END]
        }

        float _configRowHeightBackup = 0;

		#region 열거형
		
		#endregion

		#region 상수
		private const int HEIGHT_OF_ROWS					= 30;
		private const int EXTENDED_HEIGHT_OF_ROWS           = 540;

		private const int SELECT_NONE						= -1;

		private readonly string MIN_OF_POWER				= "1";
		private readonly string MAX_OF_POWER				= "100";

		#region COLUMN_INDEX
		private const int COLUMN_INDEX_OF_INDEX				= 0;
		private const int COLUMN_INDEX_OF_NAME				= 1;
		private const int COLUMN_INDEX_OF_TARGET			= 2;
		private const int COLUMN_INDEX_OF_ENABLE			= 3;
		private const int COLUMN_INDEX_OF_COMMAND			= 4;
		private const int COLUMN_INDEX_OF_ACTUAL			= 5;
		private const int COLUMN_INDEX_OF_ALARM				= 6;
		private const int COLUMN_INDEX_OF_MOTOR_ON			= 7;
		private const int COLUMN_INDEX_OF_HOME_END			= 8;
		private const int COLUMN_INDEX_OF_DONE				= 9;
		#endregion

		#region GUI
		private readonly string LEFT_ARROW					= "◀";
		private readonly string RIGHT_ARROW					= "▶";
		private readonly string DOWN_ARROW					= "▼";
		private readonly string UP_ARROW					= "▲";

		private const int EXTEND_WIDTH_X					= 272;

		private readonly Point c_ptBasePosition				= new Point(3, 200);
		private readonly Point c_ptExtendedPosition			= new Point(3, 740);

		private readonly Color c_clrTrue					= Color.DodgerBlue;
		private readonly Color c_clrFalse					= Color.White;
		#endregion

		
		#endregion

		#region 변수
		private int m_nIndexOfSelectedRows				= SELECT_NONE;
		private int m_nIndexOfSelectedItem				= SELECT_NONE;

		private Sys3Controls.Sys3button			m_tabClicked        = null;
		private UserControl                     m_selectedPanel     = null;

		#region Instance
		FrameOfSystem3.Config.ConfigMotion m_InstanceOfMotion		= null;
		Functional.Form_Calculator m_InstanceOfCalculator			= null;
		Functional.Form_SelectionList m_InstancOfSelectionList		= null;
		Functional.Form_Keyboard m_InstanceOfKeyboard				= null;
		Functional.Form_ProgressBar m_InstanceOfProgressBar			= null;
		Functional.Form_MessageBox m_InstanceOfMessageBox			= null;
		#endregion

		#region 패널
		private MotionPanel.MotorGantry     m_viewMotorGantry       = new MotionPanel.MotorGantry();
		private MotionPanel.MotorGeneral    m_viewMotorGeneral      = new MotionPanel.MotorGeneral();
		private MotionPanel.MotorLimit		m_viewMotorLimit		= new MotionPanel.MotorLimit();
		private MotionPanel.MotorHome       m_viewMotorHome         = new MotionPanel.MotorHome();
		private MotionPanel.MotorSpeed      m_viewMotorSpeed        = new MotionPanel.MotorSpeed();
		private MotionPanel.MotorState      m_viewMotorState        = new MotionPanel.MotorState();
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
			UpdateGrid();
			if(SELECT_NONE != m_nIndexOfSelectedRows)
			{
				m_dgViewList.Rows[m_nIndexOfSelectedRows].Selected		= true;
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
			int nRowCount	= m_dgViewList.Rows.Count;

			for (int nIndex = 0; nIndex < nRowCount; nIndex++)
			{
				int nIndexOfItem = int.Parse(m_dgViewList[COLUMN_INDEX_OF_INDEX, nIndex].Value.ToString());
				int nState = 0;
				double dbResult = 0;

				dbResult = m_InstanceOfMotion.GetCommandPosition(nIndexOfItem);
				m_dgViewList[COLUMN_INDEX_OF_COMMAND, nIndex].Value = String.Format("{0:0.0000}", dbResult);

				dbResult = m_InstanceOfMotion.GetActualPosition(nIndexOfItem);
				m_dgViewList[COLUMN_INDEX_OF_ACTUAL, nIndex].Value = String.Format("{0:0.0000}", dbResult);

				m_InstanceOfMotion.GetMotorState(nIndexOfItem, ref nState);

				m_dgViewList.Rows[nIndex].Cells[COLUMN_INDEX_OF_ALARM].Style.BackColor
					= m_InstanceOfMotion.GetState(ref nState, MotionInstance::Motion_.MOTOR_STATE.ALARM) ? c_clrTrue : c_clrFalse;
				m_dgViewList.Rows[nIndex].Cells[COLUMN_INDEX_OF_ALARM].Style.SelectionBackColor
					= m_InstanceOfMotion.GetState(ref nState, MotionInstance::Motion_.MOTOR_STATE.ALARM) ? c_clrTrue : c_clrFalse;

				m_dgViewList.Rows[nIndex].Cells[COLUMN_INDEX_OF_MOTOR_ON].Style.BackColor
					= m_InstanceOfMotion.GetState(ref nState, MotionInstance::Motion_.MOTOR_STATE.MOTOR_ON) ? c_clrTrue : c_clrFalse;
				m_dgViewList.Rows[nIndex].Cells[COLUMN_INDEX_OF_MOTOR_ON].Style.SelectionBackColor
					= m_InstanceOfMotion.GetState(ref nState, MotionInstance::Motion_.MOTOR_STATE.MOTOR_ON) ? c_clrTrue : c_clrFalse;

				m_dgViewList.Rows[nIndex].Cells[COLUMN_INDEX_OF_HOME_END].Style.BackColor
					= m_InstanceOfMotion.GetState(ref nState, MotionInstance::Motion_.MOTOR_STATE.HOME_DONE) ? c_clrTrue : c_clrFalse;
				m_dgViewList.Rows[nIndex].Cells[COLUMN_INDEX_OF_HOME_END].Style.SelectionBackColor
					= m_InstanceOfMotion.GetState(ref nState, MotionInstance::Motion_.MOTOR_STATE.HOME_DONE) ? c_clrTrue : c_clrFalse;

				m_dgViewList.Rows[nIndex].Cells[COLUMN_INDEX_OF_DONE].Style.BackColor
					= m_InstanceOfMotion.GetState(ref nState, MotionInstance::Motion_.MOTOR_STATE.MOTION_DONE) ? c_clrTrue : c_clrFalse;
				m_dgViewList.Rows[nIndex].Cells[COLUMN_INDEX_OF_DONE].Style.SelectionBackColor
					= m_InstanceOfMotion.GetState(ref nState, MotionInstance::Motion_.MOTOR_STATE.MOTION_DONE) ? c_clrTrue : c_clrFalse;
			}

			switch(m_tabClicked.TabIndex)
			{
				case 0: // STATE
					m_viewMotorState.UpdateState();
					break;
				case 1: // GENERAL
				case 2: // LIMIT
				case 3:	// SPEED
					break;
				case 4:	// HOME
					m_viewMotorHome.UpdateState();
					break;
				case 5:	// GANTRY
					break;
			}
		}
		#endregion

        #region 내부인터페이스

		#region 초기화
		/// <summary>
		/// 2020.06.23 by twkang [ADD] 패널 초기화를 진행한다.
		/// </summary>
		private void InitializePanel()
		{
			m_panel.Controls.Add(m_viewMotorGantry);
			m_panel.Controls.Add(m_viewMotorGeneral);
			m_panel.Controls.Add(m_viewMotorHome);
			m_panel.Controls.Add(m_viewMotorSpeed);
			m_panel.Controls.Add(m_viewMotorState);
			m_panel.Controls.Add(m_viewMotorLimit);

            // 2023.09.07. by shkim. [ADD] Frame Size 가변
            foreach(Control userControl in m_panel.Controls)
            {
                userControl.Dock = DockStyle.Fill;   
            }
            // 2023.09.07. by shkim. [END]

			m_tabClicked		= m_tabState;
			m_selectedPanel		= m_viewMotorState;

			m_viewMotorLimit.Hide();
			m_viewMotorGantry.Hide();
			m_viewMotorGeneral.Hide();
			m_viewMotorHome.Hide();
			m_viewMotorSpeed.Hide();
		}
		/// <summary>
		/// 2020.07.07 by twkang [ADD] 라벨 상태와, 값을 초기화한다.
		/// </summary>
		private void ResetSelectedItem()
		{
			m_nIndexOfSelectedItem		= SELECT_NONE;
			m_nIndexOfSelectedRows		= SELECT_NONE;

			m_labelIndex.Text			= "";
			m_labelName.Text			= "--";
			m_labelTarget.Text			= "--";
			m_labelPower.Text			= "--";

			m_viewMotorState.SetValuesToControls(m_nIndexOfSelectedItem);
			m_viewMotorGeneral.SetValuesToControls(m_nIndexOfSelectedItem);
			m_viewMotorLimit.SetValuesToControls(m_nIndexOfSelectedItem);
			m_viewMotorSpeed.SetValuesToControls(m_nIndexOfSelectedItem);
			m_viewMotorHome.SetValuesToControls(m_nIndexOfSelectedItem);
			m_viewMotorGantry.SetValuesToControls(m_nIndexOfSelectedItem);

			ActiveControls(false);

			m_btnEnable.Enabled			= false;;
		}
		#endregion

        /// <summary>
        /// 2020.06.23 by twkang [ADD] 패널의 화면을 바꾼다.
        /// </summary>
        private void SetPanelState(int nIndex)
        {
            if(m_tabClicked.TabIndex == nIndex) { return;}

			m_tabClicked.GradientFirstColor		= Color.White;
			m_tabClicked.GradientSecondColor	= Color.White;
			m_tabClicked.MainFontColor			= Color.DarkBlue;
            m_tabClicked.ButtonClicked			= false;
            m_selectedPanel.Hide();

            switch(nIndex)
            {
                case 0: //STATE
                    m_tabClicked    = m_tabState;
                    m_selectedPanel = m_viewMotorState;
                    break;
                case 1: //GENERAL
                    m_tabClicked    = m_tabGeneral;
                    m_selectedPanel = m_viewMotorGeneral;
                    break;
				case 2:
					m_tabClicked	= m_tabLimit;
					m_selectedPanel	= m_viewMotorLimit;
					break;
                case 3: //SPEED
                    m_tabClicked    = m_tabSpeed;
                    m_selectedPanel = m_viewMotorSpeed;
                    break;
                case 4: //HOME
                    m_tabClicked    = m_tabHome;
                    m_selectedPanel = m_viewMotorHome;
                    break;
                case 5: //GANTRY
                    m_tabClicked    = m_tabGantry;
                    m_selectedPanel = m_viewMotorGantry;
                    break;
            }
            m_selectedPanel.Show();
            m_tabClicked.ButtonClicked  = true;
			m_tabClicked.GradientFirstColor		= Color.DarkBlue;
			m_tabClicked.GradientSecondColor	= Color.DarkBlue;
			m_tabClicked.MainFontColor			= Color.White;
        }
		/// <summary>
		/// 2020.06.23 by twkang [ADD] 전체 아이템으로 GridView 를 갱신한다.
		/// </summary>
		private void UpdateGrid()
		{
			m_dgViewList.Rows.Clear();

			int[] nArrOfItem			= null;

			if (false == m_InstanceOfMotion.GetListOfItem(ref nArrOfItem))
			{
				return;
			}
			
			for(int nIndex = 0, nEnd = nArrOfItem.Length; nIndex < nEnd; ++nIndex)
			{
				AddMotionItem(nArrOfItem[nIndex]);
			}
		}
		/// <summary>
		/// 2020.07.06 by twkang [ADD] GridView 에 아이템을 추가한다.
		/// </summary>
		private void AddMotionItem(int nIndexOfItem)
		{
			string strValue		= string.Empty;
			bool bResult		= false;

			int nValue			= -1;

			int nIndexRow		= m_dgViewList.RowCount;

			// 2024.03.08 by junho [ADD] filtering 기능 추가
			string itemName = string.Empty;
			if (false == m_InstanceOfMotion.GetMotionParameter(nIndexOfItem, FrameOfSystem3.Config.ConfigMotion.EN_PARAM_MOTION.NAME, ref itemName))
				itemName = string.Empty;

			// 2024.06.25 by junho [MOD] 대소문자 구분하지 않도록 변경
			//if (_filterKeyword != string.Empty && false == itemName.Contains(_filterKeyword))
			if (_filterKeyword != string.Empty && itemName.IndexOf(_filterKeyword, StringComparison.OrdinalIgnoreCase) < 0)
				return;

			m_dgViewList.Rows.Add();

			m_dgViewList.Rows[nIndexRow].Height						= HEIGHT_OF_ROWS;
			m_dgViewList[COLUMN_INDEX_OF_INDEX, nIndexRow].Value	= nIndexOfItem.ToString();

			m_dgViewList.Rows[nIndexRow].Cells[COLUMN_INDEX_OF_INDEX].Style.BackColor			= Color.Silver;
			m_dgViewList.Rows[nIndexRow].Cells[COLUMN_INDEX_OF_INDEX].Style.SelectionBackColor	= Color.Silver;
			m_dgViewList.Rows[nIndexRow].Cells[COLUMN_INDEX_OF_INDEX].Style.SelectionForeColor	= Color.Black;

			// 2024.03.08 by junho [MOD] filtering 기능 추가
			//if (false == m_InstanceOfMotion.GetMotionParameter(nIndexOfItem, FrameOfSystem3.Config.ConfigMotion.EN_PARAM_MOTION.NAME, ref strValue))
			//{
			//	strValue = string.Empty;
			//}
			//m_dgViewList[COLUMN_INDEX_OF_NAME, nIndexRow].Value		= strValue;
			m_dgViewList[COLUMN_INDEX_OF_NAME, nIndexRow].Value = itemName;

			if(false == m_InstanceOfMotion.GetMotionParameter(nIndexOfItem, FrameOfSystem3.Config.ConfigMotion.EN_PARAM_MOTION.TARGET, ref nValue))
			{
				nValue	= -1;
			}
			m_dgViewList[COLUMN_INDEX_OF_TARGET, nIndexRow].Value	= nValue;

			m_InstanceOfMotion.GetMotionParameter(nIndexOfItem, FrameOfSystem3.Config.ConfigMotion.EN_PARAM_MOTION.ENABLE, ref bResult);
			m_dgViewList.Rows[nIndexRow].Cells[COLUMN_INDEX_OF_ENABLE].Style.BackColor			= bResult ? c_clrTrue : c_clrFalse;
			m_dgViewList.Rows[nIndexRow].Cells[COLUMN_INDEX_OF_ENABLE].Style.SelectionBackColor	= bResult ? c_clrTrue : c_clrFalse;

			m_dgViewList.Rows[nIndexRow].Selected		= false;

		}
		/// <summary>
		/// 2020.06.23 by twkang [ADD] 설정 라벨들의 값을 채운다.
		/// </summary>
		private void SetConfigurationLabels(int nRowIndex)
		{
			if (m_nIndexOfSelectedItem == SELECT_NONE) { return; }

			string strValue			= string.Empty;

			m_labelIndex.Text       = m_dgViewList[COLUMN_INDEX_OF_INDEX, nRowIndex].Value.ToString();
            m_labelName.Text        = m_dgViewList[COLUMN_INDEX_OF_NAME, nRowIndex].Value.ToString();
            m_labelTarget.Text      = m_dgViewList[COLUMN_INDEX_OF_TARGET, nRowIndex].Value.ToString();
						
			m_InstanceOfMotion.GetMotionParameter(m_nIndexOfSelectedItem, FrameOfSystem3.Config.ConfigMotion.EN_PARAM_MOTION.POWER, ref strValue);
			m_labelPower.Text		= strValue;

			m_InstanceOfMotion.GetMotionParameter(m_nIndexOfSelectedItem, FrameOfSystem3.Config.ConfigMotion.EN_PARAM_MOTION.TAG_NO, ref strValue);
			m_labelTag.Text			= strValue;
		}
		/// <summary>
		/// 2020.06.23 by twkang [ADD] 컨트롤들의 활성화 상태를 설정한다.
		/// </summary>
		private void ActiveControls(bool bEnable)
		{
			m_btnEnable.Enabled			= !bEnable;

			m_btnRemove.Enabled			= bEnable;
			m_btnDisable.Enabled		= bEnable;

			m_labelName.Enabled			= bEnable;
			m_labelTarget.Enabled		= bEnable;
			m_labelPower.Enabled		= bEnable;
			m_labelTag.Enabled			= bEnable;

			m_btnStop.Enabled			= bEnable;
			m_btnHome.Enabled			= bEnable;
			m_btnMotorOn.Enabled		= bEnable;
			m_btnMotorOff.Enabled		= bEnable;
			m_btnReset.Enabled			= bEnable;

			#region Panel
			m_viewMotorState.SetActiveControls(bEnable);
			m_viewMotorGeneral.SetActiveControls(bEnable);
			m_viewMotorLimit.SetActiveControls(bEnable);
			m_viewMotorSpeed.SetActiveControls(bEnable);
			m_viewMotorHome.SetActiveControls(bEnable);
			m_viewMotorGantry.SetActiveControls(bEnable);
			#endregion
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
        /// 2020.06.23 by twkang [ADD] 패널 버튼 클릭 이벤트
        /// </summary>
        private void Clicked_Panel(object sender, EventArgs e)
        {
            Control ctr = sender as Control;

            SetPanelState(ctr.TabIndex);
        }
		/// <summary>
		/// 2020.06.23 by twkang [ADD] 그리드 뷰 클릭 이벤트
		/// </summary>
		private void Click_Dgview(object sender, DataGridViewCellEventArgs e)
		{
			int nRowIndex   = e.RowIndex;

			if (nRowIndex < 0 || nRowIndex >= m_dgViewList.RowCount) { return; }

			if (nRowIndex == m_nIndexOfSelectedRows) return;

			m_nIndexOfSelectedItem = int.Parse(m_dgViewList[COLUMN_INDEX_OF_INDEX, nRowIndex].Value.ToString());

			m_nIndexOfSelectedRows = nRowIndex;

			SetConfigurationLabels(nRowIndex);

			bool bEnable		= false;
			m_InstanceOfMotion.GetMotionParameter(m_nIndexOfSelectedItem, FrameOfSystem3.Config.ConfigMotion.EN_PARAM_MOTION.ENABLE, ref bEnable);

			#region Panels
			m_viewMotorState.SetValuesToControls(m_nIndexOfSelectedItem);
			m_viewMotorGeneral.SetValuesToControls(m_nIndexOfSelectedItem);
			m_viewMotorLimit.SetValuesToControls(m_nIndexOfSelectedItem);
			m_viewMotorSpeed.SetValuesToControls(m_nIndexOfSelectedItem);
			m_viewMotorHome.SetValuesToControls(m_nIndexOfSelectedItem);
			m_viewMotorGantry.SetValuesToControls(m_nIndexOfSelectedItem);
			#endregion
			
			ActiveControls(bEnable);
		}
		/// <summary>
		/// 2020.06.23 by twkang [ADD] 모션리스트 스크롤 클릭 이벤트
		/// </summary>
		private void Click_LabelScroll(object sender, EventArgs e)
		{
            if (m_labelScroll.Text.Equals(DOWN_ARROW))
            {
                _tableLayoutPanel_Main.RowStyles[2].Height = 0;
                m_labelScroll.Text = UP_ARROW;
            }
            else if (m_labelScroll.Text.Equals(UP_ARROW))
            {
                _tableLayoutPanel_Main.RowStyles[2].Height = _configRowHeightBackup;
                m_labelScroll.Text = DOWN_ARROW;
            }

			// 2024.03.14. by shkim. [ADD] 선택되있는 셀이 제일 위로 오도록 실수 예방 (리스트 확장 후 축소하면 현재 선택된 축이 화면에서 벗어나 알 수 없는 경우가 있었다.)
			if (m_dgViewList.SelectedRows.Count > 0)
            {
                // 선택된 행의 인덱스를 얻습니다.
                int index = m_dgViewList.SelectedRows[0].Index;
                // 해당 행이 DataGridView의 가장 위에 오도록 스크롤합니다.
                m_dgViewList.FirstDisplayedScrollingRowIndex = index;
            }
            // 2024.03.14. by shkim. [END]
		}
		/// <summary>
		/// 2020.06.23 by twkang [ADD] 설정 라벨 클릭이벤트
		/// </summary>
		private void Click_ConfigurationLabels(object sender, EventArgs e)
		{
			Control ctrl		= sender as Control;

			string strValue		= string.Empty;
			int nResult			= -1;

			switch(ctrl.TabIndex)
			{
				case 0: // NAME
					if(m_InstanceOfKeyboard.CreateForm(m_labelName.Text))
					{
						m_InstanceOfKeyboard.GetResult(ref strValue);
						if(m_InstanceOfMotion.SetMotionParameter(m_nIndexOfSelectedItem, FrameOfSystem3.Config.ConfigMotion.EN_PARAM_MOTION.NAME, strValue))
						{
							m_dgViewList[COLUMN_INDEX_OF_NAME, m_nIndexOfSelectedRows].Value	= strValue;
							m_labelName.Text		= strValue;
						}
					}
					break;
				case 1: // TARGET
					if(m_InstanceOfCalculator.CreateForm(m_labelTarget.Text, "0", "999999")) // 임시
					{
						m_InstanceOfCalculator.GetResult(ref nResult);
						if(m_InstanceOfMotion.SetMotionParameter(m_nIndexOfSelectedItem, FrameOfSystem3.Config.ConfigMotion.EN_PARAM_MOTION.TARGET, nResult))
						{
							m_dgViewList[COLUMN_INDEX_OF_TARGET, m_nIndexOfSelectedRows].Value		= nResult.ToString();
							m_labelTarget.Text		= nResult.ToString();
						}
					}
					break;
				case 2: // TAG NUMBER
					// 2022.11.03. by shkim. [ADD] 예외처리 추가
                    int nTagNumber = 0;
                    if (false == int.TryParse(m_labelTag.Text, out nTagNumber)) return;
					// 2022.11.03. by shkim. [END]
					
                    if (m_InstanceOfKeyboard.CreateForm(m_labelTag.Text))
					{
						m_InstanceOfKeyboard.GetResult(ref strValue);
						if(m_InstanceOfMotion.SetMotionParameter(m_nIndexOfSelectedItem, FrameOfSystem3.Config.ConfigMotion.EN_PARAM_MOTION.TAG_NO, strValue))
						{
							m_labelTag.Text			= strValue;
						}
					}
					break;
				case 3:	// POWER
					if(m_InstanceOfCalculator.CreateForm(m_labelPower.Text, MIN_OF_POWER, MAX_OF_POWER))
					{
						m_InstanceOfCalculator.GetResult(ref nResult);
						if(m_InstanceOfMotion.SetMotionParameter(m_nIndexOfSelectedItem, FrameOfSystem3.Config.ConfigMotion.EN_PARAM_MOTION.POWER, nResult))
						{
							m_labelPower.Text			= nResult.ToString();
						}
					}
					break;
			}
		}
		/// <summary>
		/// 2020.06.23 by twkang [ADD] ENABLE, DISABLE 버튼 클릭 이벤트
		/// </summary>
		private void Click_EnableOrDisable(object sender, EventArgs e)
		{
			bool bEnable		= false;

			if(m_InstanceOfMotion.GetMotionParameter(m_nIndexOfSelectedItem, FrameOfSystem3.Config.ConfigMotion.EN_PARAM_MOTION.ENABLE, ref bEnable))
			{
				if(m_InstanceOfMotion.SetMotionParameter(m_nIndexOfSelectedItem, FrameOfSystem3.Config.ConfigMotion.EN_PARAM_MOTION.ENABLE, !bEnable))
				{
					m_dgViewList.Rows[m_nIndexOfSelectedRows].Cells[COLUMN_INDEX_OF_ENABLE].Style.BackColor				= !bEnable ? c_clrTrue : c_clrFalse;
					m_dgViewList.Rows[m_nIndexOfSelectedRows].Cells[COLUMN_INDEX_OF_ENABLE].Style.SelectionBackColor	= !bEnable ? c_clrTrue : c_clrFalse;
					ActiveControls(!bEnable);
				}
			}
		}
		/// <summary>
		/// 2020.06.26 by twkang [ADD] Move 관련 버튼 클릭 이벤트
		/// </summary>
		private void Click_Move(object sender, EventArgs e)
		{
			Control ctrl	= sender as Control;

			switch(ctrl.TabIndex)
			{
				case 0: // STOP
					m_InstanceOfMotion.StopMotion(m_nIndexOfSelectedItem, false);
					break;
				case 1:	// ALL STOP
					int[] nArrOfItem			= null;
					m_InstanceOfMotion.GetListOfItem(ref nArrOfItem);
					for(int nIndex = 0, nEnd = nArrOfItem.Length; nIndex < nEnd; nIndex++)
					{
						m_InstanceOfMotion.StopMotion(nArrOfItem[nIndex], false);
					}
					break;
				case 2:	// HOME
					if(ConfirmButtonClick("HOME"))
					{
						m_InstanceOfMotion.MoveToHome(m_nIndexOfSelectedItem);
					}
					break;
			}
		}
		/// <summary>
		/// 2020.06.26 by twkang [ADD] Status 관련 버튼 클릭 이벤트
		/// 2021.03.30 by jhlee [MODIFY]
		/// </summary>
		private void Click_Status(object sender, EventArgs e)
		{
			Control ctrl	= sender as Control;
			int[] nArrOfItem			= null;

			switch(ctrl.TabIndex)
			{
				case 0: // MOTOR ON
					m_InstanceOfMotion.DoMotorOn(m_nIndexOfSelectedItem, true);
					break;
				case 1:	// MOTOR OFF
					m_InstanceOfMotion.DoMotorOn(m_nIndexOfSelectedItem, false);
					break;
				case 2: // ALL MOTOR ON
					
					/// <Summery>
					/// 2021.03.30 by jhlee [ADD] 모터 ON 에 따른 ProgressBar View 변경
					/// </Summery>
                    m_InstanceOfMotion.GetListOfItem(ref nArrOfItem);
                    /// 

                    if (null == nArrOfItem)
                    {
                        return;
                    }
                    m_ProgressBar.Tick = 0;
                    m_ProgressBar.Visible = true;
                    m_GroupBox.Visible = true;

                    double dbPercentage = 0;
                    double dbTickperPercent = (100 / nArrOfItem.Length);

                    for (int nIndex = 0, nEnd = nArrOfItem.Length; nIndex < nEnd; nIndex++)
                    {
                        m_InstanceOfMotion.DoMotorOn(nArrOfItem[nIndex], true);

                        dbPercentage += dbTickperPercent;
                        m_ProgressBar.Tick += Convert.ToUInt32(dbPercentage);
                        m_GroupBox.Refresh();
                        m_ProgressBar.Refresh();
                    }
                    m_ProgressBar.Visible = false;
                    m_GroupBox.Visible = false;
                    break;
				case 3:	// ALL MOTOR OFF
					m_InstanceOfMotion.GetListOfItem(ref nArrOfItem);

                    if (null == nArrOfItem)
                        return;

					for(int nIndex = 0, nEnd = nArrOfItem.Length; nIndex < nEnd; nIndex++)
					{
						m_InstanceOfMotion.DoMotorOn(nArrOfItem[nIndex], false);
					}
					break;
			}
		}
		/// <summary>
		/// 2020.06.26 by twkang [ADD] State 관련 버튼 클릭 이벤트
		/// </summary>
		private void Click_State(object sender, EventArgs e)
		{
			Control ctrl	= sender as Control;

			switch(ctrl.TabIndex)
			{
				case 0: // RESET
					m_InstanceOfMotion.DoReset(m_nIndexOfSelectedItem);
					break;
				case 1:	// ALL RESET
					int[] nArrOfItem			= null;
					m_InstanceOfMotion.GetListOfItem(ref nArrOfItem);
					for (int nIndex = 0, nEnd = nArrOfItem.Length; nIndex < nEnd; nIndex++)
					{
						m_InstanceOfMotion.DoReset(nArrOfItem[nIndex]);
					}
					break;
			}
		}
		/// <summary>
		/// 2020.07.06 by twkang [ADD] Add, Remove 버튼 클릭 이벤트
		/// </summary>
		private void Click_AddOrRemove(object sender, EventArgs e)
		{
			Control ctrl		= sender as Control;

			switch(ctrl.TabIndex)
			{
				case 0: // ADD
					int nIndexOfItem = m_InstanceOfMotion.AddMotionItem();
					if(nIndexOfItem != SELECT_NONE)
					{
						AddMotionItem(nIndexOfItem);
					}
					break;
				case 1:	// REMOVE
					if(ConfirmButtonClick("REMOVE") && m_InstanceOfMotion.RemoveItem(m_nIndexOfSelectedItem))
					{
						UpdateGrid();
					}
					break;
			}
			ResetSelectedItem();
		}
		/// <summary>
		/// 2020.07.20 by twkang [ADD] 방향 버튼을 눌렀을 때
		/// </summary>
		private void Click_ArrowButton(object sender, EventArgs e)
		{
            // 2023.09.07. by shkim. [MOD] Frame Size 가변

            //if(m_btnExtend.Text == LEFT_ARROW)
            //{
            //    m_groupList.Width	-= EXTEND_WIDTH_X;
            //    m_btnExtend.Text	= RIGHT_ARROW;
            //}
            //else
            //{
            //    m_groupList.Width	+= EXTEND_WIDTH_X;
            //    m_btnExtend.Text	= LEFT_ARROW;
            //}
            //m_groupList.Invalidate();

            if (m_btnExtend.Text.Equals(LEFT_ARROW))
            {
                m_btnExtend.Text = RIGHT_ARROW;
                m_btnAdd.Visible = true;
                m_btnRemove.Visible = true;
            }
            else
            {
                m_btnExtend.Text = LEFT_ARROW;
                m_btnAdd.Visible = false;
                m_btnRemove.Visible = false;
			}
            // 2023.09.07. by shkim. [END]
		}
        #endregion

		private void lbl_Filtering_Click(object sender, EventArgs e)
		{
			if (false == m_InstanceOfKeyboard.CreateForm(_filterKeyword))
				return;

			m_InstanceOfKeyboard.GetResult(ref _filterKeyword);
			UpdateGrid();
			lbl_Filtering.Text = _filterKeyword;
		}
    }
}