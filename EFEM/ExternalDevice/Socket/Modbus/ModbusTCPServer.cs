using System;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

using Define.DefineConstant;
using Define.DefineEnumProject.Modbus;
using FrameOfSystem3.ExternalDevice.Modbus;

using FileIOManager_;
using FileComposite_;

namespace FrameOfSystem3.ExternalDevice.Socket
{
 
    class ModbusTCPServer
    {
        #region 변수
        private int m_nSocketIndex;

        private Config.ConfigSocket m_Socket = Config.ConfigSocket.GetInstance();
        private Dictionary<int, ModbusTCPServerDevice> m_dicOfModbus = new Dictionary<int, ModbusTCPServerDevice>(); //key = Modbus SlaveID

        private int m_nObjectCode;

        private TickCounter_.TickCounter m_TickForTimeOut = new TickCounter_.TickCounter();
        #endregion

        #region 싱글톤
        private ModbusTCPServer(int nSocketIndex)
        {
            m_nSocketIndex = nSocketIndex;

            m_nObjectCode = nSocketIndex * 100;
        }
        private static Dictionary<int, ModbusTCPServer> _Instance = null;
        public static ModbusTCPServer GetInstance(int nSocketIndex) 
        {
            if (_Instance == null)
            {
                _Instance = new Dictionary<int, ModbusTCPServer>();
            }
            if (!_Instance.ContainsKey(nSocketIndex))
            {
                _Instance.Add(nSocketIndex, new ModbusTCPServer(nSocketIndex));
            }

            return _Instance[nSocketIndex];
        }
        #endregion

        #region 주기 호출 함수
        public void Execute()
        {
            foreach (var kpv in m_dicOfModbus)
            {
                if (m_Socket.IsConnected(ref m_nSocketIndex) == false)
                    return;

                byte[] ReturnData = null;

                byte[] ReciveData = null;
                if (m_Socket.Receive(m_nSocketIndex, ref ReciveData))
                {
                    kpv.Value.ParsingData(ReciveData, ref ReturnData);
                }
                if (ReturnData != null)
                {
                    m_Socket.send(m_nSocketIndex, ReturnData);
                }
            }
        }
        #endregion


        #region 외부 인터페이스
        #region 초기화 및 종료
        public bool Init()
        {

            if (m_Socket.Connect(ref m_nSocketIndex) == false)
                return false;


            return true;
        }

        public void AddDevice(ModbusTCPServerDevice enDevice)
        {
            m_dicOfModbus.Add(enDevice.bSlaveID, enDevice);
        }
        public void Close()
        {
            m_Socket.DisConnect(ref m_nSocketIndex);
        }
        #endregion

        #region 아이템 값 반환
        /// <summary>
        /// 2018.08.31 by yjlee [ADD] 통신이 열린 상태인지 확인한다.
        /// </summary>
        public bool IsOpened()
        {
            return m_Socket.IsConnected(ref m_nSocketIndex);
        }
        #endregion

        public void SetReadWriteRegister(int nSlaveIndex, int nAddress, string strData, int nLength = -1)
        {
            m_dicOfModbus[nSlaveIndex].SetReadWriteRegister(nAddress, strData, nLength);
        }

        public string GetReadWriteRegister(int nSlaveIndex, int nAddress, int nLength)
        {
            return m_dicOfModbus[nSlaveIndex].GetReadWriteRegister(nAddress, nLength);
        }
        #endregion

        #region 내부 인터페이스
    
        #endregion
    }

    public class ModbusTCPServerDevice
    {

        #region 상수
        protected const byte Read_Coil_Status = 1;
        protected const byte Read_Input_Status = 2;
        protected const byte Read_Holding_Register = 3;
        protected const byte Read_Input_Register = 4;

        protected const byte Write_Single_Coil = 5;
        protected const byte Write_Multi_Coil = 15;
        protected const byte Write_Single_Register = 6;
        protected const byte Write_Multi_Register = 16;

        protected const ushort Protocol_ID = 0;

        #endregion

        #region 변수
        private byte m_bSlaveID;

        private EN_MODBUS_STATUS m_enStatus = EN_MODBUS_STATUS.SETTING_COMMAND;

        protected byte m_nFunctionCode;
        protected ushort m_nRequestAddress;
        protected ushort m_nRequestCount;
        protected ushort m_nTransaction_ID = 0;

        protected ModbusData m_pServerData = new ModbusData();
        private EN_MODBUS_SERVER_PROTOCOL m_enProtocol = EN_MODBUS_SERVER_PROTOCOL.TCP;
        #endregion

        public ModbusTCPServerDevice(byte SlaveID, EN_MODBUS_SERVER_PROTOCOL enProtocol)
        {
            m_bSlaveID = SlaveID;
            m_enProtocol = enProtocol;
        }

        #region 속성
        public byte bSlaveID { get { return m_bSlaveID; } }

        public EN_MODBUS_STATUS enStatus { set { m_enStatus = value; } get { return m_enStatus; } }
        public EN_MODBUS_SERVER_PROTOCOL enProtocol { get { return m_enProtocol; } }
        #endregion

        public bool ParsingData(byte[] RecieveData, ref byte[] ReturnData)
        {
        
            switch(enProtocol)
            {
                case EN_MODBUS_SERVER_PROTOCOL.TCP:
                    return ParseTCP(RecieveData, ref ReturnData);

                case EN_MODBUS_SERVER_PROTOCOL.RTU_OVER_TCP:
                    return ParseRTU(RecieveData, ref ReturnData);
            }
            return false;
        }

        //적합성 검사
        public bool CheckRecieveData(byte[] arRecieveData)
        {
           
         
//             switch (m_nFunctionCode)
//             {
//                 case 1:
//                 case 2:
//                     if (arRecieveData.Length - 8        // HEADER, Function Code 제외
//                         != ((m_nRequestCount - 1) / 8) + 1) //Bit로 ON/OFF가 표시됨으로
//                         return false;
//                     break;
// 
//                 case 3:
//                 case 4:
//                     if(arRecieveData[8] != m_nRequestCount * 2) //byte 갯수
//                         return false;
//                     if (arRecieveData.Length - 9        // HEADER, Function Code, Length 제외
//                        != m_nRequestCount * 2)  //2byte 값으로 받는다.
//                         return false;
//                     break;
//             }
            return true;
        }
        
      

        private bool ParseTCP(byte[] RecieveData, ref byte[] ReturnData)
        {
            byte[] ArrayLittleEndian;

            ArrayLittleEndian = RecieveData.Skip(4).Take(2).Reverse().ToArray();
            if (BitConverter.ToUInt16(ArrayLittleEndian, 0) != RecieveData.Length - 6)//Slave ID + Function Code + Data 길이
                return false;
            if (RecieveData[6] != bSlaveID)
                return false;

            byte FunctionCode = RecieveData[7];
            switch (FunctionCode)//funcutionCode
            {
                case Read_Coil_Status:
                case Read_Input_Status:
                    ArrayLittleEndian = RecieveData.Skip(8).Take(2).Reverse().ToArray();
                    int FirstStatusAddress = BitConverter.ToUInt16(ArrayLittleEndian, 0);
                    if (FirstStatusAddress > 10000)
                        FirstStatusAddress = 10000;
                    ArrayLittleEndian = RecieveData.Skip(10).Take(2).Reverse().ToArray();
                    int RequestStatusLength = BitConverter.ToUInt16(ArrayLittleEndian, 0);
                    if (RequestStatusLength > 10000)
                        RequestStatusLength = 10000;
                    int nStatusDataLength = ((RequestStatusLength - 1) / 8) + 1;
                    ReturnData = new byte[nStatusDataLength + 9];

                    int nDataLent = ReturnData.Length - 6; //Transaction Identifier, Protocol_ID 제외한 Data 길이

                    ReturnData[0] = RecieveData[0];
                    ReturnData[1] = RecieveData[1]; //Transaction Identifier : 식별자 요청과 똑같이 Return 한다.
                    ReturnData[2] = (byte)((Protocol_ID & 0xff00) >> 8);
                    ReturnData[3] = (byte)(Protocol_ID & 0xff); //Protocol_ID는 항상 0;
                    ReturnData[4] = (byte)((nDataLent & 0xff00) >> 8);
                    ReturnData[5] = (byte)(nDataLent & 0xff);
                    ReturnData[6] = m_bSlaveID;
                    ReturnData[7] = FunctionCode; //function Code;
                    ReturnData[8] = (byte)nStatusDataLength;
                    int nBitIndex = 0;
                    int nDataIndex = 9;
                    for (int nAddress = FirstStatusAddress; nAddress < FirstStatusAddress + RequestStatusLength; nAddress++)
                    {
                        if (nAddress > 9999)
                            break;
                        byte CoilData = 0x00;
                        if (FunctionCode == Read_Coil_Status)
                        {
                            if (m_pServerData.arReadWriteStatus[nAddress])
                            {
                                CoilData = 0x01;
                            }
                        }
                        if (FunctionCode == Read_Input_Status)
                        {
                            if (m_pServerData.arReadOnlyStatus[nAddress])
                            {
                                CoilData = 0x01;
                            }
                        }
                        CoilData = (byte)(CoilData << nBitIndex);
                        ReturnData[nDataIndex] = (byte)(ReturnData[nDataIndex] | CoilData);

                        nBitIndex++;
                        if (nBitIndex >= 8)
                        {
                            nBitIndex = 0;
                            nDataIndex++;
                        }
                    }
                    break;

                case Read_Holding_Register:
                case Read_Input_Register:
                    ArrayLittleEndian = RecieveData.Skip(8).Take(2).Reverse().ToArray();
                    int FirstRegisterAddress = BitConverter.ToUInt16(ArrayLittleEndian, 0);
                    if (FirstRegisterAddress > 10000)
                        FirstRegisterAddress = 10000;
                    ArrayLittleEndian = RecieveData.Skip(10).Take(2).Reverse().ToArray();
                    int RequestRegisterLength = BitConverter.ToUInt16(ArrayLittleEndian, 0);
                    if (RequestRegisterLength > 10000)
                        RequestRegisterLength = 10000;
                    int nRegisterDataLength = RequestRegisterLength * 2;
                    ReturnData = new byte[nRegisterDataLength + 9];

                    nDataLent = ReturnData.Length - 6; //Transaction Identifier, Protocol_ID 제외한 Data 길이

                    ReturnData[0] = RecieveData[0];
                    ReturnData[1] = RecieveData[1]; //Transaction Identifier : 식별자 요청과 똑같이 Return 한다.
                    ReturnData[2] = (byte)((Protocol_ID & 0xff00) >> 8);
                    ReturnData[3] = (byte)(Protocol_ID & 0xff); //Protocol_ID는 항상 0;
                    ReturnData[4] = (byte)((nDataLent & 0xff00) >> 8);
                    ReturnData[5] = (byte)(nDataLent & 0xff);
                    ReturnData[6] = m_bSlaveID;
                    ReturnData[7] = FunctionCode; //function Code;
                    ReturnData[8] = (byte)nRegisterDataLength;

                    nDataIndex = 9;
                    for (int nAddress = FirstRegisterAddress; nAddress < FirstRegisterAddress + RequestRegisterLength; nAddress++)
                    {
                        if (nAddress > 9999)
                            break;
                        ushort RegisterData = 0;
                        if (FunctionCode == Read_Holding_Register)
                        {
                            RegisterData = m_pServerData.arReadWriteRegister[nAddress];
                        }
                        if (FunctionCode == Read_Input_Register)
                        {
                            RegisterData = m_pServerData.arReadOnlyRegister[nAddress];
                        }
                        ReturnData[nDataIndex] = (byte)((RegisterData & 0xff00) >> 8);
                        nDataIndex++;
                        ReturnData[nDataIndex] = (byte)(RegisterData & 0xff);
                        nDataIndex++;
                    }
                    break;
                case Write_Single_Coil:
                    ArrayLittleEndian = RecieveData.Skip(8).Take(2).Reverse().ToArray();
                    FirstStatusAddress = BitConverter.ToUInt16(ArrayLittleEndian, 0);
                    if (FirstStatusAddress > 10000)
                        FirstStatusAddress = 10000;

                    if (RecieveData[11] != 0x00)
                    {
                        return false;
                    }

                    if (RecieveData[10] == 0xFF)
                    {
                        m_pServerData.arReadWriteStatus[FirstStatusAddress] = true;
                    }
                    else if (RecieveData[10] == 0x00)
                    {
                        m_pServerData.arReadWriteStatus[FirstStatusAddress] = false;
                    }
                    else
                    {
                        return false;
                    }
                    //그대로 RETURN 한다.
                    ReturnData = new byte[RecieveData.Length];
                    ReturnData = RecieveData;
                    break;

                case Write_Single_Register:
                    ArrayLittleEndian = RecieveData.Skip(8).Take(2).Reverse().ToArray();
                    FirstStatusAddress = BitConverter.ToUInt16(ArrayLittleEndian, 0);
                    if (FirstStatusAddress > 10000)
                        FirstStatusAddress = 10000;

                       ArrayLittleEndian = RecieveData.Skip(10).Take(2).Reverse().ToArray();
                       m_pServerData.arReadWriteRegister[FirstStatusAddress] = BitConverter.ToUInt16(ArrayLittleEndian, 0);

                    //그대로 RETURN 한다.
                    ReturnData = new byte[RecieveData.Length];
                    ReturnData = RecieveData;
                    break;

                case Write_Multi_Coil:
                    ArrayLittleEndian = RecieveData.Skip(8).Take(2).Reverse().ToArray();
                    FirstStatusAddress = BitConverter.ToUInt16(ArrayLittleEndian, 0);
                    if (FirstStatusAddress > 10000)
                        FirstStatusAddress = 10000;
                    ArrayLittleEndian = RecieveData.Skip(10).Take(2).Reverse().ToArray();
                    RequestStatusLength = BitConverter.ToUInt16(ArrayLittleEndian, 0);
                    if (RequestStatusLength > 10000)
                        RequestStatusLength = 10000;
                    nStatusDataLength = ((RequestStatusLength - 1) / 8) + 1;

                    if (RecieveData[12] != nStatusDataLength)
                    {
                        return false;
                    }

                    nBitIndex = 0;
                    nDataIndex = 13;
                    for (int nAddress = FirstStatusAddress; nAddress < FirstStatusAddress + RequestStatusLength; nAddress++)
                    {
                        if (nAddress > 9999)
                            break;
                        byte CoilData = 0x01;

                        byte Value = (byte)(RecieveData[nDataIndex] >> nBitIndex);

                        m_pServerData.arReadWriteStatus[nAddress] = (Value & CoilData) == CoilData;

                        nBitIndex++;
                        if (nBitIndex >= 8)
                        {
                            nBitIndex = 0;
                            nDataIndex++;
                        }
                    }

                    ReturnData = new byte[12]; //데이터 갯수와 데이터 제외한 Message
                    Array.Copy(RecieveData, ReturnData, ReturnData.Length);
                    break;

                case Write_Multi_Register:
                    ArrayLittleEndian = RecieveData.Skip(8).Take(2).Reverse().ToArray();
                    FirstRegisterAddress = BitConverter.ToUInt16(ArrayLittleEndian, 0);
                    if (FirstRegisterAddress > 10000)
                        FirstRegisterAddress = 10000;

                    ArrayLittleEndian = RecieveData.Skip(10).Take(2).Reverse().ToArray();
                    RequestRegisterLength = BitConverter.ToUInt16(ArrayLittleEndian, 0);
                    if (RequestRegisterLength > 10000)
                        RequestRegisterLength = 10000;
                    if (RecieveData[12] != RequestRegisterLength * 2)
                    {
                        return false;
                    }

                    nDataIndex = 13;

                    for (int nAddress = FirstRegisterAddress; nAddress < FirstRegisterAddress + RequestRegisterLength; nAddress++)
                    {
                        ArrayLittleEndian = RecieveData.Skip(nDataIndex).Take(2).Reverse().ToArray();
                        m_pServerData.arReadWriteRegister[nAddress] = BitConverter.ToUInt16(ArrayLittleEndian, 0);
                        nDataIndex += 2;
                    }

                    ReturnData = new byte[12]; //데이터 갯수와 데이터 제외한 Message
                    Array.Copy(RecieveData, ReturnData, ReturnData.Length);
                    break;
            }
            return true;
        }

        public bool ParseRTU(byte[] RecieveData, ref byte[] ReturnData)
        {
            byte[] ArrayLittleEndian;
            if (RecieveData[0] != bSlaveID)
                return false;

            if (!CheckCRC(RecieveData, RecieveData.Length))
                return false;

            byte FunctionCode = RecieveData[1];
            switch (FunctionCode)//funcutionCode
            {
                case Read_Coil_Status:
                case Read_Input_Status:
                    ArrayLittleEndian = RecieveData.Skip(2).Take(2).Reverse().ToArray();
                    int FirstStatusAddress = BitConverter.ToUInt16(ArrayLittleEndian, 0);
                    if (FirstStatusAddress > 10000)
                        FirstStatusAddress = 10000;
                    ArrayLittleEndian = RecieveData.Skip(4).Take(2).Reverse().ToArray();
                    int RequestStatusLength = BitConverter.ToUInt16(ArrayLittleEndian, 0);
                    if (RequestStatusLength > 10000)
                        RequestStatusLength = 10000;
                    int nStatusDataLength = ((RequestStatusLength - 1) / 8) + 1;
                    ReturnData = new byte[nStatusDataLength + 5];


                    ReturnData[0] = m_bSlaveID;
                    ReturnData[1] = FunctionCode; //function Code;
                    ReturnData[2] = (byte)nStatusDataLength;
                    int nBitIndex = 0;
                    int nDataIndex = 3;
                    for (int nAddress = FirstStatusAddress; nAddress < FirstStatusAddress + RequestStatusLength; nAddress++)
                    {
                        if (nAddress > 9999)
                            break;
                        byte CoilData = 0x00;
                        if (FunctionCode == Read_Coil_Status)
                        {
                            if (m_pServerData.arReadWriteStatus[nAddress])
                            {
                                CoilData = 0x01;
                            }
                        }
                        if (FunctionCode == Read_Input_Status)
                        {
                            if (m_pServerData.arReadOnlyStatus[nAddress])
                            {
                                CoilData = 0x01;
                            }
                        }
                        CoilData = (byte)(CoilData << nBitIndex);
                        ReturnData[nDataIndex] = (byte)(ReturnData[nDataIndex] | CoilData);

                        nBitIndex++;
                        if (nBitIndex >= 8)
                        {
                            nBitIndex = 0;
                            nDataIndex++;
                        }
                    }

                    ushort usCRC = ModRTU_CRC(ReturnData, ReturnData.Length - 2);
                    ReturnData[ReturnData.Length - 2] = (byte)((usCRC & 0xff00) >> 8);
                    ReturnData[ReturnData.Length - 1] = (byte)(usCRC & 0xff);
                    break;

                case Read_Holding_Register:
                case Read_Input_Register:
                    ArrayLittleEndian = RecieveData.Skip(2).Take(2).Reverse().ToArray();
                    int FirstRegisterAddress = BitConverter.ToUInt16(ArrayLittleEndian, 0);
                    if (FirstRegisterAddress > 10000)
                        FirstRegisterAddress = 10000;
                    ArrayLittleEndian = RecieveData.Skip(4).Take(2).Reverse().ToArray();
                    int RequestRegisterLength = BitConverter.ToUInt16(ArrayLittleEndian, 0);
                    if (RequestRegisterLength > 10000)
                        RequestRegisterLength = 10000;
                    int nRegisterDataLength = RequestRegisterLength * 2;
                    ReturnData = new byte[nRegisterDataLength + 5];

                    ReturnData[0] = m_bSlaveID;
                    ReturnData[1] = FunctionCode; //function Code;
                    ReturnData[2] = (byte)nRegisterDataLength;

                    nDataIndex = 3;
                    for (int nAddress = FirstRegisterAddress; nAddress < FirstRegisterAddress + RequestRegisterLength; nAddress++)
                    {
                        if (nAddress > 9999)
                            break;
                        ushort RegisterData = 0;
                        if (FunctionCode == Read_Holding_Register)
                        {

                            RegisterData = m_pServerData.arReadWriteRegister[nAddress];
                        }
                        if (FunctionCode == Read_Input_Register)
                        {
                            RegisterData = m_pServerData.arReadOnlyRegister[nAddress];
                        }
                        ReturnData[nDataIndex] = (byte)((RegisterData & 0xff00) >> 8);
                        nDataIndex++;
                        ReturnData[nDataIndex] = (byte)(RegisterData & 0xff);
                        nDataIndex++;
                    }

                    usCRC = ModRTU_CRC(ReturnData, ReturnData.Length - 2);
                    ReturnData[ReturnData.Length - 2] = (byte)((usCRC & 0xff00) >> 8);
                    ReturnData[ReturnData.Length - 1] = (byte)(usCRC & 0xff);
                    break;
                case Write_Single_Coil:
                    ArrayLittleEndian = RecieveData.Skip(2).Take(2).Reverse().ToArray();
                    FirstStatusAddress = BitConverter.ToUInt16(ArrayLittleEndian, 0);
                    if (FirstStatusAddress > 10000)
                        FirstStatusAddress = 10000;

                    if (RecieveData[5] != 0x00)
                    {
                        return false;
                    }

                    if (RecieveData[4] == 0xFF)
                    {
                        m_pServerData.arReadWriteStatus[FirstStatusAddress] = true;
                    }
                    else if (RecieveData[4] == 0x00)
                    {
                        m_pServerData.arReadWriteStatus[FirstStatusAddress] = false;

                    }
                    else
                    {
                        return false;
                    }
                    //그대로 RETURN 한다.
                    ReturnData = new byte[RecieveData.Length];
                    ReturnData = RecieveData.ToArray();
                    break;


                case Write_Single_Register:
                    ArrayLittleEndian = RecieveData.Skip(2).Take(2).Reverse().ToArray();
                    FirstStatusAddress = BitConverter.ToUInt16(ArrayLittleEndian, 0);
                    if (FirstStatusAddress > 10000)
                        FirstStatusAddress = 10000;

                    ArrayLittleEndian = RecieveData.Skip(4).Take(2).Reverse().ToArray();
                    m_pServerData.arReadWriteRegister[FirstStatusAddress] = BitConverter.ToUInt16(ArrayLittleEndian, 0);

                    //그대로 RETURN 한다.
                    ReturnData = new byte[RecieveData.Length];
                    ReturnData = RecieveData.ToArray();
                    break;

                case Write_Multi_Coil:
                     ArrayLittleEndian = RecieveData.Skip(2).Take(2).Reverse().ToArray();
                    FirstStatusAddress = BitConverter.ToUInt16(ArrayLittleEndian, 0);
                    if (FirstStatusAddress > 10000)
                        FirstStatusAddress = 10000;
                    ArrayLittleEndian = RecieveData.Skip(4).Take(2).Reverse().ToArray();
                    RequestStatusLength = BitConverter.ToUInt16(ArrayLittleEndian, 0);
                    if (RequestStatusLength > 10000)
                        RequestStatusLength = 10000;
                    nStatusDataLength = ((RequestStatusLength - 1) / 8) + 1;

                    if (RecieveData[6] != nStatusDataLength)
                    {
                        return false;
                    }

                    nBitIndex = 0;
                    nDataIndex = 7;
                    for (int nAddress = FirstStatusAddress; nAddress < FirstStatusAddress + RequestStatusLength; nAddress++)
                    {
                        if (nAddress > 9999)
                            break;
                        byte CoilData = 0x01;

                        byte Value = (byte)(RecieveData[nDataIndex] >> nBitIndex);

                       m_pServerData.arReadWriteStatus[nAddress] = (Value & CoilData) == CoilData;

                        nBitIndex++;
                        if (nBitIndex >= 8)
                        {
                            nBitIndex = 0;
                            nDataIndex++;
                        }
                    }

                    ReturnData = new byte[8];
                    Array.Copy(RecieveData, ReturnData, 6);

                    usCRC = ModRTU_CRC(ReturnData, ReturnData.Length - 2);
                    ReturnData[ReturnData.Length - 2] = (byte)((usCRC & 0xff00) >> 8);
                    ReturnData[ReturnData.Length - 1] = (byte)(usCRC & 0xff);
                    break;

                case Write_Multi_Register:
                    ArrayLittleEndian = RecieveData.Skip(2).Take(2).Reverse().ToArray();
                    FirstRegisterAddress = BitConverter.ToUInt16(ArrayLittleEndian, 0);
                    if (FirstRegisterAddress > 10000)
                        FirstRegisterAddress = 10000;

                    ArrayLittleEndian = RecieveData.Skip(4).Take(2).Reverse().ToArray();
                    RequestRegisterLength = BitConverter.ToUInt16(ArrayLittleEndian, 0);
                    if (RequestRegisterLength > 10000)
                        RequestRegisterLength = 10000;
                    if(RecieveData[6] != RequestRegisterLength * 2)
                    {
                        return false;
                    }

                    nDataIndex = 7;
                    
                    for (int nAddress = FirstRegisterAddress; nAddress < FirstRegisterAddress + RequestRegisterLength; nAddress++)
                    {
                        ArrayLittleEndian = RecieveData.Skip(nDataIndex).Take(2).Reverse().ToArray();
                        m_pServerData.arReadWriteRegister[nAddress] = BitConverter.ToUInt16(ArrayLittleEndian, 0);
                        nDataIndex += 2;
                    }

                    ReturnData = new byte[8];
                    Array.Copy(RecieveData, ReturnData, 6);
                    
                    usCRC = ModRTU_CRC(ReturnData, ReturnData.Length - 2);
                    ReturnData[ReturnData.Length - 2] = (byte)((usCRC & 0xff00) >> 8);
                    ReturnData[ReturnData.Length - 1] = (byte)(usCRC & 0xff);
                    break;
            }
            return true;
        }

        private ushort ModRTU_CRC(byte[] data, int nLen)
        {
            ushort remainder = 0xffff;

            for (int i = 0; i < nLen; i++)
            {
                remainder ^= (ushort)data[i];
                for (int j = 8; j != 0; j--)
                {
                    if ((remainder & 0x0001) != 0)
                    {
                        remainder >>= 1;
                        remainder ^= 0xA001;
                    }
                    else
                        remainder >>= 1;
                }
            }

            // byte swap
            byte high = (byte)((remainder & 0xff00) >> 8);
            byte low = (byte)(remainder & 0xff);

            remainder = (ushort)(low << 8 | high);

            return remainder;
        }

        private bool CheckCRC(byte[] data, int nLength)
        {
            bool rtn;

            ushort crcCal = ModRTU_CRC(data, nLength - 2);
            ushort crcData = (ushort)((data[nLength - 2] << 8) | data[nLength - 1]);

            if (crcCal == crcData) rtn = true;
            else rtn = false;

            return rtn;
        }

        public void SetCoilStatus(int nAddress, bool bData)
        {
            m_pServerData.arReadWriteStatus[nAddress] = bData;
        }
        public void SetInputStatus(int nAddress, bool bData)
        {
            m_pServerData.arReadWriteStatus[nAddress] = bData;
        }

        public void SetReadWriteRegister(int nAddress, string strData, int nLength = -1)
        {
            byte[] ArrayLittleEndian;
            byte[] StrByte = Encoding.UTF8.GetBytes(strData);

            if (nLength == -1)
                nLength = strData.Length;
            
            for (int nIndex = 0; nIndex < nLength; nIndex += 2)
            {
                if (nAddress > 9999)
                    break;

                ArrayLittleEndian = StrByte.Skip(nIndex).Take(2).Reverse().ToArray();
                m_pServerData.arReadWriteRegister[nAddress] = BitConverter.ToUInt16(ArrayLittleEndian, 0);

                nAddress++;
            }
        }

        public string GetReadWriteRegister(int nAddress, int nLength)
        {
            string strReturn = "";

            byte[] arbyte = new byte[nLength * 2];
            int nDataIndex = 0;
            for (int nIndex = nAddress; nIndex < nAddress + nLength; nIndex ++)
            {
                arbyte[nDataIndex] = (byte)((m_pServerData.arReadWriteRegister[nIndex] & 0xff00) >> 8);
                nDataIndex++;
                arbyte[nDataIndex] = (byte)(m_pServerData.arReadWriteRegister[nIndex] & 0xff);
                nDataIndex++;
            }

            strReturn = Encoding.UTF8.GetString(arbyte).TrimEnd('\0'); 
            return strReturn;

        }
    }
}
