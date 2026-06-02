using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using FrameOfSystem3.SECSGEM.DefineSecsGem;

namespace FrameOfSystem3.SECSGEM.Scenario.Common
{
    public class ClientToClientCommunicationParamValues : ScenarioParamValues
    {
        #region <Constructors>
        public ClientToClientCommunicationParamValues(string targetDevice,
           string messageName,
           Dictionary<string, string> messages)
            : this(messageName, messages)
        {
            _targetDevice = new string[] { targetDevice };
        }

        public ClientToClientCommunicationParamValues(string[] targetDevices,
             string messageName,
             Dictionary<string, string> messages)
            : this(messageName, messages)
        {
            _targetDevice = new string[targetDevices.Length];
            Array.Copy(targetDevices, _targetDevice, targetDevices.Length);
        }

        private ClientToClientCommunicationParamValues(string messageName, Dictionary<string, string> messages)
        {
            MessageName = messageName;

            Count = messages.Count;

            _contentNames = new string[Count];
            _messages = new string[Count];

            _contentNames = messages.Keys.ToArray();
            _messages = messages.Values.ToArray();
        }
        #endregion </Constructors>

        #region <Types>
        #endregion </Types>

        #region <Fields>
        private string[] _targetDevice = null;
        private string[] _contentNames = null;
        private string[] _messages = null;
        #endregion </Fields>

        #region <Properties>
        public string TargetDevice { get { return _targetDevice[0]; } }

        public int Count { get; private set; }
        public string[] TargetDevices 
        {
            get 
            {
                return _targetDevice;
            }
        }

        public string MessageName { get; private set; }

        public string[] ContentNames { get { return _contentNames; } }
        public string[] Messages { get { return _messages; } }
        #endregion </Properties>

        #region <Methods>
        #endregion </Methods>
    }

    public class ClientToClientCommunicationScenario : ScenarioBaseClass
    {
        #region <Constructor>
        public ClientToClientCommunicationScenario(string name, uint timeOut = 10000, bool isLogging = true)
            : base(name, timeOut)
        {
			_isLogging = isLogging;
        }
        #endregion </Constructor>

        #region <Fields>
        ClientToClientCommunicationParamValues _paramValues = null;
		bool _isLogging;
        #endregion </Fields>

        #region <Properties>
        // 2023.09.04. jhlim [DEL] 추후 정리 혹은 재개발
        //public RemoteCommandInfo RemoteCommandInfoNormal
        //{
        //    get
        //    {
        //        return _remoteCommdInfoNormal;
        //    }
        //}

        //public RemoteCommandInfo RemoteCommandInfoNg
        //{
        //    get
        //    {
        //        return _remoteCommdInfoNg;
        //    }
        //}
        #endregion </Properties>

        #region <Type>
        private enum EN_SCENARIO_SEQ
        {
            INIT = 0,
            SEND_MESSAGE = 100,
            WAIT_FOR_PERMISSION = 200,
            FINISH,
        }
        #endregion </Type>

        #region <Methods>
        public override void UpdateParamValues(ScenarioParamValues paramValues)
        {
            _paramValues = paramValues as ClientToClientCommunicationParamValues;
        }

        public override EN_SCENARIO_RESULT ExecuteScenario()
        {
            switch (_seqNum)
            {
                case (int)EN_SCENARIO_SEQ.INIT:
                    Activate = true;
                    InitFlags();
                    _seqNum = (int)EN_SCENARIO_SEQ.SEND_MESSAGE;
                    break;

                case (int)EN_SCENARIO_SEQ.SEND_MESSAGE:
                    if (_paramValues == null)
                    {
                        return ReturnScenarioResult(EN_SCENARIO_RESULT.ERROR);
                    }

                    int count = _paramValues.TargetDevices.Length;
                    for(int i = 0; i < count; ++i)
                    {
                        _gemHandler.SendClientToClientMessage(_paramValues.TargetDevices[i],
                            _paramValues.MessageName,
                            DefinesForClientToClientMessage.VALUE_MESSAGE_TYPE_SEND,
                            Name,
                            _paramValues.ContentNames,
                            _paramValues.Messages,
                            EN_MESSAGE_RESULT.OK,
							_isLogging);
                    }

                    SetTickCount(TimeOut);
                    _seqNum = (int)EN_SCENARIO_SEQ.WAIT_FOR_PERMISSION;
                    break;

                case (int)EN_SCENARIO_SEQ.WAIT_FOR_PERMISSION:
                    if (IsTickOver(true))
                    {
                        return ReturnScenarioResult(EN_SCENARIO_RESULT.ERROR);
                    }

                    switch (Permission)
                    {
                        case EN_SCENARIO_PERMISSION_RESULT.OK:
                            return ReturnScenarioResult(EN_SCENARIO_RESULT.COMPLETED);
                        case EN_SCENARIO_PERMISSION_RESULT.ERROR:
                            return ReturnScenarioResult(EN_SCENARIO_RESULT.ERROR);
                        default:
                            break;
                    }
                    break;

                default:
                    return ReturnScenarioResult(EN_SCENARIO_RESULT.ERROR);
            }

            return EN_SCENARIO_RESULT.PROCEED;
        }
        #endregion </Methods>
    }
}
