using System.Collections.Generic;

using FrameOfSystem3.SECSGEM.DefineSecsGem;

namespace FrameOfSystem3.SECSGEM.Scenario
{
    public sealed class AutoScenarioRuntimeContext
    {
        public AutoScenarioRuntimeContext(EN_SCENARIO scenario)
        {
            Scenario = scenario;
            CurrentState = IdleAutoScenarioLifecycleState.Instance;
            LastTerminalResult = EN_SCENARIO_RESULT.ERROR;
            LatestCompletedResultData = new Dictionary<string, string>();
        }

        public EN_SCENARIO Scenario { get; }
        public object SyncRoot { get; } = new object();

        public IAutoScenarioLifecycleState CurrentState { get; private set; }

        public int WaitingCount { get; set; }
        public bool IsRunning { get; set; }
        public bool HasEverRequested { get; set; }
        public bool HasPendingTerminalResult { get; set; }

        public EN_SCENARIO_RESULT LastTerminalResult { get; set; }
        public Dictionary<string, string> LatestCompletedResultData { get; set; }

        public void ChangeState(IAutoScenarioLifecycleState nextState)
        {
            CurrentState = nextState ?? IdleAutoScenarioLifecycleState.Instance;
        }

        public void MarkEnqueued()
        {
            HasEverRequested = true;
            CurrentState.OnEnqueue(this);
        }

        public void MarkPrepareSucceeded()
        {
            CurrentState.OnPrepareSucceeded(this);
        }

        public void MarkPrepareFailed()
        {
            CurrentState.OnPrepareFailed(this);
        }

        public void MarkTerminal(EN_SCENARIO_RESULT result, Dictionary<string, string> resultData)
        {
            CurrentState.OnTerminal(this, result, resultData);
        }

        public EN_SCENARIO_RESULT GetVisibleResult()
        {
            if (false == HasEverRequested)
                return EN_SCENARIO_RESULT.ERROR;

            if (HasPendingTerminalResult)
            {
                HasPendingTerminalResult = false;
                return LastTerminalResult;
            }

            return CurrentState.GetVisibleResult(this);
        }
    }
}