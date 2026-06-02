using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Vision_;
using Socket_;

namespace FrameOfSystem3.Controller.Vision
{
    /// <summary>
    /// 2020.11.24 by yjlee [ADD] Enumerate the algorithms of the vision.
    /// </summary>
    public enum EN_VISION_ALGORITHM
    {
        SHAPE_CENTER        = 0,
        BALL_ALIGNMENT,
        CORNER_DETECTOR,
        PATTERN_MATCH,
        ANGLE_GAUGE,
        ROTATE_CENTER,
        SPECIMEN_CALIBRATION,
        AUTO_FOCUS,
        CIRCLE_LEAST_SQUARE,

        WAFER_ROTATECALIBRATION_PROC,

        LEFT_GRID_SHAPE_600_PROC,
        LEFT_GRID_SHAPE_100_PROC,
        RIGHT_GRID_SHAPE_600_PROC,
        RIGHT_GRID_SHAPE_100_PROC,

        ULC_L_HEADTOOL_PROC,
        ULC_R_HEADTOOL_PROC,

        DLC_L_SPECIMEN_GRID_PROC,
        DLC_R_SPECIMEN_GRID_PROC,

        DLC_L_GRID_TILT_PROC,
        DLC_R_GRID_TILT_PROC,

        ULC_L_ULC_TO_DLC_CENTER_CAL_PROC,
        ULC_R_ULC_TO_DLC_CENTER_CAL_PROC,

        DLC_L_DLC_TO_ULC_CENTER_CAL_PROC,
        DLC_R_DLC_TO_ULC_CENTER_CAL_PROC,

        ULC_L_SPECIMEN_PROC,
        ULC_R_SPECIMEN_PROC,
        DLC_L_SPECIMEN_PROC,
        DLC_R_SPECIMEN_PROC,

        DLC_L_GRID_CALIBRATION_PROC,
        DLC_R_GRID_CALIBRATION_PROC,
    }
    /// <summary>
    /// 2020.11.12 by yjlee [ADD] Implemenation class of the ForthLogic vision controller.
    ///     - This controller uses TCP/IP communication.
    /// </summary>
    class FourthLogicVisionController : VisionController
    {
        #region Constant
        private const byte TOKEN_MAGIC          = 0x46;

        #region Result
        private const int VISION_SUCCESS        = 0;
        #endregion

        #region Byte Size
        private const byte BYTE_SIZE_MAGIC      = 1;
        private const byte BYTE_SIZE_TYPE       = 4;
        private const byte BYTE_SIZE_PACKET_NO  = 4;
        private const byte BYTE_SIZE_LENGTH     = 4;
        private const byte BYTE_SIZE_DECRYPTED_LENGTH  = 4;

        private const byte BYTE_STARTING_INDEX_MAGIC        = 0;
        private const byte BYTE_STARTING_INDEX_TYPE         = BYTE_SIZE_MAGIC;
        private const byte BYTE_STARTING_INDEX_PACKET_NO    = BYTE_SIZE_TYPE + BYTE_STARTING_INDEX_TYPE;
        private const byte BYTE_STARTING_INDEX_LENGTH       = BYTE_SIZE_PACKET_NO + BYTE_STARTING_INDEX_PACKET_NO;
        private const byte BYTE_STARTING_INDEX_DECRYPTED_LENGTH     = BYTE_SIZE_LENGTH + BYTE_STARTING_INDEX_LENGTH;

        private const byte BYTE_SIZE_BASE                   = BYTE_SIZE_MAGIC 
                                                                + BYTE_SIZE_TYPE
                                                                + BYTE_SIZE_PACKET_NO
                                                                + BYTE_SIZE_LENGTH
                                                                + BYTE_SIZE_DECRYPTED_LENGTH;

        private const int STARTING_INDEX_RESULT_DATA = BYTE_SIZE_BASE + (int)DATA_LENGTH_INTEGER + (int)DATA_LENGTH_INTEGER + (int)DATA_LENGTH_INTEGER + (int)DATA_LENGTH_INTEGER;
        #endregion

        #region Type
        private const UInt32 TYPE_RESERVED              = 0x01;
        private const UInt32 TYPE_RESERVED2             = 0x02;

        private const UInt32 TYPE_SET_RECIPE_CHANGE     = 0x03;
        private const UInt32 TYPE_SET_RECIPE_COPY       = 0x04;
        private const UInt32 TYPE_SET_RECIPE_REMOVE     = 0x05;
        private const UInt32 TYPE_GET_CURRENT_RECIPE    = 0x06;

        private const UInt32 TYPE_GET_CURRENT_ALGORITHM = 0x08;

        private const UInt32 TYPE_EXE_PROCEDURE         = 0x0E; // 14;
        
        private const UInt32 TYPE_EXIT_VISION_CONTROLLER    = 0x0D;

        private const UInt32 TYPE_END       = 0x80;
        #endregion

        #region Method
        private const UInt32 METHOD_LOAD                = 0x01;
        private const UInt32 METHOD_GRAB                = 0x02;
        private const UInt32 METHOD_EXE                 = 0x04;
        #endregion

        #region Data Length
        private const UInt32 DATA_LENGTH_INTEGER        = 0x00000004;
        private const UInt32 DATA_LENGTH_DOUBLE         = 0x00000008;
        private const UInt32 DATA_LENGTH_CHARACTER      = 0x00000100;
        #endregion

        #region Pixed Array
        readonly byte[] BYTE_ARRAY_ZERO                 = BitConverter.GetBytes((Int32)0);
        #endregion

        #endregion

        #region Enumerate
        /// <summary>
        /// 2020.11.13 by yjlee [ADD] Enumerate the work with the single.
        /// </summary>
        private enum EN_SINGLE_WORK
        {
            GET_VISION_INFO         = 0,
            GET_CURRENT_RECIPE,
            LOAD_RECIPE,
            SET_RUN_MODE,
        }
        /// <summary>
        /// 2020.11.13 by yjlee [ADD] Enumerate the work with the multi.
        /// </summary>
        private enum EN_MULTI_WORK
        {
            CHANGE_ALGORITHM        = 0,
            EXE_GRAB_IMAGE,
            EXE_INSPECTION,
        }
        /// <summary>
        /// 2020.11.13 by yjlee [ADD] Enumerate the steps for the work.
        /// </summary>
        private enum EN_WORK_STEP
        {
            REQUEST         = 0,
            WAIT_RESPONSE,
            PROCESS,
        }
        #endregion

        #region Variables
        Socket m_instanceSocket             = null;
        int m_nIndexOfSocketItem            = -1;

        int m_nCountOfCam                   = 0;

        Queue<byte[]> m_bufferForReceivingHeader                = new Queue<byte[]>();
        Queue<byte[]>[] m_bufferForReceivingData                = new Queue<byte[]>[TYPE_END];
        System.Threading.ReaderWriterLockSlim m_pLockForRecevingData   = new System.Threading.ReaderWriterLockSlim();

        #region Algorithm
        int[] m_arAlgorithmOnCamera             = null;
        string[] m_arCameraName                 = null;
        byte[][] m_arCamera                     = null;
        byte[][] m_arSelectedAlgorithm          = null;
        Queue<VISION_DATA>[] m_bufferForResult  = null;
        System.Threading.ReaderWriterLockSlim m_pLockForResult          = new System.Threading.ReaderWriterLockSlim();

        int m_nCountOfAlgorithm             = 0;
        byte[][] m_arAlgorithm              = null;
        string[] m_arAlgorithmName          = null;
        #endregion

        #endregion

        #region Internal Interafce

        #region Mapping
        /// <summary>
        /// 2020.11.24 by yjlee [ADD] Make the mapping array for the name of the camera.
        /// </summary>
        void MakeCameraMappingByteArray(ref int nCountOfCam)
        {
            m_arAlgorithmOnCamera   = new int[nCountOfCam];
            m_arCamera              = new byte[nCountOfCam][];
            m_arCameraName          = new string[nCountOfCam];

            m_arCameraName[0]   = "WAFER_CAMERA";
            m_arCameraName[1]   = "DIE_SHUTTLE_L";
            m_arCameraName[2]   = "DIE_SHUTTLE_R";
            m_arCameraName[3]   = "BONS_HEAD_DLC_L";
            m_arCameraName[4]   = "BONS_HEAD_ULC_L";
            m_arCameraName[5]   = "BONS_HEAD_DLC_R";
            m_arCameraName[6]   = "BONS_HEAD_ULC_R";

            m_bufferForResult   = new Queue<VISION_DATA>[nCountOfCam];

            for(int nIndex = 0 ; nIndex < nCountOfCam ; ++ nIndex)
            {
                byte[] arName           = null;
            
                ConvertStringToByte(ref m_arCameraName[nIndex], ref arName);
            
                m_arCamera[nIndex]      = arName;

                m_bufferForResult[nIndex]       = new Queue<VISION_DATA>();
            }

            m_arSelectedAlgorithm       = new byte[nCountOfCam][];
        }
        /// <summary>
        /// 2020.11.24 by yjlee [ADD] Make the mapping array for the algorithms.
        /// </summary>
        void MakeAlgorithmMappingByteArray()
        {
            m_nCountOfAlgorithm         = Enum.GetValues(typeof(EN_VISION_ALGORITHM)).Length;

            m_arAlgorithm               = new byte[m_nCountOfAlgorithm][];
            m_arAlgorithmName           = new string[m_nCountOfAlgorithm];

            m_arAlgorithmName[(int)EN_VISION_ALGORITHM.SHAPE_CENTER]                        = "SHAPE_CENTER";
            m_arAlgorithmName[(int)EN_VISION_ALGORITHM.BALL_ALIGNMENT]                      = "BALL_ALIGNMENT";
            m_arAlgorithmName[(int)EN_VISION_ALGORITHM.CORNER_DETECTOR]                     = "WAFER_CORNER";
            m_arAlgorithmName[(int)EN_VISION_ALGORITHM.PATTERN_MATCH]                       = "PATTERN_MATCH";
            m_arAlgorithmName[(int)EN_VISION_ALGORITHM.ANGLE_GAUGE]                         = "ANGLE_GAUGE";
            m_arAlgorithmName[(int)EN_VISION_ALGORITHM.ROTATE_CENTER]                       = "ROTATE_CENTER_FINDER";
            m_arAlgorithmName[(int)EN_VISION_ALGORITHM.SPECIMEN_CALIBRATION]                = "ROTATE_CALIBARATION";
            m_arAlgorithmName[(int)EN_VISION_ALGORITHM.AUTO_FOCUS]                          = "AUTO_FOCUS";
            m_arAlgorithmName[(int)EN_VISION_ALGORITHM.CIRCLE_LEAST_SQUARE]                 = "CALCULATE_ROT_CENTER";

            m_arAlgorithmName[(int)EN_VISION_ALGORITHM.WAFER_ROTATECALIBRATION_PROC]        = "WAFER_ROTATECALIBRATION_PROC";

            m_arAlgorithmName[(int)EN_VISION_ALGORITHM.DLC_L_GRID_TILT_PROC]                = "DLC_L_GRID_TILT_PROC";
            m_arAlgorithmName[(int)EN_VISION_ALGORITHM.DLC_R_GRID_TILT_PROC]                = "DLC_R_GRID_TILT_PROC";
            m_arAlgorithmName[(int)EN_VISION_ALGORITHM.DLC_L_SPECIMEN_GRID_PROC]            = "DLC_L_SPECIMEN_GRID_PROC";
            m_arAlgorithmName[(int)EN_VISION_ALGORITHM.DLC_R_SPECIMEN_GRID_PROC]            = "DLC_R_SPECIMEN_GRID_PROC";
            m_arAlgorithmName[(int)EN_VISION_ALGORITHM.LEFT_GRID_SHAPE_100_PROC]            = "LEFT_GRID_SHAPE_100_PROC";
            m_arAlgorithmName[(int)EN_VISION_ALGORITHM.LEFT_GRID_SHAPE_600_PROC]            = "LEFT_GRID_SHAPE_600_PROC";
            m_arAlgorithmName[(int)EN_VISION_ALGORITHM.RIGHT_GRID_SHAPE_100_PROC]           = "RIGHT_GRID_SHAPE_100_PROC";
            m_arAlgorithmName[(int)EN_VISION_ALGORITHM.RIGHT_GRID_SHAPE_600_PROC]           = "RIGHT_GRID_SHAPE_600_PROC";

            m_arAlgorithmName[(int)EN_VISION_ALGORITHM.ULC_L_HEADTOOL_PROC]                 = "ULC_L_HEADTOOL_PROC";
            m_arAlgorithmName[(int)EN_VISION_ALGORITHM.ULC_R_HEADTOOL_PROC]                 = "ULC_R_HEADTOOL_PROC";

            m_arAlgorithmName[(int)EN_VISION_ALGORITHM.ULC_L_SPECIMEN_PROC]                 = "ULC_L_SPECIMEN_PROC";
            m_arAlgorithmName[(int)EN_VISION_ALGORITHM.ULC_R_SPECIMEN_PROC]                 = "ULC_R_SPECIMEN_PROC";

            m_arAlgorithmName[(int)EN_VISION_ALGORITHM.DLC_L_SPECIMEN_PROC]                 = "DLC_L_SPECIMEN_PROC";
            m_arAlgorithmName[(int)EN_VISION_ALGORITHM.DLC_R_SPECIMEN_PROC]                 = "DLC_R_SPECIMEN_PROC";

            m_arAlgorithmName[(int)EN_VISION_ALGORITHM.DLC_L_DLC_TO_ULC_CENTER_CAL_PROC]    = "DLC_L_DLC_TO_ULC_CENTER_CAL_PROC";
            m_arAlgorithmName[(int)EN_VISION_ALGORITHM.DLC_R_DLC_TO_ULC_CENTER_CAL_PROC]    = "DLC_R_DLC_TO_ULC_CENTER_CAL_PROC";
            m_arAlgorithmName[(int)EN_VISION_ALGORITHM.ULC_L_ULC_TO_DLC_CENTER_CAL_PROC]    = "ULC_L_ULC_TO_DLC_CENTER_CAL_PROC";
            m_arAlgorithmName[(int)EN_VISION_ALGORITHM.ULC_R_ULC_TO_DLC_CENTER_CAL_PROC]    = "ULC_R_ULC_TO_DLC_CENTER_CAL_PROC";

            m_arAlgorithmName[(int)EN_VISION_ALGORITHM.DLC_L_GRID_CALIBRATION_PROC]			= "DLC_L_GRID_CALIBRATION_PROC";
            m_arAlgorithmName[(int)EN_VISION_ALGORITHM.DLC_R_GRID_CALIBRATION_PROC]			= "DLC_R_GRID_CALIBRATION_PROC";

            for(int nIndex = 0, nEnd = m_arAlgorithmName.Length ; nIndex < nEnd ; ++ nIndex)
            {
                byte[] arAlgorithm      = null;

                ConvertStringToByte(ref m_arAlgorithmName[nIndex], ref arAlgorithm);

                m_arAlgorithm[nIndex]   = arAlgorithm;
            }
        }
        #endregion

        #region Format
        /// <summary>
        /// 2020.11.12 by yjlee [ADD] Make the base protocol for the communication.
        /// </summary>
        private void MakeBaseProtocol(ref byte[] arSendData, ref UInt32 uType, ref UInt32 uPacketNumber, ref UInt32 uLength)
        {
             arSendData[BYTE_STARTING_INDEX_MAGIC]       = TOKEN_MAGIC;

            #region Set Type
            byte[] arType           = BitConverter.GetBytes(uType);

            int nBitIndex           = -1;
            
            for(int nIndex =  BYTE_STARTING_INDEX_TYPE, nEnd = BYTE_STARTING_INDEX_TYPE + BYTE_SIZE_TYPE
                ; nIndex < nEnd ; ++ nIndex)
            {
                arSendData[nIndex]      = arType[ ++ nBitIndex];
            }
            #endregion

            #region Set Packet Number.
            byte[] arPacketNumber   = BitConverter.GetBytes(uPacketNumber);

            nBitIndex               = -1;

            for(int nIndex =  BYTE_STARTING_INDEX_PACKET_NO, nEnd = BYTE_STARTING_INDEX_PACKET_NO + BYTE_SIZE_PACKET_NO
                ; nIndex < nEnd ; ++ nIndex)
            {
                arSendData[nIndex]      = arPacketNumber[ ++ nBitIndex];
            }
            #endregion

            #region Set Length & Set Decryped Length
            byte[] arLength         = BitConverter.GetBytes(uLength);
            int nDescryptedOffset   = BYTE_STARTING_INDEX_DECRYPTED_LENGTH - BYTE_STARTING_INDEX_LENGTH;

            nBitIndex               = -1;
            
            for(int nIndex =  BYTE_STARTING_INDEX_LENGTH, nEnd = BYTE_STARTING_INDEX_LENGTH + BYTE_SIZE_LENGTH
                ; nIndex < nEnd ; ++ nIndex)
            {
                arSendData[nIndex]      = arLength[ ++ nBitIndex];
                
                // Set Decrypted data length.
                arSendData[nIndex + nDescryptedOffset] = arLength[nBitIndex];
            }

            #endregion
        }
        #endregion

        #region Socket
        /// <summary>
        /// 2020.11.24 by yjlee [ADD] Send a message through the socket.
        /// </summary>
        private bool SendMessage(ref byte[] arData)
        {
            if (m_instanceSocket.Send(m_nIndexOfSocketItem, arData))
            {
                AddHistory(true, ref arData);

                return true;
            }

            return false;
        }
        /// <summary>
        /// 2020.11.13 by yjlee [ADD] Request the data to vision controller.
        /// </summary>
        private bool RequestToVisionController(UInt32 uType, UInt32 uPacketNumber, UInt32 uLength, ref byte[] arData)
        {
            byte[] arSendData       = new byte[BYTE_SIZE_BASE + uLength];
            
            MakeBaseProtocol(ref arSendData, ref uType, ref uPacketNumber, ref uLength);

            #region Set Data
            int nDataArrayLength    = arData.Length;
            int nBitIndex           = -1;

            for(int nIndex =  BYTE_SIZE_BASE, nEnd = (int)(BYTE_SIZE_BASE + nDataArrayLength)
                ; nIndex < nEnd ; ++ nIndex)
            {
                arSendData[nIndex]      = arData[ ++ nBitIndex];
            }
            #endregion

            return SendMessage(ref arSendData);
        }
        /// <summary>
        /// 2020.11.13 by yjlee [ADD] Request the data to vision controller.
        /// </summary>
        private bool RequestToVisionController(UInt32 uType, UInt32 uPacketNumber, int nCountOfData,  ref UInt32[] arLength, ref byte[][] arData)
        {
            UInt32 uTotalLength        = 0;

            for(int nIndex = 0 ; nIndex < nCountOfData ; ++ nIndex)
            {
                uTotalLength        += arLength[nIndex];
            }

            byte[] arSendData       = new byte[BYTE_SIZE_BASE + uTotalLength];
           
            MakeBaseProtocol(ref arSendData, ref uType, ref uPacketNumber, ref uTotalLength);

            #region Set Data
            int nStartingIndex      = BYTE_SIZE_BASE;
            int nBitIndex           = -1;

            for(int nCount = 0 ; nCount < nCountOfData ; ++ nCount)
            {
                nBitIndex               = -1;

                for (int nIndex = nStartingIndex, nEnd = (int)(nStartingIndex + arData[nCount].Length)
                ; nIndex < nEnd ; ++ nIndex)
                {
                    arSendData[nIndex]      = arData[nCount][++ nBitIndex];
                }

                nStartingIndex      = (int)(nStartingIndex + arLength[nCount]);
            }
            #endregion

            return SendMessage(ref arSendData);
        }
        /// <summary>
        /// <summary>
        /// 2020.11.13 by yjlee [ADD] Receive the data from the controller.
        /// </summary>
        private bool ReceiveData(EN_SINGLE_WORK enWork, ref byte[] arData)
        {
            UInt32 uType      = TYPE_RESERVED;

            switch(enWork)
            {
                case EN_SINGLE_WORK.GET_VISION_INFO:
                case EN_SINGLE_WORK.SET_RUN_MODE:
                    // none
                    return false;

                case EN_SINGLE_WORK.GET_CURRENT_RECIPE:
                    uType       = TYPE_GET_CURRENT_RECIPE;
                    break;

                case EN_SINGLE_WORK.LOAD_RECIPE:
                    uType       = TYPE_SET_RECIPE_CHANGE;
                    break;

                default:
                    return false;
            }

            #region Find in the buffer
            bool bFoundData     = false;

            m_pLockForRecevingData.EnterWriteLock();
            if(bFoundData = (m_bufferForReceivingData[uType].Count > 0))
            {
                arData          = m_bufferForReceivingData[uType].Dequeue();
            }
            m_pLockForRecevingData.ExitWriteLock();
            #endregion

            return bFoundData;
        }
        /// <summary>
        /// 2020.11.13 by yjlee [ADD] Receive the data from the controller.
        /// </summary>
        private bool ReceiveData(EN_MULTI_WORK enWork, ref int nPacketNumber, ref byte[] arData)
        {
            UInt32 uType      = TYPE_RESERVED;

            switch(enWork)
            {
                case EN_MULTI_WORK.CHANGE_ALGORITHM:
                case EN_MULTI_WORK.EXE_GRAB_IMAGE:
                case EN_MULTI_WORK.EXE_INSPECTION:
                    uType       = TYPE_EXE_PROCEDURE;
                    break;

                default:
                    return false;
            }

            #region Find in the buffer
            Queue<byte[]> queueTemp     = new Queue<byte[]>();
            bool bFoundData             = false;

            m_pLockForRecevingData.EnterWriteLock();
            while(m_bufferForReceivingData[uType].Count > 0)
            {
                arData              = m_bufferForReceivingData[uType].Dequeue();

                UInt32 uPackNumber  = GetPacketNumberInData(ref arData);

                if(uPackNumber == (UInt32)(nPacketNumber))
                {
                    bFoundData  = true;

                    break;
                }
                else
                {
                    queueTemp.Enqueue(arData);
                }
            }
            m_pLockForRecevingData.ExitWriteLock();

            m_pLockForRecevingData.EnterWriteLock();
            while(queueTemp.Count > 0)
            {
                m_bufferForReceivingData[uType].Enqueue(queueTemp.Dequeue());
            }
            m_pLockForRecevingData.ExitWriteLock();
            #endregion

            return bFoundData;
        }
        #endregion

        #region Parse Data
        /// <summary>
        /// 2020.11.14 by yjlee [ADD] Check the prefix of the data.
        /// </summary>g
        private bool CheckPrefix(ref byte[] arData)
        {
            return BYTE_SIZE_BASE <= arData.Length
                && TOKEN_MAGIC == arData[BYTE_STARTING_INDEX_MAGIC];
        }
        /// <summary>
        /// 2020.11.14 by yjlee [ADD] Get the type in the data.
        /// </summary>
        private UInt32 GetTypeInData(ref byte[] arData)
        {
            int nType           = 0;
            int nShiftCount     = -1;

            for(int nIndex = BYTE_STARTING_INDEX_TYPE, nEnd = BYTE_STARTING_INDEX_TYPE + BYTE_SIZE_MAGIC
                ; nIndex < nEnd ; ++ nIndex)
            {
                nType       |= (arData[nIndex] << ((++ nShiftCount) << 3));
            }

            return (UInt32)nType;
        }
        /// <summary>
        /// 2020.11.22 by yjlee [ADD] Get the packet number in the data.
        /// </summary>
        private UInt32 GetPacketNumberInData(ref byte[] arData)
        {
            int nPacketNumber       = 0;
            int nShiftCount         = -1;

            for (int nIndex = BYTE_STARTING_INDEX_PACKET_NO, nEnd = BYTE_STARTING_INDEX_PACKET_NO + BYTE_SIZE_PACKET_NO
                ; nIndex < nEnd; ++ nIndex)
            {
                nPacketNumber |= (arData[nIndex] << ((++ nShiftCount) << 3));
            }

            return (UInt32)nPacketNumber;
        }
        /// <summary>
        /// 2020.11.14 by yjlee [ADD] Get the length of the data.
        /// </summary>
        private UInt32 GetLengthInData(ref byte[] arData)
        {
            int nSize           = 0;
            int nShiftCount     = -1;

            for (int nIndex = BYTE_STARTING_INDEX_LENGTH, nEnd = BYTE_STARTING_INDEX_LENGTH + BYTE_SIZE_LENGTH
                ; nIndex < nEnd; ++ nIndex)
            {
                nSize       |= (arData[nIndex] << ((++ nShiftCount) << 3));
            }

            return (UInt32)nSize;
        }
        /// <summary>
        /// 2020.11.16 by yjlee [ADD] Get the data in the received data.
        /// </summary>
        private bool GetData(ref byte[] arReceivedData, ref byte[] arData, ref UInt32 uLength)
        {
            int nTotalLength    = (int)(BYTE_SIZE_BASE + uLength);

            if (arReceivedData.Length >= nTotalLength)
            {
                arData              = new byte[uLength];

                int nIndexOfData    = -1;

                for(int nIndex = BYTE_SIZE_BASE ; nIndex < nTotalLength ; ++ nIndex)
                {
                    arData[++ nIndexOfData]     = arReceivedData[nIndex];
                }

                return true;
            }

            return false;
        }
        /// <summary>
        /// 2020.11.25 by yjlee [ADD] Get the result in the data.
        /// </summary>
		//private bool GetResultData(ref byte[] arData, int nCountOfData, ref byte[][] arResult, ref UInt32[] arLength)
		//{
		//	int nTotalLength        = STARTING_INDEX_RESULT_DATA;

		//	for(int nIndex = 0 ; nIndex < nCountOfData ; ++ nIndex)
		//	{
		//		nTotalLength        += (int)arLength[nIndex];
		//	}

		//	if (arData.Length >= nTotalLength)
		//	{
		//		int nIndexOfTotalData       = STARTING_INDEX_RESULT_DATA - 1;
				
		//		int nResultCount			= arData[++ nIndexOfTotalData];

		//		for(int nCount = 0, nEnd = nCountOfData * nResultCount; nCount < nEnd; ++ nCount)
		//		{
					
		//		}

		//		for (int nIndex = 0; nIndex < nCountOfData; ++nIndex)
		//		{
		//			int nDataLength     = (int)arLength[nIndex];

		//			byte[] arTemp       = new byte[nDataLength];

		//			for (int nData = 0; nData < nDataLength; ++nData)
		//			{
		//				arTemp[nData] = arData[++nIndexOfTotalData];
		//			}

		//			arResult[nIndex] = arTemp;
		//		}

		//		return true;
		//	}

		//	return false;
		//}

		/// <summary>
		/// 2021.05.27 by twkang [ADD]
		/// </summary>
		private bool GetResultData(ref byte[] arData, ref byte[][] arResult,  ref UInt32[] arLength, ref int nResultCount)
		{
			int nTotalLength        = STARTING_INDEX_RESULT_DATA;

			int nCountOfData		= arLength.Length;

			for(int nIndex = 0; nIndex < nCountOfData; ++nIndex)
			{
				nTotalLength	+= (int)arLength[nIndex];
			}

			if (arData.Length >= nTotalLength)
			{
				int nIndexOfTotalData       = STARTING_INDEX_RESULT_DATA - 1;

				byte[] arResultCount		= new byte[DATA_LENGTH_INTEGER];

				for(int nCount = 0; nCount < arResultCount.Length; ++ nCount)
				{
					arResultCount[nCount]	= arData[++nIndexOfTotalData];
				}

				nResultCount				= BitConverter.ToInt32(arResultCount, 0);
				
				arResult					= new byte[nResultCount * nCountOfData][];

				for (int nCount = 0, nEnd = nCountOfData * nResultCount; nCount < nEnd; ++nCount)
				{
                    int nTset = nCount % nCountOfData;

                    int nDataLength = (int)arLength[nTset];

					byte[] arTemp		= new byte[nDataLength];

					for(int nData = 0; nData < nDataLength; ++nData)
					{
						arTemp[nData]	= arData[++nIndexOfTotalData];
					}

					arResult[nCount]		= arTemp;

				}

				return true;
			}

			return false;
		}
        #endregion

        #region Convert Type

        #region Byte to Type
        /// <summary>
        /// 2020.11.13 by yjlee [ADD] Convert the byte array to string.
        /// </summary>
        private void ConvertByteToString(ref byte[] arMessage, ref string strData)
        {
            int nIndexOfNull        = -1;

            for(int nIndex = 0, nEnd = arMessage.Length ; nIndex < nEnd ; ++ nIndex)
            {
                if(arMessage[nIndex] == 0x00)   // Null
                {
                    nIndexOfNull    = nIndex;

                    break;
                }
            }

            byte[] arParsedMessage  = null;

            if(nIndexOfNull != -1)
            {
                arParsedMessage     = new byte[nIndexOfNull];

                for(int nIndex = 0 ; nIndex < nIndexOfNull ; ++ nIndex)
                {
                    arParsedMessage[nIndex]     = arMessage[nIndex];
                }
            }
            else
            {
                arParsedMessage     = arMessage;
            }

            char[] chMessage    = System.Text.Encoding.ASCII.GetChars(arParsedMessage);

            strData             = new string(chMessage);
        }
        #endregion

        #region Type to Byte
        /// <summary>
        /// 2020.11.13 by yjlee [ADD] Convert string to the byte array.
        /// </summary>
        private void ConvertStringToByte(ref string strData, ref byte[] arMessage)
        {
            char[] chMessage    = strData.ToCharArray();
            arMessage           = System.Text.Encoding.ASCII.GetBytes(chMessage);
        }
        #endregion

        #endregion

        #region Buffer
        /// <summary>
        /// 2020.11.26 by yjlee [ADD] Clear data in the buffer.
        /// </summary>
        private void ClearDataInBuffer(UInt32 uType)
        {
            m_pLockForRecevingData.EnterWriteLock();

            m_bufferForReceivingData[uType].Clear();

            m_pLockForRecevingData.ExitWriteLock();
        }
        /// <summary>
        /// 2020.11.26 by yjlee [ADD] Clear data in the buffer.
        /// </summary>
        private void ClearDataInBuffer(UInt32 uType, ref int nPacketNumber)
        {
            Queue<byte[]> queueTemp = new Queue<byte[]>();

            m_pLockForRecevingData.EnterWriteLock();

            while(m_bufferForReceivingData[uType].Count > 0)
            {
                byte[] arData       = m_bufferForReceivingData[uType].Dequeue();

                UInt32 uPackNumber  = GetPacketNumberInData(ref arData);

                if(uPackNumber != (UInt32)(nPacketNumber))
                {
                    queueTemp.Enqueue(arData);
                }
            }

            while(queueTemp.Count > 0)
            {
                m_bufferForReceivingData[uType].Enqueue(queueTemp.Dequeue());
            }

            m_pLockForRecevingData.ExitWriteLock();
        }
        /// <summary>
        /// 2020.11.24 by yjlee [ADD] Add the data toe the receiving buffer.
        /// </summary>
        private void AddDataToReceivingBuffer(ref UInt32 uType, ref byte[] arData)
        {
            m_pLockForRecevingData.EnterWriteLock();

            m_bufferForReceivingData[uType].Enqueue(arData);

            m_pLockForRecevingData.ExitWriteLock();

            AddHistory(false, ref arData);
        }
        /// <summary>
        /// 2020.11.25 by yjlee [ADD] Add the result to the buffer.
        /// </summary>
        private void AddResultToBuffer(ref int nIndexOfCam, ref int nAlgorithm, ref byte[] arData)
        {
            byte[][] arResult       = null;
            UInt32[] arLength       = null;
			int nResultCount		= -1;

            #region Set data format
            switch (nAlgorithm)
            {
                case (int)EN_VISION_ALGORITHM.ULC_R_HEADTOOL_PROC:
                case (int)EN_VISION_ALGORITHM.ULC_L_HEADTOOL_PROC:
                case (int)EN_VISION_ALGORITHM.LEFT_GRID_SHAPE_600_PROC:
                case (int)EN_VISION_ALGORITHM.RIGHT_GRID_SHAPE_600_PROC:
					// 8 byte floating : X
					// 8 byte floating : Y
                    //arResult            = new byte[2][];
                    arLength            = new UInt32[2]{ DATA_LENGTH_DOUBLE, DATA_LENGTH_DOUBLE};
                    break;

                case (int)EN_VISION_ALGORITHM.WAFER_ROTATECALIBRATION_PROC:
                case (int)EN_VISION_ALGORITHM.DLC_R_SPECIMEN_PROC:
                case (int)EN_VISION_ALGORITHM.DLC_L_SPECIMEN_PROC:
                case (int)EN_VISION_ALGORITHM.ULC_R_SPECIMEN_PROC:
                case (int)EN_VISION_ALGORITHM.ULC_L_SPECIMEN_PROC:
                case (int)EN_VISION_ALGORITHM.DLC_L_DLC_TO_ULC_CENTER_CAL_PROC:
                case (int)EN_VISION_ALGORITHM.DLC_R_DLC_TO_ULC_CENTER_CAL_PROC:
                case (int)EN_VISION_ALGORITHM.ULC_L_ULC_TO_DLC_CENTER_CAL_PROC:
                case (int)EN_VISION_ALGORITHM.ULC_R_ULC_TO_DLC_CENTER_CAL_PROC:
                case (int)EN_VISION_ALGORITHM.DLC_L_GRID_TILT_PROC:
                case (int)EN_VISION_ALGORITHM.DLC_R_GRID_TILT_PROC:
                case (int)EN_VISION_ALGORITHM.SHAPE_CENTER:
                case (int)EN_VISION_ALGORITHM.BALL_ALIGNMENT:
                case (int)EN_VISION_ALGORITHM.PATTERN_MATCH:
                
                    // 8 byte floating : X
                    // 8 byte floating : Y
                    // 8 byte floating : T
                    //arResult            = new byte[3][];
                    arLength            = new UInt32[3]{ DATA_LENGTH_DOUBLE, DATA_LENGTH_DOUBLE, DATA_LENGTH_DOUBLE };
                    break;

				case (int)EN_VISION_ALGORITHM.DLC_L_GRID_CALIBRATION_PROC:
				case (int)EN_VISION_ALGORITHM.DLC_R_GRID_CALIBRATION_PROC:
					// 8 byte floating : X
					// 8 byte floating : Y
					// 8 byte floating : Outer T (Camera)
					// 8 byte floating : Inner T (Lattice)
					arLength            = new UInt32[4]{ DATA_LENGTH_DOUBLE, DATA_LENGTH_DOUBLE, DATA_LENGTH_DOUBLE ,DATA_LENGTH_DOUBLE };
					break;

                case (int)EN_VISION_ALGORITHM.ANGLE_GAUGE:
                    // 8 byte floating : T
                    //arResult            = new byte[1][];
                    arLength            = new UInt32[1]{ DATA_LENGTH_DOUBLE };
                    break;

                case (int)EN_VISION_ALGORITHM.ROTATE_CENTER:
                case (int)EN_VISION_ALGORITHM.CIRCLE_LEAST_SQUARE:
                    // 8 byte floating : Outer X
                    // 8 byte floating : Outer Y
                    //arResult            = new byte[2][];
                    arLength            = new UInt32[2]{ DATA_LENGTH_DOUBLE, DATA_LENGTH_DOUBLE };
                    break;

                case (int)EN_VISION_ALGORITHM.CORNER_DETECTOR:
                    //arResult            = new byte[5][];
                    arLength            = new UInt32[5]{DATA_LENGTH_DOUBLE, DATA_LENGTH_DOUBLE, DATA_LENGTH_DOUBLE, DATA_LENGTH_DOUBLE, DATA_LENGTH_DOUBLE};
                    break;

                case (int)EN_VISION_ALGORITHM.DLC_R_SPECIMEN_GRID_PROC:
                case (int)EN_VISION_ALGORITHM.DLC_L_SPECIMEN_GRID_PROC:
                case (int)EN_VISION_ALGORITHM.SPECIMEN_CALIBRATION:
                    // 8 byte floating : Outer X
                    // 8 byte floating : Outer Y
                    // 8 byte floating : Outer T
                    // 8 byte floating : Inner X
                    // 8 byte floating : Inner Y
                    // 8 byte floating : Inner T 
                    //arResult            = new byte[6][];
                    arLength            = new UInt32[6]{DATA_LENGTH_DOUBLE, DATA_LENGTH_DOUBLE, DATA_LENGTH_DOUBLE, DATA_LENGTH_DOUBLE, DATA_LENGTH_DOUBLE, DATA_LENGTH_DOUBLE};
                    break;

                case (int)EN_VISION_ALGORITHM.AUTO_FOCUS:
                    // 8 byte floating : MTF
                    // 8 byte floating : Data
                    //arResult            = new byte[2][];
                    arLength            = new UInt32[2]{ DATA_LENGTH_DOUBLE, DATA_LENGTH_DOUBLE };
                    break;
            }
            #endregion

            if (GetResultData(ref arData, ref arResult, ref arLength, ref nResultCount))
            {
                VISION_DATA visionData  = new VISION_DATA();
				int nResultIndex		= -1;

                #region Set data (result)
                switch (nAlgorithm)
                {
					case (int)EN_VISION_ALGORITHM.ROTATE_CENTER:
                    case (int)EN_VISION_ALGORITHM.ULC_R_HEADTOOL_PROC:
                    case (int)EN_VISION_ALGORITHM.ULC_L_HEADTOOL_PROC:
                    case (int)EN_VISION_ALGORITHM.LEFT_GRID_SHAPE_600_PROC:
                    case (int)EN_VISION_ALGORITHM.RIGHT_GRID_SHAPE_600_PROC:
                        // 8 byte floating : X
                        // 8 byte floating : Y
						visionData.arResult			= new POINT[nResultCount];

						for(int nIndex = 0; nIndex < nResultCount; ++nIndex)
						{
                            visionData.arResult[nIndex]     = new POINT();
							visionData.arResult[nIndex].X   = BitConverter.ToDouble(arResult[++nResultIndex], 0);
							visionData.arResult[nIndex].Y   = BitConverter.ToDouble(arResult[++nResultIndex], 0);
						}
                        break;

                    case (int)EN_VISION_ALGORITHM.WAFER_ROTATECALIBRATION_PROC:
                    case (int)EN_VISION_ALGORITHM.DLC_R_SPECIMEN_PROC:
                    case (int)EN_VISION_ALGORITHM.DLC_L_SPECIMEN_PROC:
                    case (int)EN_VISION_ALGORITHM.ULC_R_SPECIMEN_PROC:
                    case (int)EN_VISION_ALGORITHM.ULC_L_SPECIMEN_PROC:
                    case (int)EN_VISION_ALGORITHM.DLC_L_DLC_TO_ULC_CENTER_CAL_PROC:
                    case (int)EN_VISION_ALGORITHM.DLC_R_DLC_TO_ULC_CENTER_CAL_PROC:
                    case (int)EN_VISION_ALGORITHM.ULC_L_ULC_TO_DLC_CENTER_CAL_PROC:
                    case (int)EN_VISION_ALGORITHM.ULC_R_ULC_TO_DLC_CENTER_CAL_PROC:
                    case (int)EN_VISION_ALGORITHM.SHAPE_CENTER:
                    case (int)EN_VISION_ALGORITHM.BALL_ALIGNMENT:
                    case (int)EN_VISION_ALGORITHM.PATTERN_MATCH:
					case (int)EN_VISION_ALGORITHM.DLC_L_GRID_TILT_PROC:
					case (int)EN_VISION_ALGORITHM.DLC_R_GRID_TILT_PROC:
                        // 8 byte floating : X
                        // 8 byte floating : Y
                        // 8 byte floating : T
						visionData.arResult			= new POINT[nResultCount];
						visionData.arResultT		= new double[nResultCount];

						for (int nIndex = 0; nIndex < nResultCount; ++nIndex)
						{
							visionData.arResult[nIndex]     = new POINT();
                            visionData.arResult[nIndex].X   = BitConverter.ToDouble(arResult[++nResultIndex], 0);
                            visionData.arResult[nIndex].Y   = BitConverter.ToDouble(arResult[++nResultIndex], 0);
							visionData.arResultT[nIndex]	= BitConverter.ToDouble(arResult[++nResultIndex], 0);
						}
                        break;

					case (int)EN_VISION_ALGORITHM.DLC_L_GRID_CALIBRATION_PROC:
					case (int)EN_VISION_ALGORITHM.DLC_R_GRID_CALIBRATION_PROC:
                        // 8 byte floating : X
                        // 8 byte floating : Y
                        // 8 byte floating : Outer T (Camera)
						// 8 byte floating : Inner T (Lattice)
                        visionData.arResult			= new POINT[nResultCount];
						visionData.arResultT		= new double[nResultCount];
						visionData.arInnerResultT	= new double[nResultCount];

						for(int nIndex = 0; nIndex < nResultCount; ++nIndex)
						{
                            visionData.arResult[nIndex]         = new POINT();
							visionData.arResult[nIndex].X		= BitConverter.ToDouble(arResult[++nResultIndex], 0);
							visionData.arResult[nIndex].Y		= BitConverter.ToDouble(arResult[++nResultIndex], 0);
							visionData.arResultT[nIndex]		= BitConverter.ToDouble(arResult[++nResultIndex], 0);
							visionData.arInnerResultT[nIndex]	= BitConverter.ToDouble(arResult[++nResultIndex], 0);
						}
                        break;

                    case (int)EN_VISION_ALGORITHM.ANGLE_GAUGE:
                        // 8 byte floating : T
						visionData.arResultT		= new double[nResultCount];

						for(int nIndex = 0; nIndex < nResultCount; ++nIndex)
						{
							visionData.arResultT[nIndex]	= BitConverter.ToDouble(arResult[++nResultIndex], 0);
						}
                        break;

                    case (int)EN_VISION_ALGORITHM.CORNER_DETECTOR:
						// 8 byte floating : X
						// 8 byte floating : Y
						// 8 byte floating : Outer T (Point to Cneter)
						// 8 byte floating : Inner T (Corner Detector 내각)
						// 8 byte floating : Result Score
						visionData.arResult			= new POINT[nResultCount];
						visionData.arResultT		= new double[nResultCount];
						visionData.arInnerResultT	= new double[nResultCount];

						for(int nIndex = 0; nIndex < nResultCount; ++nIndex)
						{
                            visionData.arResult[nIndex]         = new POINT();
							visionData.arResult[nIndex].X		= BitConverter.ToDouble(arResult[++nResultIndex], 0);
							visionData.arResult[nIndex].Y		= BitConverter.ToDouble(arResult[++nResultIndex], 0);
							visionData.arResultT[nIndex]		= BitConverter.ToDouble(arResult[++nResultIndex], 0);
							visionData.arInnerResultT[nIndex]	= BitConverter.ToDouble(arResult[++nResultIndex], 0);
							visionData.dblData					= BitConverter.ToDouble(arResult[++nResultIndex], 0);
						}
                        break;

					case (int)EN_VISION_ALGORITHM.CIRCLE_LEAST_SQUARE:
						// 8 byte floating : Outer X 
						// 8 byte floating : Outer Y 
						// 8 byte floating : Outer T (Ellipse)
						// 8 byte floating : Inner X (Ellipse Radius)
						// 8 byte floating : Inner Y (Ellipse Radius)
						
						visionData.arResult			= new POINT[nResultCount];
						visionData.arInnerResult	= new POINT[nResultCount];
						visionData.arResultT		= new double[nResultCount];
						
                        for(int nIndex = 0; nIndex < nResultCount; ++nIndex)
						{
                            visionData.arResult[nIndex]         = new POINT();
                            visionData.arInnerResult[nIndex]    = new POINT();

							visionData.arResult[nIndex].X		= BitConverter.ToDouble(arResult[++nResultIndex], 0);
							visionData.arResult[nIndex].Y		= BitConverter.ToDouble(arResult[++nResultIndex], 0);
							visionData.arResultT[nIndex]		= BitConverter.ToDouble(arResult[++nResultIndex], 0);
							visionData.arInnerResult[nIndex].X	= BitConverter.ToDouble(arResult[++nResultIndex], 0);
							visionData.arInnerResult[nIndex].Y	= BitConverter.ToDouble(arResult[++nResultIndex], 0);
						}
                        break;

                    case (int)EN_VISION_ALGORITHM.DLC_R_SPECIMEN_GRID_PROC:
                    case (int)EN_VISION_ALGORITHM.DLC_L_SPECIMEN_GRID_PROC:
                    case (int)EN_VISION_ALGORITHM.SPECIMEN_CALIBRATION:
                        // 8 byte floating : Outer X
                        // 8 byte floating : Outer Y
                        // 8 byte floating : Outer T
                        // 8 byte floating : Inner X
                        // 8 byte floating : Inner Y
                        // 8 byte floating : Inner T

						visionData.arResult				= new POINT[nResultCount];
						visionData.arInnerResult		= new POINT[nResultCount];
						visionData.arResultT			= new double[nResultCount];
						visionData.arInnerResultT		= new double[nResultCount];
						
                        for(int nIndex = 0; nIndex < nResultCount; ++nIndex)
						{
                            visionData.arResult[nIndex]         = new POINT();
                            visionData.arInnerResult[nIndex]    = new POINT();

							visionData.arResult[nIndex].X		= BitConverter.ToDouble(arResult[++nResultIndex], 0);
							visionData.arResult[nIndex].Y		= BitConverter.ToDouble(arResult[++nResultIndex], 0);
							visionData.arResultT[nIndex]		= BitConverter.ToDouble(arResult[++nResultIndex], 0);
							visionData.arInnerResult[nIndex].X	= BitConverter.ToDouble(arResult[++nResultIndex], 0);
							visionData.arInnerResult[nIndex].Y	= BitConverter.ToDouble(arResult[++nResultIndex], 0);
							visionData.arInnerResultT[nIndex]	= BitConverter.ToDouble(arResult[++nResultIndex], 0);
						}
                        break;

                    case (int)EN_VISION_ALGORITHM.AUTO_FOCUS:
                        // 8 byte floating : MTF
                        // 8 byte floating : Data

						for(int nIndex = 0; nIndex < nResultCount; ++nIndex)
						{
							visionData.dblMTF		= BitConverter.ToDouble(arResult[++nResultIndex], 0);
							visionData.dblData		= BitConverter.ToDouble(arResult[++nResultIndex], 0);
						}
                        break;
                }
                #endregion

                visionData.SetScale(1000);      // um -> mm

                m_pLockForResult.EnterWriteLock();

                m_bufferForResult[nIndexOfCam].Enqueue(visionData);

                m_pLockForResult.ExitWriteLock();
            }
        }
        /// <summary>
        /// 2020.11.25 by yjlee [ADD] Add the result to the buffer.
        /// </summary>
        private bool GetResultToBuffer(ref int nIndexOfCam, ref VISION_DATA visionData)
        {
            m_pLockForResult.EnterWriteLock();

            bool bExist     = 0 < m_bufferForResult[nIndexOfCam].Count;

            if(bExist)
            {
                visionData    = m_bufferForResult[nIndexOfCam].Dequeue();
            }

            m_pLockForResult.ExitWriteLock();

            return bExist;
        }
        #endregion

        #region Parameter Checking
        /// <summary>
        /// 2020.11.25 by yjlee [ADD] Check the index of the camera is vaild.
        /// </summary>
        private bool IsCameraIndexVaild(ref int nIndexOfCam)
        {
            return nIndexOfCam >= 0 && nIndexOfCam < m_nCountOfCam;
        }
        private bool IsAlgorithmIndexVaild(ref int nAlgorithm)
        {
            return nAlgorithm >= 0 && nAlgorithm < m_nCountOfAlgorithm;
        }
        #endregion

        #endregion

        #region External Interface

        #region Controller
        /// <summary>
        /// 2020.11.12 by yjlee [ADD] Get the instances for the class.
        /// </summary>
        public override bool InitController(int nCountOfCam)
        {
            m_instanceSocket            = Socket.GetInstance();
            m_nIndexOfSocketItem        = (int)Define.DefineEnumeration.Socket.EN_SOCKET_INDEX.VISION;

            if (nCountOfCam < 1) { return false;}

            m_nCountOfCam               = nCountOfCam;

            int nCountOfMultiWork       = Enum.GetValues(typeof(EN_MULTI_WORK)).Length;

            for(int nIndex = 0 ; nIndex < TYPE_END ; ++ nIndex)
            {
                m_bufferForReceivingData[nIndex]     = new Queue<byte[]>();
                m_bufferForReceivingData[nIndex].Clear();
            }

            m_bufferForReceivingHeader.Clear();

            MakeAlgorithmMappingByteArray();
            MakeCameraMappingByteArray(ref nCountOfCam);

            m_instanceSocket.Connect(m_nIndexOfSocketItem);

            return true; 
        }
        /// <summary>
        /// 2020.11.12 by yjlee [ADD] Release the resources of the class.
        /// </summary> 
		public override void ExitController()
        {
        }
        /// <summary>
        /// 2020.11.22 by yjlee [ADD] Gather the message from the socket.
        /// </summary>
        public override void Execute()
        {
            byte[] arData       = null;

            if (m_instanceSocket.Receive(m_nIndexOfSocketItem, ref arData))
            {
                // It's header.
                if(CheckPrefix(ref arData))
                {
                    UInt32 uLength      = GetLengthInData(ref arData);

                    if(uLength < 1)
                    {
                        UInt32 uTypeInData      = GetTypeInData(ref arData);

                        AddDataToReceivingBuffer(ref uTypeInData, ref arData);
                    }
                    else
                    {
                        m_bufferForReceivingHeader.Enqueue(arData);
                    }
                }
                // It's data.
                else
                {
                    byte[] arHeader     = m_bufferForReceivingHeader.Dequeue();
                    int nLengthHeader   = arHeader.Length;

                    UInt32 uTypeInData  = GetTypeInData(ref arHeader);
                    UInt32 uLength      = GetLengthInData(ref arHeader);

                    if(uLength == (UInt32)arData.Length)
                    {
                        byte[] arCompleteData       = new byte[uLength + nLengthHeader];

                        for(int nIndex = 0 ; nIndex < nLengthHeader ; ++ nIndex)
                        {
                            arCompleteData[nIndex]  = arHeader[nIndex];
                        }

                        int nIndexOfData        = -1;

                        for(int nIndex = nLengthHeader, nEnd = nLengthHeader + (int)uLength ; nIndex < nEnd ; ++ nIndex)
                        {
                            arCompleteData[nIndex]  = arData[++ nIndexOfData];
                        }

                        AddDataToReceivingBuffer(ref uTypeInData, ref arCompleteData);
                    }
                }
            }
        }
        /// <summary>
        /// 2020.11.16 by yjlee [ADD] Check the controller state.
        /// </summary>
        public override CONTROLLER_STATE UpdateControllerState()
        {
            switch(m_instanceSocket.GetState(m_nIndexOfSocketItem))
            {
                case SOCKET_ITEM_STATE.READY:
                    if(m_instanceSocket.Connect(m_nIndexOfSocketItem))
                    {
                        return CONTROLLER_STATE.READY;
                    }
                    return CONTROLLER_STATE.ERROR;

                case SOCKET_ITEM_STATE.DISABLED:
                case SOCKET_ITEM_STATE.WAITING_CONNECTION:
                case SOCKET_ITEM_STATE.DISCONNECTED:
                    return CONTROLLER_STATE.ERROR;

                default:
                    return CONTROLLER_STATE.RUNNING;
            }
        }
        #endregion

        #region Common
        /// <summary>
        /// 2020.11.13 by yjlee [ADD] Get the current recipe setted the controller.
        /// </summary>
		public override VISION_RESULT GetCurrectRecipe(ref int nStep, ref string strRecipeName)
        {
            byte[] arData       = null;

            switch (nStep)
            {
                case 0:
                    ClearDataInBuffer(TYPE_GET_CURRENT_RECIPE);

                    arData      = BitConverter.GetBytes((UInt32)1);

                    if(false == RequestToVisionController(TYPE_GET_CURRENT_RECIPE, (UInt32)EN_SINGLE_WORK.GET_CURRENT_RECIPE,DATA_LENGTH_INTEGER, ref arData))
                    {
                        return VISION_RESULT.ERROR_COMMAND;
                    }

                    ++ nStep;
                    break;

                case 1:
                    if(ReceiveData(EN_SINGLE_WORK.GET_CURRENT_RECIPE, ref arData))
                    {
                        byte[] arRecipeName     = null;
                        UInt32 uLength          = GetLengthInData(ref arData);

                        if(GetData(ref arData, ref arRecipeName, ref uLength))
                        {
                            ConvertByteToString(ref arRecipeName, ref strRecipeName);

                            return VISION_RESULT.COMPLETE;
                        }
                    }
                    break;
            }

            return VISION_RESULT.IN_PROCESS;
        }
        /// <summary>
        /// 2020.11.13 by yjlee [ADD] Load the recipe has the name.
        /// </summary>
		public override VISION_RESULT LoadRecipe(ref int nStep, ref string strRecipeName)
        {
            byte[] arData       = null;

            switch (nStep)
            {
                case 0:
                    ClearDataInBuffer(TYPE_SET_RECIPE_CHANGE);

                    ConvertStringToByte(ref strRecipeName, ref arData);

                    if(false == RequestToVisionController(TYPE_SET_RECIPE_CHANGE, (UInt32)EN_SINGLE_WORK.LOAD_RECIPE,DATA_LENGTH_CHARACTER, ref arData))
                    {
                        return VISION_RESULT.ERROR_COMMAND;
                    }

                    ++ nStep;
                    break;

                case 1:
                    if (ReceiveData(EN_SINGLE_WORK.LOAD_RECIPE, ref arData))
                    {
                        byte[] arResult     = null;
                        UInt32 uLength      = GetLengthInData(ref arData);

                        if(GetData(ref arData, ref arResult, ref uLength))
                        {
                            return BitConverter.ToInt32(arResult, 0) == VISION_SUCCESS ? VISION_RESULT.COMPLETE : VISION_RESULT.ERROR_VISION;
                        }
                    }
                    break;
            }

            return VISION_RESULT.IN_PROCESS;
        }
        #endregion

        #region Mode
        /// <summary>
        /// 2020.11.13 by yjlee [ADD] Change the scene of the algorithm.
        /// </summary>
        public override VISION_RESULT ChangeScene(ref int nStep, ref int nIndexOfCam, ref int nAlgorithm)
        {
            // 2021.01.19 by yjlee [ADD] Merge this function to grab.
            if (IsCameraIndexVaild(ref nIndexOfCam)
                && IsAlgorithmIndexVaild(ref nAlgorithm))
            {
                m_arSelectedAlgorithm[nIndexOfCam]  = m_arAlgorithm[nAlgorithm];
                m_arAlgorithmOnCamera[nIndexOfCam]  = nAlgorithm;

                return VISION_RESULT.COMPLETE;
            }
            else
            {
                return VISION_RESULT.ERROR_COMMAND;
            }

            //switch(nStep)
            //{
            //    case 0:
            //        if (IsCameraIndexVaild(ref nIndexOfCam)
            //            && IsAlgorithmIndexVaild(ref nAlgorithm))
            //        {
            //            ClearDataInBuffer(TYPE_SET_CHANGE_ALGORITHM, ref nIndexOfCam);
            //
            //            byte[] arNameOfAlgorithm    = m_arAlgorithm[nAlgorithm];
            //            UInt32 uLength              = DATA_LENGTH_CHARACTER;
            //
            //            if (false == RequestToVisionController(TYPE_SET_CHANGE_ALGORITHM, (UInt32)nIndexOfCam, uLength, ref arNameOfAlgorithm))
            //            {
            //                return VISION_RESULT.ERROR_COMMAND;
            //            }
            //
            //            ++nStep;
            //        }
            //        break;
            //
            //    case 1:
            //        {
            //            byte[] arReceivedData   = null;
            //
            //            if (ReceiveData(EN_MULTI_WORK.CHANGE_ALGORITHM, ref nIndexOfCam, ref arReceivedData))
            //            {
            //                byte[] arResult     = null;
            //                UInt32 uLength      = DATA_LENGTH_INTEGER;
            //
            //                if(GetData(ref arReceivedData, ref arResult, ref uLength))
            //                {
            //                    int nResult     = BitConverter.ToInt32(arResult, 0);
            //
            //                    if(VISION_SUCCESS == nResult)
            //                    {
            //                        m_arAlgorithmOnCamera[nIndexOfCam]  = nAlgorithm;
            //
            //                        return VISION_RESULT.COMPLETE;
            //                    }
            //                    else
            //                    {
            //                        return VISION_RESULT.ERROR_VISION;
            //                    }
            //                }
            //            }
            //        }
            //        break;
            //}
            //
            //return VISION_RESULT.IN_PROCESS;
        }
        #endregion

        #region Inspection
        /// <summary>
        /// 2020.11.13 by yjlee [ADD] Grab the image of the camera.
        /// 2021.01.19 by yjlee [MOD] Change the format of the vision controller.
        /// </summary>
        public override VISION_RESULT GrabImage(ref int nStep, ref int nIndexOfCam)
        {
            switch (nStep)
            {
                case 0:
                    if (IsCameraIndexVaild(ref nIndexOfCam))
                    {
                        ClearDataInBuffer(TYPE_EXE_PROCEDURE, ref nIndexOfCam);

                        byte[] arMethod             = BitConverter.GetBytes(METHOD_LOAD | METHOD_GRAB);
                        byte[] arAlgorithm          = m_arSelectedAlgorithm[nIndexOfCam];
                        byte[] arAddtionalLength    = BYTE_ARRAY_ZERO;
                         
                        List<UInt32> listLength     = new List<uint>();

                        listLength.Add(DATA_LENGTH_INTEGER);
                        listLength.Add(DATA_LENGTH_CHARACTER);
                        listLength.Add(DATA_LENGTH_INTEGER);

                        List<byte[]> listTemp       = new List<byte[]>();

                        listTemp.Add(arMethod);
                        listTemp.Add(arAlgorithm);
                        listTemp.Add(arAddtionalLength);

                        UInt32[] arLength       = listLength.ToArray();
                        byte[][] arSendData     = listTemp.ToArray();

                        if(false == RequestToVisionController(TYPE_EXE_PROCEDURE, (UInt32)nIndexOfCam, listLength.Count, ref arLength, ref arSendData))
                        {
                            return VISION_RESULT.ERROR_COMMAND;
                        }

                        ++ nStep;
                    }
                    else
                    {
                        return VISION_RESULT.ERROR_COMMAND;
                    }
                    break;

                case 1:
                    {
                        byte[] arReceivedData       = null;

                        if (ReceiveData(EN_MULTI_WORK.EXE_GRAB_IMAGE, ref nIndexOfCam, ref arReceivedData))
                        {
                            byte[] arResult     = null;
                            UInt32 uLength      = DATA_LENGTH_INTEGER + DATA_LENGTH_INTEGER;  // Light, Grab Result

                            if(GetData(ref arReceivedData, ref arResult, ref uLength))
                            {
                                for(int nCount = 0 ; nCount < 2 ; ++ nCount)
                                {
                                    byte[] arData       = new byte[DATA_LENGTH_INTEGER];
                                    int nIndexOffset    = nCount * (int)DATA_LENGTH_INTEGER;

                                    for (int nIndex = 0; nIndex < DATA_LENGTH_INTEGER ; ++ nIndex)
                                    {
                                        arData[nIndex]  = arResult[nIndex + nIndexOffset];
                                    }

                                    int nResult         = BitConverter.ToInt32(arData, 0);

                                    if(nResult != VISION_SUCCESS)
                                    {
                                        return VISION_RESULT.ERROR_VISION;
                                    }
                                }

                                return VISION_RESULT.COMPLETE;
                            }
                        }
                    }
                    break;
            }

            return VISION_RESULT.IN_PROCESS;
        }
        /// <summary>
        /// 2020.11.24 by yjlee [ADD] Execute inspection of the vision.
        /// 2021.01.19 by yjlee [MOD] Change the format of the vision controller.
        /// </summary>
        public override VISION_RESULT ExecuteInspection(ref int nStep, ref int nIndexOfCam, ref VISION_REQUEST_DATA visionRequestData)
        {
            switch (nStep)
            {
                case 0:
                    if (IsCameraIndexVaild(ref nIndexOfCam)
                        && null != m_arSelectedAlgorithm[nIndexOfCam])
                    {
                        ClearDataInBuffer(TYPE_EXE_PROCEDURE, ref nIndexOfCam);

                        int nAlgorithm              = m_arAlgorithmOnCamera[nIndexOfCam];

                        byte[] arMethod             = BitConverter.GetBytes(METHOD_EXE);
                        byte[] arAlgorithm          = m_arSelectedAlgorithm[nIndexOfCam];
                        byte[] arAddtionalLength    = null;

                        List<UInt32> listLength     = new List<uint>();

                        listLength.Add(DATA_LENGTH_INTEGER);
                        listLength.Add(DATA_LENGTH_CHARACTER);
                        listLength.Add(DATA_LENGTH_INTEGER);

                        #region Set Addtional Length
                        // If the algorithm needs some data, set the additional data length.
                        switch (nAlgorithm)
                        {
                            case (int)EN_VISION_ALGORITHM.AUTO_FOCUS:
                                if(null == visionRequestData
                                    || null == visionRequestData.dataAutoFocus)
                                { 
                                    return VISION_RESULT.ERROR_COMMAND; 
                                }
    
                                listLength.Add(DATA_LENGTH_INTEGER);
                                listLength.Add(DATA_LENGTH_INTEGER);
                                listLength.Add(DATA_LENGTH_DOUBLE);

                                arAddtionalLength   = BitConverter.GetBytes(DATA_LENGTH_INTEGER + DATA_LENGTH_INTEGER + DATA_LENGTH_DOUBLE);
                                break;
                        
                            case (int)EN_VISION_ALGORITHM.CIRCLE_LEAST_SQUARE:
                                if(null == visionRequestData
                                    || null == visionRequestData.dataCircleLeastSquare)
                                { 
                                    return VISION_RESULT.ERROR_COMMAND; 
                                }

                                listLength.Add(DATA_LENGTH_INTEGER);

                                for(int nIndex = 0, nEnd = visionRequestData.dataCircleLeastSquare.nCountOfPoint
                                    ; nIndex < nEnd ; ++ nIndex)
                                {
                                    listLength.Add(DATA_LENGTH_DOUBLE);
                                    listLength.Add(DATA_LENGTH_DOUBLE);
                                }

                                arAddtionalLength   = BitConverter.GetBytes(DATA_LENGTH_INTEGER + (visionRequestData.dataCircleLeastSquare.nCountOfPoint * DATA_LENGTH_DOUBLE * 2));
                                break;

                            case (int)EN_VISION_ALGORITHM.DLC_R_GRID_CALIBRATION_PROC:
                            case (int)EN_VISION_ALGORITHM.DLC_L_GRID_CALIBRATION_PROC:
                                listLength.Add(DATA_LENGTH_INTEGER);

                                arAddtionalLength = BitConverter.GetBytes(DATA_LENGTH_INTEGER);

                                break;

                            default:
                                arAddtionalLength   = BYTE_ARRAY_ZERO;
                                break;
                        }
                        #endregion

                        List<byte[]> listTemp       = new List<byte[]>();
                        
                        listTemp.Add(arMethod);
                        listTemp.Add(arAlgorithm);
                        listTemp.Add(arAddtionalLength);

                        #region Set Addtional Data
                        // To request the inspection, have to send some data.
                        switch (nAlgorithm)
                        {
                            case (int)EN_VISION_ALGORITHM.AUTO_FOCUS:
                                visionRequestData.SetScale(0.001);  // mm -> um

                                listTemp.Add(BitConverter.GetBytes(visionRequestData.dataAutoFocus.nCountOfCurrentShot));
                                listTemp.Add(BitConverter.GetBytes(visionRequestData.dataAutoFocus.nCountOfTotalShot));
                                listTemp.Add(BitConverter.GetBytes(visionRequestData.dataAutoFocus.dblCurrentPosition));
                                break;

                            case (int)EN_VISION_ALGORITHM.CIRCLE_LEAST_SQUARE:
                                visionRequestData.SetScale(0.001);  // mm -> um

                                listTemp.Add(BitConverter.GetBytes(visionRequestData.dataCircleLeastSquare.nCountOfPoint));

                                var arTemp      = visionRequestData.dataCircleLeastSquare.arPoint;

                                for(int nIndex = 0, nEnd = visionRequestData.dataCircleLeastSquare.nCountOfPoint
                                    ; nIndex < nEnd ; ++ nIndex)
                                {
                                    listTemp.Add(BitConverter.GetBytes(arTemp[nIndex].X));
                                    listTemp.Add(BitConverter.GetBytes(arTemp[nIndex].Y));
                                }
                                break;

                            case (int)EN_VISION_ALGORITHM.DLC_R_GRID_CALIBRATION_PROC:
                            case (int)EN_VISION_ALGORITHM.DLC_L_GRID_CALIBRATION_PROC:
								if(visionRequestData == null)
								{
									UInt32 uIntfindGridPoint    = 0x00;
									listTemp.Add(BitConverter.GetBytes(uIntfindGridPoint));
								}
								else
								{
									listTemp.Add(BitConverter.GetBytes(visionRequestData.nWildNumber));
								}
                                break;
                            default:
                                break;
                        }
                        #endregion

                        byte[][] arSendData         = listTemp.ToArray();
                        UInt32[] arLength           = listLength.ToArray();

                        if(false == RequestToVisionController(TYPE_EXE_PROCEDURE, (UInt32)nIndexOfCam, listTemp.Count, ref arLength, ref arSendData))
                        {
                            return VISION_RESULT.ERROR_COMMAND;
                        }

                        ++ nStep;
                    }
                    else
                    {
                        return VISION_RESULT.ERROR_COMMAND;
                    }
                    break;

                case 1:
                    {
                        byte[] arReceivedData   = null;

                        if (ReceiveData(EN_MULTI_WORK.EXE_INSPECTION, ref nIndexOfCam, ref arReceivedData))
                        {
                            byte[] arResult     = null;
                            UInt32 uLength      = DATA_LENGTH_INTEGER + DATA_LENGTH_INTEGER + DATA_LENGTH_INTEGER;

                            if(GetData(ref arReceivedData, ref arResult, ref uLength))
                            {
                                // 2021.01.19 by yjlee [ADD] Must check the third byte result.
                                int nResult     = BitConverter.ToInt32(arResult, (int)(DATA_LENGTH_INTEGER + DATA_LENGTH_INTEGER));

                                if (VISION_SUCCESS == nResult)
                                {
                                    AddResultToBuffer(ref nIndexOfCam, ref m_arAlgorithmOnCamera[nIndexOfCam], ref arReceivedData);

                                    return VISION_RESULT.COMPLETE;
                                }
                                else
                                {
                                    return VISION_RESULT.ERROR_VISION;
                                }
                            }

                            return VISION_RESULT.ERROR_COMMAND;
                        }
                    }
                    break;
            }

            return VISION_RESULT.IN_PROCESS;
        }

        /// <summary>
        /// 2020.11.13 by yjlee [ADD] Get the vision result of the camera.
        /// </summary>
		public override VISION_RESULT GetInspectionData(ref int nIndexOfCam, ref VISION_DATA visionData)
        {
            if (IsCameraIndexVaild(ref nIndexOfCam))
            {
                if (GetResultToBuffer(ref nIndexOfCam, ref visionData))
                {
                    return VISION_RESULT.COMPLETE;
                }
            }
            
            return VISION_RESULT.ERROR_COMMAND;
        }
        #endregion

        #region Message

        #region Variables
        static private Queue<string> m_queueForMessageLogging       = new Queue<string>();
        static private System.Threading.ReaderWriterLockSlim m_pLockForLogging  = new System.Threading.ReaderWriterLockSlim();
        #endregion

        /// <summary>
        /// 2020.11.18 by yjlee [ADD] Add a history with the communication.
        /// </summary>
        static private void AddHistory(bool bSend, ref byte[] arData)
        {
            //FrameOfSystem3.Log.LogManager.GetInstance().WriteCommunicationLog(string.Format("[{0}]\t{1}", bSend ? "SEND" : "RECV", BitConverter.ToString(arData)));

            m_pLockForLogging.EnterWriteLock();

            //m_queueForMessageLogging.Enqueue(string.Format("[{0}]\t{1}", bSend ? "SEND" : "RECV", BitConverter.ToString(arData)));

            m_pLockForLogging.ExitWriteLock();
        }
        /// <summary>
        /// 2020.11.18 by yjlee [ADD] Get the history wi the communication.
        /// </summary>
        static public bool GetHistory(ref string[] arData)
        {
            bool bReturnValue       = false;

            m_pLockForLogging.EnterWriteLock();

            int nCountOfQueue       = m_queueForMessageLogging.Count;

            if(nCountOfQueue > 0)
            {
                arData              = new string[nCountOfQueue];

                int nIndex          = -1;

                while(m_queueForMessageLogging.Count > 0)
                {
                    arData[++ nIndex]   = m_queueForMessageLogging.Dequeue();
                }

                bReturnValue        = true;
            }

            m_pLockForLogging.ExitWriteLock();

            return bReturnValue;
        }
        #endregion

        #endregion
    }
}
