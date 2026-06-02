using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using FrameOfSystem3.SECSGEM.DefineSecsGem;
using FrameOfSystem3.SECSGEM.Scenario;

namespace EFEM.CustomizedByProcessType.PWA500Common
{
    public class ScenarioReqBinMapUploadGEM300ParamValues : ScenarioParamValues
    {
        public ScenarioReqBinMapUploadGEM300ParamValues(List<string> values, bool useEventHandling, string dataPathToUpload) : base(values)
        {
            UseEventHandling = useEventHandling;
            DataPathToUpload = dataPathToUpload;
        }
        public readonly bool UseEventHandling;
        public readonly string DataPathToUpload;
    }
    class ScenarioReqBinMapUploadGEM300 : AutoScenarioBase
    {
        #region <Constructor>
        public ScenarioReqBinMapUploadGEM300(string name, long eventId, List<long> variables,
            bool usePermission = false,
            uint timeOut = 10000,
            bool useCheckSecondaryAck = true)
            : base(name, timeOut)
        {
            MessageFormatToSend = new List<SemiObject>();

            _eventId = eventId;
            _variables = variables;
        }
        #endregion </Constructor>

        #region <Fields>
        ScenarioReqBinMapUploadGEM300ParamValues _paramValue = null;
        private List<SemiObject> MessageFormatToSend = null;

        private long _eventId;
        private List<long> _variables;

        #endregion </Fields>

        #region <Types>
        private enum EN_SCENARIO_SEQ
        {
            INIT = 0,
            SEND_MESSAGE = 100,
            WAIT_FOR_PERMISSION = 200,
            FINISH,
        }
        #endregion </Types>

        #region <Properties>
        public long StreamToSend { get; private set; }
        public long FunctionToSend { get; private set; }
        #endregion </Properties>

        #region <Methods>
        public override EN_SCENARIO_RESULT ExecuteScenario()
        {
            switch (_seqNum)
            {
                case (int)EN_SCENARIO_SEQ.INIT:
                    {
                        Activate = true;
                        InitFlags();
                        _seqNum = (int)EN_SCENARIO_SEQ.SEND_MESSAGE;
                        break;
                    }
                case (int)EN_SCENARIO_SEQ.SEND_MESSAGE:
                    {
                        if (_paramValue == null)
                        {
                            return ReturnScenarioResult(EN_SCENARIO_RESULT.ERROR);
                        }

                        _gemHandler.SendEvent(_eventId, _variables.ToArray(), _paramValue.VariableValues);
                        SetTickCount(TimeOut);
                        ++_seqNum;
                        break;
                    }
                case (int)EN_SCENARIO_SEQ.SEND_MESSAGE + 1:
                    {
                        if (IsTickOver(true))
                        {
                            return ReturnScenarioResult(EN_SCENARIO_RESULT.TIMEOUT_ERROR);
                        }

                        if (false == _gemHandler.IsSendingEventCompleted(_eventId))
                            break;

                        _seqNum = (int)EN_SCENARIO_SEQ.WAIT_FOR_PERMISSION;
                    }
                    break;

                case (int)EN_SCENARIO_SEQ.WAIT_FOR_PERMISSION:
                    {
                        if (IsTickOver(true))
                        {
                            return ReturnScenarioResult(EN_SCENARIO_RESULT.ERROR);
                        }

                        return ReturnScenarioResult(EN_SCENARIO_RESULT.COMPLETED);
                    }

                default:
                    return ReturnScenarioResult(EN_SCENARIO_RESULT.ERROR);
            }

            return EN_SCENARIO_RESULT.PROCEED;
        }

        public override void UpdateParamValues(ScenarioParamValues paramValues)
        {
            _paramValue = paramValues as ScenarioReqBinMapUploadGEM300ParamValues;
        }

        //public bool UpdateReceivedSecsMessage(long function, List<SemiObject> listOfReceive)
        //{
        //    if (listOfReceive.Count != 1)
        //        return false;

        //    if (!(listOfReceive[0] is SemiObjectBinary ack))
        //        return false;

        //    return true;
        //}
        #endregion </Methods>
    }
}
