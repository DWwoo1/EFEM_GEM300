using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using FrameOfSystem3.SECSGEM.DefineSecsGem;

namespace FrameOfSystem3.SECSGEM.Scenario.Common.Auto
{
    public class SendingEventParamValues : ScenarioParamValues
    {
        public SendingEventParamValues(List<string> values) : base(values)
        {
        }
    }

    public class SendingEventScenario : AutoScenarioBase
    {
        #region <Constructor>
        public SendingEventScenario(string name, long eventId,
            List<long> variableIds = null,
            bool usePermission = false,
            uint timeOut = 10000,
            bool useCheckSecondaryAck = true)
            : base(name, timeOut)
        {
            _eventId = eventId;
            _usePermission = usePermission;
            if (variableIds != null)
            {
                _statusVariableIds = new long[variableIds.Count];

                for (int i = 0; i < variableIds.Count; ++i)
                {
                    _statusVariableIds[i] = variableIds[i];
                }
            }

            UseCheckSecondaryAck = useCheckSecondaryAck;
        }
        #endregion </Constructor>

        #region <Fields>
        private long _eventId;
        private long[] _statusVariableIds = null;
        private bool _usePermission = false;
        SendingEventParamValues _paramValues = null;
        private readonly bool UseCheckSecondaryAck;
        private readonly TickCounter_.TickCounter TicksForDelay = new TickCounter_.TickCounter();
        #endregion </Fields>

        #region <Properties>
        public long EventId
        {
            get
            {
                return _eventId;        
            }
        }
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
            _paramValues = paramValues as SendingEventParamValues;
        }

        public override EN_SCENARIO_RESULT ExecuteScenario()
        {
            switch (_seqNum)
            {
                case (int)EN_SCENARIO_SEQ.INIT:
                    _seqNum = (int)EN_SCENARIO_SEQ.SEND_MESSAGE;
                    break;

                case (int)EN_SCENARIO_SEQ.SEND_MESSAGE:
                    {
                        _gemHandler.SendEvent(_eventId, _statusVariableIds, _paramValues.VariableValues, UseCheckSecondaryAck);
                        if (UseCheckSecondaryAck)
                        {
                            SetTickCount(TimeOut);
                        }
                        else
                        {
                            TicksForDelay.SetTickCount(200);
                        }
                        ++_seqNum;
                    }
                    break;

                case (int)EN_SCENARIO_SEQ.SEND_MESSAGE + 1:
                    if (UseCheckSecondaryAck)
                    {
                        if (IsTickOver(true))
                        {
                            return ReturnScenarioResult(EN_SCENARIO_RESULT.TIMEOUT_ERROR);
                        }

                        if (false == _gemHandler.IsSendingEventCompleted(_eventId))
                            break;
                    }
                    else
                    {
                        if (false == TicksForDelay.IsTickOver(false))
                            break;
                    }

                    if (false == _usePermission)
                    {
                        return ReturnScenarioResult(EN_SCENARIO_RESULT.COMPLETED);
                    }
                    else
                    {
                        SetTickCount(TimeOut);
                        _seqNum = (int)EN_SCENARIO_SEQ.WAIT_FOR_PERMISSION;
                    }
                    break;

                case (int)EN_SCENARIO_SEQ.WAIT_FOR_PERMISSION:
                    if (IsTickOver(true))
                    {
                        return ReturnScenarioResult(EN_SCENARIO_RESULT.TIMEOUT_ERROR);
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
