using System;

namespace FrameOfSystem3.Controller.Motion.MechatroLinkAPI.MechatroLinkDefines
{
    #region <Constants>
    public static class MechatrolinkDefines
    {
        public const UInt32 SERVO_RUNNING = 0x02;

        public const UInt32 LIMIT_POS = 0x04;
        public const UInt32 LIMIT_NEG = 0x08;

        #region <Ini>
        public const string SECTION_COUNTS = "AXIS_COUNT";
        public const string KEY_COUNT_SERVO_AXIS = "COUNT_SERVO_AXIS";
        public const string KEY_COUNT_STEPPING_AXIS = "COUNT_STEPPING_AXIS";

        public const string SECTION_SERVO_AXIS_INFO = "SERVO_AXIS_INFO";
        public const string KEY_SERVO_INPUT_ADDRESS = "SERVO_I_ADDRESS";
        public const string KEY_SERVO_OUTPUT_ADDRESS = "SERVO_O_ADDRESS";

        public const string SECTION_STEPPING_AXIS_INFO = "STEPPING_AXIS_INFO";
        public const string KEY_STEPPING_INPUT_ADDRESS = "STEPPING_I_ADDRESS";
        public const string KEY_STEPPING_OUTPUT_ADDRESS = "STEPPING_O_ADDRESS";
        public const string KEY_AXIS_GENERAL_INPUT_ADDRESS = "GENERAL_INPUT_ADDRESS";
        public const string KEY_AXIS_GENERAL_OUTPUT_ADDRESS = "GENERAL_OUTPUT_ADDRESS";
        #endregion </Ini>

        #region <Register Offset>

        #region <Motion Command Address>
        //public const int INPUT_REGISTER_ADDRESS_MOTOR_USABLE = 0x00;

        public const int OUTPUT_REGISTER_ADDRESS_USING_C_PHASE = 4;
        public const int OUTPUT_REGISTER_ADDRESS_MOTION_COMMAND = 8;
        public const int OUTPUT_REGISTER_ADDRESS_BASE_POSITION = 72;
        #endregion </Motion Command Address>

        #endregion </Register Offset>
    }
    #endregion </Constants>

    #region <Interfaces>
    public interface iCommunicator
    {
        bool GetConnectionState();

        bool OpenApi(bool bReOpen = false);

        bool CheckingApi(UInt32 uCode, string strFuncName, int nAxis, string strMessage = "");

        //void RequestEnableGantry(int[] arAxes);
    }

    public interface IMechatrolinkAxis
    {
        bool GetServoReady();

        bool GetMotionReady();

        void ResetAxis();

        void SetServoState(bool bEnabled);

        void SetHomePosition(double dPosition, bool bEnable);

        void MoveToHomeOffset(MCTL_PARAM_HOME stParam);
    }
    #endregion </Interfaces>

    #region <Enums>
    public enum CONTROLLER_COMMAND_TYPE
    {
        INIT_CONTROLLER = 0,
        EXIT_CONTROLLER,
        SET_GANTRY,
    }

    public enum MOTOR_STATE
    {
        MOTOR_ON = 0,
        ALARM = 1,
        LIMIT_POSITIVE = 2,
        LIMIT_NEGATIVE = 3,
        SWLIMIT_POSITIVE = 4,
        SWLIMIT_NEGATIVE = 5,
        HOME_SWITCH = 6,
        HOME_DONE = 16,
        HOME_TIMEOUT = 17,
        MOTION_DONE = 18,
        MOTION_TIMEOUT = 19,
    }

    public enum MECHATROLINK_SPEED_PATTERN
    {
        //CONSTANT = 0,
        TRAPEZODIAL = 1,
        S_CURVE = 2,
        //ANTI_VABRATION = 3,
    }

    public enum EXECUTING_COMMAND_TYPE
    {
        UNKNOWN = -1,
        STOP = 0,
        MOVE,
        JOG,
        HOME
    }

    public enum MOTION_COMMAND_MODE_TYPE
    {
        MOTION_COMMAND_STOP = 0x00,
        MOTION_COMMAND_MOVE = 0x01,
        MOTION_COMMAND_HOME = 0x03,
        MOTION_COMMAND_JOG = 0x07
    }

    public enum MCTL_HOME_TYPE
    {
        HMETHOD_DEC1_C = 0,                        	// 0: DEC1 + phase-C pulse method
        HMETHOD_ZERO = 1,                          	// 1: ZERO signal method
        HMETHOD_DEC1_ZERO = 2,                     	// 2: DEC1 + ZERO signal method
        HMETHOD_C = 3,                            	// 3: Phase-C pulse method
        HMETHOD_DEC2_ZERO = 4,                    	// 4: DEC 2 + ZERO signal method
        HMETHOD_DEC1_L_ZERO = 5,                  	// 5: DEC1 + LMT + ZERO signal method
        HMETHOD_DEC2_C = 6,                     	// 6: DEC2 + phase-C pulse method
        HMETHOD_DEC1_L_C = 7,                    	// 7: DEC 1 + LMT + phase-C pulse method
        HMETHOD_C_ONLY = 11,                    	// 11: C Pulse Only
        HMETHOD_POT_C = 12,                      	// 12: POT & C Pulse
        HMETHOD_POT_ONLY = 13,                   	// 13: POT Only
        HMETHOD_HOMELS_C = 14,                    	// 14: Home LS C Pulse
        HMETHOD_HOMELS_ONLY = 15,                 	// 15: Home LS Only
        HMETHOD_NOT_C = 16,                        	// 16: NOT & C Pulse
        HMETHOD_NOT_ONLY = 17,                    	// 17: NOT Only
        HMETHOD_INPUT_C = 18,                     	// 18: Input & C Pulse
        HMETHOD_INPUT_ONLY = 19,                 	// 19: Input Only

        HMETHOD_POT_C2,                      	    // 20: JOG POT & C Pulse Only
        HMETHOD_NOT_C2,                      	    // 20: JOG NOT & C Pulse Only
    }

    public enum AXIS_COMMAND_TYPE
    {
        STOP = 0,
        SERVO_ON,
        SERVO_OFF,
        RESET,
        SET_POSITION,
        MOVE_MOTION,
        MOVE_JOG,
        OVERRIDE_VELOCITY,
        ENABLE_GEAR,
        DISABLE_GEAR,
        HOMING,
    }
    #endregion </Enums>

    #region <Structs>
    #endregion </Structs>

    #region <Parameter classes>

    public class MCTL_PARAM_CONFIG
    {
        #region <Fields>
        //public MCTL_MOTOR_INPUTMODE enInMode;        // 입력 모드
        //public MCTL_MOTOR_OUTPUTMODE enOutMode;      // 출력 모드
        //public bool bInputDirection;            // INPUT MODE의 엔코더 값의 방향의 반전 유무
        //public double dblInOutRatio;            // INPUT과 OUTPUT의 분해능
        //public double dblUnitDistance;          // 거리 당 펄스 수, Du = Pr(1회전 당 펄스 수)/Lr(1회전 당 거리)
        public bool bAlarmLogic;                // 알람 로직에 대한 설정, FALSE : A(NO), TRUE : B(NC)
        public bool bAlarmStopMode;             // 정지 모드, FALSE : 즉시정지, TRUE: 감속 후 정지

        //public bool bEZLogic;                   // Z상 로직에 대한 설정, FALSE : A(NO), TRUE : B(NC)

        //public bool bUseInposition;             // INPOSITION 사용 유무에 대한 설정, 사용 시 Command 출력이 완료되더라도 INP신호 ON 전에는 미완료
        //public bool bInpositionLogic;           // INPOSITION 로직에 대한 설정, FALSE : A(NO), TRUE : B(NC)

        //public MOTION_TYPE enMotorApplicationType;        // Linear or Circular 인지 설정.

        //public bool bCommandDirection;          // 커맨드 방향, FALSE : No Invert, TRUE : Invert
        //public bool bEncoderDirection;          // 엔코더 방향, FALSE : No Invert, TRUE : Invert

        public bool bUseHWLimitPositive;
        public bool bUseHWLimitNegative;
        public bool bHWLimitLogicPositive;
        public bool bHWLimitLogicNegative;
        public bool bHWLimitStopModePositive;       // FALSE(DECEL), TRUE(EMERGENCY)
        public bool bHWLimitStopModeNegative;       // FALSE(DECEL), TRUE(EMERGENCY)

        public bool bUseSWLimitPositive;
        public bool bUseSWLimitNegative;
        public bool bSWLimitStopModePositive;       // FALSE(DECEL), TRUE(EMERGENCY)
        public bool bSWLimitStopModeNegative;       // FALSE(DECEL), TRUE(EMERGENCY)
        public double dblSWLimitPositionPositive;
        public double dblSWLimitPositionNegative;

        #endregion </Fields>

        public MCTL_PARAM_CONFIG()
        {

        }
    }

    public class MCTL_PARAM_HOME
    {
        #region <Fields>
        //public bool bHomeLogic;                  // 홈의 로직을 설정, FALSE : A(NO), TRUE : B(NC)
        public bool bHomeDirection;                 // 홈의 방향, FALSE : - , TRUE : +

        public double dblVelocity;

        public double dblHomeVelocity;
        public double dblHomeAccelTime;
        public double dblHomeDecelTime;
        public double dblHomeAcceleration;
        public double dblHomeDeceleration;

        //public bool bAlwaysHomeEnd;
        public MCTL_HOME_TYPE enHomeMode;
        //public int nEZCount;
        public double dblEscDist;                       // 원점 탈출 거리
        public double dblHomeOffset;                    // 원점 복귀 후 Offset 값
        public double dblHomeBasePosition;        // Homing 후 지정할 포지션

        public MECHATROLINK_SPEED_PATTERN enHomeSpeedPattern;     // 홈 속도 모드
        public double dblHomeSecondVelocity;              // 홈 역방향 속도
        //public double dblHomeJerkAccelration;            // 모터 가속 Jerk
        //public double dblHomeJerkDecelration;            // 모터 감속 Jerk
        public uint uHomeTimeout;
        #endregion </Fields>

        public MCTL_PARAM_HOME()
        {

        }
    }

    public class MCTL_PARAM_SPEED
    {
        #region <Fields>
        public double dblVelocity;       // 모터의 속도(mm/s)
        public double dblAccelTime;      // 모터 가속 시간(ms)
        public double dblDecelTime;      // 모터 감속 시간(ms)
        public double dblAcceleration;   // 모터 가속도
        public double dblDeceleration;   // 모터 감속도

        public MECHATROLINK_SPEED_PATTERN enSpeedPattern;  // 속도 모드
        public double dblMaxVelocity;                 // 모터 속도
        //public double dblJerkAccelration;           // 모터 가속 Jerk
        //public double dblJerkDecelration;           // 모터 감속 Jerk
        public uint uMotionTimeout;
        public uint uFilter;                //Mechatrolink 0.1ms
        #endregion </Fields>

        //#region <Properties>
        ////public string strSpeedPattern
        ////{
        ////    set
        ////    {
        ////        foreach (MECHATROLINK_SPEED_PATTERN spdPt in Enum.GetValues(typeof(MECHATROLINK_SPEED_PATTERN)))
        ////        {
        ////            if (value.Equals(spdPt.ToString()))
        ////            {
        ////                enSpeedPattern = spdPt;

        ////                return;
        ////            }
        ////        }

        ////        enSpeedPattern = MECHATROLINK_SPEED_PATTERN.CONSTANT;
        ////    }
        ////    get
        ////    {
        ////        return enSpeedPattern.ToString();
        ////    }
        ////}
        ////public double dblVelocity
        ////{
        ////    set
        ////    {
        ////        m_dblVelocity = value;

        ////        ConvertTimeToValue(m_dblVelocity
        ////            , m_dblAccelTime
        ////            , ref m_dblAcceleration);

        ////        ConvertTimeToValue(m_dblVelocity
        ////            , m_dblDecelTime
        ////            , ref m_dblDeceleration);
        ////    }
        ////    get { return m_dblVelocity; }
        ////}

        ////public double dblAccelTime
        ////{
        ////    set
        ////    {
        ////        m_dblAccelTime = value;

        ////        ConvertTimeToValue(m_dblVelocity
        ////            , m_dblAccelTime
        ////            , ref m_dblAcceleration);
        ////    }
        ////    get { return m_dblAccelTime; }
        ////}

        ////public double dblDecelTime
        ////{
        ////    set
        ////    {
        ////        m_dblDecelTime = value;

        ////        ConvertTimeToValue(m_dblVelocity
        ////            , m_dblDecelTime
        ////            , ref m_dblDeceleration);
        ////    }
        ////    get { return m_dblDecelTime; }
        ////}

        ////public double dblAcceleration
        ////{
        ////    get { return m_dblAcceleration; }
        ////}

        ////public double dblDeceleration
        ////{
        ////    get { return m_dblDeceleration; }
        ////}

        //#endregion </Properties>

        public MCTL_PARAM_SPEED()
        {

        }

        //#region <Private methods>
        //private static void ConvertTimeToValue(double dblVelocity, double dblTime, ref double dblResult)
        //{
        //    dblResult = dblVelocity / (dblTime * 0.001);
        //}

        //#endregion </Private methods>
    }

    public class MCTL_PARAM_GANTRY
    {
        public bool bEnableGantry;
        public bool bReverseSlaveDirection;
        public int nAxisOfMaster;
        public int nAxisOfSlave;

        public MCTL_PARAM_GANTRY()
        {

        }
    }

    public class MCTL_MOTION_PARAM
    {
        #region <AxisCommand>
        public double TargetPosition { get; set; }
        #endregion </AxisCommand>

        #region <Moving>
        public bool Direction { get; set; }
        
        public bool Absolutely { get; set; }

        public double Position { get; set; }
        #endregion </Moving>

        #region <Stopping>
        public bool Emergency { get; set; }
        #endregion </Stopping>

        #region <Homing>
        public int Seq { get; set; }
        
        public int Delay { get; set; }
        #endregion </Homing>

        #region <Overriding>
        public double Velocity { get; set; }
        #endregion </Overriding>

        #region <Gear>
        private int[] m_arGearAxes = new int[2];
        public int[] GearAxes
        {
            get
            {
                return m_arGearAxes;
            }
            set
            {
                Array.Copy(value, m_arGearAxes, m_arGearAxes.Length);
            }
        }
        #endregion </Gear>
    }
    #endregion </Parameter classes>
}