using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

namespace FrameOfSystem3.ExternalDevice.Modbus
{
    public class ModbusData
    {
        private bool[] m_arReadWriteStatus;     //Coil Status : 0~9999 FunctionCode(1, 5, 15)에 대응
        private bool[] m_arReadOnlyStatus;      //Input Status : 10000~19999 FunctionCode(2)에 대응

        private byte[] _readOnlyStates;         //Input Status : 10000~19999 FunctionCode(2)에 대응 (Byte 기준)
        private byte[] _readOnlyCoils;          //Coil Status : 0~9999 FunctionCode(1, 5, 15)에 대응 (Byte 기준)

        private ushort[] m_arReadWriteRegister; //Holding Register : 40000~49999  FunctionCode(3, 6, 16)에 대응
        private ushort[] m_arReadOnlyRegister;  //Input Register : 30000~39999  FunctionCode(4)에 대응

        private System.Threading.ReaderWriterLockSlim m_rwlockForReadWriteStatus = new System.Threading.ReaderWriterLockSlim();
        private System.Threading.ReaderWriterLockSlim m_rwlockForReadOnlyStatus = new System.Threading.ReaderWriterLockSlim();
        private System.Threading.ReaderWriterLockSlim m_rwlockForReadWriteRegister = new System.Threading.ReaderWriterLockSlim();
        private System.Threading.ReaderWriterLockSlim m_rwlockForReadOnlyRegister = new System.Threading.ReaderWriterLockSlim();

        private System.Threading.ReaderWriterLockSlim _rwlockForReadOnlyStates = new System.Threading.ReaderWriterLockSlim();
        private System.Threading.ReaderWriterLockSlim _rwlockForReadOnlyCoils = new System.Threading.ReaderWriterLockSlim();
        #region 생성자
        public ModbusData()
        {
            m_arReadWriteStatus = new bool[10000];
            m_arReadOnlyStatus = new bool[10000];
            m_arReadOnlyRegister = new ushort[10000];
            m_arReadWriteRegister = new ushort[10000];

            _readOnlyStates = new byte[1250];   // 10000 / 8
            _readOnlyCoils = new byte[1250];
        }

        #endregion

        public bool[] arReadWriteStatus
        {
            get
            {
                m_rwlockForReadWriteStatus.EnterReadLock();
                bool[] arReturn = m_arReadWriteStatus;
                m_rwlockForReadWriteStatus.ExitReadLock();
                return arReturn;
            }

            set
            {
                m_rwlockForReadWriteStatus.EnterWriteLock();
                m_arReadWriteStatus = value;
                m_rwlockForReadWriteStatus.ExitWriteLock();
            }
        }

        public bool[] arReadOnlyStatus
        {
            get
            {
                m_rwlockForReadOnlyStatus.EnterReadLock();
                bool[] arReturn = m_arReadOnlyStatus;
                m_rwlockForReadOnlyStatus.ExitReadLock();
                return arReturn;
            }

            set
            {
                m_rwlockForReadOnlyStatus.EnterWriteLock();
                m_arReadOnlyStatus = value;
                m_rwlockForReadOnlyStatus.ExitWriteLock();
            }
        }

        public byte[] ReadOnlyStates
        {
            get
            {
                _rwlockForReadOnlyStates.EnterReadLock();
                byte[] returnArray = _readOnlyStates;
                _rwlockForReadOnlyStates.ExitReadLock();
                return returnArray;
            }
            set
            {
                _rwlockForReadOnlyStates.EnterWriteLock();
                _readOnlyStates = value;
                _rwlockForReadOnlyStates.ExitWriteLock();
            }
        }

        public byte[] ReadOnlyCoils
        {
            get
            {
                _rwlockForReadOnlyCoils.EnterReadLock();
                byte[] returnArray = _readOnlyCoils;
                _rwlockForReadOnlyCoils.ExitReadLock();
                return returnArray;
            }
            set
            {
                _rwlockForReadOnlyCoils.EnterWriteLock();
                _readOnlyCoils = value;
                _rwlockForReadOnlyCoils.ExitWriteLock();
            }
        }
        public ushort[] arReadWriteRegister
        {
            get
            {
                m_rwlockForReadWriteRegister.EnterReadLock();
                ushort[] arReturn = m_arReadWriteRegister;
                m_rwlockForReadWriteRegister.ExitReadLock();
                return arReturn;
            }

            set
            {
                m_rwlockForReadWriteRegister.EnterWriteLock();
                m_arReadWriteRegister = value;
                m_rwlockForReadWriteRegister.ExitWriteLock();
            }
        }

        public ushort[] arReadOnlyRegister
        {
            get
            {
                m_rwlockForReadOnlyRegister.EnterReadLock();
                ushort[] arReturn = m_arReadOnlyRegister;
                m_rwlockForReadOnlyRegister.ExitReadLock();
                return arReturn;
            }

            set
            {
                m_rwlockForReadOnlyRegister.EnterWriteLock();
                m_arReadOnlyRegister = value;
                m_rwlockForReadOnlyRegister.ExitWriteLock();
            }
        }

        #region InputRegister
        #region Get
        public short GetshortInputRegister(int Address)
        {
            byte[] arbyte = new byte[2];
            int nDataIndex = 0;
            m_rwlockForReadOnlyRegister.EnterReadLock();
            for (int nIndex = Address; nIndex < Address + 1; nIndex++)
            {
                arbyte[nDataIndex] = (byte)((m_arReadOnlyRegister[nIndex] & 0xff00) >> 8);
                nDataIndex++;
                arbyte[nDataIndex] = (byte)(m_arReadOnlyRegister[nIndex] & 0xff);
                nDataIndex++;
            }
            m_rwlockForReadOnlyRegister.ExitReadLock();

            return BitConverter.ToInt16(arbyte, 0);
        }

        public uint GetuintInputRegister(int Address)
        {
            byte[] arbyte = new byte[4];
            int nDataIndex = 0;
            m_rwlockForReadOnlyRegister.EnterReadLock();
            for (int nIndex = Address; nIndex < Address + 2; nIndex++)
            {
                arbyte[nDataIndex] = (byte)((m_arReadOnlyRegister[nIndex] & 0xff00) >> 8);
                nDataIndex++;
                arbyte[nDataIndex] = (byte)(m_arReadOnlyRegister[nIndex] & 0xff);
                nDataIndex++;
            }
            m_rwlockForReadOnlyRegister.ExitReadLock();

            return BitConverter.ToUInt32(arbyte, 0);
        }

        public int GetintInputRegister(int Address)
        {
            byte[] arbyte = new byte[4];
            int nDataIndex = 0;
            m_rwlockForReadOnlyRegister.EnterReadLock();
            for (int nIndex = Address; nIndex < Address + 2; nIndex++)
            {
                arbyte[nDataIndex] = (byte)((m_arReadOnlyRegister[nIndex] & 0xff00) >> 8);
                nDataIndex++;
                arbyte[nDataIndex] = (byte)(m_arReadOnlyRegister[nIndex] & 0xff);
                nDataIndex++;
            }
            m_rwlockForReadOnlyRegister.ExitReadLock();

            return BitConverter.ToInt32(arbyte, 0);
        }

        public ulong GetulongInputRegister(int Address)
        {
            byte[] arbyte = new byte[8];
            int nDataIndex = 0;
            m_rwlockForReadOnlyRegister.EnterReadLock();
            for (int nIndex = Address; nIndex < Address + 4; nIndex++)
            {
                arbyte[nDataIndex] = (byte)((m_arReadOnlyRegister[nIndex] & 0xff00) >> 8);
                nDataIndex++;
                arbyte[nDataIndex] = (byte)(m_arReadOnlyRegister[nIndex] & 0xff);
                nDataIndex++;
            }
            m_rwlockForReadOnlyRegister.ExitReadLock();

            return BitConverter.ToUInt64(arbyte, 0);
        }

        public long GetlongInputRegister(int Address)
        {
            byte[] arbyte = new byte[8];
            int nDataIndex = 0;
            m_rwlockForReadOnlyRegister.EnterReadLock();
            for (int nIndex = Address; nIndex < Address + 4; nIndex++)
            {
                arbyte[nDataIndex] = (byte)((m_arReadOnlyRegister[nIndex] & 0xff00) >> 8);
                nDataIndex++;
                arbyte[nDataIndex] = (byte)(m_arReadOnlyRegister[nIndex] & 0xff);
                nDataIndex++;
            }
            m_rwlockForReadOnlyRegister.ExitReadLock();

            return BitConverter.ToInt64(arbyte, 0);
        }

        public float GetfloatInputRegister(int Address)
        {
            byte[] arbyte = new byte[4];
            int nDataIndex = 0;
            m_rwlockForReadOnlyRegister.EnterReadLock();
            for (int nIndex = Address; nIndex < Address + 2; nIndex++)
            {
                arbyte[nDataIndex] = (byte)((m_arReadOnlyRegister[nIndex] & 0xff00) >> 8);
                nDataIndex++;
                arbyte[nDataIndex] = (byte)(m_arReadOnlyRegister[nIndex] & 0xff);
                nDataIndex++;
            }
            m_rwlockForReadOnlyRegister.ExitReadLock();

            return BitConverter.ToSingle(arbyte, 0);
        }

        public double GetdoubleInputRegister(int Address)
        {
            byte[] arbyte = new byte[8];
            int nDataIndex = 0;
            m_rwlockForReadOnlyRegister.EnterReadLock();
            for (int nIndex = Address; nIndex < Address + 4; nIndex++)
            {
                arbyte[nDataIndex] = (byte)((m_arReadOnlyRegister[nIndex] & 0xff00) >> 8);
                nDataIndex++;
                arbyte[nDataIndex] = (byte)(m_arReadOnlyRegister[nIndex] & 0xff);
                nDataIndex++;
            }
            m_rwlockForReadOnlyRegister.ExitReadLock();

            return BitConverter.ToDouble(arbyte, 0);
        }

        public string GetstringInputRegister(int nAddress, int nRegisterLength)
        {
            string strReturn = "";

            byte[] arbyte = new byte[nRegisterLength * 2];
            int nDataIndex = 0;
            m_rwlockForReadOnlyRegister.EnterReadLock();
            for (int nIndex = nAddress; nIndex < nAddress + nRegisterLength; nIndex++)
            {
                arbyte[nDataIndex] = (byte)((m_arReadOnlyRegister[nIndex] & 0xff00) >> 8);
                nDataIndex++;
                arbyte[nDataIndex] = (byte)(m_arReadOnlyRegister[nIndex] & 0xff);
                nDataIndex++;
            }
            m_rwlockForReadOnlyRegister.ExitReadLock();

            strReturn = Encoding.ASCII.GetString(arbyte);
            return strReturn;

        }
        #endregion /Get

        #region Set
        public void SetInputRegister(int Address, short Value)
        {
            byte[] arbyte = BitConverter.GetBytes(Value);
            int RegisterLength = arbyte.Length / 2;
            int nDataIndex = 0;
            m_rwlockForReadOnlyRegister.EnterWriteLock();
            for (int nIndex = Address; nIndex < Address + RegisterLength; nIndex++)
            {
                byte[] arRegister = arbyte.Skip(nDataIndex).Take(2).Reverse().ToArray();
                m_arReadOnlyRegister[nIndex] = BitConverter.ToUInt16(arRegister, 0);
                nDataIndex += 2;
            }
            m_rwlockForReadOnlyRegister.ExitWriteLock();
        }
        public void SetInputRegister(int Address, uint Value)
        {
            byte[] arbyte = BitConverter.GetBytes(Value);
            int RegisterLength = arbyte.Length / 2;
            int nDataIndex = 0;
            m_rwlockForReadOnlyRegister.EnterWriteLock();
            for (int nIndex = Address; nIndex < Address + RegisterLength; nIndex++)
            {
                byte[] arRegister = arbyte.Skip(nDataIndex).Take(2).Reverse().ToArray();
                m_arReadOnlyRegister[nIndex] = BitConverter.ToUInt16(arRegister, 0);
                nDataIndex += 2;
            }
            m_rwlockForReadOnlyRegister.ExitWriteLock();
        }
        public void SetInputRegister(int Address, int Value)
        {
            byte[] arbyte = BitConverter.GetBytes(Value);
            int RegisterLength = arbyte.Length / 2;
            int nDataIndex = 0;
            m_rwlockForReadOnlyRegister.EnterWriteLock();
            for (int nIndex = Address; nIndex < Address + RegisterLength; nIndex++)
            {
                byte[] arRegister = arbyte.Skip(nDataIndex).Take(2).Reverse().ToArray();
                m_arReadOnlyRegister[nIndex] = BitConverter.ToUInt16(arRegister, 0);
                nDataIndex += 2;
            }
            m_rwlockForReadOnlyRegister.ExitWriteLock();
        }
        public void SetInputRegister(int Address, ulong Value)
        {
            byte[] arbyte = BitConverter.GetBytes(Value);
            int RegisterLength = arbyte.Length / 2;
            int nDataIndex = 0;
            m_rwlockForReadOnlyRegister.EnterWriteLock();
            for (int nIndex = Address; nIndex < Address + RegisterLength; nIndex++)
            {
                byte[] arRegister = arbyte.Skip(nDataIndex).Take(2).Reverse().ToArray();
                m_arReadOnlyRegister[nIndex] = BitConverter.ToUInt16(arRegister, 0);
                nDataIndex += 2;
            }
            m_rwlockForReadOnlyRegister.ExitWriteLock();
        }
        public void SetInputRegister(int Address, long Value)
        {
            byte[] arbyte = BitConverter.GetBytes(Value);
            int RegisterLength = arbyte.Length / 2;
            int nDataIndex = 0;
            m_rwlockForReadOnlyRegister.EnterWriteLock();
            for (int nIndex = Address; nIndex < Address + RegisterLength; nIndex++)
            {
                byte[] arRegister = arbyte.Skip(nDataIndex).Take(2).Reverse().ToArray();
                m_arReadOnlyRegister[nIndex] = BitConverter.ToUInt16(arRegister, 0);
                nDataIndex += 2;
            }
            m_rwlockForReadOnlyRegister.ExitWriteLock();
        }
        public void SetInputRegister(int Address, float Value)
        {
            byte[] arbyte = BitConverter.GetBytes(Value);
            int RegisterLength = arbyte.Length / 2;
            int nDataIndex = 0;
            m_rwlockForReadOnlyRegister.EnterWriteLock();
            for (int nIndex = Address; nIndex < Address + RegisterLength; nIndex++)
            {
                byte[] arRegister = arbyte.Skip(nDataIndex).Take(2).Reverse().ToArray();
                m_arReadOnlyRegister[nIndex] = BitConverter.ToUInt16(arRegister, 0);
                nDataIndex += 2;
            }
            m_rwlockForReadOnlyRegister.ExitWriteLock();
        }
        public void SetInputRegister(int Address, double Value)
        {
            byte[] arbyte = BitConverter.GetBytes(Value);
            int RegisterLength = arbyte.Length / 2;
            int nDataIndex = 0;
            m_rwlockForReadOnlyRegister.EnterWriteLock();
            for (int nIndex = Address; nIndex < Address + RegisterLength; nIndex++)
            {
                byte[] arRegister = arbyte.Skip(nDataIndex).Take(2).Reverse().ToArray();
                m_arReadOnlyRegister[nIndex] = BitConverter.ToUInt16(arRegister, 0);
                nDataIndex += 2;
            }
            m_rwlockForReadOnlyRegister.ExitWriteLock();
        }
        public void SetInputRegister(int Address, string Value, int RegisterLength = -1)
        {
            byte[] arbyte = Encoding.ASCII.GetBytes(Value);

            int nLength = Value.Length;

            if (RegisterLength == -1)
            {
                RegisterLength = nLength / 2;

                if (nLength % 2 == 1)
                {
                    RegisterLength += 1;
                    byte[] arbuff = new byte[arbyte.Length + 1];
                    Array.Copy(arbyte, arbuff, arbyte.Length);
                    arbyte = arbuff;
                }
            }


            int nDataIndex = 0;
            m_rwlockForReadOnlyRegister.EnterWriteLock();
            for (int nIndex = Address; nIndex < Address + RegisterLength; nIndex++)
            {
                byte[] arRegister = arbyte.Skip(nDataIndex).Take(2).Reverse().ToArray();
                m_arReadOnlyRegister[nIndex] = BitConverter.ToUInt16(arRegister, 0);
                nDataIndex += 2;
            }
            m_rwlockForReadOnlyRegister.ExitWriteLock();
        }
        #endregion /Set
        #endregion /InputRegister

        #region HoldingRegister
        #region Get
        public short GetshortHoldingRegister(int Address)
        {
            byte[] arbyte = new byte[2];
            int nDataIndex = 0;
            m_rwlockForReadWriteRegister.EnterReadLock();
            for (int nIndex = Address; nIndex < Address + 1; nIndex++)
            {
                arbyte[nDataIndex] = (byte)((m_arReadWriteRegister[nIndex] & 0xff00) >> 8);
                nDataIndex++;
                arbyte[nDataIndex] = (byte)(m_arReadWriteRegister[nIndex] & 0xff);
                nDataIndex++;
            }
            m_rwlockForReadWriteRegister.ExitReadLock();

            return BitConverter.ToInt16(arbyte, 0);
        }

        public uint GetuintHoldingRegister(int Address)
        {
            byte[] arbyte = new byte[4];
            int nDataIndex = 0;
            m_rwlockForReadWriteRegister.EnterReadLock();
            for (int nIndex = Address; nIndex < Address + 2; nIndex++)
            {
                arbyte[nDataIndex] = (byte)((m_arReadWriteRegister[nIndex] & 0xff00) >> 8);
                nDataIndex++;
                arbyte[nDataIndex] = (byte)(m_arReadWriteRegister[nIndex] & 0xff);
                nDataIndex++;
            }
            m_rwlockForReadWriteRegister.ExitReadLock();

            return BitConverter.ToUInt32(arbyte, 0);
        }

        public int GetintHoldingRegister(int Address)
        {
            byte[] arbyte = new byte[4];
            int nDataIndex = 0;
            m_rwlockForReadWriteRegister.EnterReadLock();
            for (int nIndex = Address; nIndex < Address + 2; nIndex++)
            {
                arbyte[nDataIndex] = (byte)((m_arReadWriteRegister[nIndex] & 0xff00) >> 8);
                nDataIndex++;
                arbyte[nDataIndex] = (byte)(m_arReadWriteRegister[nIndex] & 0xff);
                nDataIndex++;
            }
            m_rwlockForReadWriteRegister.ExitReadLock();

            return BitConverter.ToInt32(arbyte, 0);
        }

        public ulong GetulongHoldingRegister(int Address)
        {
            byte[] arbyte = new byte[8];
            int nDataIndex = 0;
            m_rwlockForReadWriteRegister.EnterReadLock();
            for (int nIndex = Address; nIndex < Address + 4; nIndex++)
            {
                arbyte[nDataIndex] = (byte)((m_arReadWriteRegister[nIndex] & 0xff00) >> 8);
                nDataIndex++;
                arbyte[nDataIndex] = (byte)(m_arReadWriteRegister[nIndex] & 0xff);
                nDataIndex++;
            }
            m_rwlockForReadWriteRegister.ExitReadLock();

            return BitConverter.ToUInt64(arbyte, 0);
        }

        public long GetlongHoldingRegister(int Address)
        {
            byte[] arbyte = new byte[8];
            int nDataIndex = 0;
            m_rwlockForReadWriteRegister.EnterReadLock();
            for (int nIndex = Address; nIndex < Address + 4; nIndex++)
            {
                arbyte[nDataIndex] = (byte)((m_arReadWriteRegister[nIndex] & 0xff00) >> 8);
                nDataIndex++;
                arbyte[nDataIndex] = (byte)(m_arReadWriteRegister[nIndex] & 0xff);
                nDataIndex++;
            }
            m_rwlockForReadWriteRegister.ExitReadLock();

            return BitConverter.ToInt64(arbyte, 0);
        }

        public float GetfloatHoldingRegister(int Address)
        {
            byte[] arbyte = new byte[4];
            int nDataIndex = 0;
            m_rwlockForReadWriteRegister.EnterReadLock();
            for (int nIndex = Address; nIndex < Address + 2; nIndex++)
            {
                arbyte[nDataIndex] = (byte)((m_arReadWriteRegister[nIndex] & 0xff00) >> 8);
                nDataIndex++;
                arbyte[nDataIndex] = (byte)(m_arReadWriteRegister[nIndex] & 0xff);
                nDataIndex++;
            }
            m_rwlockForReadWriteRegister.ExitReadLock();

            return BitConverter.ToSingle(arbyte, 0);
        }

        public double GetdoubleHoldingRegister(int Address)
        {
            byte[] arbyte = new byte[8];
            int nDataIndex = 0;
            m_rwlockForReadWriteRegister.EnterReadLock();
            for (int nIndex = Address; nIndex < Address + 4; nIndex++)
            {
                arbyte[nDataIndex] = (byte)((m_arReadWriteRegister[nIndex] & 0xff00) >> 8);
                nDataIndex++;
                arbyte[nDataIndex] = (byte)(m_arReadWriteRegister[nIndex] & 0xff);
                nDataIndex++;
            }
            m_rwlockForReadWriteRegister.ExitReadLock();

            return BitConverter.ToDouble(arbyte, 0);
        }

        public string GetstringHoldingRegister(int nAddress, int nLength)
        {
            string strReturn = "";

            byte[] arbyte = new byte[nLength * 2];
            int nDataIndex = 0;
            m_rwlockForReadWriteRegister.EnterReadLock();
            for (int nIndex = nAddress; nIndex < nAddress + nLength; nIndex++)
            {
                arbyte[nDataIndex] = (byte)((m_arReadWriteRegister[nIndex] & 0xff00) >> 8);
                nDataIndex++;
                arbyte[nDataIndex] = (byte)(m_arReadWriteRegister[nIndex] & 0xff);
                nDataIndex++;
            }
            m_rwlockForReadWriteRegister.ExitReadLock();

            strReturn = Encoding.ASCII.GetString(arbyte).Trim('\0');
            return strReturn;

        }
        #endregion /Get

        #region Set
        public void SetHoldingRegister(int Address, short Value)
        {
            byte[] arbyte = BitConverter.GetBytes(Value);
            int RegisterLength = arbyte.Length / 2;
            int nDataIndex = 0;
            m_rwlockForReadWriteRegister.EnterWriteLock();
            for (int nIndex = Address; nIndex < Address + RegisterLength; nIndex++)
            {
                byte[] arRegister = arbyte.Skip(nDataIndex).Take(2).Reverse().ToArray();
                m_arReadWriteRegister[nIndex] = BitConverter.ToUInt16(arRegister, 0);
                nDataIndex += 2;
            }
            m_rwlockForReadWriteRegister.ExitWriteLock();
        }
        public void SetHoldingRegister(int Address, uint Value)
        {
            byte[] arbyte = BitConverter.GetBytes(Value);
            int RegisterLength = arbyte.Length / 2;
            int nDataIndex = 0;
            m_rwlockForReadWriteRegister.EnterWriteLock();
            for (int nIndex = Address; nIndex < Address + RegisterLength; nIndex++)
            {
                byte[] arRegister = arbyte.Skip(nDataIndex).Take(2).Reverse().ToArray();
                m_arReadWriteRegister[nIndex] = BitConverter.ToUInt16(arRegister, 0);
                nDataIndex += 2;
            }
            m_rwlockForReadWriteRegister.ExitWriteLock();
        }
        public void SetHoldingRegister(int Address, int Value)
        {
            byte[] arbyte = BitConverter.GetBytes(Value);
            int RegisterLength = arbyte.Length / 2;
            int nDataIndex = 0;
            m_rwlockForReadWriteRegister.EnterWriteLock();
            for (int nIndex = Address; nIndex < Address + RegisterLength; nIndex++)
            {
                byte[] arRegister = arbyte.Skip(nDataIndex).Take(2).Reverse().ToArray();
                m_arReadWriteRegister[nIndex] = BitConverter.ToUInt16(arRegister, 0);
                nDataIndex += 2;
            }
            m_rwlockForReadWriteRegister.ExitWriteLock();
        }
        public void SetHoldingRegister(int Address, ulong Value)
        {
            byte[] arbyte = BitConverter.GetBytes(Value);
            int RegisterLength = arbyte.Length / 2;
            int nDataIndex = 0;
            m_rwlockForReadWriteRegister.EnterWriteLock();
            for (int nIndex = Address; nIndex < Address + RegisterLength; nIndex++)
            {
                byte[] arRegister = arbyte.Skip(nDataIndex).Take(2).Reverse().ToArray();
                m_arReadWriteRegister[nIndex] = BitConverter.ToUInt16(arRegister, 0);
                nDataIndex += 2;
            }
            m_rwlockForReadWriteRegister.ExitWriteLock();
        }
        public void SetHoldingRegister(int Address, long Value)
        {
            byte[] arbyte = BitConverter.GetBytes(Value);
            int RegisterLength = arbyte.Length / 2;
            int nDataIndex = 0;
            m_rwlockForReadWriteRegister.EnterWriteLock();
            for (int nIndex = Address; nIndex < Address + RegisterLength; nIndex++)
            {
                byte[] arRegister = arbyte.Skip(nDataIndex).Take(2).Reverse().ToArray();
                m_arReadWriteRegister[nIndex] = BitConverter.ToUInt16(arRegister, 0);
                nDataIndex += 2;
            }
            m_rwlockForReadWriteRegister.ExitWriteLock();
        }
        public void SetHoldingRegister(int Address, float Value)
        {
            byte[] arbyte = BitConverter.GetBytes(Value);
            int RegisterLength = arbyte.Length / 2;
            int nDataIndex = 0;
            m_rwlockForReadWriteRegister.EnterWriteLock();
            for (int nIndex = Address; nIndex < Address + RegisterLength; nIndex++)
            {
                byte[] arRegister = arbyte.Skip(nDataIndex).Take(2).Reverse().ToArray();
                m_arReadWriteRegister[nIndex] = BitConverter.ToUInt16(arRegister, 0);
                nDataIndex += 2;
            }
            m_rwlockForReadWriteRegister.ExitWriteLock();
        }
        public void SetHoldingRegister(int Address, double Value)
        {
            byte[] arbyte = BitConverter.GetBytes(Value);
            int RegisterLength = arbyte.Length / 2;
            int nDataIndex = 0;
            m_rwlockForReadWriteRegister.EnterWriteLock();
            for (int nIndex = Address; nIndex < Address + RegisterLength; nIndex++)
            {
                byte[] arRegister = arbyte.Skip(nDataIndex).Take(2).Reverse().ToArray();
                m_arReadWriteRegister[nIndex] = BitConverter.ToUInt16(arRegister, 0);
                nDataIndex += 2;
            }
            m_rwlockForReadWriteRegister.ExitWriteLock();
        }
        public void SetHoldingRegister(int Address, string Value, int RegisterLength = -1)
        {
            byte[] arbyte = Encoding.ASCII.GetBytes(Value);

              int  nLength = Value.Length;

              if (RegisterLength == -1)
              {
                  RegisterLength = nLength / 2;

                  if(nLength % 2 == 1)
                  {
                      RegisterLength += 1;
                      byte[] arbuff = new byte[arbyte.Length + 1];
                      Array.Copy(arbyte, arbuff, arbyte.Length);
                      arbyte = arbuff;
                  }
              }


            int nDataIndex = 0;
            m_rwlockForReadWriteRegister.EnterWriteLock();
            for (int nIndex = Address; nIndex < Address + RegisterLength; nIndex++)
            {
                byte[] arRegister = arbyte.Skip(nDataIndex).Take(2).Reverse().ToArray();
                m_arReadWriteRegister[nIndex] = BitConverter.ToUInt16(arRegister, 0);
                nDataIndex += 2;
            }
            m_rwlockForReadWriteRegister.ExitWriteLock();
        }
        #endregion /Set
        #endregion /HoldingRegister
    }
}
