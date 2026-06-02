using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FrameOfSystem3.SECSGEM.DefineSecsGem;

namespace FrameOfSystem3.SECSGEM.Scenario.Common.Auto
{
    // S6F11 이후 SecsMessage 를 받고, RemoteCommand 까지 옵션에 따라 받는 Base Scenario
    public abstract class ScenarioSendEventThenHandlingSecsMessage : AutoScenarioBase
    {
        #region <Constructors>
        public ScenarioSendEventThenHandlingSecsMessage(string name, long eventId, List<long> variables,
            long streamToReceive, long funcToReceive, bool useRemoteCommandConfirmation, uint timeOut)
            : base(name, timeOut)
        {
            EventId = eventId;
            Variables = new List<long>(variables);

            ReceiveStream = streamToReceive;
            ReceiveFunction = funcToReceive;

            SendStream = streamToReceive;
            SendFunction = funcToReceive + 1;

            NameToReceive = name;
            UseRemoteCommandConfirmation = useRemoteCommandConfirmation;
        }
        #endregion </Constructors>

        #region <Fields>
        protected readonly string NameToReceive = string.Empty;

        protected List<SemiObject> _receiveMessageFormat = new List<SemiObject>();

        protected ScenarioParamValues _paramValue = null;

        protected readonly List<long> Variables;
        #endregion </Fields>

        #region <Properties>
        public long EventId { get; protected set; }
        public long SendStream { get; protected set; }
        public long SendFunction { get; protected set; }

        public List<SemiObject> MessageFormatToSend { get; protected set; }
        public bool UseRemoteCommandConfirmation { get; protected set; }
        #endregion </Properties>

        #region <Methods>
        public override EN_SCENARIO_RESULT ExecuteScenario()
        {
            switch (_seqNum)
            {
                case (int)EN_SCENARIO_SEQ.INIT:
                    {
                        Receiving = true;
                        _seqNum = (int)EN_SCENARIO_SEQ.SEND_EVENT;
                    }
                    break;

                case (int)EN_SCENARIO_SEQ.SEND_EVENT:
                    {
                        if (_paramValue == null)
                        {
                            return ReturnScenarioResult(EN_SCENARIO_RESULT.ERROR);
                        }
                        _gemHandler.SendEvent(EventId, Variables.ToArray(), _paramValue.VariableValues);
                        SetTickCount(TimeOut);
                        ++_seqNum;
                    }
                    break;

                case (int)EN_SCENARIO_SEQ.SEND_EVENT + 1:
                    {
                        if (IsTickOver(true))
                        {
                            return ReturnScenarioResult(EN_SCENARIO_RESULT.TIMEOUT_ERROR);
                        }

                        if (false == _gemHandler.IsSendingEventCompleted(EventId))
                            break;

                        switch (Permission)     // Receive SecsMessage에 대한 Permission 이다.
                        {
                            case EN_SCENARIO_PERMISSION_RESULT.OK:

                                ++_seqNum;
                                break;
                            case EN_SCENARIO_PERMISSION_RESULT.ERROR:
                                return ReturnScenarioResult(EN_SCENARIO_RESULT.ERROR);

                            default:
                                break;
                        }
                    }
                    break;

                case (int)EN_SCENARIO_SEQ.SEND_EVENT + 2:
                    {
                        if (false == UseRemoteCommandConfirmation)
                        {
                            return ReturnScenarioResult(EN_SCENARIO_RESULT.COMPLETED);
                        }
                        else
                        {
                            SetTickCount(TimeOut);
                            _seqNum = (int)EN_SCENARIO_SEQ.WAIT_FOR_PERMISSION;
                        }
                    }
                    break;

                case (int)EN_SCENARIO_SEQ.WAIT_FOR_PERMISSION:
                    if (IsTickOver(true))
                    {
                        return ReturnScenarioResult(EN_SCENARIO_RESULT.TIMEOUT_ERROR);
                    }

                    switch (Permission)         // RemoteCommande에 대한 Permission 이다. 
                    {
                        case EN_SCENARIO_PERMISSION_RESULT.OK:
                            return ReturnScenarioResult(EN_SCENARIO_RESULT.COMPLETED);
                        case EN_SCENARIO_PERMISSION_RESULT.ERROR:
                            return ReturnScenarioResult(EN_SCENARIO_RESULT.ERROR);
                    }
                    break;

                default:
                    return EN_SCENARIO_RESULT.ERROR;
            }

            return EN_SCENARIO_RESULT.PROCEED;
        }

        public sealed override bool UpdateReceiveMessage(List<SemiObject> listOfReceive)
        {
            if (UpdateReceivedSecsMessage(listOfReceive))
            {
                if (MakeMessageToSend())
                {
                    Permission = EN_SCENARIO_PERMISSION_RESULT.OK;
                }
                else
                {
                    Permission = EN_SCENARIO_PERMISSION_RESULT.ERROR;
                }
                return true;
            }
            return false;
        }
        
        protected abstract bool UpdateReceivedSecsMessage(List<SemiObject> listOfReceive);
        protected abstract bool MakeMessageToSend();
        #endregion </Methods>
    }
}
