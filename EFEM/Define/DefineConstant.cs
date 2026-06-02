using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Define
{
    namespace DefineConstant
    {
        #region Common
        /// <summary>
        /// 2020.06.19 by yjlee [ADD] Define the constants for the common string.
        /// </summary>
        public static class Common
        {
            public static readonly string NONE          = "NONE";
			public static readonly string SUCCESS		= "SUCCESS";
			public static readonly string FAIL			= "FAIL";
            public static readonly string UNKNOWN       = "UNKNOWN";

			public static readonly string DEFAULT_MIN	= "-1";
			public static readonly string DEFAULT_MAX	= "999999";
			// 2020.12.05 by jhchoo [ADD] Default Value
			public static readonly int DEFAULT_INT          = 0;
			public static readonly double DEFAULT_DOUBLE    = 0.0;
			public static readonly string DEFAULT_STRING    = string.Empty;

        }
        #endregion

        #region InitializationProgressForm
        /// <summary>
        /// 2020.05.13 [ADD] Define the constants to process the form.
        /// </summary>
        public static class InitializationProgressForm
        {
            public const int INTERVAL_CHECKING_QUEUE_OF_PROGRESS    = 50;
            public const int INTERVAL_CHECKING_INIT_STATE           = 1;
        }
        #endregion

        #region MainForm
        /// <summary>
        /// 2020.05.15 by yjlee [ADD] Define the constants for the main form.
        /// </summary>
        public static class MainForm
        {
			public const int INTERVAL_LOADING				= 15;
			public const int INTERVAL_GRAPH					= 10;
            public const int INTERVAL_UPDATE_GUI            = 200; // ms
        }
        #endregion

        #region Task
        /// <summary>
        /// 2020.06.02 by yjlee [ADD] Define the task.
        /// </summary>
        public static class Task
        {
            public const int TIMEOUT_SEQUENCE = 300000;//6000000; // ms -> 100min	// Sequence Time Out
        }
        #endregion

        #region Action
        /// <summary>
        /// 2020.06.15 by yjlee [ADD] Define the action for constants.
        /// </summary>
        public static class Action
        {
            public static readonly string ACTION_NONE            = Define.DefineConstant.Common.NONE;
        }
        #endregion

        #region FilePath
        /// <summary>
        /// 2020.02.05 by yjlee [ADD] Define the constants for the file paths.
        /// </summary>
        public static class FilePath
        {
            public static readonly string FILEPATH_EXE              = System.Environment.CurrentDirectory;

			// 2022.11.10 by junho [MOD] 상위폴더에 따로 저장되도록 변경
			//public static readonly string FILEPATH_LOG              = FILEPATH_EXE + "\\Log";
			//public static readonly string FILEPATH_RECIPE           = FILEPATH_EXE + "\\Recipe";
			//public static readonly string FILEPATH_WORK 			= FILEPATH_EXE + "\\Log\\Work";
            public static readonly string FILEPATH_LOG              = FILEPATH_EXE + "\\..\\Log";
            public static readonly string FILEPATH_RECIPE           = FILEPATH_EXE + "\\..\\Recipe";
			public static readonly string FILEPATH_WORK				= FILEPATH_LOG + "\\Work";
			public static readonly string FILEPATH_LOG_MAIN			= FILEPATH_LOG + "\\Main";

            public static readonly string FILEPATH_EXCEPTION_LOG    = FILEPATH_LOG + "\\Exception";
            public static readonly string FILEPATH_CONFIG           = FILEPATH_EXE + "\\Config";

			
            public static readonly string FILEPATH_RECOVERY 		= FILEPATH_EXE + "\\Recovery";
			public static readonly string FILEPATH_INTERPOLATION    = FILEPATH_EXE + "\\MotionInterpolation";
			// 2022.04.14 by jhchoo [ADD]
            public static readonly string FILEPATH_DISPENSER = FILEPATH_EXE + "\\Dispenser";
            public static readonly string FILEPATH_DISPENSER_STOCK = FILEPATH_DISPENSER + "\\Stock";

            public static readonly string FILEPATH_RAM_MATRICS = FILEPATH_LOG + "\\RAM_Metrics";

			public static readonly string FILEPATH_MANUALMAP		= "\\Manual_Map";
        }
        #endregion

        #region Process Name
        /// <summary>
        /// 2021.01.13 by yjlee [ADD] Define the name of the process.
        /// </summary>
        public static class ExternalProcess
        {
            public static readonly string PROCESS_NAME_LOG          = "System3Log";
        }
        #endregion End - Process Name

        #region FileName
        /// <summary>
        /// 2020.06.04 by yjlee [ADD] Define the constants for the file names.
        /// </summary>
        public static class FileName
        {
            public static readonly string FILENAME_CONFIG_ALARM         = "Alarm";
            public static readonly string FILENAME_CONFIG_ANALOGIO      = "AnalogIO";
			public static readonly string FILENAME_CONFIG_ANALOG_TABLE	= "AnalogLookupTable";
            public static readonly string FILENAME_CONFIG_CYLINDER      = "Cylinder";
            public static readonly string FILENAME_CONFIG_DIGITALIO     = "DigitalIO";
            public static readonly string FILENAME_CONFIG_TRIGGER       = "Trigger";
            public static readonly string FILENAME_CONFIG_LANGUAGE      = "Language";
            public static readonly string FILENAME_CONFIG_MOTION        = "Motion";
			public static readonly string FILENAME_CONFIG_MOTION_SPEED  = "MotionSpeedParameters";
            public static readonly string FILENAME_CONFIG_SERIAL        = "Serial";
            public static readonly string FILENAME_CONFIG_SOCKET        = "Socket";
            public static readonly string FILENAME_CONFIG_WCF           = "WCF"; // 2024.02.01 [ADD] by jhlee
            public static readonly string FILENAME_CONFIG_INTERRUPT     = "Interrupt";
			public static readonly string FILENAME_CONFIG_ACTION        = "Action";
			public static readonly string FILENAME_CONFIG_DYNAMIC_LINK  = "DynamcicLink";
			public static readonly string FILENAME_CONFIG_PORT			= "Port";
            public static readonly string FILENAME_CONFIG_FLOW          = "Flow";
			public static readonly string FILENAME_CONFIG_JOG			= "Jog";
			public static readonly string FILENAME_CONFIG_JOG_REVERSE	= "JogReverse";
            public static readonly string FILENAME_CONFIG_TASK   		= "Task";
            public static readonly string FILENAME_CONFIG_TASK_DEVICE	= "TaskDevice";
			public static readonly string FILENAME_CONFIG_ACCOUNT		= "Account";
            public static readonly string FILENAME_CONFIG_TOOL          = "Tool";
			public static readonly string FILENAME_CONFIG_INTERLOCK		= "Interlock";

            public static readonly string FILENAME_RECIPE_COMMON        = "common";
            public static readonly string FILENAME_RECIPE_EQUIPMENT     = "equipment";
			public static readonly string FILENAME_RECIPE_PROPERTY		= "property";
        }
        #endregion

        #region FileFormat
        /// <summary>
        /// 2020.07.03 by yjlee [ADD] Define the format for the files.
        /// </summary>
        public static class FileFormat
        {
            public static readonly string FILEFORMAT_CONFIG         = ".cfg";
            public static readonly string FILEFORMAT_PARAMETER      = ".prm";
            public static readonly string FILEFORMAT_LOG            = ".txt";
			public static readonly string FILEFORMAT_ZIP			= ".zip";
            public static readonly string FILEFORMAT_RECIPE         = ".rcp";
            public static readonly string FILEFORMAT_INTERPOLATION  = ".itp";
			public static readonly string FILEFORMAT_CSV			= ".csv";

            // 2020.11.29 by jhchoo [ADD] 파일 포맷 추가
            public static readonly string FILEFORMAT_RECOVERY = ".rcv";
            public static readonly string FILEFORMAT_MAPINFO = ".mapinfo";
            public static readonly string FILEFORMAT_MAPDATA = ".mapdata";
			// 2022.04.14 by jhchoo [ADD] For dispenser
			public static readonly string FILEFORMAT_PATTERN = ".pattern";
            public static readonly string FILEFORMAT_MODEL = ".model";
            public static readonly string FILEFORMAT_PARAM = ".param";
			
			// 2022.04.22. byu shkim. [ADD]  For dispenser
			public static readonly string FILEFORMAT_PROCESS_PARAM = ".processparam";
			public static readonly string FILEFORMAT_YIELD = ".yield";
			public static readonly string FILEFORMAT_XML = ".xml";
        }
        #endregion

        #region File IO
        /// <summary>
        /// 2020.06.04 by yjlee [ADD] Define the constants for the file IO.
        /// </summary>
        public static class FileIO
        {
            public const int TIMEOUT_READ               = 2000;
        }
        #endregion

        #region Motion
        /// <summary>
        /// 2020.05.12 by yjlee [ADD] Define the constants for the motion.
        /// </summary>
        public static class Motion
        {
            public const int INTERVAL_CHECKING_CONNECTION   = 1000; // ms

			#region INVERT & NO_INVERT
			public static readonly string INVERT_ON				= "INVERT";
			public static readonly string INVERT_OFF			= "NO_INVERT";
			#endregion			

			#region STOP_MODE
			public static readonly string STOPMODE_EMG			= "INSTANT_STOP";
			public static readonly string STOPMODE_DECEL		= "DECELERATION_STOP";
			#endregion

			#region LOGIC_ACTIVE
			public static readonly string LOGIC_ACTIVE_HIGH		= "ACTIVE_HIGH";
			public static readonly string LOGIC_ACTIVE_LOW		= "ACTIVE_LOW";
			#endregion

			#region DIRECTION
			public static readonly string DIRECTION_POSITIVE	= "POSITIVE(+)";
			public static readonly string DIRECITON_NEGATIVE	= "NEGATIVE(-)";
			#endregion
		}
        #endregion

        #region Vision
        /// <summary>
        /// 2020.05.12 by yjlee [ADD] Define the constants for the vision.
        /// </summary>
        public static class Vision
        {
            public const UInt32 INTERVAL_UPDATE = 1000;
            public const int COUNT_CAM          = 3;

            public const UInt32 TIMEOUT_RECEIVE = 1000;
        }
        #endregion

        #region ThreadTimerInterval
        /// <summary>
        /// 2020.05.12 by yjlee [ADD] Enumerate the interval of the thread timer.
        /// - unit : milliseconds.
        /// </summary>
         public static class ThreadTimerInterval
        {
            public const byte THREADTIMER_INTERVAL_MAIN             = 1;
            public const byte THREADTIMER_INTERVAL_OBSERVER         = 1;
            public const byte THREADTIMER_INTERVAL_FILEIO           = 1;
            public const byte THREADTIMER_INTERVAL_DIGITALIO        = 1;
            public const byte THREADTIMER_INTERVAL_ANALOGIO         = 1;
            public const byte THREADTIMER_INTERVAL_MOTION           = 1;
            public const byte THREADTIMER_INTERVAL_MOTION_GATHERING = 1;
            public const byte THREADTIMER_INTERVAL_COMMUNICATION    = 1;
            public const byte THREADTIMER_INTERVAL_ETC_INSTANCES    = 1;
            public const byte THREADTIMER_INTERVAL_LOGGING          = 5;
        }
        #endregion

		#region SelectionList
		/// <summary>
		/// 2020.06.05 by twkang [ADD]  Define the constants for the SelectionList.
		/// </summary>
		public static class SelectionList
		{
			#region ENABLE & DISABLE
			public static readonly string ENABLE		= "ENABLE";
			public static readonly string DISABLE		= "DISABLE";
			#endregion

			#region TRUE & FALSE
			public static readonly string TRUE			= "TRUE";
			public static readonly string FALSE			= "FALSE";
			#endregion
			
		}
		#endregion

        #region Message
        /// <summary>
        /// 2020.10.12 by yjlee [ADD] Define the constants for the message.
        /// </summary>
        public static class Message
        {
            public static readonly string MESSAGE_TOKEN = "$T";
        }
        #endregion
    }
}
