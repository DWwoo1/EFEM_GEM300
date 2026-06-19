using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Define
{
    namespace DefineEnumProject
    {
        namespace Task
        {
            /// <summary>
            /// 2020.07.26 by yjlee [ADD] Enumerate the status fo the task.
            /// </summary>
            public enum EN_TASK_STATUS
            {
                DISABLED = 0,        // 사용 안함
                UNABLED,             // 사용 불가
                EMPTY,               // 자재 없음
                EXIST,               // 자재 있음
                EXIST_FULL,          // 자재 가득 참
                WORKING,             // 작업 진행 중
                FINISH,              // 작업 완료
            }

            /// <summary>
            /// 2020.07.29 by yjlee [ADD] Enumerate the devices of the task.
            /// </summary>
            public enum EN_TASK_DEVICE
            {
                MOTION = 0,
                CYLINDER,
                ANALOG_INPUT,
                ANALOG_OUPUT,
                DIGITAL_INPUT,
                DIGITAL_OUTPUT,
            }

            public enum EN_TASK_LIST
            {
                LoadPort1,
                LoadPort2,
                LoadPort3,
                LoadPort4,
#if PWA500W
#else
                LoadPort5,
                LoadPort6,
#endif
                AtmRobot,
                Global
            }

            public enum EN_MOTION_ALARM_TYPE
            {
                CATEGORY,
                INTERLOCK,
            }
            public enum EN_SOFT_STEP_TYPE
            {
                PRE,	// 첫 움직임이 slow
                POST,	// 마지막 움직임이 slow
                DUAL,	// 앞, 뒤로 모두 slow
            }

            namespace LoadPort
            {
                public enum EN_AXIS
                {
                    NONE = -1,
                }
                public enum EN_CYLINDER
                {
                    NONE = -1,
                }
                public enum EN_DIGITAL_IN
                {
                    NONE = -1,
                }
                public enum EN_DIGITAL_OUT
                {
                    NONE = -1,
                }
                public enum EN_ANALOG_IN
                {
                    NONE = -1,
                }
                public enum EN_ANALOG_OUT
                {
                    NONE = -1,
                }
                public enum EN_PORT
                {
                    LOADPORT_STATE,
                }
                public enum PARAM_PROCESS
                {
                    NONE = 0,
                }
                public enum EN_ALARM
                {
                    // 로드포트 동작 실패 (60~)
                    LOADPORT_HAS_NOT_BEEN_INITIALIZED = 60,         // [ $T ] loadport module has not been initialized!
                    LOADPORT_HAS_ALARM,                             // [ $T ] loadport module has alarm
                    LOADPORT_PLACEMENT_ERROR,                       // [ $T ] loadport module has placement error!
                    LOADPORT_CARRIER_OUT_ERROR,                     // [ $T ] loadport module has carrier out error!

                    LOADPORT_ACCESS_VIOLATION_ERROR,            // 2025.10.10 dwlim [ADD] E84 PIO 사양 맞게 Alarm 추가

                    LOADPORT_FAILED_TO_ACTION = 65,                 // [ $T ] loadport module failed to actions! ([ $T ])
                    LOADPORT_FAILED_TO_CHANGE_MODE = 66,            // [ $T ] loadport module failed to change modes! 

                    // RFID 동작 실패(75~)
                    RFID_READ_COMMAND_HAS_FAILED = 75,              // [ $T ] RFID failed to actions! ([ $T ])
                    RFID_WRITE_COMMAND_HAS_FAILED = 76,              // [ $T ] RFID failed to actions! ([ $T ])
                    RFID_CONTROLLER_NOT_CONNECTED = 79,

                    // 물류 에러(80~)
                    LOADPORT_HAS_FAILED_DURING_LOADING_HANDOFF = 80,
                    LOADPORT_HAS_FAILED_DURING_UNLOADING_HANDOFF = 85,
                    SAFTY_BAR_DETECTED_DURING_HANDOFF = 89,

                    // 기타 에러
                    LOADPORT_SLOT_STATUS_IS_WRONG = 90,
                    LOADPORT_FAILED_TO_EXECUTE_SCENARIO_ID_READ,
                    LOADPORT_FAILED_TO_EXECUTING_SCENARIO_ID_VERIFICATION,
                    LOADPORT_FAILED_TO_EXECUTING_SCENARIO_SLOT_VERIFICATION,

                    LOADPORT_SECSGEM_ERROR_BEFORE_LOADING_CARRIER = 95,
                    LOADPORT_SECSGEM_ERROR_BEFORE_UNLOADING_CARRIER,

                    LOADPORT_CONTROLLER_NOT_CONNECTED = 99,
                }
            }
            namespace AtmRobot
            {
                public enum EN_AXIS
                {
                    NONE = -1,
                }
                public enum EN_CYLINDER
                {
                    NONE = -1,
                }
                public enum EN_DIGITAL_IN
                {
                    NONE = 0,
                }
                public enum EN_DIGITAL_OUT
                {
                    NONE = 0,
                }
                public enum EN_ANALOG_IN
                {
                    NONE = -1,

                }
                public enum EN_ANALOG_OUT
                {
                    NONE = -1,

                }
                public enum EN_PORT
                {
                    ROBOT_STATE,
                }
                public enum PARAM_PROCESS
                {
                    NONE = 0,
                }
                public enum EN_ALARM
                {
                    // 물류 인터페이스 실패 (1~59) : 솔루션 X
                    INTERFACE_BEFORE_LOADING_DATA_INVALID = 1,
                    INTERFACE_BEFORE_LOADING_SENDING_FAILED,
                    INTERFACE_BEFORE_LOADING_SENDING_COMPLETED_TIMEOUT_ACK,
                    INTERFACE_BEFORE_LOADING_SENDING_COMPLETED_BUT_NACK,
                    INTERFACE_BEFORE_LOADING_RECEIVING_RESPONSE_MESSAGE_TIMEOUT,
                    INTERFACE_BEFORE_LOADING_RECEIVING_RESPONSE_DATA_TIMEOUT,
                    INTERFACE_BEFORE_LOADING_RECEIVING_COMPLETED_BUT_ERROR,
                    INTERFACE_BEFORE_LOADING_RECEIVING_COMPLETED_BUT_DATA_INVALID,

                    INTERFACE_ACTION_LOADING_DATA_INVALID = 11,
                    INTERFACE_ACTION_LOADING_SENDING_FAILED,
                    INTERFACE_ACTION_LOADING_SENDING_COMPLETED_TIMEOUT_ACK,
                    INTERFACE_ACTION_LOADING_SENDING_COMPLETED_BUT_NACK,
                    INTERFACE_ACTION_LOADING_RECEIVING_RESPONSE_MESSAGE_TIMEOUT,
                    INTERFACE_ACTION_LOADING_RECEIVING_RESPONSE_DATA_TIMEOUT,
                    INTERFACE_ACTION_LOADING_RECEIVING_COMPLETED_BUT_ERROR,
                    INTERFACE_ACTION_LOADING_RECEIVING_COMPLETED_BUT_DATA_INVALID,

                    INTERFACE_AFTER_LOADING_DATA_INVALID = 21,
                    INTERFACE_AFTER_LOADING_SENDING_FAILED,
                    INTERFACE_AFTER_LOADING_SENDING_COMPLETED_TIMEOUT_ACK,
                    INTERFACE_AFTER_LOADING_SENDING_COMPLETED_BUT_NACK,
                    INTERFACE_AFTER_LOADING_RECEIVING_RESPONSE_MESSAGE_TIMEOUT,
                    INTERFACE_AFTER_LOADING_RECEIVING_RESPONSE_DATA_TIMEOUT,
                    INTERFACE_AFTER_LOADING_RECEIVING_COMPLETED_BUT_ERROR,
                    INTERFACE_AFTER_LOADING_RECEIVING_COMPLETED_BUT_DATA_INVALID,
                    INTERFACE_AFTER_LOADING_SMEMA_TIMEOUT,

                    INTERFACE_BEFORE_UNLOADING_DATA_INVALID = 31,
                    INTERFACE_BEFORE_UNLOADING_SENDING_FAILED,
                    INTERFACE_BEFORE_UNLOADING_SENDING_COMPLETED_TIMEOUT_ACK,
                    INTERFACE_BEFORE_UNLOADING_SENDING_COMPLETED_BUT_NACK,
                    INTERFACE_BEFORE_UNLOADING_RECEIVING_RESPONSE_MESSAGE_TIMEOUT,
                    INTERFACE_BEFORE_UNLOADING_RECEIVING_RESPONSE_DATA_TIMEOUT,
                    INTERFACE_BEFORE_UNLOADING_RECEIVING_COMPLETED_BUT_ERROR,
                    INTERFACE_BEFORE_UNLOADING_RECEIVING_COMPLETED_BUT_DATA_INVALID,

                    INTERFACE_ACTION_UNLOADING_DATA_INVALID = 41,
                    INTERFACE_ACTION_UNLOADING_SENDING_FAILED,
                    INTERFACE_ACTION_UNLOADING_SENDING_COMPLETED_TIMEOUT_ACK,
                    INTERFACE_ACTION_UNLOADING_SENDING_COMPLETED_BUT_NACK,
                    INTERFACE_ACTION_UNLOADING_RECEIVING_RESPONSE_MESSAGE_TIMEOUT,
                    INTERFACE_ACTION_UNLOADING_RECEIVING_RESPONSE_DATA_TIMEOUT,
                    INTERFACE_ACTION_UNLOADING_RECEIVING_COMPLETED_BUT_ERROR,
                    INTERFACE_ACTION_UNLOADING_RECEIVING_COMPLETED_BUT_DATA_INVALID,

                    INTERFACE_AFTER_UNLOADING_DATA_INVALID = 51,
                    INTERFACE_AFTER_UNLOADING_SENDING_FAILED,
                    INTERFACE_AFTER_UNLOADING_SENDING_COMPLETED_TIMEOUT_ACK,
                    INTERFACE_AFTER_UNLOADING_SENDING_COMPLETED_BUT_NACK,
                    INTERFACE_AFTER_UNLOADING_RECEIVING_RESPONSE_MESSAGE_TIMEOUT,
                    INTERFACE_AFTER_UNLOADING_RECEIVING_RESPONSE_DATA_TIMEOUT,
                    INTERFACE_AFTER_UNLOADING_RECEIVING_COMPLETED_BUT_ERROR,
                    INTERFACE_AFTER_UNLOADING_RECEIVING_COMPLETED_BUT_DATA_INVALID,
                    INTERFACE_AFTER_UNLOADING_SMEMA_TIMEOUT,

                    // 로봇 동작 실패 (60~79) : 컨트롤러에서 에러등 동작 실패
                    ATM_ROBOT_IS_NOT_INITIALIZED = 60,                              // 70060 : 홈 다시 잡음
                    ATM_ROBOT_INITIALIZING_FAILED,                                  // 70061 : 컨트롤러 알람코드 확인
                    ATM_ROBOT_ALARM_CLEARING_FAILED,                                // 70061 : 컨트롤러 알람코드 확인
                    ATM_ROBOT_PICKING_ACTION_FAILED,                                // 70061 : 컨트롤러 알람코드 확인
                    ATM_ROBOT_PLACING_ACTION_FAILED,                                // 70061 : 컨트롤러 알람코드 확인
                    ATM_ROBOT_APPROACH_LOADING_FAILED,                              // 70061 : 컨트롤러 알람코드 확인
                    ATM_ROBOT_LOADING_FAILED,                                       // 70061 : 컨트롤러 알람코드 확인
                    ATM_ROBOT_APPROACH_UNLOADING_FAILED,                            // 70061 : 컨트롤러 알람코드 확인
                    ATM_ROBOT_UNLOADING_FAILED,                                     // 70061 : 컨트롤러 알람코드 확인

                    // 로봇 정보 에러 (80~89)
                    ATM_ROBOT_CANNOT_GET_WORKING_INFO = 80,                         // 70080
                    ATM_ROBOT_HAS_NO_AVAILABLE_ARM,                                 // 70081              
                    ATM_ROBOT_DOES_NOT_HAVE_CARRIER,                                // 70082

                    // ~90
                    ATM_ROBOT_SECSGEM_ERROR_BEFORE_PICK = 90,
                    ATM_ROBOT_SECSGEM_ERROR_AFTER_PICK,

                    ATM_ROBOT_SECSGEM_ERROR_BEFORE_PLACE = 95,
                    ATM_ROBOT_SECSGEM_ERROR_AFTER_PLACE,

                    ATM_ROBOT_CONTROLLER_NOT_CONNECTED = 99,
                }
            }
            namespace Global
            {
                public enum PARAM_GLOBAL
                {
                    NONE = 0,
                }
                public enum ALARM_GLOBAL
                {
                    DOOR_UNLOCKED = 60,
                    FAN_ERROR,
                    FFU_ERROR,
                    IONIZER_ERROR,
                    AIR_ERROR,
                    WrongScenarioCirculation,
                    FDC_ERROR,

                    COMMUNICATION_ERROR_WITH_PM = 70,

                    FFU_CONTROLLER_NOT_CONNECTED = 99,
                }
            }
        }

        namespace Map
        {
            public enum EN_MAP_TYPE
            {
                SAMPLE,
            }
            public enum EN_UNIT_STATUS
            {
                NONE,           // Map 생성 시 사용하지 않는 영역
                EMPTY,
                REFERENCE,
                SKIP,
                WAIT,           // 작업 가능
                IDCODE,
                ALIGN,
                INSP,
                REJECT,         // 비전 인식 불량
                STACK,
                DONE,           // 작업 완료
                FAIL            // 작업 실패
            }
            public enum EN_WORK_START_POINT
            {
                LEFT_TOP,
                LEFT_BOTTOM,
                RIGHT_TOP,
                RIGHT_BOTTOM,
            }
            public enum EN_WORK_DIRECTION
            {
                HORIZONTAL_ONEWAY,
                HORIZONTAL_ZIGZAG,
                VERTICAL_ONEWAY,
                VERTICAL_ZIGZAG,
            }
            public enum EN_SUBJECT_STATUS
            {
                Empty,
                Exist,
                Working,
                Finished,
                Ng,
            }
        }

        // 2021.07.13. by shkim. [ADD] 비전관련Enum
        namespace Vision
        {
            public enum EN_CAMERA_LIST
            {
                CAM1 = 0,
            }
            public enum EN_VISION_ALGORITHM
            {
                NONE = -1,
                SAMPLE = 0,
            }
            public enum EN_TARGET_LAYER
            {
                ALIGN = 0,
            }
            public enum EN_VISION_CALIBRATION_SCENE
            {
                RESOLUTION_FRONT = 0,
                DISTORTION_FRONT,
                RESOLUTION_REAR,
                DISTORTION_REAR,
            }
            public enum EN_VISION_CALIBRATION_MODE
            {
                RESOLUTION = 0,
                DISTORTION = 2,
            }
            public enum EN_VISION_RESULT_TYPE
            {
                XYT = 0,
                BARCODE,
            }
        }

        namespace ButtonEvent
        {
            /// <summary>
            /// 2020.02.05 by yjlee [ADD] Enumerate the names of the sub menus.
            /// A button will be added to the view if you add a name to the enum.
            /// </summary>
            public enum EN_BUTTONEVENT_SUBMENU
            {
                OPERATION_MAIN,
                //OPERATION_MANUAL,
                OPERATION_MONITORING,
                OPERATION_RAM_METRICS,
                OPERATION_TRACKING,
                OPERATION_SECSGEM,
                OPERATION_LOT_HISTORY,
                OPERATION_JOB_INFO,
                OPERATION_EFEM_SIMULATOR,

                RECIPE_MAIN = 100,
                //RECIPE_OPTIONS,

                HISTORY_MAIN_LOG = 200,

                //SETUP_OPTIONS = 300,
                SETUP_LOADPORT = 300,
                SETUP_ROBOT,
                SETUP_RFID,
                SETUP_PROCESSMODULE,
                SETUP_FFU,

                //SETUP_VISION,
                //SETUP_JOG,

                //CONFIG_MOTION = 400,
                CONFIG_ANALOG = 400,
                CONFIG_DIGITAL,
                CONFIG_CYLINDER,
                CONFIG_ALARM,
                CONFIG_INTERRUPT,
                CONFIG_LANGUAGE,
                CONFIG_TRIGGER,

                CONFIG_COMMUNICATION,
                CONFIG_ACTION,
                //CONFIG_INTERLOCK,
                //CONFIG_TOOL,
                //CONFIG_JOG,
                CONFIG_DEVICE,
                CONFIG_PARAMETERS,
            }
        }

        namespace SelectionList
        {
            /// <summary>
            /// 2020.06.05 by twkang [ADD] Selection List 에 사용하는 열거형이다.
            /// </summary>
            public enum EN_SELECTIONLIST
            {
                NONE = 0,

                #region base
                ENABLE_DISABLE,

                TRUE_FALSE,

                LOG_TYPE,
                USER_AUTHORITY,

                DEVICE_TYPE,

                EQUIPMENT_STATE,
                OPERATION_EQUIPMENT,

                CYLINDER_MONITORING_MODE,

                SOCKET_PROTOCOL_TYPE,
                SOCKET_LOG_TYPE,

                SERIAL_DATA_BIT,
                SERIAL_BAUDRATE,
                SERIAL_STOPBIT,
                SERIAL_PARITY,
                SERIAL_LOG_TYPE,
                WCF_CALLBACK_FUNCTION,

                INTERRUPT_ACTION,

                ALARM_STATE,

                MOTION_MOTOR_TYPE,
                MOTION_MOTION_TYPE,
                MOTION_MOTOR_DIRECTION,
                MOTION_LIMIT_STOP_MODE,
                MOTION_LOGIC,
                MOTION_HOME_MODE,
                MOTION_HOME_DIRECTION,
                MOTION_SPEED_PATTERN,

                MOTION_INMODE,
                MOTION_OUTMODE,

                ACTION_LOGIC,
                ACTION_COMPARE_STATE,

                PORT_STATUS,
                RECIPE_TYPE,

                VISION_CAMERA_LIST,    // 2021.07.13. by shkim [ADD] 비전 카메라 리스트
                ALIGN_CAMERA_SCENE,

                INTERLOCK_COMPARE_DEVICE,
                MOTION_COMPARE_DIRECTION,
                CYLINDER_COMPARE_DIRECTION,
                MOTION_MOVING_DIRECTION,
                LANGUAGE,
                VISION_CALIBRATION_MODE,
                VISION_CALIBRATION_SCENE,
                FORWARD_BACKWARD,
                TASK_LIST,
                ANALOG_DATA_TYPE,   // 2022.11.04. by shkim. [ADD] Raw data type에 따른 계산과정 추가
                MATRIX,
                #endregion /base

                // PROJECT ONLY
                #region project only
                WORK_DIRECTION,
                WORK_START_POINT,
                WARPAGE_TYPE,
                WORK_STATUS,
                SUBJECT_ANGLE,

                FLUX_CLEANING_TYPE,
                FLUX_CLEANING_DIRECTION,
                ALIGN_POINT_TYPE,

                WAFER_TYPE_BIN,
                WAFER_TYPE_500W,
                WAFER_SIZE,
                ARM_TYPE,

                SUBSTRATE_TRANSFER_STATE,
                SUBSTRATE_PROCESSING_STATE,
                SUBSTRATE_ID_READING_STATE,

                SUBSTRATE_TYPE,
                #endregion project only
            }
        }

        namespace Socket
        {
            /// <summary>
            /// 2020.11.14 by yjlee [ADD] Enumerate the index of the socket.
            /// </summary>
            public enum EN_SOCKET_INDEX
            {
                LOG = 0,
                MODBUS,
                ATM_ROBOT,
            }
        }
        namespace Serial
        {
            public enum EN_SERIAL_INDEX
            {
                LOADPORT_1 = 0,
                LOADPORT_2,
                LOADPORT_3,
                LOADPORT_4,
                LOADPORT_5,
                LOADPORT_6,
                RFID_FOUP_1,
                RFID_FOUP_2,
                RFID_FOUP_3,
                RFID_FOUP_4,
                RFID_FOUP_5,
                RFID_FOUP_6,
                RFID_CASSETTE_1,
                RFID_CASSETTE_2,
                RFID_CASSETTE_3,
                RFID_CASSETTE_4,
                RFID_CASSETTE_5,
                RFID_CASSETTE_6,
                ATM_ROBOT,
                FFU,
            }
        }
        namespace WCF
        {
            public enum EN_WCF_INDEX
            {
            }

            public enum EN_CALLBACK_FUNCTION_TYPE
            {
                DEFAULT_OPENED = 0,
                DEFAULT_CLOSED,
                DEFAULT_ALAYSIS,

                DEFAULT_CONNECTED,
                DEFAULT_DISCONNECTED,
                DEFAULT_FAULTED,
            }
        }

        namespace DigitalIO
        {
            namespace PWA500BIN
            {
                public enum EN_DIGITAL_IN
                {
                    LP1_PIO_VALID = 0,  // > X00
                    LP1_PIO_CS0,
                    LP1_PIO_CS1,
                    LP1_PIO_CS2,
                    LP1_PIO_CS3,
                    LP1_PIO_TRIGGER_REQUEST,
                    LP1_PIO_BUSY,
                    LP1_PIO_COMPLETED,

                    LP2_PIO_VALID,
                    LP2_PIO_CS0,
                    LP2_PIO_CS1,
                    LP2_PIO_CS2,
                    LP2_PIO_CS3,
                    LP2_PIO_TRIGGER_REQUEST,
                    LP2_PIO_BUSY,
                    LP2_PIO_COMPLETED,

                    LP3_PIO_VALID,
                    LP3_PIO_CS0,
                    LP3_PIO_CS1,
                    LP3_PIO_CS2,
                    LP3_PIO_CS3,
                    LP3_PIO_TRIGGER_REQUEST,
                    LP3_PIO_BUSY,
                    LP3_PIO_COMPLETED,

                    LP4_PIO_VALID,
                    LP4_PIO_CS0,
                    LP4_PIO_CS1,
                    LP4_PIO_CS2,
                    LP4_PIO_CS3,
                    LP4_PIO_TRIGGER_REQUEST,
                    LP4_PIO_BUSY,
                    LP4_PIO_COMPLETED,

                    LP5_PIO_VALID,
                    LP5_PIO_CS0,
                    LP5_PIO_CS1,
                    LP5_PIO_CS2,
                    LP5_PIO_CS3,
                    LP5_PIO_TRIGGER_REQUEST,
                    LP5_PIO_BUSY,
                    LP5_PIO_COMPLETED,

                    LP6_PIO_VALID,
                    LP6_PIO_CS0,
                    LP6_PIO_CS1,
                    LP6_PIO_CS2,
                    LP6_PIO_CS3,
                    LP6_PIO_TRIGGER_REQUEST,
                    LP6_PIO_BUSY,
                    LP6_PIO_COMPLETED,

                    LP1_RUN,
                    LP1_OPEN,
                    LP1_PLACEMENT_CASSETTE_STATUS,
                    LP1_PLACEMENT_MAC_FOUP_STATUS,
                    LP1_PRESENT_STATUS,
                    LP1_MANUAL_BUTTON_CASSETTE_STATUS,
                    LP1_MANUAL_BUTTON_MAC_FOUP_STATUS,
                    LP1_STATUS_SPARE,

                    LP2_RUN,
                    LP2_OPEN,
                    LP2_PLACEMENT_CASSETTE_STATUS,
                    LP2_PLACEMENT_MAC_FOUP_STATUS,
                    LP2_PRESENT_STATUS,
                    LP2_MANUAL_BUTTON_CASSETTE_STATUS,
                    LP2_MANUAL_BUTTON_MAC_FOUP_STATUS,
                    LP2_STATUS_SPARE,

                    LP3_RUN,
                    LP3_OPEN,
                    LP3_PLACEMENT_CASSETTE_STATUS,
                    LP3_PLACEMENT_MAC_FOUP_STATUS,
                    LP3_PRESENT_STATUS,
                    LP3_MANUAL_BUTTON_CASSETTE_STATUS,
                    LP3_MANUAL_BUTTON_MAC_FOUP_STATUS,
                    LP3_STATUS_SPARE,

                    LP4_RUN,
                    LP4_OPEN,
                    LP4_PLACEMENT_CASSETTE_STATUS,
                    LP4_PLACEMENT_MAC_FOUP_STATUS,
                    LP4_PRESENT_STATUS,
                    LP4_MANUAL_BUTTON_CASSETTE_STATUS,
                    LP4_MANUAL_BUTTON_MAC_FOUP_STATUS,
                    LP4_STATUS_SPARE,

                    LP5_RUN,
                    LP5_OPEN,
                    LP5_PLACEMENT_CASSETTE_STATUS,
                    LP5_PLACEMENT_MAC_FOUP_STATUS,
                    LP5_PRESENT_STATUS,
                    LP5_MANUAL_BUTTON_CASSETTE_STATUS,
                    LP5_MANUAL_BUTTON_MAC_FOUP_STATUS,
                    LP5_STATUS_SPARE,

                    LP6_RUN,
                    LP6_OPEN,
                    LP6_PLACEMENT_CASSETTE_STATUS,
                    LP6_PLACEMENT_MAC_FOUP_STATUS,
                    LP6_PRESENT_STATUS,
                    LP6_MANUAL_BUTTON_CASSETTE_STATUS,
                    LP6_MANUAL_BUTTON_MAC_FOUP_STATUS,
                    LP6_STATUS_SPARE,

                    EFEM_POWER_BOX_FAN_STATUS,
                    EFEM_IO_BOX_FAN_STATUS,
                    FFU_ALARM,
                    IONIZER_1_ALARM_STATUS,     //LPM 1, 2, 3
                    IONIZER_2_ALARM_STATUS,     //LPM 4, 5, 6
                    IONIZER_3_ALARM_STATUS,     //EQ 1, 2
                    IONIZER_4_ALARM_STATUS,     //EQ 3, 4
                    EFEM_MAIN_CDA_PRESSURE_SWITCH,

                    EFEM_MAIN_VACUUM_PRESSURE_SWITCH,
                    ROBOT_CDA_PRESSURE_SWITCH,
                    IONIZER_CDA_PRESSURE_SWITCH,
                    IONIZER_1_FLOW_METER,   //LPM 1, 2, 3
                    IONIZER_2_FLOW_METER,   //LPM 4, 5, 6
                    IONIZER_3_FLOW_METER,   //EQ 1, 2
                    IONIZER_4_FLOW_METER,   //EQ 3, 4
                    SPARE_1,    //SPARE

                    ROBOT_RETRACT_STATION_1,    //LPM #1 - MAC FOUP
                    ROBOT_RETRACT_STATION_2,    //LPM #2 - MAC FOUP
                    ROBOT_RETRACT_STATION_3,    //LPM #3 - MAC FOUP
                    ROBOT_RETRACT_STATION_4,    //LPM #4 - MAC FOUP
                    ROBOT_RETRACT_STATION_5,    //LPM #5 - MAC FOUP
                    ROBOT_RETRACT_STATION_6,    //LPM #6 - MAC FOUP
                    ROBOT_RETRACT_STATION_7,    //LPM #1 - CASSETTE
                    ROBOT_RETRACT_STATION_8,    //LPM #2 - CASSETTE

                    ROBOT_RETRACT_STATION_9,    //LPM #3 - CASSETTE
                    ROBOT_RETRACT_STATION_10,    //LPM #4 - CASSETTE
                    ROBOT_RETRACT_STATION_11,    //LPM #5 - CASSETTE
                    ROBOT_RETRACT_STATION_12,    //LPM #6 - CASSETTE
                    ROBOT_RETRACT_STATION_13,    //EQ #1
                    ROBOT_RETRACT_STATION_14,    //EQ #2
                    ROBOT_RETRACT_STATION_15,    //EQ #3
                    ROBOT_RETRACT_STATION_16,    //EQ #4

                    ROBOT_LOWER_ARM_RETRACT,
                    ROBOT_UPPER_ARM_RETRACT,
                    ROBOT_MODE,
                    ROBOT_INITIALIZE_COMPLETE,
                    ROBOT_BUSY_STATUS,
                    ROBOT_ALARM_STATUS,
                    ROBOT_WAFER_ON_ARM_LOWER,
                    ROBOT_WAFER_ON_ARM_UPPER,

                    ROBOT_CONTROLLER_FAN_ALARM,
                    ROBOT_SERVO_ON_OFF_STATUS,
                    EFEM_EMS_STATUS,
                    PROTECTION_BAR_LP_1_2_3,
                    PROTECTION_BAR_LP_4_5_6,
                    EFEM_DOOR_CLOSE,
                    AUTO_MANUAL_MODE,
                    FIRE_DETECTOR,

                    EQ_1_READY,
                    EQ_2_READY,
                    EQ_3_READY,
                    EQ_4_READY,
                    EQ_1_HANDSHAKE,
                    EQ_2_HANDSHAKE,
                    EQ_3_HANDSHAKE,
                    EQ_4_HANDSHAKE,

                    SPARE_2,    //SPARE
                    SPARE_3,    //SPARE
                    SPARE_4,    //SPARE
                    SPARE_5,    //SPARE
                    SPARE_6,    //SPARE
                    SPARE_7,    //SPARE
                    SPARE_8,    //SPARE
                    SPARE_9,    //SPARE
                }

                public enum EN_DIGITAL_OUT
                {
                    LP1_PIO_L_REQ = 0,
                    LP1_PIO_U_REQ,
                    LP1_PIO_ABORT,
                    LP1_PIO_READY,
                    LP1_SPARE5,
                    LP1_SPARE6,
                    LP1_SPARE7,
                    LP1_SPARE8,

                    LP2_PIO_L_REQ,
                    LP2_PIO_U_REQ,
                    LP2_PIO_ABORT,
                    LP2_PIO_READY,
                    LP2_SPARE5,
                    LP2_SPARE6,
                    LP2_SPARE7,
                    LP2_SPARE8,

                    LP3_PIO_L_REQ,
                    LP3_PIO_U_REQ,
                    LP3_PIO_ABORT,
                    LP3_PIO_READY,
                    LP3_SPARE5,
                    LP3_SPARE6,
                    LP3_SPARE7,
                    LP3_SPARE8,

                    LP4_PIO_L_REQ,
                    LP4_PIO_U_REQ,
                    LP4_PIO_ABORT,
                    LP4_PIO_READY,
                    LP4_SPARE5,
                    LP4_SPARE6,
                    LP4_SPARE7,
                    LP4_SPARE8,

                    LP5_PIO_L_REQ,
                    LP5_PIO_U_REQ,
                    LP5_PIO_ABORT,
                    LP5_PIO_READY,
                    LP5_SPARE5,
                    LP5_SPARE6,
                    LP5_SPARE7,
                    LP5_SPARE8,

                    LP6_PIO_L_REQ,
                    LP6_PIO_U_REQ,
                    LP6_PIO_ABORT,
                    LP6_PIO_READY,
                    LP6_SPARE5,
                    LP6_SPARE6,
                    LP6_SPARE7,
                    LP6_SPARE8,

                    LP1_MANUAL_CASSETTE,
                    LP1_MANUAL_MAC_FOUP,
                    LP2_MANUAL_CASSETTE,
                    LP2_MANUAL_MAC_FOUP,
                    LP3_MANUAL_CASSETTE,
                    LP3_MANUAL_MAC_FOUP,
                    LP4_MANUAL_CASSETTE,
                    LP4_MANUAL_MAC_FOUP,

                    LP5_MANUAL_CASSETTE,
                    LP5_MANUAL_MAC_FOUP,
                    LP6_MANUAL_CASSETTE,
                    LP6_MANUAL_MAC_FOUP,
                    IONIZER_1_On_OFF,   //LPM 1, 2, 3
                    IONIZER_2_On_OFF,   //LPM 4, 5, 6
                    IONIZER_3_On_OFF,   //LPM 1, 2
                    IONIZER_4_On_OFF,   //LPM 3, 4

                    SIGNAL_TOWER_RED,
                    SIGNAL_TOWER_YELLOW,
                    SIGNAL_TOWER_GREEN,
                    SIGNAL_TOWER_BLUE,
                    SIGNAL_TOWER_BUZZER_1,
                    SIGNAL_TOWER_BUZZER_2,
                    EFEM_DOOR_OPEN_CLOSE,
                    ATM_ROBOT_HANDSHAKE_EQ_1,

                    ATM_ROBOT_HANDSHAKE_EQ_2,
                    ATM_ROBOT_HANDSHAKE_EQ_3,
                    ATM_ROBOT_HANDSHAKE_EQ_4,
                    SPARE_1,    //SPARE
                    SPARE_2,    //SPARE
                    SPARE_3,    //SPARE
                    SPARE_4,    //SPARE
                    SPARE_5,    //SPARE

                }
            }
            namespace PWA500W
            {
                public enum EN_DIGITAL_IN
                {
                    LP1_PIO_VALID = 0,  // > X00
                    LP1_PIO_CS0,
                    LP1_PIO_CS1,
                    LP1_PIO_CS2,
                    LP1_PIO_CS3,
                    LP1_PIO_TRIGGER_REQUEST,
                    LP1_PIO_BUSY,
                    LP1_PIO_COMPLETED,

                    LP2_PIO_VALID,
                    LP2_PIO_CS0,
                    LP2_PIO_CS1,
                    LP2_PIO_CS2,
                    LP2_PIO_CS3,
                    LP2_PIO_TRIGGER_REQUEST,
                    LP2_PIO_BUSY,
                    LP2_PIO_COMPLETED,

                    LP3_PIO_VALID,
                    LP3_PIO_CS0,
                    LP3_PIO_CS1,
                    LP3_PIO_CS2,
                    LP3_PIO_CS3,
                    LP3_PIO_TRIGGER_REQUEST,
                    LP3_PIO_BUSY,
                    LP3_PIO_COMPLETED,

                    LP4_PIO_VALID,
                    LP4_PIO_CS0,
                    LP4_PIO_CS1,
                    LP4_PIO_CS2,
                    LP4_PIO_CS3,
                    LP4_PIO_TRIGGER_REQUEST,
                    LP4_PIO_BUSY,
                    LP4_PIO_COMPLETED,

                    LP1_RUN,
                    LP1_OPEN,
                    LP1_PLACEMENT_STATUS,
                    LP1_PRESENT_STATUS,
                    LP2_RUN,
                    LP2_OPEN,
                    LP2_PLACEMENT_STATUS,
                    LP2_PRESENT_STATUS,

                    LP3_RUN,
                    LP3_OPEN,
                    LP3_PLACEMENT_STATUS,
                    LP3_PRESENT_STATUS,
                    LP4_RUN,
                    LP4_OPEN,
                    LP4_PLACEMENT_STATUS,
                    LP4_PRESENT_STATUS,

                    EFEM_POWER_BOX_FAN_STATUS,
                    EFEM_IO_BOX_FAN_STATUS,
                    FFU_ALARM,
                    IONIZER_1_ALARM_STATUS,     //LPM 1, 2
                    IONIZER_2_ALARM_STATUS,     //LPM 3, 4
                    IONIZER_3_ALARM_STATUS,     //EQ
                    EFEM_MAIN_CDA_PRESSURE_SWITCH,
                    EFEM_MAIN_VACUUM_PRESSURE_SWITCH,

                    ROBOT_CDA_PRESSURE_SWITCH,
                    IONIZER_CDA_PRESSURE_SWITCH,
                    IONIZER_1_FLOW_METER,   //LPM 1, 2
                    IONIZER_2_FLOW_METER,   //LPM 3, 4
                    IONIZER_3_FLOW_METER,   //EQ
                    SPARE_77,    //SPARE(Analog Unit 겸용)
                    SPARE_78,    //SPARE
                    SPARE_79,    //SPARE

                    ROBOT_RETRACT_STATION_1,    //LPM #1 - 12
                    ROBOT_RETRACT_STATION_2,    //LPM #2 - 12
                    ROBOT_RETRACT_STATION_3,    //LPM #3 - 8
                    ROBOT_RETRACT_STATION_4,    //LPM #4 - 8
                    ROBOT_RETRACT_STATION_5,    //EQ 1 - 8 PLACE
                    ROBOT_RETRACT_STATION_6,    //EQ 1 - 8 PICK
                    ROBOT_RETRACT_STATION_7,    //EQ 1 - 12 PLACE
                    ROBOT_RETRACT_STATION_8,    //EQ 1 - 12 PICK

                    ROBOT_RETRACT_STATION_9,    //EQ 2 - 12 PLACE
                    ROBOT_RETRACT_STATION_10,   //EQ 2 - 12 PICK
                    ROBOT_LOWER_ARM_RETRACT,
                    ROBOT_UPPER_ARM_RETRACT,
                    ROBOT_MODE,
                    ROBOT_INITIALIZE_COMPLETE,
                    ROBOT_BUSY_STATUS,
                    ROBOT_ALARM_STATUS,

                    ROBOT_WAFER_ON_ARM_LOWER,
                    ROBOT_WAFER_ON_ARM_UPPER,
                    ROBOT_CONTROLLER_FAN_ALARM,
                    ROBOT_SERVO_ON_OFF_STATUS,
                    EFEM_EMS_STATUS,
                    PROTECTION_BAR_LP,
                    EFEM_DOOR_CLOSE,
                    AUTO_MANUAL_MODE,

                    FIRE_DETECTOR,
                    SPARE_105,
                    SPARE_106,
                    SPARE_107,
                    SPARE_108,
                    SPARE_109,
                    SPARE_110,
                    SPARE_111,

                    EQ_1_8_PLACE_READY,
                    EQ_1_8_PICK_READY,
                    EQ_1_12_PLACE_READY,
                    EQ_1_12_PICK_READY,
                    EQ_2_12_PLACE_READY,
                    EQ_2_12_PICK_READY,
                    EQ_1_8_PLACE_HANDSHAKE,
                    EQ_1_8_PICK_HANDSHAKE,

                    EQ_1_12_PLACE_HANDSHAKE,
                    EQ_1_12_PICK_HANDSHAKE,
                    EQ_2_12_PLACE_HANDSHAKE,
                    EQ_2_12_PICK_HANDSHAKE,
                    SPARE_124,    //SPARE
                    SPARE_125,    //SPARE
                    SPARE_126,    //SPARE
                    SPARE_127,    //SPARE
                }

                public enum EN_DIGITAL_OUT
                {
                    LP1_PIO_L_REQ = 0,
                    LP1_PIO_U_REQ,
                    LP1_PIO_ABORT,
                    LP1_PIO_READY,
                    LP1_SPARE5,
                    LP1_SPARE6,
                    LP1_SPARE7,
                    LP1_SPARE8,

                    LP2_PIO_L_REQ,
                    LP2_PIO_U_REQ,
                    LP2_PIO_ABORT,
                    LP2_PIO_READY,
                    LP2_SPARE5,
                    LP2_SPARE6,
                    LP2_SPARE7,
                    LP2_SPARE8,

                    LP3_PIO_L_REQ,
                    LP3_PIO_U_REQ,
                    LP3_PIO_ABORT,
                    LP3_PIO_READY,
                    LP3_SPARE5,
                    LP3_SPARE6,
                    LP3_SPARE7,
                    LP3_SPARE8,

                    LP4_PIO_L_REQ,
                    LP4_PIO_U_REQ,
                    LP4_PIO_ABORT,
                    LP4_PIO_READY,
                    LP4_SPARE5,
                    LP4_SPARE6,
                    LP4_SPARE7,
                    LP4_SPARE8,

                    IONIZER_1_On_OFF,   //LPM 1, 2
                    IONIZER_2_On_OFF,   //LPM 3, 5
                    IONIZER_3_On_OFF,   //EQ
                    SIGNAL_TOWER_RED,
                    SIGNAL_TOWER_YELLOW,
                    SIGNAL_TOWER_GREEN,
                    SIGNAL_TOWER_BUZZER,
                    EFEM_DOOR_OPEN_CLOSE,

                    ATM_ROBOT_HANDSHAKE_EQ_1_8_PLACE,
                    ATM_ROBOT_HANDSHAKE_EQ_1_8_PICK,
                    ATM_ROBOT_HANDSHAKE_EQ_1_12_PLACE,
                    ATM_ROBOT_HANDSHAKE_EQ_1_12_PICK,
                    ATM_ROBOT_HANDSHAKE_EQ_2_12_PLACE,
                    ATM_ROBOT_HANDSHAKE_EQ_2_12_PICK,
                    IONIZER_SOL,
                    SPARE_47,    //SPARE
                }
            }
        }
        namespace AnalogIO
        {
            namespace PWA500BIN
            {
                public enum EN_ANALOG_IN
                {
                    EFEM_MAIN_CDA_PRESSURE_SWITCH = 0,      // 0V ~ +5V				// SCALE RANGE (16bit) -10V ~ +10v
                    EFEM_MAIN_CDA_VACUUM_SWITCH,
                    ROBOT_CDA_PRESSURE_SWITCH,
                    IONIZER_PRESSURE_SWITCH,

                    IONIZER_1,      //LPM 1, 2, 3
                    IONIZER_2,      //LPM 4, 5, 6
                    IONIZER_3,      //EQ 1, 2
                    IONIZER_4,      //EQ 3, 4
                }
                public enum EN_ANALOG_OUT
                {
                    NONE = 0,       // 0V ~ 5V				// SCALE RANGE (16bit) -10V ~ +10V
                }
            }
            namespace PWA500W
            {
                public enum EN_ANALOG_IN
                {
                    EFEM_MAIN_CDA_PRESSURE_SWITCH = 0,      // 0V ~ +5V				// SCALE RANGE (16bit) -10V ~ +10v
                    EFEM_MAIN_CDA_VACUUM_SWITCH,
                    ROBOT_CDA_PRESSURE_SWITCH,
                    IONIZER_PRESSURE_SWITCH,

                    IONIZER_1,      //LPM 1, 2
                    IONIZER_2,      //LPM 3, 4
                    IONIZER_3,      //EQ
                    SPARE,
                }
                public enum EN_ANALOG_OUT
                {
                    NONE = 0,       // 0V ~ 5V				// SCALE RANGE (16bit) -10V ~ +10V
                }
            }
        }

        namespace Cylinder
        {
            public enum EN_CYLINDER
            {
                NONE = -1,

                SAMPLE = 0,
            }
        }

        namespace Motion
        {
            public enum EN_AXIS
            {
                NONE = -1,

                SAMPLE = 0,
            }
        }

        namespace Tool
        {
            public enum EN_WORK_TOOL
            {
            }
        }

        namespace SubSequence
        {
            public enum EN_MOTION_CONTROL_TYPE
            {
                ABSOLUTELY,
                RELEATIVELY,
                SPEED,
                OVERRIDE,
            }
            public enum EN_SUBSEQUENCE_RESULT
            {
                OK,
                WORKING,

                // Error 목록
                ERROR,
                ERROR_VISION,
                ERROR_TIMEOUT,
                ERROR_MOTION,
                ERROR_COMMAND,
                ERROR_ABNORMAL_CYLINDER_STATUS,
            }

            namespace Sample
            {
                public enum EN_SEQUENCE_BRANCH { }
            }
        }

        namespace Mail
        {
            public enum EN_SUBSCRIBER
            {
                Unknown,

                OPERATION_MAIN,
                OPERATION_PROCESS,
                OPERATION_IOMONITOR,
                SQUEEGEE_CALIBRATION,

                ProcessingScenario,
                ScenarioCirculator,
            }
            public enum EN_MAIL
            {
                INIT_MAIL,
                CHECK_MAIL,
                SendScenario,
                ScenarioCurculatorRun,
                ScenarioCurculatorUse,
                RecipeParameterChanged,

                UI_ShowMessageBox,
            }
        }

        namespace AppConfig
        {
            public enum KEY
            {
                MachineName,

                ControllerMotion,
                ControllerDigital,
                ControllerAnalog,

                BrakeAxis,
            }

            #region <Customer>
            public enum EN_CUSTOMER
            {
                NONE,
                S_TP,           // Samsung 전자 TP
                S_NRD,
                S_NRD_300
            }

            public enum EN_PROCESS_TYPE
            {
                NONE,
                BIN_SORTER,
                DIE_TRANSFER,
                DIE_TRANSFER_300,
            }

            public enum EN_SECSGEM_SPEC
            {
                SECSGEM,
                GEM300,
            }
            #endregion </Customer>

            #region <Device>
            public enum EN_ROBOT_CONTROLLER
            {
                NONE,
                QUADRA_ATM_ROBOT,
                NRC
            }
            public enum EN_LOADPORT_CONTROLLER
            {
                NONE,
                DURAPORT,
                SELOP8,
            }
            public enum EN_MOTION_CONTROLLER
            {
                NONE
            }

            public enum EN_DIGITAL_IO_CONTROLLER
            {
                NONE,
                CREVIS_MODBUS_TCP,
            }

            public enum EN_ANALOG_IO_CONTROLLER
            {
                NONE,
                CREVIS_MODBUS_TCP,
            }


            public enum EN_RFID_CONTROLLER
            {
                NONE,
                XEDION,
                CEYON,
            }
            public enum EN_PIO_INTERFACE_TYPE
            {
                NONE,
                E84,
                E23
            }
            #endregion </Device>
        }

        namespace Modbus
        {
            public enum EN_MODBUS_STATUS
            {
                WAITING_READ,
                SETTING_COMMAND,
            }

            public enum EN_MODBUS_SERVER_PROTOCOL
            {
                TCP,
                RTU_OVER_TCP
            }
        }
		
		namespace Common
		{
			public enum EN_UNKNOWN_DEVICE_TYPE
			{
				OPERATION = -1,
				CONFIG = -2,
				COMMON = -3,
				JOG = -4,
				MOTERSTATE = -5,
			}
            public enum EN_SYSTEM_ALARM
            {
                EMO_ALARM = 500,
            }
		}
    }
}