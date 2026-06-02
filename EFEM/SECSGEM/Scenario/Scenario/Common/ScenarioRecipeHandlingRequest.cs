using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using FrameOfSystem3.SECSGEM.DefineSecsGem;

namespace FrameOfSystem3.SECSGEM.Scenario.Common
{
    public class RecipeHandlingRequestParamValues : ScenarioParamValues
    {
        public RecipeHandlingRequestParamValues(List<string> values, string recipeId, string recipePath) : base(values)
        {
            RecipeId = recipeId;
            RecipePath = recipePath;
        }

        public string RecipeId { get; private set; }
        public string RecipePath { get; private set; }
    }
    
    // Recipe Upload/Download를 위한 이벤트 전송 후 처리를 기다리는 시나리오
    // 이벤트 전송 후 서버에서 Upload(S7F5) or Download(S7F3) 전송하는 시나리오이다.
    public class ScenarioRecipeHandlingRequest : ScenarioBaseClass
    {
        #region <Constructor>
        public ScenarioRecipeHandlingRequest(string name,
            long eventId,
            List<long> variableIds = null,
            uint timeOut = 10000)
            : base(name, timeOut)
        {
            if (variableIds != null)
            {
                _statusVariableIds = new long[variableIds.Count];

                for (int i = 0; i < variableIds.Count; ++i)
                {
                    _statusVariableIds[i] = variableIds[i];
                }
            }

            EventId = eventId;
        }
        #endregion </Constructor>

        #region <Fields>
        private long[] _statusVariableIds = null;
        RecipeHandlingRequestParamValues _paramValues = null;
        #endregion </Fields>

        #region <Properties>
        public long EventId { get; private set; }
        #endregion </Properties>

        #region <Type>
        private enum HandlingTypes
        {
            Upload,
            Download
        }
        private enum EN_SCENARIO_SEQ
        {
            INIT = 0,
            SEND_EVENT = 100,
            WAIT_FOR_HANDLING_COMPLETION = 200,
            FINISH,
        }
        #endregion </Type>

        #region <Methods>
        public override void UpdateParamValues(ScenarioParamValues paramValues)
        {
            _paramValues = paramValues as RecipeHandlingRequestParamValues;
        }

        public override EN_SCENARIO_RESULT ExecuteScenario()
        {
            switch (_seqNum)
            {
                case (int)EN_SCENARIO_SEQ.INIT:
                    Activate = true;
                    InitFlags();
                    if (EventId < 0)
                    {
                        SetTickCount(TimeOut);
                        _seqNum = (int)EN_SCENARIO_SEQ.WAIT_FOR_HANDLING_COMPLETION;
                    }
                    else
                    {
                        _seqNum = (int)EN_SCENARIO_SEQ.SEND_EVENT;
                    }
                    break;

                case (int)EN_SCENARIO_SEQ.SEND_EVENT:
                    if (_paramValues == null)
                        return ReturnScenarioResult(EN_SCENARIO_RESULT.ERROR);

                    _gemHandler.SendEvent(EventId, _statusVariableIds, _paramValues.VariableValues);
                    SetTickCount(TimeOut);
                    ++_seqNum; break;

                case (int)EN_SCENARIO_SEQ.SEND_EVENT + 1:
                    if (IsTickOver(true))
                    {
                        return ReturnScenarioResult(EN_SCENARIO_RESULT.TIMEOUT_ERROR);
                    }

                    if (false == _gemHandler.IsSendingEventCompleted(EventId))
                        break;

                    SetTickCount(TimeOut);
                    _seqNum = (int)EN_SCENARIO_SEQ.WAIT_FOR_HANDLING_COMPLETION;
                    break;

                case (int)EN_SCENARIO_SEQ.WAIT_FOR_HANDLING_COMPLETION:
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
