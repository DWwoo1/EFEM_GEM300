using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using FrameOfSystem3.SECSGEM.DefineSecsGem;

namespace FrameOfSystem3.SECSGEM.Scenario.Common
{
    public class UpdateVariablesParam : ScenarioParamValues
    {
        public UpdateVariablesParam(List<string> values) : base(values)
        {
        }
    }

    public class UpdateVariablesScenario : ScenarioBaseClass
    {
        #region <Constructor>
		public UpdateVariablesScenario(List<long> variableIds = null)
            : base("UpdateVariables", 5000)
        {
            if (variableIds != null)
            {
                _statusVariableIds = new long[variableIds.Count];

                for (int i = 0; i < variableIds.Count; ++i)
                {
                    _statusVariableIds[i] = variableIds[i];
                }
            }
        }
        #endregion </Constructor>

        #region <Fields>
        private long[] _statusVariableIds = null;

		UpdateVariablesParam _paramValues = null;
        #endregion </Fields>

        #region <Type>
        private enum EN_SCENARIO_SEQ
        {
            Init = 0,
			UpdateVariable = 100,
        }
        #endregion </Type>

        #region <Methods>
        public override void UpdateParamValues(ScenarioParamValues paramValues)
        {
			_paramValues = paramValues as UpdateVariablesParam;
        }

        public override EN_SCENARIO_RESULT ExecuteScenario()
        {
            switch (_seqNum)
            {
                case (int)EN_SCENARIO_SEQ.Init:
                    Activate = true;
                    InitFlags();
					_seqNum = (int)EN_SCENARIO_SEQ.UpdateVariable;
                    break;

				case (int)EN_SCENARIO_SEQ.UpdateVariable:
					_gemHandler.UpdateVariables(_statusVariableIds, _paramValues.VariableValues.ToArray());
                    SetTickCount(TimeOut);
					return ReturnScenarioResult(EN_SCENARIO_RESULT.COMPLETED);

                default:
                    return ReturnScenarioResult(EN_SCENARIO_RESULT.ERROR);
            }

            return EN_SCENARIO_RESULT.PROCEED;
        }
        #endregion </Methods>
    }
}
