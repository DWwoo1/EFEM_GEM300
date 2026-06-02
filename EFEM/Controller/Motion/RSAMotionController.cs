using System;
using System.Collections.Generic;
using System.Collections.Concurrent;
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
        ConcurrentDictionary<int, bool> m_dicIsZPhaseTouchProbe = null;
        ConcurrentDictionary<int, DelegateChangeMotorBrakeState> m_dicFuncChangeMotorBrakeState = null;
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
        /// 2022.08.17. by shkim [MOD] 홈 로직 추가
        /// 2020.11.09 by yjlee [ADD] Copy the parameter value from the app to the controller.
        /// </summary>
        private void CopyParameter(ref RSAPARAM_HOME rsaParam, ref PARAM_HOME paramHome, int axisNumber)
        {
            switch (paramHome.enHomeMode)
            {
                case HOME_MODE.ORG_ONESTEP:
                case HOME_MODE.ORG_TWOSTEP:
                    rsaParam.enHomeType = HOME_TYPE.ABS_SWITCH;
                    break;

                case HOME_MODE.EZ_ONESTEP:
                    rsaParam.enHomeType = (false == IsZPhaseTouchProbe(axisNumber))
                        ?  HOME_TYPE.REF_PULSE : HOME_TYPE.TOUCH_PROBE_REF_PULSE;
                    break;

                case HOME_MODE.ORG_EZ_ONESTEP_INSTANTSTOP:
                case HOME_MODE.ORG_EZ_ONESTEP_STOP:
                case HOME_MODE.ORG_EZ_TWOSTEP_INSTANTSTOP:
                case HOME_MODE.ORG_EZ_TWOSTEP_STOP:
                    rsaParam.enHomeType = (false == IsZPhaseTouchProbe(axisNumber))
                        ? HOME_TYPE.HOME_REF_PULSE : HOME_TYPE.HOME_TOUCH_PROBE_REF_PULSE;
                    break;

                case HOME_MODE.EL_ONESTEP:
                case HOME_MODE.EL_TWOSTEP:
                    rsaParam.enHomeType = HOME_TYPE.LIMIT_SWITCH;
                    break;

                case HOME_MODE.EL_EZ_TWOSTEP_STOP:
                case HOME_MODE.EL_EZ_TWOSTEP_INSTANTSTOP:
                    rsaParam.enHomeType = (false == IsZPhaseTouchProbe(axisNumber))
                        ? HOME_TYPE.LIMIT_REF_PULSE : HOME_TYPE.LIMIT_TOUCH_PROBE_REF_PULSE;
                    break;

                case HOME_MODE.FIXED:
                case HOME_MODE.ABSOLUTE_ONLY:
                    rsaParam.enHomeType = HOME_TYPE.FIXED;
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

        /// <summary>
        /// 2022.08.17. by shkim [ADD] Z상을 Touch Probe로 확인하는 축인지 확인
        /// </summary>
        bool IsZPhaseTouchProbe(int nAxis)
        {
            bool isZPhaseTouchProbe = false;
            if (false == m_dicIsZPhaseTouchProbe.TryGetValue(nAxis, out isZPhaseTouchProbe))
            {
                return false;
            }
            return isZPhaseTouchProbe;
        }

        /// <summary>
        /// 2022.08.17. by shkim [ADD] Motor의 Brake 상태를 Digital IO 등으로 직접 해제하여야할 때 사용한다. (사전 등록 필요)
        /// </summary>
        void ChangeMotorBrakeStateByExternalFunction(int nAxis, bool bTrigger)
        {
            DelegateChangeMotorBrakeState externalFunction;
            if (m_dicFuncChangeMotorBrakeState == null || 
                false == m_dicFuncChangeMotorBrakeState.TryGetValue(nAxis, out externalFunction)
                || externalFunction == null)
            {
                return;
            }
            else
            {
                externalFunction(bTrigger);
            }
            
        }

        #endregion

        #region Controller
        /// <summary>
        /// 2022.08.17 by shkim [MOD] Touch Probe를 사용하여 Z상을 확인하는 경우를 위해 설정 파일 읽기 추가
        /// 2020.11.09 by yjlee [ADD] Initialize the controller.
        /// </summary>
        public override bool InitController()
        {
            m_instanceSDK       = RSAMotion.GetInstance();
            m_dicIsZPhaseTouchProbe = new ConcurrentDictionary<int, bool>();

            bool isSuccessInitController = m_instanceSDK.InitController();

            const string DIRECTORY_PATH	= "RSAConfig\\";
            const string AXIS_ADDITIONAL_INFORMATION_FILENAME = "RSAAxisAdditionalInformation.ini";
            
            const string INI_SECTION_AXIS_ADDITIONAL_INFORMATION = "ADDITIONAL_INFO_AXIS_";
            const string INI_KEY_Z_PHASE_IS_TOUCH_PROBE = "Z_PHASE_TOUCH_PROBE";

            string fullFilePath = System.IO.Path.Combine(DIRECTORY_PATH, AXIS_ADDITIONAL_INFORMATION_FILENAME);

            INIParser_.INIParser iniAxisAdditionalInfo = new INIParser_.INIParser();
            iniAxisAdditionalInfo.OpenOrCreate(fullFilePath);

            if(true == isSuccessInitController)
            {
                int axisCount = m_instanceSDK.GetCountOfAxis();

                for(int i = 0; i < axisCount; i++)
                {
                    string strAxisSection = string.Format("{0}{1}", INI_SECTION_AXIS_ADDITIONAL_INFORMATION, i);
                    int isZPhaseTouchProbe;
                    bool isDataExist = false;

                    isZPhaseTouchProbe = iniAxisAdditionalInfo.GetValue(strAxisSection, INI_KEY_Z_PHASE_IS_TOUCH_PROBE, 0);
                    isDataExist |= (isZPhaseTouchProbe != 0);
                    m_dicIsZPhaseTouchProbe.TryAdd(i, (isZPhaseTouchProbe == 1));
                    iniAxisAdditionalInfo.SetValue(strAxisSection, INI_KEY_Z_PHASE_IS_TOUCH_PROBE, isZPhaseTouchProbe);
                }
            }
            return isSuccessInitController;
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

            CopyParameter(ref rsaParam, ref paramHome, nAxis);

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
            if(bON)
            {
                m_instanceSDK.DoMotorOn(ref nAxis, ref bON);
                ChangeMotorBrakeStateByExternalFunction(nAxis, bON);
            }
            else
            {
                ChangeMotorBrakeStateByExternalFunction(nAxis, bON);
                m_instanceSDK.DoMotorOn(ref nAxis, ref bON);
            }
            // m_instanceSDK.DoMotorOn(ref nAxis, ref bON);
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
        /// 2025.03.18 by shkim [MOD] 구현 전인 코드라 예외발생 구문 추가
        /// 2021.02.04 by yjlee [ADD] Get the absolute position.
        /// </summary>
        public override void GetAbsolutePosition(ref int nAxis, ref double dblPosition)
        {
            throw new NotImplementedException("GetAbsolutePosition method is not implemented yet.");
        }

        public override void GetCommandVelocity(ref int nAxis, ref double dblCommandVelocity)
        {
            m_instanceSDK.GetCommandVelocity(ref nAxis, ref dblCommandVelocity);
        }

        public override void GetActualVelocity(ref int nAxis, ref double dblActualVelocity)
        {
            m_instanceSDK.GetActualVelocity(ref nAxis, ref dblActualVelocity);
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

            CopyParameter(ref rsaParam, ref parapHome, nAxis);

            m_instanceSDK.SetMotorHomeSpeedConfiguration(ref nAxis, ref rsaParam);
        }
        #endregion

        #region Gain Table
        public override bool GetGainTable(ref int nAxis, ref int nIndexOfTable)
        {
          //  nIndexOfTable = MPISDK_.Motion_.MPIMotion.GetInstance().GetGainTable(ref nAxis);

            return nIndexOfTable != -1;
		}
		public override void SetGainTable(ref int nAxis, ref int nIndexOfTable)
		{
			//MPISDK_.Motion_.MPIMotion.GetInstance().SetGainTable(ref nAxis, ref nIndexOfTable);
		}
		public override void SetGainType(ref int nAxis, ref int nIndexOfTable)
		{
			//MPISDK_.Motion_.MPIMotion.GetInstance().SetGainType(ref nAxis, ref nIndexOfTable);
		}
        public override double GetOutputOffset(ref int nIndexOfItem)
        {
            return 0; //MPISDK_.Motion_.MPIMotion.GetInstance().GetOutputOffset(ref nIndexOfItem);
        }
        public override void SetOutputOffset(ref int nIndexOfItem, ref double dblOffsetValue)
        {
            //MPISDK_.Motion_.MPIMotion.GetInstance().SetOutputOffset(ref nIndexOfItem, ref dblOffsetValue);
        }
        public override void SetOutputLimit(ref int nIndexOfItem, ref double dblHighLimit, ref double dblLowLimit)
        {
            //MPISDK_.Motion_.MPIMotion.GetInstance().SetOutputLimit(ref nIndexOfItem, ref dblHighLimit, ref dblLowLimit);
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

        public override void MoveSyncVectorMotion(ref int nCountOfAxis, ref int[] arIndexes, ref double[] arDestination, ref PARAM_SPEED arSpeedParam, ref bool bAbsolute, ref bool bOverride)
        {
        }
        public override void MoveSyncVectorMotionList(ref int nCountOfAxis, ref int[] arIndexes, ref double[,] arDestination, ref PARAM_SPEED[] arSpeedParam, ref bool bAbsolute)
        {
        }
        public override void MoveSyncVectorMotionCompare(ref int nCountOfAxis, ref int[] arIndexes, ref double[] arDestination, ref PARAM_SPEED SpeedParam, ref bool bAbsolute, ref int nCompareMotion, ref double dComparePostion, ref bool bLogic, ref bool bActual)
        {
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

        public override bool MoveUntilTouch_Analog(ref int nAxis, ref int nSeqNum, ref double dblDestination, ref int[] arIndexAnalog, ref double dblAnalogThreshold, ref bool bTouched, ref double dblTouchedPosition, ref double dblTouchedAnalogValue)
        {
            return false;
        }
        public override bool MoveByListUntilTouch(ref int nAxis, ref int nSeqNum, ref int nCount, ref double[] arDestination, ref PARAM_SPEED[] arSpeedParam, ref int nEncoderAxis, ref double dblEncoderThreshold, ref bool bTouched, ref double dblTouchedPosition, ref double dblTouchedEncoderPosition)
        {
            return false;
        }

        public override bool MoveByListUntilTouch_Analog(ref int nAxis, ref int nSeqNum, ref int nCount, ref double[] arDestination, ref PARAM_SPEED[] arSpeedParam, ref int[] arIndexAnalog, ref double dblAnalogThreshold, ref bool bTouched, ref double dblTouchedPosition, ref double dblTouchedAnalogValue)
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
			// 2024.03.27 by junho [ADD]
			ushort errorId = 0;
			ushort errorInfo = 0;
			ushort errorInfoExt = 0;
			m_instanceSDK.GetReadAxisError(ref nAxis, ref errorId, ref errorInfo, ref errorInfoExt);
			Console.WriteLine(string.Format("[RSA read axis error result] id:{0}, info:{1}, infoExt:{2}"
				, errorId, errorInfo, errorInfoExt));
			return true;
        }

        /// <summary>
        /// 2022.08.17. by shkim [ADD] Motor의 Brake 상태를 Digital IO 등으로 직접 해제하여야할 때 사전 등록하기 위해 사용한다.
        /// </summary>
        public override void SetCallbackChangeMotorBrakeState(int nAxis, DelegateChangeMotorBrakeState pFuncSetMotorBrakeState)
        {
            if(m_dicFuncChangeMotorBrakeState == null)
            {
                m_dicFuncChangeMotorBrakeState = new ConcurrentDictionary<int, DelegateChangeMotorBrakeState>();
            }
            m_dicFuncChangeMotorBrakeState.TryAdd(nAxis, pFuncSetMotorBrakeState);
        }
        public override bool GetCurrentMotorTorqueValue(int nAxis, ref short nCurrentTorque)
        {
            return m_instanceSDK.GetCurrentMotorTorqueValue(nAxis, ref nCurrentTorque);
        }
        public override bool SetChangeTorqueLimitPositionEvent(int nAxis, double dEventPosition, ushort nDefaultTorque, ushort nLimitTorque, double dEventPositionWidth)
        {
            return m_instanceSDK.SetChangeTorqueLimitPositionEvent(nAxis, dEventPosition, nDefaultTorque, nLimitTorque, dEventPositionWidth); ;
        }
    }
}
