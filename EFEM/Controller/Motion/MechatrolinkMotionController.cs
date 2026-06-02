using FrameOfSystem3.Task;
using System.Collections.Concurrent;

using Motion_;

using FrameOfSystem3.Controller.Motion.MechatroLinkAPI;
using FrameOfSystem3.Controller.Motion.MechatroLinkAPI.MechatroLinkDefines;

namespace FrameOfSystem3.Controller.Motion
{
    extern alias MotionInstance;

    /// <summary>
    /// 2021.06.01 by Thienvv [MOD] Motion Controller for MechatroLink
    /// </summary>
    class MechatrolinkMotionController : MotionController
    {
        #region <Fields>
        //private MechatrolinkMotion m_instanceSDK = null;
        ConcurrentDictionary<int, DelegateChangeMotorBrakeState> m_dicFuncChangeMotorBrakeState = null;
        
        //private bool m_bInit = false;

        private MechatrolinkApiWrapper m_instanceWrapper = MechatrolinkApiWrapper.Instance;
        #endregion </Fields>

        #region <Methods>
        #region <Controller>
        public override bool InitController()
        {
            return m_instanceWrapper.Init();
        }

        public override void ExitController()
        {
            m_instanceWrapper.Exit();
        }

        public override int GetCountOfAxis()
        {
            return m_instanceWrapper.AxisCount;
        }
        #endregion </Controller>

        #region <Configuration Coversion>
        private void CopyParameter(ref PARAM_CONFIG inParamConfig, ref MCTL_PARAM_CONFIG mctlParam)
        {
            // enMotorInput
            //switch (inParamConfig.enMotorInput)
            //{
            //    case MOTOR_INPUTMODE.AB1X:
            //        mctlParam.enInMode = MCTL_MOTOR_INPUTMODE.AB1X;
            //        break;
            //    case MOTOR_INPUTMODE.AB2X:
            //        mctlParam.enInMode = MCTL_MOTOR_INPUTMODE.AB2X;
            //        break;
            //    case MOTOR_INPUTMODE.AB4X:
            //        mctlParam.enInMode = MCTL_MOTOR_INPUTMODE.AB4X;
            //        break;
            //    case MOTOR_INPUTMODE.CWCCW:
            //        mctlParam.enInMode = MCTL_MOTOR_INPUTMODE.CWCCW;
            //        break;
            //    case MOTOR_INPUTMODE.STEP:
            //        mctlParam.enInMode = MCTL_MOTOR_INPUTMODE.STEP;
            //        break;
            //    default:
            //        break;
            //}

            // enMotorOutput
            //switch (inParamConfig.enMotorOutput)
            //{
            //    case MOTOR_OUTPUTMODE.ABPHASE:
            //        mctlParam.enOutMode = MCTL_MOTOR_OUTPUTMODE.ABPHASE_CWCCW;
            //        break;
            //    case MOTOR_OUTPUTMODE.CWCCW_HIGH:
            //        mctlParam.enOutMode = MCTL_MOTOR_OUTPUTMODE.CWCCW_HIGH;
            //        break;
            //    case MOTOR_OUTPUTMODE.CWCCW_LOW:
            //        mctlParam.enOutMode = MCTL_MOTOR_OUTPUTMODE.CWCCW_LOW;
            //        break;
            //    case MOTOR_OUTPUTMODE.CW_PULSE:
            //        mctlParam.enOutMode = MCTL_MOTOR_OUTPUTMODE.CW_HIGH;
            //        break;
            //    case MOTOR_OUTPUTMODE.CW_PULSE_REVERSE:
            //        mctlParam.enOutMode = MCTL_MOTOR_OUTPUTMODE.CW_LOW;
            //        break;
            //    default:
            //        break;
            //}

            //mctlParam.dblInOutRatio = 1;
            //mctlParam.dblUnitDistance = 1.0;
            mctlParam.bAlarmLogic = inParamConfig.bAlarmLogic;
            mctlParam.bAlarmStopMode = inParamConfig.bAlarmStopMode;

            //mctlParam.bEZLogic = inParamConfig.bZPhaseLogic;

            //mctlParam.bUseInposition = false;
            //mctlParam.bInpositionLogic = false;

            //mctlParam.enMotorApplicationType = inParamConfig.enMotionType;

            //mctlParam.bCommandDirection = inParamConfig.bEncoderDirection;
            //mctlParam.bEncoderDirection = inParamConfig.bCommandDirection;

            mctlParam.bUseHWLimitPositive = inParamConfig.bUseHWLimitPositive;
            mctlParam.bUseHWLimitNegative = inParamConfig.bUseHWLimitNegative;
            mctlParam.bHWLimitLogicPositive = inParamConfig.bHWLimitLogicPositive;
            mctlParam.bHWLimitLogicNegative = inParamConfig.bHWLimitLogicNegative;
            mctlParam.bHWLimitStopModePositive = inParamConfig.bHWLimitStopModePositive;
            mctlParam.bHWLimitStopModeNegative = inParamConfig.bHWLimitStopModeNegative;

            mctlParam.bUseSWLimitPositive = inParamConfig.bUseSWLimitPositive;
            mctlParam.bUseSWLimitNegative = inParamConfig.bUseSWLimitNegative;
            mctlParam.bSWLimitStopModePositive = inParamConfig.bSWLimitStopModePositive;
            mctlParam.bSWLimitStopModeNegative = inParamConfig.bSWLimitStopModeNegative;
            mctlParam.dblSWLimitPositionPositive = inParamConfig.dblSWLimitPositionPositive;
            mctlParam.dblSWLimitPositionNegative = inParamConfig.dblSWLimitPositionNegative;
        }

        private void CopyParameter(ref PARAM_HOME inParamHome, ref MCTL_PARAM_HOME mctlParam)
        {
            //mctlParam.bHomeLogic = inParamHome.bHomeLogic;                  // 홈의 로직을 설정, FALSE : A(NO), TRUE : B(NC)
            mctlParam.bHomeDirection = inParamHome.bHomeDirection;              // 홈의 방향, FALSE : - , TRUE : +

            mctlParam.dblHomeVelocity = inParamHome.dblHomeVelocityStart;
            mctlParam.dblHomeAccelTime = inParamHome.dblHomeAccelTime;
            mctlParam.dblHomeDecelTime = inParamHome.dblHomeDecelTime;
            mctlParam.dblHomeAcceleration = inParamHome.dblHomeAcceleration;
            mctlParam.dblHomeDeceleration = inParamHome.dblHomeDeceleration;

            //mctlParam.bAlwaysHomeEnd = inParamHome.bAlwaysHomeEnd;
            switch (inParamHome.enHomeMode)
            {
                case HOME_MODE.ABSOLUTE_ONLY:
                    //mctlParam.bAlwaysHomeEnd = true;
                    break;

                case HOME_MODE.EL_EZ_TWOSTEP_INSTANTSTOP:
                    if (inParamHome.bHomeDirection == true)
                        mctlParam.enHomeMode = MCTL_HOME_TYPE.HMETHOD_POT_C;
                    else
                        mctlParam.enHomeMode = MCTL_HOME_TYPE.HMETHOD_NOT_C;
                    break;

                case HOME_MODE.EL_EZ_TWOSTEP_STOP:
                    if (inParamHome.bHomeDirection == true)
                        mctlParam.enHomeMode = MCTL_HOME_TYPE.HMETHOD_POT_C2;
                    else
                        mctlParam.enHomeMode = MCTL_HOME_TYPE.HMETHOD_NOT_C2;
                    break;

                case HOME_MODE.EL_ONESTEP:
                    if (inParamHome.bHomeDirection == true)
                        mctlParam.enHomeMode = MCTL_HOME_TYPE.HMETHOD_POT_ONLY;
                    else
                        mctlParam.enHomeMode = MCTL_HOME_TYPE.HMETHOD_NOT_ONLY;
                    break;

                case HOME_MODE.EL_TWOSTEP:
                    if (inParamHome.bHomeDirection == true)
                        mctlParam.enHomeMode = MCTL_HOME_TYPE.HMETHOD_POT_ONLY;
                    else
                        mctlParam.enHomeMode = MCTL_HOME_TYPE.HMETHOD_NOT_ONLY;
                    break;

                case HOME_MODE.EZ_ONESTEP:
                    mctlParam.enHomeMode = MCTL_HOME_TYPE.HMETHOD_C_ONLY;
                    break;

                case HOME_MODE.FIXED:
                    //mctlParam.bAlwaysHomeEnd = true;
                    break;

                case HOME_MODE.ORG_EZ_ONESTEP_INSTANTSTOP:
                    mctlParam.enHomeMode = MCTL_HOME_TYPE.HMETHOD_HOMELS_C;
                    break;

                case HOME_MODE.ORG_EZ_ONESTEP_STOP:
                    mctlParam.enHomeMode = MCTL_HOME_TYPE.HMETHOD_HOMELS_C;
                    break;

                case HOME_MODE.ORG_EZ_TWOSTEP_INSTANTSTOP:
                    mctlParam.enHomeMode = MCTL_HOME_TYPE.HMETHOD_HOMELS_C;
                    break;

                case HOME_MODE.ORG_EZ_TWOSTEP_STOP:
                    mctlParam.enHomeMode = MCTL_HOME_TYPE.HMETHOD_HOMELS_C;
                    break;

                case HOME_MODE.ORG_ONESTEP:
                    mctlParam.enHomeMode = MCTL_HOME_TYPE.HMETHOD_HOMELS_ONLY;
                    break;

                case HOME_MODE.ORG_TWOSTEP:
                    mctlParam.enHomeMode = MCTL_HOME_TYPE.HMETHOD_HOMELS_ONLY;
                    break;

                default:
                    break;
            }
            //mctlParam.nEZCount = inParamHome.nEZCount;
            mctlParam.dblEscDist = inParamHome.dblHomeEscapeDist;                // 원점 탈출 거리
            mctlParam.dblHomeOffset = inParamHome.dblHomeOffset;                    // 원점 복귀 후 Offset 값
            mctlParam.dblHomeBasePosition = inParamHome.dblHomeBasePosition;        // Homing 후 지정할 포지션

            mctlParam.enHomeSpeedPattern = GetPattern(inParamHome.enHomeSpeedPattern);     // 홈 속도 모드
            mctlParam.dblHomeSecondVelocity = inParamHome.dblHomeVelocityLast;              // 홈 역방향 속도
            //mctlParam.dblHomeJerkAccelration = inParamHome.dblHomeAccelJerk;            // 모터 가속 Jerk
            //mctlParam.dblHomeJerkDecelration = inParamHome.dblHomeDecelJerk;            // 모터 감속 Jerk
            mctlParam.uHomeTimeout = (uint)inParamHome.nHomeTimeout;
        }

        private void CopyParameter(ref PARAM_GANTRY inParamGantry, ref MCTL_PARAM_GANTRY mctlParam)
        {
            mctlParam.bEnableGantry = inParamGantry.bEnableGantry;
            mctlParam.bReverseSlaveDirection = inParamGantry.bReverseSlaveDirection;
            mctlParam.nAxisOfMaster = inParamGantry.nIndexOfMaster;
            mctlParam.nAxisOfSlave = inParamGantry.nIndexOfSlave;
        }

        private void CopyParameter(ref PARAM_SPEED inParamSpeed, ref MCTL_PARAM_SPEED mctlParam)
        {
            mctlParam.dblVelocity = inParamSpeed.dblVelocity;       // 모터의 속도(mm/s)
            mctlParam.dblAccelTime = inParamSpeed.dblAccelTime;      // 모터 가속 시간(ms)
            mctlParam.dblDecelTime = inParamSpeed.dblDecelTime;      // 모터 감속 시간(ms)
            //mctlParam.dblAcceleration = inParamSpeed.dblAcceleration;   // 모터 가속도
            //mctlParam.dblDeceleration = inParamSpeed.dblDeceleration;   // 모터 감속도

            mctlParam.enSpeedPattern = GetPattern(inParamSpeed.enSpeedPattern);  // 속도 모드
            mctlParam.dblMaxVelocity = inParamSpeed.dblMaxVelocity;                 // 모터 속도
            //mctlParam.dblJerkAccelration = inParamSpeed.dblAccelJerk;           // 모터 가속 Jerk
            //mctlParam.dblJerkDecelration = inParamSpeed.dblDecelJerk;           // 모터 감속 Jerk
            mctlParam.uMotionTimeout = (uint)inParamSpeed.nMotionTimeout;
            mctlParam.uFilter = 10; //Mechatrolink 0.1ms
        }

        private MECHATROLINK_SPEED_PATTERN GetPattern(Motion_.MOTION_SPEED_PATTERN enPattern)
        {
            switch(enPattern)
            {
                case MOTION_SPEED_PATTERN.TRAPEZODIAL:
                    return MECHATROLINK_SPEED_PATTERN.TRAPEZODIAL;
                default:
                    return MECHATROLINK_SPEED_PATTERN.S_CURVE;
            }
        }
        #endregion </Configuration Coversion>

        #region <Motor Configuration>
        public override void SetMotorConfiguration(ref int nAxis, ref PARAM_CONFIG paramConfig)
        {
            // 2021.07.11 by Thienvv [ADD] FROM_HERE need ask framework why call SetMotorHomeConfiguration in gathering
            //string strCallMethod = (new System.Diagnostics.StackTrace()).GetFrame(1).GetMethod().Name; // == "SetMotorParameter"
            //string strCallMethod = (new System.Diagnostics.StackTrace()).GetFrame(2).GetMethod().Name; // == "ExecuteForGathering"
            //if (strCallMethod.Equals("ExecuteForGathering"))
            //{
            //    return;
            //}

            MCTL_PARAM_CONFIG mctlParam = new MCTL_PARAM_CONFIG();

            CopyParameter(ref paramConfig, ref mctlParam);

            m_instanceWrapper.SetMotorConfiguration(nAxis, mctlParam);
        }

        public override void SetMotorHomeConfiguration(ref int nAxis, ref PARAM_HOME paramHome)
        {
            // 2021.07.11 by Thienvv [ADD] FROM_HERE need ask framework why call SetMotorHomeConfiguration in gathering
            //string strCallMethod = (new System.Diagnostics.StackTrace()).GetFrame(1).GetMethod().Name; // == "SetMotorParameter"
            //string strCallMethod = (new System.Diagnostics.StackTrace()).GetFrame(2).GetMethod().Name; // == "ExecuteForGathering"
            //if (strCallMethod.Equals("ExecuteForGathering"))
            //{
            //    return;
            //}

            MCTL_PARAM_HOME mctlParam = new MCTL_PARAM_HOME();

            CopyParameter(ref paramHome, ref mctlParam);

            m_instanceWrapper.SetMotorHomeSpeedConfiguration(nAxis, mctlParam);
        }

        public override void SetGantry(ref int nAxis, ref PARAM_GANTRY paramGantry, ref PARAM_SPEED paramSpeed)
        {
            if (false == paramGantry.bEnableGantry && (0 > paramGantry.nIndexOfMaster || 0 > paramGantry.nIndexOfSlave))
                return;
            MCTL_PARAM_GANTRY mctlGantryParam = new MCTL_PARAM_GANTRY();
            MCTL_PARAM_SPEED mctlParamSpeed = new MCTL_PARAM_SPEED();

            CopyParameter(ref paramGantry, ref mctlGantryParam);
            CopyParameter(ref paramSpeed, ref mctlParamSpeed);

            m_instanceWrapper.SetGantryParam(mctlGantryParam, mctlParamSpeed);

            if (nAxis == paramGantry.nIndexOfSlave)
                return;

            int[] arAxes = { paramGantry.nIndexOfMaster, paramGantry.nIndexOfSlave };
            m_instanceWrapper.SetGantry(arAxes);
        }

        public override bool GetGantryState(ref int nAxis, ref PARAM_GANTRY paramGantry)
        {
            MCTL_PARAM_GANTRY mctlParam = new MCTL_PARAM_GANTRY();

            CopyParameter(ref paramGantry, ref mctlParam);

            return m_instanceWrapper.GetGantryState(mctlParam);
        }
        #endregion </Motor Configuration>

        #region <Motor Information>
        public override void GetMotorInformationAll(ref int nCountOfAxis, ref int[] arState, ref double[] arActualPosition, ref double[] arCommandPosition)
        {
            //for (int nAxis = 0; nAxis < nCountOfAxis; ++nAxis)
            //{
            //    m_instanceWrapper.UpdateAxisStatus(nAxis);

            //    arState[nAxis] = m_instanceWrapper.GetMotorState(nAxis);
            //    arActualPosition[nAxis] = m_instanceWrapper.GetActualPosition(nAxis);
            //    arCommandPosition[nAxis] = m_instanceWrapper.GetCommandPosition(nAxis);
            //}
            m_instanceWrapper.GetActualPositionAll(ref arActualPosition); //m_instanceSDK.GetActualPosition(ref nAxis);
            m_instanceWrapper.GetCommandPositionAll(ref arCommandPosition);
            m_instanceWrapper.GetMotorStateAll(ref arState);
            
            // 2020.11.17 by yjlee [ADD] Convert the bit state to the app.
            //for (int nAxis = 0; nAxis < nCountOfAxis; ++nAxis)
            {
                // 2021.07.06 by Thienvv [ADD]

                //bool[] bArrState = new bool[(int)MCTL_MOTOR_STATE.MAXCOUNT];

                //int nControllerState = arState[nAxis];

                //int nAppState = (bArrState[(int)MCTL_MOTOR_STATE.MOTOR_ON] ? 1 : 0) << (int)MotionInstance::Motion_.MOTOR_STATE.MOTOR_ON;
                //nAppState |= (bArrState[(int)MCTL_MOTOR_STATE.ALARM] ? 1 : 0) << (int)MotionInstance::Motion_.MOTOR_STATE.ALARM;
                //nAppState |= (bArrState[(int)MCTL_MOTOR_STATE.LIMIT_POSITIVE] ? 1 : 0) << (int)MotionInstance::Motion_.MOTOR_STATE.LIMIT_POSITIVE;
                //nAppState |= (bArrState[(int)MCTL_MOTOR_STATE.LIMIT_NEGATIVE] ? 1 : 0) << (int)MotionInstance::Motion_.MOTOR_STATE.LIMIT_NEGATIVE;
                //nAppState |= (bArrState[(int)MCTL_MOTOR_STATE.SWLIMIT_POSITIVE] ? 1 : 0) << (int)MotionInstance::Motion_.MOTOR_STATE.SWLIMIT_POSITIVE;
                //nAppState |= (bArrState[(int)MCTL_MOTOR_STATE.SWLIMIT_NEGATIVE] ? 1 : 0) << (int)MotionInstance::Motion_.MOTOR_STATE.SWLIMIT_NEGATIVE;
                //nAppState |= (bArrState[(int)MCTL_MOTOR_STATE.HOME_SWITCH] ? 1 : 0) << (int)MotionInstance::Motion_.MOTOR_STATE.HOME_SWITCH;

                ////int nAppState = ((~nControllerState >> (int)MCTL_MOTOR_STATE.MOTOR_ON) & 1) << (int)MotionInstance::Motion_.MOTOR_STATE.MOTOR_ON;
                ////nAppState |= ((nControllerState >> (int)MCTL_MOTOR_STATE.ALARM) & 1) << (int)MotionInstance::Motion_.MOTOR_STATE.ALARM;
                ////nAppState |= ((nControllerState >> (int)MCTL_MOTOR_STATE.LIMIT_POSITIVE) & 1) << (int)MotionInstance::Motion_.MOTOR_STATE.LIMIT_POSITIVE;
                ////nAppState |= ((nControllerState >> (int)MCTL_MOTOR_STATE.LIMIT_NEGATIVE) & 1) << (int)MotionInstance::Motion_.MOTOR_STATE.LIMIT_NEGATIVE;
                ////nAppState |= ((nControllerState >> (int)MCTL_MOTOR_STATE.SWLIMIT_POSITIVE) & 1) << (int)MotionInstance::Motion_.MOTOR_STATE.SWLIMIT_POSITIVE;
                ////nAppState |= ((nControllerState >> (int)MCTL_MOTOR_STATE.SWLIMIT_NEGATIVE) & 1) << (int)MotionInstance::Motion_.MOTOR_STATE.SWLIMIT_NEGATIVE;
                ////nAppState |= ((nControllerState >> (int)MCTL_MOTOR_STATE.HOME_SWITCH) & 1) << (int)MotionInstance::Motion_.MOTOR_STATE.HOME_SWITCH;

                //arState[nAxis] = nAppState;
            }
        }

        public override bool IsMotionDone(ref int nAxis)
        {
            return m_instanceWrapper.IsMotionDone(nAxis);
        }

        public bool IsMotionHWLimitP(int nIndex)
        {
            return m_instanceWrapper.IsMotionHWLimitP(nIndex);
        }
        public bool IsMotionHWLimitN(int nIndex)
        {
            return m_instanceWrapper.IsMotionHWLimitN(nIndex);
        }

        public override bool IsHomeDone(ref int nAxis)
        {
            return m_instanceWrapper.IsHomeDone(nAxis);
        }

        /// <summary>
        /// 2021.04.29 by yjlee [ADD] Check whether the motions are done or not.
        /// </summary>
        public override bool IsMultiMotionDone(ref int nCountOfAxis, ref int[] arAxes)
        {
            bool bResult = true;
            for(int i = 0; i < nCountOfAxis; ++i)
            {
                bResult &= m_instanceWrapper.IsMotionDone(i);
            }

            return bResult;
        }

        /// <summary>
        /// 2021.04.29 by yjlee [ADD] Check whether the homings are done or not.
        /// </summary>
        public override bool IsMultiHomeDone(ref int nCountOfAxis, ref int[] arAxes)
        {
            bool bResult = true;
            for (int i = 0; i < nCountOfAxis; ++i)
            {
                bResult &= m_instanceWrapper.IsHomeDone(i);
            }

            return bResult;
        }

        public override bool IsMotorOn(ref int nAxis)
        {
            return m_instanceWrapper.IsMotorOn(nAxis);
        }

        public override void DoMotorOn(ref int nAxis, ref bool bON)
        {
            if (bON)
            {
                m_instanceWrapper.DoMotorOn(nAxis, true);
                ChangeMotorBrakeStateByExternalFunction(nAxis, bON);
            }
            else
            {
                ChangeMotorBrakeStateByExternalFunction(nAxis, bON);
                m_instanceWrapper.DoMotorOn(nAxis, false);
            }
        }

        public override void DoReset(ref int nAxis)
        {
            m_instanceWrapper.DoReset(nAxis);
        }

        public override void SetActualPosition(ref int nAxis, ref double dblPosition)
        {
            m_instanceWrapper.SetPosition(nAxis, dblPosition);
        }

        public override void SetCommandPosition(ref int nAxis, ref double dblPosition)
        {
            //m_instanceWrapper.SetPosition(nAxis, dblPosition);
        }

        public override void GetAbsolutePosition(ref int nAxis, ref double dblPosition)
        {
            dblPosition = m_instanceWrapper.GetActualPosition(nAxis);
        }

        public override bool GetCurrentMotorTorqueValue(int nAxis, ref short dCurrentTorque)
        {
            // 추후 필요시 추가
            dCurrentTorque = 0;

            return false;
        }

        public override bool GetGainTable(ref int nAxis, ref int nIndexOfTable)
        {
            return base.GetGainTable(ref nAxis, ref nIndexOfTable);
        }

        public override bool MakeDumpFile(ref int nAxis, string sFileName)
        {
            return false;
        }

        public override bool ReleaseMotionGroup(ref int[] arAxisIndexes)
        {
            return false;
        }

        public override void SetCallbackChangeMotorBrakeState(int nAxis, DelegateChangeMotorBrakeState pFuncSetMotorBrakeState)
        {
            if (m_dicFuncChangeMotorBrakeState == null)
            {
                m_dicFuncChangeMotorBrakeState = new ConcurrentDictionary<int, DelegateChangeMotorBrakeState>();
            }
            m_dicFuncChangeMotorBrakeState.TryAdd(nAxis, pFuncSetMotorBrakeState);
        }

        public override bool SetChangeTorqueLimitPositionEvent(int nAxis, double dEventPosition, ushort nDefaultTorque, ushort nLimitTorque, double dEventPositionWidth)
        {
            return false;
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
        
        #endregion </Motor Information>

        #region <Speed Interface>
        /// <summary>
        /// 2020.11.09 by yjlee [ADD] Set the motor configuration for the speed.
        /// </summary>
        public override void SetMotorSpeedConfiguration(ref int nAxis, ref PARAM_SPEED paramSpeed)
        {
            MCTL_PARAM_SPEED mctlParam = new MCTL_PARAM_SPEED();

            CopyParameter(ref paramSpeed, ref mctlParam);

            if (nAxis == 26)
            {
                System.Console.WriteLine("--------------------");
                System.Console.WriteLine("Set Speed Axis : {0} -> {1}", nAxis, mctlParam.dblVelocity);
            }
            m_instanceWrapper.SetMotorSpeedConfiguration(nAxis, mctlParam);
        }

        /// <summary>
        /// 2020.11.09 by yjlee [ADD] Set the motor configuration for the home's speed.
        /// </summary>
        public override void SetMotorHomeSpeedConfiguration(ref int nAxis, ref PARAM_HOME paramHome)
        {
            MCTL_PARAM_HOME mctlParam = new MCTL_PARAM_HOME();

            CopyParameter(ref paramHome, ref mctlParam);

            m_instanceWrapper.SetMotorHomeSpeedConfiguration(nAxis, mctlParam);
        }
        #endregion <Speed Interface>

        #region <Move Interface>
        /// <summary>
        /// 2020.11.09 by yjlee [ADD] Stop the home motion.
        /// </summary>
        public override void StopHomeMotion(ref int nAxis, ref bool bEmergency)
        {
            m_instanceWrapper.StopHomeMotion(nAxis, bEmergency);

        }

        /// <summary>
        /// 2020.11.09 by yjlee [ADD] Move to the home position.
        /// </summary>
        public override bool MoveToHome(ref int nAxis, ref int nSeqNum, ref int nDelay)
        {
            return m_instanceWrapper.MoveToHome(nAxis, ref nSeqNum, ref nDelay);
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
            m_instanceWrapper.StopMotion(nAxis, bEmergency);
            //m_instanceSDK.StopMotion(ref nAxis, ref bEmergency);
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
            m_instanceWrapper.MoveMotion(nAxis, dblDestination, true);
        }

        /// <summary>
        /// 2021.04.29 by yjlee [ADD] Move the axes to the positions absolutely.
        /// </summary>
        public override void MoveMultiAbsolutely(ref int nCountOfAxis, ref int[] arAxes, ref double[] arDestination)
        {
			// 추후 필요시 추가
        }


        /// <summary>
        /// 2020.11.09 by yjlee [ADD] Move an axis to the position releatively.
        /// </summary>
        public override void MoveReleatively(ref int nAxis, ref double dblDestination)
        {
            m_instanceWrapper.MoveMotion(nAxis, dblDestination, false);
        }


        /// <summary>
        /// 2021.04.29 by yjlee [ADD] Move the axes to the positions releatively.
        /// </summary>
        public override void MoveMultiReleatively(ref int nCountOfAxis, ref int[] arAxes, ref double[] arDestination)
        {
            // 추후 필요시 추가
        }

        /// <summary>
        /// 2020.11.09 by yjlee [ADD] Move an axis at the speed.
        /// </summary>
        public override void MoveAtSpeed(ref int nAxis, ref bool bDirection)
        {
            m_instanceWrapper.MoveAtSpeed(nAxis, bDirection);
        }

        /// <summary>
        /// 2021.04.29 by yjlee [ADD] Move the axes to the positions at the speed.
        /// </summary>
        public override void MoveMultiAtSpeed(ref int nCountOfAxis, ref int[] arAxes, ref bool[] arDirection)
        {
            // 추후 필요시 추가
        }

        ///// <summary>
        ///// 2020.11.09 by yjlee [ADD] Override the speed on moving motion.
        ///// </summary>
        //public override void OverrideMotion(ref int nAxis, ref EN_OVERRIDE_TYPE paramSpeed)
        //{
        //    MCTL_PARAM_SPEED mctlParam = new MCTL_PARAM_SPEED();

        //    CopyParameter(ref paramSpeed, ref mctlParam);

        //    m_instanceSDK.OverrideSpeed(ref nAxis, ref mctlParam);
        //}

        /// <summary>
        /// 2020.11.09 by yjlee [ADD] Override the speed on moving motion.
        /// </summary>
        public override void OverrideMotion(ref int nAxis, ref EN_OVERRIDE_TYPE enTypeOfOverride,
            ref double dblDestination, ref PARAM_SPEED paramSpeed)
        {
            switch(enTypeOfOverride)
            {
                case EN_OVERRIDE_TYPE.SPEED:
                    {
                        MCTL_PARAM_SPEED mctlParam = new MCTL_PARAM_SPEED();
                        CopyParameter(ref paramSpeed, ref mctlParam);

                        //m_instanceWrapper.
                    }
                    break;
                default:
                    break;
            }
            
            //RSAPARAM_SPEED rsaParam = new RSAPARAM_SPEED();
            //CopyParameter(ref rsaParam, ref paramSpeed);
            //m_instanceSDK.OverrideSpeed(ref nAxis, ref rsaParam);

            //MCTL_PARAM_SPEED mctlParam = new MCTL_PARAM_SPEED();

            //CopyParameter(ref paramSpeed, ref mctlParam);

			// 추후 필요시 추가
        }

        /// <summary>
        /// 2020.11.09 by yjlee [ADD] Move the motion by the list steps.
        /// </summary>
        public override void MoveByList(ref int nAxis, ref int nCount, ref double[] arDestination, ref PARAM_SPEED[] arSpeedParam)
        {
            //MCTL_PARAM_SPEED[] arParam = new MCTL_PARAM_SPEED[nCount];

            //for (int nIndex = 0; nIndex < nCount; ++nIndex)
            //{
            //    arParam[nIndex] = new MCTL_PARAM_SPEED();

            //    CopyParameter(ref arSpeedParam[nIndex], ref arParam[nIndex]);
            //}

            // 추후 필요시 추가
            //m_instanceSDK.MoveByList(ref nAxis, ref nCount, ref arDestination, ref arParam);
        }

        /// <summary>
        /// 2020.11.09 by yjlee [ADD] Not yet.
        /// </summary>
        public override void MoveByLinearCoordination(ref int nCountOfAxis, ref int[] arIndexes, ref int nCountOfStep, ref double[,] arDestination, ref PARAM_SPEED[,] arSpeedParam)
        {
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

        // 2021.07.10 by Thienvv [ADD]
        // 2023.03.21. jhlim [DEL]
        //public bool MoveExternalPositioning(EN_MOTION_LIST enIndex, double dblDestination, PARAM_SPEED paramSpeed, bool bWaitDone)
        //{
        //    MCTL_PARAM_SPEED mctlParam = new MCTL_PARAM_SPEED();
        //    CopyParameter(ref paramSpeed, ref mctlParam);

        //    // 2021.08.16 by Thienvv [ADD] Convert to pulse
        //    //double dblDestinationPulse = ConvertUmToPulse(enIndex, dblDestination);

        //    //int nTargetIndex = GetTargetIndex(enIndex);

        //    return m_instanceSDK.MoveExternalPositioning((int)enIndex, dblDestination, mctlParam, bWaitDone);
        //}

        // 2021.08.16 by Thienvv [ADD] Linear Interpolation for BALL HEAD XY
        //public bool MoveLinearAbsolutely(EN_MOTION_LIST[] enIndexs, double[] arrAbsolutePos, MotionInstance::Motion_.MOTION_SPEED_CONTENT speedContent, double dblVelocity, int uRatio = 100, int uDelay = 0)
        //{
        //    Motion_.Motion motionDll = Motion_.Motion.GetInstance();

        //    if (m_bInit == false) return false; // 장치가 초기화 되지 않았을 경우 리턴

        //    MCTL_PARAM_SPEED[] arrMctlParam = new MCTL_PARAM_SPEED[enIndexs.Length];
        //    int[] arrTargetIndex = new int[enIndexs.Length];
        //    double[] arrAbsolutePosPulse = new double[enIndexs.Length];
        //    for (int i = 0; i < enIndexs.Length; i++)
        //    {
        //        PARAM_SPEED paramSpeed = new PARAM_SPEED();
        //        MCTL_PARAM_SPEED mctlParam = new MCTL_PARAM_SPEED();
        //        GetSpeedConfig(enIndexs[i], speedContent, ref paramSpeed);
        //        CopyParameter(ref paramSpeed, ref mctlParam);

        //        // Convert To pulse value
        //        mctlParam.dblMaxVelocity = ConvertUmToPulse(enIndexs[i], mctlParam.dblMaxVelocity);
        //        mctlParam.dblVelocity = ConvertUmToPulse(enIndexs[i], mctlParam.dblVelocity);

        //        arrMctlParam[i] = mctlParam;

        //        arrTargetIndex[i] = GetTargetIndex(enIndexs[i]);
        //        arrAbsolutePosPulse[i] = ConvertUmToPulse(enIndexs[i], arrAbsolutePos[i]);
        //    }

        //    // Velocity of master axis
        //    double dVelocityPulse = ConvertUmToPulse(enIndexs[0], dblVelocity);

        //    return m_instanceSDK.MoveLinearAbsolutelyIX(arrTargetIndex, arrAbsolutePos, arrMctlParam, dVelocityPulse, uRatio, uDelay);
        //}

        // 2021.08.16 by Thienvv [ADD] Move Circle interpolation for BALL ATTACH XY
        /// <summary>
        /// Parameters (position, speed) are in um
        /// </summary>
        /// <param name="nIndexs"></param>
        /// <param name="arrParamSpeed"></param>
        /// <param name="dVelocity"></param>
        /// <param name="dcceleration"></param>
        /// <param name="dDeceleration"></param>
        /// <param name="dXrest"></param>
        /// <param name="dYrest"></param>
        /// <param name="dXcenter"></param>
        /// <param name="dYcenter"></param>
        /// <param name="dRadius"></param>
        /// <param name="dPitch"></param>
        /// <param name="nRepeat"></param>
        /// <param name="enBallAttachDir"></param>
        //public void MoveCircleIX(EN_MOTION_LIST[] enIndexs,  MotionInstance::Motion_.MOTION_SPEED_CONTENT speedContent,
        //    double dVelocity, 
        //    double dXrest, double dYrest, double dXcenter, double dYcenter, double dRadius, double dPitch, int nRepeat, int nBallAttachDir)
        //{
        //    // if (m_bInit == false) return; // 장치가 초기화 되지 않았을 경우 리턴

        //    // Check target for Ball Attach XY
        //    if (enIndexs[0] != EN_MOTION_LIST.BALLHEAD_X
        //        || enIndexs[1] != EN_MOTION_LIST.BALLHEAD_Y)
        //    {
        //        return;
        //    }

        //    MCTL_PARAM_SPEED[] arrMctlParam = new MCTL_PARAM_SPEED[enIndexs.Length];
        //    int[] nTargetIndex = new int[enIndexs.Length];
        //    for (int i = 0; i < enIndexs.Length; i++)
        //    {
        //        PARAM_SPEED paramSpeed = new PARAM_SPEED();
        //        MCTL_PARAM_SPEED mctlParam = new MCTL_PARAM_SPEED();
        //        GetSpeedConfig(enIndexs[i], speedContent, ref paramSpeed);
        //        CopyParameter(ref paramSpeed, ref mctlParam);

        //        // Convert To pulse value
        //        mctlParam.dblMaxVelocity = ConvertUmToPulse(enIndexs[i], mctlParam.dblMaxVelocity);
        //        mctlParam.dblVelocity = ConvertUmToPulse(enIndexs[i], mctlParam.dblVelocity);

        //        arrMctlParam[i] = mctlParam;
        //        nTargetIndex[i] = GetTargetIndex(enIndexs[i]);
        //    }

        //    // Convert From um to Pulse for positions
        //    EN_MOTION_LIST enIndexAxisX = enIndexs[0];
        //    EN_MOTION_LIST enIndexAxisY = enIndexs[1];

        //    double dVelocityPulse = ConvertUmToPulse(enIndexAxisX, dVelocity);
        //    int nAccelerationTime = 80; // ConvertUmToPulse(enIndexAxisX, dAcceleration);
        //    int nDecelerationTime = 80; // ConvertUmToPulse(enIndexAxisX, dDeceleration);

        //    // Positions
        //    double dXrestPulse = ConvertUmToPulse(enIndexAxisX, dXrest);
        //    double dYrestPulse = ConvertUmToPulse(enIndexAxisY, dYrest);
        //    double dXcenterPulse = ConvertUmToPulse(enIndexAxisX, dXcenter);
        //    double dYcenterPulse = ConvertUmToPulse(enIndexAxisY, dYcenter);

        //    // Radius and Pitch use X Axis
        //    double dRadiusPulse = ConvertUmToPulse(enIndexAxisX, dRadius);
        //    double dPitchPulse = ConvertUmToPulse(enIndexAxisX, dPitch);

        //    m_instanceSDK.MoveCircleIX(nTargetIndex, arrMctlParam, dVelocityPulse, nAccelerationTime, nDecelerationTime,
        //        dXrestPulse, dYrestPulse, dXcenterPulse, dYcenterPulse, dRadiusPulse, dPitchPulse, nRepeat, nBallAttachDir);
        //}
        #endregion </Move Interface>

        #region <Network Interface>
        /// <summary>
        /// 2020.11.09 by yjlee [ADD] Connect the link on the network for the motion.
        /// </summary>
        public override bool ConnectLink()
        {
            return true;// m_instanceSDK.ConnectLink();
        }

        /// <summary>
        /// 2020.11.09 by yjlee [ADD] Check the link is connected without the problems.
        /// </summary>
        public override bool IsLinkConnected()
        {
            return m_instanceWrapper.Connect;// m_instanceSDK.IsLinkConnected();
        }
        #endregion </Network Interface>

        #region <Move Others>
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

        #endregion </Move Others>

        private void ChangeMotorBrakeStateByExternalFunction(int nAxis, bool bTrigger)
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
        #endregion </Methods>
    }
}