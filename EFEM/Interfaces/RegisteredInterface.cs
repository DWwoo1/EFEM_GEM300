using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using RegisteredInstances_;

using Alarm_;
using Cylinder_;
using DigitalIO_;
using AnalogIO_;
using Motion_;
using FrameOfSystem3.Config;

namespace FrameOfSystem3.Interface
{
    /// <summary>
	/// 2020.05.20 by yjlee [ADD] Override the interfaces for the registered instances.
	/// </summary>
	public class RegisteredInterface : RegisteredInterfaces
	{
		#region Variable
		private Alarm m_instanceAlarm         = null;
		private Cylinder m_instanceCylinder     = null;
		private DigitalIO m_instanceDigitalIO = null;
		private AnalogIO m_instanceAnalogIO   = null;
		private Motion m_instanceMotion         = null;

		#endregion

		public RegisteredInterface()
		{
			m_instanceAlarm     = Alarm.GetInstance();
			m_instanceCylinder  = Cylinder.GetInstance();
			m_instanceDigitalIO = DigitalIO.GetInstance();
			m_instanceAnalogIO  = AnalogIO.GetInstance();
			m_instanceMotion    = Motion.GetInstance();
		}
		#region Alarm Interfaces
		/// <summary>
		/// 2020.05.20 by yjlee [ADD] Add the instance of the alarm.
		/// </summary>
		public override bool AddAlarmInstance(ref int nIndexOfInstance)
		{
			return m_instanceAlarm.AddAlarmInstance(nIndexOfInstance);
		}

		/// <summary>
		/// 2020.05.20 by yjlee [ADD] Make the default alarm and add the alarm of the task.
		/// </summary>
        public override void MakeDefaultAlarm(ref int nIndexOfInstance, ref Alarm_.ALARM_TYPE enType, ref int[] arData)
		{
            m_instanceAlarm.MakeDefaultAlarm(nIndexOfInstance, enType, ref arData);
		}

		/// <summary>
		/// 2020.05.20 by yjlee [ADD] Generate an alarm on the task.
		/// </summary>
		public override void GenerateAlarm(ref int nIndexOfInstance, ref int nObjectCode, ref int nAlarmCode, ref bool bUseOption, ref string[] arSubInformation)
        {
            int nGeneratedAlarmCode = (int)Alarm_.ALARM_CODE_SECTION.TASK * nIndexOfInstance
                                            + (int)Alarm_.ALARM_CODE_SECTION.OBJECT * nObjectCode
                                            + nAlarmCode;

            Config.ConfigAlarm.GetInstance().AddSubInformation(ref nGeneratedAlarmCode, ref arSubInformation);

            m_instanceAlarm.GenerateAlarm(nIndexOfInstance, nObjectCode, nAlarmCode, bUseOption);
        }
        #endregion Alarm Interfaces

		#region Cylinder Interfaces
		/// <summary>
		/// 2020.09.23 by yjlee [ADD] Check whether a cylinder item is enabled.
		/// </summary>
		public override bool IsCylinderItemEnabled(ref int nIndexOfItem, ref string strNameOfItem, ref int nForwardDelay, ref int nBackwardDelay)
		{
			return m_instanceCylinder.IsItemEnabled(nIndexOfItem, ref strNameOfItem, ref nForwardDelay, ref nBackwardDelay);
		}

		/// <summary>
		/// 2024.09.02 by jhshin [MOD] add instance index, action name, retry count
		/// 2020.05.20 by yjlee [ADD] Move the cylinder to forward.
		/// </summary>
		public override CYLINDER_RESULT MoveForward(int nIndexOfInstance, ref int nIndexOfItem, string actionName, int retryCount)
		{
            return m_instanceCylinder.MoveForward(nIndexOfInstance, nIndexOfItem, actionName, retryCount);
		}

		/// <summary>
		/// 2024.09.02 by jhshin [MOD] add instance index, action name, retry count
		/// 2020.05.20 by yjlee [ADD] Move the cylinder to backward.
		/// </summary>
		public override CYLINDER_RESULT MoveBackward(int nIndexOfInstance, ref int nIndexOfItem, string actionName, int retryCount)
		{
            return m_instanceCylinder.MoveBackward(nIndexOfInstance, nIndexOfItem, actionName, retryCount);
		}

		/// <summary>
		/// 2020.05.20 by yjlee [ADD] Get the state of the cylinder.
		/// </summary>
		public override CYLINDER_STATE GetCylinderState(ref int nIndexOfItem)
		{
			return m_instanceCylinder.GetCylinderState(nIndexOfItem);
		}
		#endregion Cylinder Interfaces

		#region DigitalIO Interfaces
		/// <summary>
		/// 2020.09.22 by yjlee [ADD] Check whether an item is enabled.
		/// </summary>
		public override bool IsDigitalItemEnabled(ref bool bInput, ref int nIndexOfItem, ref string strNameOfItem)
		{
			// 2023.06.27 by junho [ADD] program 종료 시 exception 발생하여 일단 try-catch 걸어 둠.
			try
			{
				return m_instanceDigitalIO.IsItemEnabled(bInput, nIndexOfItem, ref strNameOfItem);
			}
			catch
			{
				return false;
			}
		}

		/// <summary>
		/// 2020.05.20 by yjlee [ADD] Read the data from the digital input channel.
		/// </summary>
		public override bool ReadInput(ref int nIndexOfItem)
		{
			return m_instanceDigitalIO.ReadInput(nIndexOfItem);
		}

		/// <summary>
		/// 2020.05.20 by yjlee [ADD] Read the data from the digital Output channel.
		/// </summary>
		public override bool ReadOutput(ref int nIndexOfItem)
		{
			return m_instanceDigitalIO.ReadOutput(nIndexOfItem);
		}

		/// <summary>
		/// 2024.09.12 by jhshin [MOD] add instance index, action name, retry
		/// 2020.05.20 by yjlee [ADD] Write the data to digital output channel.
		/// </summary>
		public override DIO_RESULT WriteOutput(ref int nIndexOfInstance, ref int nIndexOfItem, string actionName, ref bool bPulse, ref int retry)
		{
			return m_instanceDigitalIO.WriteOutput(nIndexOfInstance, nIndexOfItem, actionName, bPulse, retry);
		}

        public override bool IsOutputItemWaitingDelay(ref int nIndexOfItem)
        {
			return m_instanceDigitalIO.IsOutputItemWaitingDelay(nIndexOfItem);
        }
        #endregion DigitalIO Interfaces

        #region AnalogIO Interfaces
        /// <summary>
        /// 2020.09.22 by yjlee [ADD] Check whether an analog item is vaild or not.
        /// </summary>
        public override bool IsAnalogItemEnabled(ref bool bInput, ref int nIndexOfItem, ref string strNameOfItem)
		{
			return m_instanceAnalogIO.IsItemEnabled(bInput, nIndexOfItem, ref strNameOfItem);
		}

		/// <summary>
		/// 2020.05.20 by yjlee [ADD] Read the input voltage from the analog input channel.
		/// </summary>
		public override double ReadInputVolt(ref int nIndexOfItem)
		{
			return m_instanceAnalogIO.ReadInputVolt(nIndexOfItem);
		}

		/// <summary>
		/// 2020.05.20 by yjlee [ADD] Read the input value from the analog input channel.
		/// </summary>
		public override double ReadInputValue(ref int nIndexOfItem)
		{
			return m_instanceAnalogIO.ReadInputValue(nIndexOfItem);
		}

		/// <summary>
		/// 2020.05.20 by yjlee [ADD] Read the output voltage from the analog output channel.
		/// </summary>
		public override double ReadOutputVolt(ref int nIndexOfItem)
		{
			return m_instanceAnalogIO.ReadOutputVolt(nIndexOfItem);
		}

		/// <summary>
		/// 2020.05.20 by yjlee [ADD] Read the output value from the analog output channel.
		/// </summary>
		public override double ReadOutputValue(ref int nIndexOfItem)
		{
			return m_instanceAnalogIO.ReadOutputValue(nIndexOfItem);
		}

		/// <summary>
		/// 2024.09.02 by jhshin [MOD] add instance index, action name, retry count
		/// 2020.05.20 by yjlee [ADD] Write the output voltage to the analog output channel.
		/// </summary>
		public override void WriteOutputVolt(int nIndexOfInstance, ref int nIndexOfItem, string actionName, ref double dblVolt, int retryCount)
		{
			m_instanceAnalogIO.WriteOutputVolt(nIndexOfInstance, nIndexOfItem, actionName, dblVolt, retryCount);
		}

		/// <summary>
		/// 2024.09.02 by jhshin [MOD] add instance index, action name, retry count
		/// 2020.05.20 by yjlee [ADD] Write the output value to the analog output channel.
		/// </summary>
		public override void WriteOutputValue(int nIndexOfInstance, ref int nIndexOfItem, string actionName, ref double dblValue, int retryCount)
		{
			m_instanceAnalogIO.WriteOutputValue(nIndexOfInstance, nIndexOfItem, actionName, dblValue, retryCount);
		}
		#endregion AnalogIO Interfaces

		#region Motion Interfaces
		/// <summary>
		/// 2020.09.23 by yjlee [ADD] Check whether an item is enabled or not.
		/// </summary>
		public override bool IsMotionItemEnabled(ref int nIndexOfItem, ref string strNameOfItem, ref double dblVelocity, ref double dblAccDecTime)
		{
			return m_instanceMotion.IsItemEnabled(nIndexOfItem, ref strNameOfItem, ref dblVelocity, ref dblAccDecTime);
		}

		/// <summary>
		/// 2020.05.29 by yjlee [ADD] Reset the alarm of the motor.
		/// </summary>
		public override void DoReset(ref int nIndexOfItem)
		{
			m_instanceMotion.DoReset(nIndexOfItem);
		}

		/// <summary>
		/// 2020.05.29 by yjlee [ADD] On/Off the power of the motor.
		/// </summary>
		public override void DoMotorOn(ref int nIndexOfItem, ref bool bOn)
		{
			m_instanceMotion.DoMotorOn(nIndexOfItem, bOn);
		}

		#region Motor Information
		/// <summary>
		/// 2020.05.20 by yjlee [ADD] Get the states of the motor.
		/// </summary>
		public override void GetMotorState(ref int nIndexOfItem, ref int nState)
		{
			m_instanceMotion.GetMotorState(nIndexOfItem, ref nState);
		}

		/// <summary>
		/// 2020.05.20 by yjlee [ADD] Get the command position of the motor. 
		/// </summary>
		public override double GetCommandPosition(ref int nIndexOfItem)
		{
			return m_instanceMotion.GetCommandPosition(nIndexOfItem);
		}

		/// <summary>
		/// 2020.05.20 by yjlee [ADD] Set the command position of the motor. 
		/// </summary>
		public override void SetCommandPosition(ref int nIndexOfItem, ref double dblPosition)
		{
			m_instanceMotion.SetCommandPosition(nIndexOfItem, dblPosition);
		}

		/// <summary>
		/// 2020.05.20 by yjlee [ADD] Get the actual position of the motor. 
		/// </summary>
		public override double GetActualPosition(ref int nIndexOfItem)
		{
			return m_instanceMotion.GetActualPosition(nIndexOfItem);
		}

		/// <summary>
		/// 2020.05.20 by yjlee [ADD] Set the actual position of the motor. 
		/// </summary>
		public override void SetActualPosition(ref int nIndexOfItem, ref double dblPosition)
		{
			m_instanceMotion.SetActualPosition(nIndexOfItem, dblPosition);
		}

        public override double GetCommandVelocity(ref int nIndexOfItem)
        {
            return m_instanceMotion.GetCommandVelocity(nIndexOfItem);
        }

        public override double GetActualVelocity(ref int nIndexOfItem)
        {
            return m_instanceMotion.GetActualVelocity(nIndexOfItem);
        }


        /// <summary>
        /// 2021.01.14 by yjlee [ADD] Check the motor has been touched the threshold position.
        /// </summary>
        public override bool IsMotionTouched(ref int nIndexOfItem, ref double dblTouchedPosition, ref double dblTouchedEncoderPosition)
        {
            return m_instanceMotion.IsMotionTouched(nIndexOfItem, ref dblTouchedPosition, ref dblTouchedEncoderPosition);
        }
		#endregion Motor Information

		#region Move Commands
		/// <summary>
		/// 2024.09.02 by jhshin [MOD] add instance index
		/// 2020.05.20 by yjlee [ADD] Move to home position of the motor.
		/// </summary>
		public override MOTION_RESULT MoveToHome(int nIndexOfInstance, ref int nIndexOfItem, string actionName, ref int nPreDelay, ref int retryCount)
		{
            return m_instanceMotion.MoveToHome(nIndexOfInstance, nIndexOfItem, actionName, nPreDelay, retryCount);
		}

		/// <summary>
		/// 2024.09.02 by jhshin [MOD] add instance index
		/// 2020.05.20 by yjlee [ADD] Move the motor to the destinaition absolutely.
		/// </summary>
		public override MOTION_RESULT MoveAbsolutely(int nIndexOfInstance, ref int nIndexOfItem, string actionName, ref double dblDestination, ref double dblCustomSpeed, ref MOTION_SPEED_CONTENT enSpeedContent, ref int nRatio, ref int nDelay, ref int nPreDelay, ref  int retryCount)
		{
            return m_instanceMotion.MoveAbsolutely(nIndexOfInstance, nIndexOfItem, actionName, dblDestination, dblCustomSpeed, enSpeedContent, nRatio, nDelay, nPreDelay, retryCount);
		}

		/// <summary>
		/// 2024.09.02 by jhshin [MOD] add instance index
		/// 2020.05.20 by yjlee [ADD] Move the motor to the destinaition releatively.
		/// </summary>
		public override MOTION_RESULT MoveReleatively(int nIndexOfInstance, ref int nIndexOfItem, string actionName, ref double dblDestination, ref double dblCustomSpeed, ref MOTION_SPEED_CONTENT enSpeedContent, ref int nRatio, ref int nDelay, ref int nPreDelay, ref int retryCount)
		{
            return m_instanceMotion.MoveReleatively(nIndexOfInstance, nIndexOfItem, actionName, dblDestination, dblCustomSpeed, enSpeedContent, nRatio, nDelay, nPreDelay, retryCount);
		}

		/// <summary>
		/// 2024.09.02 by jhshin [MOD] add instance index
		/// 2020.05.20 by yjlee [ADD] Move the motor at speed.
		/// </summary>
		public override MOTION_RESULT MoveAtSpeed(int nIndexOfInstance, ref int nIndexOfItem, string actionName, ref double dblCustomSpeed, ref bool bDirection, ref int nRatio, ref int nPreDelay, ref int retryCount)
		{
			return m_instanceMotion.MoveAtSpeed(nIndexOfInstance, nIndexOfItem, actionName, dblCustomSpeed, bDirection, nRatio, nPreDelay, retryCount);
		}

		/// <summary>
		/// 2024.09.02 by jhshin [MOD] add instance index
		/// 2020.05.20 by yjlee [ADD] Move the motor by the list.
		/// </summary>
		public override MOTION_RESULT MoveByList(int nIndexOfInstance, ref int nIndexOfItem, string actionName, ref int nCountOfStep, ref double[] arDestination, ref double[] arCustomSpeed, ref MOTION_SPEED_CONTENT[] arSpeedContent, ref int[] arRatio, ref int nDelay, ref int nPreDelay, ref int retryCount)
		{
            return m_instanceMotion.MoveByList(nIndexOfInstance, nIndexOfItem, actionName, nCountOfStep, ref arDestination, ref arCustomSpeed, ref arSpeedContent, ref arRatio, nDelay, nPreDelay, retryCount);
		}

		/// <summary>
		/// 2024.09.02 by jhshin [MOD] add instance index
		/// 2020.05.20 by yjlee [ADD] Move the motor by the parameters in the list.
		/// </summary>
		public override MOTION_RESULT MoveByLinearCoordination(int nIndexOfInstance, string actionName, ref int nCountOfAxis, ref int[] arIndexes, ref int nCountOfStep, ref double[,] arDestination, ref double[,] arCustomSpeed, ref MOTION_SPEED_CONTENT[,] arSpeedContent, ref int[,] arRatio, ref int nDelay, ref int nPreDelay, ref int retryCount)
		{
            return m_instanceMotion.MoveByLinearCoordination(nIndexOfInstance, actionName,  nCountOfAxis, ref arIndexes, ref nCountOfStep, ref arDestination, ref arCustomSpeed, ref arSpeedContent, ref arRatio, nDelay, nPreDelay, retryCount);
		}
		//2024.09.02 by jhshin [MOD] add instance index
		public override MOTION_RESULT MoveSyncVectorMotion(int nIndexOfInstance, string actionName, ref int nCountOfAxis, ref int[] arIndexes, ref double[] arDestination, ref double dCustomSpeed, ref MOTION_SPEED_CONTENT SpeedContent, ref int nRatio, ref bool bAbsolute, ref bool bOverride, ref int nDelay, ref int nPreDelay, ref int retryCount)
        {
            return m_instanceMotion.MoveSyncVectorMotion(nIndexOfInstance, actionName, ref nCountOfAxis, ref arIndexes, ref arDestination, ref dCustomSpeed, ref SpeedContent, ref nRatio, ref bAbsolute, ref bOverride, ref nDelay, ref nPreDelay, retryCount);
        }
		//2024.09.02 by jhshin [MOD] add instance index
		public override MOTION_RESULT MoveSyncVectorMotionList(int nIndexOfInstance, string actionName, ref int nCountOfAxis, ref int[] arIndexes, ref double[,] arDestination, ref double[] arCustomSpeed, ref MOTION_SPEED_CONTENT[] arSpeedContent, ref int[] arRatio, ref bool bAbsolute, ref int nDelay, ref int nPreDelay, ref int retryCount)
        {
            return m_instanceMotion.MoveSyncVectorMotionList(nIndexOfInstance, actionName, ref nCountOfAxis, ref arIndexes, ref arDestination, ref arCustomSpeed, ref arSpeedContent, ref arRatio, ref bAbsolute, ref nDelay, ref nPreDelay, retryCount);
        }
		//2024.09.02 by jhshin [MOD] add instance index
		public override MOTION_RESULT MoveSyncVectorMotionCompare(int nIndexOfInstance, string actionName, ref int nCountOfAxis, ref int[] arIndexes, ref double[] arDestination, ref double dCustomSpeed, ref MOTION_SPEED_CONTENT SpeedContent, ref int nRatio, ref bool bAbsolute, ref int nIndexOfCompareItem, ref double dblComparePosition, ref bool bPositionLogic, ref bool bUseActualPosition, ref int nDelay, ref int nPreDelay, ref int retryCount)
        {
            return m_instanceMotion.MoveSyncVectorMotionCompare(nIndexOfInstance, actionName, ref nCountOfAxis, ref arIndexes, ref arDestination, ref dCustomSpeed, ref SpeedContent, ref nRatio, ref bAbsolute, ref nIndexOfCompareItem, ref dblComparePosition, ref bPositionLogic, ref bUseActualPosition, ref nDelay, ref nPreDelay, retryCount);
        }
		/// <summary>
		/// 2024.09.02 by jhshin [MOD] add instance index
		/// 2020.12.29 by yjlee [ADD] Move the motor until the encoder position is over with the threshold.
		/// </summary>
		public override MOTION_RESULT MoveUntilTouch(int nIndexOfInstance, ref int nIndexOfItem, string actionName, ref double dblDestination, ref int nIndexOfEncoder, ref double dblEncoderThreshold, ref double dblCustomSpeed, ref MOTION_SPEED_CONTENT enSpeedContent, ref int nRatio, ref int nDelay, ref int nPreDelay, ref int retryCount)
        {
            return m_instanceMotion.MoveUntilTouch(nIndexOfInstance, nIndexOfItem, actionName, dblDestination, nIndexOfEncoder, dblEncoderThreshold, dblCustomSpeed, enSpeedContent, nRatio, nDelay, nPreDelay, retryCount);
        }
		/// <summary>
		/// 2024.09.02 by jhshin [MOD] add instance index
		/// 2022.10.20 by wdw [ADD] Move the motor until the analog value is over with the threshold.
		/// </summary>
		public override MOTION_RESULT MoveUntilTouch_Analog(int nIndexOfInstance, ref int nIndexOfItem, string actionName, ref double dblDestination, ref int[] nArrConfigIndexesOfAnalog, ref double dblAnalogThreshold, ref double dblCustomSpeed, ref MOTION_SPEED_CONTENT enSpeedContent, ref int nRatio, ref int nDelay, ref int nPreDelay, ref int retryCount)
        {
            // 2024.03.11. by shkim. [MOD] Multi Controller를 위한 코드로 수정(Registered 측에서는 Analog device의 실제 타겟 넘버를 알 수 있는 방법이 없다.)
            int analogCount = nArrConfigIndexesOfAnalog == null ? 0 : nArrConfigIndexesOfAnalog.Length;

            int[] contollerTargetAnalogIndexes = new int[analogCount];

            for (int i = 0; i < analogCount; i++)
			{
				if(false == AnalogIO.GetInstance().GetChannelInController(true, nArrConfigIndexesOfAnalog[i], ref contollerTargetAnalogIndexes[i]))
				{
                    return MOTION_RESULT.MISMATCH_LIST_PARAMETER;
                }
            }
            return m_instanceMotion.MoveUntilTouch_Analog(nIndexOfInstance, nIndexOfItem, actionName, dblDestination, ref contollerTargetAnalogIndexes, dblAnalogThreshold, dblCustomSpeed, enSpeedContent, nRatio, nDelay, nPreDelay, retryCount);
            // 2024.03.11. by shkim. [END]
        }
		/// <summary>
		/// 2024.09.02 by jhshin [MOD] add instance index
		/// 2022.10.25 by wdw [ADD] Move the motor by the list until the encoder value is over with the threshold.
		/// </summary>
		/// 
		public override MOTION_RESULT MoveByListUntilTouch(int nIndexOfInstance, ref int nIndexOfItem, string actionName, ref int nStepCount, ref double[] arDestination, ref int nIndexOfEncoder, ref double dblEncoderThreshold,  ref double[] arCustomSpeed, ref MOTION_SPEED_CONTENT[] arSpeedContent, ref int[] arRatio, ref int nDelay, ref int nPreDelay, ref int retryCount)
        {
            return m_instanceMotion.MoveByListUntilTouch(nIndexOfInstance, nIndexOfItem, actionName, nStepCount, ref arDestination, nIndexOfEncoder, dblEncoderThreshold, ref arCustomSpeed, ref arSpeedContent, ref arRatio, nDelay, nPreDelay, retryCount);
        }
		/// <summary>
		/// 2024.09.02 by jhshin [MOD] add instance index
		/// 2022.10.25 by wdw [ADD] Move the motor by the list until the analog value is over with the threshold.
		/// </summary>
		public override MOTION_RESULT MoveByListUntilTouch_Analog(int nIndexOfInstance, ref int nIndexOfItem, string actionName, ref int nStepCount, ref double[] arDestination, ref int[] nArrConfigIndexesOfAnalog, ref double dblAnalogThreshold, ref double[] arCustomSpeed, ref MOTION_SPEED_CONTENT[] arSpeedContent, ref int[] arRatio, ref int nDelay, ref int nPreDelay, ref int retryCount)
        {
            // 2024.03.11. by shkim. [MOD] Multi Controller를 위한 코드로 수정(Registered 측에서는 Analog device의 실제 타겟 넘버를 알 수 있는 방법이 없다.)
            int analogCount = nArrConfigIndexesOfAnalog == null ? 0 : nArrConfigIndexesOfAnalog.Length;

            int[] contollerTargetAnalogIndexes = new int[analogCount];

            for (int i = 0; i < analogCount; i++)
            {
                if (false == AnalogIO.GetInstance().GetChannelInController(true, nArrConfigIndexesOfAnalog[i], ref contollerTargetAnalogIndexes[i]))
                {
                    return MOTION_RESULT.MISMATCH_LIST_PARAMETER;
                }
            }
            return m_instanceMotion.MoveByListUntilTouch_Analog(nIndexOfInstance, nIndexOfItem, actionName, nStepCount, ref arDestination, ref contollerTargetAnalogIndexes, dblAnalogThreshold, ref arCustomSpeed, ref arSpeedContent, ref arRatio, nDelay, nPreDelay, retryCount);
            // 2024.03.11. by shkim. [END]
        }
		/// <summary>
		/// 2024.09.02 by jhshin [MOD] add instance index
		/// 2020.05.20 by yjlee [ADD] Override the speed of the motor.
		/// </summary>
		public override MOTION_RESULT OverrideMotion(int nIndexOfInstance, ref int nIndexOfItem, string actionName, ref MOTION_OVERRIDE_TYPE enOverrideType, ref double dblDestination, ref MOTION_SPEED_CONTENT enSpeedContent, ref int nRatio, ref int retryCount)
        {
            return m_instanceMotion.OverrideMotion(nIndexOfInstance, nIndexOfItem, actionName, enOverrideType, dblDestination, enSpeedContent, nRatio, retryCount);
        }

		/// <summary>
		/// 2024.09.02 by jhshin [MOD] add instance index
		/// 2020.05.20 by yjlee [ADD] Stop the motor.
		/// </summary>
		public override MOTION_RESULT StopMotion(int nIndexOfInstance, ref int nIndexOfItem, string actionName, ref bool bEmergency, ref int nDelay, ref int retryCount)
		{
            return m_instanceMotion.StopMotion(nIndexOfInstance, nIndexOfItem, actionName, bEmergency, nDelay, retryCount);
		}
		/// <summary>
		/// 2024.09.02 by jhshin [MOD] add instance index
		/// 2021.07.13 by yjlee [ADD] Move the axes while comparing.
		/// </summary>
		public override MOTION_RESULT MoveCompare(int nIndexOfInstance, ref int nIndexOfItem, string actionName, ref double dblPosition, ref MOTION_SPEED_CONTENT enSpeedContent, ref int nIndexOfCompareItem, ref double dblComparePosition, ref bool bPositionLogic, ref bool bUseActualPosition, ref int nPreDelay, ref int retryCount)
        {
            return m_instanceMotion.MoveCompare(nIndexOfInstance, nIndexOfItem, actionName, dblPosition, enSpeedContent, nIndexOfCompareItem, dblComparePosition, bPositionLogic, bUseActualPosition, nPreDelay, retryCount);
        }

		/// <summary>
		/// 2024.09.02 by jhshin [MOD] add instance index
		/// 2021.07.13 by yjlee [ADD] Move an axis with the position, velocity and the time.
		/// </summary>
		public override MOTION_RESULT MovePVT(int nIndexOfInstance, ref int nIndexOfItem, string actionName, ref int nCountOfStep, ref double[] arPosition, ref double[] arVelocity, ref double[] arTime, ref int nPreDelay, ref int retryCount)
        {
            return m_instanceMotion.MovePVT(nIndexOfInstance, nIndexOfItem, actionName, nCountOfStep, ref arPosition, ref arVelocity, ref arTime, nPreDelay, retryCount);
        }
		#endregion

        #region Multi Commands

        /// <summary>
        /// 2021.05.06 by yjlee [ADD] Move the multi axes to the home position.
        /// </summary>
        public override MOTION_RESULT MoveMultiToHome(int nIndexOfInstance, string actionName, ref int nCountOfAxis, ref int[] arAxes, ref int retryCount)
        {
            return m_instanceMotion.MoveMultiToHome(nIndexOfInstance, actionName, nCountOfAxis, ref arAxes, retryCount);
        }

		/// <summary>
		/// 2024.09.02 by jhshin [MOD] add instance index
		/// 2021.05.06 by yjlee [ADD] Move the multi axes absolutely.
		/// </summary>
		public override MOTION_RESULT MoveMultiAbsolutely(int nIndexOfInstance, string actionName ,ref int nCountOfAxis, ref int[] arAxes, ref double[] arDestination, ref MOTION_SPEED_CONTENT[] arSpeedContent, ref int[] arRatio, ref int nDelay, ref int retryCount)
        {
            return m_instanceMotion.MoveMultiAbsolutely(nIndexOfInstance, actionName, nCountOfAxis, ref arAxes, ref arDestination, ref arSpeedContent, ref arRatio, nDelay, retryCount);
        }

		/// <summary>
		/// 2024.09.02 by jhshin [MOD] add instance index
		/// 2021.05.06 by yjlee [ADD] Move the multi axes releatively.
		/// </summary>
		public override MOTION_RESULT MoveMultiReleatively(int nIndexOfInstance, string actionName, ref int nCountOfAxis, ref int[] arAxes, ref double[] arDestination, ref MOTION_SPEED_CONTENT[] arSpeedContent, ref int[] arRatio, ref int nDelay, ref int retryCount)
        {
            return m_instanceMotion.MoveMultiReleatively(nIndexOfInstance, actionName, nCountOfAxis, ref arAxes, ref arDestination, ref arSpeedContent, ref arRatio, nDelay, retryCount);
        }

		/// <summary>
		/// 2024.09.02 by jhshin [MOD] add instance index
		/// 2021.05.06 by yjlee [ADD] Move the multi axes at speed.
		/// </summary>
		public override MOTION_RESULT MoveMultiAtSpeed(int nIndexOfInstance, string actionName, ref int nCountOfAxis, ref int[] arAxes, ref bool[] arDirection, ref int[] arRatio, ref int retryCount)
        {
            return m_instanceMotion.MoveMultiAtSpeed(nIndexOfInstance, actionName, nCountOfAxis, ref arAxes, ref arDirection, ref arRatio, retryCount);
        }

		/// <summary>
		/// 2024.09.02 by jhshin [MOD] add instance index
		/// 2021.05.06 by yjlee [ADD] Stop the multi motion.
		/// - If uses the multi command, have to use it to stop the motion.
		/// </summary>
		public override MOTION_RESULT StopMultiMotion(int nIndexOfInstance, string actionName, ref int nCountOfAxis, ref int[] arAxes, ref bool bEmergency, ref int retryCount)
        {
            return m_instanceMotion.StopMultiMotion(nIndexOfInstance, actionName, nCountOfAxis, ref arAxes, bEmergency, retryCount);
        }
        #endregion End - Multi Commands

        /// <summary>
        /// 2022.09.06. by shkim. [ADD] 현재 Torque 값을 확인한다. (For PAA-100HTX)
        /// </summary>
        public override bool GetCurrentMotorTorqueValue(int nAxis, ref short dCurrentTorque)
        {
            return m_instanceMotion.GetCurrentMotorTorqueValue(nAxis, ref dCurrentTorque);
        }
		
		/// <summary>
        /// 2022.09.06. by shkim. [ADD] Torque Limit Position Event (For PAA-100HTX)
        /// </summary>
        public override bool SetChangeTorqueLimitPositionEvent(int nIndexOfItem, double dEventPosition, ushort nDefaultTorque, ushort nLimitTorque, double dEventPositionWidth)
        {
            return m_instanceMotion.SetChangeTorqueLimitPositionEvent(nIndexOfItem, dEventPosition, nDefaultTorque, nLimitTorque, dEventPositionWidth);
        }

		/// <summary>
		/// 2024.09.02 by jhshin [MOD] add instance index
		/// 2022.09.06. by shkim. [ADD] 절대위치이동, 특정 Position에 Torque Limit이 동작하는 Trigger가 걸려있을 때 사용한다.(For PAA-100HTX)
		/// Motion Done을 Controller Signal이 아닌 Command Position과 Target Position의 차이가 dblCustomSettlingDistance[mm] 이내에 있는지로 판별한다.
		/// </summary>
		public override MOTION_RESULT MoveAbsolutelyWithToqueLimitPositionEvent(int nIndexOfInstance, ref int nIndexOfItem, string actionName, ref double dblDestination, ref double dblCustomSpeed, ref Motion_.MOTION_SPEED_CONTENT enSpeedContent, ref int nRatio, ref int nDelay, double dblCustomSettlingDistance, ref int retryCount)
		{
			return m_instanceMotion.MoveAbsolutelyWithToqueLimitPositionEvent(nIndexOfInstance, nIndexOfItem, actionName, dblDestination, dblCustomSpeed, enSpeedContent, nRatio, nDelay, dblCustomSettlingDistance, retryCount);
		}

		/// <summary>
		/// 2024.09.02 by jhshin [MOD] add instance index
		/// 2022.09.06. by shkim. [ADD] 절대위치리스트이동, 특정 Position에 Torque Limit이 동작하는 Trigger가 걸려있을 때 사용한다.(For PAA-100HTX)
		/// Motion Done을 Controller Signal이 아닌 Command Position과 Target Position의 차이가 dblCustomSettlingDistance[mm] 이내에 있는지로 판별한다.
		/// </summary>
		public override MOTION_RESULT MoveByListWithToqueLimitPositionEvent(int nIndexOfInstance, ref int nIndexOfItem, string actionName, ref int nCountOfStep, ref double[] arDestination, ref double[] arCustomSpeed, ref MOTION_SPEED_CONTENT[] arSpeedContent, ref int[] arRatio, ref int nDelay, ref bool bIsToqueLimitCondition, ref double dCustomSettlingDistance, ref int retryCount)
		{
            return m_instanceMotion.MoveByListWithToqueLimitPositionEvent(nIndexOfInstance, nIndexOfItem, actionName, nCountOfStep, ref arDestination, ref arCustomSpeed, ref arSpeedContent, ref arRatio, nDelay, bIsToqueLimitCondition, dCustomSettlingDistance, retryCount);
		}
        #endregion Motion Interfaces
    }
}
