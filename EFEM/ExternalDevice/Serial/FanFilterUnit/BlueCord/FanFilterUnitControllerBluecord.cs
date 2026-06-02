using System;
using System.Text;

using System.Threading;

using System.Collections.Concurrent;
using Define.DefineEnumBase.Common;
using EFEM.Defines.Communicator;
using TickCounter_;

namespace FrameOfSystem3.ExternalDevice.Serial.FanFilterUnit.Bluecord
{
    /// <summary>
    /// Anderson-FFU
    /// </summary>
    public class FanFilterUnitControllerBluecord : FanFilterUnitController
    {
        #region <Constructors>
        public FanFilterUnitControllerBluecord(int commIndex, EN_CONNECTION_TYPE commType, bool useDifferentialPressureMode, int unitCount)
            : base(commIndex, commType, useDifferentialPressureMode, unitCount)
        {
            UnitItems = new ConcurrentDictionary<int, UnitItem>();
            for (int i = 0; i < unitCount; ++i)
            {
                var unitInfo = new UnitItem
                {
                    UnitId = i
                };

                UnitItems[i] = unitInfo;
            }
        }
        #endregion </Constructors>

        #region <Fields>
        private readonly byte[] STX = { 0x02 };
        private readonly byte[] ETX = { 0x03 };

        private readonly byte MODE_1_READ = 0x8A;
        private readonly byte MODE_1_WRITE = 0x89;
        private readonly byte MODE_2 = 0x9f;
        private readonly byte MODE_PRESSURE_1 = 0xA1;       // 2024.12.27. by dwlim [ADD] PRESSURE SET Command 추가
        //private readonly byte MODE_PRESSURE_2 = 0xA2;     

        //private readonly byte FLAG_OK = 0xB9;

        private readonly byte DPU_ID = 0x9F;
        private readonly byte LCU_START_ID = 0x81;

        private readonly int ACK_START_DATA_BYTE = 5;
        private readonly int ACK_NUMBER_OF_DATA_BYTES = 6;

        private readonly ConcurrentQueue<byte[]> RequestMessage = new ConcurrentQueue<byte[]>();
        private readonly ConcurrentDictionary<int, UnitItem> UnitItems = null;
        private byte[] _receivedMessage = new byte[1];

        private int _getStatusStep = 0;     // 2024.12.27. by dwlim [ADD] EXECUTE Set Speed, Get Speed 분리
        private int _setSpeedStep = 0;      // 2024.12.27. by dwlim [ADD] EXECUTE Set Speed, Get Speed 분리
        private int _currentIndex = 0;
        private readonly TickCounter TickCounter = new TickCounter();
        #endregion </Fields>

        #region <Properties>
        #endregion </Properties>

        #region <Types>
        private enum EN_DATA_STRUCT
        {
            LCU_ID = 0,
            PV = 1, // Current velocity
            ALARM_STATE = 2,
            SV = 3, // Set Velocity
            DATA_mmAQ_LSV = 4, // Set Velocity
            DATA_mmAQ_HSV = 5, // Set Velocity
        }
        #endregion </Types>

        #region <Methods>
               
        #region <Override>
        public override bool Connect()
        {
            return Communicator.OpenPort();
        }
        public override bool DisConnect()
        {
            return Communicator.ClosePort();
        }
        public override bool SetSpeedAll(double speed)
        {
            byte[] messageToSend = MakeMessageSetSpeed((int)speed);
            RequestMessage.Enqueue(messageToSend);
            return true;
        }
        public override bool GetInformation(int index, ref UnitItem unitItem)
        {
            return UnitItems.TryGetValue(index, out unitItem);
        }
        public override bool GetUnitSpeedAll(ref double[] speed)
        {
            if (speed.Length != UnitItems.Count)
            {
                return false;
            }

            int index = 0;
            foreach (var item in UnitItems)
            {
                speed[index++] = item.Value.CurrentSpeed;
            }

            return true;
        }
        // 2024.12.27. by dwlim [ADD] 차압 Setting Command 추가
        public override bool SetUnitDifferentialPressure(double pressure)
        {
            int npressure = (int)Math.Round(pressure * 100);
            byte[] messageToSend = MakeMessageSetPressure(npressure);
            RequestMessage.Enqueue(messageToSend);
            return true;
        }
        public override bool GetUnitDifferentialPresureAll(ref double[] pressure)
        {
            if (pressure.Length != UnitItems.Count)
            {
                return false;
            }

            int index = 0;
            foreach (var item in UnitItems)
            {
                pressure[index++] = item.Value.CurrentDifferentialPressure;
            }

            return true;
        }
        public override bool GetUnitStatusAll(ref string[] status)
        {
            if (status.Length != UnitItems.Count)
            {
                return false;
            }

            int index = 0;
            foreach (var item in UnitItems)
            {
                status[index++] = item.Value.Status.ToString();
            }
            return true;
        }
        // 2024.12.27. by dwlim [MOD] EXECUTE Set Speed, Get Speed 분리
        public override void Execute()
        {
            if (RequestMessage.Count > 0)
            {
                _getStatusStep = 0;
                switch (_setSpeedStep)
                {
                    case 0:
                        byte[] bytesToSend;
                        if (false == RequestMessage.TryPeek(out bytesToSend))
                            break;

                        if (false == Communicator.WriteByteData(bytesToSend))
                            break;

                        TickCounter.SetTickCount(1000);
                        _setSpeedStep = 10; break;

                    case 10:
                        if (TickCounter.IsTickOver(true))
                        {
                            RequestMessage.TryDequeue(out _);
                            _setSpeedStep = 0; break;
                        }

                        if (false == Communicator.ReadByteData(ref _receivedMessage))
                            break;
                        // 2024.12.29. by dwlim [ADD] Setting된 Pressure를 Get하는 기능이 Manuaul에 없음 - 업체확인필요
                        //ParsePressureData(_receivedMessage, _currentIndex);
                        ParseData(_receivedMessage);

                        RequestMessage.TryDequeue(out _);
                        _setSpeedStep = 0; 
                        break;
                }
            }
            else
            {
                _setSpeedStep = 0;
                switch (_getStatusStep)
                {
                    case 0:
                        _currentIndex = 0;
                        if (false == IsConnected)
                        {
                            TickCounter.SetTickCount(1000);
                            ++_getStatusStep;
                        }
                        else
                        {
                            _getStatusStep = 10;
                        }
                        break;
                    case 1:
                        if (false == TickCounter.IsTickOver(true))
                            break;

                        Connect();
                        --_getStatusStep; break;

                    case 10:
                        if (_currentIndex >= UnitCount)
                        {
                            _currentIndex = 0;
                        }
                        TickCounter.SetTickCount(500);
                        ++_getStatusStep; break;

                    case 11:
                        if (false == TickCounter.IsTickOver(true))
                            break;
                        ++_getStatusStep; break;

                    case 12:
                        byte[] bytesToSend = MakeMessageGetStatus(_currentIndex);
                        if (false == Communicator.WriteByteData(bytesToSend))
                            break;

                        TickCounter.SetTickCount(500);
                        ++_getStatusStep; break;

                    case 13:
                        if (TickCounter.IsTickOver(true))
                        {
                            ++_currentIndex;
                            _getStatusStep = 0; break;
                        }

                        if (false == Communicator.ReadByteData(ref _receivedMessage))
                            break;

                        ParseData(_receivedMessage);
                        // 2024.12.29. dwlim [ADD] Setting된 Pressure를 Get하는 기능이 Manuaul에 없음 - 업체확인필요
                        //    ++_getStatusStep; break;

                        //case 14:
                        //    bytesToSend = MakeMessageGetSettingPressure(/*_currentIndex*/);
                        //    if (false == Communicator.WriteByteData(bytesToSend))
                        //        break;

                        //    TickCounter.SetTickCount(500);
                        //    ++_getStatusStep; break;

                        //case 15:
                        //    if (TickCounter.IsTickOver(true))
                        //    {
                        //        ++_currentIndex;
                        //        _getStatusStep = 0; break;
                        //    }

                        //    if (false == Communicator.ReadByteData(ref _receivedMessage))
                        //        break;

                        //    ParsePressureData(_receivedMessage, _currentIndex);

                        ++_currentIndex;
                        _getStatusStep = 10; break;
                    default:
                        break;
                }
            }
        }
        #endregion </Override>

        #region <Internal>
        private byte[] MakeMessageGetStatus(int nLCUNo)
        {
            byte[] bytesToSend = new byte[9];

            bytesToSend[0] = STX[0];
            bytesToSend[1] = MODE_1_READ;
            bytesToSend[2] = MODE_2;
            bytesToSend[3] = 0x81;
            bytesToSend[4] = DPU_ID;

            byte bStartLCU = Convert.ToByte((int)LCU_START_ID + nLCUNo);
            bytesToSend[5] = bStartLCU;
            bytesToSend[6] = bStartLCU;

            bytesToSend[7] = MakeChecksum(bytesToSend, 1, 6);
            bytesToSend[8] = ETX[0];

            return bytesToSend;
        }
        private byte[] MakeMessageSetSpeed(int nVelocity)
        {
            int velocity = nVelocity/10;    // 2024.12.27. dwlim [ADD]
            byte[] bytesToSend = new byte[10];

            bytesToSend[0] = STX[0];
            bytesToSend[1] = MODE_1_WRITE;
            bytesToSend[2] = 0x84;
            bytesToSend[3] = LCU_START_ID;
            bytesToSend[4] = DPU_ID;

            byte bStartLCU = Convert.ToByte((int)LCU_START_ID);
            bytesToSend[5] = bStartLCU;
            bytesToSend[6] = Convert.ToByte((int)LCU_START_ID + UnitCount - 1);
            bytesToSend[7] = (byte)velocity;
            bytesToSend[8] = MakeChecksum(bytesToSend, 1, 7); ;// MakeChecksum(MODE_1_WRITE, 0x84, nVelocity);

            bytesToSend[9] = ETX[0];

            return bytesToSend;
        }
        // 2024.12.29. dwlim [ADD] Setting된 Pressure를 Get하는 기능이 Manuaul에 없음 - 업체확인필요
        //private byte[] MakeMessageGetSettingPressure(/*int nLCUNo*/)
        //{
        //    byte[] bytesToSend = new byte[5];

        //    bytesToSend[0] = STX[0];
        //    bytesToSend[1] = MODE_PRESSURE_2;

        //    byte bStartLCU = Convert.ToByte((int)LCU_START_ID/* + nLCUNo*/);
        //    bytesToSend[2] = bStartLCU;

        //    bytesToSend[3] = MakeChecksum(bytesToSend, 1, 2);
        //    bytesToSend[4] = ETX[0];

        //    return bytesToSend;
        //}

        // 2024.12.27. by dwlim [ADD] 차압 Setting Command 추가
        private byte[] MakeMessageSetPressure(int nPressure)
        {
            int pressure = nPressure + 50;
            byte[] bytesToSend = new byte[10];

            bytesToSend[0] = STX[0];
            bytesToSend[1] = MODE_PRESSURE_1;
            bytesToSend[2] = LCU_START_ID;
            bytesToSend[3] = (byte)(pressure >> 8);
            bytesToSend[4] = (byte)(pressure & 0xff);
            bytesToSend[5] = MakeChecksum(bytesToSend, 1, 4);
            bytesToSend[6] = ETX[0];

            return bytesToSend;
        }
        private byte MakeChecksum(byte[] bArrToCheck, int nStartIndex, int nEndIndex)
        {
            int nTemp = 0;
            if (bArrToCheck.Length == 0)
            {
                return 0;
            }

            for (int i = nStartIndex; i <= nEndIndex; i++)
            {
                nTemp += bArrToCheck[i];
            }

            byte byteChecksum = (byte)(255 & nTemp);

            return byteChecksum;
        }
        private bool ParseData(byte[] bytesData)
        {
            bool bRetAlarm = false;
            //byte[] bytesReceiveData;
            try
            {
                if (bytesData.Length != 13)
                {
                    return false;
                }

                byte byteAlarm;
                int nLCUCount = 0;

                int nIndexAlarmByte = ACK_START_DATA_BYTE + nLCUCount * ACK_NUMBER_OF_DATA_BYTES
                    + (int)EN_DATA_STRUCT.ALARM_STATE;
                if (nIndexAlarmByte > bytesData.Length)
                {
                    return false;
                }

                int nIndexLCUNo = ACK_START_DATA_BYTE + nLCUCount * ACK_NUMBER_OF_DATA_BYTES
                    + (int)EN_DATA_STRUCT.LCU_ID;
                int nLCUNo = bytesData[nIndexLCUNo] - LCU_START_ID;
                //System.Console.WriteLine(nLCUNo);
                byteAlarm = bytesData[nIndexAlarmByte];
                FFU_ErrorCode errorCode = FFU_ErrorCode.NoConnection;
                if (byteAlarm == (byte)FFU_ErrorCode.RemoteRun)
                {
                    errorCode = FFU_ErrorCode.RemoteRun;
                    bRetAlarm = false;
                    //strReturn += "REMOTE RUN";
                }
                else if (byteAlarm == (byte)FFU_ErrorCode.LocalRun)
                {
                    errorCode = FFU_ErrorCode.LocalRun;
                    //strReturn += string.Format("[FFU #{0}] Motor Error \n", nLCUNo + 1);
                    bRetAlarm = true;
                }
                else if (byteAlarm == (byte)FFU_ErrorCode.Setting_Alarm)
                {
                    errorCode = FFU_ErrorCode.Setting_Alarm;
                    //strReturn += string.Format("[FFU #{0}] Motor Error \n", nLCUNo + 1);
                    bRetAlarm = true;
                }
                else if (byteAlarm == (byte)FFU_ErrorCode.RPM_Alarm)
                {
                    errorCode = FFU_ErrorCode.RPM_Alarm;
                    //strReturn += string.Format("[FFU #{0}] Motor Error \n", nLCUNo + 1);
                    bRetAlarm = true;
                }
                else if (byteAlarm == (byte)FFU_ErrorCode.AutoControlling)
                {
                    errorCode = FFU_ErrorCode.AutoControlling;
                    //strReturn += string.Format("[FFU #{0}] Motor Error \n", nLCUNo + 1);
                    bRetAlarm = true;
                }
                else if (byteAlarm == (byte)FFU_ErrorCode.OverCurrent)
                {
                    errorCode = FFU_ErrorCode.OverCurrent;
                    //strReturn += string.Format("[FFU #{0}] Motor Error \n", nLCUNo + 1);
                    bRetAlarm = true;
                }
                else if (byteAlarm == (byte)FFU_ErrorCode.MotorError)
                {
                    errorCode = FFU_ErrorCode.MotorError;
                    //strReturn += string.Format("[FFU #{0}] Motor Error \n", nLCUNo + 1);
                    bRetAlarm = true;
                }
                else if (byteAlarm == (byte)FFU_ErrorCode.AbnormalState)
                {
                    errorCode = FFU_ErrorCode.AbnormalState;
                    //strReturn += string.Format("[FFU #{0}] Abnomal State \n", nLCUNo + 1);
                    bRetAlarm = true;
                }
                else if (byteAlarm == (byte)FFU_ErrorCode.NoConnection)
                {
                    errorCode = FFU_ErrorCode.NoConnection;
                    //strReturn += string.Format("[FFU] 통신 ALARM \n", nLCUNo + 1);
                    bRetAlarm = true;
                }
                else
                {
                    Console.Write(string.Format("FFU Unit {0} - Invalide Alarm Code : {1}", nLCUNo, byteAlarm));
                    return true;
                }

                // 2024.12.29. by dwlim [ADD] 정상인 상태에서는 Error Code 안보이도록 수정
                UnitItems[nLCUNo].Status = errorCode.ToString();
                UnitItems[nLCUNo].IsError = bRetAlarm;
                if (errorCode == FFU_ErrorCode.LocalRun || errorCode == FFU_ErrorCode.RemoteRun)
                {
                    UnitItems[nLCUNo].AlarmCode = string.Empty;
                }
                // 2024.12.29. by dwlim [END]

                //if(bRetAlarm == false)
                {
                    // No Alarm then parsing PV
                    int nIndexCurVel = ACK_START_DATA_BYTE + nLCUCount * ACK_NUMBER_OF_DATA_BYTES + (int)EN_DATA_STRUCT.PV;
                    int nIndexSetVel = ACK_START_DATA_BYTE + nLCUCount * ACK_NUMBER_OF_DATA_BYTES + (int)EN_DATA_STRUCT.SV;

                    //m_listOfConfiguredSpeed[nLCUNo] = (int)bytesData[nIndexSetVel] * 10;
                    UnitItems[nLCUNo].CurrentSpeed = (int)bytesData[nIndexCurVel] * 10;
                    UnitItems[nLCUNo].SettingSpeed = (int)bytesData[nIndexSetVel] * 10;

                    int nLowIndex = ACK_START_DATA_BYTE + nLCUCount * ACK_NUMBER_OF_DATA_BYTES + (int)EN_DATA_STRUCT.DATA_mmAQ_LSV;
                    int nHighIndex = ACK_START_DATA_BYTE + nLCUCount * ACK_NUMBER_OF_DATA_BYTES + (int)EN_DATA_STRUCT.DATA_mmAQ_HSV;
                    byte bLow = bytesData[nLowIndex];
                    byte bHigh = bytesData[nHighIndex];

                    UnitItems[nLCUNo].CurrentDifferentialPressure = (double)(bHigh * Math.Pow(2, 8) + bLow) / 100;
                }
            }
            catch
            {

            }

            //byte byteAlarm;
            //byte[] bytesData = new byte[bytesReceiveData.Length];
            //Array.Copy(bytesReceiveData, bytesData, bytesReceiveData.Length);

            //if (bytesData.Length != 13)
            //{               
            //    return false;
            //}

            //bool bRetAlarm = false;
            //int nLCUCount = 0;

            //int nIndexAlarmByte = ACK_START_DATA_BYTE + nLCUCount * ACK_NUMBER_OF_DATA_BYTES 
            //    + (int)EN_DATA_STRUCT.ALARM_STATE;
            //if (nIndexAlarmByte > bytesData.Length)
            //{
            //    return false;
            //}

            //int nIndexLCUNo = ACK_START_DATA_BYTE + nLCUCount * ACK_NUMBER_OF_DATA_BYTES
            //    + (int)EN_DATA_STRUCT.LCU_ID;
            //int nLCUNo = bytesData[nIndexLCUNo] - LCU_START_ID;

            //byteAlarm = bytesData[nIndexAlarmByte];
            //if (byteAlarm == (byte)FFU_ErrorCode.RemoteRun)
            //{
            //    errorCodes[nLCUNo] = FFU_ErrorCode.RemoteRun;
            //    //strReturn += "REMOTE RUN";
            //}
            //else if (byteAlarm == (byte)FFU_ErrorCode.LocalRun)
            //{
            //    errorCodes[nLCUNo] = FFU_ErrorCode.LocalRun;
            //    //strReturn += string.Format("[FFU #{0}] Motor Error \n", nLCUNo + 1);
            //    bRetAlarm = true;
            //}
            //else if (byteAlarm == (byte)FFU_ErrorCode.Setting_Alarm)
            //{
            //    errorCodes[nLCUNo] = FFU_ErrorCode.Setting_Alarm;
            //    //strReturn += string.Format("[FFU #{0}] Motor Error \n", nLCUNo + 1);
            //    bRetAlarm = true;
            //}
            //else if (byteAlarm == (byte)FFU_ErrorCode.RPM_Alarm)
            //{
            //    errorCodes[nLCUNo] = FFU_ErrorCode.RPM_Alarm;
            //    //strReturn += string.Format("[FFU #{0}] Motor Error \n", nLCUNo + 1);
            //    bRetAlarm = true;
            //}
            //else if (byteAlarm == (byte)FFU_ErrorCode.AutoControlling)
            //{
            //    errorCodes[nLCUNo] = FFU_ErrorCode.AutoControlling;
            //    //strReturn += string.Format("[FFU #{0}] Motor Error \n", nLCUNo + 1);
            //    bRetAlarm = true;
            //}
            //else if (byteAlarm == (byte)FFU_ErrorCode.OverCurrent)
            //{
            //    errorCodes[nLCUNo] = FFU_ErrorCode.OverCurrent;
            //    //strReturn += string.Format("[FFU #{0}] Motor Error \n", nLCUNo + 1);
            //    bRetAlarm = true;
            //}
            //else if (byteAlarm == (byte)FFU_ErrorCode.MotorError)
            //{
            //    errorCodes[nLCUNo] = FFU_ErrorCode.MotorError;
            //    //strReturn += string.Format("[FFU #{0}] Motor Error \n", nLCUNo + 1);
            //    bRetAlarm = true;
            //}
            //else if (byteAlarm == (byte)FFU_ErrorCode.AbnormalState)
            //{
            //    errorCodes[nLCUNo] = FFU_ErrorCode.AbnormalState;
            //    //strReturn += string.Format("[FFU #{0}] Abnomal State \n", nLCUNo + 1);
            //    bRetAlarm = true;
            //}
            //else if (byteAlarm == (byte)FFU_ErrorCode.NoConnection)
            //{
            //    errorCodes[nLCUNo] = FFU_ErrorCode.NoConnection;
            //    //strReturn += string.Format("[FFU] 통신 ALARM \n", nLCUNo + 1);
            //    bRetAlarm = true;
            //}

            //// No Alarm then parsing PV
            //int nIndexCurVel = ACK_START_DATA_BYTE + nLCUCount * ACK_NUMBER_OF_DATA_BYTES + (int)EN_DATA_STRUCT.PV;
            //int nIndexSetVel = ACK_START_DATA_BYTE + nLCUCount * ACK_NUMBER_OF_DATA_BYTES + (int)EN_DATA_STRUCT.SV;

            ////m_listOfConfiguredSpeed[nLCUNo] = (int)bytesData[nIndexSetVel] * 10;
            //currentSpeed[nLCUNo] = (int)bytesData[nIndexCurVel] * 10;

            //int nLowIndex = ACK_START_DATA_BYTE + nLCUCount * ACK_NUMBER_OF_DATA_BYTES + (int)EN_DATA_STRUCT.DATA_mmAQ_LSV;
            //int nHighIndex = ACK_START_DATA_BYTE + nLCUCount * ACK_NUMBER_OF_DATA_BYTES + (int)EN_DATA_STRUCT.DATA_mmAQ_HSV;
            //byte bLow = bytesData[nLowIndex];
            //byte bHigh = bytesData[nHighIndex];
            //diffPreasure[nLCUNo] = (double)(bHigh * Math.Pow(2, 8) + bLow) / 100;

            return bRetAlarm;
        }

        // 2024.12.29. dwlim [ADD] Setting된 Pressure를 Get하는 기능이 Manuaul에 없음 - 업체확인필요
        //private bool ParsePressureData(byte[] bytesData, int nLCUNo)
        //{
        //    if (bytesData.Length != 7)
        //    {
        //        return false;
        //    }
        //    if (!(bytesData[0] == STX[0] && bytesData[1] == MODE_PRESSURE_2 && bytesData[6] == ETX[0]))
        //    {
        //        return false;
        //    }
        //    //int nIndexLCUNo = ACK_PRESSURE_START_DATA_BYTE;
        //    //int LCUNo = nLCUNo - (int)LCU_START_ID;

        //    int nLowIndex = ACK_PRESSURE_START_DATA_BYTE + 1;
        //    int nHighIndex = ACK_PRESSURE_START_DATA_BYTE + 2;
        //    byte bLow = bytesData[nLowIndex];
        //    byte bHigh = bytesData[nHighIndex];

        //    UnitItems[nLCUNo].SettingDifferentialPressure = (double)((bLow * Math.Pow(2, 8) + bHigh - 50) / 100);

        //    return true;
        //}
        #endregion </Internal>

        #endregion </Methods>
    }

    #region define
    /// <summary>
    /// API RETURN value
    /// </summary>
    public enum FFU_RET
	{
		SUCCESS = 0,
		FAIL,
		ABORT,
		TIMEOUT
	}

	/// <summary>
	/// FFU Communication State Enum
	/// </summary>
	public enum FFU_COMM
	{
		OFFLINE = 0,
		ONLINE,
		ERROR,
	}

	/// <summary>
	/// FFU Device State Enum
	/// </summary>
	public enum FFU_DEV
	{
		READY = 0,
		BUSY,
		ERROR,
	}

	/// <summary>
	/// FFU Error Code
	/// </summary>
	public enum FFU_ErrorCode
	{
		//NoConnection = 0,
		//FanOperation = 1,
		//DiffPressureInputSensor = 2,
		//RPMHighSetting = 3,
		//RPMLowSetting = 4,
		//DiffPressureHighSetting = 5,
		//DiffPressureLowSetting = 6,
		//StabilizationTimeOver = 7,
		//NoError = 8

		// 0901
		NoConnection = 0,
		RemoteRun = 128,
		LocalRun = 129,
		Setting_Alarm=132,
		RPM_Alarm=133,
		AutoControlling=134,
		OverCurrent=135,
		MotorError = 162,
		AbnormalState = 192,
		NoError = 9999,
	}
	#endregion
}
