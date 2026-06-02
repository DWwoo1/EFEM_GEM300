using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using RFIDOnly;
using EFEM.Defines.Common;
using EFEM.Defines.RFID;
using Define.DefineEnumBase.Common;

namespace EFEM.Modules.RFID.Controllers
{
    public enum XedionFrameType
    {
        None,
        ReadData,   // 페이지에서 읽은 데이터
        WriteAck,   // 쓰기 명령 정상 ACK
        Error       // 에러 응답 (ErrorMessage 'e')
    }

    public class XedionParsedFrame
    {
        public XedionFrameType Type { get; set; }
        public RfidCommand Command { get; set; }

        // Read 응답일 때 사용 (페이지 데이터 그대로)
        public byte[] Data { get; set; }

        // Read 응답일 때 페이지 번호 (예: "12")
        public string Page { get; set; }

        // 에러일 때 에러 코드 (Reader에서 내려오는 '0'~'B')
        public char ErrorCode { get; set; }
        public string ErrorMessage { get; set; }
    }

    public class XedionRfid : RFIDReader
    {
        #region <Constructors>
        public XedionRfid(int portId, EN_CONNECTION_TYPE interfaceType, int commIndex)
            : base(portId, interfaceType, commIndex)
        {
            _transaction = new XedionTransaction(commIndex);
        }
        #endregion </Constructors>

        #region <Fields>
        private readonly XedionTransaction _transaction = null;
        private byte[] _receiveMessage;
        
        //private bool _writeCommandAckFlag = false;
        
        private string _dataToWrite = string.Empty;
        private string[] _dataToWriteBySplit;
        private const int StringLengthAtPage = 8;
        private string _temporaryData = string.Empty;
        private int _currentCycleTime;
        private int _totalCycleTime;
        private string _errorDiscription = string.Empty;

        private XedionOperationStatus _operationStatus = XedionOperationStatus.Idle;
        private string _operationErrorDescription = string.Empty;
        private string _operationData = string.Empty;        // 전체 페이지 누적 문자열
        private RfidCommand _operationCommand = RfidCommand.IDLE;

        // Read 응답 한 페이지 분
        private bool _hasPendingReadDataPage = false;
        private byte[] _pendingReadDataPageBytes = null;
        private bool _writeAckReceived = false;
        #endregion </Fields>

        #region <Type>
        private enum XedionOperationStatus
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
            
            _transaction.InitFlag();

            _operationStatus = XedionOperationStatus.Idle;
            _operationErrorDescription = string.Empty;
            _operationData = string.Empty;
            _operationCommand = RfidCommand.IDLE;

            _hasPendingReadDataPage = false;
            _pendingReadDataPageBytes = null;
            _writeAckReceived = false;
        }

        protected override CommandResults DoReadLotId(ref string lotId)
        {
            var command = RfidCommand.READ_LOT_ID;

            switch (_operationStatus)
            {
                case XedionOperationStatus.Error:
                    _actionStep = 0;
                    _operationStatus = XedionOperationStatus.Idle;
                    return new CommandResults(
                        command.ToString(),
                        CommandResult.Error,
                        _operationErrorDescription);

                case XedionOperationStatus.Completed:
                    if (!string.IsNullOrEmpty(_operationData))
                    {
                        lotId = _operationData;
                    }
                    _operationData = string.Empty;
                    _actionStep = 0;
                    _operationStatus = XedionOperationStatus.Idle;
                    return new CommandResults(
                        command.ToString(),
                        CommandResult.Completed);

                case XedionOperationStatus.Timeout:
                    _actionStep = 0;
                    _operationStatus = XedionOperationStatus.Idle;
                    return new CommandResults(
                        command.ToString(),
                        CommandResult.Timeout);
            }

            // 아직 진행 중(Idle/Waiting)이면 기존 FSM 진행
            return ExecuteReadingAction(command);
        }
        protected override CommandResults DoReadCarrierId(ref string carrierId)
        {
            var command = RfidCommand.READ_CARRIER_ID;

            switch (_operationStatus)
            {
                case XedionOperationStatus.Error:
                    _actionStep = 0;
                    _operationStatus = XedionOperationStatus.Idle;
                    return new CommandResults(
                        command.ToString(),
                        CommandResult.Error,
                        _operationErrorDescription);

                case XedionOperationStatus.Completed:
                    if (!string.IsNullOrEmpty(_operationData))
                    {
                        carrierId = _operationData;
                    }
                    _operationData = string.Empty;
                    _actionStep = 0;
                    _operationStatus = XedionOperationStatus.Idle;
                    return new CommandResults(
                        command.ToString(),
                        CommandResult.Completed);

                case XedionOperationStatus.Timeout:
                    _actionStep = 0;
                    _operationStatus = XedionOperationStatus.Idle;
                    return new CommandResults(
                        command.ToString(),
                        CommandResult.Timeout);
            }

            return ExecuteReadingAction(command);
        }

        protected override CommandResults DoWriteLotId(string lotId)
        {
            var command = RfidCommand.WRITE_LOT_ID;

            switch (_operationStatus)
            {
                case XedionOperationStatus.Error:
                    _actionStep = 0;
                    _operationStatus = XedionOperationStatus.Idle;
                    return new CommandResults(
                        command.ToString(),
                        CommandResult.Error,
                        _operationErrorDescription);

                case XedionOperationStatus.Completed:
                    _actionStep = 0;
                    _operationStatus = XedionOperationStatus.Idle;
                    return new CommandResults(
                        command.ToString(),
                        CommandResult.Completed);

                case XedionOperationStatus.Timeout:
                    _actionStep = 0;
                    _operationStatus = XedionOperationStatus.Idle;
                    return new CommandResults(
                        command.ToString(),
                        CommandResult.Timeout);
            }

            return ExecuteWritingAction(command, lotId);
        }

        protected override CommandResults DoWriteCarrierId(string carrierId)
        {
            var command = RfidCommand.WRITE_CARRIER_ID;

            switch (_operationStatus)
            {
                case XedionOperationStatus.Error:
                    _actionStep = 0;
                    _operationStatus = XedionOperationStatus.Idle;
                    return new CommandResults(
                        command.ToString(),
                        CommandResult.Error,
                        _operationErrorDescription);

                case XedionOperationStatus.Completed:
                    _actionStep = 0;
                    _operationStatus = XedionOperationStatus.Idle;
                    return new CommandResults(
                        command.ToString(),
                        CommandResult.Completed);

                case XedionOperationStatus.Timeout:
                    _actionStep = 0;
                    _operationStatus = XedionOperationStatus.Idle;
                    return new CommandResults(
                        command.ToString(),
                        CommandResult.Timeout);
            }

            return ExecuteWritingAction(command, carrierId);
        }

        #endregion </Actions>

        #region <Internals>
        private string StringHexToStringAscii(string inputHex)
        {
            var stringBuilder = new StringBuilder();

            for (int i = 0; i < inputHex.Length; i += 2)
            {
                string hexValue = inputHex.Substring(i, 2);
                int convertHex = Convert.ToInt32(hexValue, 16);
                char convertAscii = Convert.ToChar(convertHex);
                stringBuilder.Append(convertAscii);
            }

            return stringBuilder.ToString();
        }
        #endregion </Internals>

        #region <Methods>
        private bool SendMessageForRead(RfidCommand command, int currentPage)
        {
            int startingPage = 0;

            switch (command)
            {
                case RfidCommand.READ_LOT_ID:
                    startingPage = LotIdAddress + currentPage;
                    break;

                case RfidCommand.READ_CARRIER_ID:
                    startingPage = CarrierIdAddress + currentPage;
                    break;

                default:
                    break;
            }

            _transaction.PageToHandling = startingPage.ToString().PadLeft(2, '0');
            byte[] data = _transaction.ReadDataAtPage(command, _transaction.PageToHandling);

            return DoAction(command, data);
        }
        private bool SendMessageForWrite(RfidCommand command, string dataToWrite, int currentPage)
        {
            int startingPage = 0;

            switch (command)
            {
                case RfidCommand.WRITE_LOT_ID:
                    startingPage = LotIdAddress + currentPage;
                    break;

                case RfidCommand.WRITE_CARRIER_ID:
                    startingPage = CarrierIdAddress + currentPage;
                    break;

                default:
                    break;
            }

            _transaction.PageToHandling = startingPage.ToString().PadLeft(2, '0');
            byte[] data = _transaction.WriteDataToPage(command, dataToWrite, _transaction.PageToHandling);

            return DoAction(command, data);
        }
        #endregion </Methods>

        #region <Execute>
        public override void Execute()
        {
            if (_result.CommandResult.Equals(CommandResult.Proceed))
            {
                if (false == _comm.ReadByteData(ref _receiveMessage))
                    return;

                ParseMessages(_receiveMessage, _doingAction);
            }
        }
        private int CalculateCycle(RfidCommand command)
        {
            switch (command)
            {
                case RfidCommand.READ_LOT_ID:
                case RfidCommand.WRITE_LOT_ID:
                    return (_lotIdLength - 1) / StringLengthAtPage + 1;
                    
                case RfidCommand.READ_CARRIER_ID:
                case RfidCommand.WRITE_CARRIER_ID:
                    return (_carrierIdLength - 1) / StringLengthAtPage + 1;

                default:
                    return 1;
            }
        }
        private string[] SplitStringIntoChunks(int countOfPages, string dataToWrite)
        {
            string[] dataToWriteEachPages = new string[countOfPages];

            string paddedString = dataToWrite.PadRight(countOfPages * StringLengthAtPage, ' ');

            for (int i = 0; i < dataToWriteEachPages.Length; ++i)
            {
                int startIndex = i * StringLengthAtPage;
                int length = Math.Min(StringLengthAtPage, paddedString.Length - startIndex);
                dataToWriteEachPages[i] = paddedString.Substring(startIndex, length);
            }

            return dataToWriteEachPages;
        }
        private CommandResults ExecuteReadingAction(RfidCommand command)
        {
            switch (_actionStep)
            {
                case 0:
                    {
                        _result.CommandResult = CommandResult.Proceed;
                        _totalCycleTime = CalculateCycle(command);
                        _currentCycleTime = 0;
                        _result.Description = string.Empty;
                        _temporaryData = string.Empty;

                        // Transaction 내부 플래그 초기화
                        _transaction.InitFlag();

                        // Operation 상태 초기화
                        _operationStatus = XedionOperationStatus.Waiting;
                        _operationErrorDescription = string.Empty;
                        _operationData = string.Empty;
                        _operationCommand = command;

                        // Pending frame 관련 플래그도 초기화
                        _hasPendingReadDataPage = false;
                        _pendingReadDataPageBytes = null;
                        _writeAckReceived = false;

                        _timeChecker.SetTickCount(5000);
                        ++_actionStep;
                    }
                    break;

                case 1:
                    {
                        if (SendMessageForRead(command, _currentCycleTime))
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
                        _operationStatus = XedionOperationStatus.Timeout;
                        break;
                    }

                    // ParseMessages → ApplyParsedFrame 에서 에러가 올라왔으면 여기서 바로 처리
                    if (_operationStatus == XedionOperationStatus.Error)
                    {
                        _operationStatus = XedionOperationStatus.Error;
                        //_result.CommandResult = CommandResult.Error;
                        _result.Description = _operationErrorDescription;
                        break;
                    }

                    // 한 페이지 분 ReadData frame 이 도착했는지 확인
                    if (_hasPendingReadDataPage && _pendingReadDataPageBytes != null)
                    {
                        string pageData = StringHexToStringAscii(
                            Encoding.ASCII.GetString(_pendingReadDataPageBytes));

                        // 이 페이지는 소비했으니 플래그/버퍼 초기화
                        _hasPendingReadDataPage = false;
                        _pendingReadDataPageBytes = null;

                        // 받은 메시지를 연결한다.
                        _temporaryData = $"{_temporaryData}{pageData}";

                        if (++_currentCycleTime == _totalCycleTime)
                        {
                            //_result.CommandResult = CommandResult.Completed;

                            _operationStatus = XedionOperationStatus.Completed;
                            _operationData = _temporaryData;
                        }
                        else
                        {
                            _doingAction = RfidCommand.IDLE;
                            _timeChecker.SetTickCount(5000);
                            --_actionStep;
                        }
                    }
                    // 아직 frame이 안 들어왔으면 그대로 대기 (Proceed 유지)
                    break;

                default:
                    //_result.CommandResult = CommandResult.Invalid;
                    _operationStatus = XedionOperationStatus.Error;
                    _operationErrorDescription = string.Format("Invalid Seq Num : {0}", _actionStep);
                    break;
            }

            if (!_result.CommandResult.Equals(CommandResult.Proceed))
            {
                _doingAction = RfidCommand.IDLE;
                _actionStep = 0;
            }
            return _result;
        }


        private CommandResults ExecuteWritingAction(RfidCommand command, string dataToWrite)
        {
            switch (_actionStep)
            {
                case 0:
                    {
                        _result.CommandResult = CommandResult.Proceed;
                        _currentCycleTime = 0;
                        _result.Description = string.Empty;
                        _temporaryData = string.Empty;
                        _transaction.InitFlag();

                        _dataToWrite = dataToWrite;
                        _totalCycleTime = CalculateCycle(command);
                        _dataToWriteBySplit = SplitStringIntoChunks(_totalCycleTime, _dataToWrite);

                        _operationStatus = XedionOperationStatus.Waiting;
                        _operationErrorDescription = string.Empty;
                        _operationData = string.Empty;
                        _operationCommand = command;

                        // Pending 플래그 초기화
                        _hasPendingReadDataPage = false;
                        _pendingReadDataPageBytes = null;
                        _writeAckReceived = false;

                        _timeChecker.SetTickCount(5000);
                        ++_actionStep;
                    }
                    break;

                case 1:
                    {
                        if (_dataToWriteBySplit.Length <= _currentCycleTime)
                        {
                            //_result.CommandResult = CommandResult.Error;
                            _result.Description = "Invalid Splitting";

                            _operationStatus = XedionOperationStatus.Error;
                            _operationErrorDescription = _result.Description;
                            break;
                        }

                        string splittedData = _dataToWriteBySplit[_currentCycleTime];
                        if (SendMessageForWrite(command, splittedData, _currentCycleTime))
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
                        _operationStatus = XedionOperationStatus.Timeout;
                        break;
                    }

                    // ParseMessages → ApplyParsedFrame 에서 에러가 올라왔으면 여기서 처리
                    if (_operationStatus == XedionOperationStatus.Error)
                    {
                        _operationStatus = XedionOperationStatus.Error;
                        _result.Description = _operationErrorDescription;
                        break;
                    }

                    // Write ACK frame 이 도착했는지 확인
                    if (!_writeAckReceived)
                        break;

                    // 이 ACK는 소비했으니 다음 루프를 위해 내려준다.
                    _writeAckReceived = false;

                    if (++_currentCycleTime == _totalCycleTime)
                    {
                        //_result.CommandResult = CommandResult.Completed;
                        _operationStatus = XedionOperationStatus.Completed;
                    }
                    else
                    {
                        _doingAction = RfidCommand.IDLE;
                        _timeChecker.SetTickCount(5000);
                        --_actionStep;
                    }
                    break;

                default:
                    _operationStatus = XedionOperationStatus.Error;
                    _operationErrorDescription = string.Format("Invalid Seq Num : {0}", _actionStep);
                    break;
            }

            if (!_result.CommandResult.Equals(CommandResult.Proceed))
            {
                _doingAction = RfidCommand.IDLE;
                _actionStep = 0;
            }
            return _result;
        }


        private void ApplyParsedFrame(XedionParsedFrame frame)
        {
            if (frame == null)
                return;

            // 이미 끝난 상태면 더 건드리지 않는다.
            if (_operationStatus == XedionOperationStatus.Completed ||
                _operationStatus == XedionOperationStatus.Error ||
                _operationStatus == XedionOperationStatus.Timeout)
            {
                return;
            }

            // 현재 진행 중인 명령과 다르면 무시 (다른 타이밍의 패킷 대비)
            if (frame.Command != _operationCommand)
            {
                return;
            }

            switch (frame.Type)
            {
                case XedionFrameType.Error:
                    _operationStatus = XedionOperationStatus.Error;
                    _operationErrorDescription = frame.ErrorMessage;
                    // 읽기/쓰기 관련 pending 플래그는 모두 내려준다.
                    _hasPendingReadDataPage = false;
                    _pendingReadDataPageBytes = null;
                    _writeAckReceived = false;
                    break;

                case XedionFrameType.ReadData:
                    // 한 페이지 분 데이터만 임시 저장, FSM에서 소비
                    _pendingReadDataPageBytes = frame.Data;
                    _hasPendingReadDataPage = true;
                    break;

                case XedionFrameType.WriteAck:
                    _writeAckReceived = true;
                    break;

                case XedionFrameType.None:
                default:
                    // 아무 변화 없음
                    break;
            }
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

                return true;
            }
            return false;
        }
        #endregion </Execute>

        #endregion </Methods>
    }

    #region <Transaction Class>
    public class XedionTransaction
    {
        #region <Constructors>
        public XedionTransaction(int commIndex)
        {
        }
        #endregion </Constructors>

		#region <Fields>

        #region <Const>
        private const char StartSign = (char)83;
        private const char EndSign = (char)13;
        private const char ReaderID = '0';
        private const string Page98 = "98";     // ID 비트 0에서 3까지의 빈 표지나 종료 기호까지 모든 페이지 읽기
        private const string Page99 = "99";     // 전체 태그 읽기

        #region <Command to Reader>
        private const string ReadData = "X";
        private const string WriteData = "W";
        private const string RequestParameter = "G";
        private const string SetParameter = "P";
        private const string HeartBeat = "H";
        private const string Reset = "N";
        private const string TransponderMode = "M";
        private const string LockPage = "L";
        private const string CheckVersionAndSerialNumber = "I";
        private const string WriteSerialNumber = "S";
        private const string CheckVersion = "V";
        #endregion </Command To Reader>

        #region <Command from Reader>
        private const char ReadDataAck = 'x';
        private const char WriteDataAck = 'w';
        private const char SetParameterAck = 'p';
        private const char RequestParameterAck = 'g';
        private const char HeartbeatAck = 'h';
        private const char ResetAck = 'n';
        private const char ErrorMessage = 'e';
        private const char TransponderModeAck = 'm';
        private const char LockPageAck = 'l';
        private const char CheckVersionAndSerialNumberAck = 'i';
        private const char CheckVersionAck = 'v';
        #endregion </Command from Reader>

        #endregion </Const>

        #endregion </Fields>

        #region <Enumerations>
        // 번역기 돌려서 해석이 힘들수 있음..
        public enum CommandToReader
        {
            X,  // 외부에서 트리거된 읽기 시작
            W,  // TAG에 쓰기
            G,  // 매개변수 요청
            P,  // 매개변수 요청
            H,  // HEART BEAT 시작
            N,  // 소프트웨어 리셋 시작
            M,  // TRANSPONDER MODE 설정/읽기 (단일/다중)
            L,  // 한 페이지 잠금
            I,  // 버전 및 시리얼 번호 조회
            S,  // 시리얼 번호 쓰기(비밀번호 필요)
            V,  // 버전 조회
        }

        public enum CommandFromReader
        {
            x,  // 페이지에서 데이터(외부에서 트리거 된 읽기)
            w,  // TAG에 쓰기 후 응답
            p,  // 매개변수 설정 중 응답
            g,  // 매개변수 읽기 요청에 대한 응답
            h,  // HEART BEAT 후 응답
            n,  // 소프트웨어 또는하드웨어 리셋 후 응답
            e,  // 실패 메시지
            m,  // TRANSPONDER MODE 설정 후 응답/확인
            l,  // 한 페이지 잠금 후 피드백
            i,  // 버전 및 시리얼 번호 조회에 대한 응답
            v,  // 버전 조회에 대한 응답
        }
		public enum CategoriesOfData
        {
            Header,
            Data,
            EndOfPackage,
        }
        #endregion </Enumerations>

        #region <Fields>
        private int ReceiveDataCount;
        private string ReceivePage;
        #endregion </Fields>

        #region <Properties>
        public string PageToHandling { get; set; }
        public string DataToWrite { get; set; }
        public byte[] ByteData { get; private set; }
        #endregion </Properties>

        #region <Methods>
        public void InitFlag()
        {
            ByteData = null;
        }
        public byte[] ReadDataAtPage(RfidCommand command, string pageToRead)
        {
            return CreateCommand(ReaderID, command, pageToRead);
        }
        public byte[] WriteDataToPage(RfidCommand command, string dataToWrite, string pageToWrite)
        {
            string[] convertArray = new string[9];
            convertArray[0] = pageToWrite;

            for (int i = 1; i < convertArray.Length ; i++)
            {
                if (dataToWrite.Length >= i)
                {
                    convertArray[i] = ((int)dataToWrite[i - 1]).ToString("X2");
                }
                else
                {
                    convertArray[i] = " ";
                }
            }
            return CreateCommand(ReaderID, command, convertArray);
        }

        public byte[] CreateCommand(char readerId, RfidCommand foupCommand, params string[] parameters)
        {
            string command = string.Empty;
			switch (foupCommand)
            {
                case RfidCommand.READ_LOT_ID:
                case RfidCommand.READ_CARRIER_ID:
                    command = ReadData.ToString();
                    break;
                case RfidCommand.WRITE_LOT_ID:
                case RfidCommand.WRITE_CARRIER_ID:
                    command = WriteData.ToString();
                    break;
                default:
                    break;
            }

            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.Append(command);
            stringBuilder.Append(readerId);

            foreach (string parameter in parameters)
            {
                stringBuilder.Append(parameter);
            }

            string message = stringBuilder.ToString();
            string header = string.Empty, endPackage = string.Empty;
            MakePackageHeader(ref header, message.Length);
            MakeEndingOfPackage(ref endPackage, header, message);
			string strMessage = $"{header}{message}{endPackage}";
            byte[] sendMessage = Encoding.ASCII.GetBytes(strMessage);

            return sendMessage;
        }
        private bool MakePackageHeader(ref string packageHeader, int length)
        {
            try
            {
                string strHexHeader = string.Format("{0:X2}", length);
			    char[] charMsgLength = new char[2];
                charMsgLength = strHexHeader.ToCharArray();

                packageHeader = StartSign.ToString();
                foreach (char c in charMsgLength)
                {
                    packageHeader += c.ToString();
                }
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
        private bool MakeEndingOfPackage(ref string endOfPackage, string header, string message)
        {
            try
            {
                byte[] inputBytes = Encoding.UTF8.GetBytes(header + message + EndSign);

                int tempXORCheckSum = 0;
                int tempXORAdditionCheckSum = 0;
                bool first = true;

                foreach (byte b in inputBytes)
                {
                    int value = b;

                    if (first && b == StartSign)
                    {
                        tempXORCheckSum = value;
                        tempXORAdditionCheckSum = value;
                        first = false;
                    }
                    else
                    {
                        tempXORCheckSum ^= value;
                        tempXORAdditionCheckSum += value;
                    }
                }

                string xorString = string.Format("{0:X2}", tempXORCheckSum);
                char[] arrChecksumXOR = xorString.ToCharArray();

                string addString = string.Format("{0:X2}", tempXORAdditionCheckSum);
                if (addString.Length > 2)
                {
                    addString = addString.Substring(addString.Length - 2, 2);
                }
                else if (addString.Length < 2)
                {
                    addString = addString.PadLeft(2, '0');
                }

                char[] arrChecksumAddition = addString.ToCharArray();

                endOfPackage = EndSign.ToString();
                foreach (char c in arrChecksumXOR)
                {
                    endOfPackage += c.ToString();
                }
                foreach (char c in arrChecksumAddition)
                {
                    endOfPackage += c.ToString();
                }

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
        public XedionParsedFrame ParseMessages(byte[] receivedMessage, RfidCommand command)
        {
            // 기본값: 아무 의미 없는 프레임
            var frame = new XedionParsedFrame
            {
                Type = XedionFrameType.None,
                Command = command,
                Data = null,
                Page = null,
                ErrorCode = '\0',
                ErrorMessage = string.Empty
            };

            if (receivedMessage == null || command == RfidCommand.IDLE)
            {
                return frame;
            }

            // 기존 MatchReceiveMessage를 확장해서 frame을 채우도록 변경
            MatchReceiveMessage(receivedMessage, command, frame);

            return frame;
        }

        private bool MatchReceiveMessage(byte[] receivedMessage, RfidCommand receiveCommand, XedionParsedFrame frame)
        {
            byte[] compareHeader = SearchForData(receivedMessage, receiveCommand, CategoriesOfData.Header);
            byte[] compareData = SearchForData(receivedMessage, receiveCommand, CategoriesOfData.Data);
            byte[] compareEndOfPackage = SearchForData(receivedMessage, receiveCommand, CategoriesOfData.EndOfPackage);
            List<byte> byteResult = new List<byte>();

            // 에러 메시지 처리
            if (compareData.Length > 0 && compareData[0] == ErrorMessage)
            {
                // 새 구조: frame에도 에러 정보 기록
                frame.Type = XedionFrameType.Error;
                frame.ErrorCode = (char)compareData[2];
                frame.ErrorMessage = GetErrorMessage((char)compareData[2]);

                return false; // 읽기/쓰기 성공은 아님
            }

            if (MatchReceiveData(compareData, receivedMessage, receiveCommand) == false)
            {
                return false;
            }
            if (MatchReceiveHeader(compareHeader, compareData.Length) == false)
            {
                return false;
            }

            if (MatchReceiveEndOfPackage(compareEndOfPackage))
            {
                switch (receiveCommand)
                {
                    case RfidCommand.READ_LOT_ID:
                    case RfidCommand.READ_CARRIER_ID:
                        // 기존 동작 유지: ByteData 채우기
                        for (int i = 4; i < compareData.Length; i++)
                        {
                            byteResult.Add(compareData[i]);
                        }
                        ByteData = byteResult.ToArray();

                        // 새 구조: frame에도 데이터와 페이지 기록
                        frame.Type = XedionFrameType.ReadData;
                        frame.Data = ByteData;
                        frame.Page = ReceivePage; // SearchForData에서 이미 세팅됨
                        break;

                    case RfidCommand.WRITE_LOT_ID:
                    case RfidCommand.WRITE_CARRIER_ID:
                        // 쓰기는 데이터가 따로 없고, ACK만 확인
                        frame.Type = XedionFrameType.WriteAck;
                        break;

                    default:
                        break;
                }
                return true;
            }
            return false;
        }

        private bool MatchReceiveData(byte[] receiveData, byte[] receivedMessage, RfidCommand command)
        {
            byte[] checkMatchingCommand = new byte[1];
            byte[] checkMatchingPage;
            string checkCommand = string.Empty;

            switch (command)
            {
                case RfidCommand.READ_LOT_ID:
                    checkCommand = ReadDataAck.ToString();
                    break;
                case RfidCommand.READ_CARRIER_ID:
                    checkCommand = ReadDataAck.ToString();
                    break;
                case RfidCommand.WRITE_LOT_ID:
                case RfidCommand.WRITE_CARRIER_ID:
                    checkCommand = WriteDataAck.ToString();
                    break;
                default:
                    break;
            }

            checkMatchingCommand = Encoding.ASCII.GetBytes(checkCommand);
            checkMatchingPage = Encoding.ASCII.GetBytes(PageToHandling);
            // Message 4번째 문자부터 Data 시작
            if (receivedMessage[3] == checkMatchingCommand[0])
            {
                if (checkCommand == ReadDataAck.ToString())
                {
                    if (ReceivePage == PageToHandling)
                    {
                        return true;
                    }
                    return false;
                }
                if (checkCommand == WriteDataAck.ToString())
                {
                    return true;
                }
                return false;
            }
            else
            {
                return false;
            }
        }
        private bool MatchReceiveHeader(byte[] packageHeader, int length)
        {
//             string stringLength = ((char)packageHeader[2]).ToString();
//             int checkLength = Convert.ToInt32(stringLength,16);

            if (packageHeader[0] == StartSign)
            {
                if (ReceiveDataCount == length)
                {
                    return true;
                }
            }
            return false;
        }
        private bool MatchReceiveEndOfPackage(byte[] inputBytes)
        {
            if (inputBytes[0] == EndSign)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        private byte[] SearchForData(byte[] receivedMessage, RfidCommand command, CategoriesOfData category)
        {
            //byte[] foundData;
            List<byte> dynamicByteArray = new List<byte>();
            byte[] result;
            string receiveDataCount = ((char)receivedMessage[1]).ToString() + ((char)receivedMessage[2]).ToString();
            ReceiveDataCount = Convert.ToInt32(receiveDataCount, 16);
            switch (category)
            {
                case CategoriesOfData.Header:
                    for (int i = 0; i < 3; i++)
                    {
                        dynamicByteArray.Add(receivedMessage[i]);
                    }
                    result = new byte[dynamicByteArray.ToArray().Length];
                    result = dynamicByteArray.ToArray();

                    dynamicByteArray.Clear();
                    return result;
                case CategoriesOfData.Data:
                    for (int i = 3; i < 3 + ReceiveDataCount; i++)
                    {
                        dynamicByteArray.Add(receivedMessage[i]);
                    }
                    result = new byte[dynamicByteArray.ToArray().Length];
                    result = dynamicByteArray.ToArray();
                    if (command == RfidCommand.READ_LOT_ID || command == RfidCommand.READ_CARRIER_ID)
                    {
                        if (result[0] != ErrorMessage && result.Length > 3)
                        {
                            ReceivePage = ((char)result[2]).ToString() + ((char)result[3]).ToString();
                        }
                    }
                    dynamicByteArray.Clear();
                    return result;
                case CategoriesOfData.EndOfPackage:
                    for (int i = 3 + ReceiveDataCount; i < receivedMessage.Length; i++)
                    {
                        dynamicByteArray.Add(receivedMessage[i]);
                    }
                    result = new byte[dynamicByteArray.ToArray().Length];
                    result = dynamicByteArray.ToArray();

                    dynamicByteArray.Clear();
                    return result;
                default:
                    return null;
            }
        }
        public string GetErrorMessage(char errorCode)
        {
            string msg = "";
            switch (errorCode)
            {
                case '0':
                    msg = "none";    // 오류 없음
                    break;
                case '1':
                    msg = "auto fail";    // 자동 읽기가 불가능합니다.
                    break;
                case '2':
                    msg = "ex fail";    // 외부 트리거된 읽기가 불가능합니다.
                    break;
                case '3':
                    msg = "write fail";    // 태그로 데이터 전송이 불가능합니다.
                    break;
                case '4':
                    msg = "no tag";    // 태그가 없거나 안테나가 설치되어 있지 않습니다.
                    break;
                case '5':
                    msg = "invalid";    // 잘못된 매개 변수 또는 명령입니다.
                    break;
                case '6':
                    msg = "unknown";    // 알 수 없는 오류
                    break;
                case '7':
                    msg = "unconfig";    // 장치가 구성되지 않았습니다.
                    break;
                case '8':
                    msg = "check";    // 패리티 또는 체크섬 오류를 확인하세요.
                    break;
                case '9':
                    msg = "void ackn";    // 유효한 확인이 없습니다.
                    break;
                case 'A':								// ERROR MESSAGE 아래 3개중 하나
                    msg = "locked, msg len, invalid";    // 잠긴 페이지에는 쓸 수 없습니다.
                    break;								// 메시지가 너무 깁니다. , 잘못된 매개 변수 또는 명령입니다.
                case 'B':
                    msg = "no ackn";    // 확인해야 할 메시지가 이미 전송되었지만 지정된 시간 내에 터미널에서 확인되지 않았습니다.
                    break;
            }
            return (msg);
        }
        #endregion </Methods>
    }
    #endregion </Transaction Class>

}
