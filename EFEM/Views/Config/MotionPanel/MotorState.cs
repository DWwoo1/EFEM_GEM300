using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using Motion_;

namespace FrameOfSystem3.Views.Config.MotionPanel
{
    public partial class MotorState : UserControlForMainView.CustomView
    {
        public MotorState()
		{
			#region Instance
			m_InstanceOfMotion				= FrameOfSystem3.Config.ConfigMotion.GetInstance();
			m_InstanceOfCalculator			= Functional.Form_Calculator.GetInstance();
			m_InstanceOfMessageBox			= Functional.Form_MessageBox.GetInstance();
			#endregion

			

            InitializeComponent();
		}

		#region 상수
		private readonly string MIN_OF_PARAM				= "0";
		private readonly string MAX_OF_PARAM				= "999999";

		private const int SELECT_NONE						= -1;
		private const int DEFAULT_RATIO						= 100;
		private const int DEFAULT_DELAY						= 0;
		private const int POSITION_CLEAR					= 0;
		#endregion

		#region 변수
		private int m_nIndexOfSelectedItem							= -1;

		#region Instance
		Functional.Form_MessageBox m_InstanceOfMessageBox			= null;
		Functional.Form_Calculator m_InstanceOfCalculator			= null;
		FrameOfSystem3.Config.ConfigMotion m_InstanceOfMotion		= null;
		#endregion

		#endregion

		#region 내부인터페이스
		/// <summary>
		/// 2020.06.30 by twkang [ADD] JOG 정보를 초기화한다.
		/// </summary>
		private void ResetJog()
        {
            m_progressbarJog.Tick   = 50;
        }
		/// <summary>
		/// 2020.06.30 by twkang [ADD] 리핏 정보를 초기화한다.
		/// </summary>
		private void ResetRepeat()
        {
            m_labelPositionFirst.Text   = "0";
            m_labelDelayFirst.Text      = "0";
            m_labelPositionSecond.Text  = "0";
            m_labelDelaySecond.Text     = "0";

			ResetRepeatLed();
        }
		private void ResetRepeatLed()
		{
			m_ledFirstPosition.Active	= false;
			m_ledFirstDelay.Active		= false;
			m_ledSecondPosition.Active	= false;
			m_ledSecondDelay.Active		= false;
		}
		/// <summary>
		/// 2020.06.30 by twkang [ADD] 포지션 값을 초기화한다.
		/// </summary>
		private void ResetPosition()
        {
            m_labelCommand.Text			= "0";
            m_labelActual.Text			= "0";
            m_labelError.Text			= "0";
        }
		/// <summary>
		/// 
		/// </summary>
		private void ResetState()
        {
            m_ledMotorOn.Active         = false;
            m_ledAlarm.Active           = false;
            m_ledLimitPositive.Active   = false;
            m_ledLimitNegative.Active   = false;
            m_ledMotionDone.Active      = false;
            m_ledMotionTimeout.Active   = false;
            m_ledHomeSwitch.Active      = false;
            m_ledHomeEnd.Active         = false;
            m_ledHomeTimeout.Active     = false;
            m_ledInterlock.Active       = false;
        }
		/// <summary>
		/// 
		/// </summary>
		private void ResetControlValues()
		{
			ResetState();
			ResetPosition();
			ResetRepeat();
			ResetJog();
		}
		/// <summary>
		/// 2021.07.28 by twkang [ADD] 버튼 클릭 확인
		/// </summary>
		private bool ConfirmButtonClick(string strActionName)
		{
			return m_InstanceOfMessageBox.ShowMessage(string.Format("Action : [{0}], This is a confirmation message", strActionName));
		}
		#endregion

		#region 외부인터페이스
		/// <summary>
		/// 2020.06.23 by twkang [ADD] 컨트롤들의 활성화 상태를 설정한다.
		/// </summary>
		public void SetActiveControls(bool bEnable)
		{
			m_btnClear.Enabled				= bEnable;
            m_btnMoveFirst.Enabled			= bEnable;
            m_btnMoveSecond.Enabled			= bEnable;
            m_btnRepeat.Enabled				= bEnable;

            m_btnMoveNegative.Enabled		= bEnable;
            m_btnMovePositive.Enabled		= bEnable;

            m_labelPositionFirst.Enabled    = bEnable;
            m_labelPositionSecond.Enabled   = bEnable;

            m_labelDelayFirst.Enabled       = bEnable;
            m_labelDelaySecond.Enabled      = bEnable;

            m_labelJogGroup.Enabled         = bEnable;

			m_progressbarJog.Enabled		= bEnable;
		}
		/// <summary>
		/// 2020.06.23 by twkang [ADD] 
		/// </summary>
		public void SetValuesToControls(int nIndexOfItem)
		{			
			m_nIndexOfSelectedItem	= nIndexOfItem;

			ResetControlValues();

			int nFirstDelay				= 0;
			int nSecondDelay			= 0;
			double dbFirstPosition		= 0;
			double dbSecondPosition		= 0;

			m_InstanceOfMotion.GetRepeatDelay(nIndexOfItem, ref nFirstDelay, ref nSecondDelay);
			m_InstanceOfMotion.GetRepeatPosition(nIndexOfItem, ref dbFirstPosition, ref dbSecondPosition);

			m_labelDelayFirst.Text		= nFirstDelay.ToString();
			m_labelDelaySecond.Text		= nSecondDelay.ToString();
			m_labelPositionFirst.Text	= dbFirstPosition.ToString();
			m_labelPositionSecond.Text	= dbSecondPosition.ToString();
		}
		/// <summary>
		/// 2020.06.23 by twkang [ADD] Motor State 와 Position 을 업데이트한다.
		/// </summary>
		public void UpdateState()
		{
			if(SELECT_NONE == m_nIndexOfSelectedItem)
			{
				return;
			}

            int nState      = 0;

			m_InstanceOfMotion.GetMotorState(m_nIndexOfSelectedItem, ref nState);
			
			m_ledMotorOn.Active         = m_InstanceOfMotion.GetState(ref nState, MOTOR_STATE.MOTOR_ON);
            m_ledAlarm.Active           = m_InstanceOfMotion.GetState(ref nState, MOTOR_STATE.ALARM);
            m_ledLimitPositive.Active   = m_InstanceOfMotion.GetState(ref nState, MOTOR_STATE.LIMIT_POSITIVE);
            m_ledLimitNegative.Active   = m_InstanceOfMotion.GetState(ref nState, MOTOR_STATE.LIMIT_NEGATIVE);
            m_ledSWLimitPositive.Active = m_InstanceOfMotion.GetState(ref nState, MOTOR_STATE.SWLIMIT_POSITIVE);
            m_ledSWLimitNegative.Active = m_InstanceOfMotion.GetState(ref nState, MOTOR_STATE.SWLIMIT_NEGATIVE);
            m_ledMotionDone.Active      = m_InstanceOfMotion.GetState(ref nState, MOTOR_STATE.MOTION_DONE);
            m_ledMotionTimeout.Active   = m_InstanceOfMotion.GetState(ref nState, MOTOR_STATE.MOTION_TIMEOUT);
            m_ledHomeSwitch.Active      = m_InstanceOfMotion.GetState(ref nState, MOTOR_STATE.HOME_SWITCH);
            m_ledHomeEnd.Active         = m_InstanceOfMotion.GetState(ref nState, MOTOR_STATE.HOME_DONE);
            m_ledHomeTimeout.Active     = m_InstanceOfMotion.GetState(ref nState, MOTOR_STATE.HOME_TIMEOUT);
            m_ledInterlock.Active       = m_InstanceOfMotion.GetState(ref nState, MOTOR_STATE.OCCUR_INTERLOCK);

			double dbActualPos		= m_InstanceOfMotion.GetActualPosition(m_nIndexOfSelectedItem);
			double dbCommandPos		= m_InstanceOfMotion.GetCommandPosition(m_nIndexOfSelectedItem);
			double dbErrorPos		= dbCommandPos	- dbActualPos;

			m_labelError.Text		= String.Format("{0:0.0000}", dbErrorPos);
			m_labelCommand.Text		= String.Format("{0:0.0000}", dbCommandPos);
			m_labelActual.Text		= String.Format("{0:0.0000}", dbActualPos);

			SetRepeatControls();
		}
		/// <summary>
		/// 2020.06.30 by twkang [ADD] 리핏 동작에 따라 컨트롤 사용 유무를 설정한다.
		/// </summary>
		public void SetRepeatControls()
		{
			bool bRepeat			= m_InstanceOfMotion.IsRepeat(m_nIndexOfSelectedItem);

			DisplayRepeatState();

			m_ledRepeat.Active		= bRepeat;

			m_btnMoveFirst.Enabled			= !bRepeat;
			m_btnMoveSecond.Enabled			= !bRepeat;
			m_labelPositionFirst.Enabled	= !bRepeat;
			m_labelDelayFirst.Enabled		= !bRepeat;
			m_labelPositionSecond.Enabled	= !bRepeat;
			m_labelDelaySecond.Enabled		= !bRepeat;
		}
		private void DisplayRepeatState()
		{
			ResetRepeatLed();

			FrameOfSystem3.Config.ConfigMotion.RepeatItem.EN_REPEAT_STATE enState	= FrameOfSystem3.Config.ConfigMotion.RepeatItem.EN_REPEAT_STATE.DELAY_FIRST;
			if(false == m_InstanceOfMotion.GetRepeatItemState(m_nIndexOfSelectedItem, ref enState))
			{
				return;
			}
			switch(enState)
			{
				case FrameOfSystem3.Config.ConfigMotion.RepeatItem.EN_REPEAT_STATE.DELAY_FIRST:
				case FrameOfSystem3.Config.ConfigMotion.RepeatItem.EN_REPEAT_STATE.FIRST_MOTION_DELAY_FINISH:
					m_ledFirstDelay.Active	= true;
					break;
				case FrameOfSystem3.Config.ConfigMotion.RepeatItem.EN_REPEAT_STATE.SECOND_MOTION_DELAY_FINISH:
				case FrameOfSystem3.Config.ConfigMotion.RepeatItem.EN_REPEAT_STATE.DELAY_SECOND:
					m_ledSecondDelay.Active	= true;
					break;
				case FrameOfSystem3.Config.ConfigMotion.RepeatItem.EN_REPEAT_STATE.WATING_SET_DELAY_FIRST:
                    m_ledFirstPosition.Active = true;
					break;
				case FrameOfSystem3.Config.ConfigMotion.RepeatItem.EN_REPEAT_STATE.WATING_SET_DELAY_SECOND:
                    m_ledSecondPosition.Active = true;
					break;
			}
		}
		#endregion

		#region UI 이벤트
		/// <summary>
		/// 2020.06.30 by twkang [ADD] MOVE 관련 버튼을 클릭했을 때
		/// </summary>
		private void Click_Move(object sender, EventArgs e)
		{
			double dblPositionFirst		= double.Parse(m_labelPositionFirst.Text);
			int nDelayFirst				= int.Parse(m_labelDelayFirst.Text);
			double dblPositionSecond	= double.Parse(m_labelPositionSecond.Text);
			int nDelaySecond			= int.Parse(m_labelDelaySecond.Text);

			Control ctrl	= sender as Control;

			if(false == ConfirmButtonClick("MOVE " + ctrl.Text))
				return;

			switch(ctrl.TabIndex)
			{
				case 0: // MOVE FIRST
					m_InstanceOfMotion.MoveAbsolutely(m_nIndexOfSelectedItem, dblPositionFirst, MOTION_SPEED_CONTENT.RUN, DEFAULT_RATIO, DEFAULT_DELAY);
					break;
				case 1:	// MOVE SECOND
					m_InstanceOfMotion.MoveAbsolutely(m_nIndexOfSelectedItem, dblPositionSecond, MOTION_SPEED_CONTENT.RUN, DEFAULT_RATIO, DEFAULT_DELAY);
					break;
				case 2:	// MOVE REPEAT
					m_InstanceOfMotion.SetRepeat(m_nIndexOfSelectedItem, dblPositionFirst, nDelayFirst, dblPositionSecond, nDelaySecond);
					SetRepeatControls();
					break;
			}
		}
		/// <summary>
		/// 2020.06.30 by twkang [ADD] 라벨을 클릭했을 때
		/// </summary>
		private void Click_Labels(object sender, EventArgs e)
		{
			Control ctrl		= sender as Control;
			double dblValue		= 0;

			switch(ctrl.TabIndex)
			{
				case 0: // POSITION FIRST
					if(m_InstanceOfCalculator.CreateForm(m_labelPositionFirst.Text, MIN_OF_PARAM, MAX_OF_PARAM))
					{
						m_InstanceOfCalculator.GetResult(ref dblValue);
						m_labelPositionFirst.Text		= dblValue.ToString();
					}
					break;
				case 1:	// DELAY FIRST
					if(m_InstanceOfCalculator.CreateForm(m_labelDelayFirst.Text, MIN_OF_PARAM, MAX_OF_PARAM))
					{
						m_InstanceOfCalculator.GetResult(ref dblValue);
						m_labelDelayFirst.Text			= dblValue.ToString();
					}
					break;
				case 2:	// POSITION SECOND
					if(m_InstanceOfCalculator.CreateForm(m_labelPositionSecond.Text, MIN_OF_PARAM, MAX_OF_PARAM))
					{
						m_InstanceOfCalculator.GetResult(ref dblValue);
						m_labelPositionSecond.Text		= dblValue.ToString();
					}
					break;
				case 3:	// DELAY SECOND
					if(m_InstanceOfCalculator.CreateForm(m_labelDelaySecond.Text, MIN_OF_PARAM, MAX_OF_PARAM))
					{
						m_InstanceOfCalculator.GetResult(ref dblValue);
						m_labelDelaySecond.Text			= dblValue.ToString();
					}
					break;
			}
		}
		/// <summary>
		/// 2020.07.01 by twkang [ADD] State Clear 버튼을 눌렀을 때
		/// </summary>
		private void Click_StateClear(object sender, EventArgs e)
		{
			if(ConfirmButtonClick("POSITION CLEAR"))
			{
				m_InstanceOfMotion.SetActualPosition(m_nIndexOfSelectedItem, POSITION_CLEAR);
				m_InstanceOfMotion.SetCommandPosition(m_nIndexOfSelectedItem, POSITION_CLEAR);
			}
		}
		/// <summary>
		/// 2020.11.05 by twkang [ADD] 마우스를 눌렀을 떄 이벤트. 모션 속도 이동을 수행한다.
		/// </summary>
		private void Down_MoveAtSpeed(object sender, MouseEventArgs e)
		{
			Control ctrl	= sender as Control;

			bool bDir		= ctrl.TabIndex == 1;

			m_InstanceOfMotion.MoveAtSpeed(m_nIndexOfSelectedItem, bDir, (int)m_progressbarJog.Tick);
		}
		/// <summary>
		/// 2020.11.05 by twkang [ADD] 마우스 Up 이벤트 처리, 모션 정지
		/// </summary>
		private void Up_MoveAtSpeed(object sender, MouseEventArgs e)
		{
			m_InstanceOfMotion.StopMotion(m_nIndexOfSelectedItem, false);
		}
		#endregion
	}
}
