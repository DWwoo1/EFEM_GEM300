///-----------------------------------------------------------------------
///
/// Motion.cs -> MotionInterfaces.cs로 명칭 변경 2021.07.13. by hkyu [ADD]
///------------------------------------------------------------------------
///

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Concurrent;

// # 패턴 - 옵저버 #
//using FrameOfSystem3.Pattern_Observer;
using FrameOfSystem3.Controller.Motion.MotionOnly;

using TickCounter_;
using Alarm_;
using FrameOfSystem3.Controller;
using Define.DefineConstant;

namespace FrameOfSystem3.Controller.Motion
{
    extern alias RegisteredInstance;
    extern alias AlarmInstance;
    extern alias MotionInstance;

    /*
     * 2018.07.09 by yjlee [MOD] 모션 클래스 리펙토링
     * : 모션에 대한 클래스이다.
     * : 모터(축)를 관리하는 MotorItemManager 클래스, 모터(축)를 제어하는 MotionController 클래스, 모션 구동 상태를 확인하는 MotionChecker Class로 구성된다.
     * : 한 모터(축)는 하나의 아이템으로 취급되며 관리된다.
     */
    #region MOTION 관련 열거체
    public enum EN_MOTIONController
    {
        AJIN,
        COMIZOA,
        MEI,
        MKUNIT,
        RSA,
        MECHATROLINK,
    }

    /// <summary>
    /// 2020.04.22 by Thienvv [ADD] For MK-D12
    /// </summary>
    public enum EN_LIMIT_POS
    {
        POSITIVE,
        NEGATIVE
    }
    public enum EN_MOTOR_CONTENTOFSPEED
    {
        RUN,
        JOG_LOW,
        JOG_HIGH,
        MANUAL,
        CUSTOM_1,
        CUSTOM_2,
        MAXCOUNT
    }

    #endregion

    public class MotionInterfaces   // : subject
    {
        

        #region 상수
        private const uint c_uIntervalForCheckingLink   = 1000;     // 1000 ms 마다 모션 링크를 확인한다.
        private const uint c_uDelayForConnecting        = 15000;    // 네트워크 연결 딜레이
        private const uint c_uDelayForInitDriver        = 500;    // 드라이버와 모터의 초기화 딜레이
        private const int c_nDelayForSettingParameters  = 3;        // 각 파라미터 설정 딜레이
        #endregion

        #region 변수
        private MotionItemManager m_motionItemManager = new MotionItemManager();
        private MotionController m_motionController = null;        
        private Compensator m_CompensatorInstance = Compensator.GetInstance();
        private EN_MOTIONController m_enMotionControlType = EN_MOTIONController.MECHATROLINK ; // 2021.07.12 by hkyu // 2020.03.10 by Thienvv [ADD] UI for motion control type
        //private Log m_InstanceLog   = null;

        private bool m_bInit = false;
        private int m_nCountOfAxis = 0;

        private Dictionary<int, MotionItem> m_dicOfMotionItem                   = null;

        private Dictionary<int, bool> m_dicOfSkipLimitItemWhenHoming            = new Dictionary<int,bool>();

        // 2021.08.09 by hkyu [ADD]
        Config.ConfigMotion m_instanceMotionConfig = Config.ConfigMotion.GetInstance();

        #region 링크 연결 관련
        private TickCounter m_tcForWaitingForConnecting  = new TickCounter();
        private TickCounter m_tcForConnectionDelay      = new TickCounter();
        private TickCounter m_tcForIntervalOfChecking   = new TickCounter();
        private bool m_bTryingToConnect                 = false;
        private int m_nSeqNumberForConnection           = 0;
        #endregion

        #region 구동 관련
        private List<MovedMotion> m_listOfMovingMotion        = new List<MovedMotion>();
        private List<RepeatItem> m_listOfRepeatingItem        = new List<RepeatItem>();
        #endregion

        #region 상태 관련
        private List<bool[]> m_listOfArState                = new List<bool[]>();
        private List<EN_MOTION_STATUS> m_listOfMotionStatus = new List<EN_MOTION_STATUS>();

        
        // 2020.05.05 by Thienvv [DEL] MK-D12 does not use UVW
        //// 2019.08.21 by hkyu [ADD] UVW function - origin variable
        //public double oVx;
        //public double oVy;
        //public double oUx;
        //public double oUy;
        //public double oWx;
        //public double oWy;

        // 2020.05.05 by Thienvv [DEL] MK-D12 does not use UVW
        //// 2019.08.21 by hkyu [ADD] UVW function - X,Y,T absolute variable
        //private double absX;
        //private double absY;
        //private double absT;

        //private UVW_ORG OriginUVW;
        #endregion


        public const string TRUE = "TRUE";
        public const string FALSE = "FALSE";

        #region 모션 설정 관련 
        public const string LOGIC_ACTIVE_HIGH   = "ACTIVE_HIGH";
        public const string LOGIC_ACTIVE_LOW    = "ACTIVE_LOW";

        public const string STOPMODE_EMG    = "INSTANT_STOP";
        public const string STOPMODE_DECEL  = "DECELERATION_STOP";

        public const string DIRECTION_POSITIVE = "POSITIVE(+)";
        public const string DIRECITON_NEGATIVE = "NEGATIVE(-)";

        public const string INVERT_OFF      = "NO_INVERT";
        public const string INVERT_ON       = "INVERT";

        public const string MOTION_TOKEN_LIMIT  = "EL";

        public const string REPEAT_ENABLE = "REPEAT";
        public const string REPEAT_DISABLE = "STOP";
        public const string NONE = "NONE";
        public const string MOVE = "MOVE";
        public const string SETTING = "SETTING";
        public const string MOVE_MANUAL = "MANUAL";
        public const string MOVE_REPEAT = "REPEAT";
        public const string ACT_MANUAL = "MANUAL";
        #endregion

        /// <summary>
        /// 2019.04.30 by yjlee [ADD] 구조체 정보 수정
        /// </summary>
        public struct MOTOR_HOME_PARAMETER
        {
            #region 공용 함수
            /// <summary>
            /// 2019.05.06 by yjlee [ADD] 가속도 및 감속도를 계산한다. (Second to MilliSecond)
            /// </summary>
            private static void ConvertTimeToValue(double dblVelocity, double dblTime, ref double dblResult)
            {
                dblResult = dblVelocity / (dblTime * 0.001);
            }
            #endregion

            #region 변수
            private bool m_bHomeLogic;                  // 홈의 로직을 설정, FALSE : A(NO), TRUE : B(NC)
            private bool m_bHomeDirection;                 // 홈의 방향, FALSE : - , TRUE : +

            private double m_dblHomeVelocity;
            private double m_dblHomeAccelTime;
            private double m_dblHomeDecelTime;
            private double m_dblHomeAcceleration;
            private double m_dblHomeDeceleration;

            public bool bAlwaysHomeEnd;
            public EN_MOTOR_HOMEMODE enHomeMode;
            public int nEZCount;
            public double dblEscDist;                       // 원점 탈출 거리
            public double dblHomeOffset;                    // 원점 복귀 후 Offset 값
            public double dblHomeBasePosition;        // Homing 후 지정할 포지션

            public EN_SPEED_PATTERN enHomeSpeedPattern;     // 홈 속도 모드
            public double dblHomeSecondVelocity;              // 홈 역방향 속도
            public double dblHomeJerkAccelration;            // 모터 가속 Jerk
            public double dblHomeJerkDecelration;            // 모터 감속 Jerk
            public uint uHomeTimeout;
            #endregion

            #region 속성
            public string strHomeSpeedPattern
            {
                set
                {
                    foreach (EN_SPEED_PATTERN spdPt in Enum.GetValues(typeof(EN_SPEED_PATTERN)))
                    {
                        if (value.Equals(spdPt.ToString()))
                        {
                            enHomeSpeedPattern = spdPt;

                            break;
                        }
                    }
                }
                get
                {
                    return enHomeSpeedPattern.ToString();
                }
            }
            public int nHomeMode
            {
                set
                {
                    foreach (EN_MOTOR_HOMEMODE homeMode in Enum.GetValues(typeof(EN_MOTOR_HOMEMODE)))
                    {
                        if ((int)homeMode == value)
                        {
                            enHomeMode = homeMode;

                            break;
                        }
                    }
                }
                get
                {
                    return (int)enHomeMode;
                }
            }
            public string strHomeMode
            {
                set
                {
                    foreach (EN_MOTOR_HOMEMODE homeMode in Enum.GetValues(typeof(EN_MOTOR_HOMEMODE)))
                    {
                        if (value.Equals(homeMode.ToString()))
                        {
                            enHomeMode = homeMode;

                            break;
                        }
                    }
                }
                get
                {
                    return enHomeMode.ToString();
                }
            }
            public bool bHomeLogic
            {
                get { return m_bHomeLogic; }
            }
            public string strHomeLogic
            {
                set
                {
                    m_bHomeLogic = value.Equals(LOGIC_ACTIVE_LOW);
                }
                get
                {
                    //return m_bHomeLogic ? DefineClass.LOGIC_ACTIVE_LOW : DefineClass.LOGIC_ACTIVE_HIGH;
                    return m_bHomeLogic ? LOGIC_ACTIVE_LOW : LOGIC_ACTIVE_HIGH;
                }
            }
            public string strHomeDirection
            {
                set
                {
                    //m_bHomeDirection = value.Equals(DefineClass.DIRECTION_POSITIVE);
                    m_bHomeDirection = value.Equals(DIRECTION_POSITIVE);
                }
                //get { return m_bHomeDirection ? DefineClass.DIRECTION_POSITIVE : DefineClass.DIRECITON_NEGATIVE; }
                get { return m_bHomeDirection ? DIRECTION_POSITIVE : DIRECITON_NEGATIVE; }
            }
            public bool bHomeDirection
            {
                set
                {
                    m_bHomeDirection = value;
                }
                get
                {
                    return m_bHomeDirection;
                }
            }
            public double dblHomeVelocity
            {
                set
                {
                    m_dblHomeVelocity = value;

                    ConvertTimeToValue(m_dblHomeVelocity
                        , m_dblHomeAccelTime
                        , ref m_dblHomeAcceleration);

                    ConvertTimeToValue(m_dblHomeVelocity
                        , m_dblHomeDecelTime
                        , ref m_dblHomeDeceleration);
                }
                get { return m_dblHomeVelocity; }
            }
            public double dblHomeAccelTime
            {
                set
                {
                    m_dblHomeAccelTime = value;

                    ConvertTimeToValue(m_dblHomeVelocity
                        , m_dblHomeAccelTime
                        , ref m_dblHomeAcceleration);
                }
                get { return m_dblHomeAccelTime; }
            }
            public double dblHomeDecelTime
            {
                set
                {
                    m_dblHomeDecelTime = value;

                    ConvertTimeToValue(m_dblHomeVelocity
                        , m_dblHomeDecelTime
                        , ref m_dblHomeDeceleration);
                }
                get { return m_dblHomeDecelTime; }
            }
            public double dblHomeAcceleration
            {
                get { return m_dblHomeAcceleration; }
            }
            public double dblHomeDeceleration
            {
                get { return m_dblHomeDeceleration; }
            }
            #endregion
        }
        /// <summary>
        /// 2019.04.30 by yjlee [MOD] 구조체 정보 수정
        /// </summary>
        public struct MOTOR_SPEED_PARAMETER
        {
            #region 공용 함수
            /// <summary>
            /// 2019.05.06 by yjlee [ADD] 가속도 및 감속도를 계산한다. (Second to MilliSecond)
            /// </summary>
            private static void ConvertTimeToValue(double dblVelocity, double dblTime, ref double dblResult)
            {
                dblResult = dblVelocity / (dblTime * 0.001);
            }
            #endregion

            #region 변수
            private double m_dblVelocity;       // 모터의 속도(mm/s)
            private double m_dblAccelTime;      // 모터 가속 시간(ms)
            private double m_dblDecelTime;      // 모터 감속 시간(ms)
            private double m_dblAcceleration;   // 모터 가속도
            private double m_dblDeceleration;   // 모터 감속도

            public EN_SPEED_PATTERN enSpeedPattern;  // 속도 모드
            public double dblMaxVelocity;                 // 모터 속도
            public double dblJerkAccelration;           // 모터 가속 Jerk
            public double dblJerkDecelration;           // 모터 감속 Jerk
            public uint uMotionTimeout;
            public uint uFilter;                //Mechatrolink 0.1ms
            #endregion

            #region 속성
            public string strSpeedPattern
            {
                set
                {
                    foreach (EN_SPEED_PATTERN spdPt in Enum.GetValues(typeof(EN_SPEED_PATTERN)))
                    {
                        if (value.Equals(spdPt.ToString()))
                        {
                            enSpeedPattern = spdPt;

                            return;
                        }
                    }

                    enSpeedPattern = EN_SPEED_PATTERN.CONSTANT;
                }
                get
                {
                    return enSpeedPattern.ToString();
                }
            }
            public double dblVelocity
            {
                set
                {
                    m_dblVelocity = value;

                    ConvertTimeToValue(m_dblVelocity
                        , m_dblAccelTime
                        , ref m_dblAcceleration);

                    ConvertTimeToValue(m_dblVelocity
                        , m_dblDecelTime
                        , ref m_dblDeceleration);
                }
                get { return m_dblVelocity; }
            }
            public double dblAccelTime
            {
                set
                {
                    m_dblAccelTime = value;

                    ConvertTimeToValue(m_dblVelocity
                        , m_dblAccelTime
                        , ref m_dblAcceleration);
                }
                get { return m_dblAccelTime; }
            }
            public double dblDecelTime
            {
                set
                {
                    m_dblDecelTime = value;

                    ConvertTimeToValue(m_dblVelocity
                        , m_dblDecelTime
                        , ref m_dblDeceleration);
                }
                get { return m_dblDecelTime; }
            }
            public double dblAcceleration
            {
                get { return m_dblAcceleration; }
            }
            public double dblDeceleration
            {
                get { return m_dblDeceleration; }
            }
            #endregion
        }
        /// <summary>
        /// 2019.04.30 by yjlee [MOD] Gantry 관련 정보
        /// </summary>
        public struct MOTOR_GANTRY_PARAMETER
        {
            #region 변수
            private bool m_bMaster;
            private bool m_bSlave;
            private bool m_bSlaveInverse;
            private int m_nIndexOfMaster;
            private int m_nIndexOfSlave;
            #endregion

            #region 속성
            public bool bMaster { set { m_bMaster = value; } get { return m_bMaster; } }
            public bool bSlave { set { m_bSlave = value; } get { return m_bSlave; } }
            public bool bSlaveInverse { set { m_bSlaveInverse = value; } get { return m_bSlaveInverse; } }
            public int nIndexOfSlave { set { m_nIndexOfSlave = value; } get { return m_nIndexOfSlave; } }
            public int nIndexOfMaster { set { m_nIndexOfMaster = value; } get { return m_nIndexOfMaster; } }
            #endregion
        }
        public struct MOTOR_CONFIG_PARAMETER
        {
            #region 변수
            public EN_MOTOR_INMODE enInMode;        // 입력 모드   dblSWLimitPositionPositive
            public EN_MOTOR_OUTMODE enOutMode;      // 출력 모드
            public bool bInputDirection;            // INPUT MODE의 엔코더 값의 방향의 반전 유무
            public double dblInOutRatio;            // INPUT과 OUTPUT의 분해능
            public double dblUnitDistance;          // 거리 당 펄스 수, Du = Pr(1회전 당 펄스 수)/Lr(1회전 당 거리)
            public bool bAlarmLogic;                // 알람 로직에 대한 설정, FALSE : A(NO), TRUE : B(NC)
            public bool bAlarmStopMode;             // 정지 모드, FALSE : 즉시정지, TRUE: 감속 후 정지

            public bool bEZLogic;                   // Z상 로직에 대한 설정, FALSE : A(NO), TRUE : B(NC)

            public bool bUseInposition;             // INPOSITION 사용 유무에 대한 설정, 사용 시 Command 출력이 완료되더라도 INP신호 ON 전에는 미완료
            public bool bInpositionLogic;           // INPOSITION 로직에 대한 설정, FALSE : A(NO), TRUE : B(NC)

            public EN_MOTOR_APPLICATION enMotorApplicationType;        // Linear or Circular 인지 설정.
            public double dblNumeratorForScale;       // Scale 에서의 분자값 -> 엔코더 분해능 당 이동하는 거리를 입력 한다.(Linear Type 인 경우 mm, Circular Type 인 경우 Deg)
            public double dblDenominatorForScale;     // Scale 에서의 분모값 -> 엔코더 분해능을 입력 한다.(단위는 Pulse or Count)

            public bool bCommandDirection;          // 커맨드 방향, FALSE : No Invert, TRUE : Invert
            public bool bEncoderDirection;          // 엔코더 방향, FALSE : No Invert, TRUE : Invert

            #region 리밋 관련
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
            #endregion

            #endregion

            #region 속성
            public string strInMode
            {
                set
                {
                    foreach (EN_MOTOR_INMODE inMode in Enum.GetValues(typeof(EN_MOTOR_INMODE)))
                    {
                        if (value.Equals(inMode.ToString()))
                        {
                            enInMode = inMode;

                            break;
                        }
                    }
                }
                get
                {
                    return enInMode.ToString();
                }
            }

            // 2019.02.13 by junho [ADD] Comizoa 필요 함수 추가
            public string strInDirection
            {
                set
                {
                    bInputDirection = value.Equals( TRUE);
                }
                get
                {
                    return bInputDirection ? TRUE : FALSE;
                }
            }
            public string strOutMode
            {
                set
                {
                    foreach (EN_MOTOR_OUTMODE outMode in Enum.GetValues(typeof(EN_MOTOR_OUTMODE)))
                    {
                        if (value.Equals(outMode.ToString()))
                        {
                            enOutMode = outMode;

                            break;
                        }
                    }
                }
                get
                {
                    return enOutMode.ToString();
                }
            }
            public string strMotorApplicationType
            {
                set
                {
                    foreach (EN_MOTOR_APPLICATION type in Enum.GetValues(typeof(EN_MOTOR_APPLICATION)))
                    {
                        if (value.Equals(type.ToString()))
                        {
                            enMotorApplicationType = type;
                            break;
                        }
                    }
                }
                get
                {
                    return enMotorApplicationType.ToString();
                }
            }
            public string strSWLimitStopModePositive
            {
                set
                {
                    bSWLimitStopModePositive = value.Equals(STOPMODE_EMG);
                }
                get
                {
                    return bSWLimitStopModePositive ? STOPMODE_EMG : STOPMODE_DECEL;
                }
            }
            public string strSWLimitStopModeNegative
            {
                set
                {
                    bSWLimitStopModeNegative = value.Equals(STOPMODE_EMG);
                }
                get
                {
                    return bSWLimitStopModeNegative ? STOPMODE_EMG : STOPMODE_DECEL;
                }
            }
            public string strHWLimitStopModePositive
            {
                set
                {
                    bHWLimitStopModePositive = value.Equals(STOPMODE_EMG);
                }
                get
                {
                    return bHWLimitStopModePositive ? STOPMODE_EMG : STOPMODE_DECEL;
                }
            }
            public string strHWLimitStopModeNegative
            {
                set
                {
                    bHWLimitStopModeNegative = value.Equals(STOPMODE_EMG);
                }
                get
                {
                    return bHWLimitStopModeNegative ? STOPMODE_EMG : STOPMODE_DECEL;
                }
            }
            public string strHWLimitLogicPositive
            {
                set
                {
                    bHWLimitLogicPositive = value.Equals(LOGIC_ACTIVE_LOW);
                }
                get
                {
                    return bHWLimitLogicPositive ? LOGIC_ACTIVE_LOW : LOGIC_ACTIVE_HIGH;
                }
            }
            public string strHWLimitLogicNegative
            {
                set
                {
                    bHWLimitLogicNegative = value.Equals(LOGIC_ACTIVE_LOW);
                }
                get
                {
                    return bHWLimitLogicNegative ? LOGIC_ACTIVE_LOW : LOGIC_ACTIVE_HIGH;
                }
            }
            public string strCommandDirection
            {
                set
                {
                    bCommandDirection = value.Equals(INVERT_ON);
                }
                get
                {
                    return bCommandDirection ? INVERT_ON : INVERT_OFF;
                }
            }
            public string strEncoderDirection
            {
                set
                {
                    bEncoderDirection = value.Equals(INVERT_ON);
                }
                get
                {
                    return bEncoderDirection ? INVERT_ON : INVERT_OFF;
                }
            }
            #endregion
        }


        #region 에러 관련
        private string m_strErrorMessage    = NONE;
        #endregion

        #region 등록 모션
        //private Dictionary<int, RegisteredMotion> m_dicOfRegisteredMotion           = new Dictionary<int,RegisteredMotion>();
        #endregion

        #region 공유 모션
        //private TokenManagement m_tokenManagement   = new TokenManagement();
        #endregion

        #region 파라미터 설정
        private bool m_bNeedToSetParameter      = false;
        #endregion

        #endregion

        #region 속성
        public MotionItem AddedItem
        {
            get
            {
                return m_motionItemManager.AddedItem;
            }
        }
        public MotionItem ChangedItem
        {
            get
            {
                return m_motionItemManager.ChangedItem;
            }
        }
        public Dictionary<int, string> DicOfItemName
        {
            get
            {
                return m_motionItemManager.dicOfItemName;
            }
        }
        public int nMaximumTarget
        {
            get
            {
                return m_nCountOfAxis - 1;
            }
        }
        private bool bNeedToSetParameter
        {
            set 
            {
                m_nSeqNumberForConnection   = 0;
                m_bNeedToSetParameter       = value;
            }
            get { return m_bNeedToSetParameter; }
        }

        // 2020.03.10 by Thienvv [ADD]
        //public EN_MOTIONController MotionControllerType { get { return m_enMotionControlType; } }
        #endregion

        #region 싱글톤
        private MotionInterfaces () { }
        private static MotionInterfaces _Instance = new MotionInterfaces();
        public static MotionInterfaces GetInstance() 
        {  
            if(_Instance == null)
            {
                //_Instance = new MotionInterfaces();
            }
            return _Instance; 
        }
        #endregion

        #region 외부 인터페이스

        #region 모션 인터페이스

        #region 모터 상태 확인
        /// <summary>
        /// 2018.07.12 by yjlee [ADD] 모션의 활성상태를 반환한다.
        /// </summary>
        public bool GetActive(int nIndex)
        {
            if (m_dicOfMotionItem.ContainsKey(nIndex) == false) { return false; }

            MotionItem mItem        = m_dicOfMotionItem[nIndex];

            return mItem.nTarget != -1 && mItem.bEnable;
        }
        /// <summary>
        /// 2019.04.29 by yjlee [ADD] 현재 읽어져있는 상태를 반환한다.
        /// </summary>
        public bool GetMotionState(int nIndex, ref bool[] arState)
        {
            if (false == m_dicOfMotionItem.ContainsKey(nIndex)) { return false;}
            
            MotionItem mItem    = m_dicOfMotionItem[nIndex];

            int nTarget = 0;

            if (false == GetVaildTarget(mItem, ref nTarget)) { return false; }

            arState     = m_listOfArState[nTarget];

            return true;
        }

        /// <summary>
        /// 2019.10.17. by shkim. [ADD] Servo On, Off 후 실제 State를 정상적으로 리턴 받는데까지 소요되는 시간이 있음으로,
        /// 따로 Refresh 하는 함수가 필요하다.
        /// </summary>
        public bool DoRefreshMotionState(int nIndex)
        {
            if (false == m_dicOfMotionItem.ContainsKey(nIndex)) { return false; }

            MotionItem mItem = m_dicOfMotionItem[nIndex];

            int nTarget = 0;

            if (false == GetVaildTarget(mItem, ref nTarget)) { return false; }
            //2020.09.15 nogami modify start
            //GetStateFromController(mItem);
            if(m_listOfArState[nTarget][(int)EN_MOTOR_STATE.HOMEEND])
            {
                GetStateFromController(mItem,false);
            }
            else
            {
                GetStateFromController(mItem,true);
            }
            //2020.09.15 nogami modify end
            return true;
        }

        /// <summary>
        /// 2019.04.20 by yjlee [ADD] 모터 상태를 반환한다.
        /// </summary>
        public bool GetMotionStateFromController(int nIndex, ref bool[] arState)
        {
            if (false == m_dicOfMotionItem.ContainsKey(nIndex)) { return false;}
            
            MotionItem mItem    = m_dicOfMotionItem[nIndex];

            int nTarget = 0;

            if (false == GetVaildTarget(mItem, ref nTarget)) { return false; }
			
			// For MK-Unit
            bool bIsHoming;
            //2020.09.15 nogami modify if (m_listOfMotionStatus[nTarget] == EN_MOTION_STATUS.HOMING || m_listOfMotionStatus[nIndex] == EN_MOTION_STATUS.ELHOMING) bIsHoming = true;
            if (m_listOfMotionStatus[nTarget] == EN_MOTION_STATUS.HOMING || 
                m_listOfMotionStatus[nIndex] == EN_MOTION_STATUS.ELHOMING ||
                m_listOfArState[nTarget][(int)EN_MOTOR_STATE.HOMEEND] == false ) 
                bIsHoming = true;
            else 
                bIsHoming = false;
            
            GetStateFromController(mItem, bIsHoming);

            arState     = m_listOfArState[nTarget];

            return true;
        }
        /// <summary>
        /// 2018.08.30 by yjlee [ADD] 해당 아이템의 모션이 이송 중인지 판단한다.
        /// </summary>
        public bool IsMotionMoving(int nIndex)
        {
            int nTarget = 0;

            if (false == GetVaildTarget(nIndex, ref nTarget)) { return false;}

            return IsMotionMovingAtTarget(nTarget);
        }
        public bool IsMotionJogMoving(int nIndex)
        {
            int nTarget = 0;

            if (false == GetVaildTarget(nIndex, ref nTarget)) { return false; }

            return IsMotionJogMovingAtTarget(nTarget);
        }
        /// <summary>
        /// 2018.07.12 by yjlee [ADD] 모터의 모션 완료 상태를 확인한다.
        /// </summary>
        /// <param name="nIndex"></param>
        /// <returns></returns>
        public bool IsMotionDone(int nIndex)
        {
            int nTarget = 0;

            if (false == GetVaildTarget(nIndex, ref nTarget)) { return true; }

            return m_listOfArState[nTarget][(int)EN_MOTOR_STATE.MOTIONDONE];
            
        }
        /// <summary>
        /// 2018.07.12 by yjlee [ADD] 모터의 홈 완료 상태를 확인한다.
        /// </summary>
        /// <param name="nIndex"></param>
        /// <returns></returns>
        public bool IsHomeEnd(int nIndex)
        {
            int nTarget = 0;

            if (false == GetVaildTarget(nIndex, ref nTarget)) { return true; }

            return m_listOfArState[nTarget][(int)EN_MOTOR_STATE.HOMEEND];
        }
        /// <summary>
        /// 2018.07.12 by yjlee [ADD] 모터의 전원 인가 상태를 확인한다.
        /// </summary>
        /// <param name="nIndex"></param>
        /// <returns></returns>
        public bool IsMotorOn(int nIndex)
        {
            int nTarget = 0;

            if (false == GetVaildTarget(nIndex, ref nTarget)) { return true; }

            return m_listOfArState[nTarget][(int)EN_MOTOR_STATE.MOTORON];
        }
        /// <summary>
        /// 2018.07.12 by yjlee [ADD] 모터의 알람 상태를 확인한다.
        /// </summary>
        /// <param name="nIndex"></param>
        /// <returns></returns>
        public bool IsMotionAlarm(int nIndex)
        {
            int nTarget = 0;

            if (false == GetVaildTarget(nIndex, ref nTarget)) { return false; }

            return m_listOfArState[nTarget][(int)EN_MOTOR_STATE.ALARM];
        }
        /// <summary>
        /// 2018.07.12 by yjlee [ADD] 모터의 - 리밋 상태를 확인한다.
        /// </summary>
        /// <param name="nIndex"></param>
        /// <returns></returns>
        public bool IsMotionHWLimitNegative(int nIndex)
        {
            if (false == m_dicOfMotionItem.ContainsKey(nIndex)) { return false; }

            MotionItem mItem = m_dicOfMotionItem[nIndex];

            int nTarget = 0;

            if (false == GetVaildTarget(mItem, ref nTarget)) { return false; }

            return m_listOfArState[nTarget][(int)EN_MOTOR_STATE.LIMITNEGATIVE];
        }
        public bool GetState(ref int nState, MotionInstance::Motion_.MOTOR_STATE enState)
        {
            return 1 == ((nState >> (int)enState) & 1);
        }
        //2020.03.06 nogami add start
        public bool IsMotionHWLimitNegativeReal(int nIndex)
        {
            /// --------------------------------------------------
            /// 신규 구조에 따른  코딩 추가 및 수정하여 적용했음 ( 2021.08.06 by Thienvv )
            MechatrolinkMotionController inst = MechatrolinkMotionController.GetInstance();
            /// Console.WriteLine("IsMotionHWLimitNegativeReal, {0}", nIndex);
            bool bHWLimit = inst.IsMotionHWLimitN(nIndex);
            ///Console.WriteLine("IsMotionHWLimitN(nIndex) = {0}", bHWLimit);
            return inst.IsMotionHWLimitN(nIndex);            
/***
            //if (false == m_dicOfMotionItem.ContainsKey(nIndex)) { return false; }
            //MotionItem mItem = m_dicOfMotionItem[nIndex];
            //int nTarget = 0;
            //if (false == GetVaildTarget(mItem, ref nTarget)) { return false; }
            //bool[] arState = new bool[Enum.GetNames(typeof(EN_MOTOR_STATE)).Length];        //m_listOfArState[nTarget];                        
            //m_motionController.GetState(nTarget, arState);
            //return arState[(int)EN_MOTOR_STATE.LIMITNEGATIVE];
 ***/

        }
        //2020.03.06 nogami add end
        /// <summary>
        /// 2018.07.12 by yjlee [ADD] 모터의 + 리밋 상태를 확인한다.
        /// </summary>
        /// <param name="nIndex"></param>
        /// <returns></returns>
        public bool IsMotioHWLimitPositive(int nIndex)
        {
            if (false == m_dicOfMotionItem.ContainsKey(nIndex)) { return false;}            
            MotionItem mItem    = m_dicOfMotionItem[nIndex];
            int nTarget = 0;
            if (false == GetVaildTarget(mItem, ref nTarget)) { return false; }
            return m_listOfArState[nTarget][(int)EN_MOTOR_STATE.LIMITPOSITIVE];
        }
        //2020.03.06 nogami add start
        public bool IsMotionHWLimitPositiveReal(int nIndex)
        {
            ///---------
            MechatrolinkMotionController inst = MechatrolinkMotionController.GetInstance();
            bool bHWLimit = inst.IsMotionHWLimitP(nIndex);
            ///Console.WriteLine("IsMotionHWLimitN(nIndex) = {0}", bHWLimit);
            return inst.IsMotionHWLimitP(nIndex);

            /***
                        return true;
                        if (false == m_dicOfMotionItem.ContainsKey(nIndex)) { return false; }
                        MotionItem mItem = m_dicOfMotionItem[nIndex];
                        int nTarget = 0;
                        if (false == GetVaildTarget(mItem, ref nTarget)) { return false; }
                        bool[] arState = new bool[Enum.GetNames(typeof(EN_MOTOR_STATE)).Length];        //m_listOfArState[nTarget];

                        ///  code modify from here............ 2021.07.12
                        ///  
                        // m_motionController.GetState(nTarget, arState);
                        return arState[(int)EN_MOTOR_STATE.LIMITPOSITIVE];
             * 
             * ***/
        }
        //2020.03.06 nogami add end
        /// <summary>
        /// 2019.04.29 by yjlee [ADD] SW - 리밋인지 확인한다.
        /// </summary>
        public bool IsMotionSWLimitNegative(int nIndex)
        {
            if (false == m_dicOfMotionItem.ContainsKey(nIndex)) { return false;}
            
            MotionItem mItem    = m_dicOfMotionItem[nIndex];

            int nTarget = 0;

            if (false == GetVaildTarget(mItem, ref nTarget)) { return false; }

            return m_listOfArState[nTarget][(int)EN_MOTOR_STATE.SWLIMITNEGATIVE];
        }
        /// <summary>
        /// 2019.04.29 by yjlee [ADD] SW + 리밋인지 확인한다.
        /// </summary>
        public bool IsMotionSWLimitPositive(int nIndex)
        {
            if (false == m_dicOfMotionItem.ContainsKey(nIndex)) { return false; }

            MotionItem mItem = m_dicOfMotionItem[nIndex];

            int nTarget = 0;

            if (false == GetVaildTarget(mItem, ref nTarget)) { return false; }

            return m_listOfArState[nTarget][(int)EN_MOTOR_STATE.SWLIMITPOSITIVE];
        }
        /// <summary>
        /// 2018.07.12 by yjlee [ADD] 모터의 홈 타임아웃 상태를 확인한다.
        /// </summary>
        /// <param name="nIndex"></param>
        /// <returns></returns>
        public bool IsHomeTimeout(int nIndex)
        {
            int nTarget = 0;

            if (false == GetVaildTarget(nIndex, ref nTarget)) { return false; }

            return m_listOfArState[nTarget][(int)EN_MOTOR_STATE.HOMETIMEOUT];
        }
        /// <summary>
        /// 2018.07.12 by yjlee [ADD] 모터의 모션 타임아웃 상태를 확인한다.
        /// </summary>
        /// <param name="nIndex"></param>
        /// <returns></returns>
        public bool IsMotionTimeout(int nIndex)
        {
            int nTarget = 0;

            if (false == GetVaildTarget(nIndex, ref nTarget)) { return false; }

            return m_listOfArState[nTarget][(int)EN_MOTOR_STATE.MOTIONTIMEOUT];
        }
        public bool IsMotionTimeoutOver(int nIndex)
        {
            int nTarget = 0;

            if (false == GetVaildTarget(nIndex, ref nTarget)) { return false; }

            MovedMotion mMotion = m_listOfMovingMotion[nTarget];
            return (mMotion.IsTimeoutOver());
        }
        //2020.03.09 nogami add start
        public bool IsMotionPreSWLimitNegative(int nIndex)
        {
            if (false == m_dicOfMotionItem.ContainsKey(nIndex)) { return false; }

            MotionItem mItem = m_dicOfMotionItem[nIndex];

            int nTarget = 0;

            if (false == GetVaildTarget(mItem, ref nTarget)) { return false; }

            return m_listOfArState[nTarget][(int)EN_MOTOR_STATE.PRE_SWLIMITNEGATIVE];
        }
        public bool IsMotionPreSWLimitPositive(int nIndex)
        {
            if (false == m_dicOfMotionItem.ContainsKey(nIndex)) { return false; }

            MotionItem mItem = m_dicOfMotionItem[nIndex];

            int nTarget = 0;

            if (false == GetVaildTarget(mItem, ref nTarget)) { return false; }

            return m_listOfArState[nTarget][(int)EN_MOTOR_STATE.PRE_SWLIMITPOSITIVE];
        }
        //2020.03.09 nogami add end
        /// <summary>
        /// 2018.07.12 by yjlee [ADD] 명령어 포지션을 반환한다.
        /// </summary>
        /// <param name="nIndex"></param>
        /// <returns></returns>
        public double GetCommandPosition(int nIndex)
        {
            int nTarget = 0;

            if (false == GetVaildTarget(nIndex, ref nTarget)) { return 0; }
            ///-----
            ///  code modify from here............ 2021.07.12            
            return m_motionController.GetCommandPosition(nTarget);
           // return 0.0;
        }
        /// <summary>
        /// 2018.07.12 by yjlee [ADD] 엔코더 포지션을 반환한다.
        /// </summary>
        /// <param name="nIndex"></param>
        /// <returns></returns>
        public double GetActualPosition(int nIndex)
        {
            int nTarget = 0;

            if (false == GetVaildTarget(nIndex, ref nTarget)) { return 0; }

            //return m_motionController.GetActualPosition(nTarget);
            return 0.0;
        }
        /// <summary>
        /// 2019.06.25 by yjlee [ADD] 해당 축의 터치 엔코더 값을 읽어온다.
        /// </summary>
        public double GetTouchEncoderPosition(int nIndex)
        {
            int nTarget = 0;

            if (false == GetVaildTarget(nIndex, ref nTarget)) { return 0; }

           // return m_motionController.GetTouchEncoderPosition(nTarget);
            return 0.0;
        }
        #endregion

        #region 모터 상태 설정
        /// <summary>
        /// 2018.07.12 by yjlee [ADD] 모터의 상태를 리셋한다.
        /// </summary>
        /// <param name="nIndex"></param>
        public void DoReset(int nIndex)
        {
            if (false == m_dicOfMotionItem.ContainsKey(nIndex)) { return;}
            
            MotionItem mItem    = m_dicOfMotionItem[nIndex];

            int nTarget = 0;

            if (false == GetVaildTarget(mItem, ref nTarget)) { return; }

            m_listOfArState[nTarget][(int)EN_MOTOR_STATE.MOTIONTIMEOUT]     = false;
            m_listOfArState[nTarget][(int)EN_MOTOR_STATE.HOMETIMEOUT]       = false;
            //2020.03.09 nogami add start
            m_listOfArState[nTarget][(int)EN_MOTOR_STATE.PRE_SWLIMITNEGATIVE] = false;
            m_listOfArState[nTarget][(int)EN_MOTOR_STATE.PRE_SWLIMITPOSITIVE] = false;
            //2020.03.09 nogami add end

            ///
            m_motionController.DoReset(nTarget);

            //2020.09.15 nogami modify start
            //GetStateFromController(mItem);
            if (m_listOfArState[nTarget][(int)EN_MOTOR_STATE.HOMEEND])
            {
                GetStateFromController(mItem, false);
            }
            else
            {
                GetStateFromController(mItem, true);
            }
            //2020.09.15 nogami modify end
        }
        /// <summary>
        /// 2018.07.14 by yjlee [ADD] 모든 모터의 상태를 리셋한다.
        /// </summary>
        public void DoResetAll()
        {
            int nCountOfItem    = m_motionItemManager.nMaximunIndexOfItem;

            for(int i = 0 ; i < nCountOfItem ; ++ i)
            {
                DoReset(i);
            }
        }
        /// <summary>
        /// 2018.07.12 by yjlee [ADD] 모터의 전원을 인가한다.
        /// 2019.02.04 by yjlee [MOD] 서보 전원 인가 시 포지션 일치여부 추가
        /// </summary>
        /// <param name="nIndex"></param>
        public void DoMotorOn(int nIndex, bool bPositionAdjustment = false)
        {
            if (false == m_dicOfMotionItem.ContainsKey(nIndex)) { return;}
            
            MotionItem mItem    = m_dicOfMotionItem[nIndex];

            int nTarget = 0;

            if (false == GetVaildTarget(mItem, ref nTarget)) { return; }
            
            // 위지 조정옵션일 경우
            if(bPositionAdjustment)            
            {
                ///--------------- modfy from here... 2021.07.12
                ///
                m_motionController.AdjustCommandPositionByActualPosition(nTarget);
            }

            m_motionController.DoMotorOn(nTarget);

            //2020.09.15 nogami modify start
            //GetStateFromController(mItem);
            if (m_listOfArState[nTarget][(int)EN_MOTOR_STATE.HOMEEND])
            {
                GetStateFromController(mItem, false);
            }
            else
            {
                GetStateFromController(mItem, true);
            }
            //2020.09.15 nogami modify end
        }
        /// <summary>
        /// 2018.07.14 by yjlee [ADD] 모든 모터의 전원을 인가한다.
        /// </summary>
        public void DoMotorOnAll()
        {
            int nCountOfItem = m_motionItemManager.nMaximunIndexOfItem;

            for (int i = 0; i < nCountOfItem; ++ i)
            {
                DoMotorOn(i);
            }
        }
        /// <summary>
        /// 2018.07.12 by yjlee [ADD] 모터의 전원을 해제한다.
        /// 2019.02.04 by yjlee [MOD] 서보 전원 해제 시 포지션 일치 여부 추가
        /// </summary>
        public void DoMotorOff(int nIndex, bool bPositionAdjustment = false)
        {
            if (false == m_dicOfMotionItem.ContainsKey(nIndex)) { return;}
            
            MotionItem mItem    = m_dicOfMotionItem[nIndex];

            int nTarget = 0;

            if (false == GetVaildTarget(mItem, ref nTarget)) { return; }

            m_listOfArState[nTarget][(int)EN_MOTOR_STATE.HOMEEND]       = false;
            m_listOfArState[nTarget][(int)EN_MOTOR_STATE.MOTIONDONE]    = false;

            //m_motionController.DoMotorOff(nTarget);

            // 위지 조정옵션일 경우
            if (bPositionAdjustment)
            {
                ///------- 2021.07.12
                /// 
              //  m_motionController.AdjustCommandPositionByActualPosition(nTarget);
            }
        }
        //2020.09.27 nogami add start
        public void HomeEndReset(int nIndex)
        {
            if (false == m_dicOfMotionItem.ContainsKey(nIndex)) { return; }

            MotionItem mItem = m_dicOfMotionItem[nIndex];

            int nTarget = 0;

            if (false == GetVaildTarget(mItem, ref nTarget)) { return; }
            m_listOfArState[nTarget][(int)EN_MOTOR_STATE.HOMEEND] = false;
        }
        /// <summary>
        /// 2018.07.14 by yjlee [ADD] 모든 모터의 전원을 해제한다.
        /// </summary>
        public void DoMotorOffAll()
        {
            int nCountOfItem = m_motionItemManager.nMaximunIndexOfItem;

            for (int i = 0; i < nCountOfItem; ++ i)
            {
                DoMotorOff(i);
            }
        }
        /// <summary>
        /// 2018.07.12 by yjlee [ADD] 모터의 명령 포지션을 설정한다.
        /// </summary>
        /// <param name="nIndex"></param>
        /// <param name="dblPosition"></param>
        public void SetCommandPosition(int nIndex, double dblPosition)
        {
            int nTarget = 0;

            if (false == GetVaildTarget(nIndex, ref nTarget)) { return; }

            m_motionController.SetCommandPosition(nTarget, dblPosition);  // 2021.07.12
        }
        /// <summary>
        /// 2018.07.12 by yjlee [ADD] 모터의 엔코더 포지션을 설정한다.
        /// </summary>
        /// <param name="nIndex"></param>
        /// <param name="dblPosition"></param>
        public void SetActualPosition(int nIndex, double dblPosition)
        {
            int nTarget = 0;

            if (false == GetVaildTarget(nIndex, ref nTarget)) { return; }

            if (nTarget == -1) { return; }

            m_motionController.SetActualPosition(nTarget, dblPosition);   // 2021.07.12 
        }
        /// <summary>
        /// 2018.07.12 by yjlee [ADD] 모터의 포지션을 초기화한다.
        /// </summary>
        /// <param name="nIndex"></param>
        public void ClearPosition(int nIndex)
        {
            int nTarget = 0;

            if (false == GetVaildTarget(nIndex, ref nTarget)) { return; }

            ///---
            ///---   code modify from here....      2021.07.12 
            ///
            //m_motionController.ClearPosition(nTarget);

            
        }
        #endregion

        #region 모터 이송 명령

        #region Stop
        /// <summary>
        /// 2018.07.12 by yjlee [ADD] 모션을 정지시킨다.
        /// </summary>
        /// <param name="nIndex"></param>
        public void StopMotion(int nIndex, bool bEmergency = true)
        {
            if (false == m_dicOfMotionItem.ContainsKey(nIndex)) { return; }
            
            MotionItem mItem    = m_dicOfMotionItem[nIndex];

            int nTarget = 0;

            if (false == GetVaildTarget(mItem, ref nTarget)) { return; }

            switch(m_listOfMotionStatus[nTarget])
            {
                case EN_MOTION_STATUS.READY:
                    return;
                case EN_MOTION_STATUS.HOMING:
                case EN_MOTION_STATUS.ELHOMING:
                    //m_motionController.StopHomeMotion(nTarget, bEmergency);
                    break;
                default:
                     //m_motionController.StopMotion(nTarget, bEmergency);
                    break;
            }

            m_listOfRepeatingItem[nTarget].StopRepeat();

            m_listOfMotionStatus[nTarget]   = EN_MOTION_STATUS.STOP;
        }
        //2020.12.17 nogami add start
        public void PauseMotion(int nIndex)
        {
            if (false == m_dicOfMotionItem.ContainsKey(nIndex)) { return; }

            MotionItem mItem = m_dicOfMotionItem[nIndex];

            int nTarget = 0;

            if (false == GetVaildTarget(mItem, ref nTarget)) { return; }

            switch (m_listOfMotionStatus[nTarget])
            {
                case EN_MOTION_STATUS.MOVING:
                    //m_motionController.PauseMotion(nTarget);
                    break;
                default:
                    return;
            }
        }
        public void ResumeMotion(int nIndex)
        {
            if (false == m_dicOfMotionItem.ContainsKey(nIndex)) { return; }

            MotionItem mItem = m_dicOfMotionItem[nIndex];

            int nTarget = 0;

            if (false == GetVaildTarget(mItem, ref nTarget)) { return; }

            switch (m_listOfMotionStatus[nTarget])
            {
                case EN_MOTION_STATUS.MOVING:
                    //m_motionController.ResumeMotion(nTarget);
                    break;
                default:
                    return;
            }
        }
        //2020.12.17 nogami add end
        /// <summary>
        /// 2018.07.12 by yjlee [ADD] 모든 모션을 정지시킨다.
        /// </summary>
        /// <param name="bEmergency"></param>
        public void StopMotionAll(bool bEmergency = true)
        {
            int nCountOfItem    = m_motionItemManager.nMaximunIndexOfItem;

            for(int i = 0 ; i < nCountOfItem ; ++ i)
            {
                StopMotion(i, bEmergency);
            }
        }
        #endregion

        #region Move
        /// <summary>
        /// 2018.07.12 by yjlee [ADD] 절대좌표계로 축을 이동시킨다.
        /// </summary>
        /// <param name="nIndex"></param>
        /// <param name="dblPosition">목적지</param>
        /// <param name="spdContent">사용할 속도 항목</param>
        /// <param name="dblVelocity">속도</param>
        /// <param name="uDelay">모션 완료 딜레이</param>
        public bool MoveSxAbsolutely(int nIndex, double dblDestination, EN_MOTOR_CONTENTOFSPEED spdContent , double dblVelocity, uint uRatio, uint uDelay, 
                                      string strUsingTask = MOVE_MANUAL)
        {
            if (false == m_dicOfMotionItem.ContainsKey(nIndex)) { return false; }

            MotionItem mItem = m_dicOfMotionItem[nIndex];
            //2020.03.09 nogami add start
            if (IsSoftwareLimitNegativeCheck(nIndex, EN_MOTION_TYPE.ABSOLUTE, dblDestination))
            {
                bool[] arState = m_listOfArState[mItem.nTarget];
                arState[(int)EN_MOTOR_STATE.PRE_SWLIMITNEGATIVE] = true;
                return (false);
            }
            else if (IsSoftwareLimitPositiveCheck(nIndex, EN_MOTION_TYPE.ABSOLUTE, dblDestination))
            {
                bool[] arState = m_listOfArState[mItem.nTarget];
                arState[(int)EN_MOTOR_STATE.PRE_SWLIMITPOSITIVE] = true;
                return (false);
            }
            //2020.03.09 nogami add end

            bool bInputShaping  = false;
            uint uTimeout       = 0;

            // 2018.10.16 by yjlee [ADD] 현재 엔코더 포지션과 동일할 경우 이송하지 않는다.
            //double dblCommandPosition   = Math.Round(mItem.dblCommandPosition, 3);
            //if(dblDestination >= dblCommandPosition - 0.002
            //    && dblDestination <= dblCommandPosition + 0.002) { return false; }
            double dblCommandPosition = Math.Round(mItem.dblCommandPosition, 3);
            if (dblDestination == dblCommandPosition) { return false; }

            if (false == PrepareForMoving(mItem, spdContent, dblDestination, dblVelocity, uRatio, ref uTimeout, ref bInputShaping))
            {
                return false;
            }
            else if(bInputShaping == false)
            {
                ///--------------
                /// 2021.07.12
             //   m_motionController.MoveSxAbsolutely(mItem.nTarget, dblDestination);
            }

            mItem.strUsingTask      = strUsingTask;
            mItem.enMotionType      = EN_MOTION_TYPE.ABSOLUTE;
            mItem.dblDestination    = dblDestination;

            AddMovingMotion(mItem, uTimeout, uDelay);

            return true;
        }
        /// <summary>
        /// 2018.07.12 by yjlee [ADD] 상대좌표계로 축을 이동시킨다.
        /// </summary>
        /// <param name="nIndex"></param>
        /// <param name="dblPosition">목적지</param>
        /// <param name="spdContent">사용할 속도 항목</param>
        /// <param name="dblVelocity">속도</param>
        /// <param name="uDelay">모션 완료 딜레이</param>
        public bool MoveSxRelatively(int nIndex, double dblDestination, EN_MOTOR_CONTENTOFSPEED spdContent, double dblVelocity, uint uRatio, uint uDelay, string strUsingTask = MOVE_MANUAL)
        {
            if (false == m_dicOfMotionItem.ContainsKey(nIndex)
                || dblDestination == 0) { return false; }

            MotionItem mItem    = m_dicOfMotionItem[nIndex];
            //2020.03.09 nogami add start
            if (IsSoftwareLimitNegativeCheck(nIndex, EN_MOTION_TYPE.RELATIVE, dblDestination))
            {
                bool[] arState = m_listOfArState[mItem.nTarget];
                arState[(int)EN_MOTOR_STATE.PRE_SWLIMITNEGATIVE] = true;
                return (false);
            }
            //2020.10.02 else if (IsSoftwareLimitPositiveCheck(nIndex, EN_MOTION_TYPE.ABSOLUTE, dblDestination))
            else if (IsSoftwareLimitPositiveCheck(nIndex, EN_MOTION_TYPE.RELATIVE, dblDestination))
            {
                bool[] arState = m_listOfArState[mItem.nTarget];
                arState[(int)EN_MOTOR_STATE.PRE_SWLIMITPOSITIVE] = true;
                return (false);
            }
            //2020.03.09 nogami add end

            bool bInputShaping  = false;
            uint uTimeout       = 0;

            if (false == PrepareForMoving(mItem, spdContent, dblDestination, dblVelocity, uRatio, ref uTimeout, ref bInputShaping))
            {
                return false;
            }
            else if(bInputShaping == false)
            {
                /// 2021.07.12
                /// 
                //m_motionController.MoveSxRelatively(mItem.nTarget, dblDestination);
            }

            mItem.strUsingTask      = strUsingTask;
            mItem.enMotionType      = EN_MOTION_TYPE.RELATIVE;
			mItem.dblDestination    = dblDestination;
			
            AddMovingMotion(mItem, uTimeout, uDelay);
            return true;
        }
        public bool MoveExternalPositioning(int nIndex, double dblDestination, EN_MOTOR_CONTENTOFSPEED spdContent, double dblVelocity, uint uRatio, uint uDelay, string strUsingTask = MOVE_MANUAL)
        {
            if (false == m_dicOfMotionItem.ContainsKey(nIndex)
                || dblDestination == 0) { return false; }

            MotionItem mItem = m_dicOfMotionItem[nIndex];

            bool bInputShaping = false;
            uint uTimeout = 0;

            if (false == PrepareForMoving(mItem, spdContent, dblDestination, dblVelocity, uRatio, ref uTimeout, ref bInputShaping))
            {
                return false;
            }
            else if (bInputShaping == false)
            {
                /// 2021.07.12
                /// 
                //m_motionController.MoveExternalPositioning(mItem.nTarget, dblDestination);
            }

            mItem.strUsingTask = strUsingTask;
            mItem.enMotionType = EN_MOTION_TYPE.RELATIVE;
            mItem.dblDestination = dblDestination;

            AddMovingMotion(mItem, uTimeout, uDelay);
            return true;
        }
        //線形補間 2020.10.18 nogami add
        public bool MoveLinearAbsolutely(int[] nIndexs, double[] dAbsolutePos, EN_MOTOR_CONTENTOFSPEED spdContent, double dblVelocity, uint uRatio, uint uDelay, string strUsingTask = MOVE_MANUAL)
        {
            int[] nTargets = new int[nIndexs.Length];
            for (int i = 0; i < nIndexs.Length; i++)
            {
                if (false == m_dicOfMotionItem.ContainsKey(i)) { return false; }
                MotionItem mItem = m_dicOfMotionItem[nIndexs[i]];
                nTargets[i] = mItem.nTarget;
            }
            for (int i = 0; i < nIndexs.Length; i++)
            {
                MotionItem mItem = m_dicOfMotionItem[nIndexs[i]];

                bool bInputShaping = false;
                uint uTimeout = 0;

                if (false == PrepareForMoving(mItem, spdContent, dAbsolutePos[i], dblVelocity, uRatio, ref uTimeout, ref bInputShaping))
                {
                    return false;
                }
                else if (bInputShaping == false)
                {
                    if (i == 0)
                    {/// 2021.07.12    --- 4차구조에서 사용안하므로 생략...........
                        //m_motionController.MoveLinearAbsolutely(nTargets, dAbsolutePos);
                    }
                }

                mItem.strUsingTask = strUsingTask;
                mItem.enMotionType = EN_MOTION_TYPE.ABSOLUTE;
                mItem.dblDestination = dAbsolutePos[i];

                AddMovingMotion(mItem, uTimeout, uDelay);
            }
            return true;
        }
        public bool MoveCircle(int[] nIndexs, EN_MOTOR_CONTENTOFSPEED spdContent, double dblVelocity, double Xrest, double Yrest, double Xcenter, double Ycenter, double Radius, double Pitch, int Repeat, int Dir,
            uint uRatio, uint uDelay, uint uTimeout, string strUsingTask = MOVE_MANUAL)
        {
            int[] nTargets = new int[nIndexs.Length];
            for (int i = 0; i < nIndexs.Length; i++)
            {
                if (false == m_dicOfMotionItem.ContainsKey(i)) { return false; }
                MotionItem mItem = m_dicOfMotionItem[nIndexs[i]];
                nTargets[i] = mItem.nTarget;
            }
            for (int i = 0; i < nIndexs.Length; i++)
            {
                MotionItem mItem = m_dicOfMotionItem[nIndexs[i]];
                bool bInputShaping = false;
                uint dummy = 0;

                if (false == PrepareForMoving(mItem, spdContent, 0.0, dblVelocity, uRatio, ref dummy, ref bInputShaping))
                {
                    return false;
                }
                else if (bInputShaping == false)
                {
                    if (i == 0)
                    {
                        //m_motionController.YmcCallMPM001(dblVelocity, dblVelocity, (int)mItem[i].spdRecentParam.dblAccelTime, (int)mItem[i].spdRecentParam.dblDecelTime,
                        //    Xrest, Yrest, Xcenter, Ycenter, Radius, Pitch, Repeat, (EN_BALL_ATTACH_MOTION)Dir);
                        MOTOR_SPEED_PARAMETER spdParam = mItem.speedParam[(int)spdContent];

                        m_motionController.YmcCallMPM001(spdParam.dblMaxVelocity, dblVelocity, (int)mItem.spdRecentParam.dblAccelTime,
                                                          (int)mItem.spdRecentParam.dblDecelTime, Xrest, Yrest, Xcenter, Ycenter, Radius, Pitch,
                                                           Repeat, (EN_BALL_ATTACH_MOTION)Dir);         
                    }
                }
                mItem.strUsingTask = strUsingTask;
                mItem.enMotionType = (Dir == 0) ? (EN_MOTION_TYPE.CIRCLE_CCW) : (EN_MOTION_TYPE.CIRCLE_CW);

                AddMovingMotion(mItem, uTimeout, uDelay);
            }

            return true;
        }

        /// <summary>
        /// 2019.03.29 by Thienvv [ADD] Add MoveCompare which use in VCM
        /// </summary>
        public bool MoveCompare(int nIndex, double dblDestination, int nCompareAxis, double dComparePosition, bool bLogic, bool bActual = true, uint uDelay = 0, string strUsingTask = MOVE_MANUAL)
        {
            if (false == m_dicOfMotionItem.ContainsKey(nIndex)
                || dblDestination == 0) { return false; }

            MotionItem mItem = m_dicOfMotionItem[nIndex];
            bool bInputShaping = false;
            uint uTimeout = 0;

            MOTOR_SPEED_PARAMETER spdLow = mItem.speedParam[(int)EN_MOTOR_CONTENTOFSPEED.CUSTOM_2];
            double uRatio = 1;
            double dblRatio = uRatio * 0.01;
            EN_MOTOR_CONTENTOFSPEED spdContent = EN_MOTOR_CONTENTOFSPEED.CUSTOM_2;

            if (false == PrepareForMoving(mItem, spdContent, dblDestination, 0.0, 100, ref uTimeout, ref bInputShaping))
            {
                return false;
            }
            else if (bInputShaping == false)
            {
                m_motionController.MoveCompare(mItem.nTarget, dblDestination, nCompareAxis, dComparePosition, bLogic, bActual);
            }

            mItem.strUsingTask = strUsingTask;
            mItem.enMotionType = EN_MOTION_TYPE.RELATIVE;

            AddMovingMotion(mItem, uTimeout, uDelay);

            return true;
        }
        /// <summary>
        /// 2018.07.12 by yjlee [ADD] 등속으로 축을 이동시킨다.
        /// </summary>
        /// <param name="nIndex"></param>
        /// <param name="nDir">방향</param>
        /// <param name="spdPattern">모션 프로파일</param>
        /// <param name="dblVelocity">속도</param>
        /// <param name="dblAccel">가속</param>
        /// <param name="dblDecel">감속</param>
        //public bool MoveSxAtSpeed(int nIndex, bool nDir, EN_SPEED_PATTERN spdPattern, double dblVelocity, double dblAccel, double dblDecel, double dblAccelJerk, double dblDecelJerk, uint uTimeout, string strUsingTask = MOVE_MANUAL)
        public bool MoveSxAtSpeed(int nIndex, bool nDir, EN_SPEED_PATTERN spdPattern, double dblVelocity, double dblAccel, double dblDecel, double dblAccelJerk, double dblDecelJerk, uint uTimeout, string strUsingTask = MOVE_MANUAL, uint uFilter = 100)
        {
            // 추후 확인 2021.07.13
            //if (false == m_dicOfMotionItem.ContainsKey(nIndex)) { return false; }

            //MotionItem mItem    = m_dicOfMotionItem[nIndex];
            //int nTarget = 0;

            //// 유효한 타겟이 아닐 경우
            //if (GetVaildTarget(mItem, ref nTarget) == false) { return false; }

            //// 모션이 구동 중일 경우
            //if (IsMotionMovingAtTarget(nTarget)) { return false; }

            //// Gantry 상태를 확인한다.
            //if (false == CheckStateOfGantry(mItem)) { return false; }

            //MOTOR_SPEED_PARAMETER stSpeed = new MOTOR_SPEED_PARAMETER();
            
            //stSpeed.enSpeedPattern     = spdPattern;
            //stSpeed.dblMaxVelocity     = dblVelocity;
            //stSpeed.dblVelocity        = dblVelocity;
            //stSpeed.dblAccelTime       = dblAccel;
            //stSpeed.dblDecelTime       = dblDecel;
            //stSpeed.uMotionTimeout     = uTimeout;
            //stSpeed.dblJerkAccelration = dblAccelJerk;
            //stSpeed.dblJerkDecelration = dblDecelJerk;
            //stSpeed.uFilter = uFilter;      //Mechatrolink

            //m_motionController.SetMotorSpeedConfiguration(ref nTarget, ref stSpeed);
            //m_motionController.MoveAtSpeed(ref nTarget, ref nDir);

            //#region Log 관련 파라미터 정보
            //mItem.strUsingTask  = strUsingTask;
            //mItem.enMotionType  = nDir ? EN_MOTION_TYPE.SPEED_POS : EN_MOTION_TYPE.SPEED_NEG;
            //mItem.spdRecentParam.dblVelocity = dblVelocity;
            //mItem.spdRecentParam.dblAccelTime = dblAccel;
            //mItem.spdRecentParam.dblDecelTime = dblDecel;

            //mItem.dblActualPosition = m_motionController.GetActualPosition(nTarget);
            //#endregion

            //AddMovingMotion(mItem, uTimeout);

            return true;
        }
        /// <summary>
        /// 2018.07.12 by yjlee [ADD] 등속으로 축을 이동시킨다.
        /// </summary>
        /// <param name="nIndex"></param>
        /// <param name="nDir">방향</param>
        /// <param name="nRatio">속도 비율 = 조그 (최고 속도 + 최소 속도) * 비율</param>
        public bool MoveSxAtSpeed(int nIndex, bool nDir, uint uRatio, string strUsingTask = MOVE_MANUAL)
        {
            // 201.07.13  by hkyu [COMMENT]  -  coding modify need................
            
            //if(uRatio < 1) return false;

            //MotionItem mItem    = m_dicOfMotionItem[nIndex];
            //int nTarget = 0;

            //if (GetVaildTarget(mItem, ref nTarget) == false) { return false; }

            //MotionOnly.MOTOR_SPEED_PARAMETER spdHigh   = mItem.speedParam[(int)EN_MOTOR_CONTENTOFSPEED.JOG_HIGH];
            //MotionOnly.MOTOR_SPEED_PARAMETER spdLow = mItem.speedParam[(int)EN_MOTOR_CONTENTOFSPEED.JOG_LOW];

            //double dblRatio    = uRatio * 0.01;

            //double dblVelocity  = spdLow.dblVelocity + ((spdHigh.dblVelocity - spdLow.dblVelocity) * dblRatio);
            //double dblAccel = spdLow.dblAccelTime + ((spdHigh.dblAccelTime - spdLow.dblAccelTime) * (1 - dblRatio));
            //double dblDecel = spdLow.dblDecelTime + ((spdHigh.dblDecelTime - spdLow.dblDecelTime) * (1 - dblRatio));

            //double dblAccelJerk = spdLow.dblJerkAccelration + ((spdHigh.dblJerkAccelration - spdLow.dblJerkAccelration) * (1 - dblRatio));
            //double dblDecelJerk = spdLow.dblJerkDecelration + ((spdHigh.dblJerkDecelration - spdLow.dblJerkDecelration) * (1 - dblRatio));

            //MoveSxAtSpeed(nIndex
            //    , nDir
            //    , spdHigh.enSpeedPattern
            //    , dblVelocity
            //    , dblAccel
            //    , dblDecel
            //    , dblAccelJerk
            //    , dblDecelJerk
            //    , spdHigh.uMotionTimeout > spdLow.uMotionTimeout ? spdHigh.uMotionTimeout : spdLow.uMotionTimeout
            //    , strUsingTask);

            return true;
        }
        /// <summary>
        /// 2018.09.13 by jhlim [ADD] 절대좌표계로 축을 예약 이동시킨다.
        /// </summary>        
        public bool MoveByList(int nIndex, int nLength, double[] arDestination, MotionInterfaces.MOTOR_HOME_PARAMETER[] arSpeedContent, double[] arManualVelocity, uint[] uRatio, uint uDelay, string strUsingTask = MOVE_MANUAL)
        {

        // 201.07.13  by hkyu [COMMENT]  -  coding modify need................

            //int nTarget             = 0;
            //MotionItem mItem        = m_dicOfMotionItem[nIndex];

            //if (GetVaildTarget(mItem, ref nTarget) == false) { return false; }

            //// 모션이 구동 중일 경우
            //if (IsMotionMovingAtTarget(nTarget)) { return false; }

            ////MotionOnly.MOTOR_SPEED_PARAMETER[] arSpeedParam = new MotionOnly.MOTOR_SPEED_PARAMETER[nLength];
            //Motion_.PARAM_SPEED[] arSpeedParam = new Motion_.PARAM_SPEED[nLength];

            //for (int i = 0 ; i < nLength ; ++ i)
            //{
            //    arSpeedParam[i]  = mItem.speedParam[(int)arSpeedContent[i]];                

            //    // 사용자가 메뉴얼 속도를 정의했을 경우
            //    if (arManualVelocity[i] > 0)
            //    {
            //        arSpeedParam[i].dblVelocity = arManualVelocity[i];
            //    }

            //    arSpeedParam[i].dblVelocity     = arSpeedParam[i].dblVelocity * mItem.nPower * uRatio[i] * 0.0001;
            //}
            
            //mItem.dblDestination    = arDestination[nLength - 1];
            //mItem.strUsingTask      = strUsingTask;
            //mItem.enMotionType      = EN_MOTION_TYPE.ABSOLUTE;

            //// 첫 번째 무빙 속도 파라메터를 설정한다.
            //m_motionController.MoveByList(nTarget, ref nLength, ref arDestination, ref arSpeedParam);

            //AddMovingMotion(mItem, arSpeedParam[0].uMotionTimeout, uDelay);

            return true;
        }
        /// <summary>
        /// 2019.04.30 by yjlee [ADD] 지정된 축들에 대해서 선형 보간 이동을 수행한다.
        /// 최대 3개의 축만 가능하다.
        /// * 속도는 첫 번째 축의 속도를 기준으로 한다.
        /// </summary>
        public bool MoveByLinearCoordination(int nCountOfAxises, int[] arIndexes, int nCountOfStep, double[,] arDestination, EN_MOTOR_CONTENTOFSPEED[] arContent, double[] arManualVelocity, uint[] arRatio, uint uDelay, string strUsingTask = MOVE_MANUAL)
        {
            // 201.07.13  by hkyu [COMMENT]  -  coding modify need................
            //MotionItem mItem = null;

            //// 에러 검사
            //for (int i = 0; i < nCountOfAxises; ++i)
            //{
            //    int nIndex = arIndexes[i];

            //    // 해당 인덱스의 아이템이 존재하지 않을 경우
            //    if (false == m_dicOfMotionItem.ContainsKey(nIndex))
            //    {
            //        return false;
            //    }

            //    mItem = m_dicOfMotionItem[nIndex];

            //    int nTarget = 0;

            //    // 유효한 타겟 번호가 아닐 경우
            //    if (false == GetVaildTarget(nIndex, ref nTarget)) { return false; }

            //    // 축이 구동 중일 경우
            //    if (IsMotionMoving(nTarget)) { return false; }
            //}

            //MOTOR_SPEED_PARAMETER[] arSpeedParameters = new MOTOR_SPEED_PARAMETER[nCountOfStep];
            //int[] arTarget = new int[nCountOfAxises];

            //// 첫 번째 축의 모션 아이템을 가져온다.
            //mItem = m_dicOfMotionItem[arIndexes[0]];

            //// 속도 파라미터를 설정한다.
            //for (int i = 0; i < nCountOfStep; ++i)
            //{
            //    arSpeedParameters[i] = mItem.speedParam[(int)arContent[i]];

            //    // 사용자가 정의한 속도가 존재할 경우
            //    if (arManualVelocity[i] > 0)
            //    {
            //        arSpeedParameters[i].dblVelocity = arManualVelocity[i];
            //    }

            //    arSpeedParameters[i].dblVelocity = arSpeedParameters[i].dblVelocity * arRatio[i] * 0.01;
            //}

            //// 모션 정보를 설정한다.
            //for (int i = 0; i < nCountOfAxises; ++i)
            //{
            //    mItem = m_dicOfMotionItem[arIndexes[i]];

            //    arTarget[i] = mItem.nTarget;

            //    // 가장 마지막 포지션을 목표 위치로 지정한다.
            //    mItem.dblDestination = arDestination[nCountOfStep - 1, i];
            //    mItem.enMotionType = EN_MOTION_TYPE.ABSOLUTE;
            //    mItem.strUsingTask = strUsingTask;
            //}

            //m_motionController.MoveByLinearCoordination(nCountOfAxises
            //    , arTarget
            //    , nCountOfStep
            //    , arDestination
            //    , arSpeedParameters);

            //// 이송한 모든 축 정보를 이동 관리에 추가한다.
            //for (int i = 0; i < nCountOfAxises; ++i)
            //{
            //    AddMovingMotion(m_dicOfMotionItem[arIndexes[i]]
            //        , arSpeedParameters[nCountOfStep - 1].uMotionTimeout   // 마지막 파라미터를 기준으로 수행한다.
            //        , uDelay);
            //}

            return true;
        }
        #endregion

        #region Override
        /// <summary>
        /// 2018.07.12 by yjlee [ADD] 모션의 속도를 재정의 한다.
        /// </summary>
        /// <param name="nIndex"></param>
        /// <param name="spdContent"></param>
        /// <param name="nRatio"></param>
        public void OverrideSxSpeed(int nIndex, EN_MOTOR_CONTENTOFSPEED spdContent, uint nRatio, string strUsingTask = MOVE_MANUAL)
        {
            if (false == m_dicOfMotionItem.ContainsKey(nIndex)) { return; }

            MotionItem mItem    = m_dicOfMotionItem[nIndex];
            int nTarget         = 0;

            if (GetVaildTarget(mItem, ref nTarget) == false) { return; }

            MOTOR_SPEED_PARAMETER spdParam  = mItem.speedParam[(int)spdContent];

            spdParam.dblVelocity    = spdParam.dblVelocity * nRatio * 0.01;

            // // 201.07.13  by hkyu [COMMENT]  -  coding modify need................
            //m_motionController.OverrideSxSpeed(nTarget, mItem.enMotionType, spdParam);            

            #region Log 관련 파라미터 정보
            mItem.strUsingTask                  = strUsingTask;
            mItem.enMotionType                  = EN_MOTION_TYPE.OVERRIDE;
            mItem.spdRecentParam.dblVelocity    = spdParam.dblVelocity;
            mItem.spdRecentParam.dblAccelTime   = spdParam.dblAccelTime;
            mItem.spdRecentParam.dblDecelTime   = spdParam.dblDecelTime;

            //m_InstanceLog.WriteMotionLog(mItem, true);
            #endregion
        }
        public void OverrideSxSpeed(int nIndex, EN_SPEED_PATTERN spdPattern, double dblVel, double dblAccel, double dblDecel, string strUsingTask = MOVE_MANUAL)
        {
            if (false == m_dicOfMotionItem.ContainsKey(nIndex)) { return; }

            MotionItem mItem = m_dicOfMotionItem[nIndex];
            int nTarget = 0;

            if (GetVaildTarget(mItem, ref nTarget) == false) { return; }

            MOTOR_SPEED_PARAMETER spdParam = new MOTOR_SPEED_PARAMETER(){ 
                enSpeedPattern    = spdPattern
                , dblVelocity = dblVel
                , dblAccelTime = dblAccel
                , dblDecelTime = dblDecel
            };

            // // 201.07.13  by hkyu [COMMENT]  -  coding modify need................
            // m_motionController.OverrideSxSpeed(nTarget, mItem.enMotionType, spdParam);

            #region Log 관련 파라미터 정보
            mItem.strUsingTask = strUsingTask;
            mItem.enMotionType = EN_MOTION_TYPE.OVERRIDE;
            mItem.spdRecentParam.dblVelocity = spdParam.dblVelocity;
            mItem.spdRecentParam.dblAccelTime = spdParam.dblAccelTime;
            mItem.spdRecentParam.dblDecelTime = spdParam.dblDecelTime;

            //m_InstanceLog.WriteMotionLog(mItem, true);
            #endregion
        }
        #endregion

        #region Home
        /// <summary>
        /// 2018.07.12 by yjlee [ADD] 모션의 홈을 수행한다.
        /// </summary>
        /// <param name="nIndex"></param>
        public void MoveToHome(int nIndex, string strUsingTask = MOVE_MANUAL)
        {
            if (false == m_dicOfMotionItem.ContainsKey(nIndex)) { return; }

            MotionItem mItem    = m_dicOfMotionItem[nIndex];
            int nTarget         = 0;

            if (GetVaildTarget(mItem, ref nTarget) == false) { return; }

            // 모션이 구동 중일 경우
            if (IsMotionMovingAtTarget(nTarget))
            {
                return;
            }
            
            // Gantry 상태를 확인한다.
            if (false == CheckStateOfGantry(mItem)) { return; }
            
            MOTOR_HOME_PARAMETER homeParam  = mItem.homeParam;

            m_listOfArState[nTarget][(int)EN_MOTOR_STATE.HOMETIMEOUT]   = false;

            // 2018.07.18 by yjlee [ADD] 항상 홈 종료일 경우
            if(homeParam.bAlwaysHomeEnd)
            {
                m_listOfArState[nTarget][(int)EN_MOTOR_STATE.HOMEEND]           = true;
                m_listOfArState[nTarget][(int)EN_MOTOR_STATE.MOTIONDONE]        = true;
                return;
            }

            int nPower  = mItem.nPower;

            homeParam.dblHomeVelocity  = homeParam.dblHomeVelocity * nPower * 0.01;

            #region Log 관련 파라미터 정보
            mItem.strUsingTask                  = strUsingTask;
            mItem.spdRecentParam.dblVelocity    = homeParam.dblHomeVelocity;
            mItem.spdRecentParam.dblAccelTime   = homeParam.dblHomeAccelTime;
            mItem.spdRecentParam.dblDecelTime   = homeParam.dblHomeDecelTime;
            #endregion

            // 호밍 수행
            // 2020.04.25 by Thienvv [MOD] Selectable Sensor type: Home 2-steps for MK-D12
            AddHomingMotion(mItem,homeParam.enHomeMode,  homeParam.uHomeTimeout);
        }

        /// <summary>
        /// 2020.04.22 by Thienvv [ADD]
        /// </summary>
        public void MoveToHomeByLimitSensor(int nIndex, EN_LIMIT_POS enLimitPos, string strUsingTask = MOVE_MANUAL)
        {
            if (false == m_dicOfMotionItem.ContainsKey(nIndex)) { return; }

            MotionItem mItem = m_dicOfMotionItem[nIndex];
            int nTarget = 0;

            if (GetVaildTarget(mItem, ref nTarget) == false) { return; }

            // 모션이 구동 중일 경우
            if (IsMotionMovingAtTarget(nTarget))
            {
                return;
            }

            // Gantry 상태를 확인한다.
            if (false == CheckStateOfGantry(mItem)) { return; }

            MOTOR_HOME_PARAMETER homeParam = mItem.homeParam;

            //2020.09.18 nogami modify start
            if (enLimitPos == EN_LIMIT_POS.NEGATIVE)
            {
                switch (m_enMotionControlType)
                {
                    case EN_MOTIONController.MKUNIT:
                        homeParam.enHomeMode = EN_MOTOR_HOMEMODE.H4_MINUS_DIR_EL_BACK_P_EDGE;
                        break;
                    case EN_MOTIONController.MECHATROLINK:
                        homeParam.enHomeMode = EN_MOTOR_HOMEMODE.HMETHOD_NOT_ONLY;
                        break;
                    default:
                        return;
                }
            }
            else if (enLimitPos == EN_LIMIT_POS.POSITIVE)
            {
                switch (m_enMotionControlType)
                {
                    case EN_MOTIONController.MKUNIT:
                        homeParam.enHomeMode = EN_MOTOR_HOMEMODE.H5_PLUS_DIR_EL_BACK_P_EDGE;
                        break;
                    case EN_MOTIONController.MECHATROLINK:
                        homeParam.enHomeMode = EN_MOTOR_HOMEMODE.HMETHOD_POT_ONLY;
                        break;
                    default:
                        return;
                }
            }
            //2020.09.18 nogami modify end

            m_listOfArState[nTarget][(int)EN_MOTOR_STATE.HOMETIMEOUT] = false;

            // 2018.07.18 by yjlee [ADD] 항상 홈 종료일 경우
            if (homeParam.bAlwaysHomeEnd)
            {
                m_listOfArState[nTarget][(int)EN_MOTOR_STATE.HOMEEND] = true;
                m_listOfArState[nTarget][(int)EN_MOTOR_STATE.MOTIONDONE] = true;
                return;
            }

            int nPower = mItem.nPower;

            homeParam.dblHomeVelocity = homeParam.dblHomeVelocity * nPower * 0.01;

            #region Log 관련 파라미터 정보
            mItem.strUsingTask = strUsingTask;
            mItem.spdRecentParam.dblVelocity = homeParam.dblHomeVelocity;
            mItem.spdRecentParam.dblAccelTime = homeParam.dblHomeAccelTime;
            mItem.spdRecentParam.dblDecelTime = homeParam.dblHomeDecelTime;
            #endregion

            // 호밍 수행
            AddHomingMotion(mItem, homeParam.enHomeMode, homeParam.uHomeTimeout);
        }
        #endregion

        #endregion

        #endregion

        #region 초기화 및 종료
        // 장치 초기화 및 기본 동작을 수행한다.
        public bool Init(EN_MOTIONController motionController, string dllFileName, int nCount)
        {
            // 2019.08.18. by shkim [MOD]    
            #region RSA - DLL 파일 필요없음으로 주석처리
            string path = System.IO.Directory.GetCurrentDirectory();
            string strFullFilePath = path + "\\" + dllFileName;


            // DLL 파일이 없을 경우
            System.IO.FileInfo fi = new System.IO.FileInfo(strFullFilePath);
            if (!fi.Exists) return false;
            #endregion

            //m_InstanceLog   = Log.GetInstance();
            m_enMotionControlType = motionController; // 2020.03.10 by Thienvv [ADD] 

            // Motion 컨트롤러 할당
            switch (motionController)
            {
                case EN_MOTIONController.MKUNIT:
                    //m_motionController = new MkUnitMotionController();
                    break;
                case EN_MOTIONController.RSA:
                    //m_motionController = new RSAMotionController();
                    break;
                //case EN_MOTIONController.MECHATROLINK:
                //    m_motionController = new MechatrolinkMotionController();  // 2021.07.13 -> 4차 구조에서 사용 않함.
                //    break;
            }

            // 모션 컨트롤러를 초기화한다. 성공 시 현재 연결된 축 수를 반환한다.
            if (1 > (m_nCountOfAxis = m_motionController.InitController()))
            {
                //return false;
            }
            else
            {
                for (int i = 0; i < m_nCountOfAxis; ++ i)
                {
                    m_motionItemManager.Add();

                    m_listOfArState.Add(new bool[(int)EN_MOTOR_STATE.MAXCOUNT]);

                    m_listOfMotionStatus.Add(EN_MOTION_STATUS.READY);
                    m_listOfMovingMotion.Add(new MovedMotion());
                    m_listOfRepeatingItem.Add(new RepeatItem());
                }

                m_bInit = true;
            }

            bool bLoaded = m_motionItemManager.Load();
            bool bLoaded2 = m_CompensatorInstance.Load();

            m_dicOfMotionItem = m_motionItemManager.dicOfMotionItem;   /// -----><--------------

            // 파라미터 설정을 수행한다.
            m_bNeedToSetParameter       = true;
                        
            // 파라미터 설정 전이므로, 컨트롤러 사용을 불허한다.
            m_motionController.bInit    = false;

            ///  필요여부 확인 요망 ... 201.07.13             
            //base.Notify(OBSERVER_COMMAND.ADD);
            //base.Notify(OBSERVER_COMMAND.CHANGE);            

            return m_bInit && bLoaded && bLoaded2;
        }

        /// 2021.08.07  by hkyu  [ADD] -  for 4th framework .( motion index matching )
        public bool InitMotionIndex()
        {
            bool bLoaded = m_motionItemManager.Load();

            m_dicOfMotionItem = m_motionItemManager.dicOfMotionItem;

            return bLoaded;
        }


        // 2018.05.02 by yjlee [ADD] 소프트웨어 종료 시 처리해야할 부분을 명세한다.
        public void Exit()
        {
            if (m_motionController == null) return;

            m_motionController.ExitController();
        }
        #endregion

        #region 주기 호출 함수
        /// <summary>
        /// 2018.07.11 by yjlee [ADD] 메인 쓰레드에 의해 주기적으로 호출되는 함수이다.
        /// </summary>
        public void Execute()
        {
            if (m_bInit == false)
            {
                return;
            }

            try
            {
                foreach (var item in m_dicOfMotionItem)
                {
                    MotionItem mItem    = item.Value;

                    int nTarget         = mItem.nTarget;

                    if (nTarget < 0 || nTarget >= m_nCountOfAxis) { continue;}

                    if (mItem.bEnable == false)
                    {
                        if (m_listOfMotionStatus[nTarget] != EN_MOTION_STATUS.READY)
                        {
                            m_listOfMotionStatus[nTarget] = EN_MOTION_STATUS.STOP;
                        }
                        else
                        {
                            continue;
                        }
                    }

                    bool bMotionEnd = false;

                    // 모션 상태에 따른 동작
                    switch (m_listOfMotionStatus[nTarget])
                    {
                        case EN_MOTION_STATUS.MOVING:
                            DoMove(mItem);
                            break;
                        case EN_MOTION_STATUS.HOMING:
                        case EN_MOTION_STATUS.ELHOMING:
                            DoHoming(mItem);
                            break;
                        case EN_MOTION_STATUS.STOP:
                            bMotionEnd  = DoStop(nTarget);
                            break;
                        case EN_MOTION_STATUS.DELAY:
                            bMotionEnd  = DoDelay(nTarget);
                            break;
                    }

                    // 모션 동작이 종료되었을 경우
                    if(bMotionEnd)
                    {
                       // m_tokenManagement.ReturnTokenToMaster(item.Key);

                        m_listOfArState[nTarget][(int)EN_MOTOR_STATE.MOTIONDONE]    = true;

                        mItem.dblActualPosition     = m_motionController.GetActualPosition(nTarget);
                        mItem.dblCommandPosition    = m_motionController.GetCommandPosition(nTarget);

                        m_listOfMotionStatus[nTarget]   = EN_MOTION_STATUS.READY;

//                        m_InstanceLog.WriteMotionLog(mItem, false);

                    }

                    // 현재 아이템이 가르키는 타겟이 리핏 중일 경우
                    if(m_listOfRepeatingItem[nTarget].bStart)
                    {
                        // 모션에 에러가 존재할 경우
                        if(CheckMotionError(mItem))
                        {
                            // 리핏을 중지한다.
                            m_listOfRepeatingItem[nTarget].StopRepeat();
                        }
                        else
                        {
                            // 2019.09.26. by shkim [MOD] REPEAT 모션에 횟수 지정 가능하도록 수정
                            if (m_listOfRepeatingItem[nTarget].RepeatCount != 0)
                            {
                                if (m_listOfRepeatingItem[nTarget].RepeateadCount >= m_listOfRepeatingItem[nTarget].RepeatCount)
                                {
                                    m_listOfRepeatingItem[nTarget].StopRepeat();
                                }
                                else
                                {
                                    DoRepeat(item.Key, nTarget);
                                }
                            }
                            else
                            {
                                DoRepeat(item.Key, nTarget);
                            }
                        }
                    }
                }
            }
            catch(InvalidOperationException e)
            {
                System.Console.WriteLine(e.ToString());
            }

            //TimeChecker.EndChecking(EN_TIMECHECKLIST.MOTION);
        }
        /// <summary>
        /// 2018.12.17 by yjlee [ADD] 네트워크 타입 컨트롤러의 경우, 링크 상태가 정상인지 판단한다.
        /// </summary>
        public void ExecuteForSubWorking()
        {
            // 컨트롤러 초기화가 되지 않은 상태일 경우
            if(false == m_bInit)
            {
                return;
            }

            // 모터 파라미터 설정이 필요한 경우
            if(m_bNeedToSetParameter)
            {
                // 네트워크 연결이 완료된 경우에만 진행한다.
                if(IsLinkConnected())
                {
                    switch (m_nSeqNumberForConnection)
                    {
                        case 0:
                            // 연결이 된 후에도 드라이버 및 모터 초기화 시간을 대기한다.
                            m_tcForConnectionDelay.SetTickCount(c_uDelayForInitDriver);
                            ++ m_nSeqNumberForConnection;
                            break;
                        case 1:
                            if(m_tcForConnectionDelay.IsTickOver(true))
                            {
                                ++ m_nSeqNumberForConnection;
                            }
                            break;
                        case 2:
                            m_motionController.bInit    = true;
                            
                            SetMotorConfig();
                            
                            ++ m_nSeqNumberForConnection;
                            break;
                        case 3:
                            m_nSeqNumberForConnection   = 0;
                            m_bNeedToSetParameter       = false;
                            break;
                    }
                }
            }

            // 네트워크 연결 상태를 관리한다.
            CheckLinkIsConnected();
        }
        /// <summary>
        /// 2019.04.04 by yjlee [ADD] 연결된 상태인지 확인한다.
        /// </summary>
        public bool IsLinkConnected()
        {
            if (null == m_motionController) { return false; }

            return m_motionController.IsLinkConnected();
        }
        /// <summary>
        /// 2019.04.04 by yjlee [ADD] 연결 시도 중인지 확인한다.
        /// </summary>
        public bool IsTryingToConnect()
        {
            return m_bTryingToConnect;
        }
        //2020.03.09 nogami add start
        public bool IsSoftwareLimitNegativeCheck(int nIndex, EN_MOTION_TYPE type, double dblDestination)
        {
            MotionItem mItem = m_dicOfMotionItem[nIndex];

            if (mItem.configParam.bUseSWLimitPositive)
            {
                // 현재 포지션을 읽어온다.
                double dblActualPosition = dblDestination;
                if( type == EN_MOTION_TYPE.RELATIVE ) 
                {
                    dblActualPosition += GetActualPosition(nIndex);
                }
                    

                // 리밋 포지션 초과일 경우
                if (dblActualPosition < mItem.configParam.dblSWLimitPositionNegative)
                {
                    return( true );
                }
            }
            return (false);
        }
        public bool IsSoftwareLimitPositiveCheck(int nIndex, EN_MOTION_TYPE type, double dblDestination)
        {
            MotionItem mItem = m_dicOfMotionItem[nIndex];

            if (mItem.configParam.bUseSWLimitPositive)
            {
                // 현재 포지션을 읽어온다.
                double dblActualPosition = dblDestination;
                if (type == EN_MOTION_TYPE.RELATIVE)
                {
                    dblActualPosition += GetActualPosition(nIndex);
                }


                // 리밋 포지션 초과일 경우
                if (dblActualPosition > mItem.configParam.dblSWLimitPositionPositive)
                {
                    return (true);
                }
            }
            return (false);
        }
        //2020.03.09 nogami add end
        #endregion

        #region 아이템 값 설정
        /// <summary>
        /// 2018.07.09 by yjlee [ADD] 선택된 아이템을 설정한다.
        /// </summary>
        /// <param name="nIndex"></param>
        public void SetSelectedItem(int nIndex)
        {
            m_motionItemManager.SetSelectedItem(nIndex);
        }
        /// <summary>
        /// 2018.07.10 by yjlee [ADD] 아이템의 활성 상태를 설정한다.
        /// </summary>
        /// <param name="bEnable"></param>
        public void SetEnable(bool bEnable)
        {
            if(m_motionItemManager.SetEnable(bEnable))
            {
                /// 2021.07.13 사용여부 확인 요망.
                //base.Notify(OBSERVER_COMMAND.CHANGE);
            }
        }
        /// <summary>
        /// 2018.07.09 by yjlee [ADD] 아이템의 이름을 설정한다.
        /// </summary>
        /// <param name="strName"></param>
        public void SetName(string strName)
        {
            if(m_motionItemManager.SetName(strName))
            {
                /// 2021.07.13 사용여부 확인 요망.
                /// 
                //base.Notify(OBSERVER_COMMAND.CHANGE);
            }
        }
        /// <summary>
        /// 2018.07.09 by yjlee [ADD] 아이템의 타겟을 설정한다.
        /// </summary>
        /// <param name="strName"></param>
        public void SetTarget(int nTarget)
        {
            if (m_motionItemManager.SetTarget(nTarget))
            {
                /// 2021.07.13 사용여부 확인 요망.
                /// 
                // base.Notify(OBSERVER_COMMAND.CHANGE);
            }
        }
        /// <summary>
        /// 2018.07.09 by yjlee [ADD] 아이템의 모터 타입을 설정한다.
        /// </summary>
        /// <param name="strName"></param>
        public void SetMotorType(int nIndex, string strType)
        {
            if(false == m_dicOfMotionItem.ContainsKey(nIndex))
            {
                return;
            }

            MotionItem mItem    = m_dicOfMotionItem[nIndex];

            if (m_motionItemManager.SetMotorType(strType))
            {
                m_motionController.SetMotorType(mItem.nTarget
                    , mItem.strMotorType.Equals(EN_MOTOR_TYPE.SERVO.ToString()) ? EN_MOTOR_TYPE.SERVO : EN_MOTOR_TYPE.STEP);
                
                /// 2021.07.13 사용여부 확인 요망.
                /// 
                //base.Notify(OBSERVER_COMMAND.CHANGE);
            }
        }
        /// <summary>
        /// 2018.07.09 by yjlee [ADD] 아이템의 파워를 설정한다.
        /// </summary>
        public void SetPower(int nPower)
        {
            if (m_motionItemManager.SetPower(nPower))
            {
                /// 2021.07.13 사용여부 확인 요망.
                /// 
                //base.Notify(OBSERVER_COMMAND.CHANGE);
            }
        }
        /// <summary>
        /// 2018.07.18 by yjlee [ADD] 조그 그룹 번호를 설정한다.
        /// </summary>
        public bool SetGroupOfJog(int nGroup)
        {
            return m_motionItemManager.SetGroupOfJog(nGroup);
        }
        #endregion

        #region 아이템 값 반환
        /// <summary>
        /// 2018.07.09 by yjlee [ADD] 해당 아이템의 활성 상태를 반환한다.
        /// </summary>
        /// <param name="nIndex"></param>
        /// <returns></returns>
        public bool GetEnable(int nIndex)
        {
            if(false == m_dicOfMotionItem.ContainsKey(nIndex)){return false;}

            return m_dicOfMotionItem[nIndex].bEnable;
        }
        /// <summary>
        /// 2018.08.18 by yjlee [ADD] 해당 아이템의 이름을 가져온다.
        /// </summary>
        public string GetName(int nIndex)
        {
            if (false == m_dicOfMotionItem.ContainsKey(nIndex)) { return NONE; }

            return m_dicOfMotionItem[nIndex].strName;
        }
        /// <summary>^
        /// 2018.07.10 by yjlee [ADD] 해당 아이템의 모터 타입을 반환한다.
        /// </summary>
        /// <param name="nIndex"></param>
        /// <returns></returns>
        public string GetMotorType(int nIndex)
        {
            if (false == m_dicOfMotionItem.ContainsKey(nIndex)) { return NONE; }

            return m_dicOfMotionItem[nIndex].strMotorType;
        }
        /// <summary>
        /// 2018.07.10 by yjlee [ADD] 해당 아이템의 파워를 반환한다.
        /// </summary>
        /// <param name="nIndex"></param>
        /// <returns></returns>
        public int GetMotorPower(int nIndex)
        {
            if (false == m_dicOfMotionItem.ContainsKey(nIndex)) { return 0; }

            return m_dicOfMotionItem[nIndex].nPower;
        }
        /// <summary>
        /// 2018.07.18 by yjlee [ADD] 해당 아이템의 조그 그룹을 반환한다.
        /// </summary>
        public int GetGroupOfJog(int nIndex)
        {
            if (false == m_dicOfMotionItem.ContainsKey(nIndex)) { return -1; }

            return m_dicOfMotionItem[nIndex].nGroupOfJog;
        }
        /// <summary>
        /// 2019.04.21 by yjlee [ADD] 해당 아이템의 SW 리밋 역방향 값을 읽어온다.
        /// </summary>
        public double GetSWLimitNegative(int nIndex)
        {
            if (false == m_dicOfMotionItem.ContainsKey(nIndex)) { return 0; }

            return m_dicOfMotionItem[nIndex].configParam.dblSWLimitPositionNegative;
        }
        /// <summary>
        /// 2019.04.21 by yjlee [ADD] 해당 아이템의 SW 리밋 정방향 값을 읽어온다.
        /// </summary>
        public double GetSWLimitPositive(int nIndex)
        {
            if (false == m_dicOfMotionItem.ContainsKey(nIndex)) { return 0; }

            return m_dicOfMotionItem[nIndex].configParam.dblSWLimitPositionPositive;
        }
        #endregion

        #region 등록 관련 함수
        /// <summary>
        /// 2019.02.05 by yjlee [MOD] 태스크에서 사용할 모션을 등록한다.
        /// </summary>
        //public bool RegisterMotion(string strTaskName, int nKey, int nIndex, ref RegisteredMotion rMotion)
        //{

            /// 2021.07.13 사용여부 확인 요망.
            /// 
            //// 동일한 축이 이미 등록되었을 경우
            //if (m_dicOfRegisteredMotion.ContainsKey(nIndex))
            //{
            //    return false;
            //}

            //rMotion = new RegisteredMotion(GetInstance()
            //    , strTaskName
            //    , nKey
            //    , nIndex);

            //m_dicOfRegisteredMotion.Add(nIndex
            //    , rMotion);

        //    return true;
        //}
        /// <summary>
        /// 2019.02.05 by yjlee [MOD] 공유 모션을 등록한다.
        /// </summary>
//        public bool RegisterSharedMotion(string strTaskName, int nKey, int nIndexOfMotionItem, bool bMaster, ref SharedMotion sMotion)
        //{

            /// 2021.07.13 사용여부 확인 요망.
            /// 

            //int nIndexOfSharedList  = -1;

            //// 사용 가능한 공유 객체 인덱스를 얻어온다.
            //if(false == m_tokenManagement.GetAvailableIndex(nIndexOfMotionItem
            //    , bMaster
            //    , ref nIndexOfSharedList))
            //{
            //    return false;
            //}

            //sMotion = new SharedMotion(GetInstance()
            //    , strTaskName
            //    , nKey
            //    , nIndexOfMotionItem
            //    , nIndexOfSharedList
            //    , bMaster);

            //// 생성한 공유 객체를 등록한다.
            //return m_tokenManagement.AddSharedObject(nIndexOfMotionItem
            //    , nIndexOfSharedList
            //    , sMotion);


          //  return false;
        //}
        /// <summary>
        /// 2019.02.05 by yjlee [ADD] 상태 정보를 확인하기 위한 모션을 등록한다.
        /// </summary>
        /// 
        /// 2021.07.13 확인 요망......
        /// 
        //public CheckedMotion RegisterCheckedMotion(int nIndex)
        //{
        //    return new CheckedMotion(GetInstance()
        //        , nIndex);
        //}
        #endregion

        #region Repeat 관련
        /// <summary>
        /// 2018.07.11 by yjlee [ADD] 리핏 운전 설정 수행한다.
        /// 운전 중일 경우 정지, 정지 상태일 경우 운전 상태가 된다.
        /// </summary>
        /// <param name="nIndex"></param>
        public void SetRepeat(int nIndex, double dblPosFirst, uint uDelayFirst, double dblPosSecond, uint uDelaySecond, uint uRepeatCount = 0)
        {
            if (false == m_dicOfMotionItem.ContainsKey(nIndex)) { return; }

            MotionItem mItem    = m_dicOfMotionItem[nIndex];

            int nTarget         = 0;

            if (false == GetVaildTarget(mItem, ref nTarget)) { return; }

            RepeatItem rItem    = m_listOfRepeatingItem[nTarget];

            // 운전 중일 경우
            if (rItem.bStart)
            {
                rItem.StopRepeat();
            }
            else
            {
                // 모션이 이송 중일 경우
                if (m_listOfMotionStatus[nTarget] != EN_MOTION_STATUS.READY) { return; }

                // 2019.09.26. by shkim. [ADD] REPEAT 카운트
                rItem.RepeateadCount = 0;

                rItem.StartRepeat(dblPosFirst
                        , uDelayFirst
                        , dblPosSecond
                        , uDelaySecond
                        , uRepeatCount);
            }
        }
        /// <summary>
        /// 2018.08.30 by yjlee [ADD] 모션이 리핏 동작 중인지 반환한다.
        /// </summary>
        public bool IsMotionRepeating(int nIndex)
        {
            if (false == m_dicOfMotionItem.ContainsKey(nIndex)) { return false; }

            int nTarget     = 0;

            if (false == GetVaildTarget(m_dicOfMotionItem[nIndex], ref nTarget)) { return false; }

            return m_listOfRepeatingItem[nTarget].bStart;
        }

        // 2019.09.26. by shkim. [ADD] REPEAT 카운트
        public uint GetRepeatedCount(int nIndex)
        {
            if (false == m_dicOfMotionItem.ContainsKey(nIndex)) { return 0; }

            MotionItem mItem = m_dicOfMotionItem[nIndex];

            int nTarget = 0;

            if (false == GetVaildTarget(mItem, ref nTarget)) { return 0; }

            RepeatItem rItem = m_listOfRepeatingItem[nTarget];

            return rItem.RepeateadCount;
        }


        #endregion

        #region 공유 모션
        /// <summary>
        /// 2019.02.06 by yjlee [ADD] 토큰을 요청한다.
        /// </summary>
        /// 2021.07.13 사용 유무 확인 요망
        /// 
     //   public bool RequestToken(int nIndexOfMotionItem, int nIndexOfRequestItem)
     //   {
     //       // 해당 모션 아이템이 존재하지 않을 경우
     //       if (false == m_bInit
     //           || false == m_dicOfMotionItem.ContainsKey(nIndexOfMotionItem)) { return false; }

     //       MotionItem mItem    = m_dicOfMotionItem[nIndexOfMotionItem];
     //       int nTarget         = mItem.nTarget;

     //       // 모션이 대기 상태가 아닐 경우
     //       if(m_listOfMotionStatus[nTarget] != EN_MOTION_STATUS.READY)
     //       {
     //           return false;
     //       }

     ////       return m_tokenManagement.RequestToken(nIndexOfMotionItem
     //           //, nIndexOfRequestItem);
     //   }
        #endregion

        #region 파라미터 관련

        #region 파라미터 값 반환

        #region Config
        /// <summary>
        /// 2018.08.21 by jhlim [ADD] 컨피그 파라미터 구조체를 반환한다.
        /// </summary>
        /// <param name="nIndex"></param>
        /// <returns></returns>
        public MOTOR_CONFIG_PARAMETER GetConfigParameter(int nIndex)
        {
            if(false == m_dicOfMotionItem.ContainsKey(nIndex))
            {
                return new MOTOR_CONFIG_PARAMETER();
            }

            return m_dicOfMotionItem[nIndex].configParam;
        }
        #endregion

        #region Speed
        /// <summary>
        /// 2018.07.15 by yjlee [ADD] 스피드 파라미터 구조체를 반환한다.
        /// </summary>
        public MOTOR_SPEED_PARAMETER GetSpeedParameter(int nIndex, int nIndexOfContents)
        {
            return new MOTOR_SPEED_PARAMETER();

            ///-------------
            ///-
            ///
            if (false == m_dicOfMotionItem.ContainsKey(nIndex))
            {
                return new MOTOR_SPEED_PARAMETER();
            }

            return m_dicOfMotionItem[nIndex].speedParam[nIndexOfContents];
        }
        #endregion

        #region Home
        /// <summary>
        /// 2018.07.15 by yjlee [ADD] 홈 파라미터 구조체를 반환한다.
        /// </summary>
        /// <param name="nIndex"></param>
        /// <returns></returns>
        public MOTOR_HOME_PARAMETER GetHomeParameter(int nIndex)
        {
            return new MOTOR_HOME_PARAMETER();

            ///----------------
            /// m_dicOfMotionItem -------- check

            if (false == m_dicOfMotionItem.ContainsKey(nIndex)) 
            { 
                return new MOTOR_HOME_PARAMETER();
            }

            return m_dicOfMotionItem[nIndex].homeParam;
        }
        #endregion

        #region Gantry
        /// <summary>
        /// 2018.07.18 by yjlee [ADD] 해당 아이템이 겐트리 마스터인지 확인한다.
        /// </summary>
        public bool IsMasterOfGantry(int nIndex)
        {
            if (false == m_dicOfMotionItem.ContainsKey(nIndex)) { return false; }

            return m_dicOfMotionItem[nIndex].gantryParam.bMaster;
        }
        /// <summary>
        /// 2018.07.18 by yjlee [ADD] 해당 아이템이 겐트리 슬레이브인지 확인한다.
        /// </summary>
        public bool IsSlaveOfGantry(int nIndex)
        {
            if (false == m_dicOfMotionItem.ContainsKey(nIndex)) { return false; }

            return m_dicOfMotionItem[nIndex].gantryParam.bSlave;
        }
        /// <summary>
        /// 2018.07.18 by yjlee [ADD] 해당 아이템의 겐트리 마스터 아이템 이름을 반환한다.
        /// </summary>
        public string GetNameOfMaster(int nIndex)
        {
            if (m_dicOfMotionItem.ContainsKey(nIndex))
            {
                int nIndexOfMaster  = m_dicOfMotionItem[nIndex].gantryParam.nIndexOfMaster;

                if (DicOfItemName.ContainsKey(nIndexOfMaster))
                {
                    return DicOfItemName[nIndexOfMaster];
                }
            }

            return NONE;
        }
        /// <summary>
        /// 2018.07.18 by yjlee [ADD] 해당 아이템의 겐트리 마스터 아이템 이름을 반환한다.
        /// </summary>
        public string GetNameOfSlave(int nIndex)
        {
            if (m_dicOfMotionItem.ContainsKey(nIndex))
            {
                int nIndexOfSlave = m_dicOfMotionItem[nIndex].gantryParam.nIndexOfSlave;

                if (DicOfItemName.ContainsKey(nIndexOfSlave))
                {
                    return DicOfItemName[nIndexOfSlave];
                }
            }

            return NONE;
        }
        /// <summary>
        /// 2018.07.19 by yjlee [ADD] 슬레이브 축의 반전 여부를 반환한다.
        /// </summary>
        /// <param name="nIndex"></param>
        /// <returns></returns>
        public bool GetSlaveInverse(int nIndex)
        {
            if (false == m_dicOfMotionItem.ContainsKey(nIndex)) { return false; }

            return m_dicOfMotionItem[nIndex].gantryParam.bSlaveInverse;
        }
        #endregion

        #endregion

        #region 파라미터 값 설정
        
        #region Config
        /// <summary>
        /// 2018.08.21 by jhlim [ADD] 모터 어플리케이션 종류를 입력한다.
        /// </summary>
        public bool SetMotorApplicationType(string strType)
        {
            if(m_motionItemManager.SetMotorApplicationType(strType))
            {
                int nIndex = m_motionItemManager.GetIndexOfSelectedItem();

                MotionItem mItem = m_dicOfMotionItem[nIndex];

                m_motionController.SetMotorType(mItem.nTarget
                    , m_dicOfMotionItem[nIndex].enMotorType);

                return true;
            }

            return false;
        }
        /// <summary>
        /// 2019.04.27 by yjlee [MOD] 엔코더 분해능(분자) 설정을 한다.
        /// </summary>
        public bool SetNumeratorForScale(int nIndex, double dblNumerator)
        {
            if (false == m_dicOfMotionItem.ContainsKey(nIndex)) { return false; }

            // 기존 값과 동일한지 검사
            if (false == m_motionItemManager.SetNumeratorForScale(dblNumerator)) { return false; }

            SetMotorConfiguration(m_dicOfMotionItem[nIndex]);

            return true;
        }
        /// <summary>
        /// 2019.04.27 by yjlee [MOD] 1회전 당 거리(분모)를 설정한다.
        /// </summary>
        public bool SetDenominatorForScale(int nIndex, double dblDenominator)
        {
            if (false == m_dicOfMotionItem.ContainsKey(nIndex)) { return false; }

            // 기존 값과 동일한지 검사한다.
            if (false == m_motionItemManager.SetDenominatorForScale(dblDenominator)) { return false; }

            SetMotorConfiguration(m_dicOfMotionItem[nIndex]);

            return true;
        }
        /// <summary>
        /// 2019.04.23 by yjlee [ADD] 커맨드 방향을 설정한다.
        /// </summary>
        public bool SetCommandDirection(int nIndex, string strCommandDirection)
        {
            if (false == m_dicOfMotionItem.ContainsKey(nIndex)) { return false; }

            if (false == m_motionItemManager.SetCommandDirection(strCommandDirection)) { return false; }

            SetMotorConfiguration(m_dicOfMotionItem[nIndex]);

            return true;
        }
        /// <summary>
        /// 2019.04.23 by yjlee [ADD] 커맨드 방향을 설정한다.
        /// </summary>
        public bool SetEncoderDirection(int nIndex, string strEncoderDirection)
        {
            if (false == m_dicOfMotionItem.ContainsKey(nIndex)) { return false; }

            if (false == m_motionItemManager.SetEncoderDirection(strEncoderDirection)) { return false; }

            SetMotorConfiguration(m_dicOfMotionItem[nIndex]);

            return true;
        }
        /// <summary>
        /// 2018.08.21 by jhlim [ADD] HW Limit 사용유무를 설정한다.
        /// 2019.04.27 by yjlee [MOD] HW 리밋 사용 유무를 설정한다.
        /// 2020.04.25 by Thienvv [ADD] Servo Off Flag
        /// </summary>
        public bool SetUseHWLimitPositive(bool bEnable, bool bDoServoOff = true)
        {
            if(m_motionItemManager.SetUseHWLimitPositive(bEnable))
            {
                int nIndex = m_motionItemManager.GetIndexOfSelectedItem();

                SetMotorConfiguration(m_dicOfMotionItem[nIndex], bDoServoOff);

                return true;
            }

            return false;
        }
        /// <summary>
        /// 2018.08.21 by jhlim [ADD] HW Limit 사용유무를 설정한다.
        /// 2019.04.27 by yjlee [MOD] HW 리밋 사용 유무를 설정한다.
        /// 2020.04.25 by Thienvv [ADD] Servo Off Flag
        /// </summary>
        public bool SetUseHWLimitNegative(bool bEnable, bool bDoServoOff = true)
        {
            if(m_motionItemManager.SetUseHWLimitNegative(bEnable))
            {
                int nIndex = m_motionItemManager.GetIndexOfSelectedItem();

                SetMotorConfiguration(m_dicOfMotionItem[nIndex], bDoServoOff);

                return true;
            }

            return false;
        }
        /// <summary>
        /// 2018.08.21 by jhlim [ADD] HW Limit 로직을 설정한다.
        /// 2019.04.27 by yjlee [MOD] HW 리밋 로직을 설정한다.
        /// </summary>
        public bool SetHWLimitLogicPositive(string strLogic)
        {
           if(m_motionItemManager.SetHWLimitLogicPositive(strLogic))
           {
               int nIndex = m_motionItemManager.GetIndexOfSelectedItem();

               SetMotorConfiguration(m_dicOfMotionItem[nIndex]);

               return true;
           }

           return false;
        }
        /// <summary>
        /// 2018.08.21 by jhlim [ADD] HW Limit 로직을 설정한다.
        /// 2019.04.27 by yjlee [MOD] HW 리밋 로직을 설정한다.
        /// </summary>
        public bool SetHWLimitLogicNegative(string strLogic)
        {
           if(m_motionItemManager.SetHWLimitLogicNegative(strLogic))
           {
               int nIndex = m_motionItemManager.GetIndexOfSelectedItem();

               SetMotorConfiguration(m_dicOfMotionItem[nIndex]);

               return true;
           }

           return false;
        }
        /// <summary>
        /// 2018.08.21 by jhlim [ADD] HW Limit 스탑모드를 설정한다.
        /// 2019.04.27 by yjlee [MOD] HW 리밋 정지 모드를 설정한다.
        /// </summary>
        public bool SetHWLimitStopModePositive(string strStopMode)
        {
            if(m_motionItemManager.SetHWLimitStopModePositive(strStopMode))
            {
                int nIndex = m_motionItemManager.GetIndexOfSelectedItem();

                SetMotorConfiguration(m_dicOfMotionItem[nIndex]);

                return true;
            }

            return false;
        }
        /// <summary>
        /// 2018.08.21 by jhlim [ADD] HW Limit 스탑모드를 설정한다.
        /// 2019.04.27 by yjlee [MOD] HW 리밋 정지 모드를 설정한다.
        /// </summary>
        public bool SetHWLimitStopModeNegative(string strStopMode)
        {
           if(m_motionItemManager.SetHWLimitStopModeNegative(strStopMode))
           {
               int nIndex = m_motionItemManager.GetIndexOfSelectedItem();

               SetMotorConfiguration(m_dicOfMotionItem[nIndex]);

               return true;
           }

           return false;
        }
        /// <summary>
        /// 2018.08.21 by jhlim [ADD] SW Limit 사용유무를 설정한다.
        /// 2019.04.27 by yjlee [MOD] SW 리밋 사용 유무를 설정한다.
        /// </summary>
        public bool SetUseSWLimitPositive(bool bEnable)
        {
           if(m_motionItemManager.SetUseSWLimitPositive(bEnable))
           {
               int nIndex = m_motionItemManager.GetIndexOfSelectedItem();

               // 2019.08.19 by Thienvv [MOD] MK-UNIT No need to servo off because of SW limit parameter is not stored in motion board/driver
               SetMotorConfiguration(m_dicOfMotionItem[nIndex], false); 

               return true;
           }

           return false;
        }
        /// <summary>
        /// 2018.08.21 by jhlim [ADD] SW Limit 사용유무를 설정한다.
        /// 2019.04.27 by yjlee [MOD] SW 리밋 사용 유무를 설정한다.
        /// </summary>
        public bool SetUseSWLimitNegative(bool bEnable)
        {
            if(m_motionItemManager.SetUseSWLimitNegative(bEnable))
            {
                int nIndex = m_motionItemManager.GetIndexOfSelectedItem();

                // 2019.08.19 by Thienvv [MOD] MK UNIT No need to servo off because of SW limit parameter is not stored in motion board/driver
                SetMotorConfiguration(m_dicOfMotionItem[nIndex], false);

                return true;
            }

            return false;
        }
        /// <summary>
        /// 2018.08.21 by jhlim [ADD] SW Limit 스탑모드를 설정한다.
        /// 2019.04.27 by yjlee [MOD] SW 리밋 정지모드를 설정한다.
        /// </summary>
        public bool SetSWLimitStopModePositive(string strStopMode)
        {
            if(m_motionItemManager.SetSWLimitStopModePositive(strStopMode))
            {
                int nIndex = m_motionItemManager.GetIndexOfSelectedItem();

                // 2019.08.19 by Thienvv [MOD] MK UNIT No need to servo off because of SW limit parameter is not stored in motion board/driver
                SetMotorConfiguration(m_dicOfMotionItem[nIndex], false);

                return true;
            }

            return false;
        }
        /// <summary>
        /// 2018.08.21 by jhlim [ADD] SW Limit 스탑모드를 설정한다.
        /// 2019.04.27 by yjlee [MOD] SW 리밋 정지모드를 설정한다.
        /// </summary>
        public bool SetSWLimitStopModeNegative(string strStopMode)
        {
            if(m_motionItemManager.SetSWLimitStopModeNegative(strStopMode))
            {
                int nIndex = m_motionItemManager.GetIndexOfSelectedItem();

                 // 2019.08.19 by Thienvv [MOD] MK UNIT No need to servo off because of SW limit parameter is not stored in motion board/driver
                SetMotorConfiguration(m_dicOfMotionItem[nIndex], false);

                return true;
            }

            return false;
        }
        /// <summary>
        /// 2018.08.21 by jhlim [ADD] SW +Limit 값을 설정한다.
        /// 2019.04.27 by yjlee [MOD] SW 리밋을 설정한다.
        /// </summary>
        public bool SetSWLimitPositionPositive(double dblPosition)
        {
           if(m_motionItemManager.SetSWLimitPositionPositive(dblPosition))
           {
               int nIndex = m_motionItemManager.GetIndexOfSelectedItem();

               // 2019.08.19 by Thienvv [MOD] MK UNIT No need to servo off because of SW limit parameter is not stored in motion board/driver
               SetMotorConfiguration(m_dicOfMotionItem[nIndex], false);

               return true;
           }

           return false;
        }
        /// <summary>
        /// 2018.08.21 by jhlim [ADD] SW -Limit 값을 설정한다.
        /// 2019.04.27 by yjlee [MOD] SW 리밋을 설정한다.
        /// </summary>
        public bool SetSWLimitPositionNegative(double dblPosition)
        {
           if(m_motionItemManager.SetSWLimitPositionNegative(dblPosition))
           {
               int nIndex = m_motionItemManager.GetIndexOfSelectedItem();

               // 2019.08.19 by Thienvv [MOD] MK- UNIT No need to servo off because of SW limit parameter is not stored in motion board/driver
               SetMotorConfiguration(m_dicOfMotionItem[nIndex], false);

               return true;
           }

           return false;
        }
        
        // 2019.07.17 by Thienvv [ADD] MK-Unit
        public bool SetOutputMode(string strOutput)
        {
            if (m_motionItemManager.SetOutputMode(strOutput))
            {
                int nIndex = m_motionItemManager.GetIndexOfSelectedItem();

                MotionItem mItem = m_dicOfMotionItem[nIndex];

                m_motionController.SetMotorOutputMode(mItem.nTarget
                    , m_dicOfMotionItem[nIndex].configParam.enOutMode);

                SetMotorConfiguration(m_dicOfMotionItem[nIndex]);
                return true;
            }

            return false;
        }
        public bool SetInputMode(string strInput)
        {
            if (m_motionItemManager.SetInputMode(strInput))
            {
                int nIndex = m_motionItemManager.GetIndexOfSelectedItem();

                MotionItem mItem = m_dicOfMotionItem[nIndex];

                m_motionController.SetMotorInputMode(mItem.nTarget
                    , m_dicOfMotionItem[nIndex].configParam.enInMode
                    , m_dicOfMotionItem[nIndex].configParam.bInputDirection);
                SetMotorConfiguration(m_dicOfMotionItem[nIndex]);
                return true;
            }

            return false;
        }
        public bool SetInputDirection(bool bDirection)
        {
            if (m_motionItemManager.SetInputDirection(bDirection))
            {
                int nIndex = m_motionItemManager.GetIndexOfSelectedItem();

                MotionItem mItem = m_dicOfMotionItem[nIndex];

                m_motionController.SetMotorInputMode(mItem.nTarget
                    , m_dicOfMotionItem[nIndex].configParam.enInMode
                    , m_dicOfMotionItem[nIndex].configParam.bInputDirection);
                SetMotorConfiguration(m_dicOfMotionItem[nIndex]);
                return true;
            }

            return false;
        }
        #endregion

        #region Speed
        /// <summary>
        /// 2018.07.16 by yjlee [ADD] 모션 프로파일을 설정한다.
        /// </summary>
        public bool SetSpeedPattern(int nSpdContent, string strPattern)
        {
            return m_motionItemManager.SetSpeedPattern(nSpdContent, strPattern);
        }
        /// <summary>
        /// 2018.07.16 by yjlee [ADD] 모션의 속도를 설정한다.
        /// </summary>
        public bool SetVelocity(int nSpdContent, double dblVelocity)
        {
            return m_motionItemManager.SetVelocity(nSpdContent, dblVelocity);
        }
        /// <summary>
        /// 2018.07.16 by yjlee [ADD] 모션의 최대 속도를 설정한다.
        /// </summary>
        public bool SetMaxVelocity(int nSpdContent, double dblMaxVelocity)
        {
            return m_motionItemManager.SetMaxVelocity(nSpdContent, dblMaxVelocity);
        }
        /// <summary>
        /// 2018.07.16 by yjlee [ADD] 모션의 가속 시간을 설정한다.
        /// </summary>
        public bool SetAccelerationTime(int nSpdContent, double dblAccelTime)
        {
            return m_motionItemManager.SetAccelerationTime(nSpdContent, dblAccelTime);
        }
        /// <summary>
        /// 2018.07.16 by yjlee [ADD] 모션의 감속 시간을 설정한다.
        /// </summary>
        public bool SetDecelerationTime(int nSpdContent, double dblDecelTime)
        {
            return m_motionItemManager.SetDecelerationTime(nSpdContent, dblDecelTime);
        }
        /// <summary>
        /// 2018.08.22 by jhlim [ADD] 모션의 Jerk 가속도를 설정한다.
        /// </summary>
        public bool SetJerkAcceleration(int nSpdContent, double dblJerkAccel)
        {
            return m_motionItemManager.SetJerkAcceleration(nSpdContent, dblJerkAccel);
        }
        /// <summary>
        /// 2018.08.22 by jhlim [ADD] 모션의 Jerk 감속도를 설정한다.
        /// </summary>
        public bool SetJerkDeceleration(int nSpdContent, double dblJerkDecel)
        {
            return m_motionItemManager.SetJerkDeceleration(nSpdContent, dblJerkDecel);
        }
        /// <summary>
        /// 2018.07.16 by yjlee [ADD] 모션의 타임아웃을 설정한다.
        /// </summary>
        public bool SetMotionTimeout(int nSpdContent, uint uTimeout)
        {
            return m_motionItemManager.SetMotionTimeout(nSpdContent, uTimeout);
        }
        //Mechatrolink
        public bool SetMotionFilter(int nSpdContent, uint uFilter)
        {
            return m_motionItemManager.SetMotionFilter(nSpdContent, uFilter);
        }
        #endregion

        #region Home
        /// <summary>
        /// 2018.07.17 by yjlee [ADD] 홈의 완료 상태를 설정한다.
        /// </summary>
        public bool SetAlwaysHomeEnd(bool bEnable)
        {
            return m_motionItemManager.SetAlwaysHomeEnd(bEnable);
        }
        /// <summary>
        /// 2018.08.27 by yjlee [ADD] 강제로 홈을 완료 시킨다.
        /// </summary>
        public bool SetHomeEnd(int nIndex, bool bEnable)
        {
            int nTarget = 0;

            if (false == GetVaildTarget(nIndex, ref nTarget)) { return false; }

            m_listOfArState[nTarget][(int)EN_MOTOR_STATE.HOMEEND]       = true;
            m_listOfArState[nTarget][(int)EN_MOTOR_STATE.MOTIONDONE]    = true;

            return true;
        }
        /// <summary>
        /// 2018.07.17 by yjlee [ADD] 홈의 모드를 설정한다.
        /// </summary>
        public bool SetHomeMode(string strMode)
        {
            if(m_motionItemManager.SetHomeMode(strMode))
            {
                int nIndex = m_motionItemManager.GetIndexOfSelectedItem();

                MotionItem mItem = m_dicOfMotionItem[nIndex];

                m_motionController.SetMotorHomeConfiguration(mItem.nTarget
                    , m_dicOfMotionItem[nIndex].homeParam);

                return true;
            }

            return false;
        }
        /// <summary>
        /// 2018.07.17 by yjlee [ADD] 홈의 펄스 로직을 설정한다.
        /// </summary>
        public bool SetHomeLogic(string strLogic)
        {
            if (m_motionItemManager.SetHomeLogic(strLogic))
            {
                int nIndex = m_motionItemManager.GetIndexOfSelectedItem();

                MotionItem mItem = m_dicOfMotionItem[nIndex];

                m_motionController.SetMotorHomeConfiguration(mItem.nTarget
                    , m_dicOfMotionItem[nIndex].homeParam);

                return true;
            }

            return false;
        }
        /// <summary>
        /// 2018.07.17 by yjlee [ADD] 홈의 방향을 설정한다.
        /// </summary>
        public bool SetHomeDirection(string strDirection)
        {
            if(m_motionItemManager.SetHomeDirection(strDirection))
            {
                int nIndex = m_motionItemManager.GetIndexOfSelectedItem();

                MotionItem mItem = m_dicOfMotionItem[nIndex];

                m_motionController.SetMotorHomeConfiguration(mItem.nTarget
                    , m_dicOfMotionItem[nIndex].homeParam);

                return true;
            }

            return false;
        }
        /// <summary>
        /// 2018.07.17 by yjlee [ADD] 홈의 탈출 거리를 설정한다.
        /// 센서 감지 후 해당 거리만큼 탈출한다.
        /// </summary>
        public bool SetEscapeDistance(double dblDistance)
        {
            if(m_motionItemManager.SetEscapeDistance(dblDistance))
            {
                int nIndex = m_motionItemManager.GetIndexOfSelectedItem();

                MotionItem mItem = m_dicOfMotionItem[nIndex];

                m_motionController.SetMotorHomeConfiguration(mItem.nTarget
                    , m_dicOfMotionItem[nIndex].homeParam);

                return true;
            }

            return false;
        }
        /// <summary>
        /// 2018.07.17 by yjlee [ADD] 홈 오프셋을 설정한다.
        /// 홈 완료 후 해당 거리만큼 이동한다.
        /// </summary>
        public bool SetHomeOffset(double dblOffset)
        {
            if(m_motionItemManager.SetHomeOffset(dblOffset))
            {
                int nIndex = m_motionItemManager.GetIndexOfSelectedItem();

                MotionItem mItem = m_dicOfMotionItem[nIndex];

                m_motionController.SetMotorHomeConfiguration(mItem.nTarget
                    , m_dicOfMotionItem[nIndex].homeParam);

                return true;
            }

            return false;
        }
        /// <summary>
        /// 2018.07.17 by yjlee [ADD] 지상 카운터를 설정한다.
        /// 지상으로 홈 동작을 수행할 경우에 필요하다.
        /// </summary>
        public bool SetEZCount(int nCount)
        {
            if(m_motionItemManager.SetEZCount(nCount))
            {
                int nIndex = m_motionItemManager.GetIndexOfSelectedItem();

                MotionItem mItem = m_dicOfMotionItem[nIndex];

                m_motionController.SetMotorHomeConfiguration(mItem.nTarget
                    , m_dicOfMotionItem[nIndex].homeParam);

                return true;
            }

            return false;
        }
        /// <summary>
        /// 2018.07.17 by yjlee [ADD] 홈 완료 후 설정될 포지션을 설정한다.
        /// </summary>
        public bool SetHomeBasePosition(double dblPosition)
        {
            if(m_motionItemManager.SetHomeBasePosition(dblPosition))
            {
                int nIndex = m_motionItemManager.GetIndexOfSelectedItem();

                MotionItem mItem = m_dicOfMotionItem[nIndex];

                m_motionController.SetMotorHomeConfiguration(mItem.nTarget
                    , m_dicOfMotionItem[nIndex].homeParam);

                return true;
            }

            return false;
        }
        /// <summary>
        /// 2018.07.17 by yjlee [ADD] 홈의 모션 프로파일을 선택한다.
        /// </summary>
        public bool SetHomeSpeedPattern(string strPattern)
        {
            return m_motionItemManager.SetHomeSpeedPattern(strPattern);
        }
        /// <summary>
        /// 2018.07.17 by yjlee [ADD] 홈의 첫 번째 속도를 설정한다.
        /// </summary>
        public bool SetHomeVelocity(double dblVelocity)
        {
            return m_motionItemManager.SetHomeVelocity(dblVelocity);
        }
        /// <summary>
        /// 2018.07.17 by yjlee [ADD] 홈의 두 번째 속도를 설정한다.
        /// </summary>
        public bool SetHomeVelocitySecond(double dblVelocity)
        {
            return m_motionItemManager.SetHomeVelocitySecond(dblVelocity);
        }
        /// <summary>
        /// 2018.07.17 by yjlee [ADD] 홈의 가속 시간을 설정한다.
        /// </summary>
        public bool SetHomeAccelerationTime(double dblAccelTime)
        {
            return m_motionItemManager.SetHomeAccelerationTime(dblAccelTime);
        }
        /// <summary>
        /// 2018.07.17 by yjlee [ADD] 홈의 감속 시간을 설정한다.
        /// </summary>
        public bool SetHomeDecelerationTime(double dblDecelTime)
        {
            return m_motionItemManager.SetHomeDecelerationTime(dblDecelTime);
        }
        /// <summary>
        /// 2018.08.22 by jhlim [ADD] 홈의 Jerk 가속도를 설정한다.
        /// </summary>
        public bool SetHomeJerkAcceleration(double dblJerkAccel)
        {
            return m_motionItemManager.SetHomeJerkAcceleration(dblJerkAccel);
        }
        /// <summary>
        /// 2018.08.22 by jhlim [ADD] 홈의 Jerk 감속도를 설정한다.
        /// </summary>
        public bool SetHomeJerkDeceleration(double dblJerkDecel)
        {
            return m_motionItemManager.SetHomeJerkDeceleration(dblJerkDecel);
        }
        /// <summary>
        /// 2018.07.17 by yjlee [ADD] 홈의 타임 아웃을 설정한다.
        /// </summary>
        public bool SetHomeTimeout(uint uTimeout)
        {
            return m_motionItemManager.SetHomeTimeout(uTimeout);
        }
        #endregion

        #region Gantry
        /// <summary>
        /// 2018.07.13 by yjlee [ADD] 해당 아이템을 GANTRY 마스터로 사용할 것인지 설정한다.
        /// </summary>
        /// <param name="bEnable"></param>
        /// <returns></returns>
        public bool EnableMasterAsGantry(int nIndex, bool bEnable)
        {
            if (m_motionItemManager.EnableMaster(bEnable))
            {
                return EnableGantry(nIndex, bEnable);
            }

            return false;
        }
        /// <summary>
        /// 2018.07.13 by yjlee [ADD] 선택한 아이템을 GANTRY 슬레이브로 설정한다.
        /// </summary>
        /// <param name="nIndex"></param>
        /// <returns></returns>
        public bool SetIndexOfSlave(int nIndex, int nIndexOfSlave)
        {
            if (m_motionItemManager.SetIndexOfSlave(nIndexOfSlave))
            {
                return EnableGantry(nIndex, true);
            }

            return false;
        }
        /// <summary>
        /// 2018.07.19 by yjlee [ADD] 슬레이브 축 방향의 반전 여부를 설정한다.
        /// </summary>
        public bool SetSlaveInverse(bool bEnable)
        {
            return m_motionItemManager.SetSlaveInverse(bEnable);
        }
        #endregion

        #endregion

        #endregion

        #region 에러 관련
        /// <summary>
        /// 2018.07.18 by yjlee [ADD] 마지막으로 발생한 에러를 반환한다.
        /// </summary>
        public string GetLastError()
        {
            return m_strErrorMessage;
        }
        /// <summary>
        /// 2018.07.18 by yjlee [ADD] 마지막으로 발생한 에러를 클리어한다.
        /// </summary>
        public void ClearErrorMessage()
        {
            //m_strErrorMessage   = NONE;
        }
        #endregion

        #region Compensation 관련 함수
        public void AddCompensationItem(CompensationItem item)
        {
            int nIndex = 0;
            m_CompensatorInstance.AddItem(item, ref nIndex);
            m_CompensatorInstance.Save();
            m_motionController.SetCompensation(nIndex, item);
        }
        #endregion

        #endregion

        #region 내부 인터페이스

        #region 모션 인터페이스
        /// <summary>
        /// 2019.04.20 by yjlee [ADD] 컨트롤러로부터 모션 상태를 읽어온다.
        /// </summary>
        private void GetStateFromController(MotionItem mItem, bool bHoming = false)
        {
            int nTarget     = mItem.nTarget;

            m_motionController.GetState(nTarget, m_listOfArState[nTarget]);

            bool[] arState  = m_listOfArState[nTarget];

            // 리밋 홈 동작 중일 경우
			// 2020.03.05 by Thienvv [MOD] Merge from MK-D100 Minami
            if (m_listOfMotionStatus[nTarget] == EN_MOTION_STATUS.ELHOMING || m_listOfMotionStatus[nTarget] == EN_MOTION_STATUS.HOMING)
            //if(m_listOfMotionStatus[nTarget] == EN_MOTION_STATUS.ELHOMING)
            {
                arState[(int)EN_MOTOR_STATE.LIMITPOSITIVE]      = false;
                arState[(int)EN_MOTOR_STATE.LIMITNEGATIVE]      = false;
				
				// 2020.03.05 by Thienvv [MOD] Merge from MK-D100 Minami
                arState[(int)EN_MOTOR_STATE.SWLIMITNEGATIVE] = false;
                arState[(int)EN_MOTOR_STATE.SWLIMITPOSITIVE] = false;
            }
            else
            {
                // 정방향 리밋 센서를 사용하지 않을 경우
                if(false == mItem.configParam.bUseHWLimitPositive)
                {
                    arState[(int)EN_MOTOR_STATE.LIMITPOSITIVE]  = false;
                }
                // 역방향
                if(false == mItem.configParam.bUseHWLimitNegative)
                {
                   arState[(int)EN_MOTOR_STATE.LIMITNEGATIVE]   = false;
                }
            }

            #region Check SW Limit

            // 정방향 소프트웨어 리밋을 사용할 경우
            if (false == bHoming
                && mItem.configParam.bUseSWLimitPositive)
            {
                // 현재 포지션을 읽어온다.
                double dblActualPosition    = m_motionController.GetActualPosition(nTarget);

                // 리밋 포지션 초과일 경우
                if(dblActualPosition > mItem.configParam.dblSWLimitPositionPositive)
                {
                    arState[(int)EN_MOTOR_STATE.SWLIMITPOSITIVE]    = true;
                }
                else
                {
                    arState[(int)EN_MOTOR_STATE.SWLIMITPOSITIVE]    = false;
                }
            }
            else
            {
                arState[(int)EN_MOTOR_STATE.SWLIMITPOSITIVE]    = false;
            }

            // 역방향 소프트웨어 리밋을 사용할 경우
            if (false == bHoming
                && mItem.configParam.bUseSWLimitNegative)
            {
                // 현재 포지션을 읽어온다.
                double dblActualPosition    = m_motionController.GetActualPosition(nTarget);

                // 리밋 포지션 미만일 경우
                if(dblActualPosition < mItem.configParam.dblSWLimitPositionNegative)
                {
                    arState[(int)EN_MOTOR_STATE.SWLIMITNEGATIVE]    = true;
                }
                else
                {
                    arState[(int)EN_MOTOR_STATE.SWLIMITNEGATIVE]    = false;
                }
            }
            else
            {
                arState[(int)EN_MOTOR_STATE.SWLIMITNEGATIVE]    = false;
            }

            #endregion
        }
        /// <summary>
        /// 2018.08.30 by yjlee [ADD] 리핏 동작을 위한 모션을 이송 시킨다.
        /// 이송 전 모션의 이송 여부를 계산하지 않는다.
        /// </summary>
        private bool MoveSxAbsolutelyForRepeating(int nIndex, double dblDestination)
        {
            if (false == m_dicOfMotionItem.ContainsKey(nIndex)) { return false; }

            MotionItem mItem = m_dicOfMotionItem[nIndex];

            int nTarget = 0;

            if (GetVaildTarget(mItem, ref nTarget) == false) { return false; }

            if (m_listOfMotionStatus[nTarget] != EN_MOTION_STATUS.READY) { return false; }

            //2020.03.09 nogami add start
            if (IsSoftwareLimitNegativeCheck(nIndex, EN_MOTION_TYPE.ABSOLUTE, dblDestination))
            {
                if (false == GetVaildTarget(mItem, ref nTarget)) { return false; }
                bool[] arState = m_listOfArState[nTarget];
                arState[(int)EN_MOTOR_STATE.PRE_SWLIMITNEGATIVE] = true;
                return (false);
            }
            else if (IsSoftwareLimitPositiveCheck(nIndex, EN_MOTION_TYPE.ABSOLUTE, dblDestination))
            {
                if (false == GetVaildTarget(mItem, ref nTarget)) { return false; }
                bool[] arState = m_listOfArState[nTarget];
                arState[(int)EN_MOTOR_STATE.PRE_SWLIMITPOSITIVE] = true;
                return (false);
            }
            //2020.03.09 nogami add end
            MOTOR_SPEED_PARAMETER spdParam = mItem.speedParam[(int)EN_MOTOR_CONTENTOFSPEED.RUN];

            // 현재 아이템의 파워를 가지고 온다.
            int nPower = mItem.nPower;

            spdParam.dblVelocity = spdParam.dblVelocity * nPower * 0.01;

            #region Log 관련 파라미터 정보
            mItem.strUsingTask                  = MOVE_REPEAT;
            mItem.enMotionType                  = EN_MOTION_TYPE.ABSOLUTE;
            mItem.dblDestination                = dblDestination;
            mItem.spdRecentParam.dblVelocity    = spdParam.dblVelocity;
            mItem.spdRecentParam.dblAccelTime   = spdParam.dblAccelTime;
            mItem.spdRecentParam.dblDecelTime   = spdParam.dblDecelTime;
            mItem.dblActualPosition = m_motionController.GetActualPosition(nTarget);
            #endregion

            // 속도를 설정한다.
            m_motionController.SetMotorSpeedConfiguration(nTarget, spdParam);
            m_motionController.MoveSxAbsolutely(mItem.nTarget, dblDestination);

            AddMovingMotion(mItem, spdParam.uMotionTimeout);

            return true;
        }
        /// <summary>
        /// 2018.08.30 by yjlee [ADD] 리핏 동작 중이 아닌데 모션이 가동 중인지 판단한다.
        /// </summary>
        private bool IsMotionMovingAtTarget(int nTarget)
        {
            if(m_listOfRepeatingItem[nTarget].bStart
                || m_listOfMotionStatus[nTarget] != EN_MOTION_STATUS.READY)
            {
                return true;
            }

            return false;
        }
        private bool IsMotionJogMovingAtTarget(int nTarget)
        {
            if (m_listOfRepeatingItem[nTarget].bStart)
            {
                return true;
            }
            switch (m_listOfMotionStatus[nTarget])
            {
                case EN_MOTION_STATUS.MOVING:
                case EN_MOTION_STATUS.ELHOMING:
                case EN_MOTION_STATUS.HOMING:
                case EN_MOTION_STATUS.DELAY:
                    return true;
            }
            return false;
        }
        /// <summary>
        /// 2018.07.12 by yjlee [ADD] 유효한 타겟 번호를 반환한다.
        /// 타겟 번호 범위를 벗어났거나, 슬레이브 축이거나, 비활성화 아이템일 경우 false를 반환한다.
        /// </summary>
        private bool GetVaildTarget(MotionItem mItem, ref int nTarget)
        {
            // 장치가 초기화되지 않았을 경우
            if (m_bInit == false) 
            {
              //  m_strErrorMessage   = SystemMessages.GetMessage(EN_MESSAGELIST.MOTION_NOT_INIT_CONTROLLER);
            
                return false; 
            }

            // 비활성화 아이템일 경우
            if(mItem.bEnable == false)
            {
             //   m_strErrorMessage   = SystemMessages.GetMessage(EN_MESSAGELIST.ITEM_DISABLED);

                return false;
            }

            nTarget = mItem.nTarget;

            // 유효한 타겟이 아닐 경우
            if (nTarget < 0 
                || nTarget >= m_nCountOfAxis)
            {
             //   m_strErrorMessage   = SystemMessages.GetMessage(EN_MESSAGELIST.MOTION_INVALID_TARGET);
            
                return false;
            }

            return true;
        }
        /// <summary>
        /// 2018.07.12 by yjlee [ADD] 유효한 타겟 번호를 반환한다.
        /// 타겟 번호 범위를 벗어났거나, 슬레이브 축이거나, 비활성화 아이템일 경우 false를 반환한다.
        /// </summary>
        private bool GetVaildTarget(int nIndex, ref int nTarget)
        {
            if (false == m_bInit
                || false == m_dicOfMotionItem.ContainsKey(nIndex)) { return false; }

            return GetVaildTarget(m_dicOfMotionItem[nIndex], ref nTarget);
        }
        /// <summary>
        /// 2018.07.12 by yjlee [ADD] 모션 움직이기 위한 준비 작업을 수행한다.
        /// </summary>
        private bool PrepareForMoving(MotionItem mItem, EN_MOTOR_CONTENTOFSPEED spdContent, double dblDestination, double dblVelocity, uint uRatio, ref uint uMotionTimeout, ref bool bInputShaping)
        {
            int nTarget = 0;
            
            if (GetVaildTarget(mItem, ref nTarget) == false) { return false; }

            // 모션이 구동 중일 경우
            if (IsMotionMovingAtTarget(nTarget)) { return false;}

            // Gantry 상태를 확인한다.
            if (false == CheckStateOfGantry(mItem)) { return false; }

            MOTOR_SPEED_PARAMETER spdParam = mItem.speedParam[(int)spdContent];

            // 사용자가 메뉴얼 속도를 정의했을 경우
            if (dblVelocity > 0)
            {
                spdParam.dblVelocity = dblVelocity;
            }

            // 현재 아이템의 파워를 가지고 온다.
            int nPower      = mItem.nPower;

            dblVelocity             = spdParam.dblVelocity * nPower * uRatio * 0.0001;
            spdParam.dblVelocity    = dblVelocity;
            bInputShaping           = spdParam.enSpeedPattern == EN_SPEED_PATTERN.ANTI_VIBRATION;

            #region Log 관련 파라미터 정보
            mItem.dblDestination                = dblDestination;
            mItem.dblActualPosition             = m_motionController.GetActualPosition(nTarget);
            mItem.spdRecentParam.dblVelocity    = spdParam.dblVelocity;
            mItem.spdRecentParam.dblAccelTime   = spdParam.dblAccelTime;
            mItem.spdRecentParam.dblDecelTime   = spdParam.dblDecelTime;
            #endregion

            // 입력성형 기법일 경우
            if(bInputShaping)
            {
                double[] arVelocity     = null;
                double[] arAccelTime    = null;
                double[] arDecelTime    = null;
                int nLength             = 0;

                double dCurrent     = m_motionController.GetCommandPosition(nTarget);
                double dDistance    = dblDestination - dCurrent;

                ConvertByInputShaping(true
                    , dDistance
                    , dblVelocity
                    , spdParam.dblAccelTime
                    , spdParam.dblDecelTime
                    , 60
                    , out arVelocity
                    , out arAccelTime
                    , out arDecelTime
                    , ref nLength);

                // 2019.05.02 by yjlee [MOD] 인터페이스 변경으로 인한 주석
                // m_motionController.MoveByList(nTarget, dCurrent, dblDestination, nLength, arVelocity, arAccelTime, arDecelTime);
            }
            else
            {
                // 속도를 설정한다.
                m_motionController.SetMotorSpeedConfiguration(nTarget, spdParam);
            }

            uMotionTimeout  = spdParam.uMotionTimeout;

            return true;
        }
        #endregion

        #region 모터설정용 함수
        /// <summary>
        /// 2019.04.27 by yjlee [MOD] 모터 설정을 진행한다.
        /// </summary>
        private void SetMotorConfig()
        {
            // 설비가 초기화되지 않았을 경우
            if (false == m_bInit) { return; }

            foreach(var kvp in m_dicOfMotionItem)
            {
                MotionItem mItem    = kvp.Value;
                int nTarget         = mItem.nTarget;

                // 타겟이 유효하지 않을 경우`
                if (nTarget < 0
                    || nTarget >= m_nCountOfAxis)
                {
                    continue;
                }

                // 모터 타입 설정
                m_motionController.SetMotorType(nTarget, mItem.enMotorType);
                System.Threading.Thread.Sleep(c_nDelayForSettingParameters);

                // 모터 일반 설정
                SetMotorConfiguration(mItem);
                System.Threading.Thread.Sleep(c_nDelayForSettingParameters);

                // 모터 홈 설정
                m_motionController.SetMotorHomeConfiguration(nTarget, mItem.homeParam);
                System.Threading.Thread.Sleep(c_nDelayForSettingParameters);
            }
        }
        /// <summary>
        /// 2019.04.27 by yjlee [ADD] 모터 설정을 진행한다.
        /// 2019.08.19 by Thienvv [MOD]  Add option for set Servo Off depends on Motion Parameters
		/// 2020.03.05 by Thienvv [MOD] Merge from MK-D100 Minami
        /// </summary>
        private bool SetMotorConfiguration(MotionItem mItem, bool bDoServoOff = true)
        {
            int nTarget = 0;

            if (false == GetVaildTarget(mItem, ref nTarget)) { return false; }

            // 2020.04.25 by Thienvv [MOD] Reset Home End when setvo Off
            //m_listOfArState[nTarget][(int)EN_MOTOR_STATE.HOMEEND]       = false;

            m_listOfArState[nTarget][(int)EN_MOTOR_STATE.MOTIONDONE]    = false;

            // 모터 설정 시, 스케일과 같은 모터에 영향을 미치는 파라미터들 때문에 전원을 해제한다.
            if(bDoServoOff)
            {
                m_listOfArState[nTarget][(int)EN_MOTOR_STATE.HOMEEND] = false;
                m_motionController.DoMotorOff(nTarget);
            }
            
            // 전원이 꺼지기 전까지 잠시 대기한다.
            System.Threading.Thread.Sleep(c_nDelayForSettingParameters);

            // 2020.03.05 by Thienvv [MOD] Merge from MK-D100 Minami
            m_motionController.SetMotorOutputMode(nTarget, mItem.configParam.enOutMode);
            m_motionController.SetMotorInputMode(nTarget, mItem.configParam.enInMode, true);

            // 모터 설정을 진행한다.
            m_motionController.SetMotorConfiguration(nTarget, mItem.configParam);

            return true;
        }
        #endregion

        #region Gantry 설정
        /// <summary>
        /// 2018.07.13 by yjlee [ADD] GANTRY 유무를 설정한다.
        /// </summary>
        /// <param name="nIndex">마스터 아이템의 인덱스</param>
        /// <param name="bEnable">사용 유무</param>
        private bool EnableGantry(int nIndex, bool bEnable)
        {
            if (false == m_dicOfMotionItem.ContainsKey(nIndex)) { return false; }

            MotionItem mItem    = m_dicOfMotionItem[nIndex];
            int nTarget         = 0;

            if (false == GetVaildTarget(mItem, ref nTarget)) { return false; }

            MOTOR_GANTRY_PARAMETER gantryParam  = m_dicOfMotionItem[nIndex].gantryParam;

            int nIndexOfSlave   = mItem.gantryParam.nIndexOfSlave;

            // 유효한 아이템이 없을 경우
            if (false == m_dicOfMotionItem.ContainsKey(nIndexOfSlave)) { return false; }
            
            int nTargetOfSlave  = 0;

            // 슬레이브 축의 타겟 번호가 유효한지 검사한다.
            if (false == GetVaildTarget(m_dicOfMotionItem[nIndexOfSlave], ref nTargetOfSlave)) { return false; }

            // Gantry 상태인지 확인한다.
            bool bRegistered    = m_motionController.GetGantryState(nTarget, nTargetOfSlave);

            if (true == bEnable
                && false == bRegistered)
            {
                return m_motionController.RegisterGantry(nTarget
                    , m_dicOfMotionItem[nIndex].speedParam[(int)EN_MOTOR_CONTENTOFSPEED.RUN]
                    , nTargetOfSlave
                    , gantryParam.bSlaveInverse);
            }
            else if(false == bEnable
                && true == bRegistered)
            {
                mItem.gantryParam.nIndexOfSlave = -1;
                mItem.gantryParam.bSlaveInverse = false;

                m_motionController.UnRegisterGantry(nTarget, nTargetOfSlave);
            }

            return true;
        }
        /// <summary>
        /// 2018.12.27 by yjlee [ADD] Gantry 상태를 확인한다.
        /// </summary>
        private bool CheckStateOfGantry(MotionItem mItem)
        {
            // Gantry 마스터 축일 경우
            if (mItem.gantryParam.bMaster)
            {
                int nTarget         = mItem.nTarget;
                int nIndexOfSlave   = mItem.gantryParam.nIndexOfSlave;

                // 등록된 슬레이브의 모션 아이템이 존재하지 않을 경우
                if (false == m_dicOfMotionItem.ContainsKey(nIndexOfSlave)) { return false; }

                int nTargetOfSlave =    0;

                // 슬레이브 축의 타겟 번호가 유효한지 검사한다.
                if (false == GetVaildTarget(m_dicOfMotionItem[nIndexOfSlave], ref nTargetOfSlave)) { return false; }

                // Gantry 활성이 안 된 상태일 경우
                if (false == m_motionController.GetGantryState(nTarget, nTargetOfSlave))
                {
                    // Gantry를 활성화 시킨다.
                    if (false == m_motionController.RegisterGantry(nTarget
                        , mItem.speedParam[(int)EN_MOTOR_CONTENTOFSPEED.RUN]
                        , nTargetOfSlave
                        , mItem.gantryParam.bSlaveInverse))
                    {
                        // 활성화가 안될 경우
                        return false;
                    }
                }

                if(false == m_motionController.isMotorOn(nTargetOfSlave))
                {
                    m_motionController.DoReset(nTargetOfSlave);

                    return m_motionController.DoMotorOn(nTargetOfSlave);
                }
            }
            // Gantry 슬레이브 축일 경우
            else if (mItem.gantryParam.bSlave)
            {
            //    m_strErrorMessage = SystemMessages.GetMessage(EN_MESSAGELIST.MOTION_AXIS_OF_SLAVE);

                return false;
            }

            return true;
        }
        #endregion

        #region 감시 관련
        /// <summary>
        /// 2018.07.12 by yjlee [ADD] 움직인 축의 정보를 등록한다.
        /// </summary>
        private void AddMovingMotion(MotionItem mItem, uint uTimeout = 60000, uint uDelay = 0)
        {
            int nTarget     = mItem.nTarget;

            m_listOfArState[nTarget][(int)EN_MOTOR_STATE.MOTIONDONE]    = false;
            m_listOfArState[nTarget][(int)EN_MOTOR_STATE.MOTIONTIMEOUT] = false;
            //2020.03.09 nogami add start
            m_listOfArState[nTarget][(int)EN_MOTOR_STATE.PRE_SWLIMITNEGATIVE] = false;
            m_listOfArState[nTarget][(int)EN_MOTOR_STATE.PRE_SWLIMITPOSITIVE] = false;
            //2020.03.09 nogami add end

            m_listOfMovingMotion[nTarget].StartMoving(uTimeout, uDelay);
            m_listOfMotionStatus[nTarget]   = EN_MOTION_STATUS.MOVING;

            ///---
            ///
            //m_InstanceLog.WriteMotionLog(mItem, true);
        }
        /// <summary>
        /// 2018.07.12 by yjlee [ADD] 홈 중인 축의 정보를 등록한다.
        /// </summary>
        private void AddHomingMotion(MotionItem mItem, EN_MOTOR_HOMEMODE enHomeMode , uint uTimeout = 60000)
        {
            int nTarget     = mItem.nTarget;

            m_listOfArState[nTarget][(int)EN_MOTOR_STATE.MOTIONDONE]     = false;
            m_listOfArState[nTarget][(int)EN_MOTOR_STATE.HOMEEND]        = false;
            m_listOfArState[nTarget][(int)EN_MOTOR_STATE.HOMETIMEOUT]    = false;

            // 홈 동작 중 리밋 스킵 관련
            bool bSkip      = m_dicOfMotionItem[mItem.nIndex].homeParam.strHomeMode.Contains(MOTION_TOKEN_LIMIT);

            m_listOfMovingMotion[nTarget].StartMoving(uTimeout);

            if(bSkip)
            {
                m_listOfMotionStatus[nTarget]   = EN_MOTION_STATUS.ELHOMING;
            }
            else
            {
                m_listOfMotionStatus[nTarget]   = EN_MOTION_STATUS.HOMING;
            }
            // 2020.04.25 by Thienvv [MOD] Selectable Sensor type: Home 2-steps for MK-D12
            MOTOR_HOME_PARAMETER homeParam = mItem.homeParam;
            homeParam.enHomeMode = enHomeMode;
            //2020.10.07 nogami modify m_motionController.SetMotorHomeSpeedConfiguration(mItem.nTarget, homeParam);
            m_motionController.SetMotorHomeSpeedConfiguration(mItem.nTarget, homeParam, mItem.speedParam[(int)EN_MOTOR_CONTENTOFSPEED.CUSTOM_2]);
            //m_motionController.SetMotorHomeSpeedConfiguration(mItem.nTarget, mItem.homeParam); // ORG CODE

            mItem.enMotionType      = EN_MOTION_TYPE.HOMING;
            /// m_InstanceLog.WriteMotionLog(mItem, true);
        }
        /// <summary>
        /// 2018.08.25 by yjlee [ADD] 이동 관련 작업을 수행한다.
        /// </summary>
        private void DoMove(MotionItem mItem)
        {
            int nTarget     = mItem.nTarget;

            // 모터 상태 확인
            //2020.09.15 nogami modify start
            //GetStateFromController(mItem);
            if (m_listOfArState[nTarget][(int)EN_MOTOR_STATE.HOMEEND])
            {
                GetStateFromController(mItem, false);
            }
            else
            {
                GetStateFromController(mItem, true);
            }
            //2020.09.15 nogami modify end

            // 모션에 에러가 있을 경우
            if (CheckMotionError(mItem)) { return; }

            MovedMotion mMotion = m_listOfMovingMotion[nTarget];

            // 모션 타임아웃 확인
            if (mMotion.IsTimeoutOver())
            {
                StopDoingMoving(nTarget);

                m_listOfArState[nTarget][(int)EN_MOTOR_STATE.MOTIONTIMEOUT]     = true;

                return;
            }

            // 모션이 완료 되었을 경우 딜레이 확인
            if (m_motionController.IsMotionDone(nTarget))
            {
                if(mMotion.uDelay > 0)
                {
                    mMotion.SetDelay();

                    m_listOfMotionStatus[nTarget]   = EN_MOTION_STATUS.DELAY;
                }
                else
                {
                    m_listOfMotionStatus[nTarget]   = EN_MOTION_STATUS.STOP;
                }
            }
        }
        /// <summary>
        /// 2018.08.25 by yjlee [ADD] 홈 관련 작업을 수행한다.
        /// </summary>
        private void DoHoming(MotionItem mItem)
        {
            int nTarget     = mItem.nTarget;

            // 모터 상태 확인
            GetStateFromController(mItem, true);

            // 모션에 에러가 있을 경우
            if (CheckMotionError(mItem)) { return; }

            MovedMotion mMotion     = m_listOfMovingMotion[nTarget];

            // 홈 타임아웃 확인
            if (mMotion.IsTimeoutOver())
            {
                StopDoingMoving(nTarget);

                m_listOfArState[nTarget][(int)EN_MOTOR_STATE.HOMETIMEOUT] = true;
            
                return;
            }
            
            if (mMotion.IsDelayOver() == false) return;
            
            uint uDelay = 0;
            
            // 홈 시퀀스를 수행한다.
            if (m_motionController.MoveToHome(nTarget, ref mMotion.nSeqNumber, ref uDelay))
            {
                m_listOfArState[nTarget][(int)EN_MOTOR_STATE.HOMEEND]      = true;

                m_listOfMotionStatus[nTarget]   = EN_MOTION_STATUS.STOP;

                // 2019.11.07. by shkim [ADD] 홈 완료 후 상태 갱신 필요.
                // 다음 스텝에서 상태 확인하는 부분이 있지만, 갱신이 없음.
                GetStateFromController(mItem, true);
            }
            else if (uDelay > 0)
            {
                mMotion.SetDelay(uDelay);
            }
        }
        /// <summary>
        /// 2019.04.26 by yjlee [ADD] 모션의 움직임을 정지시킨다.
        /// </summary>
        private void StopDoingMoving(int nTarget)
        {
            m_motionController.StopMotion(nTarget, true);

            m_listOfMotionStatus[nTarget] = EN_MOTION_STATUS.STOP;
        }
        /// <summary>
        /// 2018.08.25 by yjlee [ADD] 딜레이 관련 작업을 수행한다.
        /// </summary>
        private bool DoDelay(int nTarget)
        {
            if (m_listOfMovingMotion[nTarget].IsDelayOver())
            {
                return true;
            }

            return false;
        }
        /// <summary>
        /// 2018.08.25 by yjlee [ADD] 정지 관련 작업을 수행한다.
        /// </summary>
        private bool DoStop(int nTarget)
        {
            if(m_motionController.IsMotionDone(nTarget))
            {
                return true;
            }

            return false;
        }
        /// <summary>
        /// 2018.07.11 by yjlee [ADD] 모션 아이템의 리핏 운전을 수행한다.
        /// </summary>
        private void DoRepeat(int nIndex, int nTarget)
        {
            RepeatItem rItem    = m_listOfRepeatingItem[nTarget];

            // 지연 시간이 지나지 않았을 경우
            if (false == rItem.IsDelayOver()) return;

            // 첫 번째 위치일 경우
            if (rItem.IsFirst)
            {
                switch (rItem.nStep)
                {
                    case 0:
                        // Move Second
                        if(false == MoveSxAbsolutelyForRepeating(nIndex, rItem.dblPositionSecond))
                        {
                            rItem.StopRepeat();
                        }

                        ++ rItem.nStep;
                        break;
                    case 1:
                        if (m_listOfArState[nTarget][(int)EN_MOTOR_STATE.MOTIONDONE])
                        {
                            // Motion Done
                            rItem.SetSecondDelay();

                            //2020.09.21 nogami modify start
                            //rItem.nStep = 0;
                            //rItem.IsFirst = false;
                            ++rItem.nStep;
                            //2020.09.21 nogami modify end
                        }
                        break;
                        //2020.09.21 nogami add start
                    case 2:
                        if(m_listOfMotionStatus[nTarget] == EN_MOTION_STATUS.READY)
                        {
                            rItem.nStep = 0;
                            rItem.IsFirst = false;
                        }
                        break;
                    //2020.09.21 nogami add end
                }
            }
            else
            {
                switch (rItem.nStep)
                {
                    case 0:
                        // Move First
                        if (false == MoveSxAbsolutelyForRepeating(nIndex, rItem.dblPositionFirst))
                        {
                            rItem.StopRepeat();
                        }

                        ++ rItem.nStep;
                        break;
                    case 1:
                        if (m_listOfArState[nTarget][(int)EN_MOTOR_STATE.MOTIONDONE])
                        {
                            // Motion Done
                            rItem.SetFirstDelay();

                            //2020.09.21 nogami modify start
                            //rItem.nStep = 0;
                            //rItem.IsFirst = true;
                            //rItem.RepeateadCount++;     // 2019.09.26. by shkim. [ADD] REPEAT 카운트
                            ++rItem.nStep;
                            //2020.09.21 nogami modify end
                        }
                        break;
                    //2020.09.21 nogami add start
                    case 2:
                        if (m_listOfMotionStatus[nTarget] == EN_MOTION_STATUS.READY)
                        {
                            rItem.nStep = 0;
                            rItem.IsFirst = true;
                            rItem.RepeateadCount++;     // 2019.09.26. by shkim. [ADD] REPEAT 카운트
                        }
                        break;
                    //2020.09.21 nogami add end
                }
            }
        }
        /// <summary>
        /// 2018.09.18 by yjlee [ADD] 해당 타겟의 모션에 에러가 있는지 확인한다.
        /// </summary>
        private bool CheckMotionError(MotionItem mItem)
        {
            int nTarget = mItem.nTarget;
            var arState = m_listOfArState[nTarget];
            bool bMotionError = false;
            bool bEmergency = true;

            EN_MOTION_TYPE enMotionType = mItem.enMotionType;

            // 모션 에러를 체크한다.
            if (false == arState[(int)EN_MOTOR_STATE.MOTORON]
                || arState[(int)EN_MOTOR_STATE.ALARM]
                || arState[(int)EN_MOTOR_STATE.HOMETIMEOUT]
                || arState[(int)EN_MOTOR_STATE.MOTION_COMMAND_ABNORMAL]    //2021.03.31 nogami add
                || arState[(int)EN_MOTOR_STATE.MOTIONTIMEOUT])
            {
                bMotionError = true;
            }
            else if (arState[(int)EN_MOTOR_STATE.LIMITPOSITIVE])
            {
                //2020.09.18 nogami modify start
                //bMotionError = true;
                //bEmergency = mItem.configParam.bHWLimitStopModePositive;
                //2020.12.05 nogami modify start
                //if (enMotionType != EN_MOTION_TYPE.SPEED_NEG)
                if (enMotionType == EN_MOTION_TYPE.RELATIVE)
                {
                    if (m_listOfArState[nTarget][(int)EN_MOTOR_STATE.HOMEEND])
                    {
                        bMotionError = true;
                        bEmergency = mItem.configParam.bHWLimitStopModePositive;
                    }
                }
                else if (enMotionType != EN_MOTION_TYPE.SPEED_NEG)
                //2020.12.05 nogami modify end
                {
                    bMotionError = true;
                    bEmergency = mItem.configParam.bHWLimitStopModePositive;
                }
                //2020.09.18 nogami modify end
            }
            else if (arState[(int)EN_MOTOR_STATE.LIMITNEGATIVE])
            {
                //2020.09.18 nogami modify start
                //bMotionError = true;
                //bEmergency = mItem.configParam.bHWLimitStopModeNegative;
                //2020.12.05 nogami modify start
                //if (enMotionType != EN_MOTION_TYPE.SPEED_POS)
                if (enMotionType == EN_MOTION_TYPE.RELATIVE)
                {
                    if (m_listOfArState[nTarget][(int)EN_MOTOR_STATE.HOMEEND])
                    {
                        bMotionError = true;
                        bEmergency = mItem.configParam.bHWLimitStopModeNegative;
                    }
                }
                else if (enMotionType != EN_MOTION_TYPE.SPEED_POS)
                //2020.12.05 nogami modify end
                {
                    bMotionError = true;
                    bEmergency = mItem.configParam.bHWLimitStopModeNegative;
                }
                //2020.09.18 nogami modify end
            }
            else if (arState[(int)EN_MOTOR_STATE.SWLIMITPOSITIVE]
                && (enMotionType == EN_MOTION_TYPE.SPEED_POS        // 조그 정방향 이동
                    || (enMotionType == EN_MOTION_TYPE.RELATIVE     // 상대 이동이지만, 이동 포지션이 정방향일 경우
                        && mItem.dblDestination > 0)
                    || (enMotionType == EN_MOTION_TYPE.ABSOLUTE     // 절대 이동이지만, 목표 포지션이 리밋을 초과할 경우
                        && mItem.dblDestination > mItem.configParam.dblSWLimitPositionPositive)))
            {
                //2020.09.15 nogami modify start
                //bMotionError = true;
                //bEmergency = mItem.configParam.bSWLimitStopModePositive;
                if (arState[(int)EN_MOTOR_STATE.HOMEEND])
                {
                    bMotionError = true;
                    bEmergency = mItem.configParam.bSWLimitStopModePositive;
                }
                //2020.09.15 nogami modify end
            }
            else if (arState[(int)EN_MOTOR_STATE.SWLIMITNEGATIVE]
                && (enMotionType == EN_MOTION_TYPE.SPEED_NEG            // 조그 역방향 이동
                    || (enMotionType == EN_MOTION_TYPE.RELATIVE         // 상대 이동이지만, 이동 포지션이 정방향일 경우
                        && mItem.dblDestination > 0)
                    || (enMotionType == EN_MOTION_TYPE.ABSOLUTE         // 절대 이동이지만, 목표 포지션이 리밋을 초과할 경우
                        && mItem.dblDestination < mItem.configParam.dblSWLimitPositionNegative)))
            {
                //2020.09.15 nogami modify start
                //bMotionError = true;
                //bEmergency      = mItem.configParam.bSWLimitStopModeNegative;
                if (arState[(int)EN_MOTOR_STATE.HOMEEND])
                {
                    bMotionError = true;
                    bEmergency = mItem.configParam.bSWLimitStopModeNegative;
                }
                //2020.09.15 nogami modify end
            }

            // 모션에 에러가 존재할 경우, 정지한다.
            if (bMotionError)
            {
                switch (m_listOfMotionStatus[nTarget])
                {
                    case EN_MOTION_STATUS.HOMING:
                    case EN_MOTION_STATUS.ELHOMING:
                        m_motionController.StopHomeMotion(nTarget, bEmergency);
                        break;
                    case EN_MOTION_STATUS.MOVING:
                        m_motionController.StopMotion(nTarget, bEmergency);
                        break;
                }

                m_listOfMotionStatus[nTarget] = EN_MOTION_STATUS.STOP;
            }

            return bMotionError;
        }
        #endregion

        #region 모션 프로파일 관련
        private bool ConvertByInputShaping(bool isTime, double dblDistance, double dblVel, double dblAccel, double dblDecel, double dblHz
            , out double[] arVel, out double[] arAccelTime, out double[] arDecelTime, ref int nLength)
        {
            double dblShaperTime        = 0.5 / dblHz;
            int nSign                   = 1;

            arVel       = new double[5]{0,0,0,0,0};
            arAccelTime = new double[5]{0,0,0,0,0};
            arDecelTime = new double[5]{0,0,0,0,0};

            // ms로 입력 받아서 내부에서는 s로 계산 후 다시 출력을 ms로 변환한다.
            dblAccel /= 1000.0;
            dblDecel /= 1000.0;

            // 가속도일 경우, 가속 시간으로 변경한다.
            if(isTime == false)
            {
                dblAccel    = dblVel / dblAccel;
                dblDecel    = dblVel / dblDecel;
            }
            
            // 포지션이 음수일 경우
            if(dblDistance < 0)
            {
                nSign   = -1;
            }

            // 등속 시간을 계산한다.
            double dblConstantTime  = (dblDistance * nSign / dblVel) - ((dblAccel + dblDecel) * 0.5);
            double dblmVel          = 1;

            if (dblConstantTime < 0)
            {
                dblmVel     = (dblDistance * nSign) / (((dblAccel + dblDecel) * 0.5) * dblVel);
            }

            if(dblAccel < dblShaperTime
                || dblDecel < dblShaperTime)
            {
                nLength = 0;

                return false;
            }
            else if (dblAccel > dblShaperTime)
            {
                if (dblDecel > dblShaperTime)
                {
                    nLength = 5;

                    arVel[0] = 0.5 * dblVel * (dblShaperTime / (dblAccel - dblShaperTime)) * dblmVel;
                    arVel[1] = (dblVel - 0.5 * dblVel * (dblShaperTime / (dblAccel - dblShaperTime))) * dblmVel;
                    arVel[2] = dblVel * dblmVel;
                    arVel[3] = (dblVel - 0.5 * dblVel * (dblShaperTime / (dblDecel - dblShaperTime))) * dblmVel;
                    arVel[4] = 0.5 * dblVel * (dblShaperTime / (dblDecel - dblShaperTime)) * dblmVel;

                    arDecelTime[2] = arVel[4] / dblShaperTime;
                    arDecelTime[3] = (arVel[3] - arVel[4]) / (dblDecel - 2 * dblShaperTime);
                    arDecelTime[4] = arVel[4] / dblShaperTime;
                }
                else
                {
                    nLength = 3;

                    arVel[0] = 0.5 * dblVel * (dblShaperTime / (dblAccel - dblShaperTime)) * dblmVel;
                    arVel[1] = (dblVel - 0.5 * dblVel * (dblShaperTime / (dblAccel / dblShaperTime))) * dblmVel;
                    arVel[2] = dblVel * dblmVel;

                    arDecelTime[2] = arVel[2] / dblDecel;
                }

                arAccelTime[0] = arVel[0] / dblShaperTime;
                arAccelTime[1] = (arVel[1] - arVel[0]) / (dblAccel - 2 * dblShaperTime);
                arAccelTime[2] = arVel[0] / dblShaperTime;

                for (int i = 0; i < nLength; ++i)
                {
                    arVel[i] *= nSign;
                }

                return true;
            }
            else
            {
                if (dblDecel > dblShaperTime)
                {
                    nLength = 3;

                    arVel[0] = dblVel * dblmVel;
                    arVel[1] = (dblVel - 0.5 * dblVel * (dblShaperTime / (dblDecel - dblShaperTime))) * dblmVel;
                    arVel[2] = 0.5 * dblVel * (dblShaperTime / (dblDecel - dblShaperTime)) * dblmVel;

                    arAccelTime[0] = arVel[0] / dblAccel;

                    arDecelTime[0] = arVel[2] / dblShaperTime;
                    arDecelTime[1] = (arVel[1] - arVel[2]) / (dblDecel - 2 * dblShaperTime);
                    arDecelTime[2] = arVel[2] / dblShaperTime;
                }
                else
                {
                    nLength = 1;

                    arVel[0] = dblVel * dblmVel;

                    arAccelTime[0] = dblVel * dblmVel / dblAccel;

                    arDecelTime[0] = dblVel * dblmVel / dblDecel;
                }

                for (int i = 0; i < nLength; ++ i)
                {
                    arVel[i] *= nSign;
                }

                return true;
            }
        }
        #endregion

        #region 연결 상태 관리
        /// <summary>
        /// 2019.04.22 by yjlee [ADD] 링크 연결 상태를 확인하고, 비연결 시 재접속 시도한다.
        /// </summary>
        private void CheckLinkIsConnected()
        {
            //// 초기화가 되어있지 않을 경우
            //if (false == m_bInit) { return; }

            //if (false == m_tcForIntervalOfChecking.isTimeOver()) { return; }

            //m_tcForIntervalOfChecking.SetTickCount(c_uIntervalForCheckingLink);

            //// 연결이 끊긴 상태일 경우
            //if(false == m_motionController.IsLinkConnected())
            //{
            //    m_motionController.bInit    = false;

            //    // 재접속을 시도하지 않은 상태
            //    if(false == m_bTryingToConnect)
            //    {
            //        // 링크 연결을 시도한다.
            //        if (m_motionController.ConnectLink())
            //        {
            //            m_bTryingToConnect      = true;

            //            m_tcForWaitingForConnecting.SetTickCount(c_uDelayForConnecting);
            //        }
            //    }
            //    // 재접속을 시도했을 경우
            //    else
            //    {
            //        if (m_tcForWaitingForConnecting.isTimeOver())
            //        {
            //            m_tcForWaitingForConnecting.SetTickCount(c_uDelayForConnecting);

            //            Alarm.GetInstance().GenerateAlarm((int)DefineSystemAlarm.EN_SYS_ALARM.ETHERCAT_DISCONNECTED
            //                , 0
            //                , false
            //                , EN_ALARM_GRADE.ERROR);
            //        }
            //    }
            //}
            //else
            //{
            //    // 재접속 시도 중일 경우 시퀀스를 수행한다.
            //    if(m_bTryingToConnect)
            //    {
            //        m_bTryingToConnect          = false;
            //        // 파라미터를 설정한다.
            //        m_bNeedToSetParameter       = true;
            //    }
            //}
        }
        #endregion
        //Select servo parameter 2021.01.03 nogami add
        public bool SelectServoParameter(int nIndex, int nSelect)
        {
            /**
            if (false == m_dicOfMotionItem.ContainsKey(nIndex)) { return false; }
            MotionItem mItem = m_dicOfMotionItem[nIndex];
            int nTarget = 0;
            if (false == GetVaildTarget(mItem, ref nTarget)) { return false; }
            **/
            //System.Threading.Thread.Sleep(500);
            //return m_motionController.SelectServoParameter(nTarget,nSelect);
             // return m_motionController.SelectServoParameter(nIndex, nSelect);
            return true;
        }
        public int GetSelectServoParameterNum(int nIndex)
        {
            if (false == m_dicOfMotionItem.ContainsKey(nIndex)) { return 0; }

            MotionItem mItem = m_dicOfMotionItem[nIndex];

            int nTarget = 0;

            if (false == GetVaildTarget(mItem, ref nTarget)) { return 0; }

            return m_motionController.GetSelectServoParameterNum(nTarget);
        }
        #region UVT -> XYT
        // 2020.05.05 by Thienvv [DEL] MK-D12 does not use UVW
        //public void UVW_origin_update(double dX, double dY)
        //{
        //    double dR = 0.0;//dT * Math.PI / 180.0;  // convert to radian
        //    OriginUVW.OUx = dX + oUx * Math.Cos(-dR) - oUy * Math.Sin(-dR);
        //    OriginUVW.OUy = dY + oUx * Math.Sin(-dR) + oUy * Math.Cos(-dR);
        //    OriginUVW.OVx = dX + oVx * Math.Cos(-dR) - oVy * Math.Sin(-dR);
        //    OriginUVW.OVy = dY + oVx * Math.Sin(-dR) + oVy * Math.Cos(-dR);
        //    OriginUVW.OWx = dX + oWx * Math.Cos(-dR) - oWy * Math.Sin(-dR);
        //    OriginUVW.OWy = dY + oWx * Math.Sin(-dR) + oWy * Math.Cos(-dR);

        //    oUx = OriginUVW.OUx;
        //    oUy = OriginUVW.OUy;
        //    oVx = OriginUVW.OVx;
        //    oVy = OriginUVW.OVy;
        //    oWx = OriginUVW.OWx;
        //    oWy = OriginUVW.OWy;
        //}

        // 2020.05.05 by Thienvv [DEL] MK-D12 does not use UVW
        ////  2019.08.18 by hkyu [ADD] UVW function for Jog------------------------------------------
        //// 2019.08.09 by hkyu [ADD] convertXYRtoUVW (reference from [ppt,xls] files and minami prg.)
        //// 좌표계상의 dX, dY, dR 기준 moving(변화량) 값을, 실제 움직여야할 축 dU, dV, dW 의 moving(변화량) 값으로 환산. ( UVW관련 설명문서 참조요망.)
        //// XYR -> UVW
        //public void JogConvertXYTtoUVW(double dX, double dY, double dT, ref double dU, ref double dV, ref double dW)
        //{
        //    //double oUx = 210, oUy = -190;  // 기구설계값 U축 origin 위치
        //    //double oVx = 210, oVy = 190;   // 기구설계값 V축 origin 위치
        //    //double oWx = -210, oWy = 190;  // 기구설계값 W축 origin 위치

        //    double dR = dT * Math.PI / 180.0;  // convert to radian

        //    // 2019.08.25 by hkyu [ADD] 
        //    // update Joint of Motor Position

        //    dU = dX + oUx * (Math.Cos(-dR) - 1) - oUy * Math.Sin(-dR);
        //    dV = dY + oVx * Math.Sin(-dR) + oVy * (Math.Cos(-dR) - 1);
        //    dW = dY + oWx * Math.Sin(-dR) + oWy * (Math.Cos(-dR) - 1);

        //    OriginUVW.OUx = dX + oUx * Math.Cos(-dR) - oUy * Math.Sin(-dR);
        //    OriginUVW.OUy = dY + oUx * Math.Sin(-dR) + oUy * Math.Cos(-dR);
        //    OriginUVW.OVx = dX + oVx * Math.Cos(-dR) - oVy * Math.Sin(-dR);
        //    OriginUVW.OVy = dY + oVx * Math.Sin(-dR) + oVy * Math.Cos(-dR);
        //    OriginUVW.OWx = dX + oWx * Math.Cos(-dR) - oWy * Math.Sin(-dR);
        //    OriginUVW.OWy = dY + oWx * Math.Sin(-dR) + oWy * Math.Cos(-dR);

        //    oUx = OriginUVW.OUx;
        //    oUy = OriginUVW.OUy;
        //    oVx = OriginUVW.OVx;
        //    oVy = OriginUVW.OVy;
        //    oWx = OriginUVW.OWx;
        //    oWy = OriginUVW.OWy;
        //}

        // 2020.05.05 by Thienvv [DEL] MK-D12 not use UVW
        //// 2019.08.16 by hkyu [ADD] convertUVWtoXYR (reference from [ppt,xls] files and minami prg.)
        //// UVW -> XYR
        //public void JogConvertUVWtoXYT(double dU, double dV, double dW, ref double dOffsetX, ref double dOffsetY, ref double dOffsetT) // absolute value
        //{
        //    // for absolute position
        //    double Ux = 210, Uy = -190;  // 기구설계값 U축 origin 위치
        //    double Vx = 210, Vy = 190;   // 기구설계값 V축 origin 위치
        //    double Wx = -210, Wy = 190;   // 기구설계값 W축 origin 위치

        //    double dR_plus_alphaR = Math.Asin(((Wy - Vy) + (dW - dV)) / Math.Sqrt((Wx - Vx) * (Wx - Vx) + (Wy - Vy) * (Wy - Vy)));
        //    double alphaR = Math.Asin((Wy - Vy) / Math.Sqrt((Wx - Vx) * (Wx - Vx) + (Wy - Vy) * (Wy - Vy)));
        //    dOffsetT = dR_plus_alphaR - alphaR;
        //    dOffsetY = dV - Vx * Math.Sin(-dOffsetT) + Vy - Vy * Math.Cos(-dOffsetT);
        //    dOffsetX = dU + Uy * Math.Sin(-dOffsetT) + Ux - Ux * Math.Cos(-dOffsetT);
        //    dOffsetT = dOffsetT * 180.0 / Math.PI;

        //    //double dR_plus_alphaR = Math.Asin(((oWy - oVy) + (dW - dV)) / Math.Sqrt((oWx - oVx) * (oWx - oVx) + (oWy - oVy) * (oWy - oVy)));
        //    //double alphaR = Math.Asin((oWy - oVy) / Math.Sqrt((oWx - oVx) * (oWx - oVx) + (oWy - oVy) * (oWy - oVy)));
        //    //dOffsetT = dR_plus_alphaR - alphaR;
        //    //dOffsetY = dV - oVx * Math.Sin(-dOffsetT) + oVy - oVy * Math.Cos(-dOffsetT);
        //    //dOffsetX = dU + oUy * Math.Sin(-dOffsetT) + oUx - oUx * Math.Cos(-dOffsetT);
        //    //dOffsetT = dOffsetT * 180.0 / Math.PI;

        //    //return ( STS_NORMAL ) ;
        //} 
        #endregion
        #endregion

        #region 내부 클래스
        /// <summary>
        /// 2018.07.09 by yjlee [MOD] 모션 아이템을 관리하는 클래스
        /// </summary>
        private class MotionItemManager
        {
            #region 상수
            private const int c_nMaxCountOfItem = 200;  // 아이템의 최대 개수
            #endregion

            #region 변수
            private Dictionary<int, MotionItem> m_dicOfMotionItem = new Dictionary<int, MotionItem>();
            private Dictionary<int, Dictionary<int, MotionItem>> m_dicOfTargetItem = new Dictionary<int, Dictionary<int, MotionItem>>();

            private ConcurrentQueue<MotionItem> m_queueForAddedItem = new ConcurrentQueue<MotionItem>();
            private ConcurrentQueue<MotionItem> m_queueForChangedItem = new ConcurrentQueue<MotionItem>();

            private MotionItem m_selectedItem = null;

            private int m_nMaxiumIndexOfItem = -1;

            private Dictionary<int, string> m_dicOfItemName = new Dictionary<int, string>();

            // 2021.08.09 by hkyu [ADD]
            Config.ConfigMotion m_instanceMotionConfig = Config.ConfigMotion.GetInstance();
            
            #endregion

            #region 속성
            public MotionItem AddedItem
            {
                get
                {
                    if (m_queueForAddedItem.Count < 1) return null;

                    MotionItem mItem = null;

                    m_queueForAddedItem.TryDequeue(out mItem);

                    return mItem;
                }
            }
            public MotionItem ChangedItem
            {
                get
                {
                    if (m_queueForChangedItem.Count < 1) return null;

                    MotionItem mItem = null;

                    m_queueForChangedItem.TryDequeue(out mItem);

                    return mItem;
                }
            }
            public int nMaximunIndexOfItem
            {
                get
                {
                    return m_nMaxiumIndexOfItem;
                }
            }
            public Dictionary<int, string> dicOfItemName
            {
                get { return m_dicOfItemName; }
            }
            public Dictionary<int, MotionItem> dicOfMotionItem
            {
                get { return m_dicOfMotionItem; }
            }
            public Dictionary<int, Dictionary<int, MotionItem>> dicOfTargetItem
            {
                get { return m_dicOfTargetItem; }
            }
            #endregion

            #region 외부 인터페이스

            #region 아이템 값 설정
            /// <summary>
            /// 2018.07.09 by yjlee [ADD] 아이템을 추가한다.
            /// </summary>
            public void Add()
            {
                for (int i = 0; i < c_nMaxCountOfItem; ++i)
                {
                    if (m_dicOfMotionItem.ContainsKey(i) == false)
                    {
                        Add(new MotionItem(i));

                        return;
                    }
                }
            }
            /// <summary>
            /// 2018.07.09 by yjlee [ADD] 선택된 아이템을 설정한다.
            /// </summary>
            /// <param name="nIndex"></param>
            public void SetSelectedItem(int nIndex)
            {
                m_selectedItem = GetItem(nIndex);
            }
            /// <summary>
            /// 2018.12.02 by yjlee [ADD] 선택된 아이템의 인덱스를 얻어온다.
            /// </summary>
            public int GetIndexOfSelectedItem()
            {
                if (null == m_selectedItem) { return -1; }

                return m_selectedItem.nIndex;
            }
            /// <summary>
            /// 2018.07.09 by yjlee [ADD] 아이템의 사용 유무를 설정한다.
            /// </summary>
            /// <param name="bEnable"></param>
            /// <returns></returns>
            public bool SetEnable(bool bEnable)
            {
                if (m_selectedItem == null
                    || m_selectedItem.bEnable == bEnable)
                {
                    return false;
                }

                m_selectedItem.bEnable = bEnable;

                UpdateInformation();

                return true;
            }
            /// <summary>
            /// 2018.07.09 by yjlee [ADD] 아이템의 이름을 설정한다.
            /// </summary>
            /// <param name="strName"></param>
            /// <returns></returns>
            public bool SetName(string strName)
            {
                if (m_selectedItem == null
                    || m_selectedItem.strName.Equals(strName))
                {
                    return false;
                }

                if (string.IsNullOrEmpty(strName))
                {
                    strName = NONE;
                }

                m_selectedItem.strName = strName;

                // 이름 정보 업데이트
                m_dicOfItemName[m_selectedItem.nIndex] = strName;

                UpdateInformation();

                return true;
            }
            /// <summary>
            /// 2018.07.09 by yjlee [ADD] 아이템의 타겟 번호를 설정한다.
            /// </summary>
            /// <param name="nTarget"></param>
            /// <returns></returns>
            public bool SetTarget(int nTarget)
            {
                if (m_selectedItem == null
                    || m_selectedItem.nTarget == nTarget)
                {
                    return false;
                }

                if(m_dicOfTargetItem.ContainsKey(m_selectedItem.nTarget))
                {
                    m_dicOfTargetItem[m_selectedItem.nTarget].Remove(m_selectedItem.nIndex);
                }

                if (false == m_dicOfTargetItem.ContainsKey(nTarget))
                {
                    m_dicOfTargetItem.Add(nTarget, new Dictionary<int, MotionItem>());
                }

                m_dicOfTargetItem[nTarget].Add(m_selectedItem.nIndex, m_selectedItem);

                m_selectedItem.nTarget = nTarget;

                UpdateInformation();

                return true;
            }
            /// <summary>
            /// 2018.07.18 by yjlee [ADD] 조그의 그룹을 설정한다.
            /// 같은 그룹에 있을 경우 동시 제어가 가능하다.
            /// </summary>
            /// <param name="nGroup"></param>
            /// <returns></returns>
            public bool SetGroupOfJog(int nGroup)
            {
                if (m_selectedItem == null
                    || m_selectedItem.nGroupOfJog == nGroup)
                {
                    return false;
                }

                m_selectedItem.nGroupOfJog = nGroup;

                Save();

                return true;
            }

            #region 모터 설정
            /// <summary>
            /// 2018.07.09 by yjlee [ADD] 모터의 파워를 설정한다.
            /// </summary>
            /// <param name="strMotorType"></param>
            /// <returns></returns>
            public bool SetPower(int nPower)
            {
                if (m_selectedItem == null
                    || m_selectedItem.nPower == nPower)
                {
                    return false;
                }

                m_selectedItem.nPower = nPower;

                UpdateInformation();

                return true;
            }
            /// <summary>
            /// 2018.07.09 by yjlee [ADD] 모터 타입을 설정한다.
            /// </summary>
            /// <param name="strMotorType"></param>
            /// <returns></returns>
            public bool SetMotorType(string strMotorType)
            {
                if (m_selectedItem == null
                    || m_selectedItem.strMotorType.Equals(strMotorType))
                {
                    return false;
                }

                m_selectedItem.strMotorType = strMotorType;

                UpdateInformation();

                return true;
            }
            #endregion

            #endregion

            #region 파라미터 값 설정

            #region Config
            /// <summary>
            /// 2018.08.21 by jhlim [ADD] 모터의 타입을 설정한다.
            /// 직선운동의 경우 Linear, 회전 운동의 경우 Circular 를 설정 한다.
            /// </summary>
            public bool SetMotorApplicationType(string strType)
            {
                if (m_selectedItem == null
                    || m_selectedItem.configParam.strMotorApplicationType == strType)
                {
                    return false;
                }

                m_selectedItem.configParam.strMotorApplicationType = strType;

                Save();

                return true;
            }
            /// <summary>
            /// 2018.08.21 by jhlim [ADD] 스케일의 분자를 설정한다.
            /// 엔코더 분해능 당 입력 거리를 입력한다.
            /// 타입이 Circular 일 경우에는 분해능 당 각도를 입력 한다.
            /// </summary>
            public bool SetNumeratorForScale(double dblNumerator)
            {
                if (m_selectedItem == null
                    || m_selectedItem.configParam.dblNumeratorForScale == dblNumerator)
                {
                    return false;
                }

                m_selectedItem.configParam.dblNumeratorForScale = dblNumerator;

                Save();

                return true;
            }
            /// <summary>
            /// 2018.08.21 by jhlim [ADD] 스케일의 분자를 설정한다.
            /// 엔코더 분해능 당 입력 거리를 입력한다.
            /// 타입이 Circular 일 경우에는 분해능 당 각도를 입력 한다.
            /// </summary>
            public bool SetDenominatorForScale(double dblDenominator)
            {
                if (m_selectedItem == null
                    || m_selectedItem.configParam.dblDenominatorForScale == dblDenominator)
                {
                    return false;
                }

                m_selectedItem.configParam.dblDenominatorForScale = dblDenominator;

                Save();

                return true;
            }
            /// <summary>
            /// 2019.04.23 by yjlee [ADD] 커맨드 방향을 설정한다.
            /// </summary>
            public bool SetCommandDirection(string strCommandDirection)
            {
                if (m_selectedItem == null
                    || m_selectedItem.configParam.strCommandDirection.Equals(strCommandDirection))
                {
                    return false;
                }

                m_selectedItem.configParam.strCommandDirection  = strCommandDirection;

                Save();

                return true;
            }
            /// <summary>
            /// 2019.04.23 by yjlee [ADD] 커맨드 방향을 설정한다.
            /// </summary>
            public bool SetEncoderDirection(string strEncoderDirection)
            {
                if (m_selectedItem == null
                    || m_selectedItem.configParam.strEncoderDirection.Equals(strEncoderDirection))
                {
                    return false;
                }

                m_selectedItem.configParam.strEncoderDirection  = strEncoderDirection;

                Save();

                return true;
            }
            /// <summary>
            /// 2018.08.21 by jhlim [ADD] HW Limit 사용유무를 설정한다.
            /// </summary>
            public bool SetUseHWLimitPositive(bool bEnable)
            {
                if (m_selectedItem == null
                    || m_selectedItem.configParam.bUseHWLimitPositive == bEnable)
                {
                    return false;
                }

                m_selectedItem.configParam.bUseHWLimitPositive = bEnable;

                Save();

                return true;
            }
            /// <summary>
            /// 2018.08.21 by jhlim [ADD] HW Limit 사용유무를 설정한다.
            /// </summary>
            public bool SetUseHWLimitNegative(bool bEnable)
            {
                if (m_selectedItem == null
                    || m_selectedItem.configParam.bUseHWLimitNegative == bEnable)
                {
                    return false;
                }

                m_selectedItem.configParam.bUseHWLimitNegative = bEnable;

                Save();

                return true;
            }
            /// <summary>
            /// 2018.08.21 by jhlim [ADD] HW Limit 로직을 설정한다.
            /// </summary>
            public bool SetHWLimitLogicPositive(string strLogic)
            {
                if (m_selectedItem == null
                    || m_selectedItem.configParam.strHWLimitLogicPositive.Equals(strLogic))
                {
                    return false;
                }

                m_selectedItem.configParam.strHWLimitLogicPositive = strLogic;

                Save();

                return true;
            }
            /// <summary>
            /// 2018.08.21 by jhlim [ADD] HW Limit 로직을 설정한다.
            /// </summary>
            public bool SetHWLimitLogicNegative(string strLogic)
            {
                if (m_selectedItem == null
                    || m_selectedItem.configParam.strHWLimitLogicNegative.Equals(strLogic))
                {
                    return false;
                }

                m_selectedItem.configParam.strHWLimitLogicNegative = strLogic;

                Save();

                return true;
            }
            /// <summary>
            /// 2018.08.21 by jhlim [ADD] HW Limit 스탑모드를 설정한다.
            /// </summary>
            public bool SetHWLimitStopModePositive(string strStopMode)
            {
                if (m_selectedItem == null
                    || m_selectedItem.configParam.strHWLimitStopModePositive.Equals(strStopMode))
                {
                    return false;
                }

                m_selectedItem.configParam.strHWLimitStopModePositive = strStopMode;

                Save();

                return true;
            }
            /// <summary>
            /// 2018.08.21 by jhlim [ADD] HW Limit 스탑모드를 설정한다.
            /// </summary>
            public bool SetHWLimitStopModeNegative(string strStopMode)
            {
                if (m_selectedItem == null
                    || m_selectedItem.configParam.strHWLimitStopModeNegative.Equals(strStopMode))
                {
                    return false;
                }

                m_selectedItem.configParam.strHWLimitStopModeNegative = strStopMode;

                Save();

                return true;
            }
            /// <summary>
            /// 2018.08.21 by jhlim [ADD] SW Limit 사용유무를 설정한다.
            /// </summary>
            public bool SetUseSWLimitPositive(bool bEnable)
            {
                if (m_selectedItem == null
                    || m_selectedItem.configParam.bUseSWLimitPositive == bEnable)
                {
                    return false;
                }

                m_selectedItem.configParam.bUseSWLimitPositive = bEnable;

                Save();

                return true;
            }
            /// <summary>
            /// 2018.08.21 by jhlim [ADD] SW Limit 사용유무를 설정한다.
            /// </summary>
            public bool SetUseSWLimitNegative(bool bEnable)
            {
                if (m_selectedItem == null
                    || m_selectedItem.configParam.bUseSWLimitNegative == bEnable)
                {
                    return false;
                }

                m_selectedItem.configParam.bUseSWLimitNegative = bEnable;

                Save();

                return true;
            }
            /// <summary>
            /// 2018.08.21 by jhlim [ADD] SW Limit 스탑모드를 설정한다.
            /// </summary>
            public bool SetSWLimitStopModePositive(string strStopMode)
            {
                if (m_selectedItem == null
                    || m_selectedItem.configParam.strSWLimitStopModePositive.Equals(strStopMode))
                {
                    return false;
                }

                m_selectedItem.configParam.strSWLimitStopModePositive = strStopMode;

                Save();

                return true;
            }
            /// <summary>
            /// 2018.08.21 by jhlim [ADD] SW Limit 스탑모드를 설정한다.
            /// </summary>
            public bool SetSWLimitStopModeNegative(string strStopMode)
            {
                if (m_selectedItem == null
                    || m_selectedItem.configParam.strSWLimitStopModeNegative.Equals(strStopMode))
                {
                    return false;
                }

                m_selectedItem.configParam.strSWLimitStopModeNegative = strStopMode;

                Save();

                return true;
            }
            /// <summary>
            /// 2018.08.21 by jhlim [ADD] SW +Limit 값을 설정한다.
            /// </summary>
            public bool SetSWLimitPositionPositive(double dblPosition)
            {
                if (m_selectedItem == null
                    || m_selectedItem.configParam.dblSWLimitPositionPositive == dblPosition)
                {
                    return false;
                }

                m_selectedItem.configParam.dblSWLimitPositionPositive = dblPosition;

                Save();

                return true;
            }
            /// <summary>
            /// 2018.08.21 by jhlim [ADD] SW -Limit 값을 설정한다.
            /// </summary>
            public bool SetSWLimitPositionNegative(double dblPosition)
            {
                if (m_selectedItem == null
                    || m_selectedItem.configParam.dblSWLimitPositionNegative == dblPosition)
                {
                    return false;
                }

                m_selectedItem.configParam.dblSWLimitPositionNegative = dblPosition;

                Save();

                return true;
            }
            
            // 2020.03.05 by Thienvv [MOD] Merge from MK-D100 Minami
            public bool SetOutputMode(string strOutMode)
            {
                if (m_selectedItem == null
                   || m_selectedItem.configParam.strOutMode.Equals(strOutMode))
                {
                    return false;
                }

                m_selectedItem.configParam.strOutMode = strOutMode;

                Save();

                return true;
            }
			// 2020.03.05 by Thienvv [MOD] Merge from MK-D100 Minami
            public bool SetInputMode(string strInMode)
            {
                if (m_selectedItem == null
                   || m_selectedItem.configParam.strInMode.Equals(strInMode))
                {
                    return false;
                }

                m_selectedItem.configParam.strInMode = strInMode;

                Save();

                return true;
            }
			// 2020.03.05 by Thienvv [MOD] Merge from MK-D100 Minami
            public bool SetInputDirection(bool bInDirection)
            {
                if (m_selectedItem == null
                    || m_selectedItem.configParam.bInputDirection == bInDirection)
                {
                    return false;
                }

                m_selectedItem.configParam.bInputDirection = bInDirection;

                Save();

                return true;
            }
            #endregion

            #region Speed
            /// <summary>
            /// 2018.07.16 by yjlee [ADD] 모션 프로파일을 설정한다.
            /// </summary>
            public bool SetSpeedPattern(int nSpdContents, string strPattern)
            {
                if (m_selectedItem == null
                    || m_selectedItem.speedParam[nSpdContents].strSpeedPattern.Equals(strPattern))
                {
                    return false;
                }

                m_selectedItem.speedParam[nSpdContents].strSpeedPattern = strPattern;

                SaveSpeedParameters();

                return true;
            }
            /// <summary>
            /// 2018.07.16 by yjlee [ADD] 모터의 속도를 설정한다.
            /// </summary>
            public bool SetVelocity(int nSpdContents, double dblVelocity)
            {
                if (m_selectedItem == null
                    || m_selectedItem.speedParam[nSpdContents].dblVelocity == dblVelocity)
                {
                    return false;
                }

                m_selectedItem.speedParam[nSpdContents].dblVelocity = dblVelocity;

                SaveSpeedParameters();

                return true;
            }
            /// <summary>
            /// 2018.07.16 by yjlee [ADD] 모터의 최대 속도를 설정한다.
            /// </summary>
            public bool SetMaxVelocity(int nSpdContents, double dblMaxVelocity)
            {
                if (m_selectedItem == null
                    || m_selectedItem.speedParam[nSpdContents].dblMaxVelocity == dblMaxVelocity)
                {
                    return false;
                }

                m_selectedItem.speedParam[nSpdContents].dblMaxVelocity = dblMaxVelocity;

                SaveSpeedParameters();

                return true;
            }
            /// <summary>
            /// 2018.07.16 by yjlee [ADD] 가속도 시간을 설정한다.
            /// </summary>
            public bool SetAccelerationTime(int nSpdContents, double dblAccelTime)
            {
                if (m_selectedItem == null
                    || m_selectedItem.speedParam[nSpdContents].dblAccelTime == dblAccelTime)
                {
                    return false;
                }

                m_selectedItem.speedParam[nSpdContents].dblAccelTime = dblAccelTime;

                SaveSpeedParameters();

                return true;
            }
            /// <summary>
            /// 2018.07.16 by yjlee [ADD] 감속도 시간을 설정한다.
            /// </summary>
            public bool SetDecelerationTime(int nSpdContents, double dblDecelTime)
            {
                if (m_selectedItem == null
                    || m_selectedItem.speedParam[nSpdContents].dblDecelTime == dblDecelTime)
                {
                    return false;
                }

                m_selectedItem.speedParam[nSpdContents].dblDecelTime = dblDecelTime;

                SaveSpeedParameters();

                return true;
            }
            /// <summary>
            /// 2018.07.16 by yjlee [ADD] 감속도 시간을 설정한다.
            /// </summary>
            public bool SetJerkAcceleration(int nSpdContents, double dblJerkAccel)
            {
                if (m_selectedItem == null
                    || m_selectedItem.speedParam[nSpdContents].dblJerkAccelration == dblJerkAccel)
                {
                    return false;
                }

                m_selectedItem.speedParam[nSpdContents].dblJerkAccelration = dblJerkAccel;

                SaveSpeedParameters();

                return true;
            }
            /// <summary>
            /// 2018.07.16 by yjlee [ADD] 감속도 시간을 설정한다.
            /// </summary>
            public bool SetJerkDeceleration(int nSpdContents, double dblJerkDecel)
            {
                if (m_selectedItem == null
                    || m_selectedItem.speedParam[nSpdContents].dblJerkDecelration == dblJerkDecel)
                {
                    return false;
                }

                m_selectedItem.speedParam[nSpdContents].dblJerkDecelration = dblJerkDecel;

                SaveSpeedParameters();

                return true;
            }
            /// <summary>
            /// 2018.07.16 by yjlee [ADD] 모션 타임 아웃을 설정한다.
            /// </summary>
            public bool SetMotionTimeout(int nSpdContents, uint uTimeout)
            {
                if (m_selectedItem == null
                    || m_selectedItem.speedParam[nSpdContents].uMotionTimeout == uTimeout)
                {
                    return false;
                }

                m_selectedItem.speedParam[nSpdContents].uMotionTimeout = uTimeout;

                SaveSpeedParameters();

                return true;
            }
            //Mechatrolink
            public bool SetMotionFilter(int nSpdContents, uint uFilter)
            {
                if (m_selectedItem == null
                    || m_selectedItem.speedParam[nSpdContents].uFilter == uFilter)
                {
                    return false;
                }

                m_selectedItem.speedParam[nSpdContents].uFilter = uFilter;

                SaveSpeedParameters();

                return true;
            }
            #endregion

            #region Home
            /// <summary>
            /// 2018.07.17 by yjlee [ADD] 항상 HomeEnd 상태인지 설정한다.
            /// </summary>
            public bool SetAlwaysHomeEnd(bool bEnable)
            {
                if (m_selectedItem == null
                    || m_selectedItem.homeParam.bAlwaysHomeEnd == bEnable)
                {
                    return false;
                }

                m_selectedItem.homeParam.bAlwaysHomeEnd = bEnable;

                Save();

                return true;
            }
            /// <summary>
            /// 2018.07.17 by yjlee [ADD] 항상 HomeEnd 상태인지 설정한다.
            /// </summary>
            public bool SetAlwaysHomeEnd(int nIndex, bool bEnable)
            {
                MotionItem mItem = GetItem(nIndex);

                if (mItem == null) { return false; }

                mItem.homeParam.bAlwaysHomeEnd = bEnable;

                return true;
            }
            /// <summary>
            /// 2018.07.17 by yjlee [ADD] 홈 동작 방식을 설정한다.
            /// </summary>
            public bool SetHomeMode(string strHomeMode)
            {
                if (m_selectedItem == null
                    || m_selectedItem.homeParam.strHomeMode.Equals(strHomeMode))
                {
                    return false;
                }

                m_selectedItem.homeParam.strHomeMode = strHomeMode;

                Save();

                return true;
            }
            /// <summary>
            /// 2018.07.17 by yjlee [ADD] 홈 방향을 설정한다.
            /// </summary>
            public bool SetHomeDirection(string strDirection)
            {
                if (m_selectedItem == null
                    || m_selectedItem.homeParam.strHomeDirection.Equals(strDirection))
                {
                    return false;
                }

                m_selectedItem.homeParam.strHomeDirection = strDirection;

                Save();

                return true;
            }
            /// <summary>
            /// 2018.07.17 by yjlee [ADD] 홈의 펄스 로직을 설정한다.
            /// </summary>
            public bool SetHomeLogic(string strHomeLogic)
            {
                if (m_selectedItem == null
                    || m_selectedItem.homeParam.strHomeLogic.Equals(strHomeLogic))
                {
                    return false;
                }

                m_selectedItem.homeParam.strHomeLogic = strHomeLogic;

                Save();

                return true;
            }
            /// <summary>
            /// 2018.07.17 by yjlee [ADD] 홈의 탈출 거리를 설정한다.
            /// </summary>
            public bool SetEscapeDistance(double dblDistance)
            {
                if (m_selectedItem == null
                    || m_selectedItem.homeParam.dblEscDist == dblDistance)
                {
                    return false;
                }

                m_selectedItem.homeParam.dblEscDist = dblDistance;

                Save();

                return true;
            }
            /// <summary>
            /// 2018.07.17 by yjlee [ADD] 홈의 오프셋을 설정한다.
            /// </summary>
            public bool SetHomeOffset(double dblOffset)
            {
                if (m_selectedItem == null
                    || m_selectedItem.homeParam.dblHomeOffset == dblOffset)
                {
                    return false;
                }

                m_selectedItem.homeParam.dblHomeOffset = dblOffset;

                Save();

                return true;
            }
            /// <summary>
            /// 2018.07.17 by yjlee [ADD] 홈의 엔코더 카운터를 설정한다.(지상 홈 모드 시 필요)
            /// </summary>
            public bool SetEZCount(int nCount)
            {
                if (m_selectedItem == null
                    || m_selectedItem.homeParam.nEZCount == nCount)
                {
                    return false;
                }

                m_selectedItem.homeParam.nEZCount = nCount;

                Save();

                return true;
            }
            /// <summary>
            /// 2018.07.17 by yjlee [ADD] 홈 완료 후 설정될 포지션을 설정한다.
            /// </summary>
            public bool SetHomeBasePosition(double dblPosition)
            {
                if (m_selectedItem == null
                    || m_selectedItem.homeParam.dblHomeBasePosition == dblPosition)
                {
                    return false;
                }

                m_selectedItem.homeParam.dblHomeBasePosition = dblPosition;

                Save();

                return true;
            }
            /// <summary>
            /// 2018.07.17 by yjlee [ADD] 홈의 모션 프로파일을 설정한다.
            /// </summary>
            public bool SetHomeSpeedPattern(string strPattern)
            {
                if (m_selectedItem == null
                    || m_selectedItem.homeParam.strHomeSpeedPattern.Equals(strPattern))
                {
                    return false;
                }

                m_selectedItem.homeParam.strHomeSpeedPattern = strPattern;

                Save();

                return true;
            }
            /// <summary>
            /// 2018.07.17 by yjlee [ADD] 홈의 속도를 설정한다.
            /// </summary>
            public bool SetHomeVelocity(double dblVelocity)
            {
                if (m_selectedItem == null
                    || m_selectedItem.homeParam.dblHomeVelocity == dblVelocity)
                {
                    return false;
                }

                m_selectedItem.homeParam.dblHomeVelocity = dblVelocity;

                Save();

                return true;
            }
            /// <summary>
            /// 2018.07.17 by yjlee [ADD] 홈의 두 번째 속도를 설정한다.
            /// </summary>
            public bool SetHomeVelocitySecond(double dblVelocity)
            {
                if (m_selectedItem == null
                    || m_selectedItem.homeParam.dblHomeSecondVelocity == dblVelocity)
                {
                    return false;
                }

                m_selectedItem.homeParam.dblHomeSecondVelocity = dblVelocity;

                Save();

                return true;
            }
            /// <summary>
            /// 2018.07.17 by yjlee [ADD] 홈의 가속 시간을 설정한다.
            /// </summary>
            public bool SetHomeAccelerationTime(double dblAccelTime)
            {
                if (m_selectedItem == null
                    || m_selectedItem.homeParam.dblHomeAccelTime == dblAccelTime)
                {
                    return false;
                }

                m_selectedItem.homeParam.dblHomeAccelTime = dblAccelTime;

                Save();

                return true;
            }
            /// <summary>
            /// 2018.07.17 by yjlee [ADD] 홈의 감속 시간을 설정한다.
            /// </summary>
            public bool SetHomeDecelerationTime(double dblDecelTime)
            {
                if (m_selectedItem == null
                    || m_selectedItem.homeParam.dblHomeDecelTime == dblDecelTime)
                {
                    return false;
                }

                m_selectedItem.homeParam.dblHomeDecelTime = dblDecelTime;

                Save();

                return true;
            }
            /// <summary>
            /// 2018.08.22 by jhlim [ADD] 홈의 Jerk 가속도를 설정한다.
            /// </summary>
            public bool SetHomeJerkAcceleration(double dblJerkAccel)
            {
                if (m_selectedItem == null
                    || m_selectedItem.homeParam.dblHomeJerkAccelration == dblJerkAccel)
                {
                    return false;
                }

                m_selectedItem.homeParam.dblHomeJerkAccelration = dblJerkAccel;

                Save();

                return true;
            }
            /// <summary>
            /// 2018.08.22 by jhlim [ADD] 홈의 Jerk 감속도를 설정한다.
            /// </summary>
            public bool SetHomeJerkDeceleration(double dblJerkDecel)
            {
                if (m_selectedItem == null
                    || m_selectedItem.homeParam.dblHomeJerkDecelration == dblJerkDecel)
                {
                    return false;
                }

                m_selectedItem.homeParam.dblHomeJerkDecelration = dblJerkDecel;

                Save();

                return true;
            }
            /// <summary>
            /// 2018.07.17 by yjlee [ADD] 홈의 모션 완료를 설정한다.
            /// </summary>
            public bool SetHomeTimeout(uint uTimeout)
            {
                if (m_selectedItem == null
                    || m_selectedItem.homeParam.uHomeTimeout == uTimeout)
                {
                    return false;
                }

                m_selectedItem.homeParam.uHomeTimeout = uTimeout;

                Save();

                return true;
            }
            #endregion

            #region Gantry
            /// <summary>
            /// 2018.07.13 by yjlee [ADD] 현재 아이템을 GANTRY 마스터로 할 것인지 설정한다.
            /// </summary>
            /// <param name="bEnable"></param>
            /// <returns></returns>
            public bool EnableMaster(bool bEnable)
            {
                if (m_selectedItem == null
                    || m_selectedItem.gantryParam.bMaster == bEnable
                    || m_selectedItem.gantryParam.bSlave == true)
                {
                    return false;
                }

                m_selectedItem.gantryParam.nIndexOfMaster = -1;
                m_selectedItem.gantryParam.bMaster = bEnable;

                // 마스터를 해제할 경우, 슬레이브를 해제한다.
                if (bEnable == false)
                {
                    MotionItem mItem = GetItem(m_selectedItem.gantryParam.nIndexOfSlave);

                    if (mItem != null)
                    {
                        mItem.gantryParam.bSlave = false;
                        mItem.gantryParam.nIndexOfMaster = -1;
                    }
                }

                Save();

                return true;
            }
            /// <summary>
            /// 2018.07.13 by yjlee [ADD] 현재 아이템의 슬레이브를 매개변수 아이템으로 지정한다.
            /// </summary>
            /// <param name="nIndexOfSlave"></param>
            /// <returns></returns>
            public bool SetIndexOfSlave(int nIndexOfSlave)
            {
                if (m_selectedItem == null
                    || m_selectedItem.gantryParam.bMaster == false
                    || m_selectedItem.nIndex == nIndexOfSlave
                    || m_selectedItem.gantryParam.nIndexOfSlave == nIndexOfSlave)
                {
                    return false;
                }

                MotionItem mNewSlave = GetItem(nIndexOfSlave);

                // 해당 아이템이 없거나, 이미 슬레이브인 아이템을 선택하였을 경우
                if (mNewSlave == null
                    || (mNewSlave != null
                    && mNewSlave.gantryParam.bSlave))
                {
                    return false;
                }

                mNewSlave.gantryParam.bSlave = true;
                mNewSlave.gantryParam.nIndexOfMaster = m_selectedItem.nIndex;

                // 기존 슬레이브를 해제한다.
                MotionItem mOldSlave = GetItem(m_selectedItem.gantryParam.nIndexOfSlave);

                if (mOldSlave != null)
                {
                    mOldSlave.gantryParam.bSlave = false;
                    mOldSlave.gantryParam.nIndexOfMaster = -1;
                }

                m_selectedItem.gantryParam.nIndexOfSlave = nIndexOfSlave;

                Save();

                return true;
            }
            /// <summary>
            /// 2018.07.19 by yjlee [ADD] 슬레이브 축 방향을 반전시킨다.
            /// </summary>
            public bool SetSlaveInverse(bool bEnable)
            {
                if (m_selectedItem == null
                    || m_selectedItem.gantryParam.bSlaveInverse == bEnable)
                {
                    return false;
                }

                m_selectedItem.gantryParam.bSlaveInverse = bEnable;

                Save();

                return true;
            }
            #endregion

            #endregion

            #endregion

            #region 내부 인터페이스

            #region 아이템 관리
            /// <summary>
            /// 2018.07.09 by yjlee [ADD] 아이템의 변경 사항을 저장하고 업데이트한다.
            /// </summary>
            private void UpdateInformation()
            {
                Save();

                m_queueForChangedItem.Enqueue(m_selectedItem);
            }
            /// <summary>
            /// 2018.07.09 by yjlee [ADD] 해당 인덱스의 아이템을 반환한다.
            /// </summary>
            /// <param name="nIndex"></param>
            /// <returns></returns>
            private MotionItem GetItem(int nIndex)
            {
                if (false == m_dicOfMotionItem.ContainsKey(nIndex))
                {
                    return null;
                }

                return m_dicOfMotionItem[nIndex];
            }
            /// <summary>
            /// 2018.07.09 by yjlee [ADD] 아이템을 추가한다.
            /// </summary>
            private void Add(MotionItem mItem)
            {
                int nIndex = mItem.nIndex;

                m_dicOfMotionItem.Add(nIndex, mItem);

                // 해당 타겟이 존재할 경우
                if (mItem.nTarget != -1)
                {
                    if (false == m_dicOfTargetItem.ContainsKey(mItem.nTarget))
                    {
                        m_dicOfTargetItem.Add(mItem.nTarget, new Dictionary<int, MotionItem>());
                    }

                    m_dicOfTargetItem[mItem.nTarget].Add(nIndex, mItem);
                }

                m_queueForAddedItem.Enqueue(mItem);

                if (mItem.nIndex > m_nMaxiumIndexOfItem)
                {
                    m_nMaxiumIndexOfItem = nIndex;
                }

                // 이름 관리 맵
                m_dicOfItemName.Add(nIndex, mItem.strName);
            }
            /// <summary>
            /// 2018.08.25 by yjlee [ADD] 아이템 데이터를 교체한다.
            /// </summary>
            private void ChangeItem(MotionItem mItem)
            {
                int nIndex = mItem.nIndex;

                m_dicOfItemName[nIndex] = mItem.strName;
                m_queueForChangedItem.Enqueue(mItem);

                // 해당 타겟이 존재할 경우
                if (mItem.nTarget != -1)
                {
                    if (false == m_dicOfTargetItem.ContainsKey(mItem.nTarget))
                    {
                        m_dicOfTargetItem.Add(mItem.nTarget, new Dictionary<int, MotionItem>());
                    }

                    m_dicOfTargetItem[mItem.nTarget].Add(nIndex, mItem);
                }
            }
            #endregion

            #endregion

            #region 파일 입/출력
            /// <summary>
            /// 2018.07.09 by yjlee [ADD] 모터 정보를 파일로 저장한다.
            /// </summary>
            private bool Save()
            {
/***
                FileComposite fComp = new FileComposite("Motion");

                fComp.SetFileVersioin("1.0.0.0");

                foreach (KeyValuePair<int, MotionItem> kvp in m_dicOfMotionItem)
                {
                    FileGroup fg = fComp.Root.Add(kvp.Key.ToString());

                    MotionItem mItem = kvp.Value;

                    fg.Add("ENABLE", mItem.bEnable);
                    fg.Add("NAME", mItem.strName);
                    fg.Add("TARGET", mItem.nTarget);
                    fg.Add("TYPE", mItem.strMotorType);
                    fg.Add("POWER", mItem.nPower);
                    fg.Add("JOGGROUP", mItem.nGroupOfJog);

                    #region Config
                    fg = fg.Add("CONFIG");
                    fg.Add("MOTOR_APPLICATION_TYPE", mItem.configParam.enMotorApplicationType);
                    fg.Add("NUMERATOR", mItem.configParam.dblNumeratorForScale);
                    fg.Add("DENOMINATOR", mItem.configParam.dblDenominatorForScale);

                    fg.Add("DIR_CMD", mItem.configParam.bCommandDirection);
                    fg.Add("DIR_ENC", mItem.configParam.bEncoderDirection);

                    // 2020.03.05 by Thienvv [MOD] Merge from MK-D100 Minami
                    fg.Add("OUT_MODE", mItem.configParam.strOutMode);
                    fg.Add("IN_MODE", mItem.configParam.strInMode);
                    fg.Add("INPUT_DIRECTION", mItem.configParam.bInputDirection);

                    #region Limit
                    fg.Add("USEHWLIMITP", mItem.configParam.bUseHWLimitPositive);
                    fg.Add("USEHWLIMITN", mItem.configParam.bUseHWLimitNegative);
                    fg.Add("HWLIMITLOGICP", mItem.configParam.strHWLimitLogicPositive);
                    fg.Add("HWLIMITLOGICN", mItem.configParam.strHWLimitLogicNegative);
                    fg.Add("HWSTOPMODEP", mItem.configParam.strHWLimitStopModePositive);
                    fg.Add("HWSTOPMODEN", mItem.configParam.strHWLimitStopModeNegative);

                    fg.Add("USESWLIMITP", mItem.configParam.bUseSWLimitPositive);
                    fg.Add("USESWLIMITN", mItem.configParam.bUseSWLimitNegative);
                    fg.Add("SWSTOPMODEP", mItem.configParam.strSWLimitStopModePositive);
                    fg.Add("SWSTOPMODEN", mItem.configParam.strSWLimitStopModeNegative);
                    fg.Add("SWLIMITPOSP", mItem.configParam.dblSWLimitPositionPositive);
                    fg.Add("SWLIMITPOSN", mItem.configParam.dblSWLimitPositionNegative);
                    #endregion

                    #endregion

                    #region Home
                    fg = fg.Add("HOME");

                    fg.Add("HOMEEND", mItem.homeParam.bAlwaysHomeEnd);
                    fg.Add("LOGIC", mItem.homeParam.strHomeLogic);
                    fg.Add("MODE", mItem.homeParam.strHomeMode);
                    fg.Add("DIR", mItem.homeParam.strHomeDirection);
                    fg.Add("ESC", mItem.homeParam.dblEscDist);
                    fg.Add("OFFSET", mItem.homeParam.dblHomeOffset);
                    fg.Add("BASEPOS", mItem.homeParam.dblHomeBasePosition);
                    fg.Add("EZCOUNT", mItem.homeParam.nEZCount);

                    fg.Add("PATTERN", mItem.homeParam.strHomeSpeedPattern);
                    fg.Add("VEL", mItem.homeParam.dblHomeVelocity);
                    fg.Add("VEL2", mItem.homeParam.dblHomeSecondVelocity);
                    fg.Add("ACC", mItem.homeParam.dblHomeAccelTime);
                    fg.Add("DEC", mItem.homeParam.dblHomeDecelTime);
                    fg.Add("JERK ACC", mItem.homeParam.dblHomeJerkAccelration);
                    fg.Add("JERK DEC", mItem.homeParam.dblHomeJerkDecelration);
                    fg.Add("TIMEOUT", mItem.homeParam.uHomeTimeout);
                    #endregion

                    #region Gantry
                    fg = fg.Add("GANTRY");

                    fg.Add("MASTER", mItem.gantryParam.bMaster);
                    fg.Add("SLAVE", mItem.gantryParam.bSlave);
                    fg.Add("INVERSE", mItem.gantryParam.bSlaveInverse);
                    fg.Add("MASTERINDEX", mItem.gantryParam.nIndexOfMaster);
                    fg.Add("SLAVEINDEX", mItem.gantryParam.nIndexOfSlave);

                    fg = fg.fgParent;
                    #endregion
                }

                return fComp.Save(FILEPATH_CONFIG, "Motion", FILEFORMAT_CONFIG);
***/
                return true; //

            }
            /// <summary>
            /// 2018.07.09 by yjlee [ADD] 모터 정보를 읽어온다.
            /// </summary>
            public bool Load()
            {

                //FileComposite fComp = new FileComposite("Motion");

                ////파일 읽어오기에 실패했을 경우
                //if (!fComp.Load(FILEPATH_CONFIG, "Motion", FILEFORMAT_CONFIG, Save)) return false;

                ////루트 그룹을 읽어온다.
                //FileGroup fRootGroup = fComp.Root.FindGroup("Motion");

                ////그룹이 없을 경우 리턴
                //if (fRootGroup == null) return false;

                ////각 아이템을 읽어온다.
                //foreach (FileGroup fGroup in fRootGroup)
                //{
                //    int nIndex = -1;

                //    if (false == int.TryParse(fGroup.strName, out nIndex)) { continue; }

                //    if (nIndex == -1) continue;

                //    MotionItem mItem = GetItem(nIndex);
                //    bool bExist = true;

                //    if (mItem == null)
                //    {
                //        mItem = new MotionItem(nIndex);
                //        bExist = false;
                //    }

                //    mItem.bEnable = fGroup.GetBoolValue("ENABLE", false);
                //    mItem.strName = fGroup.GetStringValue("NAME", NONE);
                //    mItem.nTarget = fGroup.GetIntegerValue("TARGET", -1);
                //    mItem.strMotorType = fGroup.GetStringValue("TYPE", EN_MOTOR_TYPE.SERVO.ToString());
                //    mItem.nPower = fGroup.GetIntegerValue("POWER", 100);
                //    mItem.nGroupOfJog = fGroup.GetIntegerValue("JOGGROUP", -1);

                //    #region Config
                //    FileGroup fg = fGroup.FindGroup("CONFIG");

                //    if (fg != null)
                //    {
                //        mItem.configParam.strMotorApplicationType = fg.GetStringValue("MOTOR_APPLICATION_TYPE", EN_MOTOR_APPLICATION.LINEAR.ToString());
                //        mItem.configParam.dblNumeratorForScale = fg.GetDoubleValue("NUMERATOR", 1);
                //        mItem.configParam.dblDenominatorForScale = fg.GetDoubleValue("DENOMINATOR", 1);

                //        mItem.configParam.bCommandDirection = fg.GetBoolValue("DIR_CMD", false);
                //        mItem.configParam.bEncoderDirection = fg.GetBoolValue("DIR_ENC", false);

                //        // 2020.03.05 by Thienvv [MOD] Merge from MK-D100 Minami
                //        mItem.configParam.strOutMode = fg.GetStringValue("OUT_MODE", EN_MOTOR_OUTMODE.ABPHASE.ToString());
                //        mItem.configParam.strInMode = fg.GetStringValue("IN_MODE", EN_MOTOR_INMODE.AB4X.ToString());
                //        mItem.configParam.bInputDirection = fg.GetBoolValue("INPUT_DIRECTION", false);

                //        #region Limit
                //        mItem.configParam.bUseHWLimitPositive = fg.GetBoolValue("USEHWLIMITP", false);
                //        mItem.configParam.bUseHWLimitNegative = fg.GetBoolValue("USEHWLIMITN", false);
                //        mItem.configParam.strHWLimitLogicPositive = fg.GetStringValue("HWLIMITLOGICP", LOGIC_ACTIVE_HIGH);
                //        mItem.configParam.strHWLimitLogicNegative = fg.GetStringValue("HWLIMITLOGICN", LOGIC_ACTIVE_HIGH);
                //        mItem.configParam.strHWLimitStopModePositive = fg.GetStringValue("HWSTOPMODEP", STOPMODE_EMG);
                //        mItem.configParam.strHWLimitStopModeNegative = fg.GetStringValue("HWSTOPMODEN", STOPMODE_EMG);

                //        mItem.configParam.bUseSWLimitPositive = fg.GetBoolValue("USESWLIMITP", false);
                //        mItem.configParam.bUseSWLimitNegative = fg.GetBoolValue("USESWLIMITN", false);
                //        mItem.configParam.strSWLimitStopModePositive = fg.GetStringValue("SWSTOPMODEP", STOPMODE_EMG);
                //        mItem.configParam.strSWLimitStopModeNegative = fg.GetStringValue("SWSTOPMODEN", STOPMODE_EMG);
                //        mItem.configParam.dblSWLimitPositionPositive = fg.GetDoubleValue("SWLIMITPOSP", 0);
                //        mItem.configParam.dblSWLimitPositionNegative = fg.GetDoubleValue("SWLIMITPOSN", 0);
                //        #endregion
                //    }
                //    #endregion

                //    #region Home
                //    fg = fGroup.FindGroup("HOME");

                //    if (fg != null)
                //    {
                //        mItem.homeParam.bAlwaysHomeEnd = fg.GetBoolValue("HOMEEND", false);
                //        mItem.homeParam.strHomeLogic = fg.GetStringValue("LOGIC", LOGIC_ACTIVE_HIGH);

                //        //      2020.03.05 by Thienvv [MOD] Merge from MK-D100 Minami
                //        mItem.homeParam.strHomeMode = fg.GetStringValue("MODE", EN_MOTOR_HOMEMODE.ORG_ONESTEP.ToString());
                //        mItem.homeParam.strHomeMode = fg.GetStringValue("MODE", EN_MOTOR_HOMEMODE.H10_PLUS_DIR_EL_BACK_ORG_Z_N_EDGE.ToString());

                //        mItem.homeParam.strHomeDirection = fg.GetStringValue("DIR", DIRECTION_POSITIVE);
                //        mItem.homeParam.dblEscDist = fg.GetDoubleValue("ESC", 1);
                //        mItem.homeParam.dblHomeOffset = fg.GetDoubleValue("OFFSET", 0);
                //        mItem.homeParam.dblHomeBasePosition = fg.GetDoubleValue("BASEPOS", 0);
                //        mItem.homeParam.nEZCount = fg.GetIntegerValue("EZCOUNT", 0);

                //        mItem.homeParam.strHomeSpeedPattern = fg.GetStringValue("PATTERN", EN_SPEED_PATTERN.CONSTANT.ToString());
                //        mItem.homeParam.dblHomeVelocity = fg.GetDoubleValue("VEL", 0);
                //        mItem.homeParam.dblHomeSecondVelocity = fg.GetDoubleValue("VEL2", 0);
                //        mItem.homeParam.dblHomeAccelTime = fg.GetDoubleValue("ACC", 0);
                //        mItem.homeParam.dblHomeDecelTime = fg.GetDoubleValue("DEC", 0);
                //        mItem.homeParam.dblHomeJerkAccelration = fg.GetDoubleValue("JERK ACC", 0);
                //        mItem.homeParam.dblHomeJerkDecelration = fg.GetDoubleValue("JERK DEC", 0);

                //        mItem.homeParam.uHomeTimeout = fg.GetUnsignedIntegerValue("TIMEOUT", 0);                        
                //    }
                //    #endregion

                //    #region GANTRY

                //    fg = fGroup.FindGroup("GANTRY");

                //    if (fg != null)
                //    {
                //        mItem.gantryParam.bMaster = fGroup.GetBoolValue("MASTER", false);
                //        mItem.gantryParam.bSlave = fGroup.GetBoolValue("SLAVE", false);
                //        mItem.gantryParam.bSlaveInverse = fGroup.GetBoolValue("INVERSE", false);
                //        mItem.gantryParam.nIndexOfMaster = fGroup.GetIntegerValue("MASTERINDEX", -1);
                //        mItem.gantryParam.nIndexOfSlave = fGroup.GetIntegerValue("SLAVEINDEX", -1);
                //    }
                //    #endregion

                //    if (bExist)
                //    {
                //        ChangeItem(mItem);
                //    }
                //    else
                //    {
                //        Add(mItem);
                //    }
                //}
            
                return LoadSpeedParameters();
            }
            /// <summary>
            /// 2018.07.17 by yjlee [ADD] 스피드 파라미터를 저장한다.
            /// </summary>
            private bool SaveSpeedParameters()
            {                
                //FileComposite fComp = new FileComposite("MotionSpeedParameters");

                //fComp.SetFileVersioin("1.0.0.0");

                //foreach (var item in m_dicOfMotionItem)
                //{
                //    FileGroup fg = fComp.Root.Add(item.Key.ToString());

                //    fg.Add("NAME", m_dicOfItemName[item.Key]);

                //    MotionItem mItem = item.Value;

                //    foreach (EN_MOTOR_CONTENTOFSPEED en in Enum.GetValues(typeof(EN_MOTOR_CONTENTOFSPEED)))
                //    {
                //        if (en == EN_MOTOR_CONTENTOFSPEED.MAXCOUNT) break;

                //        fg = fg.Add(en.ToString());

                //        fg.Add("SPEED PATTERN", mItem.speedParam[(int)en].strSpeedPattern);
                //        fg.Add("VELOCITY", mItem.speedParam[(int)en].dblVelocity);
                //        fg.Add("MAX VELOCITY", mItem.speedParam[(int)en].dblMaxVelocity);
                //        fg.Add("ACCEL TIME", mItem.speedParam[(int)en].dblAccelTime);
                //        fg.Add("DECEL TIME", mItem.speedParam[(int)en].dblDecelTime);
                //        fg.Add("MOTION TIMEOUT", mItem.speedParam[(int)en].uMotionTimeout);
                //        fg.Add("JERK ACCEL", mItem.speedParam[(int)en].dblJerkAccelration);
                //        fg.Add("JERK DECEL", mItem.speedParam[(int)en].dblJerkDecelration);
                //        fg.Add("MECHATROLINK FILTER", mItem.speedParam[(int)en].uFilter);            //Mechatrolink 0.1ms

                //        fg = fg.fgParent;
                //    }
                //}
                //return fComp.Save(FILEPATH_CONFIG, "MotionSpeedParameters", FILEFORMAT_CONFIG);
                return true;
            }
            /// <summary>
            /// 2018.07.17 by yjlee [ADD] 스피드 파라미터를 불러온다.
            /// </summary>
            private bool LoadSpeedParameters()
            {

                //FileComposite fComp = new FileComposite("MotionSpeedParameters");

                //// 파일 읽어오기에 실패했을 경우
                //if (!fComp.Load(FILEPATH_CONFIG, "MotionSpeedParameters", FILEFORMAT_CONFIG, SaveSpeedParameters)) return false;

                //// 루트 그룹을 읽어온다.
                //FileGroup fRootGroup = fComp.Root.FindGroup("MotionSpeedParameters");

                //// 그룹이 없을 경우 리턴
                //if (fRootGroup == null) return false;

                //// 각 아이템을 읽어온다.
                //foreach (FileGroup fGroup in fRootGroup)
                //{
                //    int nIndex = -1;

                //    if (false == int.TryParse(fGroup.strName, out nIndex)) { continue; }

                //    if (false == m_dicOfMotionItem.ContainsKey(nIndex)) continue;

                //    MotionItem mItem = m_dicOfMotionItem[nIndex];

                //    foreach (EN_MOTOR_CONTENTOFSPEED en in Enum.GetValues(typeof(EN_MOTOR_CONTENTOFSPEED)))
                //    {
                //        if (en == EN_MOTOR_CONTENTOFSPEED.MAXCOUNT) break;

                //        FileGroup fg = fGroup.FindGroup(en.ToString());

                //        mItem.speedParam[(int)en].strSpeedPattern = fg.GetStringValue("SPEED PATTERN", EN_SPEED_PATTERN.CONSTANT.ToString());
                //        mItem.speedParam[(int)en].dblVelocity = fg.GetDoubleValue("VELOCITY", 0);
                //        mItem.speedParam[(int)en].dblMaxVelocity = fg.GetDoubleValue("MAX VELOCITY", 0);
                //        mItem.speedParam[(int)en].dblAccelTime = fg.GetDoubleValue("ACCEL TIME", 0);
                //        mItem.speedParam[(int)en].dblDecelTime = fg.GetDoubleValue("DECEL TIME", 0);
                //        mItem.speedParam[(int)en].uMotionTimeout = fg.GetUnsignedIntegerValue("MOTION TIMEOUT", 0);
                //        mItem.speedParam[(int)en].dblJerkAccelration = fg.GetDoubleValue("JERK ACCEL", 0);
                //        mItem.speedParam[(int)en].dblJerkDecelration = fg.GetDoubleValue("JERK DECEL", 0);
                //        mItem.speedParam[(int)en].uFilter = fg.GetUnsignedIntegerValue("MECHATROLINK FILTER", 100); //Mechatrolink 0.1ms

                //        fg = fg.fgParent;
                //    }
                //}

                return true;
            }
            #endregion
        }

        /// <summary>
        /// 2018.07.11 by yjlee [ADD] 리핏 아이템에 대한 클래스이다.
        /// </summary>
        private class RepeatItem
        {
            #region 변수
            private bool m_bFirst = true;
            private double m_dblPositionFirst = 0;
            private double m_dblPositionSecond = 0;
            private uint m_uDelayFirst = 0;
            private uint m_uDelaySecond = 0;
            private uint m_uStep = 0;
            private bool m_bStart = false;
            private TickCounter m_tcForDelay = new TickCounter();

            private uint m_uRepeatCount = 0;      // 2019.09.26. by shkim. [ADD] REPEAT 카운트
            private uint m_uRepeatedCount = 0;      // 2019.09.26. by shkim. [ADD] REPEAT 카운트
            #endregion

            #region 속성

            public uint RepeatCount
            {
                set { m_uRepeatCount = value;}
                get { return m_uRepeatCount; }
            }
            public uint RepeateadCount
            {
                set { m_uRepeatedCount = value; }
                get { return m_uRepeatedCount; }
            }

            public bool IsFirst
            {
                set { m_bFirst = value; }
                get { return m_bFirst; }
            }
            public double dblPositionFirst
            {
                get { return m_dblPositionFirst; }
            }
            public double dblPositionSecond
            {
                get { return m_dblPositionSecond; }
            }
            public uint nStep
            {
                set { m_uStep = value; }
                get { return m_uStep; }
            }
            public bool bStart
            {
                set { m_bStart = value; }
                get { return m_bStart; }
            }
            #endregion

            #region 외부 인터페이스
            /// <summary>
            /// 2018.08.25 by yjlee [ADD] 리핏을 시작한다.
            /// </summary>
            public void StartRepeat(double dblPosFirst, uint uDelayFirst, double dblPosSecond, uint uDelaySecond, uint uRepeatCount = 0)
            {
                m_bStart = true;

                m_dblPositionFirst = dblPosFirst;
                m_dblPositionSecond = dblPosSecond;

                m_uStep = 0;

                m_uDelayFirst = uDelayFirst;
                m_uDelaySecond = uDelaySecond;

                m_uRepeatCount = uRepeatCount;
            }
            /// <summary>
            /// 2018.08.25 by yjlee [ADD] 리핏을 종료한다.
            /// </summary>
            public void StopRepeat()
            {
                m_bStart = false;
            }
            /// <summary>
            /// 2018.07.11 by yjlee [ADD] 첫 번째 딜레이를 설정한다.
            /// </summary>
            public void SetFirstDelay()
            {
                m_tcForDelay.SetTickCount(m_uDelayFirst);
            }
            /// <summary>
            /// 2018.07.11 by yjlee [ADD] 두 번째 딜레이를 설정한다.
            /// </summary>
            public void SetSecondDelay()
            {
                m_tcForDelay.SetTickCount(m_uDelaySecond);
            }
            /// <summary>
            /// 2018.07.11 by yjlee [ADD] 지연시간이 경과되었는지 확인
            /// </summary>
            /// <returns></returns>
            public bool IsDelayOver()
            {
                return m_tcForDelay.IsTickOver(true);
            }
            #endregion
        }

        /// <summary>
        /// 2018.07.12 by yjlee [ADD] 움직인 모션 아이템의 정보를 저장하는 클래스이다.
        /// </summary>
        private class MovedMotion
        {
            private TickCounter m_tcForTimeOut = new TickCounter();
            private TickCounter m_tcForDelay = new TickCounter();

            private uint m_uTimeout;
            private uint m_uDelay;

            public int nSeqNumber;
            public uint uDelay
            {
                get { return m_uDelay; }
            }
            /// <summary>
            /// 2018.08.25 by yjlee [ADD] 움직임을 시작한다.
            /// </summary>
            public void StartMoving(uint uTimeout, uint uDelay = 0)
            {
                nSeqNumber = 0;

                m_uTimeout = uTimeout;
                m_uDelay = uDelay;

                SetTimeout();
            }
            /// <summary>
            /// 2018.07.12 by yjlee [ADD] 해당 모션 아이템의 타임아웃을 설정한다.
            /// </summary>
            /// <param name="uTimeout"></param>
            public void SetTimeout()
            {
                m_tcForTimeOut.SetTickCount(m_uTimeout);
            }
            /// <summary>
            /// 2018.07.12 by yjlee [ADD] 해당 모션 아이템의 딜레이를 설정한다.
            /// </summary>
            /// <param name="uDelay"></param>
            public void SetDelay()
            {
                m_tcForDelay.SetTickCount(m_uDelay);
            }
            /// <summary>
            /// 2018.07.14 by yjlee [ADD] 해당 모션 아이템의 딜레이를 설정한다.
            /// </summary>
            /// <param name="uDelay"></param>
            public void SetDelay(uint uDelay)
            {
                m_tcForDelay.SetTickCount(uDelay);
            }
            /// <summary>
            /// 2018.07.12 by yjlee [ADD] 해당 모션 아이템의 타임아웃이 초과했는지 확인한다.
            /// </summary>
            /// <returns></returns>
            public bool IsTimeoutOver()
            {
                return m_tcForTimeOut.IsTickOver(true);
            }
            /// <summary>
            /// 2018.07.12 by yjlee [ADD] 해당 모션 아이템의 딜레이가 초과했는지 확인한다.
            /// </summary>
            /// <returns></returns>
            public bool IsDelayOver()
            {
                return m_tcForDelay.IsTickOver(true);
            }
        }
        #endregion
    }

    namespace MotionOnly
    {
        #region 모터 설정 관련 열거체
        public enum EN_MOTOR_OUTMODE
        {
            ONEPULSE_HIGH_LOW_HIGH,
            ONEPULSE_LOW_LOW_HIGH,
            ONEPULSE_HIGH_HIGH_LOW,
            ONEPULSE_LOW_HIGH_LOW,
            TWOCW_CCW_HIGH,
            TWOCW_CCW_LOW,
            TWOCCW_CW_HIGH,
            TWOCCW_CW_LOW,
            ABPHASE,
            ABPAHSE_REVERSE
        }
        public enum EN_MOTOR_INMODE
        {
            AB1X,
            AB2X,
            AB4X,
            CWCCW,
            STEP
        }
        public enum EN_SPEED_PATTERN
        {
            CONSTANT,       // 가감속을 수행하지 않는다.
            TRAPEZOIDAL,    // 사다리꼴 가감속
            S_CURVE,        // S 커브
            ANTI_VIBRATION, // 제진제어
            KEEP            // 기존 설정 유지
        }
        public enum EN_MOTOR_STATE
        {
            MOTORON             = 0,
            MOTIONDONE          = 1,
            HOMESWITCH          = 2,
            HOMEEND             = 3,
            
            // ERRORS
            ALARM               = 4,
            LIMITPOSITIVE       = 5,
            LIMITNEGATIVE       = 6,
            SWLIMITPOSITIVE     = 7,
            SWLIMITNEGATIVE     = 8,
            HOMETIMEOUT         = 9,
            MOTIONTIMEOUT       = 10,
            //2020.03.09 nogami modify start
            //MAXCOUNT = 11,
            PRE_SWLIMITPOSITIVE = 11,
            PRE_SWLIMITNEGATIVE = 12,
            //2020.03.09 nogami modify end
            MOTION_COMMAND_ABNORMAL = 13,   //2021.03.31 nogami add

            //2020.09.30 nogami add start
            //外部入力信号
            EXT_IO_1 = 14,
            EXT_IO_2 = 15,
            EXT_IO_3 = 16,
            //2020.09.30 nogami add end

            MAXCOUNT = 17,
        }
        public enum EN_MOTOR_HOMEMODE
        {
            // 2020.03.05 by Thienvv [MOD] Merge from MK-D100 Minami
            H0_MINUS_DIR_EL_BACK_ORG_N_EDGE = 0,
            H9_PLUS_DIR_EL_BACK_ORG_N_EDGE = 9,

            H1_MINUS_DIR_EL_BACK_ORG_Z_N_EDGE = 1,
            H10_PLUS_DIR_EL_BACK_ORG_Z_N_EDGE = 10,

            H2_MINUS_DIR_S_LIMIT_BACK_P_EDGE = 2,
            H4_MINUS_DIR_EL_BACK_P_EDGE = 4,

            H3_PLUS_DIR_S_LIMIT_BACK_P_EDGE = 3,
            H5_PLUS_DIR_EL_BACK_P_EDGE = 5,

            H6_MINUS_DIR_S_LIMIT_BACK_Z_N_EDGE = 6,
            H7_PLUS_DIR_S_LIMIT_BACK_Z_N_EDGE = 7,

            H8_MINUS_DIR_EL_BACK_Z_P_EDGE = 8,

            H11_USER_MINUS_DIR_ORG_N_EDGE_ZPHASE_THEN_MODE = 11, // 2019.07.25 by Thienvv [ADD] User Custom Mode for U, V, W axes

            // 2020.03.05 by Thienvv [MOD] for RSA and others
            ORG_ONESTEP,
            ORG_TWOSTEP,
            ORG_EZ_ONESTEP_INSTANTSTOP,
            ORG_EZ_ONESTEP_STOP,
            ORG_EZ_TWOSTEP_INSTANTSTOP,
            ORG_EZ_TWOSTEP_STOP,
            EZ_ONESTEP,                 // 지상만으로 홈 로직을 수행한다.
            EL_ONESTEP,
            EL_TWOSTEP,
            EL_EZ_TWOSTEP_INSTANTSTOP,
            EL_EZ_TWOSTEP_STOP,
            FIXED,                      // 현재 위치를 홈 위치로 지정한다.

            #region <Mechatrolink>
            HMETHOD_DEC1_C,                 //  0: DEC1+C相パルス方式       
            HMETHOD_ZERO,                   //  1: ZERO信号方式             
            HMETHOD_DEC1_ZERO,              //  2: DEC1+ZERO信号方式        
            HMETHOD_C,                      //  3: C相パルス方式            
            HMETHOD_C_ONLY,                // 11: C Pulse Only				
            HMETHOD_POT_C,                 // 12: POT & C Pulse			
            HMETHOD_POT_ONLY,              // 13: POT Only					
            HMETHOD_HOMELS_C,              // 14: Home LS C Pulse			
            HMETHOD_HOMELS_ONLY,           // 15: Home LS Only				
            HMETHOD_NOT_C,                 // 16: NOT & C Pulse			
            HMETHOD_NOT_ONLY,              // 17: NOT Only
            HMETHOD_INPUT_C,               // 18: Input & C Pulse			
            HMETHOD_INPUT_ONLY,            // 19: Input Only
            #endregion </Mechatrolink>
        }
        public enum EN_MOTOR_TYPE
        {
            SERVO,
            STEP
        }
        public enum EN_MOTOR_APPLICATION
        {
            LINEAR,
            CIRCULAR,
        }
        #endregion

        #region 모터 설정 관련 구조체
        public struct MOTOR_CONFIG_PARAMETER
        {
            #region 변수
            public EN_MOTOR_INMODE enInMode;        // 입력 모드
            public EN_MOTOR_OUTMODE enOutMode;      // 출력 모드
            public bool bInputDirection;            // INPUT MODE의 엔코더 값의 방향의 반전 유무
            public double dblInOutRatio;            // INPUT과 OUTPUT의 분해능
            public double dblUnitDistance;          // 거리 당 펄스 수, Du = Pr(1회전 당 펄스 수)/Lr(1회전 당 거리)
            public bool bAlarmLogic;                // 알람 로직에 대한 설정, FALSE : A(NO), TRUE : B(NC)
            public bool bAlarmStopMode;             // 정지 모드, FALSE : 즉시정지, TRUE: 감속 후 정지

            public bool bEZLogic;                   // Z상 로직에 대한 설정, FALSE : A(NO), TRUE : B(NC)

            public bool bUseInposition;             // INPOSITION 사용 유무에 대한 설정, 사용 시 Command 출력이 완료되더라도 INP신호 ON 전에는 미완료
            public bool bInpositionLogic;           // INPOSITION 로직에 대한 설정, FALSE : A(NO), TRUE : B(NC)

            public EN_MOTOR_APPLICATION enMotorApplicationType;        // Linear or Circular 인지 설정.
            public double dblNumeratorForScale;       // Scale 에서의 분자값 -> 엔코더 분해능 당 이동하는 거리를 입력 한다.(Linear Type 인 경우 mm, Circular Type 인 경우 Deg)
            public double dblDenominatorForScale;     // Scale 에서의 분모값 -> 엔코더 분해능을 입력 한다.(단위는 Pulse or Count)

            public bool bCommandDirection;          // 커맨드 방향, FALSE : No Invert, TRUE : Invert
            public bool bEncoderDirection;          // 엔코더 방향, FALSE : No Invert, TRUE : Invert

            #region 리밋 관련
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
            #endregion

            #endregion

            #region 속성
            public string strInMode
            {
                set
                {
                    foreach (EN_MOTOR_INMODE inMode in Enum.GetValues(typeof(EN_MOTOR_INMODE)))
                    {
                        if (value.Equals(inMode.ToString()))
                        {
                            enInMode = inMode;

                            break;
                        }
                    }
                }
                get
                {
                    return enInMode.ToString();
                }
            }

            // 2019.02.13 by junho [ADD] Comizoa 필요 함수 추가
            public string strInDirection
            {
                set
                {
                    bInputDirection = value.Equals(MotionInterfaces.TRUE);
                }
                get
                {
                    return bInputDirection ? MotionInterfaces.TRUE : MotionInterfaces.FALSE;
                }
            }
            public string strOutMode
            {
                set
                {
                    foreach (EN_MOTOR_OUTMODE outMode in Enum.GetValues(typeof(EN_MOTOR_OUTMODE)))
                    {
                        if (value.Equals(outMode.ToString()))
                        {
                            enOutMode = outMode;

                            break;
                        }
                    }
                }
                get
                {
                    return enOutMode.ToString();
                }
            }
            public string strMotorApplicationType
            {
                set
                {
                    foreach (EN_MOTOR_APPLICATION type in Enum.GetValues(typeof(EN_MOTOR_APPLICATION)))
                    {
                        if (value.Equals(type.ToString()))
                        {
                            enMotorApplicationType = type;
                            break;
                        }
                    }
                }
                get
                {
                    return enMotorApplicationType.ToString();
                }
            }
            public string strSWLimitStopModePositive
            {
                set
                {
                    bSWLimitStopModePositive = value.Equals(MotionInterfaces.STOPMODE_EMG);
                }
                get
                {
                    return bSWLimitStopModePositive ? MotionInterfaces.STOPMODE_EMG : MotionInterfaces.STOPMODE_DECEL;
                }
            }
            public string strSWLimitStopModeNegative
            {
                set
                {
                    bSWLimitStopModeNegative = value.Equals(MotionInterfaces.STOPMODE_EMG);
                }
                get
                {
                    return bSWLimitStopModeNegative ? MotionInterfaces.STOPMODE_EMG : MotionInterfaces.STOPMODE_DECEL;
                }
            }
            public string strHWLimitStopModePositive
            {
                set
                {
                    bHWLimitStopModePositive = value.Equals(MotionInterfaces.STOPMODE_EMG);
                }
                get
                {
                    return bHWLimitStopModePositive ? MotionInterfaces.STOPMODE_EMG : MotionInterfaces.STOPMODE_DECEL;
                }
            }
            public string strHWLimitStopModeNegative
            {
                set
                {
                    bHWLimitStopModeNegative = value.Equals(MotionInterfaces.STOPMODE_EMG);
                }
                get
                {
                    return bHWLimitStopModeNegative ? MotionInterfaces.STOPMODE_EMG : MotionInterfaces.STOPMODE_DECEL;
                }
            }
            public string strHWLimitLogicPositive
            {
                set
                {
                    bHWLimitLogicPositive = value.Equals(MotionInterfaces.LOGIC_ACTIVE_LOW);
                }
                get
                {
                    return bHWLimitLogicPositive ? MotionInterfaces.LOGIC_ACTIVE_LOW : MotionInterfaces.LOGIC_ACTIVE_HIGH;
                }
            }
            public string strHWLimitLogicNegative
            {
                set
                {
                    bHWLimitLogicNegative = value.Equals(MotionInterfaces.LOGIC_ACTIVE_LOW);
                }
                get
                {
                    return bHWLimitLogicNegative ? MotionInterfaces.LOGIC_ACTIVE_LOW : MotionInterfaces.LOGIC_ACTIVE_HIGH;
                }
            }
            public string strCommandDirection
            {
                set
                {
                    bCommandDirection = value.Equals(MotionInterfaces.INVERT_ON);
                }
                get
                {
                    return bCommandDirection ? MotionInterfaces.INVERT_ON : MotionInterfaces.INVERT_OFF;
                }
            }
            public string strEncoderDirection
            {
                set
                {
                    bEncoderDirection = value.Equals(MotionInterfaces.INVERT_ON);
                }
                get
                {
                    return bEncoderDirection ? MotionInterfaces.INVERT_ON : MotionInterfaces.INVERT_OFF;
                }
            }
            #endregion
        }
        /// <summary>
        /// 2019.04.30 by yjlee [ADD] 구조체 정보 수정
        /// </summary>
        public struct MOTOR_HOME_PARAMETER
        {
            #region 공용 함수
            /// <summary>
            /// 2019.05.06 by yjlee [ADD] 가속도 및 감속도를 계산한다. (Second to MilliSecond)
            /// </summary>
            private static void ConvertTimeToValue(double dblVelocity, double dblTime, ref double dblResult)
            {
                dblResult = dblVelocity / (dblTime * 0.001);
            }
            #endregion

            #region 변수
            private bool m_bHomeLogic;                  // 홈의 로직을 설정, FALSE : A(NO), TRUE : B(NC)
            private bool m_bHomeDirection;                 // 홈의 방향, FALSE : - , TRUE : +

            private double m_dblHomeVelocity;
            private double m_dblHomeAccelTime;
            private double m_dblHomeDecelTime;
            private double m_dblHomeAcceleration;
            private double m_dblHomeDeceleration;

            public bool bAlwaysHomeEnd;
            public EN_MOTOR_HOMEMODE enHomeMode;
            public int nEZCount;
            public double dblEscDist;                       // 원점 탈출 거리
            public double dblHomeOffset;                    // 원점 복귀 후 Offset 값
            public double dblHomeBasePosition;        // Homing 후 지정할 포지션

            public EN_SPEED_PATTERN enHomeSpeedPattern;     // 홈 속도 모드
            public double dblHomeSecondVelocity;              // 홈 역방향 속도
            public double dblHomeJerkAccelration;            // 모터 가속 Jerk
            public double dblHomeJerkDecelration;            // 모터 감속 Jerk
            public uint uHomeTimeout;
            #endregion

            #region 속성
            public string strHomeSpeedPattern
            {
                set
                {
                    foreach (EN_SPEED_PATTERN spdPt in Enum.GetValues(typeof(EN_SPEED_PATTERN)))
                    {
                        if (value.Equals(spdPt.ToString()))
                        {
                            enHomeSpeedPattern = spdPt;

                            break;
                        }
                    }
                }
                get
                {
                    return enHomeSpeedPattern.ToString();
                }
            }
            public int nHomeMode
            {
                set
                {
                    foreach (EN_MOTOR_HOMEMODE homeMode in Enum.GetValues(typeof(EN_MOTOR_HOMEMODE)))
                    {
                        if ((int)homeMode == value)
                        {
                            enHomeMode = homeMode;

                            break;
                        }
                    }
                }
                get
                {
                    return (int)enHomeMode;
                }
            }
            public string strHomeMode
            {
                set
                {
                    foreach (EN_MOTOR_HOMEMODE homeMode in Enum.GetValues(typeof(EN_MOTOR_HOMEMODE)))
                    {
                        if (value.Equals(homeMode.ToString()))
                        {
                            enHomeMode = homeMode;

                            break;
                        }
                    }
                }
                get
                {
                    return enHomeMode.ToString();
                }
            }
            public bool bHomeLogic
            {
                get { return m_bHomeLogic; }
            }
            public string strHomeLogic
            {
                set
                {
                    m_bHomeLogic = value.Equals(MotionInterfaces.LOGIC_ACTIVE_LOW);
                }
                get
                {
                    return m_bHomeLogic ? MotionInterfaces.LOGIC_ACTIVE_LOW : MotionInterfaces.LOGIC_ACTIVE_HIGH;
                }
            }
            public string strHomeDirection
            {
                set
                {
                    m_bHomeDirection = value.Equals(MotionInterfaces.DIRECTION_POSITIVE);
                }
                get { return m_bHomeDirection ? MotionInterfaces.DIRECTION_POSITIVE : MotionInterfaces.DIRECITON_NEGATIVE; }
            }
            public bool bHomeDirection
            {
                set
                {
                    m_bHomeDirection = value;
                }
                get
                {
                    return m_bHomeDirection;
                }
            }
            public double dblHomeVelocity
            {
                set
                {
                    m_dblHomeVelocity = value;

                    ConvertTimeToValue(m_dblHomeVelocity
                        , m_dblHomeAccelTime
                        , ref m_dblHomeAcceleration);

                    ConvertTimeToValue(m_dblHomeVelocity
                        , m_dblHomeDecelTime
                        , ref m_dblHomeDeceleration);
                }
                get { return m_dblHomeVelocity; }
            }
            public double dblHomeAccelTime
            {
                set
                {
                    m_dblHomeAccelTime = value;

                    ConvertTimeToValue(m_dblHomeVelocity
                        , m_dblHomeAccelTime
                        , ref m_dblHomeAcceleration);
                }
                get { return m_dblHomeAccelTime; }
            }
            public double dblHomeDecelTime
            {
                set
                {
                    m_dblHomeDecelTime = value;

                    ConvertTimeToValue(m_dblHomeVelocity
                        , m_dblHomeDecelTime
                        , ref m_dblHomeDeceleration);
                }
                get { return m_dblHomeDecelTime; }
            }
            public double dblHomeAcceleration
            {
                get { return m_dblHomeAcceleration; }
            }
            public double dblHomeDeceleration
            {
                get { return m_dblHomeDeceleration; }
            }
            #endregion
        }
        /// <summary>
        /// 2019.04.30 by yjlee [MOD] 구조체 정보 수정
        /// </summary>
        public struct MOTOR_SPEED_PARAMETER
        {
            #region 공용 함수
            /// <summary>
            /// 2019.05.06 by yjlee [ADD] 가속도 및 감속도를 계산한다. (Second to MilliSecond)
            /// </summary>
            private static void ConvertTimeToValue(double dblVelocity, double dblTime, ref double dblResult)
            {
                dblResult   = dblVelocity / (dblTime * 0.001);
            }
            #endregion

            #region 변수
            private double m_dblVelocity;       // 모터의 속도(mm/s)
            private double m_dblAccelTime;      // 모터 가속 시간(ms)
            private double m_dblDecelTime;      // 모터 감속 시간(ms)
            private double m_dblAcceleration;   // 모터 가속도
            private double m_dblDeceleration;   // 모터 감속도

            public EN_SPEED_PATTERN enSpeedPattern;  // 속도 모드
            public double dblMaxVelocity;                 // 모터 속도
            public double dblJerkAccelration;           // 모터 가속 Jerk
            public double dblJerkDecelration;           // 모터 감속 Jerk
            public uint uMotionTimeout;
            public uint uFilter;                //Mechatrolink 0.1ms
            #endregion

            #region 속성
            public string strSpeedPattern
            {
                set
                {
                    foreach (EN_SPEED_PATTERN spdPt in Enum.GetValues(typeof(EN_SPEED_PATTERN)))
                    {
                        if (value.Equals(spdPt.ToString()))
                        {
                            enSpeedPattern = spdPt;

                            return;
                        }
                    }

                    enSpeedPattern  = EN_SPEED_PATTERN.CONSTANT;
                }
                get
                {
                    return enSpeedPattern.ToString();
                }
            }
            public double dblVelocity
            {
                set
                {
                    m_dblVelocity       = value;

                    ConvertTimeToValue(m_dblVelocity
                        , m_dblAccelTime
                        , ref m_dblAcceleration);

                    ConvertTimeToValue(m_dblVelocity
                        , m_dblDecelTime
                        , ref m_dblDeceleration);
                }
                get { return m_dblVelocity; }
            }
            public double dblAccelTime
            {
                set
                {
                    m_dblAccelTime      = value;

                    ConvertTimeToValue(m_dblVelocity
                        , m_dblAccelTime
                        , ref m_dblAcceleration);
                }
                get { return m_dblAccelTime; }
            }
            public double dblDecelTime
            {
                set
                {
                    m_dblDecelTime      = value;

                    ConvertTimeToValue(m_dblVelocity
                        , m_dblDecelTime
                        , ref m_dblDeceleration);
                }
                get { return m_dblDecelTime; }
            }
            public double dblAcceleration
            {
                get { return m_dblAcceleration; }
            }
            public double dblDeceleration
            {
                get { return m_dblDeceleration; }
            }
            #endregion
        }
        /// <summary>
        /// 2019.04.30 by yjlee [MOD] Gantry 관련 정보
        /// </summary>
        public struct MOTOR_GANTRY_PARAMETER
        {
            #region 변수
            private bool m_bMaster;
            private bool m_bSlave;
            private bool m_bSlaveInverse;
            private int m_nIndexOfMaster;
            private int m_nIndexOfSlave;
            #endregion

            #region 속성
            public bool bMaster { set { m_bMaster = value; } get { return m_bMaster; } }
            public bool bSlave { set { m_bSlave = value; } get { return m_bSlave; } }
            public bool bSlaveInverse { set { m_bSlaveInverse = value; } get { return m_bSlaveInverse; } }
            public int nIndexOfSlave { set { m_nIndexOfSlave = value; } get { return m_nIndexOfSlave; } }
            public int nIndexOfMaster { set { m_nIndexOfMaster = value; } get { return m_nIndexOfMaster; } }
            #endregion
        }
        #endregion

        #region 모션 상태 관련 구조체
        /// <summary>
        /// 2018.08.25 by yjlee [ADD] 모션 상태를 위한 구조체
        /// </summary>
        public enum EN_MOTION_STATUS
        {
            READY,      // 준비 상태
            MOVING,     // 이송 상태
            HOMING,     // 홈 상태
            ELHOMING,   // 홈 상태(에러 리밋 무시)
            STOP,       // 정지
            DELAY,      // 딜레이
        }
        #endregion

        #region 모션 타입 열거체
        public enum EN_MOTION_TYPE
        {
            ABSOLUTE,
            RELATIVE,
            SPEED_POS,
            SPEED_NEG,
            HOMING,
            OVERRIDE,
            CIRCLE_CCW,
            CIRCLE_CW,
        }
        #endregion
            #region <BallAttach>
        //ヘリカル補間回転方向
        public enum EN_BALL_ATTACH_MOTION
        {
            CCW = 0,
            CW,
        }
            #endregion </BallAttach>
        /// <summary>
        /// 2018.07.09 by yjlee [MOD] 모션 아이템의 클래스
        /// </summary>
        public class MotionItem
        {
            #region 변수
            private bool m_bEnable = true;
            private int m_nIndex = -1;
            private int m_nTarget = -1;
            private string m_strName = MotionInterfaces.NONE;
            private int m_nPowerRate = 100;
            private EN_MOTOR_TYPE m_enMotorType = EN_MOTOR_TYPE.SERVO;

            private string m_strUsingTask = MotionInterfaces.MOVE_MANUAL ;

            public MotionInterfaces.MOTOR_CONFIG_PARAMETER configParam;
            public MotionInterfaces.MOTOR_HOME_PARAMETER homeParam;     // 2021.07.13
            public MotionInterfaces.MOTOR_SPEED_PARAMETER[] speedParam;  // 2021.07.13        
            public MotionInterfaces.MOTOR_GANTRY_PARAMETER gantryParam;

            private int m_nGroupOfJog = -1;

            #region 로그 전용 변수
            private EN_MOTION_TYPE m_enMotionType = EN_MOTION_TYPE.ABSOLUTE;
            private double m_dblCommandPosition = 0;
            private double m_dblActualPosition = 0;
            private double m_dblDestination = 0;
            public MOTOR_SPEED_PARAMETER spdRecentParam = new MOTOR_SPEED_PARAMETER();
            #endregion

            #endregion

            #region 속성
            public string strUsingTask { set { m_strUsingTask = value; } get { return m_strUsingTask; } }
            public string strMotorType
            {
                set
                {
                    foreach (EN_MOTOR_TYPE mType in Enum.GetValues(typeof(EN_MOTOR_TYPE)))
                    {
                        if (value.Equals(mType.ToString()))
                        {
                            m_enMotorType = mType;

                            break;
                        }
                    }
                }
                get
                {
                    return m_enMotorType.ToString();
                }
            }
            public EN_MOTOR_TYPE enMotorType
            {
                get { return m_enMotorType; }
            }
            public bool bEnable { set { m_bEnable = value; } get { return m_bEnable; } }
            public int nIndex { get { return m_nIndex; } }
            public int nTarget { set { m_nTarget = value; } get { return m_nTarget; } }
            public string strName { set { m_strName = value; } get { return m_strName; } }
            public int nPower { set { m_nPowerRate = value; } get { return m_nPowerRate; } }
            public int nGroupOfJog { set { m_nGroupOfJog = value; } get { return m_nGroupOfJog; } }
            public EN_MOTION_TYPE enMotionType { set { m_enMotionType = value; } get { return m_enMotionType; } }
            public double dblCommandPosition { set { m_dblCommandPosition = value; } get { return m_dblCommandPosition; } }
            public double dblActualPosition { set { m_dblActualPosition = value; } get { return m_dblActualPosition; } }
            public double dblDestination { set { m_dblDestination = value; } get { return m_dblDestination; } }
            #endregion

            #region 생성자
            public MotionItem(int nIndex)
            {
                m_nIndex = nIndex;

                // 초기값 설정
                configParam.enInMode = EN_MOTOR_INMODE.CWCCW;
                configParam.enOutMode = EN_MOTOR_OUTMODE.ABPHASE;
                configParam.bInputDirection = true;
                configParam.dblInOutRatio = 1.0;
                configParam.dblUnitDistance = 1.0;

                configParam.bAlarmLogic = false;
                configParam.bAlarmStopMode = false;
                configParam.bEZLogic = false;
                configParam.bUseInposition = false;
                configParam.bInpositionLogic = false;
                configParam.enMotorApplicationType = EN_MOTOR_APPLICATION.LINEAR;
                configParam.dblNumeratorForScale = 1;
                configParam.dblDenominatorForScale = 1;

                #region Limit
                configParam.bUseHWLimitPositive = false;
                configParam.bUseHWLimitNegative = false;
                configParam.bHWLimitLogicPositive = false;
                configParam.bHWLimitLogicNegative = false;
                configParam.bHWLimitStopModePositive = false;
                configParam.bHWLimitStopModeNegative = false;

                configParam.bUseSWLimitPositive = false;
                configParam.bUseSWLimitNegative = false;
                configParam.bSWLimitStopModePositive = false;
                configParam.bSWLimitStopModeNegative = false;
                configParam.dblSWLimitPositionPositive = 0;
                configParam.dblSWLimitPositionNegative = 0;
                #endregion

                speedParam = new MotionInterfaces.MOTOR_SPEED_PARAMETER[(int)EN_MOTOR_CONTENTOFSPEED.MAXCOUNT];

                homeParam.dblEscDist = 1.0;
                homeParam.dblHomeAccelTime = 0.0;
                homeParam.dblHomeDecelTime = 0.0;
                homeParam.dblHomeOffset = 0.0;
                homeParam.dblHomeVelocity = 0.0;
                homeParam.dblHomeSecondVelocity = 0.0;
                homeParam.enHomeSpeedPattern = EN_SPEED_PATTERN.CONSTANT;
                homeParam.strHomeLogic = MotionInterfaces.LOGIC_ACTIVE_HIGH;
                homeParam.strHomeDirection = MotionInterfaces.DIRECTION_POSITIVE;
                homeParam.nEZCount = 0;

                // 2020.03.05 by Thienvv [MOD] Merge from MK-D100 Minami
                //homeParam.enHomeMode        = EN_MOTOR_HOMEMODE.ORG_ONESTEP;
                homeParam.enHomeMode = EN_MOTOR_HOMEMODE.H10_PLUS_DIR_EL_BACK_ORG_Z_N_EDGE;

                homeParam.bAlwaysHomeEnd = false;
                homeParam.dblHomeBasePosition = 0.0;

                gantryParam.bMaster = false;
                gantryParam.bSlave = false;
                gantryParam.bSlaveInverse = false;
                gantryParam.nIndexOfSlave = -1;
                gantryParam.nIndexOfMaster = -1;

                //
            }
            #endregion
        }
        /// <summary>
        /// 2018.11.19 by yjlee [MOD] 네트워크 타입(Ethercat) 관련 추상 클래스 리팩토링
        /// 2019.04.27 by yjlee [MOD] 인터페이스 변경
        /// </summary>
        abstract class MotionController 
        {
            #region 초기화 관련 변수
            protected bool m_bInit = false;
            public bool bInit { set { m_bInit = value; } get { return m_bInit; } }
            #endregion

            #region 설정 초기화 함수
            abstract public int InitController();
            abstract public void ExitController();
            #endregion

            #region 모터 설정 관련 함수
            abstract public void SetMotorType(int nAxis, EN_MOTOR_TYPE enType);
            abstract public void SetMotorConfiguration(int nAxis, MotionInterfaces.MOTOR_CONFIG_PARAMETER motorParam);
            abstract public void SetMotorSpeedConfiguration(int nAxis, MotionInterfaces.MOTOR_SPEED_PARAMETER motorParam);
            abstract public void SetMotorHomeConfiguration(int nAxis, MotionInterfaces.MOTOR_HOME_PARAMETER motorParam);
            //2020.10.07 nogami modify abstract public void SetMotorHomeSpeedConfiguration(int nAxis, MOTOR_HOME_PARAMETER motorParam);
            abstract public void SetMotorHomeSpeedConfiguration(int nAxis, MotionInterfaces.MOTOR_HOME_PARAMETER motorParam, MotionInterfaces.MOTOR_SPEED_PARAMETER stSpeedParam);

            // 2020.03.05 by Thienvv [MOD] Merge from MK-D100 Minami
            abstract public void SetMotorOutputMode(int nAxis, EN_MOTOR_OUTMODE outMode);
            abstract public void SetMotorInputMode(int nAxis, EN_MOTOR_INMODE inMode, bool bReverse);
            #endregion

            #region 모터 상태 관련 함수
            abstract public void GetState(int nAxis, bool[] arState);
            abstract public double GetCommandPosition(int nAxis);
            abstract public double GetActualPosition(int nAxis);
            abstract public double GetTouchEncoderPosition(int nAxis);
            abstract public void SetCommandPosition(int nAxis, double dblPosition);
            abstract public void SetActualPosition(int nAxis, double dblPosition);
            abstract public void AdjustCommandPositionByActualPosition(int nAxis);
            abstract public bool IsMotionDone(int nAxis);
            abstract public bool isHomeEnd(int nAxis);
            abstract public bool isMotorOn(int nAxis);
            abstract public bool DoMotorOn(int nAxis);



            abstract public bool DoMotorOff(int nAxis);
            abstract public void DoReset(int nAxis);
            abstract public void ClearPosition(int nAxis);
            #endregion

            #region 모션 이동 관련
            abstract public void MoveSxAbsolutely(int nAxis, double dblDest, bool bWaitForDone = false);
            abstract public void MoveSxRelatively(int nAxis, double dblDest, bool bWaitForDone = false);
            abstract public void MoveExternalPositioning(int nAxis, double dblDest, bool bWaitForDone = false);     //2020.09.30 nogami add
            abstract public void MoveLinearAbsolutely(int[] axisTarget, double[] dAbsolutePos, bool bWaitForDone = false);  //線形補間 2020.10.18 nogami add
            abstract public void MoveAtSpeed(int nAxis, bool bDirection);

            // 2019.07.04. by shkim [ADD] PLP-300 에서 사용중임으로 주의! (MEI 사용설비)
            abstract public void MoveCompare(int nAxis, double dPosition, int nCompareAxis, double dComparePosition, bool bLogic, bool bActual = true, bool bUseCount = false);
            abstract public bool MoveToHome(int nAxis, ref int nSeqNum, ref uint uDelay);
            abstract public void StopMotion(int nAxis, bool bEmergency);
            //2020.12.17 nogami add start
            abstract public void PauseMotion(int nAxis);
            abstract public void ResumeMotion(int nAxis);
            //2020.12.17 nogami add end
            abstract public void StopHomeMotion(int nAxis, bool bEmergency);
            abstract public void OverrideSxSpeed(int nAxis, EN_MOTION_TYPE enTypeOfMotion, MotionInterfaces.MOTOR_SPEED_PARAMETER motorParam);
            abstract public void MoveByList(int nAxis, int nCount, double[] dDestPosition, MotionInterfaces.MOTOR_SPEED_PARAMETER[] arSpeedParameters);
            abstract public void MoveByLinearCoordination(int nCountOfAxises, int[] arIndexes, int nCountOfStep, double[,] arDestination, MotionInterfaces.MOTOR_SPEED_PARAMETER[] arSpeedParameters);
            #endregion

            #region GANTRY 관련 함수
            public abstract bool RegisterGantry(int nAxisOfMaster, MotionInterfaces.MOTOR_SPEED_PARAMETER spdParam, int nAxisOfSlave, bool bInverse);
            public abstract void UnRegisterGantry(int nAxisOfMaster, int nAxisOfSlave);
            public abstract bool GetGantryState(int nAxisOfMaster, int nAxisOfSlave);
            #endregion

            #region COMPENSATION 관련 함수
            public abstract void SetCompensation(int nIndex, CompensationItem item);
            public abstract void ApplyCompensation();
            #endregion

            #region 네트워크 타입 관련
            /// <summary>
            /// 2018.12.15 by yjlee [ADD] 링크 연결을 시도한다.
            /// </summary>
            public abstract bool ConnectLink();
            /// <summary>
            /// 2018.11.19 by yjlee [ADD] 링크가 정상 연결된 상태인지 확인한다.
            /// </summary>
            public abstract bool IsLinkConnected();
            #endregion

            #region <BallAttach>
            //ヘリカル補間
            public abstract void YmcCallMPM001(double MaxVelocity, double Velocity, int Acceleration, int Deceleration,
                double Xrest, double Yrest, double Xcenter, double Ycenter, double Radius, double Pitch, int Repeat, EN_BALL_ATTACH_MOTION Dir);
            #endregion </BallAttach>

            //Select servo parameter 2021.01.03 nogami add
            public abstract bool SelectServoParameter(int nIndex, int nSelect);
            public abstract int GetSelectServoParameterNum(int nIndex);
        }

        #region Compensation
        public class CompensationItem
        {
            #region 변수
            private int m_nDimension;

            private int[] m_arrInputAxis;
            private int m_nOutputAxis;
            private int m_nCount;

            private double[] m_arrStartPosition;
            private double[] m_arrEndPosition;
            private double[] m_arrDistance;
            private double[] m_CompensationTable;
            #endregion

            #region 속성
            public int nDimension { set { m_nDimension = value; } get { return m_nDimension; } }
            public int nOutputAxis { set { m_nOutputAxis = value; } get { return m_nOutputAxis; } }
            public int nCount { set { m_nCount = value; } get { return m_nCount; } }
            #endregion

            #region 생성
            public CompensationItem(int nDimension)
            {
                m_nDimension = nDimension;
                m_arrInputAxis = new int[nDimension];
                m_arrStartPosition = new double[nDimension];
                m_arrEndPosition = new double[nDimension];
                m_arrDistance = new double[nDimension];
                m_CompensationTable = null;
            }

            public void ClearTable()
            {
                m_CompensationTable = null;
            }

            public bool Create(int nCount)
            {
                if (nDimension < 1) return false;

                m_CompensationTable = new double[nCount];
                return true;
            }

            #endregion

            #region 외부 인터페이스
            public int GetInputAxis(int nIndex)
            {
                return m_arrInputAxis[nIndex];
            }
            public int GetOutputAxis()
            {
                return m_nOutputAxis;
            }
            public double GetStartPosition(int nIndex)
            {
                return m_arrStartPosition[nIndex];
            }
            public double GetEndPosition(int nIndex)
            {
                return m_arrEndPosition[nIndex];
            }
            public double GetDistance(int nIndex)
            {
                return m_arrDistance[nIndex];
            }
            public void GetInputAxis(int[] arrAxis)
            {
                int i;
                for (i = 0; i < m_nDimension; ++ i)
                {
                    arrAxis[i] = m_arrInputAxis[i];
                }
            }
            public void GetStartPosition(double[] arrStartPosition)
            {
                int i;
                for (i = 0; i < m_nDimension; ++ i)
                {
                    arrStartPosition[i] = m_arrStartPosition[i];
                }
            }
            public void GetEndPosition(double[] arrEndPosition)
            {
                int i;
                for (i = 0; i < m_nDimension; ++ i)
                {
                    arrEndPosition[i] = m_arrEndPosition[i];
                }
            }
            public void GetDistance(double[] arrDistance)
            {
                int i;
                for (i = 0; i < m_nDimension; ++ i)
                {
                    arrDistance[i] = m_arrDistance[i];
                }
            }
            public void GetCompensationTable(double[] arrTable)
            {
                int i;
                for (i = 0; i < m_nCount; ++ i)
                {
                    arrTable[i] = m_CompensationTable[i];
                }
            }

            public void SetInputAxis(int nIndex, int nAxis)
            {
                m_arrInputAxis[nIndex] = nAxis;
            }
            public void SetInputAxis(int[] arrAxis)
            {
                int i;
                m_arrInputAxis = new int[m_nDimension];
                for (i = 0; i < m_nDimension; ++ i)
                {
                    m_arrInputAxis[i] = arrAxis[i];
                }
            }
            public void SetStartPosition(int nIndex, double dPosition)
            {
                m_arrStartPosition[nIndex] = dPosition;
            }
            public void SetStartPosition(double[] arrPosition)
            {
                int i;
                for (i = 0; i < m_nDimension; ++ i)
                {
                    m_arrStartPosition[i] = arrPosition[i];
                }
            }
            public void SetEndPosition(int nIndex, double dPosition)
            {
                m_arrEndPosition[nIndex] = dPosition;
            }
            public void SetEndPosition(double[] arrPosition)
            {
                int i;
                for (i = 0; i < m_nDimension; ++ i)
                {
                    m_arrEndPosition[i] = arrPosition[i];
                }
            }
            public void SetDistance(int nIndex, double dDistance)
            {
                m_arrDistance[nIndex] = dDistance;
            }
            public void SetDistance(double[] arrDistance)
            {
                int i;
                for (i = 0; i < m_nDimension; ++ i)
                {
                    m_arrDistance[i] = arrDistance[i];
                }
            }
            public void SetCompensationTable(int nIndex, double[] arrTable)
            {
                int i;
                for (i = 0; i < nCount; ++ i)
                {
                    m_CompensationTable[i] = arrTable[i];
                }
            }
            #endregion
        }

        public class Compensator
        {
            #region 변수
            private bool m_bUseCompensator;
            private Dictionary<int, CompensationItem> m_dicItem = new Dictionary<int, CompensationItem>();
            #endregion

            #region 싱글톤
            private Compensator() { }
            private static Compensator _Instance = null;
            public static Compensator GetInstance()
            {
                if (_Instance == null)
                {
                    _Instance = new Compensator();
                }
                return _Instance;
            }
            #endregion

            #region 파일 입출력
            public bool Save()
            {

                //FileComposite fComp = new FileComposite("Compensator");

                //fComp.SetFileVersioin("1.0.0.0");

                //int i, j, nTemp;
                //double dTemp;
                //double[] arrTemp;
                //FileGroup fGroup = fComp.Root.Add("OPTION");
                //fGroup.Add("USE_COMPENSATOR", m_bUseCompensator.ToString());

                //fGroup = fComp.Root.Add("ITEMS");
                //foreach (KeyValuePair<int, CompensationItem> kvp in m_dicItem)
                //{
                //    FileGroup fGroupMiddle = fGroup.Add(kvp.Key.ToString());

                //    CompensationItem comp = kvp.Value;

                //    fGroupMiddle.Add("DIMENSION", comp.nDimension);
                //    fGroupMiddle.Add("OUTPUT_AXIS", comp.nOutputAxis);
                //    fGroupMiddle.Add("TABLE_COUNT", comp.nCount);

                //    for (i = 0; i < comp.nDimension; ++ i)
                //    {
                //        FileGroup fGroupLast = fGroupMiddle.Add(String.Format("AXIS_{0}", i));
                //        {
                //            nTemp = comp.GetInputAxis(i);
                //            fGroupLast.Add("INPUT_AXIS", nTemp);
                //            dTemp = comp.GetStartPosition(i);
                //            fGroupLast.Add("START_POS", dTemp);
                //            dTemp = comp.GetEndPosition(i);
                //            fGroupLast.Add("END_POS", dTemp);
                //            dTemp = comp.GetDistance(i);
                //            fGroupLast.Add("DISTANCE", dTemp);
                //        }
                //    }

                //    fGroupMiddle = fGroupMiddle.Add("COMPENSATION_TABLE");
                //    {
                //        arrTemp = new double[comp.nCount];
                //        comp.GetCompensationTable(arrTemp);
                //        for (j = 0; j < comp.nCount; ++ j)
                //        {
                //            fGroupMiddle.Add(String.Format("TABLE_{0}", j), arrTemp[j]);
                //        }
                //    }
                //}

                //fGroup = fGroup.fgParent;

                //return fComp.Save(FILEPATH_CONFIG, "Compensator", FILEFORMAT_CONFIG);

                return true;

            }
            public bool Load()
            {

                //bool bExist;
                //int i, nTemp;
                //double dTemp;
                //double[] arrTemp;
                //CompensationItem comp = null;

                //FileComposite fComp = new FileComposite("Compensator");
                //fComp.SetFileVersioin("1.0.0.0");

                //if (!fComp.Load(FILEPATH_CONFIG, "Compensator", FILEFORMAT_CONFIG, Save))
                //{
                //    return false;
                //}

                //FileGroup fRootGroup = fComp.Root.FindGroup("OPTION");
                //if(fRootGroup == null)
                //{
                //    return false;
                //}
                //m_bUseCompensator = fRootGroup.GetBoolValue("USE_COMPENSATOR", false);

                //fRootGroup = fComp.Root.FindGroup("ITEMS");
                //foreach (FileGroup fGroup in fRootGroup)
                //{
                //    int nIndex = -1;
                //    if (false == int.TryParse(fGroup.strName, out nIndex)) { continue; }
                //    if (nIndex < 0) continue;

                //    bExist = false;
                //    // 초기화 시 Dimension이 필요하기 때문에 먼저 읽는다.
                //    nTemp = fGroup.GetIntegerValue("DIMENSION", 2);
                //    if (!m_dicItem.ContainsKey(nIndex))
                //    {
                //        comp = new CompensationItem(nTemp);
                //        comp.ClearTable();
                //    }
                //    else
                //    {
                //        bExist = true;
                //        comp = m_dicItem[nIndex];
                //    }

                //    comp.nDimension = nTemp;
                //    comp.nOutputAxis = fGroup.GetIntegerValue("OUTPUT_AXIS", 0);
                //    comp.nCount = fGroup.GetIntegerValue("TABLE_COUNT", 10);
                //    for (i = 0; i < comp.nDimension; ++ i)
                //    {
                //        FileGroup fg = fGroup.FindGroup(String.Format("AXIS_{0}", i));

                //        nTemp = fg.GetIntegerValue("INPUT_AXIS", 0);
                //        comp.SetInputAxis(i, nTemp);

                //        dTemp = fg.GetDoubleValue("START_POS", 0);
                //        comp.SetStartPosition(i, dTemp);

                //        dTemp = fg.GetDoubleValue("END_POS", 0);
                //        comp.SetEndPosition(i, dTemp);

                //        dTemp = fg.GetDoubleValue("DISTANCE", 0);
                //        comp.SetDistance(i, dTemp);
                //    }

                //    FileGroup fGroupLast = fGroup.FindGroup("COMPENSATION_TABLE");
                //    comp.Create(comp.nCount);
                //    arrTemp = new double[comp.nCount];
                //    for (i = 0; i < nTemp; ++ i)
                //    {
                //        arrTemp[i] = fGroupLast.GetDoubleValue(String.Format("TABLE_{0}", i), 0);
                //    }
                //    comp.SetCompensationTable(i, arrTemp);

                //    if (!bExist) m_dicItem.Add(nIndex, comp);
                //}
                return true;
            }
            #endregion

            #region 외부 인터페이스
            public void AddItem(CompensationItem item, ref int nIndex)
            {
                nIndex = m_dicItem.Count;
                m_dicItem.Add(m_dicItem.Count, item);
            }
            public CompensationItem GetItem(int nIndex)
            {
                return m_dicItem[nIndex];
            }
            #endregion
        }
        #endregion
    }

    namespace RegisteredInstanceOfMotion
    {
        /// <summary>
        /// 2019.02.05 by yjlee [ADD] 등록 모션의 모체이다.
        /// </summary>
        public abstract class BaseMotion
        {
            #region 변수
            //protected Motion m_InstanceMotion     = null;
            protected MotionInterfaces m_InstanceMotion = null;
            protected int m_nKey                        = -1;
            protected int m_nIndexOfMotionItem          = -1;
            protected string m_strTaskName              = null;
            protected bool m_bSkipped                   = false;
            protected bool m_bCheckingMotion            = true;
            protected bool m_bEnable
            {
                get
                {
                    if (m_InstanceMotion == null) { return false; }

                    return m_InstanceMotion.GetEnable(m_nIndexOfMotionItem);
                }
            }

            private bool[] m_arState            = null;
            #endregion

            #region 속성
            public int nKey { get { return m_nKey; } }
            public int nIndexOfMotionItem { get { return m_nIndexOfMotionItem; } }
            public string strTaskName { get { return m_strTaskName; } }
            public string strItemName { get { return m_InstanceMotion.GetName(m_nIndexOfMotionItem); } }
            public bool bSkipped
            {
                set { m_bSkipped = value; }
            }
            #endregion

            #region 생성자
            protected BaseMotion(MotionInterfaces Instance, string strTaskName, int nKey, int nIndex)
            {
                m_InstanceMotion        = Instance;
                m_nKey                  = nKey * ((int)AlarmInstance::Alarm_.ALARM_CODE_SECTION.OBJECT);  // ALARM_OBJECTCODE;   // 2021.07.13
                m_nIndexOfMotionItem    = nIndex;
                m_strTaskName           = strTaskName;
            }
            #endregion

            #region 모션 상태 관련

            #region 상태 변경
            /// <summary>
            /// 2018.08.18 by yjlee [MOD] 해당 축에 전원을 인가한다.
            /// 2019.02.04 by yjlee [MOD] 서보 전원 인가 시 포지션 일치 여부 추가
            /// </summary>
            public void DoServoOn(bool bPositionAdjustment = true)
            {
                if (IsSkipped()) { return; }

                m_InstanceMotion.DoMotorOn(m_nIndexOfMotionItem
                    , bPositionAdjustment);
            }
            /// <summary>
            /// 2018.08.18 by yjlee [MOD] 해당 축에 전원을 해제한다.
            /// 2019.02.04 by yjlee [MOD] 서보 전원 해제 시 포지션 일치 여부 추가
            /// </summary>
            public void DoServoOff(bool bPositionAdjustment = false)
            {
                if (IsSkipped()) { return; }

                m_InstanceMotion.DoMotorOff(m_nIndexOfMotionItem
                    , bPositionAdjustment);
            }
            /// <summary>
            /// 2018.08.18 by yjlee [MOD] 해당 축에 알람을 해제한다.
            /// </summary>
            public void DoReset()
            {
                if (IsSkipped()) { return; }

                m_InstanceMotion.DoReset(m_nIndexOfMotionItem);
            }
            //2020.09.27 nogami add start
            public void HomeEndReset()
            {
                if (IsSkipped()) { return; }
                m_InstanceMotion.HomeEndReset(m_nIndexOfMotionItem);
            }
            //2020.09.27 nogami add end
            /// <summary>
            /// 2018.08.18 by yjlee [MOD] 현재 위치를 0으로 초기화한다.
            /// </summary>
            public void ClearPosition()
            {
                if (IsSkipped()) { return; }

                m_InstanceMotion.ClearPosition(m_nIndexOfMotionItem);
            }
            /// <summary>
            /// 2018.08.18 by yjlee [MOD] Home End 상태를 설정한다.
            /// </summary>
            public void SetHomeEnd(bool bEnable)
            {
                if (IsSkipped()) { return; }

                m_InstanceMotion.SetHomeEnd(m_nIndexOfMotionItem, bEnable);
            }
            /// <summary>
            /// 2018.08.18 by yjlee [MOD] 엔코더 및 커맨드 포지션 값을 설정한다.
            /// </summary>
            public void SetPosition(double dblPosition)
            {
                if (IsSkipped()) { return; }

                m_InstanceMotion.SetActualPosition(m_nIndexOfMotionItem, dblPosition);
                m_InstanceMotion.SetCommandPosition(m_nIndexOfMotionItem, dblPosition);
            }
            /// <summary>
            /// 2018.08.18 by yjlee [MOD] 엔코더 포지션 값을 설정한다.
            /// </summary>
            public void SetActualPosition(double dblPosition)
            {
                if (IsSkipped()) { return; }

                m_InstanceMotion.SetActualPosition(m_nIndexOfMotionItem, dblPosition);
            }
            /// <summary>
            /// 2018.08.18 by yjlee [MOD] 명령 포지션 값을 설정한다.
            /// </summary>
            public void SetCommandPosition(double dblPosition)
            {
                if (IsSkipped()) { return; }

                m_InstanceMotion.SetCommandPosition(m_nIndexOfMotionItem, dblPosition);
            }
            #endregion

            #region 상태 반환
            /// <summary>
            /// 2018.08.30 by yjlee [ADD] 현재 모션 아이템이 이송 중인지 판단한다.
            /// </summary>
            /// <returns></returns>
            public bool IsMotionMoving()
            {
                if (IsSkipped() || false == m_bCheckingMotion) { return false; }
            
                return m_InstanceMotion.IsMotionMoving(m_nIndexOfMotionItem);
            }
            //2020.10.05 nogami add start
            //IsMotionDone()ではEN_MOTION_STATUS.READYに変わっているのか分からない
            public bool IsReady()
            {
                if (IsSkipped()) { return false; }

                return (m_InstanceMotion.IsMotionMoving(m_nIndexOfMotionItem)?(false):(true));
            }
            //2020.10.05 nogami add end
            public bool IsMotionJogMoving()
            {
                if (IsSkipped() || false == m_bCheckingMotion) { return false; }

                return m_InstanceMotion.IsMotionJogMoving(m_nIndexOfMotionItem);
            }
            /// <summary>
            /// 2018.09.16 by yjlee [ADD] 포지션 값을 비교한다.
            /// </summary>
            public bool ComparePosition(bool bActual, double dblPosition, bool bBelow = true)
            {
                if (IsSkipped()) { return true; }

                double dblRecentPosition = 0.0;

                // 엔코더 포지션 비교일 경우
                if(bActual)
                {
                    dblRecentPosition   = m_InstanceMotion.GetActualPosition(m_nIndexOfMotionItem);
                }
                else
                {
                    dblRecentPosition   = m_InstanceMotion.GetCommandPosition(m_nIndexOfMotionItem);
                }

                // 현재 포지션보다 낮은지 확인할 경우
                if (bBelow
                    && dblRecentPosition < dblPosition)
                {
                    return true;
                }
                else if (false == bBelow
                    && dblRecentPosition > dblPosition)
                {
                    return true;
                }

                return false;
            }
            /// <summary>
            /// 2019.04.18 by yjlee [ADD] 포지션이 특정 범위 안에 존재하는지 확인한다.
            /// ex) dblPosition = 10, dblRange = 0.5 일경우 9.5 ~ 10.5 안에 존재하는지 확인
            /// </summary>
            public int ComparePosition(bool bActual, double dblPosition, double dblRange)
            {
                if (IsSkipped()) { return 0; }

                double dblRecentPosition = 0.0;

                // 엔코더 포지션 비교일 경우
                if(bActual)
                {
                    dblRecentPosition   = m_InstanceMotion.GetActualPosition(m_nIndexOfMotionItem);
                }
                else
                {
                    dblRecentPosition   = m_InstanceMotion.GetCommandPosition(m_nIndexOfMotionItem);
                }

                // 최소 범위를 벗어났을 경우
                if (dblRecentPosition < dblPosition - dblRange) { return -1; }
                // 최대 범위를 벗어났을 경우
                if (dblRecentPosition > dblPosition + dblRange) { return 1; }

                return 0;
            }
            /// <summary>
            /// 2018.08.18 by yjlee [MOD] 모션 완료 상태를 점검한다.
            /// 2019.10.14 by Thienvv [MOD] Add flag to Set MotionChecking.
            /// </summary>
            public bool IsMotionDone(bool bSetMotionChecking = true)
            {
                if (IsSkipped()) { return true; }

                if (bSetMotionChecking)
                    return SetMotionChecking(m_InstanceMotion.IsMotionDone(m_nIndexOfMotionItem));
                else
                    return m_InstanceMotion.IsMotionDone(m_nIndexOfMotionItem);
            }
            /// <summary>
            /// 2018.08.18 by yjlee [MOD] 홈 완료 상태를 점검한다.
            /// </summary>
            public bool IsHomeEnd()
            {
                if (IsSkipped()) { return true; }

                return m_InstanceMotion.IsHomeEnd(m_nIndexOfMotionItem);
            }
            /// <summary>
            /// 2018.08.28 by yjlee [ADD] 모터가 온 상태인지 확인한다.
            /// </summary>
            public bool IsMotorOn()
            {
                if (IsSkipped() || false == m_bCheckingMotion) { return true; }

                return m_InstanceMotion.IsMotorOn(m_nIndexOfMotionItem);
            }
            /// <summary>
            /// 2019.10.17. by shkim. [ADD] Servo On, Off 후 실제 State를 정상적으로 리턴 받는데까지 소요되는 시간이 있음으로,
            /// 따로 Refresh 하는 함수가 필요하다.
            /// </summary>
            public void DoRefreshMotionState()
            {
                m_InstanceMotion.DoRefreshMotionState(m_nIndexOfMotionItem);
            }

            #endregion

            #endregion

            public bool IsTimeout()
            {
                return (m_InstanceMotion.IsMotionTimeoutOver(m_nIndexOfMotionItem));
            }
            #region 모션 상태 확인
            /// <summary>
            /// 2019.02.06 by yjlee [ADD] 모션이 스킵 되어야하는 상태인지 판단한다.
            /// </summary>
            public abstract bool IsSkipped();
            /// <summary>
            /// 2018.08.18 by yjlee [MOD] 현재 엔코더 포지션을 읽어온다.
            /// </summary>
            public double GetActualPosition()
            {
                return m_InstanceMotion.GetActualPosition(m_nIndexOfMotionItem);
            }
            /// <summary>
            /// 2018.08.18 by yjlee [MOD] 현재 명령 포지션을 읽어온다.
            /// </summary>
            public double GetCommandPosition()
            {
                return m_InstanceMotion.GetCommandPosition(m_nIndexOfMotionItem);
            }
            /// <summary>
            /// 2019.06.25 by yjlee [ADD] 해당 축의 터치 엔코더 값을 읽어온다.
            /// </summary>
            public double GetTouchEncoderPosition()
            {
                return m_InstanceMotion.GetTouchEncoderPosition(m_nIndexOfMotionItem);
            }
            /// <summary>
            /// 2019.04.29 by yjlee [ADD] 모션 상태를 반환한다.
            /// </summary>
            public bool UpdateMotionState()
            {
                // 잘못된 타겟번호일 경우 읽어올 수 없다.
                if(false == m_InstanceMotion.GetMotionState(m_nIndexOfMotionItem
                    , ref m_arState))
                {
                    return false;
                }

                return true;
            }
            /// <summary>
            /// 2019.04.29 by yjlee [ADD] 현재 모션에 걸린 알람을 상태를 반환한다.
            /// </summary>
            public bool GetMotionAlarmState(ref EN_MOTOR_STATE enAlarmState)
            {
                int nCountOfError   = (int)EN_MOTOR_STATE.MAXCOUNT;

                for(int nState = (int)EN_MOTOR_STATE.ALARM ; nState < nCountOfError ; ++ nState)
                {
                    //2020.03.10 nogami add start
                    switch((EN_MOTOR_STATE)nState)
                    {
                        case EN_MOTOR_STATE.LIMITNEGATIVE:
                        case EN_MOTOR_STATE.LIMITPOSITIVE:
                            if(m_arState[(int)EN_MOTOR_STATE.HOMEEND]==false)
                            {
                                continue;
                            }
                            break;
                        case MotionOnly.EN_MOTOR_STATE.EXT_IO_1:
                        case MotionOnly.EN_MOTOR_STATE.EXT_IO_2:
                        case MotionOnly.EN_MOTOR_STATE.EXT_IO_3:
                            continue;
                    }
                    //2020.03.10 nogami add end
                    if(m_arState[nState])
                    {
                        enAlarmState    = (EN_MOTOR_STATE)nState;

                        return true;
                    }
                }

                return false;
            }
            //2020.03.09 nogami add start
            public void ResetAlarmState(EN_MOTOR_STATE nState)
            {
                m_arState[(int)nState] = false;
            }
            //2020.03.09 nogami add end
            #endregion

            #region 내부 인터페이스
            /// <summary>
            /// 2018.10.02 by yjlee [ADD] 모션 체크 상태를 설정한다.
            /// </summary>
            protected bool SetMotionChecking(bool bValue)
            {
                if (false == m_bCheckingMotion)
                {
                    if (bValue)
                    {
                        m_bCheckingMotion = true;
                    }
                }

                return bValue;
            }
            /// <summary>
            /// 2019.06.04 by yjlee [ADD] 모션 체크 사용 유무를 설정한다.
            /// </summary>
            protected void ActivateCheckingMotion(bool bActive)
            {
                m_bCheckingMotion       = bActive;
            }
            #endregion
        }
        /// <summary>
        /// 2019.02.05 by yjlee [MOD] 태스크에서 등록해서 사용하기 위한 모션 클래스
        /// </summary>
        public class RegisteredMotion : BaseMotion
        {
            #region 생성자 및 소멸자
            public RegisteredMotion(MotionInterfaces Instance, string strTaskName, int nKey, int nIndex)
                : base(Instance, strTaskName, nKey, nIndex)
            {
            }
            #endregion

            #region 모션 이송 관련
            /// <summary>
            /// 2018.08.18 by yjlee [MOD] 해당 축을 원점을 이동시킨다.
            /// </summary>
            public void MoveToHome(bool bCheckingMotion = true)
            {
                if (IsSkipped()) { return; }

                m_bCheckingMotion   = bCheckingMotion;

                m_InstanceMotion.MoveToHome(m_nIndexOfMotionItem, m_strTaskName);
            }
            // 해당 축을 목표 지점으로 절대값 좌표 기준으로 움직인다.
            // 축, 지점, 속도비(%), 가속도비(%), 감속도비(%), 모션 완료 대기 사용 유무
            // false : 모션 동작 후 즉시 반환, true : 모션 완료 후 반환
            public void MoveSxAbsolutely(double dblDestination, EN_MOTOR_CONTENTOFSPEED spdContent = EN_MOTOR_CONTENTOFSPEED.RUN, uint uMotionDelay = 0, double dblVelocity = 0, uint uRatio = 100, bool bCheckingMotion = true)
            {
                if (IsSkipped()) { return; }

                m_bCheckingMotion   = bCheckingMotion;

                m_InstanceMotion.MoveSxAbsolutely(m_nIndexOfMotionItem, dblDestination, spdContent, dblVelocity, uRatio, uMotionDelay, m_strTaskName);
            }
            // 해당 축을 목표 지점으로 상대값 좌표 기준으로 움직인다.
            // 축, 지점, 속도비(%), 가속도비(%), 감속도비(%), 모션 완료 대기 사용 유무
            // false : 모션 동작 후 즉시 반환, true : 모션 완료 후 반환
            public void MoveSxRelatively(double dblDestination, EN_MOTOR_CONTENTOFSPEED spdContent = EN_MOTOR_CONTENTOFSPEED.RUN, uint uMotionDelay = 0, double dblVelocity = 0, uint uRatio = 100, bool bCheckingMotion = true)
            {
                if (IsSkipped()) { return; }

                m_bCheckingMotion   = bCheckingMotion;

                m_InstanceMotion.MoveSxRelatively(m_nIndexOfMotionItem, dblDestination, spdContent, dblVelocity, uRatio, uMotionDelay, m_strTaskName);
            }
            //2020.09.30 nogami add
            //EXT_1 sensor search
            public void MoveExternalPositioning(double dblDestination, EN_MOTOR_CONTENTOFSPEED spdContent = EN_MOTOR_CONTENTOFSPEED.RUN, uint uMotionDelay = 0, double dblVelocity = 0, uint uRatio = 100, bool bCheckingMotion = true)
            {
                if (IsSkipped()) { return; }

                m_bCheckingMotion = bCheckingMotion;

                m_InstanceMotion.MoveExternalPositioning(m_nIndexOfMotionItem, dblDestination, spdContent, dblVelocity, uRatio, uMotionDelay, m_strTaskName);
            }
            //線形補間 2020.10.18 nogami add
            public void MoveLinearAbsolutely(RegisteredMotion[] arRegisteredMotion, double[] dAbsolutePos, EN_MOTOR_CONTENTOFSPEED spdContent = EN_MOTOR_CONTENTOFSPEED.RUN, uint uMotionDelay = 0, double dblVelocity = 0, uint uRatio = 100, bool bCheckingMotion = true)
            {
                if (IsSkipped()) { return; }

                //m_bCheckingMotion = bCheckingMotion;
                int nCountOfAxis = arRegisteredMotion.Length;
                int[] arIndexes = new int[nCountOfAxis];

                for (int i = 0; i < nCountOfAxis; i++)
                {
                    arRegisteredMotion[i].ActivateCheckingMotion(bCheckingMotion);
                    arIndexes[i] = arRegisteredMotion[i].nIndexOfMotionItem;
                }

                m_InstanceMotion.MoveLinearAbsolutely(arIndexes, dAbsolutePos, spdContent, dblVelocity, uRatio, uMotionDelay, m_strTaskName);
            }
            public void MoveCircle(RegisteredMotion[] arRegisteredMotion, EN_MOTOR_CONTENTOFSPEED spdContent, double dblVelocity, double Xrest, double Yrest, double Xcenter, double Ycenter, double Radius, double Pitch, int Repeat, int Dir,
                uint uRatio, uint uDelay, uint uTimeout, bool bCheckingMotion = true)
            {
                if (IsSkipped()) { return; }

                //m_bCheckingMotion = bCheckingMotion;
                int nCountOfAxis = arRegisteredMotion.Length;
                int[] arIndexes = new int[nCountOfAxis];

                for (int i = 0; i < nCountOfAxis; i++)
                {
                    arRegisteredMotion[i].ActivateCheckingMotion(bCheckingMotion);
                    arIndexes[i] = arRegisteredMotion[i].nIndexOfMotionItem;
                }

                m_InstanceMotion.MoveCircle(arIndexes, spdContent, dblVelocity, Xrest, Yrest, Xcenter, Ycenter, Radius, Pitch, Repeat, Dir,
                    uRatio, uDelay, uTimeout, m_strTaskName);
            }            
            /// <summary>
            /// 2018.08.18 by yjlee [MOD] 조그(속도) 이동을 수행한다.
            /// </summary>
            public void MoveSxByJog(bool nDir, EN_SPEED_PATTERN spdPattern, double dblVelocity, double dblAccel, double dblDecel, double dblAccelJerk, double dblDecelJerk, uint uMotionTimeout = 0)
            {
                if (IsSkipped()) { return; }

                m_bCheckingMotion   = false;

                m_InstanceMotion.MoveSxAtSpeed(m_nIndexOfMotionItem, nDir, spdPattern, dblVelocity, dblAccel, dblDecel, dblAccelJerk, dblDecelJerk, uMotionTimeout, m_strTaskName);
            }
            /// <summary>
            /// 2018.08.18 by yjlee [MOD] 조그(속도) 이동을 수행한다.
            /// </summary>
            public void MoveSxByJog(bool nDir, uint uRatio = 100)
            {
                if (IsSkipped()) { return; }

                // 조그는 모션 체크를 하지 않는다.
                m_bCheckingMotion   = false;

                m_InstanceMotion.MoveSxAtSpeed(m_nIndexOfMotionItem, nDir, uRatio, m_strTaskName);
            }
            /// <summary>
            /// 2019.06.03 by yjlee [ADD] 모션을 2단계로 수행한다.
            /// uRationPosition 값이 70%일 경우, 총 이동거리의 70%는 arRatio[0]의 속도로,
            /// 나머지 30%는 arRatio[1]의 속도로 이동한다.
            /// </summary>
            public void MoveSxAbsolutelyAtTwoStep(double dblDestination, uint uRatioOfPosition, uint[] arRatioOfSpeed, EN_MOTOR_CONTENTOFSPEED spdContent = EN_MOTOR_CONTENTOFSPEED.RUN, uint uMotionDelay = 0, bool bCheckingMotion = true)
            {
                ///---
                /// 2021.07.13. by hkyu [comment] 추후 사용 유무 확인...

                //if (IsSkipped()) { return; }

                //m_bCheckingMotion       = bCheckingMotion;

                //double[] arPosition         = {dblDestination * uRatioOfPosition * 0.01, dblDestination};
                //double[] arManualVelocity   = {0,0};

                //EN_MOTOR_CONTENTOFSPEED[]  arContentOfSpeed  = {spdContent, spdContent};

                //m_InstanceMotion.MoveByList(m_nIndexOfMotionItem
                //    , 2
                //    , arPosition
                //    , arContentOfSpeed
                //    , arManualVelocity
                //    , arRatioOfSpeed
                //    , uMotionDelay
                //    , m_strTaskName);
            }
            /// <summary>
            /// 2018.08.18 by yjlee [MOD] 해당 축의 속도를 재정의 한다.
            /// </summary>
            public void OverrideSxSpeed(EN_MOTOR_CONTENTOFSPEED spdContent = EN_MOTOR_CONTENTOFSPEED.RUN, uint uRatio = 100)
            {
                if (IsSkipped()) { return; }

                m_InstanceMotion.OverrideSxSpeed(m_nIndexOfMotionItem
                    , spdContent
                    , uRatio
                    , m_strTaskName);
            }
            /// <summary>
            /// 2018.08.18 by yjlee [MOD] 해당 축의 속도를 재정의 한다.
            /// </summary>
            public void OverrideSxSpeed(EN_SPEED_PATTERN spdPattern, double dblVelocity, double dblAccel, double dblDecel)
            {
                if (IsSkipped()) { return; }

                m_InstanceMotion.OverrideSxSpeed(m_nIndexOfMotionItem
                    , spdPattern
                    , dblVelocity
                    , dblAccel
                    , dblDecel
                    , m_strTaskName);
            }
            // 해당 축을 목표 지점들의 절대값 좌표 기준으로 예약이송을 한다.
            // 리스트 갯수, 지점, 속도, 속도비(%), 가속도비(%), 감속도비(%), 모션 완료 대기 사용 유무
            // false : 모션 동작 후 즉시 반환, true : 모션 완료 후 반환
            public void MoveByList(int nLength, double[] dblDestination, EN_MOTOR_CONTENTOFSPEED[] spdContent, double[] dblVelocity, uint[] uRatio, uint uMotionDelay = 0, bool bCheckingMotion = true)
            {
                ///------------------
                /// 2021.07.13
                /// 
                //if (IsSkipped()) { return; }

                //m_bCheckingMotion = bCheckingMotion;

                //m_InstanceMotion.MoveByList(m_nIndexOfMotionItem, nLength, dblDestination, spdContent, dblVelocity, uRatio, uMotionDelay, m_strTaskName);
            }
            /// <summary>
            /// 2019.06.03 by yjlee [ADD] 모션 선형 보간을 수행한다.
            /// 모션 이동 시 X 축을 기준으로 보간을 수행하여야한다.
            /// </summary>
            public void MoveByLinearCoordination(RegisteredMotion[] arRegisteredMotion, int nCountOfStep, double[,] arDestination, EN_MOTOR_CONTENTOFSPEED[] arContent, double[] arManualVelocity, uint[] arRatio, uint uDelay, bool bCheckingMotion = true)
            {
                if (IsSkipped()) { return; }

                if(null == arRegisteredMotion
                    || arRegisteredMotion.Length < 1) { return; }

                m_bCheckingMotion   = bCheckingMotion;
                
                int nCountOfAxis   = arRegisteredMotion.Length + 1;
                int[] arIndexes     = new int[nCountOfAxis];

                arIndexes[0]        = m_nIndexOfMotionItem;

                for(int i = 1 ; i < nCountOfAxis ; ++ i)
                {
                    arRegisteredMotion[i-1].ActivateCheckingMotion(bCheckingMotion);
                    arIndexes[i]    = arRegisteredMotion[i-1].nIndexOfMotionItem;
                }

                m_InstanceMotion.MoveByLinearCoordination(nCountOfAxis
                    , arIndexes
                    , nCountOfStep
                    , arDestination
                    , arContent
                    , arManualVelocity
                    , arRatio
                    , uDelay
                    , m_strTaskName);
            }

            /// <summary>
            /// 2020.04.22 by Thienvv [ADD] Move To A Limit pos
            /// Using for MK-D12 because Camera Y need to move to Limit + For save movement
            /// </summary>
            public void MoveToLimitPos(EN_LIMIT_POS enLimitPos, bool bCheckingMotion = true)
            {
                if (IsSkipped()) { return; }

                m_bCheckingMotion = bCheckingMotion;

                m_InstanceMotion.MoveToHomeByLimitSensor(m_nIndexOfMotionItem, enLimitPos, m_strTaskName);
            }

            /// <summary>
            /// 2018.08.18 by yjlee [MOD] 해당 축의 움직임을 정지한다
            /// bEmergency == true, 긴급정지
            /// </summary>
            public void StopMotion(bool bEmergency = true)
            {
                if (IsSkipped()) { return; }
                
                m_bCheckingMotion   = true;

                m_InstanceMotion.StopMotion(m_nIndexOfMotionItem);
            }
            //2020.12.17 nogami add start
            public void PauseMotion()
            {
                if (IsSkipped()) { return; }

                m_InstanceMotion.PauseMotion(m_nIndexOfMotionItem);
            }
            public void ResumeMotion()
            {
                if (IsSkipped()) { return; }

                m_InstanceMotion.ResumeMotion(m_nIndexOfMotionItem);
            }
            //2020.12.17 nogami add end
            #endregion

            #region 모션 상태 관련

            #region 상태 반환
            /// <summary>
            /// 2019.02.06 by yjlee [ADD] 모션이 스킵 되어야하는 상태인지 판단한다.
            /// </summary>
            public override bool IsSkipped()
            {
                return m_bSkipped || m_bEnable == false;
            }

            /// <summary>
            /// 2019.09.30 by Thienvv [ADD] Check Limit Signal for Registered Motion
            /// </summary>
            public bool IsHWLimitNegative()
            {
                if (IsSkipped()) { return false; }

                return m_InstanceMotion.IsMotionHWLimitNegative(m_nIndexOfMotionItem);
            }
            /// <summary>
            /// 2019.09.30 by Thienvv [ADD] Check Limit Signal for Registered Motion
            /// </summary>
            public bool IsHWLimitPositive()
            {
                if (IsSkipped()) { return false; }

                return m_InstanceMotion.IsMotioHWLimitPositive(m_nIndexOfMotionItem);
            }

            /// <summary>
            /// 2019.09.30 by Thienvv [ADD] Check Limit Signal for Registered Motion
            /// </summary>
            public bool IsSWLimitNegative()
            {
                if (IsSkipped()) { return false; }

                return m_InstanceMotion.IsMotionSWLimitNegative(m_nIndexOfMotionItem);
            }
            /// <summary>
            /// 2019.09.30 by Thienvv [ADD] Check Limit Signal for Registered Motion
            /// </summary>
            public bool IsSWLimitPositive()
            {
                if (IsSkipped()) { return false; }

                return m_InstanceMotion.IsMotionSWLimitPositive(m_nIndexOfMotionItem);
            }
            #endregion

            #endregion
            //2020.01.03 nogami add
            public bool SelectServoParameter(int nSelect)
            {
                if (IsSkipped()) { return false; }

                return m_InstanceMotion.SelectServoParameter(m_nIndexOfMotionItem, nSelect);
            }
        }
        /// <summary>
        /// 2019.02.05 by yjlee [ADD] 태스크간 공유해서 사용되는 모션이다.
        /// </summary>
        //public class SharedMotion : BaseMotion, TokenOnly.Token
        //{
        //    #region 변수
        //    private int m_nIndexOfSharedList    = -1;   // 공유 리스트에서의 인덱스, 0 == 마스터
        //    private bool m_bLock                = true;
        //    #endregion

        //    #region 생성자
        //    public SharedMotion(Motion instance, string strTaskName, int nKey, int nIndex, int nIndexOfSharedList, bool bMaster)
        //        : base(instance, strTaskName, nKey, nIndex)
        //    {
        //        m_nIndexOfSharedList    = nIndexOfSharedList;
                
        //        // 마스터로 생성될 경우 언락한다.
        //        m_bLock = !bMaster;
        //    }
        //    #endregion

        //    #region 외부 인터페이스

        //    #region 공유 관리
        //    /// <summary>
        //    /// 2019.02.05 by yjlee [ADD] 공유 모션에 락을 설정한다.
        //    /// </summary>
        //    public void LockObject()
        //    {
        //        m_bLock = true;
        //    }
        //    /// <summary>
        //    /// 2019.02.05 by yjlee [ADD] 공유 모션에 락을 해제한다.
        //    /// </summary>
        //    public void UnlockObject()
        //    {
        //        m_bLock = false;
        //    }
        //    #endregion

        //    #region 모션 이송 관련
        //    /// <summary>
        //    /// 2018.08.18 by yjlee [MOD] 해당 축을 원점을 이동시킨다.
        //    /// </summary>
        //    public bool MoveToHome(bool bCheckingMotion = true)
        //    {
        //        if (CheckMoving()) { return true; }

        //        // 2021.07.13
        //        //// 토큰을 요청한다.
        //        //if(false == m_InstanceMotion.RequestToken(m_nIndexOfMotionItem
        //        //    , m_nIndexOfSharedList))
        //        //{
        //        //    return false;
        //        //}

        //        m_bCheckingMotion = bCheckingMotion;

        //        m_InstanceMotion.MoveToHome(m_nIndexOfMotionItem, m_strTaskName);

        //        return true;
        //    }
        //    // 해당 축을 목표 지점으로 절대값 좌표 기준으로 움직인다.
        //    // 축, 지점, 속도비(%), 가속도비(%), 감속도비(%), 모션 완료 대기 사용 유무
        //    // false : 모션 동작 후 즉시 반환, true : 모션 완료 후 반환
        //    public bool MoveSxAbsolutely(double dblDestination, EN_MOTOR_CONTENTOFSPEED spdContent = EN_MOTOR_CONTENTOFSPEED.RUN, uint uMotionDelay = 0, double dblVelocity = 0, uint uRatio = 100, bool bCheckingMotion = true)
        //    {
        //        if (CheckMoving()) { return true; }

        //        /// 2021.07.13
        //        /// 
        //        //// 토큰을 요청한다.
        //        //if (false == m_InstanceMotion.RequestToken(m_nIndexOfMotionItem
        //        //    , m_nIndexOfSharedList))
        //        //{
        //        //    return false;
        //        //}

        //        m_bCheckingMotion = bCheckingMotion;

        //        m_InstanceMotion.MoveSxAbsolutely(m_nIndexOfMotionItem, dblDestination, spdContent, dblVelocity, uRatio, uMotionDelay, m_strTaskName);

        //        return true;
        //    }
        //    // 해당 축을 목표 지점으로 상대값 좌표 기준으로 움직인다.
        //    // 축, 지점, 속도비(%), 가속도비(%), 감속도비(%), 모션 완료 대기 사용 유무
        //    // false : 모션 동작 후 즉시 반환, true : 모션 완료 후 반환
        //    public bool MoveSxRelatively(double dblDestination, EN_MOTOR_CONTENTOFSPEED spdContent = EN_MOTOR_CONTENTOFSPEED.RUN, uint uMotionDelay = 0, double dblVelocity = 0, uint uRatio = 100, bool bCheckingMotion = true)
        //    {
        //        if (CheckMoving()) { return true; }


        //        /// 2021.07.13
        //        /// 
        //        //// 토큰을 요청한다.
        //        //if (false == m_InstanceMotion.RequestToken(m_nIndexOfMotionItem
        //        //    , m_nIndexOfSharedList))
        //        //{
        //        //    return false;
        //        //}

        //        m_bCheckingMotion = bCheckingMotion;

        //        m_InstanceMotion.MoveSxRelatively(m_nIndexOfMotionItem, dblDestination, spdContent, dblVelocity, uRatio, uMotionDelay, m_strTaskName);

        //        return true;
        //    }
        //    //2020.09.30 nogami add
        //    //EXT_1 sensor search
        //    public bool MoveExternalPositioning(double dblDestination, EN_MOTOR_CONTENTOFSPEED spdContent = EN_MOTOR_CONTENTOFSPEED.RUN, uint uMotionDelay = 0, double dblVelocity = 0, uint uRatio = 100, bool bCheckingMotion = true)
        //    {
        //        if (CheckMoving()) { return true; }

        //        /// 2021.07.13
        //        /// 
        //        //// 토큰을 요청한다.
        //        //if (false == m_InstanceMotion.RequestToken(m_nIndexOfMotionItem
        //        //    , m_nIndexOfSharedList))
        //        //{
        //        //    return false;
        //        //}

        //        m_bCheckingMotion = bCheckingMotion;

        //        m_InstanceMotion.MoveExternalPositioning(m_nIndexOfMotionItem, dblDestination, spdContent, dblVelocity, uRatio, uMotionDelay, m_strTaskName);

        //        return true;
        //    }
        //    /// <summary>
        //    /// 2018.08.18 by yjlee [MOD] 조그(속도) 이동을 수행한다.
        //    /// </summary>
        //    public bool MoveSxByJog(bool nDir, EN_SPEED_PATTERN spdPattern, double dblVelocity, double dblAccel, double dblDecel, double dblAccelJerk, double dblDecelJerk, uint uMotionTimeout = 0)
        //    {
        //        if (CheckMoving()) { return true; }

        //        //// 토큰을 요청한다.
        //        //if (false == m_InstanceMotion.RequestToken(m_nIndexOfMotionItem
        //        //    , m_nIndexOfSharedList))
        //        //{
        //        //    return false;
        //        //}

        //        m_bCheckingMotion = false;

        //        m_InstanceMotion.MoveSxAtSpeed(m_nIndexOfMotionItem, nDir, spdPattern, dblVelocity, dblAccel, dblDecel, dblAccelJerk, dblDecelJerk, uMotionTimeout, m_strTaskName);

        //        return true;
        //    }
        //    /// <summary>
        //    /// 2018.08.18 by yjlee [MOD] 조그(속도) 이동을 수행한다.
        //    /// </summary>
        //    public bool MoveSxByJog(bool nDir, uint uRatio = 100)
        //    {
        //        if (CheckMoving()) { return true; }

        //        //// 토큰을 요청한다.
        //        //if (false == m_InstanceMotion.RequestToken(m_nIndexOfMotionItem
        //        //    , m_nIndexOfSharedList))
        //        //{
        //        //    return false;
        //        //}

        //        // 조그는 모션 체크를 하지 않는다.
        //        m_bCheckingMotion = false;

        //        m_InstanceMotion.MoveSxAtSpeed(m_nIndexOfMotionItem, nDir, uRatio, m_strTaskName);

        //        return true;
        //    }
            
        //    /// <summary>
        //    /// 2019.06.03 by yjlee [ADD] 모션을 2단계로 수행한다.
        //    /// uRationPosition 값이 70%일 경우, 총 이동거리의 70%는 arRatio[0]의 속도로,
        //    /// 나머지 30%는 arRatio[1]의 속도로 이동한다.
        //    /// </summary>
        //    public bool MoveSxAbsolutelyAtTwoStep(double dblDestination, uint uRatioOfPosition, uint[] arRatioOfSpeed, EN_MOTOR_CONTENTOFSPEED spdContent = EN_MOTOR_CONTENTOFSPEED.RUN, uint uMotionDelay = 0, bool bCheckingMotion = true)
        //    {
        //        /// 2021.07.13
        //        /// -
        //        /// -
        //        //if (CheckMoving()) { return true; }

        //        ////// 토큰을 요청한다.
        //        ////if (false == m_InstanceMotion.RequestToken(m_nIndexOfMotionItem
        //        ////    , m_nIndexOfSharedList))
        //        ////{
        //        ////    return false;
        //        ////}

        //        //m_bCheckingMotion       = bCheckingMotion;

        //        //double[] arPosition         = {dblDestination * uRatioOfPosition * 0.01, dblDestination};
        //        //double[] arManualVelocity   = {0,0};

        //        //EN_MOTOR_CONTENTOFSPEED[] arContentOfSpeed  = {spdContent, spdContent};

        //        //m_InstanceMotion.MoveByList(m_nIndexOfMotionItem
        //        //    , 2
        //        //    , arPosition
        //        //    , arContentOfSpeed
        //        //    , arManualVelocity
        //        //    , arRatioOfSpeed
        //        //    , uMotionDelay
        //        //    , m_strTaskName);

        //        return true;
        //    }
        //    /// <summary>
        //    /// 2018.08.18 by yjlee [MOD] 해당 축의 속도를 재정의 한다.
        //    /// </summary>
        //    public bool OverrideSxSpeed(EN_MOTOR_CONTENTOFSPEED spdContent = EN_MOTOR_CONTENTOFSPEED.RUN, uint uRatio = 100)
        //    {
        //        if (CheckMoving()) { return true; }

        //        //// 토큰을 요청한다.
        //        //if (false == m_InstanceMotion.RequestToken(m_nIndexOfMotionItem
        //        //    , m_nIndexOfSharedList))
        //        //{
        //        //    return false;
        //        //}

        //        m_InstanceMotion.OverrideSxSpeed(m_nIndexOfMotionItem
        //            , spdContent
        //            , uRatio
        //            , m_strTaskName);

        //        return true;
        //    }
        //    /// <summary>
        //    /// 2018.08.18 by yjlee [MOD] 해당 축의 속도를 재정의 한다.
        //    /// </summary>
        //    public bool OverrideSxSpeed(EN_SPEED_PATTERN spdPattern, double dblVelocity, double dblAccel, double dblDecel)
        //    {
        //        if (CheckMoving()) { return true; }

        //        //// 토큰을 요청한다.
        //        //if (false == m_InstanceMotion.RequestToken(m_nIndexOfMotionItem
        //        //    , m_nIndexOfSharedList))
        //        //{
        //        //    return false;
        //        //}

        //        m_InstanceMotion.OverrideSxSpeed(m_nIndexOfMotionItem
        //            , spdPattern
        //            , dblVelocity
        //            , dblAccel
        //            , dblDecel
        //            , m_strTaskName);

        //        return true;
        //    }
        //    // 해당 축을 목표 지점들의 절대값 좌표 기준으로 예약이송을 한다.
        //    // 리스트 갯수, 지점, 속도, 속도비(%), 가속도비(%), 감속도비(%), 모션 완료 대기 사용 유무
        //    // false : 모션 동작 후 즉시 반환, true : 모션 완료 후 반환
        //    public bool MoveByList(int nLength, double[] dblDestination, EN_MOTOR_CONTENTOFSPEED[] spdContent, double[] dblVelocity, uint[] uRatio, uint uMotionDelay = 0, bool bCheckingMotion = true)
        //    {
        //        /// 2021.07.13
        //        ///--
        //        //if (CheckMoving()) { return true; }

        //        //// 토큰을 요청한다.
        //        //if (false == m_InstanceMotion.RequestToken(m_nIndexOfMotionItem
        //        //    , m_nIndexOfSharedList))
        //        //{
        //        //    return false;
        //        //}

        //        //m_bCheckingMotion = bCheckingMotion;

        //        //m_InstanceMotion.MoveByList(m_nIndexOfMotionItem, nLength, dblDestination, spdContent, dblVelocity, uRatio, uMotionDelay, m_strTaskName);

        //        return true;
        //    }
        //    /// <summary>
        //    /// 2019.06.03 by yjlee [ADD] 모션 선형 보간을 수행한다.
        //    /// X 축을 기준으로 보간을 수행한다.
        //    /// </summary>
        //    //public bool MoveByLinearCoordination(SharedMotion[] arSharedMotion, int nCountOfStep, double[,] arDestination, EN_MOTOR_CONTENTOFSPEED[] arContent, double[] arManualVelocity, uint[] arRatio, uint uDelay, bool bCheckingMotion = true)
        //    //{
        //    //    if (CheckMoving()) { return true; }

        //    //    if (null == arSharedMotion
        //    //        || arSharedMotion.Length < 1) { return false; }

        //    //    //// 토큰을 요청한다.
        //    //    //if (false == m_InstanceMotion.RequestToken(m_nIndexOfMotionItem
        //    //    //    , m_nIndexOfSharedList))
        //    //    //{
        //    //    //    return false;
        //    //    //}

        //    //    m_bCheckingMotion   = bCheckingMotion;
                
        //    //    int nCountOfAxis   = arSharedMotion.Length + 1;
        //    //    int[] arIndexes     = new int[nCountOfAxis];

        //    //    arIndexes[0]        = m_nIndexOfMotionItem;

        //    //    for(int i = 1 ; i < nCountOfAxis ; ++ i)
        //    //    {
        //    //        arSharedMotion[i-1].ActivateCheckingMotion(bCheckingMotion);
        //    //        arIndexes[i]    = arSharedMotion[i-1].nIndexOfMotionItem;
        //    //    }

        //    //    m_InstanceMotion.MoveByLinearCoordination(nCountOfAxis
        //    //        , arIndexes
        //    //        , nCountOfStep
        //    //        , arDestination
        //    //        , arContent
        //    //        , arManualVelocity
        //    //        , arRatio
        //    //        , uDelay
        //    //        , m_strTaskName);

        //    //    return true;
        //    //}
        //    /// <summary>
        //    /// 2018.08.18 by yjlee [MOD] 해당 축의 움직임을 정지한다
        //    /// bEmergency == true, 긴급정지
        //    /// </summary>
        //    public bool StopMotion(bool bEmergency = true)
        //    {
        //        if (CheckMoving()) { return true; }

        //        //// 토큰을 요청한다.
        //        //if (false == m_InstanceMotion.RequestToken(m_nIndexOfMotionItem
        //        //    , m_nIndexOfSharedList))
        //        //{
        //        //    return false;
        //        //}

        //        m_bCheckingMotion = true;

        //        m_InstanceMotion.StopMotion(m_nIndexOfMotionItem);

        //        return true;
        //    }
        //    #endregion

        //    #region 모션 상태 관련

        //    #region 상태 반환
        //    /// <summary>
        //    /// 2019.02.06 by yjlee [ADD] 모션이 스킵 되어야하는 상태인지 판단한다.
        //    /// </summary>
        //    public override bool IsSkipped()
        //    {
        //        return m_bSkipped || m_bEnable == false || m_bLock;
        //    }

        //    #endregion

        //    #endregion

        //    #endregion

        //    #region 내부 인터페이스
        //    /// <summary>
        //    /// 2019.02.06 by yjlee [ADD] 이송 가능한 상태인지 확인한다.
        //    /// </summary>
        //    private bool CheckMoving()
        //    {
        //        return m_bSkipped || false == m_bEnable;
        //    }
        //    #endregion
        //}
        /// <summary>
        /// 2019.02.05 by yjlee [ADD] 단순 상태만을 확인하기 위한 모션
        /// </summary>
        public class CheckedMotion
        {
            #region 변수
            private MotionInterfaces m_instanceMotion     = null;
            private int m_nIndexOfMotionItem    = -1;

            private bool m_isSkipped            = false;
            #endregion

            #region 속성
            public bool isSkipped
            {
                get { return m_isSkipped; }
                set { m_isSkipped = value; }
            }
            #endregion

            #region 생성자
            public CheckedMotion(MotionInterfaces instance, int nIndexOfItem)
            {
                m_instanceMotion        = instance;
                m_nIndexOfMotionItem    = nIndexOfItem;
            }
            #endregion

            #region 외부 인터페이스
            /// <summary>
            /// 2018.08.30 by yjlee [ADD] 현재 모션 아이템이 이송 중인지 판단한다.
            /// </summary>
            /// <returns></returns>
            public bool IsMotionMoving()
            {
                if (m_isSkipped) { return false; }

                return m_instanceMotion.IsMotionMoving(m_nIndexOfMotionItem);
            }
            /// <summary>
            /// 2018.08.18 by yjlee [MOD] 현재 엔코더 포지션을 읽어온다.
            /// </summary>
            public double GetActualPosition()
            {
                if (m_isSkipped) { return 0; }

                return m_instanceMotion.GetActualPosition(m_nIndexOfMotionItem);
            }
            /// <summary>
            /// 2018.09.16 by yjlee [ADD] 포지션 값을 비교한다.
            /// </summary>
            public bool CompareActualPosition(double dblPosition, bool bBelow = true)
            {
                if (m_isSkipped) { return true; }

                double dblActualPosition    = m_instanceMotion.GetActualPosition(m_nIndexOfMotionItem);

                if(bBelow
                    && dblActualPosition < dblPosition)
                {
                    return true;
                }
                else if(false == bBelow
                    && dblActualPosition > dblPosition)
                {
                    return true;
                }

                return false;
            }

            /// <summary>
            /// 2018.08.18 by yjlee [MOD] 현재 명령 포지션을 읽어온다.
            /// </summary>
            public double GetCommandPosition()
            {
                if (m_isSkipped) { return 0; }

                return m_instanceMotion.GetCommandPosition(m_nIndexOfMotionItem);
            }
            // <summary>
            /// 2018.09.16 by yjlee [ADD] 포지션 값을 비교한다.
            /// </summary>
            public bool CompareCommandPosition(double dblPosition, bool bBelow = true)
            {
                if (m_isSkipped) { return true; }

                double dblCommandPosition = m_instanceMotion.GetCommandPosition(m_nIndexOfMotionItem);

                if (bBelow
                    && dblCommandPosition < dblPosition)
                {
                    return true;
                }
                else if (false == bBelow
                    && dblCommandPosition > dblPosition)
                {
                    return true;
                }

                return false;
            }
            /// <summary>
            /// 2018.08.18 by yjlee [MOD] 모션 완료 상태를 점검한다.
            /// </summary>
            public bool IsMotionDone()
            {
                if (m_isSkipped) { return true; }

                return m_instanceMotion.IsMotionDone(m_nIndexOfMotionItem);
            }
            /// <summary>
            /// 2018.08.18 by yjlee [MOD] 홈 완료 상태를 점검한다.
            /// </summary>
            public bool IsHomeEnd()
            {
                if (m_isSkipped) { return true; }

                return m_instanceMotion.IsHomeEnd(m_nIndexOfMotionItem);
            }
            /// <summary>
            /// 2018.08.18 by yjlee [MOD] 홈 시간이 초과되었는지 반환한다.
            /// </summary>
            public bool IsTimeOutMotion()
            {
                if (m_isSkipped) { return false; }

                return m_instanceMotion.IsMotionTimeout(m_nIndexOfMotionItem);
            }
            /// <summary>
            /// 2018.08.18 by yjlee [MOD] 모션 이동 시간이 초과되었는지 반환한다.
            /// </summary>
            public bool IsTimeOutHome()
            {
                if (m_isSkipped) { return false; }

                return m_instanceMotion.IsHomeTimeout(m_nIndexOfMotionItem);
            }
            /// <summary>
            /// 2018.08.18 by yjlee [MOD] 모터 알람 상태를 확인한다.
            /// </summary>
            public bool IsMotionAlarm()
            {
                if (m_isSkipped) { return false; }

                return m_instanceMotion.IsMotionAlarm(m_nIndexOfMotionItem);
            }
            /// <summary>
            /// 2018.08.18 by yjlee [MOD] - 리밋인지 확인한다.
            /// </summary>
            public bool IsHWLimitNegative()
            {
                if (m_isSkipped) { return false; }

                return m_instanceMotion.IsMotionHWLimitNegative(m_nIndexOfMotionItem);
            }
            /// <summary>
            /// 2018.08.18 by yjlee [MOD] + 리밋인지 확인한다.
            /// </summary>
            public bool IsHWLimitPositive()
            {
                if (m_isSkipped) { return false; }

                return m_instanceMotion.IsMotioHWLimitPositive(m_nIndexOfMotionItem);
            }

            /// <summary>
            /// 2019.04.29 by yjlee [ADD] SW - 리밋인지 확인한다.
            /// </summary>
            public bool IsSWLimitNegative()
            {
                if (m_isSkipped) { return false; }

                return m_instanceMotion.IsMotionSWLimitNegative(m_nIndexOfMotionItem);
            }
            /// <summary>
            /// 2019.04.29 by yjlee [ADD] SW + 리밋인지 확인한다.
            /// </summary>
            public bool IsSWLimitPositive()
            {
                if (m_isSkipped) { return false; }

                return m_instanceMotion.IsMotionSWLimitPositive(m_nIndexOfMotionItem);
            }
            /// <summary>
            /// 2018.08.28 by yjlee [ADD] 모터가 온 상태인지 확인한다.
            /// </summary>
            public bool IsMotorOn()
            {
                if (m_isSkipped) { return true; }

                return m_instanceMotion.IsMotorOn(m_nIndexOfMotionItem);
            }

            #endregion
        }


        //public struct UVW_ORG
        //{
        //    public double OUx;
        //    public double OUy;

        //    public double OVx;
        //    public double OVy;

        //    public double OWx;
        //    public double OWy;
        //};

    }
}