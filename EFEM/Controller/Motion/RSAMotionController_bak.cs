using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Motion_;
using RSASDK_.Motion_;

namespace FrameOfSystem3.Controller.Motion
{
    extern alias MotionInstance;

    /// <summary>
    /// 2020.11.09 by yjlee [ADD] Abstract class for the motion using RS Automation SDK.
    /// </summary>
    class RSAMotionController : MotionController
    {
        #region Variables
        RSAMotion m_instanceSDK     = null;
        #endregion

        #region Internal Interface
        /// <summary>
        /// 2020.11.09 by yjlee [ADD] Copy the parameter value from the app to the controller.
        /// </summary>
        private void CopyParameter(ref RSAPARAM_CONFIG rsaParam, ref PARAM_CONFIG paramConfig)
        {
            switch(paramConfig.enMotionType)
            {
                case Motion_.MOTION_TYPE.CIRCULAR:
                    rsaParam.enMotionType       = RSASDK_.Motion_.MOTION_TYPE.CIRCULAR;
                    break;

                case Motion_.MOTION_TYPE.LINEAR:
                    rsaParam.enMotionType       = RSASDK_.Motion_.MOTION_TYPE.LINEAR;
                    break;
            }

            rsaParam.bEncoderDirection              = paramConfig.bEncoderDirection;
            rsaParam.bCommandDirection              = paramConfig.bCommandDirection;

            rsaParam.bAlarmStopMode                 = paramConfig.bAlarmStopMode;

            rsaParam.bZPhaseLogic                   = paramConfig.bZPhaseLogic;

            rsaParam.bUseHWLimitPositive            = paramConfig.bUseHWLimitPositive;
            rsaParam.bHWLimitLogicPositive          = paramConfig.bHWLimitLogicPositive;
            rsaParam.bUseHWLimitNegative            = paramConfig.bUseHWLimitNegative;
            rsaParam.bHWLimitLogicNegative          = paramConfig.bHWLimitLogicNegative;

            rsaParam.bUseSWLimitPositive            = paramConfig.bUseSWLimitPositive;
            rsaParam.dblSWLimitPositionPositive     = paramConfig.dblSWLimitPositionPositive;
            rsaParam.bUseSWLimitNegative            = paramConfig.bUseSWLimitNegative;
            rsaParam.dblSWLimitPositionNegative     = paramConfig.dblSWLimitPositionNegative;
        }

        /// <summary>
        /// 2020.11.09 by yjlee [ADD] Copy the parameter value from the app to the controller.
        /// </summary>
        private void CopyParameter(ref RSAPARAM_HOME rsaParam, ref PARAM_HOME paramHome)
        {
            switch(paramHome.enHomeMode)
            {
                case HOME_MODE.ORG_ONESTEP:
                case HOME_MODE.ORG_TWOSTEP:
                case HOME_MODE.EZ_ONESTEP:
                    rsaParam.enHomeType     = HOME_TYPE.ABS_SWITCH;
                    break;

                case HOME_MODE.ORG_EZ_ONESTEP_INSTANTSTOP:
                case HOME_MODE.ORG_EZ_ONESTEP_STOP:
                case HOME_MODE.ORG_EZ_TWOSTEP_INSTANTSTOP:
                case HOME_MODE.ORG_EZ_TWOSTEP_STOP:
                case HOME_MODE.EL_EZ_TWOSTEP_STOP:
                case HOME_MODE.EL_EZ_TWOSTEP_INSTANTSTOP:
                    rsaParam.enHomeType     = HOME_TYPE.REF_PULSE;
                    break;

                case HOME_MODE.EL_ONESTEP:
                case HOME_MODE.EL_TWOSTEP:
                    rsaParam.enHomeType     = HOME_TYPE.LIMIT_SWITCH;
                    break;

                case HOME_MODE.FIXED:
                case HOME_MODE.ABSOLUTE_ONLY:
                    rsaParam.enHomeType     = HOME_TYPE.FIXED;
                    break;
            }

            rsaParam.bHomeLogic         = paramHome.bHomeLogic;

            rsaParam.bHomeDirection         = paramHome.bHomeDirection;
            rsaParam.dblHomeOffset          = paramHome.dblHomeOffset;
            rsaParam.dblHomeBasePosition    = paramHome.dblHomeBasePosition;

            rsaParam.dblHomeVelocity        = paramHome.dblHomeVelocityStart;
            rsaParam.dblHomeSecondVelocity  = paramHome.dblHomeVelocityLast;

            rsaParam.dblHomeAcceleration    = paramHome.dblHomeAcceleration;
            rsaParam.dblHomeDeceleration    = paramHome.dblHomeDeceleration;

            switch(paramHome.enHomeSpeedPattern)
            {
                case MOTION_SPEED_PATTERN.CONSTANT:
                case MOTION_SPEED_PATTERN.TRAPEZODIAL:
                    rsaParam.dblHomeJerk    = 0;
                    break;

                default:
                    rsaParam.dblHomeJerk    = (paramHome.dblHomeAccelJerk + paramHome.dblHomeDecelJerk) * 0.5;
                    break;
            }
        }

        /// <summary>
        /// 2020.11.09 by yjlee [ADD] Copy the parameter value from the app to the controller.
        /// </summary>
        private void CopyParameter(ref RSAPARAM_GANTRY rsaParam, ref PARAM_GANTRY paramGantry)
        {
            rsaParam.bEnableGantry          = paramGantry.bEnableGantry;
            rsaParam.nAxisOfMaster          = paramGantry.nIndexOfMaster;
            rsaParam.nAxisOfSlave           = paramGantry.nIndexOfSlave;
            rsaParam.bReverseSlaveDirection = paramGantry.bReverseSlaveDirection;
        }

        /// <summary>
        /// 2020.11.09 by yjlee [ADD] Copy the parameter value from the app to the controller.
        /// </summary>
        private void CopyParameter(ref RSAPARAM_SPEED rsaParam, ref PARAM_SPEED paramSpeed)
        {
            switch(paramSpeed.enSpeedPattern)
            {
                case MOTION_SPEED_PATTERN.CONSTANT:
                case MOTION_SPEED_PATTERN.TRAPEZODIAL:
                    rsaParam.dblJerk    = 0;
                    break;

                default:
                    rsaParam.dblJerk    = (paramSpeed.dblAccelJerk + paramSpeed.dblDecelJerk) * 0.5;
                    break;
            }

            rsaParam.dblVelocity        = paramSpeed.dblVelocity;
            rsaParam.dblMaxVelocity     = paramSpeed.dblMaxVelocity;
            rsaParam.dblAcceleration    = paramSpeed.dblAcceleration;
            rsaParam.dblDeceleration    = paramSpeed.dblDeceleration;
        }
        #endregion

        #region Controller
        /// <summary>
        /// 2020.11.09 by yjlee [ADD] Initialize the controller.
        /// </summary>
        public override bool InitController()
        {
            m_instanceSDK       = RSAMotion.GetInstance();

            return m_instanceSDK.InitController();
        }

        /// <summary>
        /// 2020.11.09 by yjlee [ADD] Exit the controller.
        /// </summary>
        public override void ExitController()
        {
            m_instanceSDK.ExitController();
        }

        /// <summary>
        /// 2020.11.09 by yjlee [ADD] Get the count of the axes.
        /// </summary>
        public override int GetCountOfAxis()
        {
            return m_instanceSDK.GetCountOfAxis();
        }
        #endregion

        #region Motor Configuration
        /// <summary>
        /// 2020.11.09 by yjlee [ADD] Set the motor configuration.
        /// </summary>
        public override void SetMotorConfiguration(ref int nAxis, ref PARAM_CONFIG paramConfig)
        {
            RSAPARAM_CONFIG rsaParam       = new RSAPARAM_CONFIG();

            CopyParameter(ref rsaParam, ref paramConfig);

            m_instanceSDK.SetMotorConfiguration(ref nAxis, ref rsaParam);
        }

        /// <summary>
        /// 2020.11.09 by yjlee [ADD] Set the motor configuration for the home.
        /// </summary>
        public override void SetMotorHomeConfiguration(ref int nAxis, ref PARAM_HOME paramHome)
        {
            RSAPARAM_HOME rsaParam = new RSAPARAM_HOME();

            CopyParameter(ref rsaParam, ref paramHome);

            m_instanceSDK.SetMotorHomeConfiguration(ref nAxis, ref rsaParam);
        }

        /// <summary>
        /// 2020.11.09 by yjlee [ADD] Set the gantry state of the motion.
        /// </summary>
        public override void SetGantry(ref int nAxis, ref PARAM_GANTRY paramGantry, ref PARAM_SPEED paramSpeed)
        {
            RSAPARAM_GANTRY rsaParam        = new RSAPARAM_GANTRY();
            RSAPARAM_SPEED rsaParamSpeed    = new RSAPARAM_SPEED();

            CopyParameter(ref rsaParam, ref paramGantry);
            CopyParameter(ref rsaParamSpeed, ref paramSpeed);

            m_instanceSDK.SetGantry(ref nAxis, ref rsaParam, ref rsaParamSpeed);
        }

        /// <summary>
        /// 2020.11.09 by yjlee [ADD] Get the gantry state of the motion.
        /// </summary>
        public override bool GetGantryState(ref int nAxis, ref PARAM_GANTRY paramGantry)
        {
            RSAPARAM_GANTRY rsaParam        = new RSAPARAM_GANTRY();

            CopyParameter(ref rsaParam, ref paramGantry);

            return m_instanceSDK.GetGantryState(ref nAxis, ref rsaParam);
        }
        #endregion

        #region Motor Information
        /// <summary>
        /// 2020.11.09 by yjlee [ADD] Get the motor states of the axes.
        /// </summary>
        public override void GetMotorInformationAll(ref int nCountOfAxis, ref int[] arState, ref double[] arActualPosition, ref double[] arCommandPosition)
        {
            m_instanceSDK.GetMotorInformationAll(ref nCountOfAxis, ref arState, ref arActualPosition, ref arCommandPosition);

            // 2020.11.17 by yjlee [ADD] Convert the bit state to the app.
            for (int nAxis = 0; nAxis < nCountOfAxis; ++nAxis)
            {
                int nControllerState = arState[nAxis];

                int nAppState   = ((~nControllerState >> (int)RSAMOTOR_STATE.MOTOR_OFF) & 1) << (int)MotionInstance::Motion_.MOTOR_STATE.MOTOR_ON;
                nAppState       |= ((nControllerState >> (int)RSAMOTOR_STATE.MOTOR_ALARM) & 1) << (int)MotionInstance::Motion_.MOTOR_STATE.ALARM;
                nAppState       |= ((nControllerState >> (int)RSAMOTOR_STATE.LIMIT_POSITIVE) & 1) << (int)MotionInstance::Motion_.MOTOR_STATE.LIMIT_POSITIVE;
                nAppState       |= ((nControllerState >> (int)RSAMOTOR_STATE.LIMIT_NEGATIVE) & 1) << (int)MotionInstance::Motion_.MOTOR_STATE.LIMIT_NEGATIVE;
                nAppState       |= ((nControllerState >> (int)RSAMOTOR_STATE.SWLIMIT_POSITIVE) & 1) << (int)MotionInstance::Motion_.MOTOR_STATE.SWLIMIT_POSITIVE;
                nAppState       |= ((nControllerState >> (int)RSAMOTOR_STATE.SWLIMIT_NEGATIVE) & 1) << (int)MotionInstance::Motion_.MOTOR_STATE.SWLIMIT_NEGATIVE;
                nAppState       |= ((nControllerState >> (int)RSAMOTOR_STATE.HOME_SWITCH) & 1) << (int)MotionInstance::Motion_.MOTOR_STATE.HOME_SWITCH;

                arState[nAxis] = nAppState;
            }
        }

        /// <summary>
        /// 2020.11.09 by yjlee [ADD] Check the motion is done or not.
        /// </summary>
        public override bool IsMotionDone(ref int nAxis)
        {
            return m_instanceSDK.IsMotionDone(ref nAxis);
        }

        /// <summary>
        /// 2020.11.09 by yjlee [ADD] Check the motion's homing is done or not.
        /// </summary>
        public override bool IsHomeDone(ref int nAxis)
        {
            return m_instanceSDK.IsHomeDone(ref nAxis);
        }

        /// <summary>
        /// 2021.04.29 by yjlee [ADD] Check whether the motions are done or not.
        /// </summary>
        public override bool IsMultiMotionDone(ref int nCountOfAxis, ref int[] arAxes)
        {
            return true;
        }

        /// <summary>
        /// 2021.04.29 by yjlee [ADD] Check whether the homings are done or not.
        /// </summary>
        public override bool IsMultiHomeDone(ref int nCountOfAxis, ref int[] arAxes)
        {
            return true;
        }

        /// <summary>
        /// 2020.11.09 by yjlee [ADD] Check the motor's power is on.
        /// </summary>
        public override bool IsMotorOn(ref int nAxis)
        {
            return m_instanceSDK.IsMotorOn(ref nAxis);
        }

        /// <summary>
        /// 2020.11.09 by yjlee [ADD] Do make the axis's power be on or off.
        /// </summary>
        public override void DoMotorOn(ref int nAxis, ref bool bON)
        {
            m_instanceSDK.DoMotorOn(ref nAxis, ref bON);
        }

        /// <summary>
        /// 2020.11.09 by yjlee [ADD] Reset the axis's state.
        /// </summary>
        public override void DoReset(ref int nAxis)
        {
            m_instanceSDK.DoReset(ref nAxis);
        }

        /// <summary>
        /// 2020.11.09 by yjlee [ADD] Set the actual position of the axis.
        /// </summary>
        public override void SetActualPosition(ref int nAxis, ref double dblPosition)
        {
            m_instanceSDK.SetActualPosition(ref nAxis, ref dblPosition);
        }

        /// <summary>
        /// 2020.11.09 by yjlee [ADD] Set the command position of the axis.
        /// </summary>
        public override void SetCommandPosition(ref int nAxis, ref double dblPosition)
        {
            m_instanceSDK.SetCommandPosition(ref nAxis, ref dblPosition);
        }

        /// <summary>
        /// 2021.02.04 by yjlee [ADD] Get the absolute position.
        /// </summary>
        public override void GetAbsolutePosition(ref int nAxis, ref double dblPosition)
        {
        }
        #endregion

        #region Speed Interface
        /// <summary>
        /// 2020.11.09 by yjlee [ADD] Set the motor configuration for the speed.
        /// </summary>
        public override void SetMotorSpeedConfiguration(ref int nAxis, ref PARAM_SPEED paramSpeed)
        {
            RSAPARAM_SPEED rsaParam     = new RSAPARAM_SPEED();

            CopyParameter(ref rsaParam, ref paramSpeed);

            m_instanceSDK.SetMotorSpeedConfiguration(ref nAxis, ref rsaParam);
        }

        /// <summary>
        /// 2020.11.09 by yjlee [ADD] Set the motor configuration for the home's speed.
        /// </summary>
        public override void SetMotorHomeSpeedConfiguration(ref int nAxis, ref PARAM_HOME parapHome)
        {
            RSAPARAM_HOME rsaParam      = new RSAPARAM_HOME();

            CopyParameter(ref rsaParam, ref parapHome);

            m_instanceSDK.SetMotorHomeSpeedConfiguration(ref nAxis, ref rsaParam);
        }
        #endregion

        #region Move Interface
        /// <summary>
        /// 2020.11.09 by yjlee [ADD] Stop the home motion.
        /// </summary>
        public override void StopHomeMotion(ref int nAxis, ref bool bEmergency)
        {
            m_instanceSDK.StopHomeMotion(ref nAxis, ref bEmergency);
        }

        /// <summary>
        /// 2020.11.09 by yjlee [ADD] Move to the home position.
        /// </summary>
        public override bool MoveToHome(ref int nAxis, ref int nSeqNum, ref int nDelay)
        {
            return m_instanceSDK.MoveToHome(ref nAxis, ref nSeqNum, ref nDelay);
        }

        /// <summary>
        /// 2021.04.29 by yjlee [ADD] Stop the multi axes's home motion.
        /// </summary>
        public override void StopMultiHomeMotion(ref int nCountOfAxis, ref int[] arAxes, ref bool bEmergency)
        {
        }

        /// <summary>
        /// 2021.04.29 by yjlee [ADD] Move the multi axes to the home positions.
        /// </summary>
        public override bool MoveMultiToHome(ref int nCountOfAxis, ref int[] arAxes, ref int nSeqNum, ref int nDelay)
        {
            return true;
        }

        /// <summary>
        /// 2020.11.09 by yjlee [ADD] Stop the motion.
        /// </summary>
        public override void StopMotion(ref int nAxis, ref bool bEmergency)
        {
            m_instanceSDK.StopMotion(ref nAxis, ref bEmergency);
        }

        /// <summary>
        /// 2021.04.29 by yjlee [ADD] Stop the multi motion.
        /// </summary>
        public override void StopMultiMotion(ref int nCountOfAxis, ref int[] arAxes, ref bool bEmergency)
        {
        }

        /// <summary>
        /// 2020.11.09 by yjlee [ADD] Move an axis to the position absolutely.
        /// </summary>
        public override void MoveAbsolutely(ref int nAxis, ref double dblDestination)
        {
            m_instanceSDK.MoveAbsolutely(ref nAxis, ref dblDestination);
        }

        /// <summary>
        /// 2021.04.29 by yjlee [ADD] Move the axes to the positions absolutely.
        /// </summary>
        public override void MoveMultiAbsolutely(ref int nCountOfAxis, ref int[] arAxes, ref double[] arDestination)
        {
        }

        /// <summary>
        /// 2020.11.09 by yjlee [ADD] Move an axis to the position releatively.
        /// </summary>
        public override void MoveReleatively(ref int nAxis, ref double dblDestination)
        {
            m_instanceSDK.MoveReleatively(ref nAxis, ref dblDestination);
        }

        /// <summary>
        /// 2021.04.29 by yjlee [ADD] Move the axes to the positions releatively.
        /// </summary>
        public override void MoveMultiReleatively(ref int nCountOfAxis, ref int[] arAxes, ref double[] arDestination)
        {
        }

        /// <summary>
        /// 2020.11.09 by yjlee [ADD] Move an axis at the speed.
        /// </summary>
        public override void MoveAtSpeed(ref int nAxis, ref bool bDirection)
        {
            m_instanceSDK.MoveAtSpeed(ref nAxis, ref bDirection);
        }

        /// <summary>
        /// 2021.04.29 by yjlee [ADD] Move the axes to the positions at the speed.
        /// </summary>
        public override void MoveMultiAtSpeed(ref int nCountOfAxis, ref int[] arAxes, ref bool[] arDirection)
        {
        }

        /// <summary>
        /// 2020.11.09 by yjlee [ADD] Override the speed on moving motion.
        /// </summary>
        public override void OverrideMotion(ref int nAxis, ref EN_OVERRIDE_TYPE enTypeOfOverride, ref double dblDestination, ref PARAM_SPEED paramSpeed)
        {
            RSAPARAM_SPEED rsaParam     = new RSAPARAM_SPEED();

            CopyParameter(ref rsaParam, ref paramSpeed);

            m_instanceSDK.OverrideSpeed(ref nAxis, ref rsaParam);
        }

        /// <summary>
        /// 2020.11.09 by yjlee [ADD] Move the motion by the list steps.
        /// </summary>
        public override void MoveByList(ref int nAxis, ref int nCount, ref double[] arDestination, ref PARAM_SPEED[] arSpeedParam)
        {
            RSAPARAM_SPEED[] arParam        = new RSAPARAM_SPEED[nCount];

            for(int nIndex = 0 ; nIndex < nCount ; ++ nIndex)
            {
                arParam[nIndex]             = new RSAPARAM_SPEED();

                CopyParameter(ref arParam[nIndex], ref arSpeedParam[nIndex]);
            }

            m_instanceSDK.MoveByList(ref nAxis, ref nCount, ref arDestination, ref arParam);
        }

        /// <summary>
		/// 2022.06.07 by shkim [MOD] 미완성기능보완
        /// 2020.11.09 by yjlee [ADD] Not yet.
        /// </summary>
        public override void MoveByLinearCoordination(ref int nCountOfAxis, ref int[] arIndexes, ref int nCountOfStep, ref double[,] arDestination, ref PARAM_SPEED[,] arSpeedParam)
        {
            RSAPARAM_SPEED[] arParam = new RSAPARAM_SPEED[nCountOfStep];

            for(int nStep = 0; nStep < nCountOfStep; ++nStep)
            {
                arParam[nStep] = new RSAPARAM_SPEED();
                CopyParameter(ref arParam[nStep], ref arSpeedParam[nStep, 0]);
            }
            m_instanceSDK.MoveByLinearCoordination(ref nCountOfAxis, ref arIndexes, ref nCountOfStep, ref arDestination, ref arParam);
        }
        #endregion

        #region Network Interface
        /// <summary>
        /// 2020.11.09 by yjlee [ADD] Connect the link on the network for the motion.
        /// </summary>
        public override bool ConnectLink()
        {
            return m_instanceSDK.ConnectLink();
        }

        /// <summary>
        /// 2020.11.09 by yjlee [ADD] Check the link is connected without the problems.
        /// </summary>
        public override bool IsLinkConnected()
        {
            return m_instanceSDK.IsLinkConnected();
        }
        #endregion

		public override bool MoveUntilTouch(ref int nAxis, ref int nSeqNum, ref double dblDestination, ref int nEncoderAxis, ref double dblEncoderThreshold, ref bool bTouched, ref double dblTouchedPosition, ref double dblTochedEncoderPosition)
		{
			return false;
		}

		// 2022.06.07. by shkim. [MOD] LinearCoordinate를 위해 그룹화 되었던 축들을 해제한다.
        public override bool ReleaseMotionGroup(ref int[] arAxisIndexes)
        {
            int nIndexOfFirstAxis = arAxisIndexes[0];
            m_instanceSDK.UngroupAxisByIndexOfFirstIndex(ref nIndexOfFirstAxis);
            return true;
        }

        public override bool MakeDumpFile(ref int nAxis, string sFileName)
        {
            return true;
        }
    }
}
