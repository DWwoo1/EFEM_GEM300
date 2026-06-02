using System.Text;

using Define.DefineEnumBase.Common;
using EFEM.Defines.Common;
using EFEM.Defines.RFID;
using RFIDOnly;

namespace EFEM.Modules.RFID.Controllers
{
    public class CeyonRfid : RRFIDReader
    {
        #region <Constructors>
        public CeyonRfid(int portId, EN_CONNECTION_TYPE interfaceType, int commIndex)
            : base(portId, interfaceType, commIndex)
        {
            _transaction = new CeyonTransaction(commIndex);
        }
        #endregion </Constructors>

        #region <Fields>
        private readonly CeyonTransaction _transaction = null;

        private byte[] _receiveMessage;
        StringBuilder StringBuilder = new StringBuilder();
        #endregion </Fields>

        #region <Properties>

        #endregion </Properties>

        #region <Methods>

        #region <Actions>
        public override void InitAction()
        {
            base.InitAction();
            _transaction.InitTransaction();
        }
        protected override CommandResults DoReadLotId(ref string lotId)
        {
            if (_transaction.ErrorFlag == true)
            {
                _transaction.ErrorFlag = false;
                return new CommandResults(RfidCommand.READ_LOT_ID.ToString(), CommandResult.Error, _transaction.ErrorCode);
            }

            if (_transaction.ByteLotId != null)
            {
                lotId = Encoding.ASCII.GetString(_transaction.ByteLotId);
                _transaction.ByteLotId = null;
                _actionStep = 0;
                return new CommandResults(RfidCommand.READ_LOT_ID.ToString(), CommandResult.Completed);
            }
            return ExecuteCommand(RfidCommand.READ_LOT_ID);
        }
        protected override CommandResults DoReadCarrierId(ref string CarrierId)
        {
            if (_transaction.ErrorFlag == true)
            {
			    _transaction.ErrorFlag = false;
                return new CommandResults(RfidCommand.READ_CARRIER_ID.ToString(), CommandResult.Error, _transaction.ErrorCode);
            }
		    if (_transaction.ByteCarrierId != null)
            {
                CarrierId = Encoding.ASCII.GetString(_transaction.ByteCarrierId);
                _transaction.ByteCarrierId = null;
                _actionStep = 0;
                return new CommandResults(RfidCommand.READ_CARRIER_ID.ToString(), CommandResult.Completed);
            }
            return ExecuteCommand(RfidCommand.READ_CARRIER_ID);
        }
        protected override CommandResults DoWriteLotId(string lotId)
        {
            if (_transaction.ErrorFlag == true)
            {
				_transaction.ErrorFlag = false;
                _transaction._isReceivedDataCh = false;
                return new CommandResults(RfidCommand.WRITE_LOT_ID.ToString(), CommandResult.Error, _transaction.ErrorCode);
            }
            if(_transaction._isReceivedDataCh)
            {
                _actionStep = 0;
                _transaction._isReceivedDataCh = false;
                return new CommandResults(RfidCommand.WRITE_LOT_ID.ToString(), CommandResult.Completed);
            }
            _transaction.StringLotId = lotId;
            return ExecuteCommand(RfidCommand.WRITE_LOT_ID);
        }
        protected override CommandResults DoWriteCarrierId(string carrierId)
        {
            if (_transaction.ErrorFlag == true)
            {
				_transaction.ErrorFlag = false;
                return new CommandResults(RfidCommand.WRITE_CARRIER_ID.ToString(), CommandResult.Error, _transaction.ErrorCode);
            }
            if (_transaction._isReceivedDataCh)
            {
                _actionStep = 0;
                _transaction._isReceivedDataCh = false;
                return new CommandResults(RfidCommand.WRITE_CARRIER_ID.ToString(), CommandResult.Completed);
            }
            _transaction.StringCarrierId = carrierId;
            return ExecuteCommand(RfidCommand.WRITE_CARRIER_ID);
        }
        #endregion </Actions>

        #region <Execute>
        public override void Execute()
        {
            if (_result.CommandResult == CommandResult.Proceed)
            {
                if (false == _comm.ReadByteData(ref _receiveMessage))
                    return;

                ParseMessages(_receiveMessage, _doingAction);
            }
        }
        #endregion </Execute>

        #endregion </Methods>

        private CommandResults ExecuteCommand(RfidCommand command)
        {
            switch (_actionStep)
            {
                case 0:
                    {
                        _timeChecker.SetTickCount(5000);
                        _doingAction = RfidCommand.IDLE;
                        _result.CommandResult = CommandResult.Proceed;
                        _result.Description = string.Empty;
                        ++_actionStep;
                    }
                    break;
                case 1:
                    {
                        if (SendMessage(command))
                        {
                            _doingAction = command;
                            ++_actionStep;
                        }
                    }
                    break;
                case 2:
                    if (TimeIsOver())
                    {
                        _result.CommandResult = CommandResult.Timeout;
                        break;
                    }
                    break;

                default:
                    _result.CommandResult = CommandResult.Invalid;
                    _result.Description = string.Format("Invalid Seq Num : {0}", _actionStep);
                    break;
            }

            if (false == _result.CommandResult.Equals(CommandResult.Proceed))
            {
                _doingAction = RfidCommand.IDLE;
                _actionStep = 0;
            }

            return _result;
        }
        private bool SendMessage(RfidCommand command)
        {
            byte[] data = null;

            _transaction.AddressLotId = LotIdAddress;
            _transaction.LengthLotId = LotIdLength;
            _transaction.AddressCarrierId = CarrierIdAddress;
            _transaction.LengthCarrierId = CarrierIdLength;

            switch (command)
            {
                case RfidCommand.READ_LOT_ID:
                case RfidCommand.READ_CARRIER_ID:
                    data = _transaction.commandRead(command);
                    break;
                case RfidCommand.WRITE_LOT_ID:
                case RfidCommand.WRITE_CARRIER_ID:
                    data = _transaction.commandWrite(command);
                    break;
                default:
                    break;
            }

            if (data == null)
                return false;

            return DoAction(command, data);
        }
        protected override void ParseMessages(byte[] receivedMessage, RfidCommand command)
        {
            // [DATA]를 제외한 코드 수 [STX], [RID], [CMD], [ETX] 4개
            int count = receivedMessage.Length - 4;

            switch (command)
            {
                case RfidCommand.READ_LOT_ID:
                    _transaction.ParseMessages(receivedMessage, RfidCommand.READ_LOT_ID);
                    break;
                case RfidCommand.READ_CARRIER_ID:
                    _transaction.ParseMessages(receivedMessage, RfidCommand.READ_CARRIER_ID);
                    break;
                case RfidCommand.WRITE_LOT_ID:
                    _transaction.ParseMessages(receivedMessage, RfidCommand.WRITE_LOT_ID);
                    break;
                case RfidCommand.WRITE_CARRIER_ID:
                    _transaction.ParseMessages(receivedMessage, RfidCommand.WRITE_CARRIER_ID);
                    break;
                default:
                    break;
            }
        }
        private bool TimeIsOver()
        {
            if (_timeChecker.IsTickOver(true))
            {
                _actionStep = 0;

                return true;
            }

            return false;
        }
    }

    #region <Transaction Class>
    public class CeyonTransaction
    {

        #region <Constructors>
        public CeyonTransaction(int commIndex)
        {
            _readingMode = (byte)(0x80);
            _writingMode = (byte)(0x90);
        }
        #endregion </Constructors>

        #region <Fields>
        public int AddressLotId;
        public int LengthLotId;
        public int AddressCarrierId;
        public int LengthCarrierId;
        public byte[] ByteLotId;
        public byte[] ByteCarrierId;
        public string StringLotId;
        public string StringCarrierId;
        public string[] WriteLotId;
        public string ErrorCode = string.Empty;
        public bool ErrorFlag = false;

        private byte[] STX = new byte[1];
        private byte[] NULL = new byte[1];
        private byte[] ETX = new byte[1];  //End
        private byte[] ENQ = new byte[1];  //송수신 시작
        private byte[] CMD = new byte[1];
        private byte[] ACK = new byte[1];
        private byte[] NAK = new byte[1];  //Error 알림
        private byte[] LEN = new byte[1];
        private byte[] READERID = new byte[1];

        private byte _readingMode;
        private byte _writingMode;

        private bool _stx, _etx;

        public string _readerId;
        public string _dataCassetteIDCh, _dataLotIDCh;
        public bool _isReceivedDataCh = false;

        #endregion </Fields>

        #region <Properties>
        #endregion </Properties>

        #region <Method>
        public void InitTransaction()
        {
            ErrorFlag = false;
            ErrorCode = string.Empty;
            _isReceivedDataCh = false;
        }
        public byte[] commandRead(RfidCommand command)
        {
            byte[] commandData = new byte[6];

            ENQ[0] = 0x05;
            CMD[0] = _readingMode;

            READERID[0] = (byte)1;

            byte[] DATA = new byte[2];

            switch (command)
            {
                case RfidCommand.READ_LOT_ID:
                    DATA[0] = (byte)AddressLotId;
                    DATA[1] = (byte)LengthLotId;

                    break;
                case RfidCommand.READ_CARRIER_ID:
                    DATA[0] = (byte)AddressCarrierId;
                    DATA[1] = (byte)LengthCarrierId;
                    break;
                default:
                    break;
            }

            commandData[0] = ENQ[0];
            commandData[1] = READERID[0];
            commandData[2] = CMD[0];
            commandData[3] = DATA[0];
            commandData[4] = DATA[1];
            for (int i = 0; i < commandData.Length - 1; i++) commandData[5] += (byte)commandData[i];

            return commandData;
        }
        public byte[] commandWrite(RfidCommand command)
        {
            char[] assciiValue;
            // [ENQ], [RID], [CMD], [ADD], [LEN] ....[ID]....[CS]

            ENQ[0] = 0x05;
            READERID[0] = (byte)1;
            CMD[0] = _writingMode;
            _isReceivedDataCh = false;

            switch (command)
            {
                case RfidCommand.WRITE_LOT_ID:
                    byte[] WriteLotId = new byte[StringLotId.Length + 6];
                    byte[] tempWriteLotId = new byte[StringLotId.Length];

                    WriteLotId[0] = ENQ[0];
                    WriteLotId[1] = READERID[0];
                    WriteLotId[2] = CMD[0];
                    WriteLotId[3] = (byte)AddressLotId;
                    WriteLotId[4] = (byte)StringLotId.Length;

                    assciiValue = StringLotId.ToCharArray();
                    tempWriteLotId = Encoding.UTF8.GetBytes(assciiValue);

                    for (int i = 0; i < tempWriteLotId.Length; i++)
                    {
                        WriteLotId[i + 5] = tempWriteLotId[i];
                    }

                    for (int i = 0; i < WriteLotId.Length - 1; i++)
                    {
                        WriteLotId[WriteLotId.Length - 1] += (byte)WriteLotId[i];
                    }

                    return WriteLotId;

                case RfidCommand.WRITE_CARRIER_ID:
                    byte[] WriteCarrierId = new byte[StringCarrierId.Length + 6];
                    byte[] tempWriteCarrierId = new byte[StringCarrierId.Length];

                    WriteCarrierId[0] = ENQ[0];
                    WriteCarrierId[1] = READERID[0];
                    WriteCarrierId[2] = CMD[0];
                    WriteCarrierId[3] = (byte)AddressLotId;
                    WriteCarrierId[4] = (byte)StringCarrierId.Length;

                    assciiValue = StringCarrierId.ToCharArray();
                    tempWriteCarrierId = Encoding.UTF8.GetBytes(assciiValue);

                    for (int i = 0; i < tempWriteCarrierId.Length; i++)
                    {
                        WriteCarrierId[i + 5] = tempWriteCarrierId[i];
                    }

                    for (int i = 0; i < WriteCarrierId.Length - 1; i++)
                    {
                        WriteCarrierId[WriteCarrierId.Length - 1] += (byte)WriteCarrierId[i];
                    }
                    return WriteCarrierId;

                default:
                    break;
            }
            return null;
        }
        public void ParseMessages(byte[] receivedMessage, RfidCommand command)
        {
            STX[0] = 0x02;
            NULL[0] = 0x00;
            ETX[0] = 0x03;
            ACK[0] = 0x06;
            NAK[0] = 0x15;

            _stx = false;
            _etx = false;
            _isReceivedDataCh = false;
            ErrorFlag = false;

            int rno = 0;
            byte[] buff = receivedMessage;

            if (buff[2] == _readingMode)
            {
                if (buff[0] == 0x02) _stx = true;
                if (buff[buff.Length - 1] == 0x03) _etx = true;

                // 제일 앞에 [STX], [RID], [CMD] 제외시켜야함
                if (_stx && _etx)
                {
                    switch (command)
                    {
                        case RfidCommand.READ_LOT_ID:
                            ByteLotId = null;
                            _dataLotIDCh = string.Empty;
                            byte[] lotID = new byte[LengthLotId];

                            if (lotID.Length != LengthLotId)
                            {
                                return;
                            }

                            for (int i = AddressLotId; i < AddressLotId + LengthLotId; i++)
                            {
                                lotID[i - AddressLotId] = buff[rno + 3];
                                rno++;
                            }

                            _dataLotIDCh = receivedMessage[2].ToString();
                            ByteLotId = lotID;
                            _isReceivedDataCh = true;

                            break;

                        case RfidCommand.READ_CARRIER_ID:
                            ByteCarrierId = null;
                            _dataCassetteIDCh = string.Empty;
                            byte[] carrierID = new byte[LengthCarrierId];

                            if (carrierID.Length != LengthCarrierId)
                            {
                                return;
                            }

                            for (int i = AddressCarrierId; i < AddressCarrierId + LengthCarrierId; i++)
                            {
                                carrierID[i - AddressCarrierId] = buff[rno + 3];
                                rno++;
                            }

                            _dataCassetteIDCh = receivedMessage[2].ToString();
                            ByteCarrierId = carrierID;
                            _isReceivedDataCh = true;
                            break;
                        default:
                            break;
                    }
                }
                else if (buff[0] == NAK[0] && _etx)                                    // Error -> 테스트 필요
                {
                    ErrorCode = string.Empty;

                    if (buff[0] == NAK[0])
                    {
                        ErrorCode = GetErrorMessage(buff[buff.Length - 2]);
                        ErrorFlag = true;
                    }

                    // 160127 jhlim : stx, etx가 안와도 같이 null로 만들어서 통신에러로 빼버리자..
                    switch (command)
                    {
                        case RfidCommand.READ_LOT_ID:
                            ByteLotId = null;
                            _isReceivedDataCh = true;
                            break;

                        case RfidCommand.READ_CARRIER_ID:
                            _dataCassetteIDCh = null;
                            _isReceivedDataCh = true;
                            break;
                        default:
                            break;
                    }
                }
            }
            else if (buff[2] == _writingMode)            // Write Mode
            {
                if (buff[0] == 0x06) _stx = true;
                if (buff[buff.Length - 1] == 0x03) _etx = true;
                if (buff[0] == NAK[0])
                {
                    ErrorCode = string.Empty;
                    if (buff[0] == NAK[0])
                    {
                        ErrorCode = GetErrorMessage(buff[buff.Length - 2]);
                        ErrorFlag = true;
                    }
                }

                // buff[2]가 Write 명령일 때 인데, buff[1] == 0x80이 어떻게 나오는건지 모르겠음
                if (_stx && _etx)
                {
                    //if (buff[1] == 0x80) _isReceivedDataCh = true;
                    if (buff[1] == (byte)1) _isReceivedDataCh = true;
                }
            }
        }
        public string GetErrorMessage(int errorCode)
        {
            string message = "";
            switch (errorCode)
            {
                case 0x00:
                    //msg = "No Error";
                    break;
                case 0x01:
                    message = "Unknown Command ID";
                    break;
                case 0x02:
                    message = "Not Yet Implemented Command ID";
                    break;
                case 0x03:
                    message = "Invalid Destination Address(Device ID)";
                    break;
                case 0x04:
                    message = "Invalid System Register Address";
                    break;
                case 0x05:
                    message = "Timeout Error";
                    break;
                case 0x06:
                    message = "Invalid SLRC Register Address";
                    break;
                case 0x07:
                    message = "Out of System Register Address Range";
                    break;
                case 0x08:
                    message = "Out of SLRC Register Address Range";
                    break;
                case 0x09:
                    message = "Out of RF Channel Number";
                    break;
                case 0x0A:
                    message = "Out of Bit Range";
                    break;
                case 0x0B:
                    message = "Invalid Bit Value";
                    break;
                case 0x0C:
                    message = "Check Sum Error";
                    break;
                case 0x0D:
                    message = "Write Command Fail";
                    break;
                case 0x0E:
                    message = "Read Command Fail";
                    break;
                case 0x0F:
                    message = "Long Data Length (max 32 bytes)";
                    break;
                case 0x10:
                    message = "RF Channel Disabled";
                    break;
                case 0x11:
                    message = "SLRC Reset Error";
                    break;
                case 0x12:
                    message = "SLRC Parallel Bus Error";
                    break;
                case 0x13:
                    message = "Max Timeslot Error(max 255)";
                    break;
                case 0x14:
                    message = "Not Supported RF Protocol";
                    break;
                case 0x15:
                    message = "ICODE Wrong Command Parameter";
                    break;
                case 0x16:
                    message = "ICODE Timeout";
                    break;
                case 0x17:
                    message = "ICODE No Tag";
                    break;
                case 0x18:
                    message = "ICODE CRC Error";
                    break;
                case 0x19:
                    message = "ICODE Collision Error";
                    break;
                case 0x1A:
                    message = "ICODE SNR Error";
                    break;
                case 0x1B:
                    message = "ICODE Count Error";
                    break;
                case 0x1C:
                    message = "RFU 0x1C";
                    break;
                case 0x1D:
                    message = "ICODE Invalid Quit Value";
                    break;
                case 0x1E:
                    message = "ICODE Weak Collision Error";
                    break;
                case 0x1F:
                    message = "ICODE Write Fail";
                    break;
                case 0x20:
                    message = "ICODE Halt Fail";
                    break;
                case 0x21:
                    message = "ICODE Not implemented Error";
                    break;
                case 0x22:
                    message = "RFU 0x22";
                    break;
                case 0x23:
                    message = "RFU 0x23";
                    break;
                case 0x24:
                    message = "RFU 0x24";
                    break;
                case 0x25:
                    message = "RFU 0x25";
                    break;
                case 0x26:
                    message = "RFU 0x26";
                    break;
                case 0x27:
                    message = "Family Code Mismatch";
                    break;
                case 0x28:
                    message = "Application Code Mismatch";
                    break;
                case 0x29:
                    message = "ICODE Framing Error";
                    break;
                case 0x2A:
                    message = "Carrier Disabled";
                    break;
                case 0xA1:
                    message = "during the writing Write OR Read command Receive.";
                    break;
                case 0xA2:
                    message = "during the Reading Write OR Read command Receive.";
                    break;
                case 0xA3:
                    message = "Write Data size is 112Byte OVER.";
                    break;
                case 0xA4:
                    message = "No Match Data size.";
                    break;
                default:
                    message = string.Format("Unknown error. 0x{0,2:x4}", errorCode);
                    break;
            }
            return (message);
        }
        #endregion </Method>
    }
    #endregion </Transaction Class>
}
