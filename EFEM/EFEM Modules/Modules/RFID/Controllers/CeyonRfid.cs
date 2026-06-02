using System.Text;

using Define.DefineEnumBase.Common;
using EFEM.Defines.Common;
using EFEM.Defines.RFID;
using RFIDOnly;

namespace EFEM.Modules.RFID.Controllers
{
    public enum CeyonFrameType
    {
        None,
        ReadData,
        WriteAck,
        ErrorNak
    }
    public class CeyonParsedFrame
    {
        public CeyonFrameType Type { get; set; }
        public RfidCommand Command { get; set; }

        // Read 응답일 때 데이터
        public byte[] Data { get; set; }

        // NAK일 때 에러 코드 (raw, 또는 나중에 메시지로 변환)
        public int ErrorCode { get; set; }
        public string ErrorMessage { get; set; }
    }

    public class CeyonRfid : RFIDReader
    {
        #region <Constructors>
        public CeyonRfid(int portId, EN_CONNECTION_TYPE interfaceType, int commIndex)
            : base(portId, interfaceType, commIndex)
        {
            _transaction = new CeyonTransaction();
        }
        #endregion </Constructors>

        #region <Fields>
        private readonly CeyonTransaction _transaction;
        private byte[] _receiveMessage;

        private RfidOperationStatus _operationStatus = RfidOperationStatus.Idle;
        private string _operationErrorCode = string.Empty;
        private byte[] _operationData = null;
        private RfidCommand _operationCommand = RfidCommand.IDLE;
        #endregion </Fields>

        #region <Type>
        private enum RfidOperationStatus
        {
            Idle,
            Waiting,    // 명령 전송 후 응답 대기
            Completed,
            Error,
            Timeout
        }
        #endregion </Type>

        #region <Properties>

        #endregion </Properties>

        #region <Methods>

        #region <Actions>
        public override void InitAction()
        {
            base.InitAction();
            
            _transaction.InitTransaction();
            _operationStatus = RfidOperationStatus.Idle;
            _operationErrorCode = string.Empty;
            _operationData = null;
            _operationCommand = RfidCommand.IDLE;
        }
        protected override CommandResults DoReadLotId(ref string lotId)
        {
            var command = RfidCommand.READ_LOT_ID;
            switch (_operationStatus)
            {
                case RfidOperationStatus.Error:
                    _actionStep = 0;
                    _operationStatus = RfidOperationStatus.Idle;
                    return new CommandResults(
                        command.ToString(),
                        CommandResult.Error,
                        _operationErrorCode);

                case RfidOperationStatus.Completed:
                    if (_operationData != null)
                    {
                        lotId = Encoding.ASCII.GetString(_operationData);
                    }
                    _operationData = null;
                    _actionStep = 0;
                    _operationStatus = RfidOperationStatus.Idle;
                    return new CommandResults(
                        command.ToString(),
                        CommandResult.Completed);

                case RfidOperationStatus.Timeout:
                    _actionStep = 0;
                    _operationStatus = RfidOperationStatus.Idle;
                    return new CommandResults(
                        command.ToString(),
                        CommandResult.Timeout);
            }

            return ExecuteCommand(command);
        }
        protected override CommandResults DoReadCarrierId(ref string carrierId)
        {
            var command = RfidCommand.READ_CARRIER_ID;
            switch (_operationStatus)
            {
                case RfidOperationStatus.Error:
                    _actionStep = 0;
                    _operationStatus = RfidOperationStatus.Idle;
                    return new CommandResults(
                        command.ToString(),
                        CommandResult.Error,
                        _operationErrorCode);

                case RfidOperationStatus.Completed:
                    if (_operationData != null)
                    {
                        carrierId = Encoding.ASCII.GetString(_operationData);
                    }
                    _operationData = null;
                    _actionStep = 0;
                    _operationStatus = RfidOperationStatus.Idle;
                    return new CommandResults(
                        command.ToString(),
                        CommandResult.Completed);

                case RfidOperationStatus.Timeout:
                    _actionStep = 0;
                    _operationStatus = RfidOperationStatus.Idle;
                    return new CommandResults(
                        command.ToString(),
                        CommandResult.Timeout);
            }

            return ExecuteCommand(command);
        }
        protected override CommandResults DoWriteLotId(string lotId)
        {
            var command = RfidCommand.WRITE_LOT_ID;
            switch (_operationStatus)
            {
                case RfidOperationStatus.Error:
                    _actionStep = 0;
                    _operationStatus = RfidOperationStatus.Idle;
                    return new CommandResults(
                        command.ToString(),
                        CommandResult.Error,
                        _operationErrorCode);

                case RfidOperationStatus.Completed:
                    _actionStep = 0;
                    _operationStatus = RfidOperationStatus.Idle;
                    return new CommandResults(
                        command.ToString(),
                        CommandResult.Completed);

                case RfidOperationStatus.Timeout:
                    _actionStep = 0;
                    _operationStatus = RfidOperationStatus.Idle;
                    return new CommandResults(
                        command.ToString(),
                        CommandResult.Timeout);
            }

            // 아직 진행 중인 상태라면, 처음엔 송신 정보 세팅 후 상태 머신 진행
            _transaction.StringToWrite = lotId.PadRight(LotIdLength, ' ');
            return ExecuteCommand(command);
        }
        protected override CommandResults DoWriteCarrierId(string carrierId)
        {
            var command = RfidCommand.WRITE_CARRIER_ID;
            switch (_operationStatus)
            {
                case RfidOperationStatus.Error:
                    _actionStep = 0;
                    _operationStatus = RfidOperationStatus.Idle;
                    return new CommandResults(
                        command.ToString(),
                        CommandResult.Error,
                        _operationErrorCode);

                case RfidOperationStatus.Completed:
                    _actionStep = 0;
                    _operationStatus = RfidOperationStatus.Idle;
                    return new CommandResults(
                        command.ToString(),
                        CommandResult.Completed);

                case RfidOperationStatus.Timeout:
                    _actionStep = 0;
                    _operationStatus = RfidOperationStatus.Idle;
                    return new CommandResults(
                        command.ToString(),
                        CommandResult.Timeout);
            }

            // 아직 진행 중인 상태라면, 처음엔 송신 정보 세팅 후 상태 머신 진행
            _transaction.StringToWrite = carrierId.PadRight(CarrierIdLength, ' ');
            return ExecuteCommand(command);
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
        private void ApplyParsedFrame(CeyonParsedFrame frame)
        {
            if (frame == null)
                return;

            // 이미 완료/에러/타임아웃 난 상태면 더 건드리지 않는다.
            if (_operationStatus == RfidOperationStatus.Completed ||
                _operationStatus == RfidOperationStatus.Error ||
                _operationStatus == RfidOperationStatus.Timeout)
            {
                return;
            }

            // 현재 진행 중인 명령이 아니면 무시 (혹시나 다른 타이밍 패킷 대비)
            if (frame.Command != _operationCommand)
            {
                return;
            }

            switch (frame.Type)
            {
                case CeyonFrameType.ErrorNak:
                    _operationStatus = RfidOperationStatus.Error;
                    _operationErrorCode = frame.ErrorMessage;
                    break;

                case CeyonFrameType.ReadData:
                    _operationStatus = RfidOperationStatus.Completed;
                    _operationData = frame.Data;
                    break;

                case CeyonFrameType.WriteAck:
                    _operationStatus = RfidOperationStatus.Completed;
                    break;

                case CeyonFrameType.None:
                default:
                    // 아무 변화도 없는 패킷
                    break;
            }
        }

        private CommandResults ExecuteCommand(RfidCommand command)
        {
            switch (_actionStep)
            {
                case 0:
                    {
                        _transaction.ResetStatus();

                        _operationStatus = RfidOperationStatus.Waiting;
                        _operationErrorCode = string.Empty;
                        _operationData = null;
                        _operationCommand = command;

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
                        //_result.CommandResult = CommandResult.Timeout;
                        _operationStatus = RfidOperationStatus.Timeout;
                        break;
                    }
                    break;

                default:
                    _operationStatus = RfidOperationStatus.Error;
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
                    data = _transaction.CommandRead(command);
                    break;
                case RfidCommand.WRITE_LOT_ID:
                case RfidCommand.WRITE_CARRIER_ID:
                    data = _transaction.CommandWrite(command);
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
            var frame = _transaction.ParseMessages(receivedMessage, command);
            ApplyParsedFrame(frame);
        }
        private bool TimeIsOver()
        {
            if (_timeChecker.IsTickOver(true))
            {
                _actionStep = 0;

                _operationStatus = RfidOperationStatus.Timeout;

                return true;
            }

            return false;
        }
    }

    #region <Transaction Class>
    public class CeyonTransaction
    {
        #region <Fields>
        private const byte _STX = 0x02;
        //private const byte _NULL = 0x00;
        private const byte _ETX = 0x03;
        private const byte _ACK = 0x06;
        private const byte _NAK = 0x15;
        private const byte _ENQ = 0x05;
        private const byte ReadingMode = 0x80;
        private const byte WritingMode = 0x90;

        //private byte[] CMD = new byte[1];
        //private byte[] LEN = new byte[1];
        
        private byte _readerId;
        #endregion </Fields>

        #region <Properties>
        public int AddressLotId { get; set; }
        public int LengthLotId { get; set; }
        public int AddressCarrierId { get; set; }
        public int LengthCarrierId { get; set; }
        public string StringToWrite { get; set; }
        #endregion </Properties>

        #region <Method>
        public void ResetStatus()
        {
            StringToWrite = string.Empty;
        }
        public void InitTransaction()
        {
            ResetStatus();
        }
        public byte[] CommandRead(RfidCommand command)
        {
            byte[] commandData = new byte[6];

            //ENQ[0] = 0x05;
            //CMD[0] = ReadingMode;

            _readerId = 0x01;//(byte)1;

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

            commandData[0] = _ENQ;
            commandData[1] = _readerId;
            commandData[2] = ReadingMode;
            commandData[3] = DATA[0];
            commandData[4] = DATA[1];
            
            for (int i = 0; i < commandData.Length - 1; i++) 
                commandData[5] += commandData[i];

            return commandData;
        }
        public byte[] CommandWrite(RfidCommand command)
        {
            // [ENQ], [RID], [CMD], [ADD], [LEN] ....[ID]....[CS]

            //ENQ[0] = 0x05;
            _readerId = 0x01;// (byte)1;
            //CMD[0] = WritingMode;

            switch (command)
            {
                case RfidCommand.WRITE_LOT_ID:
                    {
                        if (string.IsNullOrEmpty(StringToWrite))
                            return null;

                        byte[] writeLotId = new byte[StringToWrite.Length + 6];
                        writeLotId[0] = _ENQ;
                        writeLotId[1] = _readerId;
                        writeLotId[2] = WritingMode;
                        writeLotId[3] = (byte)AddressLotId;
                        writeLotId[4] = (byte)StringToWrite.Length;

                        byte[] tempWriteLotId = Encoding.ASCII.GetBytes(StringToWrite);

                        for (int i = 0; i < tempWriteLotId.Length; i++)
                        {
                            writeLotId[i + 5] = tempWriteLotId[i];
                        }

                        for (int i = 0; i < writeLotId.Length - 1; i++)
                        {
                            writeLotId[writeLotId.Length - 1] += writeLotId[i];
                        }

                        return writeLotId;
                    }

                case RfidCommand.WRITE_CARRIER_ID:
                    {
                        if (string.IsNullOrEmpty(StringToWrite))
                            return null;

                        byte[] writeCarrierId = new byte[StringToWrite.Length + 6];
                        writeCarrierId[0] = _ENQ;
                        writeCarrierId[1] = _readerId;
                        writeCarrierId[2] = WritingMode;
                        writeCarrierId[3] = (byte)AddressCarrierId;
                        writeCarrierId[4] = (byte)StringToWrite.Length;

                        byte[] tempWriteCarrierId = Encoding.ASCII.GetBytes(StringToWrite);

                        for (int i = 0; i < tempWriteCarrierId.Length; i++)
                        {
                            writeCarrierId[i + 5] = tempWriteCarrierId[i];
                        }

                        for (int i = 0; i < writeCarrierId.Length - 1; i++)
                        {
                            writeCarrierId[writeCarrierId.Length - 1] += writeCarrierId[i];
                        }

                        return writeCarrierId;
                    }

                default:
                    break;
            }

            return null;
        }
        public CeyonParsedFrame ParseMessages(byte[] receivedMessage, RfidCommand command)
        {
            var frame = new CeyonParsedFrame
            {
                Type = CeyonFrameType.None,
                Command = command,
                Data = null,
                ErrorCode = 0,
                ErrorMessage = string.Empty
            };

            // 유효하지 않은 프레임
            if (receivedMessage == null || receivedMessage.Length < 3)
                return frame;

            //_STX = 0x02;
            //_NULL = 0x00;
            //_ETX = 0x03;
            //_ACK = 0x06;
            //_NAK = 0x15;

            var stx = false;
            var etx = false;

            int rno = 0;
            byte[] buff = receivedMessage;

            // READ 모드 응답
            if (buff[2] == ReadingMode)
            {
                if (buff[0] == _STX) stx = true;
                if (buff[buff.Length - 1] == _ETX) etx = true;

                if (stx && etx)
                {
                    switch (command)
                    {
                        case RfidCommand.READ_LOT_ID:
                            {
                                if (LengthLotId <= 0)
                                    break;

                                byte[] lotId = new byte[LengthLotId];
                                for (int i = AddressLotId; i < AddressLotId + LengthLotId; i++)
                                {
                                    lotId[i - AddressLotId] = buff[rno + 3];
                                    rno++;
                                }

                                frame.Type = CeyonFrameType.ReadData;
                                frame.Data = lotId;
                            }
                            break;

                        case RfidCommand.READ_CARRIER_ID:
                            {
                                if (LengthCarrierId <= 0)
                                    break;

                                byte[] carrierID = new byte[LengthCarrierId];
                                for (int i = AddressCarrierId; i < AddressCarrierId + LengthCarrierId; i++)
                                {
                                    carrierID[i - AddressCarrierId] = buff[rno + 3];
                                    rno++;
                                }
                                frame.Type = CeyonFrameType.ReadData;
                                frame.Data = carrierID;
                            }
                            break;
                    }
                }
                else if (buff[0] == _NAK && etx)
                {
                    // NAK + 에러코드 위치는 기존 구현에 맞게 유지
                    int errorCode = buff[buff.Length - 2];
                    frame.Type = CeyonFrameType.ErrorNak;
                    frame.ErrorCode = errorCode;
                    frame.ErrorMessage = GetErrorMessage(errorCode);
                }
            }
            // WRITE 모드 응답
            else if (buff[2] == WritingMode)
            {
                if (buff[0] == _ACK) stx = true;
                if (buff[buff.Length - 1] == _ETX) etx = true;

                if (buff[0] == _NAK)
                {
                    int errorCode = buff[buff.Length - 2];
                    frame.Type = CeyonFrameType.ErrorNak;
                    frame.ErrorCode = errorCode;
                    frame.ErrorMessage = GetErrorMessage(errorCode);
                }
                else if (stx && etx)
                {
                    if (buff[1] == 0x01)
                    {
                        frame.Type = CeyonFrameType.WriteAck;
                    }
                }
            }

            return frame;
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
