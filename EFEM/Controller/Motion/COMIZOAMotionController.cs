using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Concurrent;

using Motion_;
using COMIZOASDK_.Motion_;

using System.Runtime.InteropServices;

namespace FrameOfSystem3.Controller.Motion
{
    extern alias MotionInstance;

    /// <summary>
    /// 2021.03.15 by yjlee [ADD] It's the wrapping class for the COMIZOA SDK.
    /// </summary>
    class COMIZOAMotionController : MotionController
    {
        struct stAdditionalConfig
        {
            public bool UseServoReverse
            {
                get
                {
                    return _useServoReverse;
                }
            }
            public bool UseInposition
            {
                get
                {
                    return _useInposition;
                }
            }

            public stAdditionalConfig(bool useServoReverse, bool useInposition)
            {
                _useServoReverse = useServoReverse;
                _useInposition = useInposition;
            }

            bool _useServoReverse;
            bool _useInposition;

            public void SetInformation(bool useServoReverse, bool useInposition)
            {
                _useServoReverse = useServoReverse;
                _useInposition = useInposition;
            }
        }

        #region Variables
        const int DELAY_RESET               = 20;       // ms
        COMIZOAMotion m_instanceSDK         = null;

        private Dictionary<int, stAdditionalConfig> _dicAdditionalConfig = null;
        private const string _cUseServoOnReverse = "USE_SERVO_ON_REVERSE";
        private const string _cUseInposition = "USE_INPOSITION";

        ConcurrentDictionary<int, DelegateChangeMotorBrakeState> m_dicFuncChangeMotorBrakeState = null;
        #endregion

        #region Internal Interface
        /// <summary>
        /// 2021.03.15 by yjlee [ADD] Copy the parameter value from the app to the controller.
        /// </summary>
        private void CopyParameter(ref CMPARAM_CONFIG cmParam, ref PARAM_CONFIG paramConfig)
        {
            cmParam.bAlarmLogic                 = paramConfig.bAlarmLogic;
            cmParam.bAlarmStopMode              = paramConfig.bAlarmStopMode;

            cmParam.bCommandDirection           = paramConfig.bCommandDirection;
            cmParam.bEncoderDirection           = paramConfig.bEncoderDirection;

            cmParam.bHWLimitLogicNegative       = paramConfig.bHWLimitLogicNegative;
            cmParam.bHWLimitLogicPositive       = paramConfig.bHWLimitLogicPositive;

            cmParam.bHWLimitStopModeNegative    = paramConfig.bHWLimitStopModeNegative;
            cmParam.bHWLimitStopModePositive    = paramConfig.bHWLimitStopModePositive;

            cmParam.bUseSWLimitNegative         = paramConfig.bUseSWLimitNegative;
            cmParam.bUseSWLimitPositive         = paramConfig.bUseSWLimitPositive;
            cmParam.dblSWLimitPositionNegative  = paramConfig.dblSWLimitPositionNegative;
            cmParam.dblSWLimitPositionPositive  = paramConfig.dblSWLimitPositionPositive;

            cmParam.bZPhaseLogic                = paramConfig.bZPhaseLogic;

            switch(paramConfig.enMotorType)
            {
                case MOTOR_TYPE.SERVO:
                    switch(paramConfig.enMotorInput)
                    {
                        case Motion_.MOTOR_INPUTMODE.AB1X:
                            cmParam.enMotorInput        = COMIZOASDK_.Motion_.MOTOR_INPUTMODE.AB1X;
                            break;

                        case Motion_.MOTOR_INPUTMODE.AB2X:
                            cmParam.enMotorInput        = COMIZOASDK_.Motion_.MOTOR_INPUTMODE.AB2X;
                            break;

                        case Motion_.MOTOR_INPUTMODE.AB4X:
                            cmParam.enMotorInput        = COMIZOASDK_.Motion_.MOTOR_INPUTMODE.AB4X;
                            break;

                        case Motion_.MOTOR_INPUTMODE.CWCCW:
                            cmParam.enMotorInput        = COMIZOASDK_.Motion_.MOTOR_INPUTMODE.CWCCW;
                            break;

                        case Motion_.MOTOR_INPUTMODE.STEP:
                            cmParam.enMotorInput        = COMIZOASDK_.Motion_.MOTOR_INPUTMODE.STEP;
                            break;
                    }
                    break;

                case MOTOR_TYPE.STEPPING:
                    cmParam.enMotorInput        = COMIZOASDK_.Motion_.MOTOR_INPUTMODE.STEP;
                    break;
            }

            switch(paramConfig.enMotorOutput)
            {
                case Motion_.MOTOR_OUTPUTMODE.CW_PULSE:
                    cmParam.enMotorOutput       = COMIZOASDK_.Motion_.MOTOR_OUTPUTMODE.CW_PULSE;
                    break;

                case Motion_.MOTOR_OUTPUTMODE.CW_PULSE_REVERSE:
                    cmParam.enMotorOutput       = COMIZOASDK_.Motion_.MOTOR_OUTPUTMODE.CW_PULSE_REVERSE;
                    break;

                case Motion_.MOTOR_OUTPUTMODE.CWCCW_HIGH:
                    cmParam.enMotorOutput       = COMIZOASDK_.Motion_.MOTOR_OUTPUTMODE.CWCCW_HIGH;
                    break;

                case Motion_.MOTOR_OUTPUTMODE.CWCCW_LOW:
                    cmParam.enMotorOutput       = COMIZOASDK_.Motion_.MOTOR_OUTPUTMODE.CWCCW_LOW;
                    break;

                case Motion_.MOTOR_OUTPUTMODE.ABPHASE:
                    cmParam.enMotorOutput       = COMIZOASDK_.Motion_.MOTOR_OUTPUTMODE.ABPHASE;
                    break;
            }
        }

        /// <summary>
        /// 2021.03.15 by yjlee [ADD] Copy the parameter value from the app to the controller.
        /// </summary>
        private void CopyParameter(ref CMPARAM_HOME cmParam, ref PARAM_HOME paramHome)
        {
            cmParam.bHomeDirection              = paramHome.bHomeDirection;
            cmParam.bHomeLogic                  = paramHome.bHomeLogic;
            cmParam.dblHomeVelocityStart        = paramHome.dblHomeVelocityStart;
            cmParam.dblHomeVelocityLast         = paramHome.dblHomeVelocityLast;
            cmParam.dblHomeAccelTime            = paramHome.dblHomeAccelTime;
            cmParam.dblHomeDecelTime            = paramHome.dblHomeDecelTime;
            cmParam.dblHomeEscapeDist           = paramHome.dblHomeEscapeDist;
            cmParam.dblHomeOffset               = paramHome.dblHomeOffset;
            cmParam.dblHomeBasePosition         = paramHome.dblHomeBasePosition;
            
            switch(paramHome.enHomeMode)
            {
                case Motion_.HOME_MODE.ORG_ONESTEP:
                case Motion_.HOME_MODE.ABSOLUTE_ONLY:
                case Motion_.HOME_MODE.FIXED:
                    cmParam.enHomeMode      = COMIZOASDK_.Motion_.HOME_MODE.ORG_ONESTEP;
                    break;

                case Motion_.HOME_MODE.ORG_TWOSTEP:
                    cmParam.enHomeMode      = COMIZOASDK_.Motion_.HOME_MODE.ORG_TWOSTEP;
                    break;

                case Motion_.HOME_MODE.ORG_EZ_ONESTEP_INSTANTSTOP:
                    cmParam.enHomeMode      = COMIZOASDK_.Motion_.HOME_MODE.ORG_EZ_ONESTEP_INSTANTSTOP;
                    break;

                case Motion_.HOME_MODE.EZ_ONESTEP:
                case Motion_.HOME_MODE.ORG_EZ_ONESTEP_STOP:
                    cmParam.enHomeMode      = COMIZOASDK_.Motion_.HOME_MODE.ORG_EZ_ONESTEP_STOP;
                    break;

                case Motion_.HOME_MODE.ORG_EZ_TWOSTEP_INSTANTSTOP:
                    cmParam.enHomeMode      = COMIZOASDK_.Motion_.HOME_MODE.ORG_EZ_TWOSTEP_INSTANTSTOP;
                    break;

                case Motion_.HOME_MODE.ORG_EZ_TWOSTEP_STOP:
                    cmParam.enHomeMode      = COMIZOASDK_.Motion_.HOME_MODE.ORG_EZ_TWOSTEP_STOP;
                    break;

                case Motion_.HOME_MODE.EL_TWOSTEP:
                    cmParam.enHomeMode      = COMIZOASDK_.Motion_.HOME_MODE.EL_ONESTEP;
                    break;

                case Motion_.HOME_MODE.EL_EZ_TWOSTEP_INSTANTSTOP:
                    cmParam.enHomeMode      = COMIZOASDK_.Motion_.HOME_MODE.EL_EZ_TWOSTEP_INSTANTSTOP;
                    break;

                case Motion_.HOME_MODE.EL_EZ_TWOSTEP_STOP:
                    cmParam.enHomeMode      = COMIZOASDK_.Motion_.HOME_MODE.EL_EZ_TWOSTEP_STOP;
                    break;
            }

            switch(paramHome.enHomeSpeedPattern)
            {
                case Motion_.MOTION_SPEED_PATTERN.ANTI_VABRATION:
                case Motion_.MOTION_SPEED_PATTERN.CONSTANT:
                    cmParam.enHomeSpeedPattern      = COMIZOASDK_.Motion_.MOTION_SPEED_PATTERN.CONSTANT;
                    break;

                case Motion_.MOTION_SPEED_PATTERN.S_CURVE:
                    cmParam.enHomeSpeedPattern      = COMIZOASDK_.Motion_.MOTION_SPEED_PATTERN.S_CURVE;
                    break;

                case Motion_.MOTION_SPEED_PATTERN.TRAPEZODIAL:
                    cmParam.enHomeSpeedPattern      = COMIZOASDK_.Motion_.MOTION_SPEED_PATTERN.TRAPEZODIAL;
                    break;
            }
        }

        /// <summary>
        /// 2021.03.15 by yjlee [ADD] Copy the parameter value from the app to the controller.
        /// </summary>
        private void CopyParameter(ref CMPARAM_GANTRY cmParam, ref PARAM_GANTRY paramGantry)
        {
            cmParam.bEnableGantry           = paramGantry.bEnableGantry;
            cmParam.bReverseSlaveDirection  = paramGantry.bReverseSlaveDirection;
            cmParam.nIndexOfMaster          = paramGantry.nIndexOfMaster;
            cmParam.nIndexOfSlave           = paramGantry.nIndexOfSlave;
        }

        /// <summary>
        /// 2021.03.15 by yjlee [ADD] Copy the parameter value from the app to the controller.
        /// </summary>
        private void CopyParameter(ref CMPARAM_SPEED cmParam, ref PARAM_SPEED paramSpeed)
        {
            cmParam.dblVelocity         = paramSpeed.dblVelocity;
            cmParam.dblAccelTime        = paramSpeed.dblAccelTime;
            cmParam.dblDecelTime        = paramSpeed.dblDecelTime;

            switch (paramSpeed.enSpeedPattern)
            {
                case Motion_.MOTION_SPEED_PATTERN.ANTI_VABRATION:
                case Motion_.MOTION_SPEED_PATTERN.CONSTANT:
                    cmParam.enSpeedPattern  = COMIZOASDK_.Motion_.MOTION_SPEED_PATTERN.CONSTANT;
                    break;

                case Motion_.MOTION_SPEED_PATTERN.S_CURVE:
                    cmParam.enSpeedPattern  = COMIZOASDK_.Motion_.MOTION_SPEED_PATTERN.S_CURVE;
                    break;

                case Motion_.MOTION_SPEED_PATTERN.TRAPEZODIAL:
                    cmParam.enSpeedPattern  = COMIZOASDK_.Motion_.MOTION_SPEED_PATTERN.TRAPEZODIAL;
                    break;
            }
        }
        #endregion

        #region Controller
        /// <summary>
        /// 2021.03.15 by yjlee [ADD] Initialize the controller.
        /// </summary>
        public override bool InitController()
        {
            m_instanceSDK       = COMIZOAMotion.GetInstance();

            bool controllerResult = m_instanceSDK.InitController();

            if (false == controllerResult) return false;
            else
            {
                _dicAdditionalConfig = new Dictionary<int, stAdditionalConfig>();

                try
                {
                    string additionalConfigFullPath = System.Environment.CurrentDirectory + @"\COMIZOASDK\AxisInfo.ini";

                    if (false == System.IO.Directory.Exists(System.IO.Path.GetDirectoryName(additionalConfigFullPath)))
                    {
                        System.IO.Directory.CreateDirectory(System.IO.Path.GetDirectoryName(additionalConfigFullPath));
                    }

                    IniUtil ini = new IniUtil(additionalConfigFullPath);

                    int axisCount = m_instanceSDK.GetCountOfAxis();

                    for (int i = 0; i < axisCount; i++)
                    {
                        int servoRevese = ini.GetInt(string.Format("AXIS_{0}", i), _cUseServoOnReverse, 0);
                        ini.WriteInt(string.Format("AXIS_{0}", i), _cUseServoOnReverse, servoRevese);

                        int inposition = ini.GetInt(string.Format("AXIS_{0}", i), _cUseInposition, 0);
                        ini.WriteInt(string.Format("AXIS_{0}", i), _cUseInposition, inposition);

                        _dicAdditionalConfig.Add(i, new stAdditionalConfig(servoRevese == 1, inposition == 1));
                    }
                }
                catch
                {

                }
                
                return true;
            }
        }

        /// <summary>
        /// 2021.03.15 by yjlee [ADD] Exit the controller.
        /// </summary>
        public override void ExitController()
        {
            m_instanceSDK.ExitController();
        }

        /// <summary>
        /// 2021.03.15 by yjlee [ADD] Get the count of the axes.
        /// </summary>
        public override int GetCountOfAxis()
        {
            return m_instanceSDK.GetCountOfAxis();
        }
        #endregion

        #region Motor Configuration
        /// <summary>
        /// 2021.03.15 by yjlee [ADD] Set the motor configuration.
        /// </summary>
        public override void SetMotorConfiguration(ref int nAxis, ref PARAM_CONFIG paramConfig)
        {
            CMPARAM_CONFIG cmParam  = new CMPARAM_CONFIG();

            CopyParameter(ref cmParam, ref paramConfig);

            // 2021.07.28. by shkim. [ADD] INPOSITION 사용유무 별도의 Config로 관리
            if (_dicAdditionalConfig != null && _dicAdditionalConfig.ContainsKey(nAxis))
            {
                cmParam.bUseInposition = _dicAdditionalConfig[nAxis].UseInposition;
            }

            m_instanceSDK.SetMotorConfiguration(ref nAxis, ref cmParam);
        }

        /// <summary>
        /// 2021.03.15 by yjlee [ADD] Set the motor configuration for the home.
        /// </summary>
        public override void SetMotorHomeConfiguration(ref int nAxis, ref PARAM_HOME paramHome)
        {
            CMPARAM_HOME cmParam    = new CMPARAM_HOME();

            CopyParameter(ref cmParam, ref paramHome);

            m_instanceSDK.SetMotorHomeConfiguration(ref nAxis, ref cmParam);
        }

        /// <summary>
        /// 2021.03.15 by yjlee [ADD] Set the gantry state of the motion.
        /// </summary>
        public override void SetGantry(ref int nAxis, ref PARAM_GANTRY paramGantry, ref PARAM_SPEED paramSpeed)
        {
            CMPARAM_GANTRY cmParam      = new CMPARAM_GANTRY();
            CMPARAM_SPEED cmParamSpeed  = new CMPARAM_SPEED();

            CopyParameter(ref cmParam, ref paramGantry);
            CopyParameter(ref cmParamSpeed, ref paramSpeed);

            m_instanceSDK.SetGantry(ref nAxis, ref cmParam, ref cmParamSpeed);
        }

        /// <summary>
        /// 2021.03.15 by yjlee [ADD] Get the gantry state of the motion.
        /// </summary>
        public override bool GetGantryState(ref int nAxis, ref PARAM_GANTRY paramGantry)
        {
            CMPARAM_GANTRY cmParam      = new CMPARAM_GANTRY();

            CopyParameter(ref cmParam, ref paramGantry);

            return m_instanceSDK.GetGantryState(ref nAxis, ref cmParam);
        }
        #endregion

        #region Motor Information
        /// <summary>
        /// 2021.03.15 by yjlee [ADD] Get the motor states of the axes.
        /// </summary>
        public override void GetMotorInformationAll(ref int nCountOfAxis, ref int[] arState, ref double[] arActualPosition, ref double[] arCommandPosition)
        {
            m_instanceSDK.GetMotorInformationAll(ref nCountOfAxis, ref arState, ref arActualPosition, ref arCommandPosition);

            // 2021.03.15 by yjlee [ADD] Convert the bit state to the app.
            for (int nAxis = 0; nAxis < nCountOfAxis; ++nAxis)
            {
                int nControllerState = arState[nAxis];

                int nAppState = ((nControllerState >> (int)COMIZOASDK_.Motion_.MOTOR_STATE.MOTOR_ON) & 1) << (int)MotionInstance::Motion_.MOTOR_STATE.MOTOR_ON;

                // 2021.07.28. by shkim. [ADD] Servo Off 상태가 정상상태로 모션 구동되는 배선 상황에서 사용하도록 Config 추가
                if (_dicAdditionalConfig != null && _dicAdditionalConfig.ContainsKey(nAxis))
                {
                    if (_dicAdditionalConfig[nAxis].UseServoReverse)
                    {
                        nAppState ^= 1;
                    }
                }

                nAppState |= ((nControllerState >> (int)COMIZOASDK_.Motion_.MOTOR_STATE.ALARM) & 1) << (int)MotionInstance::Motion_.MOTOR_STATE.ALARM;
                nAppState |= ((nControllerState >> (int)COMIZOASDK_.Motion_.MOTOR_STATE.LIMIT_POSITIVE) & 1) << (int)MotionInstance::Motion_.MOTOR_STATE.LIMIT_POSITIVE;
                nAppState |= ((nControllerState >> (int)COMIZOASDK_.Motion_.MOTOR_STATE.LIMIT_NEGATIVE) & 1) << (int)MotionInstance::Motion_.MOTOR_STATE.LIMIT_NEGATIVE;
                nAppState |= ((nControllerState >> (int)COMIZOASDK_.Motion_.MOTOR_STATE.HOME_SWITCH) & 1) << (int)MotionInstance::Motion_.MOTOR_STATE.HOME_SWITCH;

                arState[nAxis] = nAppState;
            }
        }

        /// <summary>
        /// 2021.03.15 by yjlee [ADD] Check the motion is done or not.
        /// </summary>
        public override bool IsMotionDone(ref int nAxis)
        {
            return m_instanceSDK.IsMotionDone(ref nAxis);
        }

        /// <summary>
        /// 2021.03.15 by yjlee [ADD] Check the motion's homing is done or not.
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
            return m_instanceSDK.IsMultiMotionDone(ref nCountOfAxis, ref arAxes);
        }

        /// <summary>
        /// 2021.04.29 by yjlee [ADD] Check whether the homings are done or not.
        /// </summary>
        public override bool IsMultiHomeDone(ref int nCountOfAxis, ref int[] arAxes)
        {
            return m_instanceSDK.IsMultiHomeDone(ref nCountOfAxis, ref arAxes);
        }

        /// <summary>
        /// 2021.03.15 by yjlee [ADD] Check the motor's power is on.
        /// </summary>
        public override bool IsMotorOn(ref int nAxis)
        {
            return m_instanceSDK.IsMotorOn(ref nAxis);
        }

        /// <summary>
        /// 2021.03.15 by yjlee [ADD] Do make the axis's power be on or off.
        /// </summary>
        public override void DoMotorOn(ref int nAxis, ref bool bON)
        {
            if (_dicAdditionalConfig != null && _dicAdditionalConfig.ContainsKey(nAxis))
            {
                if(_dicAdditionalConfig[nAxis].UseServoReverse)
                {
                    bON = !bON;
                }
            }
            //m_instanceSDK.DoMotorOn(ref nAxis, ref bON);

            if (bON)
            {
                m_instanceSDK.DoMotorOn(ref nAxis, ref bON);
                ChangeMotorBrakeStateByExternalFunction(nAxis, bON);
            }
            else
            {
                ChangeMotorBrakeStateByExternalFunction(nAxis, bON);
                m_instanceSDK.DoMotorOn(ref nAxis, ref bON);
            }
        }

        /// <summary>
        /// 2021.03.15 by yjlee [ADD] Reset the axis's state.
        /// </summary>
        public override void DoReset(ref int nAxis)
        {
            int nResetDelay     = DELAY_RESET;       // ms

            m_instanceSDK.DoReset(ref nAxis, ref nResetDelay);
        }

        /// <summary>
        /// 2021.03.15 by yjlee [ADD] Set the actual position of the axis.
        /// </summary>
        public override void SetActualPosition(ref int nAxis, ref double dblPosition)
        {
            m_instanceSDK.SetActualPosition(ref nAxis, ref dblPosition);
        }

        /// <summary>
        /// 2021.03.15 by yjlee [ADD] Set the command position of the axis.
        /// </summary>
        public override void SetCommandPosition(ref int nAxis, ref double dblPosition)
        {
            m_instanceSDK.SetCommandPosition(ref nAxis, ref dblPosition);
        }

        public override void GetAbsolutePosition(ref int nAxis, ref double dblPosition)
        {
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

        public override bool GetCurrentMotorTorqueValue(int nAxis, ref short dCurrentTorque)
        {
            dCurrentTorque = 0;

            return true;
        }

        public override bool GetGainTable(ref int nAxis, ref int nIndexOfTable)
        {
            return false;
        }

        public override bool ReleaseMotionGroup(ref int[] arAxisIndexes)
        {
            return false;
        }
        #endregion

        #region Speed Interface
        /// <summary>
        /// 2021.03.15 by yjlee [ADD] Set the motor configuration for the speed.
        /// </summary>
        public override void SetMotorSpeedConfiguration(ref int nAxis, ref PARAM_SPEED paramSpeed)
        {
            CMPARAM_SPEED cmParam       = new CMPARAM_SPEED();

            CopyParameter(ref cmParam, ref paramSpeed);

            m_instanceSDK.SetMotorSpeedConfiguration(ref nAxis, ref cmParam);
        }

        /// <summary>
        /// 2021.03.15 by yjlee [ADD] Set the motor configuration for the home's speed.
        /// </summary>
        public override void SetMotorHomeSpeedConfiguration(ref int nAxis, ref PARAM_HOME parapHome)
        {
            CMPARAM_HOME cmParam = new CMPARAM_HOME();

            CopyParameter(ref cmParam, ref parapHome);

            m_instanceSDK.SetMotorHomeSpeedConfiguration(ref nAxis, ref cmParam);
        }
        #endregion

        #region Move Interface
        /// <summary>
        /// 2021.03.15 by yjlee [ADD] Stop the home motion.
        /// </summary>
        public override void StopHomeMotion(ref int nAxis, ref bool bEmergency)
        {
            m_instanceSDK.StopMotion(ref nAxis, ref bEmergency);
        }

        /// <summary>
        /// 2021.03.15 by yjlee [ADD] Move to the home position.
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
            m_instanceSDK.StopMultiMotion(ref nCountOfAxis, ref arAxes, ref bEmergency);
        }

        /// <summary>
        /// 2021.04.29 by yjlee [ADD] Move the multi axes to the home positions.
        /// </summary>
        public override bool MoveMultiToHome(ref int nCountOfAxis, ref int[] arAxes, ref int nSeqNum, ref int nDelay)
        {
            return m_instanceSDK.MoveMultiToHome(ref nCountOfAxis, ref arAxes, ref nSeqNum, ref nDelay);
        }

        /// <summary>
        /// 2021.03.15 by yjlee [ADD] Stop the motion.
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
            m_instanceSDK.StopMultiMotion(ref nCountOfAxis, ref arAxes, ref bEmergency);
        }

        /// <summary>
        /// 2021.03.15 by yjlee [ADD] Move an axis to the position absolutely.
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
            m_instanceSDK.MoveMultiAbsolutely(ref nCountOfAxis, ref arAxes, ref arDestination);
        }

        /// <summary>
        /// 2021.03.15 by yjlee [ADD] Move an axis to the position releatively.
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
            m_instanceSDK.MoveMultiReleatively(ref nCountOfAxis, ref arAxes, ref arDestination);
        }

        /// <summary>
        /// 2021.03.15 by yjlee [ADD] Move an axis at the speed.
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
            m_instanceSDK.MoveMultiAtSpeed(ref nCountOfAxis, ref arAxes, ref arDirection);
        }

        /// <summary>
        /// 2021.06.30 by yjlee [ADD] Override the motion.
        /// </summary>
        public override void OverrideMotion(ref int nAxis, ref EN_OVERRIDE_TYPE enTypeOfOverride, ref double dblDestination, ref PARAM_SPEED paramSpeed)
        {
            CMPARAM_SPEED cmParam       = new CMPARAM_SPEED();

            CopyParameter(ref cmParam, ref paramSpeed);

            m_instanceSDK.OverrideSpeed(ref nAxis, ref cmParam);
        }

        /// <summary>
        /// 2021.03.15 by yjlee [ADD] Move the motion by the list steps.
        /// </summary>
        public override void MoveByList(ref int nAxis, ref int nCount, ref double[] arDestination, ref PARAM_SPEED[] arSpeedParam)
        {
            CMPARAM_SPEED[] arParam     = new CMPARAM_SPEED[nCount];

            for (int nIndex = 0; nIndex < nCount; ++nIndex)
            {
                arParam[nIndex]         = new CMPARAM_SPEED();

                CopyParameter(ref arParam[nIndex], ref arSpeedParam[nIndex]);
            }

            m_instanceSDK.MoveByList(ref nAxis, ref nCount, ref arDestination, ref arParam);
        }

        /// <summary>
        /// 2021.03.15 by yjlee [ADD] Not yet.
        /// </summary>
        public override void MoveByLinearCoordination(ref int nCountOfAxis, ref int[] arIndexes, ref int nCountOfStep, ref double[,] arDestination, ref PARAM_SPEED[,] arSpeedParam)
        {
            CMPARAM_SPEED[,] arParam         = new CMPARAM_SPEED[nCountOfStep, nCountOfAxis];

            for (int nStep = 0 ; nStep < nCountOfStep; ++ nStep)
            {
                for (int nIndex = 0; nIndex < nCountOfAxis; ++nIndex)
                {
                    arParam[nStep, nIndex]  = new CMPARAM_SPEED();

                    CopyParameter(ref arParam[nStep, nIndex], ref arSpeedParam[nStep, nIndex]);
                }
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
        /// 2021.03.15 by yjlee [ADD] Connect the link on the network for the motion.
        /// </summary>
        public override bool ConnectLink()
        {
            return true;
        }

        /// <summary>
        /// 2021.03.15 by yjlee [ADD] Check the link is connected without the problems.
        /// </summary>
        public override bool IsLinkConnected()
        {
            return true;
        }
        #endregion

        public override bool MoveUntilTouch(ref int nAxis, ref int nSeqNum, ref double dblDestination, ref int nEncoderAxis, ref double dblEncoderThreshold, ref bool bTouched, ref double dblTouchedPosition, ref double dblTochedEncoderPosition)
        {
            return m_instanceSDK.MoveUntilTouch(ref nAxis, ref nSeqNum, ref dblDestination, ref nEncoderAxis, ref dblEncoderThreshold, ref bTouched, ref dblTouchedPosition, ref dblTochedEncoderPosition);
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

        public override bool MakeDumpFile(ref int nAxis, string sFileName)
        {
            return false;
        }

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
        
    }

    public class IniUtil
    {
        #region Dll Import
        [DllImport("kernel32.dll")]
        private static extern int GetPrivateProfileString(
            String section,
            String key,
            String def,
            StringBuilder retVal,
            int size,
            String filePath);

        [DllImport("kernel32.dll")]
        private static extern long WritePrivateProfileString(
            String section,
            String key,
            String val,
            String filePath);
        #endregion

        #region Fields
        private string m_strIniPath;
        private string m_strSectionName;
        #endregion

        #region Properties
        public string SectionName
        {
            get { return m_strSectionName; }
            set { m_strSectionName = value; }
        }
        #endregion

        #region Constructor
        public IniUtil(string path)
        {
            this.m_strIniPath = path;
        }
        #endregion

        public void SetIniPath(string path)
        {
            this.m_strIniPath = path;
        }

        public String GetString(String key, String defStr)
        {   
            StringBuilder temp = new StringBuilder(511);
            int i = GetPrivateProfileString(m_strSectionName, key, defStr, temp, 511, m_strIniPath);
            return temp.ToString();
        }

        public String GetString(String section, String key, String defStr)
        {
            StringBuilder temp = new StringBuilder(255);
            int i = GetPrivateProfileString(section, key, defStr, temp, 255, m_strIniPath);
            return temp.ToString();
        }

        public long GetLong(String key, long defVal)
        {
            string defStr = String.Format("{0}", defVal);

            String stemp = GetString(m_strSectionName, key, defStr);
            return (Convert.ToInt64(stemp));
        }

        public long GetLong(String section, String key, long defVal)
        {
            string defStr = String.Format("{0}", defVal);

            String stemp = GetString(section, key, defStr);
            return (Convert.ToInt64(stemp));
        }

        public int GetInt(String key, int defVal)
        {
            string defStr = String.Format("{0}", defVal);

            String stemp = GetString(m_strSectionName, key, defStr);
            return (Convert.ToInt32(stemp));
        }

        public int GetInt(String section, String key, int defVal)
        {
            string defStr = String.Format("{0}", defVal);

            String stemp = GetString(section, key, defStr);
            return (Convert.ToInt32(stemp));
        }

        public double GetDouble(String key, double defVal)
        {
            string defStr = String.Format("{0}", defVal);

            String stemp = GetString(m_strSectionName, key, defStr);
            return (Convert.ToDouble(stemp));
        }

        public double GetDouble(String section, String key, double defVal)
        {
            string defStr = String.Format("{0}", defVal);

            String stemp = GetString(section, key, defStr);
            return (Convert.ToDouble(stemp));
        }

        public bool WriteString(String key, String writeStr)
        {
            return (WritePrivateProfileString(m_strSectionName, key, writeStr, m_strIniPath) == 0 ? false : true);
        }

        public bool WriteString(String section, String key, String writeStr)
        {
            return (WritePrivateProfileString(section, key, writeStr, m_strIniPath) == 0 ? false : true);
        }

        public bool WriteLong(String key, long writeVal)
        {
            return (WriteString(m_strSectionName, key, writeVal.ToString()));
        }

        public bool WriteLong(String section, String key, long writeVal)
        {
            return (WriteString(section, key, writeVal.ToString()));
        }

        public bool WriteInt(String key, int writeVal)
        {
            return (WriteString(m_strSectionName, key, writeVal.ToString()));
        }

        public bool WriteInt(String section, String key, int writeVal)
        {
            return (WriteString(section, key, writeVal.ToString()));
        }

        public bool WriteDouble(String key, double writeVal)
        {
            return (WriteString(m_strSectionName, key, writeVal.ToString()));
        }

        public bool WriteDouble(String section, String key, double writeVal)
        {
            return (WriteString(section, key, writeVal.ToString()));
        }
    }
}
