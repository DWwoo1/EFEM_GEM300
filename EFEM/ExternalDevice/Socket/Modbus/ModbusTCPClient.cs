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
    class ModbusTCPClient
    {
        #region 변수
        private int m_nSocketIndex;

        private Config.ConfigSocket m_Socket = Config.ConfigSocket.GetInstance();
        private Dictionary<int, ModbusTCPClientDevice> m_dicOfModbus = new Dictionary<int, ModbusTCPClientDevice>(); //key = Modbus SlaveID

        private int m_nObjectCode;
        private TickCounter_.TickCounter m_TickForTimeOut = new TickCounter_.TickCounter();

        private EN_MODBUS_STATUS m_enStatus = EN_MODBUS_STATUS.SETTING_COMMAND;
        private bool _temporaryConnected = false;
        private bool _isExiting = false;
        #endregion

        #region 싱글톤
        private ModbusTCPClient(int nSocketIndex)
        {
            m_nSocketIndex = nSocketIndex;

            m_nObjectCode = nSocketIndex * 100;
        }
        private static Dictionary<int, ModbusTCPClient> _Instance = null;
        public static ModbusTCPClient GetInstance(int nSocketIndex)
        {
            if (_Instance == null)
            {
                _Instance = new Dictionary<int, ModbusTCPClient>();
            }
            if (!_Instance.ContainsKey(nSocketIndex))
            {
                _Instance.Add(nSocketIndex, new ModbusTCPClient(nSocketIndex));
            }

            return _Instance[nSocketIndex];
        }
        #endregion

        #region 주기 호출 함수
        public void Execute()
        {
            if (_isExiting)
                return;

            foreach (var item in m_dicOfModbus)
            {
                _temporaryConnected = false;
                var state = m_Socket.GetState(m_nSocketIndex);
                switch (state)
                {
                    case Config.ConfigSocket.EN_SOCKET_STATE.CONNECTED:
                        _temporaryConnected = true;
                        break;
                    case Config.ConfigSocket.EN_SOCKET_STATE.DISCONNECTED:
                    case Config.ConfigSocket.EN_SOCKET_STATE.READY:
                    case Config.ConfigSocket.EN_SOCKET_STATE.TIMEOUT_RECEIVE:
                        {
                            m_Socket.Connect(ref m_nSocketIndex);
                        }
                        break;
                }

                if (false == _temporaryConnected)
                    return;

                switch (m_enStatus)
                {
                    case EN_MODBUS_STATUS.SETTING_COMMAND:
                        {
                            if (false == item.Value.TickOver)
                                break;

                            byte[] dataToRequest = null;
                            if (item.Value.GetCommand(ref dataToRequest))
                            {
                                if (m_Socket.send(m_nSocketIndex, dataToRequest))
                                {
                                    m_enStatus = EN_MODBUS_STATUS.WAITING_READ;
                                    m_TickForTimeOut.SetTickCount(5000);
                                }
                            }
                        }
                        break;
                    case EN_MODBUS_STATUS.WAITING_READ:
                        byte[] ReciveData = null;
                        if (m_Socket.Receive(m_nSocketIndex, ref ReciveData))
                        {
                            //데이터 쪼개저셔 들어오는 경우 버퍼 추가 필요
                            if (item.Value.ParsingData(ReciveData))
                            {
                                m_enStatus = EN_MODBUS_STATUS.SETTING_COMMAND;
                            }
                        }
                        if (m_TickForTimeOut.IsTickOver(false))
                        {
                            m_enStatus = EN_MODBUS_STATUS.SETTING_COMMAND;
                        }
                        break;
                }
            }
        }
        #endregion

        #region 외부 인터페이스
        #region 초기화 및 종료
        public bool Init()
        {
            //if (m_Socket.Connect(ref m_nSocketIndex) == false)
            //    return false;
            m_Socket.Connect(ref m_nSocketIndex);

            return true;
        }

        public void Close()
        {
            _isExiting = true;
            m_Socket.DisConnect(ref m_nSocketIndex);
        }

        public void AddDevice(ModbusTCPClientDevice enDevice)
        {
            if (m_dicOfModbus.ContainsKey(enDevice.bSlaveID))
                return;

            m_dicOfModbus.Add(enDevice.bSlaveID, enDevice);
        }

        #region AddMonitorItem
        public void AddMornitorCoilStatus(int nSlaveIndex, int nAddress, int nLength)
        {
            m_dicOfModbus[nSlaveIndex].AddMornitorCoilStatus(nAddress, nLength);
        }
        public void AddMornitorInputStatus(int nSlaveIndex, int nAddress, int nLength)
        {
            m_dicOfModbus[nSlaveIndex].AddMornitorInputStatus(nAddress, nLength);
        }
        public void AddMornitorHoldingRegister(int nSlaveIndex, int nAddress, int nLength)
        {
            m_dicOfModbus[nSlaveIndex].AddMornitorHoldingRegister(nAddress, nLength);
        }
        public void AddMornitorInputRegister(int nSlaveIndex, int nAddress, int nLength)
        {
            m_dicOfModbus[nSlaveIndex].AddMornitorInputRegister(nAddress, nLength);
        }
        #endregion
        #endregion

        #region 아이템 값 반환

        #region Status
        #region Coil
        public bool GetCoilStatus(int nSlaveIndex, int nAddress)
        {
            return m_dicOfModbus[nSlaveIndex].GetCoilStatus(nAddress);
        }
        public uint GetOutputStates(int slaveIndex, int chennel)
        {
            return m_dicOfModbus[slaveIndex].GetOutputStates(chennel);
        }
        #endregion /Coil

        #region Input
        public bool GetInputStatus(int nSlaveIndex, int nAddress)
        {
            return m_dicOfModbus[nSlaveIndex].GetInputStatus(nAddress);
        }

        public uint GetInputStates(int slaveIndex, int chennel)
        {
            return m_dicOfModbus[slaveIndex].GetInputStates(chennel);
        }
        #endregion /Input
        #endregion /Status

        #region Register
        #region Holding

        public ushort GetushortHoldingRegister(int nSlaveIndex, int nAddress)
        {
            return m_dicOfModbus[nSlaveIndex].GetushortHoldingRegister(nAddress);
        }

        public short GetshortHoldingRegister(int nSlaveIndex, int nAddress)
        {
            return m_dicOfModbus[nSlaveIndex].GetshortHoldingRegister(nAddress);
        }
        public uint GetuintHoldingRegister(int nSlaveIndex, int nAddress)
        {
            return m_dicOfModbus[nSlaveIndex].GetuintHoldingRegister(nAddress);
        }
        public int GetintHoldingRegister(int nSlaveIndex, int nAddress)
        {
            return m_dicOfModbus[nSlaveIndex].GetintHoldingRegister(nAddress);
        }
        public ulong GetulongHoldingRegister(int nSlaveIndex, int nAddress)
        {
            return m_dicOfModbus[nSlaveIndex].GetulongHoldingRegister(nAddress);
        }
        public long GetlongHoldingRegister(int nSlaveIndex, int nAddress)
        {
            return m_dicOfModbus[nSlaveIndex].GetlongHoldingRegister(nAddress);
        }
        public float GetfloatHoldingRegister(int nSlaveIndex, int nAddress)
        {
            return m_dicOfModbus[nSlaveIndex].GetfloatHoldingRegister(nAddress);
        }
        public double GetdoubleHoldingRegister(int nSlaveIndex, int nAddress)
        {
            return m_dicOfModbus[nSlaveIndex].GetdoubleHoldingRegister(nAddress);
        }
        public string GetstringValueHoldingRegister(int nSlaveIndex, int nAddress, int nLength)
        {
            return m_dicOfModbus[nSlaveIndex].GetstringHoldingRegister(nAddress, nLength);
        }
        #endregion /Holding

        #region Input

        public ushort GetushortInputRegister(int nSlaveIndex, int nAddress)
        {
            return m_dicOfModbus[nSlaveIndex].GetushortInputRegister(nAddress);
        }

        public short GetshortInputRegister(int nSlaveIndex, int nAddress)
        {
            return m_dicOfModbus[nSlaveIndex].GetshortInputRegister(nAddress);
        }
        public uint GetuintInputRegister(int nSlaveIndex, int nAddress)
        {
            return m_dicOfModbus[nSlaveIndex].GetuintInputRegister(nAddress);
        }
        public int GetintInputRegister(int nSlaveIndex, int nAddress)
        {
            return m_dicOfModbus[nSlaveIndex].GetintInputRegister(nAddress);
        }
        public ulong GetulongInputRegister(int nSlaveIndex, int nAddress)
        {
            return m_dicOfModbus[nSlaveIndex].GetulongInputRegister(nAddress);
        }
        public long GetlongInputRegister(int nSlaveIndex, int nAddress)
        {
            return m_dicOfModbus[nSlaveIndex].GetlongInputRegister(nAddress);
        }
        public float GetfloatInputRegister(int nSlaveIndex, int nAddress)
        {
            return m_dicOfModbus[nSlaveIndex].GetfloatInputRegister(nAddress);
        }
        public double GetdoubleInputRegister(int nSlaveIndex, int nAddress)
        {
            return m_dicOfModbus[nSlaveIndex].GetdoubleInputRegister(nAddress);
        }
        public string GetstringValueInputRegister(int nSlaveIndex, int nAddress, int nLength)
        {
            return m_dicOfModbus[nSlaveIndex].GetstringInputRegister(nAddress, nLength);
        }
        #endregion /Input
        #endregion /Register
        #endregion

        #endregion
        #region 아이템 값 설정
        public void SetCoilStatus(int nSlaveIndex, int nAddress, bool bData)
        {
            m_dicOfModbus[nSlaveIndex].SetCoilStatus(nAddress, bData);
        }

        public void SetHoldingRegister(int nSlaveIndex, int nAddress, ushort nData)
        {
            m_dicOfModbus[nSlaveIndex].SetHoldingRegister(nAddress, nData);
        }
        public void SetHoldingRegister(int nSlaveIndex, int nAddress, short nData)
        {
            m_dicOfModbus[nSlaveIndex].SetHoldingRegister(nAddress, nData);
        }
        public void SetHoldingRegister(int nSlaveIndex, int nAddress, uint nData)
        {
            m_dicOfModbus[nSlaveIndex].SetHoldingRegister(nAddress, nData);
        }
        public void SetHoldingRegister(int nSlaveIndex, int nAddress, int nData)
        {
            m_dicOfModbus[nSlaveIndex].SetHoldingRegister(nAddress, nData);
        }
        public void SetHoldingRegister(int nSlaveIndex, int nAddress, ulong nData)
        {
            m_dicOfModbus[nSlaveIndex].SetHoldingRegister(nAddress, nData);
        }
        public void SetHoldingRegister(int nSlaveIndex, int nAddress, long nData)
        {
            m_dicOfModbus[nSlaveIndex].SetHoldingRegister(nAddress, nData);
        }
        public void SetHoldingRegister(int nSlaveIndex, int nAddress, float dData)
        {
            m_dicOfModbus[nSlaveIndex].SetHoldingRegister(nAddress, dData);
        }
        public void SetHoldingRegister(int nSlaveIndex, int nAddress, double dData)
        {
            m_dicOfModbus[nSlaveIndex].SetHoldingRegister(nAddress, dData);
        }
        public void SetHoldingRegister(int nSlaveIndex, int nAddress, string strData, int nLength = -1)
        {
            m_dicOfModbus[nSlaveIndex].SetHoldingRegister(nAddress, strData, nLength);
        }
        #endregion

        #region 내부 인터페이스
        #endregion
    }

    public class ModbusTCPClientDevice
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

        private const uint IntervalToHandling = 100;
        private readonly TickCounter_.TickCounter HandlingTicks = new TickCounter_.TickCounter();
        #endregion

        #region 변수
        private byte m_bSlaveID;

        protected byte m_nFunctionCode;
        protected ushort m_nRequestAddress;
        protected ushort m_nRequestCount;
        protected ushort m_nTransaction_ID = 0;

        protected ModbusData m_pMonitoringData = new ModbusData();
        protected ModbusData m_pBufferDataForWrite = new ModbusData();

        private EN_MODBUS_SERVER_PROTOCOL m_enProtocol = EN_MODBUS_SERVER_PROTOCOL.TCP;

        ConcurrentQueue<CommandItem> m_queMonitoringItem = new ConcurrentQueue<CommandItem>();
        ConcurrentQueue<CommandItem> m_queWriteItem = new ConcurrentQueue<CommandItem>();
        CommandItem m_pCurrentCommandItem;
        #endregion

        #region <Properties>
        public bool TickOver
        {
            get
            {
                if (HandlingTicks.IsTickOver(true))
                {
                    HandlingTicks.SetTickCount(IntervalToHandling);
                    return true;
                }
                
                return false;
            }
        }
        #endregion </Properties>

        public ModbusTCPClientDevice(byte SlaveID, EN_MODBUS_SERVER_PROTOCOL enProtocol)
        {
            m_bSlaveID = SlaveID;
            m_enProtocol = enProtocol;
        }

        #region 속성
        public byte bSlaveID { get { return m_bSlaveID; } }

        public EN_MODBUS_SERVER_PROTOCOL enProtocol { get { return m_enProtocol; } }
        #endregion

        public bool ParsingData(byte[] RecieveData)
        {
            byte[] ArrayForEndian;
            //remove header
            byte[] arBody = null;
            switch (enProtocol)
            {
                case EN_MODBUS_SERVER_PROTOCOL.TCP:
                    ArrayForEndian = RecieveData.Skip(0).Take(2).Reverse().ToArray();
                    ushort transactionID = BitConverter.ToUInt16(ArrayForEndian, 0);

                    if (m_nTransaction_ID != transactionID)
                        return false;
                    ArrayForEndian = RecieveData.Skip(4).Take(2).Reverse().ToArray();
                    ushort BodyLength = BitConverter.ToUInt16(ArrayForEndian, 0);

                    if (RecieveData.Length - 6 != BodyLength)
                        return false;

                    arBody = new byte[RecieveData.Length - 6];
                    Array.Copy(RecieveData, 6, arBody, 0, RecieveData.Length - 6);
                    break;
                case EN_MODBUS_SERVER_PROTOCOL.RTU_OVER_TCP:
                    if (CheckCRC(RecieveData, RecieveData.Length - 2))
                    {
                        arBody = new byte[RecieveData.Length - 2];
                        Array.Copy(RecieveData, arBody, RecieveData.Length - 2);
                    }
                    break;
            }
            if (arBody == null)
                return false;
            if (arBody[0] != bSlaveID)
                return false;

            byte FunctionCode = arBody[1];
            if (m_pCurrentCommandItem.FunctionCode != FunctionCode)
                return false;

            switch (FunctionCode)
            {
                case Read_Input_Status:
                    {
                        int nStatusDataLength = ((m_pCurrentCommandItem.Length - 1) / 8) + 1;
                        if (arBody.Length - 3 != nStatusDataLength)
                        {
                            return false;
                        }

                        int nDataIndex = 3;
                        Array.Copy(arBody, nDataIndex, m_pMonitoringData.ReadOnlyStates, 0, arBody.Length - nDataIndex);
                    }
                    break;
                case Read_Coil_Status:
                    {
                        int nStatusDataLength = ((m_pCurrentCommandItem.Length - 1) / 8) + 1;
                        if (arBody.Length - 3 != nStatusDataLength)
                        {
                            return false;
                        }

                        int nDataIndex = 3;
                        Array.Copy(arBody, nDataIndex, m_pMonitoringData.ReadOnlyCoils, 0, arBody.Length - nDataIndex);

                        //int nStatusDataLength = ((m_pCurrentCommandItem.Length - 1) / 8) + 1;
                        //if (arBody.Length - 3 != nStatusDataLength)
                        //{
                        //    return false;
                        //}

                        //int nBitIndex = 0;
                        //int nDataIndex = 3;

                        //for (int nAddress = m_pCurrentCommandItem.Address; nAddress < m_pCurrentCommandItem.Address + m_pCurrentCommandItem.Length; nAddress++)
                        //{
                        //    byte CoilData = 0x01;

                        //    byte Value = (byte)(arBody[nDataIndex] >> nBitIndex);

                        //    if (FunctionCode == Read_Coil_Status)
                        //        m_pMonitoringData.arReadWriteStatus[nAddress] = (Value & CoilData) == CoilData;
                        //    else
                        //        m_pMonitoringData.arReadOnlyStatus[nAddress] = (Value & CoilData) == CoilData;

                        //    nBitIndex++;
                        //    if (nBitIndex >= 8)
                        //    {
                        //        nBitIndex = 0;
                        //        nDataIndex++;
                        //    }
                        //}
                    }
                    break;

                case Read_Holding_Register:
                case Read_Input_Register:
                    {
                        int nRegisterDataLength = m_pCurrentCommandItem.Length * 2;
                        if (arBody.Length - 3 != nRegisterDataLength)
                        {
                            return false;
                        }

                        int nDataIndex = 3;

                        for (int nAddress = m_pCurrentCommandItem.Address; nAddress < m_pCurrentCommandItem.Address + m_pCurrentCommandItem.Length; nAddress++)
                        {
                            ArrayForEndian = arBody.Skip(nDataIndex).Take(2).Reverse().ToArray();

                            if (FunctionCode == Read_Holding_Register)
                                m_pMonitoringData.arReadWriteRegister[nAddress] = BitConverter.ToUInt16(ArrayForEndian, 0);
                            else
                                m_pMonitoringData.arReadOnlyRegister[nAddress] = BitConverter.ToUInt16(ArrayForEndian, 0);

                            nDataIndex += 2;
                        }
                    }
                    break;

                case Write_Single_Coil:
                    ArrayForEndian = arBody.Skip(2).Take(2).Reverse().ToArray();
                    ushort FirstStatusAddress = BitConverter.ToUInt16(ArrayForEndian, 0);

                    if (arBody[5] != 0x00)
                    {
                        return false;
                    }

                    if (arBody[4] == 0xFF)
                    {
                        m_pMonitoringData.arReadWriteStatus[FirstStatusAddress] = true;
                    }
                    else if (arBody[4] == 0x00)
                    {
                        m_pMonitoringData.arReadWriteStatus[FirstStatusAddress] = false;

                    }
                    else
                    {
                        return false;
                    }
                    break;


                case Write_Single_Register:
                    ArrayForEndian = arBody.Skip(2).Take(2).Reverse().ToArray();
                    ushort FirstRegisterAddress = BitConverter.ToUInt16(ArrayForEndian, 0);

                    ArrayForEndian = arBody.Skip(4).Take(2).Reverse().ToArray();
                    m_pMonitoringData.arReadWriteRegister[FirstRegisterAddress] = BitConverter.ToUInt16(ArrayForEndian, 0);

                    break;

                case Write_Multi_Coil:
                    {
                        ArrayForEndian = arBody.Skip(2).Take(2).Reverse().ToArray();
                        FirstStatusAddress = BitConverter.ToUInt16(ArrayForEndian, 0);

                        ArrayForEndian = arBody.Skip(4).Take(2).Reverse().ToArray();
                        ushort RequestStatusLength = BitConverter.ToUInt16(ArrayForEndian, 0);

                        int nStatusDataLength = ((RequestStatusLength - 1) / 8) + 1;

                        //multi Wirte는 Data Return은 없음으로 WriteBuffer에서 Set하자.
                        for (int nAddress = FirstStatusAddress; nAddress < FirstStatusAddress + RequestStatusLength; nAddress++)
                        {
                            m_pMonitoringData.arReadWriteStatus[nAddress] = m_pBufferDataForWrite.arReadWriteStatus[nAddress];
                        }
                    }
                    break;

                case Write_Multi_Register:
                    ArrayForEndian = arBody.Skip(2).Take(2).Reverse().ToArray();
                    FirstRegisterAddress = BitConverter.ToUInt16(ArrayForEndian, 0);

                    ArrayForEndian = arBody.Skip(4).Take(2).Reverse().ToArray();
                    ushort RequestRegisterLength = BitConverter.ToUInt16(ArrayForEndian, 0);

                    //multi Wirte는 Data Return은 없음으로 WriteBuffer에서 Set하자.
                    for (int nAddress = FirstRegisterAddress; nAddress < FirstRegisterAddress + RequestRegisterLength; nAddress++)
                    {
                        m_pMonitoringData.arReadWriteRegister[nAddress] = m_pBufferDataForWrite.arReadWriteRegister[nAddress];
                    }
                    break;

                default:
                    return false;
            }

            return true;
        }

        public bool GetCommand(ref byte[] arCommand)
        {
            if (!m_queWriteItem.TryDequeue(out m_pCurrentCommandItem))
            {
                if (m_queMonitoringItem.TryDequeue(out m_pCurrentCommandItem))
                {
                    m_queMonitoringItem.Enqueue(m_pCurrentCommandItem);
                }
                else
                {
                    return false;
                }
            }
            byte[] arCommandWithOutHeader = new byte[6];
            switch (m_pCurrentCommandItem.FunctionCode)
            {
                case Read_Coil_Status:
                case Read_Input_Status:
                case Read_Holding_Register:
                case Read_Input_Register:
                    arCommandWithOutHeader = new byte[6];
                    arCommandWithOutHeader[0] = m_bSlaveID;
                    arCommandWithOutHeader[1] = m_pCurrentCommandItem.FunctionCode;
                    arCommandWithOutHeader[2] = (byte)((m_pCurrentCommandItem.Address & 0xff00) >> 8);
                    arCommandWithOutHeader[3] = (byte)(m_pCurrentCommandItem.Address & 0xff);
                    arCommandWithOutHeader[4] = (byte)((m_pCurrentCommandItem.Length & 0xff00) >> 8);
                    arCommandWithOutHeader[5] = (byte)(m_pCurrentCommandItem.Length & 0xff);
                    break;

                case Write_Single_Coil:
                    arCommandWithOutHeader = new byte[6];
                    arCommandWithOutHeader[0] = m_bSlaveID;
                    arCommandWithOutHeader[1] = m_pCurrentCommandItem.FunctionCode;
                    arCommandWithOutHeader[2] = (byte)((m_pCurrentCommandItem.Address & 0xff00) >> 8);
                    arCommandWithOutHeader[3] = (byte)(m_pCurrentCommandItem.Address & 0xff);
                    if (m_pBufferDataForWrite.arReadWriteStatus[m_pCurrentCommandItem.Address])
                        arCommandWithOutHeader[4] = 0xff;
                    else
                        arCommandWithOutHeader[4] = 0x00;
                    arCommandWithOutHeader[5] = 0x00;
                    break;

                case Write_Single_Register:
                    arCommandWithOutHeader = new byte[6];
                    arCommandWithOutHeader[0] = m_bSlaveID;
                    arCommandWithOutHeader[1] = m_pCurrentCommandItem.FunctionCode;
                    arCommandWithOutHeader[2] = (byte)((m_pCurrentCommandItem.Address & 0xff00) >> 8);
                    arCommandWithOutHeader[3] = (byte)(m_pCurrentCommandItem.Address & 0xff);
                    arCommandWithOutHeader[4] = (byte)((m_pBufferDataForWrite.arReadWriteRegister[m_pCurrentCommandItem.Address] & 0xff00) >> 8);
                    arCommandWithOutHeader[5] = (byte)(m_pBufferDataForWrite.arReadWriteRegister[m_pCurrentCommandItem.Address] & 0xff);
                    break;

                case Write_Multi_Coil:
                    int nCoilDataLength = ((m_pCurrentCommandItem.Length - 1) / 8) + 1;
                    arCommandWithOutHeader = new byte[7 + nCoilDataLength];
                    arCommandWithOutHeader[0] = m_bSlaveID;
                    arCommandWithOutHeader[1] = m_pCurrentCommandItem.FunctionCode;
                    arCommandWithOutHeader[2] = (byte)((m_pCurrentCommandItem.Address & 0xff00) >> 8);
                    arCommandWithOutHeader[3] = (byte)(m_pCurrentCommandItem.Address & 0xff);
                    arCommandWithOutHeader[4] = (byte)((m_pCurrentCommandItem.Length & 0xff00) >> 8);
                    arCommandWithOutHeader[5] = (byte)(m_pCurrentCommandItem.Length & 0xff);
                    arCommandWithOutHeader[6] = (byte)nCoilDataLength;
                    int nBitIndex = 0;
                    int nDataIndex = 7;
                    for (int nAddress = m_pCurrentCommandItem.Address; nAddress < m_pCurrentCommandItem.Address + m_pCurrentCommandItem.Length; nAddress++)
                    {
                        if (nAddress > 9999)
                            break;
                        byte CoilData = 0x00;

                        if (m_pBufferDataForWrite.arReadWriteStatus[nAddress])
                            CoilData = 0x01;

                        byte Value = (byte)(CoilData >> nBitIndex);

                        arCommandWithOutHeader[nDataIndex] = (byte)(Value | arCommandWithOutHeader[nDataIndex]);

                        nBitIndex++;
                        if (nBitIndex >= 8)
                        {
                            nBitIndex = 0;
                            nDataIndex++;
                        }
                    }
                    break;
                case Write_Multi_Register:
                    int nRegisterDataLength = m_pCurrentCommandItem.Length * 2;

                    arCommandWithOutHeader = new byte[7 + nRegisterDataLength];
                    arCommandWithOutHeader[0] = m_bSlaveID;
                    arCommandWithOutHeader[1] = m_pCurrentCommandItem.FunctionCode;
                    arCommandWithOutHeader[2] = (byte)((m_pCurrentCommandItem.Address & 0xff00) >> 8);
                    arCommandWithOutHeader[3] = (byte)(m_pCurrentCommandItem.Address & 0xff);
                    arCommandWithOutHeader[4] = (byte)((m_pCurrentCommandItem.Length & 0xff00) >> 8);
                    arCommandWithOutHeader[5] = (byte)(m_pCurrentCommandItem.Length & 0xff);
                    arCommandWithOutHeader[6] = (byte)nRegisterDataLength;
                    nDataIndex = 7;
                    for (int nAddress = m_pCurrentCommandItem.Address; nAddress < m_pCurrentCommandItem.Address + m_pCurrentCommandItem.Length; nAddress++)
                    {
                        if (nAddress > 9999)
                            break;

                        arCommandWithOutHeader[nDataIndex] = (byte)((m_pBufferDataForWrite.arReadWriteRegister[nAddress] & 0xff00) >> 8);
                        nDataIndex++;
                        arCommandWithOutHeader[nDataIndex] = (byte)(m_pBufferDataForWrite.arReadWriteRegister[nAddress] & 0xff);
                        nDataIndex++;
                    }
                    break;
            }


            switch (m_enProtocol)
            {
                case EN_MODBUS_SERVER_PROTOCOL.TCP: //원래 MBAP Header는 Unit ID(Slave ID)까지 포함이나 RTU에도 공통됨으로 편의상 제외
                    m_nTransaction_ID++;
                    arCommand = new byte[arCommandWithOutHeader.Length + 6];
                    Array.Copy(arCommandWithOutHeader, 0, arCommand, 6, arCommandWithOutHeader.Length);
                    arCommand[0] = (byte)((m_nTransaction_ID & 0xff00) >> 8);
                    arCommand[1] = (byte)(m_nTransaction_ID & 0xff);
                    arCommand[2] = (byte)((Protocol_ID & 0xff00) >> 8);
                    arCommand[3] = (byte)(Protocol_ID & 0xff);
                    arCommand[4] = (byte)((arCommandWithOutHeader.Length & 0xff00) >> 8);
                    arCommand[5] = (byte)(arCommandWithOutHeader.Length & 0xff); //Data +  Unit ID(Slave ID) 길이
                    break;
                case EN_MODBUS_SERVER_PROTOCOL.RTU_OVER_TCP: //crc 추가
                    arCommand = new byte[arCommandWithOutHeader.Length + 2];
                    Array.Copy(arCommandWithOutHeader, arCommand, arCommandWithOutHeader.Length);

                    ushort usCRC = ModRTU_CRC(arCommand, arCommand.Length - 2);
                    arCommand[arCommand.Length - 2] = (byte)((usCRC & 0xff00) >> 8);
                    arCommand[arCommand.Length - 1] = (byte)(usCRC & 0xff);
                    break;
            }

            return true;
        }

        #region crc
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
        #endregion /crc

        #region Write
        public void SetCoilStatus(int nAddress, bool bData)
        {
            m_pBufferDataForWrite.arReadWriteStatus[nAddress] = bData;

            CommandItem cmdItem = new CommandItem();
            cmdItem.FunctionCode = Write_Single_Coil;
            cmdItem.Address = (ushort)nAddress;
            cmdItem.Length = 1;
            m_queWriteItem.Enqueue(cmdItem);
        }

        public void SetHoldingRegister(int nAddress, ushort nData)
        {
            m_pBufferDataForWrite.arReadWriteRegister[nAddress] = nData;

            CommandItem cmdItem = new CommandItem();

            cmdItem.FunctionCode = Write_Single_Register;
            cmdItem.Address = nAddress;
            cmdItem.Length = 1;
            m_queWriteItem.Enqueue(cmdItem);
        }

        public void SetHoldingRegister(int nAddress, short nData)
        {
            m_pBufferDataForWrite.SetHoldingRegister(nAddress, nData);

            CommandItem cmdItem = new CommandItem();

            cmdItem.FunctionCode = Write_Single_Register;
            cmdItem.Address = nAddress;
            cmdItem.Length = 1;
            m_queWriteItem.Enqueue(cmdItem);
        }



        public void SetHoldingRegister(int nAddress, uint nData)
        {
            m_pBufferDataForWrite.SetHoldingRegister(nAddress, nData);

            CommandItem cmdItem = new CommandItem();

            cmdItem.FunctionCode = Write_Multi_Register;
            cmdItem.Address = nAddress;
            cmdItem.Length = 2;
            m_queWriteItem.Enqueue(cmdItem);
        }

        public void SetHoldingRegister(int nAddress, int nData)
        {
            m_pBufferDataForWrite.SetHoldingRegister(nAddress, nData);

            CommandItem cmdItem = new CommandItem();

            cmdItem.FunctionCode = Write_Multi_Register;
            cmdItem.Address = nAddress;
            cmdItem.Length = 2;
            m_queWriteItem.Enqueue(cmdItem);
        }

        public void SetHoldingRegister(int nAddress, ulong nData)
        {
            m_pBufferDataForWrite.SetHoldingRegister(nAddress, nData);

            CommandItem cmdItem = new CommandItem();

            cmdItem.FunctionCode = Write_Multi_Register;
            cmdItem.Address = nAddress;
            cmdItem.Length = 4;
            m_queWriteItem.Enqueue(cmdItem);
        }

        public void SetHoldingRegister(int nAddress, long nData)
        {
            m_pBufferDataForWrite.SetHoldingRegister(nAddress, nData);

            CommandItem cmdItem = new CommandItem();

            cmdItem.FunctionCode = Write_Multi_Register;
            cmdItem.Address = nAddress;
            cmdItem.Length = 4;
            m_queWriteItem.Enqueue(cmdItem);
        }

        public void SetHoldingRegister(int nAddress, float dData)
        {
            m_pBufferDataForWrite.SetHoldingRegister(nAddress, dData);

            CommandItem cmdItem = new CommandItem();

            cmdItem.FunctionCode = Write_Multi_Register;
            cmdItem.Address = nAddress;
            cmdItem.Length = 2;
            m_queWriteItem.Enqueue(cmdItem);
        }

        public void SetHoldingRegister(int nAddress, double dData)
        {
            m_pBufferDataForWrite.SetHoldingRegister(nAddress, dData);

            CommandItem cmdItem = new CommandItem();

            cmdItem.FunctionCode = Write_Multi_Register;
            cmdItem.Address = nAddress;
            cmdItem.Length = 4;
            m_queWriteItem.Enqueue(cmdItem);
        }

        public void SetHoldingRegister(int nAddress, string strData, int nRegisterLength = -1)
        {
            m_pBufferDataForWrite.SetHoldingRegister(nAddress, strData, nRegisterLength);

            CommandItem cmdItem = new CommandItem();

            cmdItem.FunctionCode = Write_Multi_Register;
            cmdItem.Address = (ushort)nAddress;
            int nLength = strData.Length;
            if (nRegisterLength == -1)
            {
                nRegisterLength = nLength / 2;

                if (nLength % 2 == 1)
                {
                    nRegisterLength += 1;
                }
            }
            cmdItem.Length = (ushort)nRegisterLength;
            m_queWriteItem.Enqueue(cmdItem);
        }

        #endregion /Write

        #region Monitor
        public void AddMornitorCoilStatus(int Address, int Length)
        {
            CommandItem MonitoringCommand = new CommandItem();
            MonitoringCommand.FunctionCode = Read_Coil_Status;
            MonitoringCommand.Address = Address;
            MonitoringCommand.Length = Length;

            m_queMonitoringItem.Enqueue(MonitoringCommand);
        }
        public void AddMornitorInputStatus(int Address, int Length)
        {
            CommandItem MonitoringCommand = new CommandItem();
            MonitoringCommand.FunctionCode = Read_Input_Status;
            MonitoringCommand.Address = Address;
            MonitoringCommand.Length = Length;

            m_queMonitoringItem.Enqueue(MonitoringCommand);
        }
        public void AddMornitorHoldingRegister(int Address, int Length)
        {
            CommandItem MonitoringCommand = new CommandItem();
            MonitoringCommand.FunctionCode = Read_Holding_Register;
            MonitoringCommand.Address = Address;
            MonitoringCommand.Length = Length;

            m_queMonitoringItem.Enqueue(MonitoringCommand);
        }
        public void AddMornitorInputRegister(int Address, int Length)
        {
            CommandItem MonitoringCommand = new CommandItem();
            MonitoringCommand.FunctionCode = Read_Input_Register;
            MonitoringCommand.Address = Address;
            MonitoringCommand.Length = Length;

            m_queMonitoringItem.Enqueue(MonitoringCommand);
        }
        #endregion /Monitor

        #region Read
        public bool GetInputStatus(int Address)
        {
            return m_pMonitoringData.arReadOnlyStatus[Address];
        }

        public uint GetOutputStates(int channel)
        {
            return m_pMonitoringData.ReadOnlyCoils[channel];
        }


        public uint GetInputStates(int channel)
        {
            //System.Collections.BitArray bits = new System.Collections.BitArray(m_pMonitoringData.arReadOnlyStatus);
            //int additionalValue = 0;
            //if (m_pMonitoringData.arReadOnlyStatus.Length % 8 > 0)
            //    additionalValue = 1;

            //int length = m_pMonitoringData.arReadOnlyStatus.Length / 8 + additionalValue;
            //if (length <= 0)
            //    return 0;

            //byte[] returnValue = new byte[length];
            //bits.CopyTo(returnValue, 0);
            return m_pMonitoringData.ReadOnlyStates[channel];
            //return returnValue[channel];

        }

        public bool GetCoilStatus(int Address)
        {
            return m_pMonitoringData.arReadWriteStatus[Address];
        }

        #region InputRegister
        public ushort GetushortInputRegister(int Address)
        {
            return m_pMonitoringData.arReadOnlyRegister[Address];
        }
        public short GetshortInputRegister(int Address)
        {
            return m_pMonitoringData.GetshortInputRegister(Address);
        }
        public uint GetuintInputRegister(int Address)
        {
            return m_pMonitoringData.GetuintInputRegister(Address);
        }
        public int GetintInputRegister(int Address)
        {
            return m_pMonitoringData.GetintInputRegister(Address);
        }
        public ulong GetulongInputRegister(int Address)
        {
            return m_pMonitoringData.GetulongInputRegister(Address);
        }
        public long GetlongInputRegister(int Address)
        {
            return m_pMonitoringData.GetlongInputRegister(Address);
        }
        public float GetfloatInputRegister(int Address)
        {
            return m_pMonitoringData.GetfloatInputRegister(Address);
        }
        public double GetdoubleInputRegister(int Address)
        {
            return m_pMonitoringData.GetdoubleInputRegister(Address);
        }
        public string GetstringInputRegister(int Address, int nRegisterLength)
        {
            return m_pMonitoringData.GetstringInputRegister(Address, nRegisterLength);
        }
        #endregion /InputRegister

        #region HoldingRegister
        public ushort GetushortHoldingRegister(int Address)
        {
            return m_pMonitoringData.arReadWriteRegister[Address];
        }
        public short GetshortHoldingRegister(int Address)
        {
            return m_pMonitoringData.GetshortHoldingRegister(Address);
        }
        public uint GetuintHoldingRegister(int Address)
        {
            return m_pMonitoringData.GetuintHoldingRegister(Address);
        }
        public int GetintHoldingRegister(int Address)
        {
            return m_pMonitoringData.GetintHoldingRegister(Address);
        }
        public ulong GetulongHoldingRegister(int Address)
        {
            return m_pMonitoringData.GetulongHoldingRegister(Address);
        }
        public long GetlongHoldingRegister(int Address)
        {
            return m_pMonitoringData.GetlongHoldingRegister(Address);
        }
        public float GetfloatHoldingRegister(int Address)
        {
            return m_pMonitoringData.GetfloatHoldingRegister(Address);
        }
        public double GetdoubleHoldingRegister(int Address)
        {
            return m_pMonitoringData.GetdoubleHoldingRegister(Address);
        }
        public string GetstringHoldingRegister(int Address, int nRegisterLength)
        {
            return m_pMonitoringData.GetstringHoldingRegister(Address, nRegisterLength);
        }
        #endregion /HoldingRegister
        #endregion /Read

        struct CommandItem
        {
            public byte FunctionCode;
            public int Address;
            public int Length;
        }
    }
}
