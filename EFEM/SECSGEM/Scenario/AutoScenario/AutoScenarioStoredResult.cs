using System.Collections.Generic;
using FrameOfSystem3.SECSGEM.DefineSecsGem;

namespace FrameOfSystem3.SECSGEM.Scenario
{
    public sealed class AutoScenarioStoredResult
    {
        public string Sender { get; set; }
        public EN_SCENARIO Scenario { get; set; }
        public Dictionary<string, string> ScenarioParams { get; set; }
        public Dictionary<string, string> ResultData { get; set; }
        public EN_SCENARIO_RESULT Result { get; set; }

        public AutoScenarioStoredResult Clone()
        {
            return new AutoScenarioStoredResult
            {
                Sender = Sender,
                Scenario = Scenario,
                ScenarioParams = ScenarioParams == null
                    ? new Dictionary<string, string>()
                    : new Dictionary<string, string>(ScenarioParams),
                ResultData = ResultData == null
                    ? new Dictionary<string, string>()
                    : new Dictionary<string, string>(ResultData),
                Result = Result
            };
        }
    }
}