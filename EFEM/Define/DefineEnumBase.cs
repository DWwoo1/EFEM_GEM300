using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Define
{
	namespace DefineEnumBase
	{
		namespace Common
		{
			/// <summary>
			/// 2020.09.15 by yjlee [ADD] Enumerate the mode on running equipment.
			/// </summary>
			public enum EN_RUN_MODE
			{
				AUTO = 0,
				DRY_RUN,
				SIMULATION,
			}
			public enum EN_LANGUAGE
			{
				ENGLISH,
				KOREAN,
				CHINESE,
			}
			// 2021.09.26 by jhchoo [ADD] 알랑등급을 모든 코드에서 사용할 수 있도록 선언함
			public enum EN_ALARM_GRADE
			{
				NORMAL = 0,     // 평시
				NOTICE = 1,     // 알림
				WARNING = 2,    // 경고
				ERROR = 3,      // 에러
			}

			// 2022.06.05 by junho [ADD] 자주쓰는 용어 define
			public enum EN_LANE
			{
				NONE,

				FRONT,
				REAR,
			}
			public enum EN_OPEN_CLOSE
			{
				OPEN,
				CLOSE,
			}
			public enum EN_UP_DOWN
			{
				UP,
				DOWN,
			}
			public enum EN_HIGH_LOW_MIDDLE
			{
				HIGH,
				MIDDLE,
				LOW,
			}
			public enum EN_ON_OFF
			{
				ON,
				OFF,
			}
			public enum EN_TOP_BOTTOM
			{
				TOP,
				BOTTOM,
			}
			public enum EN_LEFT_RIGHT
			{
				LEFT,
				RIGHT,
			}
			public enum EN_TBLR
			{
				TOP,
				BOTTOM,
				LEFT,
				RIGHT,
			}
			public enum EN_UDLR
			{
				UP,
				DOWN,
				LEFT,
				RIGHT,
			}
			public enum EN_FRLR
			{
				FRONT,
				REAR,
				LEFT,
				RIGHT,
			}
			public enum EN_CORNER
			{
				LEFT_TOP,
				LEFT_BOTTOM,
				RIGHT_TOP,
				RIGHT_BOTTOM,
			}
			public enum EN_MATRIX
			{
				HORIZONTAL,
				VERTICAL,
			}
			public enum EN_GRIP_RELEASE
			{
				GRIP,
				RELEASE,
			}
			public enum EN_GRAB_RELEASE
			{
				GRAB,
				RELEASE,
			}
			public enum EN_LOCK_UNLOCK
			{
				LOCK,
				UNLOCK,
			}
			public enum EN_HOLD_RELEASE
			{
				HOLD,
				RELEASE,
			}
			public enum EN_CW_CCW
			{
				CW,
				CCW,
			}
			public enum EN_FORWARD_BACKWARD
			{
				FORWARD,
				BACKWARD,
			}
			public enum EN_RUN_STOP
			{
				RUN,
				STOP,
			}
			public enum EN_EXTEND_REDUCE
			{
				EXTEND,
				REDUCE,
			}
			public enum EN_TILT_FLAT
			{
				TILT,
				FLAT,
			}
			public enum EN_RESULT
			{
				WAIT,
				OK,
				NG,
				RETRY,
				SKIP,
				ERROR,
			}
			public enum EN_START_END
			{
				START,
				END,
			}
			public enum EN_MAINT_WORK
			{
				MAINT,
				WORK,
			}
			public enum EN_EXIST_EMPTY
			{
				Empty,
				Exist,
			}
			public enum EN_HORIZONTAL_VERTICAL
			{
				HORIZONTAL,
				VERTICAL
			}
            public enum EN_CONNECTION_TYPE
            {
                TCP,
                SERIAL
            }
		}

		namespace Initialize
		{
			/// <summary>
			/// 2020.05.12 by yjlee [ADD] Enumerate the step for the initializing.
			/// </summary>
			public enum EN_INITIALIZATION_STEP
			{
				INIT_START = 1,
				INIT_OBSERVER_START,
				INIT_OBSERVER_END,
				INIT_FILEIO_START,
				INIT_FILEIO_END,
				INIT_ACCOUNT_START,
				INIT_ACCOUNT_END,
				INIT_ALARM_START,
				INIT_ALARM_END,
				INIT_SOCKET_START,
				INIT_SOCKET_END,
				INIT_SERIAL_START,
				INIT_SERIAL_END,
				INIT_MOTION_START, // 2021.01.22. jhlim : 순서 바꿈
				INIT_MOTION_END,
                // 2023.12.29. jhlim [MOD] 모드버스는 TCP를 이용하므로 컨피그가 초기화된 이후 초기화해야한다.
                //INIT_ANALOG_IO_START,
                //INIT_ANALOG_IO_END,
                //INIT_DIGITAL_IO_START,
                //INIT_DIGITAL_IO_END,
				INIT_CYLINDER_START,
				INIT_CYLINDER_END,
				INIT_INTERRUPT_START,
				INIT_INTERRUPT_END,
				INIT_TRIGGER_START,
				INIT_TRIGGER_END,
				INIT_LANGUAGE_START,
				INIT_LANGUAGE_END,
                //INIT_VISION_START,
                //INIT_VISION_END,

                // 2021.08.14 by junho [ADD] FFU
                INIT_EXTERNAL_DEVICE_START,
                INIT_EXTERNAL_DEVICE_END,

                INIT_ANALOG_IO_START,
                INIT_ANALOG_IO_END,
                INIT_DIGITAL_IO_START,
                INIT_DIGITAL_IO_END,

				INIT_TASK_DEVICE_START,
				INIT_TASK_DEVICE_END,
				INIT_REGISTERED_INSTANCES_START,
				INIT_REGISTERED_INSTANCES_END,
				INIT_THREADTIMER_START,
				INIT_THREADTIMER_END,
				INIT_RECIPE_START,
				INIT_RECIPE_END,
				INIT_CONFIG_INSTANCES_START,
				INIT_CONFIG_INSTANCES_END,

                // 2023.12.28. jhlim [ADD]
                INIT_EFEM_MODULES_START,
                INIT_EFEM_MODULES_END,

                INIT_TASK_START,
				INIT_TASK_END,
				LOAD_RECIPE_START,
				LOAD_RECIPE_END,

				INIT_LOG_START,
				INIT_LOG_END,
				INIT_INTERLOCK_START,
				INIT_INTERLOCK_END,

                INIT_SCHEDULER_START,
                INIT_SCHEDULER_END,

				INIT_EFEM_MODULE_INFORMATION_START,
				INIT_EFEM_MODULE_INFORMATION_END,
				//INIT_WORKINFORMATION_START,
				//INIT_WORKINFORMATION_END,

                INIT_EQUIPMENT_PROPERTY_START,
                INIT_EQUIPMENT_PROPERTY_END,

                INIT_RAM_METRICS_START,
                INIT_RAM_METRICS_END,

                INIT_FTP_START,
                INIT_FTP_END,
				
				INIT_END,
			}
		}

		namespace ButtonEvent
		{
			/// <summary>
			/// 2020.02.05 by yjlee [ADD] Enumerate the names of the main menus.
			/// </summary>
			public enum EN_BUTTONEVENT_MAINMENU
			{
				OPERATION = 0,
				RECIPE,
				HISTORY,
				SETUP,
				CONFIG,
				USER,
				EXIT,
			}
		}

		namespace ThreadTimer
		{
			/// <summary>
			/// 2020.05.12 by yjlee [ADD] Enumerate the index for the thread timer.
			/// </summary>
			public enum EN_THREADTIMER_INDEX
			{
				THREADTIMER_INDEX_MAIN = 0,
				THREADTIMER_INDEX_OBSERVER,
				THREADTIMER_INDEX_FILEIO,
				THREADTIMER_INDEX_DIGITALIO,
				THERADTIMER_INDEX_ANALOGIO,
				THREADTIMER_INDEX_MOTION,
				THREADTIMER_INDEX_MOTION_GATHERING,
				THREADTIMER_INDEX_COMMUNICATION,
				THREADTIMER_INDEX_ETC_INSTANCES,
			}
		}

		namespace Storage
		{
			/// <summary>
			/// 2020.06.04 by yjlee [ADD] Enumerate the list of the storage.
			/// </summary>
			public enum EN_STORAGE_LIST
			{
				ALARM = 0,
				ACTION,
				ANALOG,
				ANALOG_LOOKUPTABLE,
				CYLINDER,
				DIGITALIO,
				TRIGGER,
				LANGUAGE,
				MOTION,
				MOTION_SPEED,
				SERIAL,
				SOCKET,
				INTERRUPT,
				FLOW,
				JOG,
				JOG_REVERSE,		// 2022.10.24 by junho [ADD] for jog reverse
				TASK,
				TASK_DEVICE,
				ACCOUNT,
				DYNAMIC_LINK,      // 2021.07.22 by jhchoo [ADD]
				PORT,              // 2021.07.22 by jhchoo [ADD]
				TOOL,			   // 2021.09.27 by jhchoo [ADD]
				INTERLOCK,         // 2023.03.22 by wdw [ADD]
				WCF,				// 2024.01.16 by jhlee [ADD]
			}
		}

		namespace Log
		{
			/// <summary>
			/// 2021.01.18 by yjlee [ADD] Enumerate the type of the function log.
			/// </summary>
			public enum EN_FNC_TYPE
			{
				START = 0,
				END,
			}
			/// <summary>
			/// 2021.01.18 by yjlee [ADD] Enumerate the type of the event log.
			/// </summary>
			public enum EN_LEH_TYPE
			{
				TRACK_IN = 0,
				TRACK_OUT,
				CST_IN,
				CST_OUT,
				LOT_IN,
				LOT_OUT,
				MERGE,
				SPLIT,
			}
			/// <summary>
			/// 2021.01.18 by yjlee [ADD] Enumerate the type of the process log.
			/// </summary>
			public enum EN_PRC_TYPE
			{
				START = 0,
				END,
			}
			/// <summary>
			/// 2021.01.18 by yjlee [ADD] Enumerate the type of the transfer log.
			/// </summary>
			public enum EN_XFR_TYPE
			{
				START = 0,
				END,
			}
			/// <summary>
			/// 2021.01.18 by yjlee [ADD] Enumerate the type of the alarm log.
			/// </summary>
			public enum EN_ALM_TYPE
			{
				OCCURRED = 0,
				RELEASED,
			}
			/// <summary>
			/// 2021.01.18 by yjlee [ADD] Enumerate the type of the configuration log.
			/// </summary>
			public enum EN_CFG_TYPE
			{
				SETTING = 0,
				CHANGE,
				SAVE,
			}
            /// <summary>
            /// 2021.08.18 by wdw [ADD] Enumerate the type of the communication log.
            /// </summary>
            public enum EN_COMMUNICATION_TYPE
            {
                SEND = 0,
                RECIEVE,
            }
		}

		namespace Interlock
		{
			public enum EN_INTERLOCK_DEVICE
			{
				MOTION,
				MOTION_RELATIVE,
				CYLINDER,
				DI,
				DO,
			}
			/// <summary>
			/// 2021.11.15 by wdw [ADD] Enumerate For Motion Compare Direction
			/// </summary>
			public enum EN_MOTION_COMPARE_DIRECTION
			{
				OVER,
				UNDER
			}

			/// <summary>
			/// 2021.11.15 by wdw [ADD] Enumerate For Cylinder Compare Direction
			/// </summary>
			public enum EN_CYLINDER_COMPARE_DIRECTION
			{
				FORWARD,
				BACKWARD
			}

			/// <summary>
			/// 2022.02.15 by wdw [ADD] Enumerate For Motion Compare Direction
			/// </summary>
			public enum EN_MOTION_MOVING_DIRECTION
			{
				BOTH,
				PLUS,
				MINUS
			}
		}

        namespace Component
        {
            public enum EN_PARAMETER_SETTING_TYPE
            {
                CALCULATE,
                KEYBOARD,
                COLOR,
                SELECT,
                TRUE_FALSE,
                FOLDER_DIALOG,
            }
        }
	}
}