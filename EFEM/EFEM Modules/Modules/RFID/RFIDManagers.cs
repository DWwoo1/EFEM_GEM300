
using System.Collections.Concurrent;
using System.Collections.Generic;

using TickCounter_;

using RFIDOnly;
using EFEM.Defines.RFID;
using EFEM.Defines.Common;
using EFEM.Defines.Communicator;
using EFEM.Defines.LoadPort;
using Define.DefineEnumBase.Common;

namespace EFEM.Defines.RFID
{
    public enum RfidCommand
    {
        IDLE,
        READ_LOT_ID,
        READ_CARRIER_ID,
        WRITE_LOT_ID,
        WRITE_CARRIER_ID,
    }

}

namespace EFEM.Modules
{
    public class RFIDManager
    {
        #region <Constructors>
        private RFIDManager() 
        {
            Readers = new Dictionary<int, Dictionary<LoadPortLoadingMode, RFIDReader>>();
        }
        #endregion </Constructors>

        #region <Fields>
        private static RFIDManager _instance = null;
        private readonly Dictionary<int, Dictionary<LoadPortLoadingMode, RFIDReader>> Readers = null;
        #endregion </Fields>

        #region <Properties>
        public static RFIDManager Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new RFIDManager();
                }

                return _instance;
            }
        }
        #endregion </Properties>

        #region <Methods>

        #region <Assign>
        public void AssigReader(int index, LoadPortLoadingMode loadingMode, RFIDReader reader)
        {
            Dictionary<LoadPortLoadingMode, RFIDReader> readers;
            if (Readers.ContainsKey(index))
            {
                readers = Readers[index];
            }
            else
            {
                readers = new Dictionary<LoadPortLoadingMode, RFIDReader>();
            }
            
            readers[loadingMode] = reader;
            Readers[index] = readers;
        }
        #endregion </Assign>

        #region <Config>
        public void SetLotIdAddress(int index, LoadPortLoadingMode loadingMode, int lotIdAddress, int length)
        {
            if (false == Readers.ContainsKey(index))
                return;

            if (false == Readers[index].ContainsKey(loadingMode))
                return;

            Readers[index][loadingMode].SetLotIdAddress(lotIdAddress, length);
        }
        public void SetCarrierIdAddress(int index, LoadPortLoadingMode loadingMode, int carrierIdAddress, int length)
        {
            if (false == Readers.ContainsKey(index))
                return;

            if (false == Readers[index].ContainsKey(loadingMode))
                return;

            Readers[index][loadingMode].SetCarrierIdAddress(carrierIdAddress, length);
        }

        public int GetLotIdAddress(int index, LoadPortLoadingMode loadingMode)
        {
            if (false == Readers.ContainsKey(index))
                return -1;

            if (false == Readers[index].ContainsKey(loadingMode))
                return -1;

            return Readers[index][loadingMode].LotIdAddress;
        }

        public int GetLotIdLength(int index, LoadPortLoadingMode loadingMode)
        {
            if (false == Readers.ContainsKey(index))
                return -1;

            if (false == Readers[index].ContainsKey(loadingMode))
                return -1;

            return Readers[index][loadingMode].LotIdLength;
        }

        public int GetCarrierIdAddress(int index, LoadPortLoadingMode loadingMode)
        {
            if (false == Readers.ContainsKey(index))
                return -1;

            if (false == Readers[index].ContainsKey(loadingMode))
                return -1;

            return Readers[index][loadingMode].CarrierIdAddress;
        }

        public int GetCarrierIdLength(int index, LoadPortLoadingMode loadingMode)
        {
            if (false == Readers.ContainsKey(index))
                return -1;

            if (false == Readers[index].ContainsKey(loadingMode))
                return -1;

            return Readers[index][loadingMode].CarrierIdLength;
        }
        #endregion </Config>

        #region <Action>
        public bool IsConnected(int index, LoadPortLoadingMode mode = LoadPortLoadingMode.Unknown)
        {
            if (Readers == null || false == Readers.ContainsKey(index))
                return false;

            switch (mode)
            {
                case LoadPortLoadingMode.Cassette:
                case LoadPortLoadingMode.Foup:
                    {
                        if (false == Readers[index].ContainsKey(mode))
                            return false;

                        return Readers[index][mode].IsConnected();
                    }

                default:
                    {
                        bool connected = true;
                        foreach (var item in Readers[index])
                        {
                            connected &= item.Value.IsConnected();
                        }

                        return connected;
                    }
                    
            }
        }
        public void InitAction(int index, LoadPortLoadingMode loadingMode)
        {
            if (false == Readers.ContainsKey(index))
                return;

            if (false == Readers[index].ContainsKey(loadingMode))
                return;

            Readers[index][loadingMode].InitAction();
        }
        public CommandResults ReadLotId(int index, LoadPortLoadingMode loadingMode, ref string lotId)
        {
            if (false == Readers.ContainsKey(index))
                return new CommandResults("Read Lot Id", CommandResult.Error, "Does not have rfid");

            if (false == Readers[index].ContainsKey(loadingMode))
                return new CommandResults("Read Lot Id", CommandResult.Error, "Does not have rfid");

            return Readers[index][loadingMode].ReadLotId(ref lotId);
        }
        public CommandResults ReadCarrierId(int index, LoadPortLoadingMode loadingMode, ref string CarrierId)
        {
            if (false == Readers.ContainsKey(index))
                return new CommandResults("Read Lot Id", CommandResult.Error, "Does not have rfid");

            if (false == Readers[index].ContainsKey(loadingMode))
                return new CommandResults("Read Lot Id", CommandResult.Error, "Does not have rfid");

            return Readers[index][loadingMode].ReadCarrierId(ref CarrierId);
        }

        public CommandResults WriteLotId(int index, LoadPortLoadingMode loadingMode, string lotId)
        {
            if (false == Readers.ContainsKey(index))
                return new CommandResults("Read Lot Id", CommandResult.Error, "Does not have rfid");

            if (false == Readers[index].ContainsKey(loadingMode))
                return new CommandResults("Read Lot Id", CommandResult.Error, "Does not have rfid");

            return Readers[index][loadingMode].WriteLotId(lotId);
        }
        public CommandResults WriteCarrierId(int index, LoadPortLoadingMode loadingMode, string CarrierId)
        {
            if (false == Readers.ContainsKey(index))
                return new CommandResults("Read Lot Id", CommandResult.Error, "Does not have rfid");

            if (false == Readers[index].ContainsKey(loadingMode))
                return new CommandResults("Read Lot Id", CommandResult.Error, "Does not have rfid");

            return Readers[index][loadingMode].WriteCarrierId(CarrierId);
        }
        #endregion </Action>

        #region <Excute>
        public void ExecuteAll()
        {
            foreach (var item in Readers)
            {
                if (item.Value == null)
                    continue;

                foreach (var kvp in item.Value)
                {
                    if (kvp.Value == null)
                        continue;

                    kvp.Value.Monitoring();
                }
            }
        }
        #endregion </Excute>

        #endregion </Methods>
    }
}

namespace RFIDOnly
{
    public abstract class RFIDReader
    {
        #region <Constructors>
        public RFIDReader(int portId, EN_CONNECTION_TYPE interfaceType, int commIndex)
        {
            _comm = new Communicator(interfaceType, commIndex);
            PortId = portId;
            TicksForConnection.SetTickCount(RetryInterval);
        }
        #endregion </Constructors>

        #region <Fields>
        protected int _lotIdAddress;
        protected int _lotIdLength;
        protected int _carrierIdAddress;
        protected int _carrierIdLength;

        protected int _actionStep = 0;
        protected RfidCommand _doingAction = RfidCommand.IDLE;

        protected CommandResults _result = new CommandResults(RfidCommand.IDLE.ToString(), CommandResult.Error);
        protected Communicator _comm = null;
        protected TickCounter _timeChecker = new TickCounter();

        private readonly TickCounter TicksForConnection = new TickCounter();
        private readonly uint RetryInterval = 5000;
        private const int RetryCountLimit = 3;
        private int _retryCount;
        #endregion </Fields>

        #region <Properties>
        public int PortId { get; private set; }
        public int LotIdAddress { get { return _lotIdAddress; } }
        public int LotIdLength { get { return _lotIdLength; } }
        public int CarrierIdAddress { get { return _carrierIdAddress; } }
        public int CarrierIdLength { get { return _carrierIdLength; } }
        #endregion </Properties>

        #region <Methods>

        #region <Interfaces>
        public virtual void InitAction()
        {
            _actionStep = 0;
            _doingAction = RfidCommand.IDLE;
        }
        public void SetLotIdAddress(int address, int length)
        {
            _lotIdAddress = address;
            _lotIdLength = length;
        }
        public void SetCarrierIdAddress(int address, int length)
        {
            _carrierIdAddress = address;
            _carrierIdLength = length;
        }

        public CommandResults ReadLotId(ref string lotId)
        {
            var result = DoReadLotId(ref lotId);
            if (result.CommandResult.Equals(CommandResult.Completed))
            {
                lotId = lotId.Replace("\0", string.Empty);
                lotId = lotId.Replace(" ", "");
            }

            return result;
        }
        public CommandResults ReadCarrierId(ref string carrierId)
        {
            var result = DoReadCarrierId(ref carrierId);
            if (result.CommandResult.Equals(CommandResult.Completed))
            {
                carrierId = carrierId.Replace("\0", string.Empty);
                carrierId = carrierId.Replace(" ", "");
            }

            return result;
        }
        public CommandResults WriteLotId(string lotId)
        {
            if (lotId.Length < _lotIdLength)
            {
                lotId = lotId.PadRight(_lotIdLength);
            }
            return DoWriteLotId(lotId);
        }
        public CommandResults WriteCarrierId(string carrierId)
        {
            return DoWriteCarrierId(carrierId);
        }
        #endregion </Interfaces>

        #region <Actions>
        protected abstract CommandResults DoReadLotId(ref string lotId);
        protected abstract CommandResults DoReadCarrierId(ref string CarrierId);
        protected abstract CommandResults DoWriteLotId(string lotId);
        protected abstract CommandResults DoWriteCarrierId(string carrierId);
        #endregion </Actions>

        public bool IsConnected()
        {
            if (this is EFEM.Modules.RFID.Controllers.RfidSimulator)
                return true;

            if (_comm == null)
                return false;

            return _comm.IsConnected;
        }
        protected bool DoAction(RfidCommand command, byte[] messageToSend)
        {
            if (false == IsConnected())
                return false;

            if (false == _doingAction.Equals(RfidCommand.IDLE))
                return false;

            _doingAction = command;

            return _comm.WriteByteData(messageToSend);
        }
        protected abstract void ParseMessages(byte[] receivedMessage, RfidCommand command);
        
        // 2024.12.21. jhlim [ADD] 연결되지 않는 경우 무한으로 익셉션이 발생해 로그가 계속 쌓이게된다..
        private bool RetryConnectIfNeed()
        {
            if (false == IsConnected())
            {
                if (_retryCount < RetryCountLimit && TicksForConnection.IsTickOver(false))
                {
                    _comm.OpenPort();

                    TicksForConnection.SetTickCount(RetryInterval);
                    ++_retryCount;
                }

                return false;
            }
            else
            {
                // Flag Reset
                _retryCount = 0;
                TicksForConnection.SetTickCount(RetryInterval);

                return true;
            }
        }
        // 2024.12.21. jhlim [END]
        #region <Execute>
        public void Monitoring()
        {
            // 2024.12.21. jhlim [ADD] 연결 체크를 상위에서 한다.
            if (false == RetryConnectIfNeed())
                return;
            // 2024.12.21. jhlim [END]

            Execute();
        }
        public abstract void Execute();
        #endregion </Execute>

        #endregion </Methods>
    }
}
