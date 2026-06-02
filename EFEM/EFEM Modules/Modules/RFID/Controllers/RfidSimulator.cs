using System.Text;
using System;
using System.Security.Cryptography;
using System.Collections.Generic;

using Define.DefineEnumBase.Common;
using EFEM.Defines.Common;
using EFEM.Defines.RFID;
using RFIDOnly;

namespace EFEM.Modules.RFID.Controllers
{
    public class RfidSimulator : RFIDReader
    {
        #region <Constructors>
        public RfidSimulator(int portId, EN_CONNECTION_TYPE interfaceType, int commIndex)
            : base(portId, interfaceType, commIndex)
        {
        }
        #endregion </Constructors>

        #region <Fields>
        private string _lotId;
        private string _carrierId;
        #endregion </Fields>

        #region <Properties>

        #endregion </Properties>

        #region <Methods>

        #region <Actions>
        public override void InitAction()
        {
            var access = MaterialTracking.CarrierManagementServer.Instance.GetCarrierAccessingStatus(PortId);
            if (access == Defines.LoadPort.CarrierAccessStates.NotAccessed)
            {
                _lotId = string.Empty;
                _carrierId = string.Empty;
            }

            base.InitAction();
        }
        protected override CommandResults DoReadLotId(ref string lotId)
        {
            return ExecuteCommand(RfidCommand.READ_LOT_ID, ref lotId);
        }
        protected override CommandResults DoReadCarrierId(ref string carrierId)
        {
            return ExecuteCommand(RfidCommand.READ_CARRIER_ID, ref carrierId);
        }
        protected override CommandResults DoWriteLotId(string lotId)
        {
            return ExecuteCommand(RfidCommand.WRITE_LOT_ID, ref lotId);
        }
        protected override CommandResults DoWriteCarrierId(string carrierId)
        {
            return ExecuteCommand(RfidCommand.WRITE_CARRIER_ID, ref carrierId);
        }
        #endregion </Actions>

        #region <Execute>
        public override void Execute()
        {
        }
        #endregion </Execute>

        #endregion </Methods>

        public static class UniqueCodeGenerator
        {
            private const string UpperLetters = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
            private const string Digits = "0123456789";

            private static readonly object _syncRoot = new object();
            private static readonly HashSet<string> _issuedCodes = new HashSet<string>();

            public static string CreateUnique8CharCode()
            {
                lock (_syncRoot)
                {
                    string code;

                    do
                    {
                        code = Create8CharCode();
                    }
                    while (!_issuedCodes.Add(code)); // Add가 false면 이미 존재

                    return code;
                }
            }

            private static string Create8CharCode()
            {
                var sb = new StringBuilder(8);

                for (int i = 0; i < 3; i++)
                {
                    sb.Append(UpperLetters[GetSecureRandomIndex(UpperLetters.Length)]);
                }

                for (int i = 0; i < 5; i++)
                {
                    sb.Append(Digits[GetSecureRandomIndex(Digits.Length)]);
                }

                return sb.ToString();
            }

            private static int GetSecureRandomIndex(int maxExclusive)
            {
                using (var rng = RandomNumberGenerator.Create())
                {
                    var bytes = new byte[4];
                    var limit = int.MaxValue - (int.MaxValue % maxExclusive);

                    while (true)
                    {
                        rng.GetBytes(bytes);
                        int value = BitConverter.ToInt32(bytes, 0) & int.MaxValue;

                        if (value < limit)
                        {
                            return value % maxExclusive;
                        }
                    }
                }
            }
        }

        private CommandResults ExecuteCommand(RfidCommand command, ref string result)
        {
            switch (_actionStep)
            {
                case 0:
                    {
                        _timeChecker.SetTickCount(1000);
                        _result.CommandResult = CommandResult.Proceed;
                        _result.Description = string.Empty;
                        ++_actionStep;
                    }
                    break;
                case 1:
                    {
                        if (false == _timeChecker.IsTickOver(true))
                            break;

                        switch (command)
                        {
                            case RfidCommand.READ_LOT_ID:
                                if (string.IsNullOrEmpty(_lotId))
                                {
                                    // 생성
                                    _lotId = UniqueCodeGenerator.CreateUnique8CharCode();//$"{DateTime.Now.ToString("HHmmss")}.LP{PortId}";
                                }
                                result = _lotId;
                                break;
                            case RfidCommand.READ_CARRIER_ID:
                                if (string.IsNullOrEmpty(_carrierId))
                                {
                                    // 생성
                                    _carrierId = string.Format("CARRIER{0:d2}", PortId);
                                }
                                result = _carrierId;
                                break;
                            case RfidCommand.WRITE_LOT_ID:
                                _lotId = result;
                                break;
                            case RfidCommand.WRITE_CARRIER_ID:
                                _carrierId = result;
                                break;
                        }

                        _result.CommandResult = CommandResult.Completed;
                    }
                    break;
              
            }

            if (false == _result.CommandResult.Equals(CommandResult.Proceed))
            {
                _doingAction = RfidCommand.IDLE;
                _actionStep = 0;
            }

            return _result;
        }
        protected override void ParseMessages(byte[] receivedMessage, RfidCommand command)
        {
        }        
    }
}
