using System.Collections.Generic;

using FrameOfSystem3.SECSGEM.DefineSecsGem;

namespace FrameOfSystem3.SECSGEM.Scenario
{
    public interface IAutoScenarioLifecycleState
    {
        EN_AUTO_SCENARIO_STATE AutoState { get; }

        void OnEnqueue(AutoScenarioRuntimeContext context);
        void OnPrepareSucceeded(AutoScenarioRuntimeContext context);
        void OnPrepareFailed(AutoScenarioRuntimeContext context);
        void OnTerminal(
            AutoScenarioRuntimeContext context,
            EN_SCENARIO_RESULT result,
            Dictionary<string, string> resultData);

        EN_SCENARIO_RESULT GetVisibleResult(AutoScenarioRuntimeContext context);
    }
}