using System;
using System.Threading;
using System.Reflection;

using FrameOfSystem3.Controller.Motion.MechatroLinkAPI.References;
using FrameOfSystem3.Controller.Motion.MechatroLinkAPI.MechatroLinkDefines;

namespace FrameOfSystem3.Controller.Motion.MechatroLinkAPI.MechatroLinkAxis
{
    class MechatrolinkSteppingAxis : MechatrolinkAxis
    {
        #region <Constructor>
        public MechatrolinkSteppingAxis() { }
        public MechatrolinkSteppingAxis(iCommunicator pComm, int nTargetIndex, int nAxisIndex, string strName)
            : base(pComm, nTargetIndex, nAxisIndex, strName)
        {
        }
        #endregion </Constructor>

        #region <Fields>

        #region <Constants>
        private const int TIME_OUT_SERVO_ON = 500;
        private const int STOP_TIME = 100;
        private const int ESTOP_TIME = 50;
        private const int LITTLE_DELAY = 10;

        #region <Type>
        private const string TYPE_INPUT = "I";
        private const string TYPE_OUTPUT = "O";

        private const string TYPE_BIT = "B";
        private const string TYPE_WORD = "W";
        private const string TYPE_LONG = "L";
        #endregion <Type>

        #region <Input>

        #endregion </Input>

        #region <Output>

        #endregion </Output>

        #region <SERVO>
        private const int RUN_STATUS_ADDRESS = 0X00;
        private const int IN_RUN_STATUS_SERVO_BIT = 0x01;
        private const int OUT_RUN_STATUS_SERVO_BIT = 0x00;
        private const int OUT_RUN_STATUS_CLEAR_ALARM_BIT = 0x0F;
        #endregion </SERVO>

        #region <Motion Command>
        private const int MOTION_COMMAND_ADDRESS = 0x04;
        #endregion </Motion Command>

        #region <Alarm>
        private const int ALARM_ADDRESS = 0X03;
        #endregion </Alarm>

        #region <Motion Done>
        private const int MOTION_DONE_ADDRESS = 0X08;
        private const int MOTION_DONE_BIT = 0x01;
        #endregion </Motion Done>

        #region <Monitoring>
        private const int MONITORING_ADDRESS = 0x01;

        private const int MONITORING_ADDRESS_CH_1 = 0x0A;
        private const int MONITORING_ADDRESS_CH_2 = 0x0C;
        #endregion <Monitoring>

        #region <Motion Command Flag>
        private const int MOTION_COMMAND_FLAG_ADDRESS = 0x05;
        private const int MOTION_COMMAND_FLAG_JOG_DIR_BIT = 0x02;

        private const Int16 JOG_DIR_CW = 0x00;
        private const Int16 JOG_DIR_CCW = 0x01;

        private const int POSITION_REFERENCE_ADDRESS = 0x0C;

        private const int POSITION_REFERENCE_TYPE_ADDRESS = 0x05;
        private const int POSITION_REFERENCE_TYPE_BIT = 0x05;
        #endregion </Motion Command Flag>

        #region <Speed>
        private const int SPEED_OVERRIDE_ADDRESS = 0x07;
        private const int SPEED_REFERENCE_ADDRESS = 0x0A;
        #endregion </Speed>

        #region <General IO Address>
        private const int GENERAL_IO_ADDRESS = 0x09;

        private const int LIMIT_POSITIVE = 0x04;
        private const int LIMIT_NEGATIVE = 0x08;
        #endregion </General IO Address>

        #endregion </Constants>

        #region <Enum>
        public enum STEPPING_HOME_SEQ
        {
            INIT = 0,
            MOVE_JOG_TO_LIMIT_SENSOR = 100,
            MOVE_JOG_ESCAPE_FROM_SENSOR = 200,
            MOVE_TO_HOME_OFFSET = 300,
            SET_BASE_POS = 400,
            END = 1000,
        }
        #endregion </Enum>

        private double m_dPositionOffset;
        private bool m_bMovingDirection;
        private double m_dTargetPos;
        #endregion </Fields>

        #region <Methods>
        #region <Override Virtual Methods>
        public override void SetSpeedParam(MCTL_PARAM_SPEED stSpeed)
        {
            base.SetSpeedParam(stSpeed);

            UInt32 pRegisterHandle = 0;
            if (false == GetRegisterHandle(false, TYPE_LONG, SPEED_REFERENCE_ADDRESS, -1, ref pRegisterHandle, true))
            {
                // 보드를 열고
                if (false == m_pCommunicator.OpenApi(true))
                    return;

                if (false == GetRegisterHandle(false, TYPE_LONG, SPEED_REFERENCE_ADDRESS, -1, ref pRegisterHandle))
                    return;
            }

            Int32[] arOutputData = new Int32[] { (Int32)stSpeed.dblVelocity };
            UInt32 uCode = CMotionAPI.ymcSetRegisterData(pRegisterHandle, 1, arOutputData);
            if (false == m_pCommunicator.CheckingApi(uCode,
                MethodBase.GetCurrentMethod().Name, AxisIndex, "After ymcGetRegisterData"))
                return;
        }
        #endregion </Override Virtual Methods>

        #region <Inhert Methods>

        #region <Destructor>
        protected override void DestoryObject(ref UInt32 pAxisHandle)
        {
            ServoControl(ref pAxisHandle, false);
        }
        #endregion </Destructor>

        #region <State>
        protected override double GetActualPosition()
        {
            UInt32 pRegisterHandle = 0;
            if (false == GetRegisterHandle(true, TYPE_LONG, MONITORING_ADDRESS_CH_2, -1, ref pRegisterHandle, true))
            {
                // 보드를 열고
                if (false == m_pCommunicator.OpenApi(true))
                    return 0;

                if (false == GetRegisterHandle(true, TYPE_LONG, MONITORING_ADDRESS_CH_2, -1, ref pRegisterHandle))
                    return 0;
            }

            Int32[] arReadData = new Int32[1];
            UInt32 nReadNum = 0;
            UInt32 uCode = CMotionAPI.ymcGetRegisterData(pRegisterHandle, 1, arReadData, ref nReadNum);
            if (false == m_pCommunicator.CheckingApi(uCode,
                MethodBase.GetCurrentMethod().Name, AxisIndex, "After ymcGetRegisterData"))
                return 0;

            return arReadData[0] + m_dPositionOffset;
        }

        protected override double GetCommandPosition()
        {
            UInt32 pRegisterHandle = 0;
            if (false == GetRegisterHandle(true, TYPE_LONG, MONITORING_ADDRESS_CH_1, -1, ref pRegisterHandle, true))
            {
                // 보드를 열고
                if (false == m_pCommunicator.OpenApi(true))
                    return 0;

                if (false == GetRegisterHandle(true, TYPE_LONG, MONITORING_ADDRESS_CH_1, -1, ref pRegisterHandle))
                    return 0;
            }

            Int32[] arReadData = new Int32[1];
            UInt32 nReadNum = 0;
            UInt32 uCode = CMotionAPI.ymcGetRegisterData(pRegisterHandle, 1, arReadData, ref nReadNum);
            if (false == m_pCommunicator.CheckingApi(uCode,
                MethodBase.GetCurrentMethod().Name, AxisIndex, "After ymcGetRegisterData"))
                return 0;

            return arReadData[0] + m_dPositionOffset;
        }

        protected override bool GetServoState()
        {
            bool bServoState = false;

            UInt32 pRegisterHandle = 0;
            if (false == GetRegisterHandle(true, TYPE_BIT, RUN_STATUS_ADDRESS, IN_RUN_STATUS_SERVO_BIT, ref pRegisterHandle, true))
            {
                // 보드를 열고
                if (false == m_pCommunicator.OpenApi(true))
                    return false;

                if (false == GetRegisterHandle(true, TYPE_BIT, RUN_STATUS_ADDRESS, OUT_RUN_STATUS_SERVO_BIT, ref pRegisterHandle))
                    return false;
            }

            Int16[] arReadData = new Int16[1];
            UInt32 nReadNum = 0;
            UInt32 uCode = CMotionAPI.ymcGetRegisterData(pRegisterHandle, 1, arReadData, ref nReadNum);
            if (false == m_pCommunicator.CheckingApi(uCode,
                MethodBase.GetCurrentMethod().Name, AxisIndex, "After ymcGetRegisterData"))
                return false;

            bServoState = arReadData[0] == 0x01 ? true : false;

            return bServoState;
        }

        protected override bool GetAlarmState()
        {
            bool bAlarm = false;

            UInt32 pRegisterHandle = 0;
            if (false == GetRegisterHandle(true, TYPE_WORD, ALARM_ADDRESS, -1, ref pRegisterHandle, true))
            {
                // 보드를 열고
                if (false == m_pCommunicator.OpenApi(true))
                    return false;

                if (false == GetRegisterHandle(true, TYPE_WORD, RUN_STATUS_ADDRESS, -1, ref pRegisterHandle))
                    return false;
            }

            Int16[] arReadData = new Int16[1];
            UInt32 nReadNum = 0;
            UInt32 uCode = CMotionAPI.ymcGetRegisterData(pRegisterHandle, 1, arReadData, ref nReadNum);
            if (false == m_pCommunicator.CheckingApi(uCode,
                MethodBase.GetCurrentMethod().Name, AxisIndex, "After ymcGetRegisterData"))
                return false;

            bAlarm = arReadData[0] != 0x00 ? true : false;

            return bAlarm;
        }

        protected override void GetHWLimitState(ref bool bLimitPos, ref bool bLimitNeg)
        {
            if (m_stConfigParam == null)
                return;

            UInt32 pRegisterHandle = 0;
            if (false == GetRegisterHandle(true, TYPE_WORD, GENERAL_IO_ADDRESS, -1, ref pRegisterHandle, true))
            {
                // 보드를 열고
                if (false == m_pCommunicator.OpenApi(true))
                    return;

                if (false == GetRegisterHandle(true, TYPE_WORD, GENERAL_IO_ADDRESS, -1, ref pRegisterHandle))
                    return;
            }

            Int16[] arReadData = new Int16[1];
            UInt32 nReadNum = 0;
            UInt32 uCode = CMotionAPI.ymcGetRegisterData(pRegisterHandle, 1, arReadData, ref nReadNum);
            if (false == m_pCommunicator.CheckingApi(uCode,
                MethodBase.GetCurrentMethod().Name, AxisIndex, "After ymcGetRegisterData"))
                return;

            if (m_stConfigParam.bUseHWLimitPositive)
            {
                bLimitPos = ((arReadData[0] & LIMIT_POSITIVE) >> 2) == 0x01 ? true : false;

                if (false == m_stConfigParam.bHWLimitLogicPositive)
                    bLimitPos = !bLimitPos;
            }
            if (m_stConfigParam.bUseHWLimitNegative)
            {
                bLimitNeg = ((arReadData[0] & LIMIT_NEGATIVE) >> 3) == 0x01 ? true : false;

                if (false == m_stConfigParam.bHWLimitLogicNegative)
                    bLimitNeg = !bLimitNeg;
            }
        }

        protected override bool GetMotionCompletion()
        {
            bool bMotionDone = false;

            UInt32 pRegisterHandle = 0;
            if (false == GetRegisterHandle(true, TYPE_BIT, MOTION_DONE_ADDRESS, MOTION_DONE_BIT, ref pRegisterHandle, true))
            {
                // 보드를 열고
                if (false == m_pCommunicator.OpenApi(true))
                    return false;

                if (false == GetRegisterHandle(true, TYPE_BIT, MOTION_DONE_ADDRESS, MOTION_DONE_BIT, ref pRegisterHandle))
                    return false;
            }

            Int16[] arReadData = new Int16[1];
            UInt32 nReadNum = 0;
            UInt32 uCode = CMotionAPI.ymcGetRegisterData(pRegisterHandle, 1, arReadData, ref nReadNum);
            if (false == m_pCommunicator.CheckingApi(uCode,
                MethodBase.GetCurrentMethod().Name, AxisIndex, "After ymcGetRegisterData"))
                return false;

            bMotionDone = arReadData[0] == 0x01 ? true : false;

            return bMotionDone;
        }

        protected override bool GetGearedState(UInt32[] arAxes)
        {
            return false;
        }
        #endregion </State>

        #region <Axis Commands>
        protected override bool ServoControl(ref UInt32 pAxisHandle, bool bEnabled)
        {
            UInt32 pRegisterHandle = 0;
            if (false == GetRegisterHandle(false, TYPE_BIT, RUN_STATUS_ADDRESS, OUT_RUN_STATUS_SERVO_BIT, ref pRegisterHandle, true))
            {
                // 보드를 열고
                if (false == m_pCommunicator.OpenApi(true))
                    return false;

                if (false == GetRegisterHandle(false, TYPE_BIT, RUN_STATUS_ADDRESS, OUT_RUN_STATUS_SERVO_BIT, ref pRegisterHandle))
                    return false;
            }

            Int16 nBit = 0;
            if (bEnabled)
                nBit = 0x01;
            else
                nBit = 0x00;

            Int16[] arOutputData = new Int16[] { nBit };
            UInt32 uCode = CMotionAPI.ymcSetRegisterData(pRegisterHandle, 1, arOutputData);
            if (false == m_pCommunicator.CheckingApi(uCode,
                MethodBase.GetCurrentMethod().Name, AxisIndex, "After ymcGetRegisterData"))
                return false;

            InitMotionFlags();

            if (false == bEnabled)
            {
                Stop(false);

                HomeCompletion = false;
            }

            return true;
        }

        protected override bool ClearAlarm(ref UInt32 pAxisHandle)
        {
            StopMotion(ref pAxisHandle, false);

            // 이미 알람 비트가 켜져있는 경우에는 껐다가 켜야 하므로 
            // 스레드 태워서 알람 비트 On 후 자동 Off 하도록 한다.
            // StopMotion 하는 이유는, 이동 명령 중 알람 발생 시 끄기 위함
            //ThreadPool.QueueUserWorkItem(x => ResetAlarm());

            // 2023.04.25. jhlim [MOD] 상위 클래스에서 스레도 호출로 변경됨
            ResetAlarm();

            return true;
        }

        protected override bool DefinePosition(ref UInt32 pAxisHandle, double dPosition)
        {
            UInt32 pRegisterHandle = 0;

            // Jog Command
            if (false == GetRegisterHandle(false, TYPE_WORD, MOTION_COMMAND_ADDRESS, -1, ref pRegisterHandle, true))
            {
                // 보드를 열고
                if (false == m_pCommunicator.OpenApi(true))
                    return false;

                if (false == GetRegisterHandle(false, TYPE_WORD, MOTION_COMMAND_ADDRESS, -1, ref pRegisterHandle))
                    return false;
            }


            Int16[] arOutputData = new Int16[] { 0x09 };
            UInt32 uCode = CMotionAPI.ymcSetRegisterData(pRegisterHandle, 1, arOutputData);
            if (false == m_pCommunicator.CheckingApi(uCode,
                MethodBase.GetCurrentMethod().Name, AxisIndex, "After ymcGetRegisterData"))
                return false;

            m_dPositionOffset = dPosition;

            return true;
            //UInt32 pAxisHandle = 0;
            //if (false == GetAxisHandle(ref pAxisHandle))
            //{
            //    // 보드를 열고
            //    if (false == m_pCommunicator.OpenApi(true))
            //        return;

            //    if (false == DeclareAxis(ref pAxisHandle))
            //        return;
            //}

            //UInt32 pDeviceHandle = 0;
            //UInt32 uCode = CMotionAPI.ymcDeclareDevice(1, new UInt32[] { pAxisHandle }, ref pDeviceHandle);
            //if (false == m_pCommunicator.CheckingApi(uCode,
            //    MethodBase.GetCurrentMethod().Name, AxisIndex, "After ymcDeclareDevice"))
            //    return;


            //CMotionAPI.POSITION_DATA[] stPosData = new CMotionAPI.POSITION_DATA[1];
            //stPosData[0].DataType = (int)CMotionAPI.ApiDefs.DATATYPE_IMMEDIATE;
            //stPosData[0].PositionData = (Int32)dPosition;


            //uCode = CMotionAPI.ymcDefinePosition(pDeviceHandle, stPosData);
            //if (false == m_pCommunicator.CheckingApi(uCode,
            //   MethodBase.GetCurrentMethod().Name, AxisIndex, "After ymcDefinePosition"))
            //    return;

            //uCode = CMotionAPI.ymcClearDevice(pDeviceHandle);
            //if (false == m_pCommunicator.CheckingApi(uCode,
            //    MethodBase.GetCurrentMethod().Name, AxisIndex, "After ymcClearDevice"))
            //    return;
        }

        protected override void SetGearedAxis(ref UInt32[] arGantryHandles, UInt32[] arAxes, bool bEnabled)
        {

        }
        #endregion </Axis Commands>

        #region <Motion Commands>
        protected override bool StopMotion(ref UInt32 pAxisHandle, bool bEmergency)
        {
            UInt32 pRegisterHandle = 0;
            if (false == GetRegisterHandle(false, TYPE_WORD, MOTION_COMMAND_ADDRESS, -1, ref pRegisterHandle, true))
            {
                // 보드를 열고
                if (false == m_pCommunicator.OpenApi(true))
                    return false;

                if (false == GetRegisterHandle(false, TYPE_WORD, MOTION_COMMAND_ADDRESS, -1, ref pRegisterHandle))
                    return false;
            }

            Int16[] arOutputData = new Int16[] { 0x00 };
            UInt32 uCode = CMotionAPI.ymcSetRegisterData(pRegisterHandle, 1, arOutputData);
            if (false == m_pCommunicator.CheckingApi(uCode,
                MethodBase.GetCurrentMethod().Name, AxisIndex, "After ymcGetRegisterData"))
                return false;

            m_bExecutingMotion = false;

            return true;
        }

        protected override bool MoveMotion(ref UInt32 pAxisHandle, ref int nMotionStep, double dPosition, bool bAbsolute, bool bCheckingMotion)
        {
            switch (nMotionStep)
            {
                case 0:
                    UInt32 pRegisterHandle = 0;

                    // Position Reference
                    if (false == GetRegisterHandle(false, TYPE_LONG, POSITION_REFERENCE_ADDRESS, -1, ref pRegisterHandle, true))
                    {
                        // 보드를 열고
                        if (false == m_pCommunicator.OpenApi(true))
                            return false;

                        if (false == GetRegisterHandle(false, TYPE_LONG, POSITION_REFERENCE_ADDRESS, -1, ref pRegisterHandle))
                            return false;
                    }


                    m_dTargetPos = dPosition;
                    if (false == bAbsolute)
                    {
                        m_dTargetPos = ActualPosition - m_dPositionOffset + dPosition;
                    }
                    else
                    {
                        m_dTargetPos = Math.Round(dPosition - m_dPositionOffset);
                    }

                    m_bMovingDirection = (ActualPosition <= dPosition ? true : false);

                    Int32[] arOutputData = new Int32[] { (Int32)m_dTargetPos };
                    UInt32 uCode = CMotionAPI.ymcSetRegisterData(pRegisterHandle, 1, arOutputData);
                    if (false == m_pCommunicator.CheckingApi(uCode,
                        MethodBase.GetCurrentMethod().Name, AxisIndex, "After ymcGetRegisterData"))
                        return false;


                    // Posing Command
                    if (false == GetRegisterHandle(false, TYPE_WORD, MOTION_COMMAND_ADDRESS, -1, ref pRegisterHandle, true))
                    {
                        // 보드를 열고
                        if (false == m_pCommunicator.OpenApi(true))
                            return false;

                        if (false == GetRegisterHandle(false, TYPE_WORD, MOTION_COMMAND_ADDRESS, -1, ref pRegisterHandle))
                            return false;
                    }

                    Int16[] arOutputCommand = new Int16[] { (Int16)MOTION_COMMAND_MODE_TYPE.MOTION_COMMAND_MOVE };
                    uCode = CMotionAPI.ymcSetRegisterData(pRegisterHandle, 1, arOutputCommand);
                    if (false == m_pCommunicator.CheckingApi(uCode,
                        MethodBase.GetCurrentMethod().Name, AxisIndex, "After ymcGetRegisterData"))
                        return false;

                    m_bExecutingMotion = true;

                    //if (false == bCheckingMotion)
                    //    return true;

                    ++nMotionStep; break;

                case 1:
                    Int32 nCheckingPos = (Int32)(m_dTargetPos + m_dPositionOffset);

                    if (NeedStop(m_bMovingDirection))
                    {
                        StopMotion(ref pAxisHandle, true);
                        return true;
                    }

                    if (EXECUTING_COMMAND_TYPE.STOP == GetExecutingCommand())
                    {
                        return true;
                    }

                    if (ActualPosition != nCheckingPos)
                        break;

                    m_bExecutingMotion = false;

                    return true;

            }

            return false;
        }

        //protected override void MoveMultiMotion(int[] arAxes, double dPosition, bool bAbsolute)
        //{

        //}

        protected override bool MoveJog(ref UInt32 pAxisHandle, bool bDirection, bool bUsingMultiJogging = true)
        {
            UInt32 pRegisterHandle = 0;

            // Jog Direction
            if (false == GetRegisterHandle(false, TYPE_BIT, MOTION_COMMAND_FLAG_ADDRESS, MOTION_COMMAND_FLAG_JOG_DIR_BIT, ref pRegisterHandle, true))
            {
                // 보드를 열고
                if (false == m_pCommunicator.OpenApi(true))
                    return false;

                if (false == GetRegisterHandle(false, TYPE_BIT, MOTION_COMMAND_FLAG_ADDRESS, MOTION_COMMAND_FLAG_JOG_DIR_BIT, ref pRegisterHandle))
                    return false;
            }

            Int16 uDir = bDirection ? JOG_DIR_CW : JOG_DIR_CCW;
            Int16[] arOutputData = new Int16[] { uDir };
            UInt32 uCode = CMotionAPI.ymcSetRegisterData(pRegisterHandle, 1, arOutputData);
            if (false == m_pCommunicator.CheckingApi(uCode,
                MethodBase.GetCurrentMethod().Name, AxisIndex, "After ymcGetRegisterData"))
                return false;


            // Jog Command
            if (false == GetRegisterHandle(false, TYPE_WORD, MOTION_COMMAND_ADDRESS, -1, ref pRegisterHandle, true))
            {
                // 보드를 열고
                if (false == m_pCommunicator.OpenApi(true))
                    return false;

                if (false == GetRegisterHandle(false, TYPE_WORD, MOTION_COMMAND_ADDRESS, -1, ref pRegisterHandle))
                    return false;
            }

            arOutputData = new Int16[] { (Int16)MOTION_COMMAND_MODE_TYPE.MOTION_COMMAND_JOG };
            uCode = CMotionAPI.ymcSetRegisterData(pRegisterHandle, 1, arOutputData);
            if (false == m_pCommunicator.CheckingApi(uCode,
                MethodBase.GetCurrentMethod().Name, AxisIndex, "After ymcGetRegisterData"))
                return false;

            m_bExecutingMotion = true;

            return true;
        }

        //protected override void MoveMultiJog(int[] arAxes, bool bDirection)
        //{


        //}

        protected override bool OverrideSpeed(ref UInt32 pAxisHandle, double dVelocity)
        {
            UInt32 pRegisterHandle = 0;
            if (false == GetRegisterHandle(false, TYPE_WORD, SPEED_OVERRIDE_ADDRESS, -1, ref pRegisterHandle, true))
            {
                // 보드를 열고
                if (false == m_pCommunicator.OpenApi(true))
                    return false;

                if (false == GetRegisterHandle(false, TYPE_WORD, SPEED_OVERRIDE_ADDRESS, -1, ref pRegisterHandle))
                    return false;
            }

            UInt16[] arOutputData = new UInt16[] { (UInt16)dVelocity };
            UInt32 uCode = CMotionAPI.ymcSetRegisterData(pRegisterHandle, 1, arOutputData);

            return m_pCommunicator.CheckingApi(uCode,
                MethodBase.GetCurrentMethod().Name, AxisIndex, "After ymcGetRegisterData");
        }

        protected override EXECUTING_COMMAND_TYPE GetExecutingCommand()
        {
            UInt32 pRegisterHandle = 0;
            if (false == GetRegisterHandle(false, TYPE_WORD, MOTION_COMMAND_ADDRESS, -1, ref pRegisterHandle, true))
            {
                // 보드를 열고
                if (false == m_pCommunicator.OpenApi(true))
                    return EXECUTING_COMMAND_TYPE.UNKNOWN;

                if (false == GetRegisterHandle(false, TYPE_WORD, MOTION_COMMAND_ADDRESS, -1, ref pRegisterHandle))
                    return EXECUTING_COMMAND_TYPE.UNKNOWN;
            }

            Int16[] arReadData = new Int16[1];
            UInt32 nReadNum = 0;
            UInt32 uCode = CMotionAPI.ymcGetRegisterData(pRegisterHandle, 1, arReadData, ref nReadNum);
            if (false == m_pCommunicator.CheckingApi(uCode,
                MethodBase.GetCurrentMethod().Name, AxisIndex, "After ymcGetRegisterData"))
                return EXECUTING_COMMAND_TYPE.UNKNOWN;

            switch (arReadData[0])
            {
                case (Int16)MOTION_COMMAND_MODE_TYPE.MOTION_COMMAND_STOP:
                    return EXECUTING_COMMAND_TYPE.STOP;
                case (Int16)MOTION_COMMAND_MODE_TYPE.MOTION_COMMAND_MOVE:
                    return EXECUTING_COMMAND_TYPE.MOVE;
                case (Int16)MOTION_COMMAND_MODE_TYPE.MOTION_COMMAND_JOG:
                    return EXECUTING_COMMAND_TYPE.JOG;
                case (Int16)MOTION_COMMAND_MODE_TYPE.MOTION_COMMAND_HOME:
                    return EXECUTING_COMMAND_TYPE.HOME;
                default:
                    return EXECUTING_COMMAND_TYPE.UNKNOWN;

            }
        }
        #endregion </Motion Commands>

        #region <Homing>
        public override int CycleHoming(ref UInt32 pAxisHandle, ref int nSeq, ref int nDelay)
        {
            if (nSeq > (int)STEPPING_HOME_SEQ.INIT && nSeq < (int)STEPPING_HOME_SEQ.END)
            {
                if (m_tickHomingTimeoutChecker.IsTickOver(true))
                {
                    StopMotion(ref pAxisHandle, false);
                    HomeTimeout = true;
                    return HOMING_RESULT_END;
                }
            }

            switch (nSeq)
            {
                case (int)STEPPING_HOME_SEQ.INIT:

                    // 플래그 초기화
                    HomeTimeout = false;
                    m_bReqStopHoming = false;
                    HomeCompletion = false;

                    SetPosition(0);

                    m_tickHomingTimeoutChecker.Clear();
                    m_tickHomingTimeoutChecker.SetTickCount(m_stHomingParam.uHomeTimeout);
                    nSeq = (int)STEPPING_HOME_SEQ.MOVE_JOG_TO_LIMIT_SENSOR;
                    break;

                case (int)STEPPING_HOME_SEQ.MOVE_JOG_TO_LIMIT_SENSOR:
                    bool bDetected = false;
                    if (m_stHomingParam.bHomeDirection)
                    {
                        bDetected = HWLimitPositive;
                    }
                    else
                    {
                        bDetected = HWLimitNegative;
                    }

                    if (bDetected)
                    {
                        nSeq = (int)STEPPING_HOME_SEQ.MOVE_JOG_ESCAPE_FROM_SENSOR;
                        break;
                    }

                    SetSpeedParam(m_stHomingJoggingParam);
                    MoveJog(ref pAxisHandle, m_stHomingParam.bHomeDirection);
                    ++nSeq; break;

                case (int)STEPPING_HOME_SEQ.MOVE_JOG_TO_LIMIT_SENSOR + 1:
                    {
                        if (m_bReqStopHoming)
                        {
                            nSeq = (int)STEPPING_HOME_SEQ.INIT;
                            m_bReqStopHoming = false;
                            return HOMING_RESULT_END;
                        }

                        bool bNeedStop = false;
                        if (m_stHomingParam.bHomeDirection)
                        {
                            bNeedStop = HWLimitPositive;
                        }
                        else
                        {
                            bNeedStop = HWLimitNegative;
                        }

                        if (false == bNeedStop)
                            break;

                        StopMotion(ref pAxisHandle, false);
                        m_tickHomingDelay.Clear();
                        m_tickHomingDelay.SetTickCount(200);
                        ++nSeq; break;
                    }

                case (int)STEPPING_HOME_SEQ.MOVE_JOG_TO_LIMIT_SENSOR + 2:
                    if (false == m_tickHomingDelay.IsTickOver(true))
                        break;

                    if (false == MotionDone)
                        break;
                    nSeq = (int)STEPPING_HOME_SEQ.MOVE_JOG_ESCAPE_FROM_SENSOR;
                    break;

                case (int)STEPPING_HOME_SEQ.MOVE_JOG_ESCAPE_FROM_SENSOR:
                    SetSpeedParam(m_stHomingEscapeParam);
                    MoveJog(ref pAxisHandle, !m_stHomingParam.bHomeDirection);
                    ++nSeq; break;

                case (int)STEPPING_HOME_SEQ.MOVE_JOG_ESCAPE_FROM_SENSOR + 1:
                    {
                        bool bNeedStop = false;
                        if (m_stHomingParam.bHomeDirection)
                        {
                            bNeedStop = !HWLimitPositive;
                        }
                        else
                        {
                            bNeedStop = !HWLimitNegative;
                        }

                        if (false == bNeedStop)
                            break;

                        StopMotion(ref pAxisHandle, false);
                        m_tickHomingDelay.Clear();
                        m_tickHomingDelay.SetTickCount(200);
                        ++nSeq; break;
                    }
                case (int)STEPPING_HOME_SEQ.MOVE_JOG_ESCAPE_FROM_SENSOR + 2:
                    if (false == m_tickHomingDelay.IsTickOver(true))
                        break;

                    if (false == MotionDone)
                        break;

                    SetPosition(0);
                    m_nMotionStep = 0;
                    m_tickHomingDelay.Clear();
                    m_tickHomingDelay.SetTickCount(200);
                    nSeq = (int)STEPPING_HOME_SEQ.MOVE_TO_HOME_OFFSET;
                    break;

                case (int)STEPPING_HOME_SEQ.MOVE_TO_HOME_OFFSET:
                    if (false == m_tickHomingDelay.IsTickOver(true))
                        break;
                    ++nSeq; break;

                case (int)STEPPING_HOME_SEQ.MOVE_TO_HOME_OFFSET + 1:
                    if (0 != m_stHomingParam.dblHomeOffset)
                    {
                        SetSpeedParam(m_stHomingJoggingParam);
                        
                        if (false == MoveMotion(ref pAxisHandle, ref m_nMotionStep, m_stHomingParam.dblHomeOffset, true, false))
                            break;

                        //Move(m_stHomingParam.dblHomeOffset, false);
                        m_tickHomingDelay.Clear();
                        m_tickHomingDelay.SetTickCount(200);
                    }
                    ++nSeq; break;

                case (int)STEPPING_HOME_SEQ.MOVE_TO_HOME_OFFSET + 2:
                    if (false == m_tickHomingDelay.IsTickOver(true))
                        break;

                    if (false == MotionDone)
                        break;

                    m_tickHomingDelay.Clear();
                    m_tickHomingDelay.SetTickCount(200);
                    nSeq = (int)STEPPING_HOME_SEQ.SET_BASE_POS;
                    break;

                case (int)STEPPING_HOME_SEQ.SET_BASE_POS:
                    if (false == m_tickHomingDelay.IsTickOver(true))
                        break;

                    SetPosition(m_stHomingParam.dblHomeBasePosition);
                    HomeCompletion = true;
                    nSeq = (int)STEPPING_HOME_SEQ.END;
                    break;

                case (int)STEPPING_HOME_SEQ.END:
                    return HOMING_RESULT_END;
            }

            return HOMING_RESULT_WORKING;
        }
        #endregion </Homing>

        #region <ETC>
        public override void InitAxis()
        {
            SetMonitoringRegister();

            UInt32 pAxisHandle = 0;
            StopMotion(ref pAxisHandle, true);

            // 시작할 때 100%로 고정한다.
            SetSpeedRatio(100);

            // 시작할 때 현재 위치를 0으로 변경한다.
            SetPosition(0);

            // Position Reference Value 를 On 으로 한다. (off면 구동 안 됨)
            SetPotisionReferenceValue(true);

            base.InitAxis();
        }
        #endregion </ETC>

        #endregion </Inhert Methods>

        #region <Internal Methods>
        private bool GetRegisterHandle(bool bInput, string strDataType, int nAddress, int nBit, ref UInt32 pHandle, bool bTry = false)
        {
            UInt32 uAddr = bInput ? RegisterInputAddress : RegisterOutputAddress;

            string strRegisterId = String.Empty;
            if (nBit < 0)
            {
                strRegisterId = String.Format("{0}{1}00{2}", (bInput ? TYPE_INPUT : TYPE_OUTPUT)
                                , strDataType, ((uAddr + nAddress).ToString("X")));
            }
            else
            {
                strRegisterId = String.Format("{0}{1}00{2}{3}", (bInput ? TYPE_INPUT : TYPE_OUTPUT)
                                , strDataType, ((uAddr + nAddress).ToString("X")), nBit.ToString("X"));
            }

            pHandle = 0;
            UInt32 uCode = CMotionAPI.ymcGetRegisterDataHandle(strRegisterId, ref pHandle);
            if (false == bTry)
            {
                if (false == m_pCommunicator.CheckingApi(uCode,
                                MethodBase.GetCurrentMethod().Name, AxisIndex, "After ymcGetRegisterDataHandle"))
                    return false;
            }
            else
            {
                return (uCode == CMotionAPI.MP_SUCCESS);
            }

            return true;
        }

        private void SetMonitoringRegister()
        {
            UInt32 pRegisterHandle = 0;
            if (false == GetRegisterHandle(false, TYPE_WORD, MONITORING_ADDRESS, -1, ref pRegisterHandle, true))
            {
                // 보드를 열고
                if (false == m_pCommunicator.OpenApi(true))
                    return;

                if (false == GetRegisterHandle(false, TYPE_WORD, MONITORING_ADDRESS, -1, ref pRegisterHandle))
                    return;
            }

            Int16[] arOutputData = new Int16[] { 1025 };
            UInt32 uCode = CMotionAPI.ymcSetRegisterData(pRegisterHandle, 1, arOutputData);
            if (false == m_pCommunicator.CheckingApi(uCode,
                MethodBase.GetCurrentMethod().Name, AxisIndex, "After ymcGetRegisterData"))
                return;
        }

        private void SetSpeedRatio(int nRatio)
        {
            UInt32 pRegisterHandle = 0;
            if (false == GetRegisterHandle(false, TYPE_WORD, SPEED_OVERRIDE_ADDRESS, -1, ref pRegisterHandle, true))
            {
                // 보드를 열고
                if (false == m_pCommunicator.OpenApi(true))
                    return;

                if (false == GetRegisterHandle(false, TYPE_WORD, SPEED_OVERRIDE_ADDRESS, -1, ref pRegisterHandle))
                    return;
            }

            UInt16[] arOutputData = new UInt16[] { (UInt16)(nRatio * 100) };
            UInt32 uCode = CMotionAPI.ymcSetRegisterData(pRegisterHandle, 1, arOutputData);
            if (false == m_pCommunicator.CheckingApi(uCode,
                MethodBase.GetCurrentMethod().Name, AxisIndex, "After ymcGetRegisterData"))
                return;
        }

        private void SetPotisionReferenceValue(bool bEnabled)
        {
            UInt32 pRegisterHandle = 0;
            if (false == GetRegisterHandle(false, TYPE_BIT, POSITION_REFERENCE_TYPE_ADDRESS, POSITION_REFERENCE_TYPE_BIT, ref pRegisterHandle, true))
            {
                // 보드를 열고
                if (false == m_pCommunicator.OpenApi(true))
                    return;

                if (false == GetRegisterHandle(false, TYPE_BIT, POSITION_REFERENCE_TYPE_ADDRESS, POSITION_REFERENCE_TYPE_BIT, ref pRegisterHandle))
                    return;
            }

            Int16 nBit = 0;
            if (bEnabled)
                nBit = 0x01;
            else
                nBit = 0x00;

            Int16[] arOutputData = new Int16[] { nBit };
            UInt32 uCode = CMotionAPI.ymcSetRegisterData(pRegisterHandle, 1, arOutputData);
            if (false == m_pCommunicator.CheckingApi(uCode,
                MethodBase.GetCurrentMethod().Name, AxisIndex, "After ymcGetRegisterData"))
                return;
        }

        private void ResetAlarm()
        {
            UInt32 pRegisterHandle = 0;
            if (false == GetRegisterHandle(false, TYPE_BIT, RUN_STATUS_ADDRESS, OUT_RUN_STATUS_CLEAR_ALARM_BIT, ref pRegisterHandle, true))
            {
                // 보드를 열고
                if (false == m_pCommunicator.OpenApi(true))
                    return;

                if (false == GetRegisterHandle(false, TYPE_BIT, RUN_STATUS_ADDRESS, OUT_RUN_STATUS_CLEAR_ALARM_BIT, ref pRegisterHandle))
                    return;
            }

            UInt32 uCode = CMotionAPI.ymcSetRegisterData(pRegisterHandle, 1, new Int16[] { 0x01 });
            if (false == m_pCommunicator.CheckingApi(uCode,
                MethodBase.GetCurrentMethod().Name, AxisIndex, "After ymcGetRegisterData"))
                return;

            Thread.Sleep(LITTLE_DELAY);

            uCode = CMotionAPI.ymcSetRegisterData(pRegisterHandle, 1, new Int16[] { 0x00 });
            if (false == m_pCommunicator.CheckingApi(uCode,
                MethodBase.GetCurrentMethod().Name, AxisIndex, "After ymcGetRegisterData"))
                return;
        }

        private bool NeedStop(bool bDirection)
        {
            if (m_bHwLimitNegative && false == bDirection)
                return true;

            if (m_bHwLimitPositive && bDirection)
                return true;

            return false;
        }
        #endregion </Internal Methods>
        #endregion </Methods>

        protected override bool GetHandle(int nAxis, ref UInt32 pAxisHandle)
        {
            return false;
        }
    }
}
