using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Threading;
using System.IO;

using Socket_;
using Serial_;
using Motion_;
using Cylinder_;
using DigitalIO_;
using AnalogIO_;

using Define.DefineEnumProject.Common;
using Define.DefineEnumBase.Log;
using Define.DefineConstant;

namespace FrameOfSystem3.Log
{
    /// <summary>
    /// 2021.01.15 by yjlee [ADD] Write and Read a log.
    /// </summary>
	public class LogManager
    {
        private const int SIZE_QUEUE                = 1 << 13;  // 8096
        public const string EMPTY_DATA = "$";

        #region Singleton
        private LogManager()
		{
		}
		private static LogManager m_instance        = new LogManager();
        public static LogManager GetInstance() { return m_instance; }
        #endregion

		#region Constants
		private readonly string FILE_NAME_TEMP_LOG	= "TemporaryLog";

		private const byte PREFIX					= 0x02;
		private const byte SUFFIX					= 0x03;

		private const byte PREFIX_REPLACE			= 0x5B;
		private const byte SUFFIX_REPLACE			= 0x5D;
		#endregion

		#region Variables
		private bool m_bInit                        = false;

        #region Instance
        private Socket m_instanceSocket             = null;
        private Recipe.Recipe m_instanceRecipe      = null;
        #endregion End - Instance

        #region Socket
        private int m_nIndexOfSocket                        = -1;
        #endregion End - Socket

        #region Delegate for writing log
        private DelegateWritingSocketLog m_dgWriteSocket            = null;
        private DelegateWritingSerialLog m_dgWriteSerial            = null;
        private DelegateWritingMotionLog m_dgWriteMotion            = null;
        private DelegateWritingCylinderLog m_dgWriteCylinder        = null;
        private DelegateWritingDigitalOutLog m_dgWriteDigitalOut    = null;
        private DelegateWritingAnalogOutLog m_dgWriteAnalogOut      = null;
        #endregion End - Delegate for writing log

        #region Mapping
        private string[] m_arMappingCFG             = null;
        private string[] m_arMappingXFR             = null;
        private string[] m_arMappingALM             = null;
        private string[] m_arMappingLEH             = null;
        private string[] m_arMappingPRC             = null;

        private Dictionary<int, string> dicForDiviceName = new Dictionary<int, string>();
        #endregion End - Mapping

        #region Logging
        private Timer m_timerForLogging             = null;
        private Queue<string> m_queueForBuffer      = new Queue<string>(SIZE_QUEUE);

        private Dictionary<string ,string> dicActNameToDeviceId = new Dictionary<string, string>();

		#region TempLogging
		private bool m_bChageSocketStatus			= false;
		private string m_strFullFilePath			= string.Empty;
		#endregion

		#endregion End - Logging

		#region Threading
		private ReaderWriterLockSlim m_rwLockForLogging = new ReaderWriterLockSlim();
        #endregion End - Threading

        private string _materialType = EMPTY_DATA;

        public delegate string DeleGetLotId();
        private DeleGetLotId _funcGetLotId = null;
        private string LotId
        {
            get 
            {
                if (_funcGetLotId == null)
                    return EMPTY_DATA;
                
                string r = _funcGetLotId();
                return string.IsNullOrWhiteSpace(r) ? EMPTY_DATA : r;
            }
        }

        public delegate string DeleGetMaterialIdFromTaskName(string taskName);
        private DeleGetMaterialIdFromTaskName _funcGetMaterialIdFromTaskName = null;
        private string GetMaterialId(string taskName)
        {
            if (_funcGetMaterialIdFromTaskName == null)
                return EMPTY_DATA;
            
            string result = _funcGetMaterialIdFromTaskName(taskName);
			return string.IsNullOrWhiteSpace(result) ? EMPTY_DATA : result;
		}
        #endregion /Variables

        #region Internal Interface

        #region Init
        /// <summary>
        /// 2021.01.18 by yjlee [ADD] Make the mapping arraies for the performance.
        /// </summary>
        private void MakeMappingArraies()
		{
			#region TempLogging
			m_strFullFilePath	= FilePath.FILEPATH_LOG + "\\" + FILE_NAME_TEMP_LOG + FileFormat.FILEFORMAT_LOG;
			#endregion

			#region XFR
			m_arMappingXFR      = new string[Enum.GetValues(typeof(EN_XFR_TYPE)).Length];

            foreach(EN_XFR_TYPE enType in Enum.GetValues(typeof(EN_XFR_TYPE)))
            {
                m_arMappingXFR[(int)enType]     = enType.ToString();
            }
            #endregion End - XFR

            #region CFG
            m_arMappingCFG      = new string[Enum.GetValues(typeof(EN_CFG_TYPE)).Length];

            foreach(EN_CFG_TYPE enType in Enum.GetValues(typeof(EN_CFG_TYPE)))
            {
                m_arMappingCFG[(int)enType]     = enType.ToString();
            }
            #endregion End - CFG

            #region ALM
            m_arMappingALM      = new string[Enum.GetValues(typeof(EN_ALM_TYPE)).Length];

            foreach(EN_ALM_TYPE enType in Enum.GetValues(typeof(EN_ALM_TYPE)))
            {
                m_arMappingALM[(int)enType]     = enType.ToString();
            }
            #endregion End - ALM

            #region LEH
            m_arMappingLEH      = new string[Enum.GetValues(typeof(EN_LEH_TYPE)).Length];

            foreach(EN_LEH_TYPE enType in Enum.GetValues(typeof(EN_LEH_TYPE)))
            {
                m_arMappingLEH[(int)enType]     = enType.ToString();
            }
            #endregion End - LEH

            #region PRC
            m_arMappingPRC      = new string[Enum.GetValues(typeof(EN_PRC_TYPE)).Length];

            foreach(EN_PRC_TYPE enType in Enum.GetValues(typeof(EN_PRC_TYPE)))
            {
                m_arMappingPRC[(int)enType]     = enType.ToString();
            }
            #endregion End - PRC
        }
        /// <summary>
        /// 2021.01.19 by yjlee [ADD] Set the delegate function.
        /// </summary>
        private void SetDelegateFunction()
        {
            m_dgWriteSocket     = new DelegateWritingSocketLog(WriteSocketLog);
            m_instanceSocket.SetLogFunction(ref m_dgWriteSocket);

            m_dgWriteSerial     = new DelegateWritingSerialLog(WriteSerialLog);
            Serial.GetInstance().SetLogFunction(ref m_dgWriteSerial);

            m_dgWriteMotion = new DelegateWritingMotionLog(WriteMotionLog);
            Motion_.Motion.GetInstance().SetLogFunction(ref m_dgWriteMotion);

            m_dgWriteCylinder   = new DelegateWritingCylinderLog(WriteCylinderLog);
            Cylinder.GetInstance().SetLogFunction(ref m_dgWriteCylinder);

            m_dgWriteDigitalOut = new DelegateWritingDigitalOutLog(WriteDigitalOutputLog);
            DigitalIO.GetInstance().SetLogFunction(ref m_dgWriteDigitalOut);

            m_dgWriteAnalogOut  = new DelegateWritingAnalogOutLog(WriteAnalogOutputLog);
            AnalogIO.GetInstance().SetLogFunction(ref m_dgWriteAnalogOut);
        }
        /// <summary>
        /// 2024.08.31 by jhshin [ADD] index를 이름으로 변환하기 위한 Dictionary 초기화
        /// </summary>
        private void MakeMappingDiviceDictionary()
        {
            string[] arTaskName = null;
            int[] arInstanceOfTask = null;

            if(FrameOfSystem3.Config.ConfigTask.GetInstance().GetListOfTask(ref arTaskName) && 
                FrameOfSystem3.Config.ConfigTask.GetInstance().GetListOfIndexOfInstance(ref arInstanceOfTask))
            {
                for(int i=0; i<arInstanceOfTask.Length; i++)
                {
                    dicForDiviceName[arInstanceOfTask[i]] = arTaskName[i];
                }
                
            }

//             foreach(EN_UNKNOWN_DEVICE_TYPE type in Enum.GetValues(typeof(EN_UNKNOWN_DEVICE_TYPE)))
//             {
//                 dicForDiviceName[(int)type] = type.ToString();
//             }
        }
        #endregion End - Init

        #region Socket
        /// <summary>
        /// 2021.01.15 by yjlee [ADD] Initialize the socket.
        /// </summary>
        private bool InitSocket()
        {
            m_instanceSocket        = Socket.GetInstance();
            m_nIndexOfSocket        = (int)Define.DefineEnumProject.Socket.EN_SOCKET_INDEX.LOG;

            m_instanceRecipe        = Recipe.Recipe.GetInstance();
            return m_instanceSocket.Connect(m_nIndexOfSocket);
        }
        #endregion End - Socket

        #region Time
        /// <summary>
        /// 2021.01.15 by yjlee [ADD] Get the current time.
        /// </summary>
        private string GetCurrentTime()
        {
            return DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss.fff");
        }
        #endregion

        #region Recipe
        /// <summary>
        /// 2021.01.18 by yjlee [ADD] Get the current process recipe name.
        /// </summary>
        private string GetCurrentProcessRecipe()
        {
            string strFileName      = string.Empty;

			m_instanceRecipe.GetProcessFileNameWithoutExtension(ref strFileName);

            return strFileName;
        }
        #endregion

        #region Logging
        /// <summary>
        /// 2021.01.15 by yjlee [ADD] Write the log message.
        /// </summary>
        private void WriteLog(ref string strMessage)
        {
            m_rwLockForLogging.EnterWriteLock();

            m_queueForBuffer.Enqueue(strMessage);

            m_rwLockForLogging.ExitWriteLock();
        }
        /// <summary>
        /// 2021.01.15 by yjlee [ADD] Send the log message through the socket.
        /// </summary>
        private void SendLog()
        {
            Queue<string> tempQueue     = new Queue<string>();

            m_rwLockForLogging.EnterWriteLock();

            while (m_queueForBuffer.Count > 0)
            {
				byte[] arMessage		= null;

				string strMessage       = m_queueForBuffer.Dequeue();

				//char[] chMessage        = strMessage.ToCharArray();

				// 2021.05.18 by twkang [ADD] 한글표시위해 Encoding 방식 변경
				//byte[] arMessage        = System.Text.Encoding.Unicode.GetBytes(chMessage);
				//byte[] arMessage        = System.Text.Encoding.ASCII.GetBytes(chMessage);

				ReplaceFixkey(strMessage, ref arMessage);
                
                int nLegnth             = arMessage.Length;
                byte[] arData           = new byte[nLegnth + 2];        // STX, ETX
                
                for(int nIndex = 0 ; nIndex < nLegnth ; ++ nIndex)
                {
                    arData[nIndex + 1]      = arMessage[nIndex];
                }
                
                arData[0]           = PREFIX; // Prefix, STX
                arData[nLegnth + 1] = SUFFIX; // Suffix, STX

                // 2021.01.18 by yjlee [ADD] Failed to send the log data.
                if(false == m_instanceSocket.Send(m_nIndexOfSocket, arData))
                {
                    tempQueue.Enqueue(strMessage);
                }
            }

            while(tempQueue.Count > 0)
            {
                m_queueForBuffer.Enqueue(tempQueue.Dequeue());
            }

            m_rwLockForLogging.ExitWriteLock();
        }
        /// <summary>
        /// 2021.01.15 by yjlee [ADD] Write the log data through the socket communication.
        /// </summary>
        private void WriteLogThroughSocket(object sender)
        {
            switch(m_instanceSocket.GetState(m_nIndexOfSocket))
            {
                case SOCKET_ITEM_STATE.CONNECTED:
                case SOCKET_ITEM_STATE.TIMEOUT_RECEIVE:
                case SOCKET_ITEM_STATE.WAITING_RECEIVE:
					if(m_bChageSocketStatus && CheckTemporaryLogFile(m_strFullFilePath))
					{
						m_bChageSocketStatus	= false;

						LoadTemporaryLog();
					}
                    SendLog();
                    break;
					
                default:
					SaveLogTemporary();
					m_bChageSocketStatus	= true;
                    break;
            }
        }

		/// <summary>
		/// 2021.06.04 by twkang [ADD] 임시 로그파일이 있는지 체크
		/// </summary>
		private bool CheckTemporaryLogFile(string strFullFilePath)
		{
			return System.IO.File.Exists(strFullFilePath);
		}

		/// <summary>
		/// 2021.06.04 by twkang [ADD] 임시 로그파일을 불러와서 버퍼에 넣어준다.
		/// </summary>
		private void LoadTemporaryLog()
		{
			string strData	= null;

			if(false == FileIOManager_.FileIOManager.GetInstance().Read(FilePath.FILEPATH_LOG
				, FILE_NAME_TEMP_LOG + FileFormat.FILEFORMAT_LOG
				, ref strData
				, FileIO.TIMEOUT_READ))
				return;

			string[] arData	= strData.Split('\n');

			m_rwLockForLogging.EnterWriteLock();

			for(int nIndex = 0, nEnd = arData.Length; nIndex < nEnd; ++nIndex)
			{
				if(string.Empty == arData[nIndex])
					continue;

				m_queueForBuffer.Enqueue(arData[nIndex]);
			}

			m_rwLockForLogging.ExitWriteLock();

			if(CheckTemporaryLogFile(m_strFullFilePath))
				File.Delete(m_strFullFilePath);
		}

		/// <summary>
		/// 2021.06.04 by twkang [ADD] 로그를 파일로 임시저장 해둔다
		/// </summary>
		private void SaveLogTemporary()
		{
			System.IO.DirectoryInfo dirInfo = new System.IO.DirectoryInfo(FilePath.FILEPATH_LOG);
			
			// 해당 경로의 폴더가 존재하지 않을 경우
			if (false == dirInfo.Exists)
			{
				dirInfo.Create();
			}

			m_rwLockForLogging.EnterWriteLock();

			while(m_queueForBuffer.Count > 0)
			{
				string strData	= m_queueForBuffer.Dequeue() + "\n";

				FileIOManager_.FileIOManager.GetInstance().Write(FilePath.FILEPATH_LOG
				, FILE_NAME_TEMP_LOG + FileFormat.FILEFORMAT_LOG
				, strData
				, true
				, false);
			}

			m_rwLockForLogging.ExitWriteLock();
		}

		/// <summary>
		/// 2021.08.17 by twkang [ADD] Message 내용에 PreFix 또는 SuffIx 를 제거
		/// </summary>
		private void ReplaceFixkey(string strMessage, ref byte[] arData)
		{
			if (strMessage == string.Empty)
				return;

			char[] chMessage        = strMessage.ToCharArray();

			Queue<char> queueForTemp	= new Queue<char>();

			for (int nIndex = 0, nEnd = chMessage.Length; nIndex < nEnd; ++nIndex)
			{
				// Prefix, Suffix 와 동일한 문자는 누락시킨다.

				switch((byte)chMessage[nIndex])
				{
					case PREFIX:
						queueForTemp.Enqueue((char)PREFIX_REPLACE);
						break;
					case SUFFIX:
						queueForTemp.Enqueue((char)SUFFIX_REPLACE);
						break;
					default:
						queueForTemp.Enqueue(chMessage[nIndex]);
						break;
				}
			}

			chMessage = queueForTemp.ToArray();

			// 2021.05.18 by twkang [ADD] 한글표시를 위하여 ASCII 가 아닌 Unicode 방식을 사용
			// 2021.09.06 by twkang [MOD] UNICODE -> UTF8
			arData = System.Text.Encoding.UTF8.GetBytes(chMessage);
		}

        #region FNC
        /// <summary>
        /// 2021.01.20 by yjlee [ADD] Write the log message about controlling the motion.
        /// 2024.09.05 by jhshin [MOD] log의 data부분을 클래스로 전달하도록 변경
        /// </summary>
        private void WriteMotionLog(int deviceID, string axisName, string actionName, bool bStart, ref MotionLogDataValues dataValues)
        {
            string strStatus = bStart ? "START" : "END";

            axisName = axisName.Replace(" ", "_");
            string eventID = string.IsNullOrEmpty(actionName) ? axisName : actionName + "_" + axisName;

            string strDeviceID = axisName;  // deviceID가 없으면 axis name을 deviceID로 사용
            string materialID = EMPTY_DATA;
            if (dicForDiviceName.ContainsKey(deviceID))
            {
                strDeviceID = dicForDiviceName[deviceID];
                materialID = GetMaterialId(strDeviceID);
            }

            string strMessage = string.Format("{0} '{1}' 'FNC' '{2}' '{3}' '{4}' '{5}' ('TYPE', 'MOTOR') ('ENCODER', {6:F4}) ('VEL', {7:F4}) ('ACC', {8:F4}) ('LOT_ID', '{9}')"
                                , GetCurrentTime()                  // 0
                                , strDeviceID                       // 1    VisionHead
                                , eventID                           // 2    ALIGN_AXIS_FRONT_STAGE_Z
                                , strStatus                         // 3    START or END
                                , materialID                        // 4    WaferID
                                , _materialType                      // 5    
                                , dataValues.EncoderDestination     // 6    
                                , dataValues.Velocity               // 7
                                , dataValues.Acceleration           // 8
                                , LotId);                           // 9    

            if (dicForDiviceName.ContainsKey(deviceID) == true)
                strMessage += string.Format(" ('ACT_NAME', '{0}')", axisName);
            if (dataValues.UseDestination)
                strMessage += string.Format(" ('POS', {0:F4})", dataValues.Destination);
            if (dataValues.CaptionRetried > 0)
                strMessage += string.Format(" ('RETRY', {0})", dataValues.CaptionRetried);


            WriteLog(ref strMessage);
        }
        //         private void WriteMotionLog(int deviceId, string strID, MOTION_LOG_TYPE enMotionType, bool bStart, double dblEncoderPosition, double dblDestination, double dblVelocity)
        //         {
        //             string strMessage = string.Empty;
        //             string strMotionType = enMotionType.ToString();
        //             string strStatus = bStart ? "START" : "END";
        // 
        //             string strDeviceId = dicForDiviceName[deviceId];
        //             string lotId = Work.WorkInformation.GetInstance().LotId;
        //             lotId = string.IsNullOrEmpty(lotId) ? EMPTY_DATA : lotId;
        // 
        //             switch (enMotionType)
        //             {
        //                 case MOTION_LOG_TYPE.SPEED:
        //                 case MOTION_LOG_TYPE.HOMING:
        //                 case MOTION_LOG_TYPE.STOP:
        //                     strMessage = string.Format("{0} '{1}' 'FNC' '{2}' '{3}' ('TYPE', 'MOTOR') ('ENCODER', {4:F4}) ('VELOCITY', {5}) ('ACT_NAME', {6}) ('LOT_ID', {7})"
        //                                         , GetCurrentTime()
        //                                         , strDeviceId			// Device ID	(FrontStage...)
        //                                         , strMotionType			// EVENT ID		(RELEATIVE...)
        //                                         , strStatus				// START, END
        //                                         , dblEncoderPosition	// DATA
        //                                         , dblVelocity
        //                                         , strID                 // Actuator Name (FRONT STAGE Z...)
        //                                         , lotId);
        //                     break;
        // 
        //                 default:
        //                     strMessage = string.Format("{0} '{1}' 'FNC' '{2}' '{3}' ('TYPE', 'MOTOR') ('ENCODER', {4}) ('POSITION', {5}) ('VELOCITY', {6}) ('ACT_NAME', {7}) ('LOT_ID', {8})"
        //                                         , GetCurrentTime()
        //                                         , strDeviceId
        //                                         , strMotionType
        //                                         , strStatus
        //                                         , dblEncoderPosition
        //                                         , dblDestination
        //                                         , dblVelocity
        //                                         , strID
        //                                         , lotId);
        //                     break;
        //             }
        // 
        //             WriteLog(ref strMessage);
        //         }
        /// <summary>
        /// 2021.01.20 by yjlee [ADD] Write the log message about controlling the cylinder.
        /// </summary>
        private void WriteCylinderLog(int deviceID, string moduleName, string actionName, double delayTime, bool isStart, bool isForward, int retry)
        {
            moduleName = moduleName.Replace(" ", "_");
            string eventID = string.IsNullOrEmpty(actionName) ? moduleName : actionName + "_" + moduleName;

            string strDeviceID = moduleName; 
            string meterialID = EMPTY_DATA;
            if (dicForDiviceName.ContainsKey(deviceID))
            {
                strDeviceID = dicForDiviceName[deviceID];
                meterialID = GetMaterialId(strDeviceID);
            }

            string strMessage   = string.Format("{0} '{1}' 'FNC' '{2}' '{3}' '{4}' '{5}' ('TYPE', 'CYLINDER') ('DIRECTION', '{6}') ('DELAYTIME', {7}) ('ACT_NAME', '{8}') ('LOT_ID', '{9}')"
                                        , GetCurrentTime()                              // 0
                                        , strDeviceID                                   // 1
                                        , eventID                                       // 2
                                        , isStart ? "START" : "END"                     // 3
                                        , meterialID                                    // 4
                                        , _materialType                                  // 5
                                        , isForward ? "FORWARD" : "BACKWARD"            // 6  FORWARD or BACKWARD
                                        , delayTime                                     // 7
                                        , moduleName                                    // 8
                                        , LotId);                                       // 9

            if (retry > 0)
                strMessage += string.Format(" ('RETRY', {0})", retry);

            WriteLog(ref strMessage);
        }

        //         private void WriteCylinderLog(string strID, string strType, bool bStart, int nDelayTime, int deviceId)
        //         {
        //             string strDeviceId = dicForDiviceName[deviceId];
        //             string lotId = Work.WorkInformation.GetInstance().LotId;
        //             lotId = string.IsNullOrEmpty(lotId) ? EMPTY_DATA : lotId;
        // 
        //             string strMessage = string.Format("{0} '{1}' 'FNC' '{2}' '{3}' ('TYPE', 'CYLINDER') ('DELAYTIME', {4}) ('ACT_NAME', '{5}') ('LOT_ID', '{6}')"
        //                                         , GetCurrentTime()
        //                                         , strDeviceId
        //                                         , strType
        //                                         , bStart ? "START" : "END"
        //                                         , nDelayTime
        //                                         , strID
        //                                         , lotId);
        // 
        //             WriteLog(ref strMessage);
        //         }

        //         class AnalogOuputLogDataValues
        //         {
        //             // header
        //             int deviceID;
        //             string actionName;
        // 
        //             // data
        //             string moduleName;
        //             string tagID;
        //             double voltage;
        //             int retry;
        // 
        //         }
        /// <summary>
        /// 2021.01.21 by yjlee [ADD] Write the analog output log.
        /// </summary>
        //private void WriteAnalogOutputLog(string strID, string strTagID, double dblVoltage, int deviceId)
        private void WriteAnalogOutputLog(int deviceID, string moduleName, string actionName, string strTagID, double dblVoltage, int retry)
       {
            moduleName = moduleName.Replace(" ", "_");
            string eventID = string.IsNullOrEmpty(actionName) ? moduleName : actionName + "_" + moduleName;

            string strDeviceID = moduleName;
            string meterialID = EMPTY_DATA;
            if (dicForDiviceName.ContainsKey(deviceID))
            {
                strDeviceID = dicForDiviceName[deviceID];
                meterialID = GetMaterialId(strDeviceID);
            }

            string strMessage = string.Format("{0} '{1}' 'FNC' '{2}' '{3}' '{4}' ('TYPE', 'ANALOG') ('{5}', '{6}') ('ACT_NAME', '{7}') ('LOT_ID', '{8}')"
                                        , GetCurrentTime()
                                        , strDeviceID
                                        , eventID
                                        , meterialID
                                        , _materialType
                                        , strTagID
                                        , dblVoltage
                                        , moduleName
                                        , LotId);
            if (retry > 0)
                strMessage += string.Format(" ('RETRY', {0})", retry);

            WriteLog(ref strMessage);
        }
        //         private void WriteAnalogOutputLog(string strID, string strTagID, double dblVoltage, int deviceId)
        //         {
        //             string strDeviceId = dicForDiviceName[deviceId];
        //             string lotId = Work.WorkInformation.GetInstance().LotId;
        //             lotId = string.IsNullOrEmpty(lotId) ? EMPTY_DATA : lotId;
        // 
        //             string strMessage = string.Format("{0} '{1}' 'FNC' ('TYPE', 'ANALOG') ('{2}', '{3}') ('ACT_NAME', '{4}') ('LOT_ID', '{5}')"
        //                                         , GetCurrentTime()
        //                                         , strDeviceId
        //                                         , strTagID
        //                                         , dblVoltage
        //                                         , strID
        //                                         , lotId);
        // 
        //             WriteLog(ref strMessage);
        //         }

        /// <summary>
        /// 2021.01.21 by yjlee [ADD] Write the digital output log.
        /// </summary>
        private void WriteDigitalOutputLog(int deviceID, string moduleName, string actionName, string strTagID, bool bOn, bool bStart, int retry)
        {
            moduleName = moduleName.Replace(" ", "_");
            string eventID = string.IsNullOrEmpty(actionName) ? moduleName : actionName + "_" + moduleName;

            string strDeviceID = moduleName;
            string meterialID = EMPTY_DATA;
            if (dicForDiviceName.ContainsKey(deviceID))
            {
                strDeviceID = dicForDiviceName[deviceID];
                meterialID = GetMaterialId(strDeviceID);
            }

            // 2025.03.31 by junho [MOD] Digital io log에 (‘DURATION’, ‘NO’) 항목이 함께 남도록 변경
            //string strMessage   = string.Format("{0} '{1}' 'FNC' '{2}' '{3}' '{4}' '{5}' ('TYPE', 'SENSOR') ('{6}', '{7}') ('ACT_NAME', '{8}') ('LOT_ID', '{9}')"
            string strMessage = string.Format("{0} '{1}' 'FNC' '{2}' '{3}' '{4}' '{5}' ('TYPE', 'SENSOR') ('{6}', '{7}') ('ACT_NAME', '{8}') ('LOT_ID', '{9}') ('DURATION', 'NO')"
                                        , GetCurrentTime()                  // 0
                                        , strDeviceID                       // 1
                                        , eventID                           // 2
                                        , bStart ? "START" : "END"          // 3
                                        , meterialID                        // 4
                                        , _materialType                     // 5
                                        , strTagID                          // 6
                                        , bOn==true ? "ON" : "OFF"          // 7
                                        , moduleName                        // 8
                                        , LotId);                           // 9

            if (retry > 0)
                strMessage += string.Format(" ('RETRY', {0})", retry);


            WriteLog(ref strMessage);
        }
        #endregion End - FNC

        #region COMM
        /// <summary>
        /// 2021.01.19 by yjlee [ADD] Write the socket log.
        /// </summary>
        private void WriteSocketLog(string strID, PROTOCOL_TYPE enType, bool bSend, string strData)
        {
            string strMessage       = string.Empty;
            string strType          = string.Empty;

            switch(enType)
            {
                case PROTOCOL_TYPE.UDP:
                    strType         = "UDP/IP";
                    break;

                case PROTOCOL_TYPE.TCP_SERVER:
                case PROTOCOL_TYPE.TCP_CLIENT:
                    strType         = "TCP/IP";
                    break;
            }
            
            strMessage          = string.Format("{0} '{1}' 'COMM' '{2}' ('TYPE', '{3}') ('DATA', '{4}')"
                                        , GetCurrentTime()
                                        , strID
                                        , bSend ? "SEND" : "RECV"
                                        , strType
                                        , strData);

            // 2025.03.10 by jaewoo [ADD] 표준로그로 인한 줄바꿈 제거 
            strMessage = strMessage.Replace("\n", "");
            strMessage = strMessage.Replace("\r", "");
            strMessage = strMessage.Replace("\x03", "");
            WriteLog(ref strMessage);
        }
        /// <summary>
        /// 2021.01.19 by yjlee [ADD] Write the socket log.
        /// </summary>
        private void WriteSerialLog(string strID, bool bSend, string strData)
        {
            string strMessage       = string.Format("{0} '{1}' 'COMM' '{2}' ('TYPE', 'SERIAL') ('DATA', '{3}')"
                                        , GetCurrentTime()
                                        , strID
                                        , bSend ? "SEND" : "RECV"
                                        , strData);

            strMessage = strMessage.Replace("\n", "");
            strMessage = strMessage.Replace("\r", "");
            strMessage = strMessage.Replace("\x03", "");
            WriteLog(ref strMessage);
        }
		public void WriteWcfLog(string strID, bool bSend, string data)
		{
            string strMessage = string.Format("{0} '{1}' 'COMM' '{2}' ('TYPE', 'WCF') ('DATA', '{3}')"
                                       , GetCurrentTime()
                                       , strID
                                       , bSend ? "SEND" : "RECV"
                                       , data);

            strMessage = strMessage.Replace("\n", "");
            strMessage = strMessage.Replace("\r", "");
            strMessage = strMessage.Replace("\x03", "");
            WriteLog(ref strMessage);
		}
        #endregion End - COMM

        #endregion End - Logging

        #endregion End - Internal Interface

        #region External Interface

        #region Init & Exit
        /// <summary>
        /// 2021.01.15 by yjlee [ADD] Initialize the instances for the log.
        /// </summary>
        public bool Init()
        {
            bool bReturnValue       = InitSocket();

            SetDelegateFunction();

            m_timerForLogging       = new Timer(WriteLogThroughSocket, null, 0, ThreadTimerInterval.THREADTIMER_INTERVAL_LOGGING);

            MakeMappingArraies();

            MakeMappingDiviceDictionary();

			/// 시작시 True 설정
			m_bChageSocketStatus	= true;

            m_bInit                 = true;

            return bReturnValue;
        }
        /// <summary>
        /// 2021.01.15 by yjlee [ADD] Release the resources of the class.
        /// </summary>
        public void Exit()
        {
            m_timerForLogging.Dispose();

            SendLog();

            m_bInit             = false;
        }
        #endregion End - Init & Exit

        #region Logging

        #region FNC (Function)
        #endregion End - FNC (Function)

        #region XFR (Transfer)
        /// <summary>
        /// 2021.01.18 by yjlee [ADD] Write the transfer log.
        /// jhshin
        /// </summary>
        public void WriteTransferLog(string strID, EN_XFR_TYPE enType, string strAction, string strMaterialID, string strFrom, string strTo)
        {
            if (string.IsNullOrEmpty(strMaterialID) || strMaterialID.Equals("NONE"))
                strMaterialID = EMPTY_DATA;

            // 2021.05.21 by twkang [MOD] Message Format 수정
            string strMessage           = string.Format("{0} '{1}' 'XFR' '{2}' '{3}' '{4}' '{5}' '{6}' '{7}' ('LOT_ID', '{8}')"
                , GetCurrentTime()
                , strID
                , strAction
                , m_arMappingXFR[(int)enType]
                , strMaterialID
                , _materialType
                , string.IsNullOrEmpty(strFrom) ? EMPTY_DATA : strFrom
                , string.IsNullOrEmpty(strTo) ? EMPTY_DATA : strTo
                , LotId);

            WriteLog(ref strMessage);
        }
		#endregion End - XFR (Transfer)

		#region PRC (Process)
		/// <summary>
		/// 2025.04.11 by junho [MOD] 데이터를 더 추가 가능하도록 기능 변경
		/// 2021.01.18 by yjlee [ADD] Write the process log.
		/// </summary>
		public void WriteProcessLog(string strID, EN_PRC_TYPE enType, string strEventID, string strMaterialID, Dictionary<string,string> extraDatas = null)
        //public void WriteProcessLog(string strID, EN_PRC_TYPE enType, string strEventID, string strMaterialID)
		{
			if (string.IsNullOrEmpty(strMaterialID) || strMaterialID.Equals("NONE"))
				strMaterialID = EMPTY_DATA;
			// 2022.08.29 by junho [MOD] message format 수정
			// 2021.05.21 by twkang [MOD] Message Format 수정
			string strMessage = string.Format("{0} '{1}' 'PRC' '{2}' '{3}' '{4}' '{5}' '{6}' ('LOT_ID', '{7}')"
                , GetCurrentTime()
                , strID
                , strEventID
                , m_arMappingPRC[(int)enType]
                , strMaterialID
                , LotId
                , GetCurrentProcessRecipe()
                , LotId);

            if(extraDatas != null)
			{
                foreach(var kvp in extraDatas)
				{
                    strMessage = string.Format("{0} ('{1}', '{2}')", strMessage, kvp.Key, kvp.Value);
				}
			}

            WriteLog(ref strMessage);
        }
		// 2024.09.26 by junho [DEL] 사양에 맞지 않은 형식이므로 삭제
		// 2021.08.01 by junho [ADD] Add Processs Log format
		//public void WriteProcessLog(string taskName, string eventID, string data)
		//{
		//	string message = string.Format("{0} '{1}' 'PRC' '{2}' ('Data', '{3}')"
		//		, GetCurrentTime()
		//		, taskName, eventID, data);

		//	WriteLog(ref message);
		//}

		// 2025.04.11 by junho [MOD] 데이터를 더 추가 가능하도록 기능 변경
		// 2024.09.26 by junho [ADD] START,END를 한번에 찍는 인터페이스 추가
		public void WriteProcessLog(string strID, string strEventID, string strMaterialID = EMPTY_DATA, Dictionary<string, string> extraDatas = null)
		//public void WriteProcessLog(string strID, string strEventID, string strMaterialID = EMPTY_DATA)
		{
            WriteProcessLog(strID, EN_PRC_TYPE.START, strEventID, strMaterialID, extraDatas);
			WriteProcessLog(strID, EN_PRC_TYPE.END, strEventID, strMaterialID, extraDatas);
		}
		#endregion End - PRC (Process)

		#region LEH (Event)
		/// <summary>
		/// 2021.01.18 by yjlee [ADD] Write the event log.
		/// </summary>
		public void WriteEventLog(EN_LEH_TYPE enType, string strLotID, string carrierId, int workQty, string swVersion)
        {
			string strMessage = string.Empty;
			switch (enType)
			{
				case EN_LEH_TYPE.LOT_IN:
					strMessage = string.Format("{0} '{1}' 'LEH' '{2}' '{3}' '{4}' '{5}' ('WF_QTY', {6}) ('RECIPE_ID', '{4}') ('SW_VERSION', '{7}')"
						, GetCurrentTime()				// 0
						, "SYSTEM"						// 1
						, m_arMappingLEH[(int)enType]	// 2
						, strLotID						// 3
						, GetCurrentProcessRecipe()		// 4
						, carrierId					// 5
						, workQty						// 6
						, swVersion);					// 7
					break;
				case EN_LEH_TYPE.LOT_OUT:
					strMessage = string.Format("{0} '{1}' 'LEH' '{2}' '{3}' '{4}' '{5}' ('WF_QTY', {6}) ('RECIPE_ID', '{4}')"
						, GetCurrentTime()				// 0
						, "SYSTEM"						// 1
						, m_arMappingLEH[(int)enType]	// 2
						, strLotID						// 3
						, GetCurrentProcessRecipe()		// 4
						, carrierId					// 5
						, workQty);						// 6
					break;
				default: return;
			}

            WriteLog(ref strMessage);
        }
        #endregion End - LEH (Event)

        #region ALM (Alarm)
        /// <summary>
        /// 2021.01.15 by yjlee [ADD] Write an alarm log.
        /// </summary>
        public void WriteAlarmLog(ref string strID, EN_ALM_TYPE enType, ref string strGrade, ref int nAlarmCode, ref string strDescription)
        {
            string strMessage           = string.Format("{0} '{1}' 'ALM' '{2}' '{3}' '{4}' ('DESCRIPTION', '{5}')"
                , GetCurrentTime()
                , strID
                , strGrade
                , nAlarmCode
                , m_arMappingALM[(int)enType]
                , strDescription);

            WriteLog(ref strMessage);
        }
        #endregion End - ALM (Alarm)

        #region CFG (Configuration)
        /// <summary>
        /// 2021.01.18 by yjlee [ADD] Write a configuration log.
        /// </summary>
        public void WriteConfigurationLog(ref string strID, EN_CFG_TYPE enType, string strParameterName, string strPreviousValue, string strValue)
        {
            // 2021.01.26 by yjlee [ADD] Checking the initialization.
            if (false == m_bInit) { return; }

            string strMessage = string.Empty;

            switch (enType)
            {
                case EN_CFG_TYPE.SETTING:
                    strMessage = string.Format("{0} '{1}' 'CFG' '{2}' ('{3}', '{4}')"
                                        , GetCurrentTime()
                                        , strID
                                        , m_arMappingCFG[(int)enType]
                                        , strParameterName
                                        , strValue);
                    break;

                case EN_CFG_TYPE.CHANGE:
                    strMessage = string.Format("{0} '{1}' 'CFG' '{2}' ('CHANGE_ITEM', '{3}') ('VALUE', [2, '{4}', '{5}'])"
                                        , GetCurrentTime()
                                        , strID
                                        , m_arMappingCFG[(int)enType]
                                        , strParameterName
                                        , strPreviousValue
                                        , strValue);
                    break;

                case EN_CFG_TYPE.SAVE:
                    strMessage = string.Format("{0} '{1}' 'CFG' '{2}' ('PATH', '{3}')"
                                        , GetCurrentTime()
                                        , strID
                                        , m_arMappingCFG[(int)enType]
                                        , strValue);
                    break;
            }

            WriteLog(ref strMessage);
        }
		#endregion End - CFG (Configuration)

		#region COMM (Communication)
		#endregion End - COMM (Communication)

		#endregion End - Logging

		public string CaptionMaterialType { set { _materialType = value; } }
        public void RegisterGetLotIdFunction(DeleGetLotId func)
		{
            _funcGetLotId = func;
		}
        public void RegisterGetMaterialIdFromTaskName(DeleGetMaterialIdFromTaskName func)
		{
            _funcGetMaterialIdFromTaskName = func;
		}
		#endregion End - External Interface
	}
}
