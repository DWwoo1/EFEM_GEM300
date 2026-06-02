using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using FrameOfSystem3.SECSGEM.DefineSecsGem;
using Define.DefineEnumBase.Log;

namespace FrameOfSystem3.SECSGEM.Scenario.Common
{
    public class AlarmStateChangedParam : ScenarioParamValues
    {
		public AlarmStateChangedParam(int alarmCode, EN_ALM_TYPE type)
        {
			_alarmCode = alarmCode;
			_type = type;
        }

		public int AlarmCode { get { return _alarmCode; } }
		public EN_ALM_TYPE AlarmType { get { return _type; } }

		protected int _alarmCode = -1;
		protected EN_ALM_TYPE _type = EN_ALM_TYPE.OCCURRED;
    }

    public class AlarmStateChangedScenario : ScenarioBaseClass
    {
        #region <Constructor>
		public AlarmStateChangedScenario()
            : base("AlarmStateChanged", 5000)
        {
        }
        #endregion </Constructor>

        #region <Fields>
		AlarmStateChangedParam _paramValues = null;
        #endregion </Fields>

        #region <Type>
        private enum EN_SCENARIO_SEQ
        {
            Init = 0,
			ReportAlarmStateChange = 100,
        }
        #endregion </Type>

        #region <Methods>
        public override void UpdateParamValues(ScenarioParamValues paramValues)
        {
			_paramValues = paramValues as AlarmStateChangedParam;
        }

        public override EN_SCENARIO_RESULT ExecuteScenario()
        {
            switch (_seqNum)
            {
                case (int)EN_SCENARIO_SEQ.Init:
                    Activate = true;
                    InitFlags();
					_seqNum = (int)EN_SCENARIO_SEQ.ReportAlarmStateChange;
                    break;

				case (int)EN_SCENARIO_SEQ.ReportAlarmStateChange:
                    SetTickCount(TimeOut);
					switch(_paramValues.AlarmType)
					{
						case EN_ALM_TYPE.OCCURRED:
							_gemHandler.SetAlarm(_paramValues.AlarmCode);
							break;
						case EN_ALM_TYPE.RELEASED:
							_gemHandler.ClearAlarm(_paramValues.AlarmCode);
							break;
						default:
							return ReturnScenarioResult(EN_SCENARIO_RESULT.ERROR);
					}
					return ReturnScenarioResult(EN_SCENARIO_RESULT.COMPLETED);

                default:
					return ReturnScenarioResult(EN_SCENARIO_RESULT.ERROR);
			}

            return EN_SCENARIO_RESULT.PROCEED;
        }
        #endregion </Methods>
    }
}
