using System;
using System.Threading;
using System.Reflection;
using System.Collections.Generic;
using System.Collections.Concurrent;

using FrameOfSystem3.Controller.Motion.MechatroLinkAPI.References;
using FrameOfSystem3.Controller.Motion.MechatroLinkAPI.MechatroLinkDefines;

namespace FrameOfSystem3.Controller.Motion.MechatroLinkAPI.MechatroLinkAxis
{
    public abstract class MechatrolinkAxis : IMechatrolinkAxis
    {
        #region <Constructor>
        public MechatrolinkAxis() { }
        public MechatrolinkAxis(iCommunicator pComm, int nTargetIndex, int nAxisIndex, string strName)
        {
            Name = strName;
            AxisIndex = nAxisIndex;
            TargetIndex = nTargetIndex;

            m_pCommunicator = pComm;


            HomeCompletion = false;

            RegisterInputAddress = 0;
            RegisterOutputAddress = 0;
            GeneralInputAddress = 0;
            GeneralOutputAddress = 0;

            m_bUsable = true;

            m_thExecutingMotion = new Thread(new ThreadStart(ExecuteAction));
            m_thExecutingMotion.IsBackground = true;
            m_thExecutingMotion.Name = String.Format("{0}_MotionThread", Name);
        }
        #endregion </Constructor>

        #region <Fields>

        #region <Parameters>
        protected MCTL_PARAM_CONFIG m_stConfigParam;         // 모터 파라메터
        protected MCTL_PARAM_HOME m_stHomingParam;           // 홈 관련 파라메터
        protected MCTL_PARAM_SPEED m_stSpeedParam;           // 속도 관련 파라메터

        protected MCTL_PARAM_SPEED m_stHomingJoggingParam = new MCTL_PARAM_SPEED();
        protected MCTL_PARAM_SPEED m_stHomingEscapeParam = new MCTL_PARAM_SPEED();

        protected MCTL_PARAM_GANTRY m_stGantryParam = new MCTL_PARAM_GANTRY();
        #endregion </Parameters>

        #region <Flags>
        protected bool m_bMotionDone;
        protected bool m_bExecutingMotion;
        protected bool m_bReqStopHoming;

        #endregion </Flags>

        #region <StateFlag>
        protected bool m_bUsable;
        protected bool m_bHwLimitPositive;
        protected bool m_bHwLimitNegative;
        #endregion </StateFlag>

        #region <Motion>
        protected int m_nMotionStep;
        protected bool m_bMotionStepCompletion;
        #endregion </Motion>

        #region <Homing>
        protected int m_nHomingStep = 1000;
        protected int m_nHomingDelay;
        protected int m_nHomingStepResult = HOMING_RESULT_END;
        protected TickCounter_.TickCounter m_tickHomingDelay = new TickCounter_.TickCounter();
        #endregion </Homing>
        
        #region <Communicator>
        protected iCommunicator m_pCommunicator;
        #endregion </Communicator>

        #region <ETC>
        protected TickCounter_.TickCounter m_tickHomingTimeoutChecker = new TickCounter_.TickCounter();
        #endregion </ETC>

        #region <Gantry>
        protected IMechatrolinkAxis m_pSlaveAxis = null;
        protected UInt32[] m_arGearedAxes = new UInt32[2];
        #endregion </Gantry>

        #region <Private Fields>
        private AutoResetEvent _are = new AutoResetEvent(false);
        private Thread m_thExecutingMotion = null;
        private ConcurrentDictionary<AXIS_COMMAND_TYPE, MCTL_MOTION_PARAM> m_dicAction = new ConcurrentDictionary<AXIS_COMMAND_TYPE, MCTL_MOTION_PARAM>();
        private bool m_bClosingRequested = false;
        private UInt32 m_pMotionHandle;
        private int m_nExecuteSeq;
        #endregion </Private Fields>

        #region <Enums>
        enum EXECUTING_STEP
        {
            OPEN_API = 0,
            GET_HANDLE,
            EXECUTING,
        }
        #endregion </Enums>

        #region <Constants>
        protected static int HOMING_RESULT_INIT = 0;
        protected static int HOMING_RESULT_WORKING = 1;
        protected static int HOMING_RESULT_END = 2;
        protected static int HOMING_RESULT_ERROR = 3;
        #endregion </Constants>
        #endregion </Fields>

        #region <Properties>

        #region <Informations>
        public int TargetIndex { get; protected set; }

        public int AxisIndex { get; protected set; }       // 1부터 시작

        public string Name { get; protected set; }

        public UInt32 RegisterInputAddress { get; set; }

        public UInt32 RegisterOutputAddress { get; set; }

        public UInt32 GeneralInputAddress { get; set; }

        public UInt32 GeneralOutputAddress { get; set; }

        public bool ServoMotor { get; set; }
        #endregion </Informations>

        #region <State>
        public double ActualPosition { get; protected set; }

        public double CommandPosition { get; protected set; }

        public int State { get; protected set; }

        public bool GantryState { get; protected set; }

        public bool MotionDone
        {
            get
            {
                // Motion 명령이 실행되고 나서 MotionDone이 False로 바뀌는 시점이 오래걸리기 때문에(10ms 이상)
                // Motion 명령과 MotionDone을 플래그로 나누어서 조합하여 사용한다.
                if (m_bMotionDone && (false == m_bExecutingMotion))
                    return true;

                return false;
            }
        }

        public bool ServoState { get; protected set; }

        public bool HWLimitPositive
        {
            get { return m_bHwLimitPositive; }
        }

        public bool HWLimitNegative
        {
            get { return m_bHwLimitNegative; }
        }

        public bool AlarmState { get; protected set; }

        public bool HomeCompletion { get; protected set; }
        public bool HomeTimeout { get; protected set; }

        public int MasterAxis { get; set; }

        public int SlaveAxis { get; set; }

        public bool IsSlave
        {
            get
            {
                return (AxisIndex == (SlaveAxis + 1));
            }
        }
        #endregion </State>

        public UInt32 AxisHandle { get; set; }
        #endregion </Properties>

        #region <Methods>

        #region <External Interfaces>

        #region <Close>
        public virtual void Close()
        {
            //m_dicAction.Clear();

            _are.Set();
            m_bClosingRequested = true;
            //DestoryObject();
        }

        #endregion </Close>

        #region <State>
        public virtual void UpdateAxisStatus(ref double dActualPos, ref double dCommandPos, ref int nState)
        {
            //if (false == m_bUsable)
            //    return;

            // Actual Position
            ActualPosition = GetActualPosition();
            dActualPos = ActualPosition;

            // Command Position
            CommandPosition = GetCommandPosition();
            dCommandPos = CommandPosition;

            // Servo State
            ServoState = GetServoState();

            AlarmState = GetAlarmState();

            GetHWLimitState(ref m_bHwLimitPositive, ref m_bHwLimitNegative);

            m_bMotionDone = GetMotionCompletion();

            int nTempState = ((ServoState ? 1 : 0) << (int)MOTOR_STATE.MOTOR_ON);
            nTempState |= ((AlarmState ? 1 : 0) << (int)MOTOR_STATE.ALARM);
            nTempState |= ((m_bHwLimitPositive ? 1 : 0) << (int)MOTOR_STATE.LIMIT_POSITIVE);
            nTempState |= ((m_bHwLimitNegative ? 1 : 0) << (int)MOTOR_STATE.LIMIT_NEGATIVE);
            nTempState |= ((MotionDone ? 1 : 0) << (int)MOTOR_STATE.MOTION_DONE);
            nTempState |= ((HomeCompletion ? 1 : 0) << (int)MOTOR_STATE.HOME_DONE);
            nTempState |= ((HomeTimeout ? 1 : 0) << (int)MOTOR_STATE.HOME_TIMEOUT);

            nState = nTempState;

            GantryState = GetGearedState(m_arGearedAxes);
        }

        //public virtual bool GetGantryState(int[] arAxes)
        //{
        //    return GantryState;// GetGearedState(arAxes);
        //}

        public virtual bool IsGantrySettingComplete()
        {
            if (MasterAxis < 0 || SlaveAxis < 0)
                return false;

            return true;
        }
        #endregion  </State>

        #region <Virtual Methods>

        #region <Axis Commands>
        public virtual void MotorOnOff(bool bEnabled)
        {
            if (bEnabled && ServoState)
                return;

            AXIS_COMMAND_TYPE enCommand = bEnabled ?
                AXIS_COMMAND_TYPE.SERVO_ON : AXIS_COMMAND_TYPE.SERVO_OFF;
            if (m_dicAction.ContainsKey(enCommand))
                return;

            SetAction(enCommand, null);
            //ServoControl(bEnabled);
        }

        public virtual void DoReset()
        {
            AXIS_COMMAND_TYPE enCommand = AXIS_COMMAND_TYPE.RESET;
            if (m_dicAction.ContainsKey(enCommand))
            {
                return;
            }

            SetAction(enCommand, null);
            //ClearAlarm();
        }

        public virtual void SetPosition(double dPosition)
        {
            AXIS_COMMAND_TYPE enCommand = AXIS_COMMAND_TYPE.SET_POSITION;
            if (m_dicAction.ContainsKey(enCommand))
            {
                return;
            }

            MCTL_MOTION_PARAM param = new MCTL_MOTION_PARAM();
            param.TargetPosition = dPosition;
            
            SetAction(enCommand, param);
            //DefinePosition(dPosition);
        }

        public virtual void SetGantry(ref UInt32[] arGantryHandles, UInt32[] arAxes, bool bEnabled)
        {
            //AXIS_COMMAND_TYPE enCommand = bEnabled ?
            //    AXIS_COMMAND_TYPE.ENABLE_GEAR : AXIS_COMMAND_TYPE.DISABLE_GEAR;
            //if (m_dicAction.ContainsKey(enCommand))
            //    return;

            //MCTL_MOTION_PARAM param = new MCTL_MOTION_PARAM();
            //param.GearAxes = arAxes;

            //SetAction(enCommand, param);

            SetGearedAxis(ref arGantryHandles, arAxes, bEnabled);
        }
        #endregion </Axis Commands>

        #region <Motion Commands>
        public virtual void Stop(bool bEmergency)
        {
            if (false == ServoState)
                return;

            AXIS_COMMAND_TYPE enCommand = AXIS_COMMAND_TYPE.STOP;
            if (m_dicAction.ContainsKey(enCommand))
            {
                return;
            }

            MCTL_MOTION_PARAM param = new MCTL_MOTION_PARAM();
            param.Emergency = bEmergency;
            
            m_bReqStopHoming = true;
            
            SetAction(enCommand, param);

            if (1000 != m_nHomingStep || HOMING_RESULT_END != m_nHomingStepResult)
            {
                m_nHomingStep = 0;
                m_nHomingDelay = 0;
                HomeCompletion = false;
            }

            //StopMotion(bEmergency);
        }

        public virtual void Move(double dPosition, bool bAbsolute)
        {
            if (bAbsolute && (CommandPosition == dPosition))
            {
                m_bExecutingMotion = false;
                return;
            }

            AXIS_COMMAND_TYPE enCommand = AXIS_COMMAND_TYPE.MOVE_MOTION;
            if (m_dicAction.ContainsKey(enCommand))
                return;

            m_bExecutingMotion = true;
            m_nMotionStep = 0;
            
            MCTL_MOTION_PARAM param = new MCTL_MOTION_PARAM();
            param.Position = dPosition;
            param.Absolutely = bAbsolute;

            SetAction(enCommand, param);


            //ThreadPool.QueueUserWorkItem(x => MoveMotion(dPosition, bAbsolute, true));
        }

        public virtual void OverridePosition(double dPosition)
        {

        }

        public virtual void OverrideVelocity(double dVelocity)
        {
            AXIS_COMMAND_TYPE enCommand = AXIS_COMMAND_TYPE.OVERRIDE_VELOCITY;
            if (m_dicAction.ContainsKey(enCommand))
            {
                return;
            }

            m_bExecutingMotion = true;

            MCTL_MOTION_PARAM param = new MCTL_MOTION_PARAM();
            param.Velocity = dVelocity;

            SetAction(enCommand, param);
            //OverrideSpeed(dVelocity);
        }

        public virtual void MoveAtSpeed(bool bDirection)
        {
            AXIS_COMMAND_TYPE enCommand = AXIS_COMMAND_TYPE.MOVE_JOG;
            if (m_dicAction.ContainsKey(enCommand))
            {
                return;
            }

            m_bExecutingMotion = true;

            MCTL_MOTION_PARAM param = new MCTL_MOTION_PARAM();
            param.Direction = bDirection;

            SetAction(enCommand, param);
            //MoveJog(bDirection);
        }
        #endregion </Motion Commands>

        #region <Params>
        public virtual void SetMotorConfig(MCTL_PARAM_CONFIG stConfig)
        {
            m_stConfigParam = stConfig;
        }

        public virtual void SetSpeedParam(MCTL_PARAM_SPEED stSpeed)
        {
            m_stSpeedParam = stSpeed;
        }

        public virtual void SetHomingParam(MCTL_PARAM_HOME stHomeParam)
        {
            m_stHomingParam = stHomeParam;

            m_stHomingJoggingParam.dblVelocity = stHomeParam.dblHomeVelocity;
            m_stHomingEscapeParam.dblVelocity = stHomeParam.dblHomeSecondVelocity;
        }

        public virtual void SetGantryParam(MCTL_PARAM_GANTRY stGantryParam)
        {
            m_stGantryParam = stGantryParam;
        }

        public virtual MCTL_PARAM_GANTRY GetGantryParam()
        {
            return m_stGantryParam;
        }
        #endregion </Params>

        #region <Homing>
        public virtual bool MoveToHome(ref int nSeq, ref int nDelay)
        {
            //if (TargetIndex == 10)
            //{
            //    System.Console.WriteLine("Homing Start");
            //}

            AXIS_COMMAND_TYPE enCommand = AXIS_COMMAND_TYPE.HOMING;

            // 명령이 들어가기 전에 또 호출 들어오면 Return False
            if (m_dicAction.ContainsKey(enCommand))
            {
                return false;
            }

            //if (0 != nSeq && nSeq == m_nHomingStep)
            //    return false;
            nDelay = 0;
            if (0 == nSeq && HOMING_RESULT_END == m_nHomingStepResult)
            {
                m_nHomingStep = 0;
                //m_nHomingDelay = 0;
                HomeCompletion = false;
                m_nHomingStepResult = HOMING_RESULT_INIT;
            }
            else
            {
                nSeq = m_nHomingStep;
                //nDelay = m_nHomingDelay;
            }

            //MCTL_MOTION_PARAM param = new MCTL_MOTION_PARAM();
            //m_nHomingStep = nSeq;
            //m_nHomingDelay = nDelay;
            SetAction(enCommand, null);

            //while (m_dicAction.ContainsKey(enCommand))
            //{
            //    Thread.Sleep(1);

            //    if (m_bReqStopHoming)
            //        return true;
            //}

            return (m_nHomingStepResult == HOMING_RESULT_END);
        }
        #endregion

        #region <ETC>
        public virtual void InitAxis() 
        {
            InitMotionFlags();

            MasterAxis = -1;
            SlaveAxis = -1;

            m_pMotionHandle = 0;
            m_nExecuteSeq = 0;

            m_thExecutingMotion.Start();
        }

        public virtual void SetGantryAxesIndex()
        {
            m_arGearedAxes[0] = (UInt32)MasterAxis;
            m_arGearedAxes[1] = (UInt32)SlaveAxis;
        }

        public virtual void SetSlaveAxis(IMechatrolinkAxis pAxis)
        {
            m_pSlaveAxis = pAxis;
        }
        #endregion </ETC>

        #endregion </Virtual Methods>

        #endregion </External Interfaces>

        #region <Inhert Methods>

        #region <Destructor>
        protected abstract void DestoryObject(ref UInt32 pAxisHandle);
        #endregion </Destructor>

        #region <State>
        protected abstract double GetActualPosition();

        protected abstract double GetCommandPosition();

        protected abstract bool GetMotionCompletion();

        protected abstract bool GetServoState();

        protected abstract bool GetAlarmState();

        protected abstract void GetHWLimitState(ref bool bLimitPos, ref bool bLimitNeg);

        protected abstract bool GetGearedState(UInt32[] arAxes);
        #endregion </State>

        #region <Axis Commands>
        protected abstract bool ServoControl(ref UInt32 pHandle, bool bEnabled);

        protected abstract bool ClearAlarm(ref UInt32 pHandle);

        protected abstract bool DefinePosition(ref UInt32 pHandle, double dPosition);

        protected abstract void SetGearedAxis(ref UInt32[] arGantryHandles, UInt32[] arAxes, bool bEnabled);
        #endregion </Axis Commands>

        #region <Motion Commands>
        protected abstract bool MoveMotion(ref UInt32 pHandle, ref int nMotionStep, double dPosition, bool bAbsolute, bool bCheckingMotion);

        protected abstract bool MoveJog(ref UInt32 pHandle, bool bDirection, bool bUsingMultiJogging = true);

        protected abstract bool OverrideSpeed(ref UInt32 pHandle, double dVelocity);

        protected abstract bool StopMotion(ref UInt32 pHandle, bool bEmergency);

        protected abstract EXECUTING_COMMAND_TYPE GetExecutingCommand();
        #endregion </Motion Commands>

        #region <Homing>
        public abstract int CycleHoming(ref UInt32 pHandle, ref int nSeq, ref int nDelay);
        #endregion </Homing>

        #region <Handling>
        protected abstract bool GetHandle(int nAxis, ref UInt32 pAxisHandle);
        
        protected virtual bool DeclareAxis(int nAxis, ref UInt32 pAxisHandle)
        {
            return true;
        }
        #endregion </Handling>

        #region <ETC>
        protected virtual void InitMotionFlags()
        {
            m_bMotionDone = true;
            m_bExecutingMotion = false;
        }

        protected virtual Int16 GetProfileType(MECHATROLINK_SPEED_PATTERN enSpeedPattern)
        {
            switch (enSpeedPattern)
            {
                //case SPEED_PATTERN.S_CURVE:
                case MECHATROLINK_SPEED_PATTERN.TRAPEZODIAL:
                    return (Int16)CMotionAPI.ApiDefs.FTYPE_NOTIONG;   // 필터 없음
                //case SPEED_PATTERN.ANTI_VABRATION:
                //return (Int16)CMotionAPI.ApiDefs.FTYPE_EXP;       // 지수 필터?
                default:
                    return (Int16)CMotionAPI.ApiDefs.FTYPE_S_CURVE;   // 이동 평균 필터(S커브)
                //return (Int16)CMotionAPI.ApiDefs.FTYPE_KEEP;      // 현재 설정 유지
            }

        }
        #endregion </ETC>

        #region <IMechatrolinkAxis>
        public bool GetServoReady()
        {
            return ServoState;
        }

        public bool GetMotionReady()
        {
            return MotionDone;
        }

        public void SetServoState(bool bEnabled)
        {
            MotorOnOff(bEnabled);
        }

        public void SetHomePosition(double dPosition, bool bEnable)
        {
            SetPosition(dPosition);
            HomeCompletion = bEnable;
        }

        public void MoveToHomeOffset(MCTL_PARAM_HOME stParam)
        {
            MCTL_PARAM_SPEED stSpeed = new MCTL_PARAM_SPEED();
            stSpeed.dblVelocity  = m_stHomingParam.dblHomeVelocity;
            stSpeed.dblAccelTime = m_stHomingParam.dblHomeAccelTime;
            stSpeed.dblDecelTime = m_stHomingParam.dblHomeDecelTime;
            SetSpeedParam(stSpeed);
            Move(m_stHomingParam.dblHomeOffset, false);
        }

        public void ResetAxis()
        {
            DoReset();
        }
        #endregion </IMechatrolinkAxis>

        #endregion </Inhert Methods>

        #region <Internal Interfaces>

        #region <Executing Action>
        private void SetAction(AXIS_COMMAND_TYPE enType, MCTL_MOTION_PARAM param)
        {
            m_dicAction.TryAdd(enType, param);
            _are.Set();
        }

        private void ExecuteAction()
        {
            while (true)
            {
                Thread.Sleep(1);

                if (m_pCommunicator == null || false == m_pCommunicator.GetConnectionState())
                    continue;

                if (m_nExecuteSeq >= (int)EXECUTING_STEP.EXECUTING)
                {
                    if (m_dicAction.Count <= 0)
                    {
                        _are.WaitOne();
                        //continue;
                    }
                }

                if (m_bClosingRequested)
                {
                    m_dicAction.Clear();

                    DestoryObject(ref m_pMotionHandle);
                    
                    break;
                }

                switch (m_nExecuteSeq)
                {
                    case (int)EXECUTING_STEP.OPEN_API:
                        if (m_pCommunicator.OpenApi(true))
                        {
                            ++m_nExecuteSeq;
                        }
                        break;
                    case (int)EXECUTING_STEP.GET_HANDLE:
                        if (DeclareAxis(AxisIndex, ref m_pMotionHandle))
                        {
                            ++m_nExecuteSeq;
                        }
                        break;
                    case (int)EXECUTING_STEP.EXECUTING:
                        {
                            if (false == m_bUsable)
                            {
                                m_dicAction.Clear();
                                break;
                            }

                            if (m_dicAction.ContainsKey(AXIS_COMMAND_TYPE.STOP))
                            {
                                MCTL_MOTION_PARAM param;
                                m_dicAction.TryGetValue(AXIS_COMMAND_TYPE.STOP, out param);
                                m_bReqStopHoming = true;
                                StopMotion(ref m_pMotionHandle, param.Emergency);

                                m_dicAction.Clear();
                            }
                            else
                            {
                                AXIS_COMMAND_TYPE enCommand = AXIS_COMMAND_TYPE.STOP;
                                foreach (KeyValuePair<AXIS_COMMAND_TYPE, MCTL_MOTION_PARAM> kvp in m_dicAction)
                                {
                                    enCommand = kvp.Key;
                                    break;
                                }

                                MCTL_MOTION_PARAM param;
                                m_dicAction.TryGetValue(enCommand, out param);

                                bool bExecutingCommand = true;
                                bool bSucceed = true;
                                switch (enCommand)
                                {
                                    case AXIS_COMMAND_TYPE.SERVO_ON:
                                        bSucceed = ServoControl(ref m_pMotionHandle, true);
                                        break;
                                    case AXIS_COMMAND_TYPE.SERVO_OFF:
                                        bSucceed = ServoControl(ref m_pMotionHandle, false);
                                        break;
                                    case AXIS_COMMAND_TYPE.RESET:
                                        bSucceed = ClearAlarm(ref m_pMotionHandle);
                                        break;
                                    case AXIS_COMMAND_TYPE.SET_POSITION:
                                        bSucceed = DefinePosition(ref m_pMotionHandle, param.TargetPosition);
                                        break;
                                    case AXIS_COMMAND_TYPE.MOVE_MOTION:
                                        m_bMotionStepCompletion = MoveMotion(ref m_pMotionHandle, ref m_nMotionStep, param.Position, param.Absolutely, true);
                                        bExecutingCommand = m_bMotionStepCompletion;
                                        break;
                                    case AXIS_COMMAND_TYPE.MOVE_JOG:
                                        bSucceed = MoveJog(ref m_pMotionHandle, param.Direction);
                                        break;
                                    case AXIS_COMMAND_TYPE.OVERRIDE_VELOCITY:
                                        bSucceed = OverrideSpeed(ref m_pMotionHandle, param.Velocity);
                                        break;
                                    //case AXIS_COMMAND_TYPE.ENABLE_GEAR:
                                    //    SetGearedAxis(ref m_pMotionHandle, param.GearAxes, true);
                                    //    break;
                                    //case AXIS_COMMAND_TYPE.DISABLE_GEAR:
                                    //    SetGearedAxis(ref m_pMotionHandle, param.GearAxes, false);
                                    //    break;
                                    case AXIS_COMMAND_TYPE.HOMING:
                                        m_nHomingStepResult
                                            = CycleHoming(ref m_pMotionHandle, ref m_nHomingStep, ref m_nHomingDelay);

                                        if (HOMING_RESULT_END.Equals(m_nHomingStepResult) ||
                                            HOMING_RESULT_ERROR.Equals(m_nHomingStepResult))
                                        {
                                            m_dicAction.TryRemove(enCommand, out param);
                                        }
                                            //bExecutingCommand = false;

                                        break;
                                    default:
                                        bExecutingCommand = false;
                                        break;
                                }

                                //if (false == bSucceed)
                                //{
                                //    m_nExecuteSeq = (int)EXECUTING_STEP.OPEN_API;
                                //    break;
                                //}

                                if (bExecutingCommand ||
                                    ((enCommand == AXIS_COMMAND_TYPE.MOVE_MOTION ||
                                    enCommand == AXIS_COMMAND_TYPE.MOVE_JOG ||
                                    enCommand == AXIS_COMMAND_TYPE.OVERRIDE_VELOCITY)
                                    && false == ServoState))
                                {
                                    m_dicAction.TryRemove(enCommand, out param);

                                    //Thread.Sleep(50);
                                }
                            }
                        }
                        break;
                }
            }
        }
        #endregion </Executing Action> 
        
        #endregion </Internal Interfaces>

        #endregion </Methods>
    }
}
