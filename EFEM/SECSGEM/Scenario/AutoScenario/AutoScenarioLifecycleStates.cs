using System.Collections.Generic;

using FrameOfSystem3.SECSGEM.DefineSecsGem;

namespace FrameOfSystem3.SECSGEM.Scenario
{
    internal abstract class AutoScenarioLifecycleStateBase : IAutoScenarioLifecycleState
    {
        public abstract EN_AUTO_SCENARIO_STATE AutoState { get; }

        public virtual void OnEnqueue(AutoScenarioRuntimeContext context) { }
        public virtual void OnPrepareSucceeded(AutoScenarioRuntimeContext context) { }
        public virtual void OnPrepareFailed(AutoScenarioRuntimeContext context) { }

        public virtual void OnTerminal(
            AutoScenarioRuntimeContext context,
            EN_SCENARIO_RESULT result,
            Dictionary<string, string> resultData)
        {
            context.IsRunning = false;
            context.LastTerminalResult = result;
            context.LatestCompletedResultData = resultData == null
                ? new Dictionary<string, string>()
                : new Dictionary<string, string>(resultData);
            context.HasPendingTerminalResult = true;

            switch (result)
            {
                case EN_SCENARIO_RESULT.COMPLETED:
                    context.ChangeState(CompletedAutoScenarioLifecycleState.Instance);
                    break;

                case EN_SCENARIO_RESULT.TIMEOUT_ERROR:
                    context.ChangeState(TimeoutAutoScenarioLifecycleState.Instance);
                    break;

                default:
                    context.ChangeState(ErrorAutoScenarioLifecycleState.Instance);
                    break;
            }
        }

        public abstract EN_SCENARIO_RESULT GetVisibleResult(AutoScenarioRuntimeContext context);
    }

    internal sealed class IdleAutoScenarioLifecycleState : AutoScenarioLifecycleStateBase
    {
        public static readonly IdleAutoScenarioLifecycleState Instance = new IdleAutoScenarioLifecycleState();

        private IdleAutoScenarioLifecycleState() { }

        public override EN_AUTO_SCENARIO_STATE AutoState
        {
            get { return EN_AUTO_SCENARIO_STATE.IDLE; }
        }

        public override void OnEnqueue(AutoScenarioRuntimeContext context)
        {
            context.WaitingCount++;
            context.ChangeState(WaitingAutoScenarioLifecycleState.Instance);
        }

        public override EN_SCENARIO_RESULT GetVisibleResult(AutoScenarioRuntimeContext context)
        {
            return EN_SCENARIO_RESULT.ERROR;
        }
    }

    internal sealed class WaitingAutoScenarioLifecycleState : AutoScenarioLifecycleStateBase
    {
        public static readonly WaitingAutoScenarioLifecycleState Instance = new WaitingAutoScenarioLifecycleState();

        private WaitingAutoScenarioLifecycleState() { }

        public override EN_AUTO_SCENARIO_STATE AutoState
        {
            get { return EN_AUTO_SCENARIO_STATE.WAITING; }
        }

        public override void OnEnqueue(AutoScenarioRuntimeContext context)
        {
            context.WaitingCount++;
        }

        public override void OnPrepareSucceeded(AutoScenarioRuntimeContext context)
        {
            if (context.WaitingCount > 0)
                context.WaitingCount--;

            context.IsRunning = true;
            context.ChangeState(RunningAutoScenarioLifecycleState.Instance);
        }

        public override void OnPrepareFailed(AutoScenarioRuntimeContext context)
        {
            if (context.WaitingCount > 0)
                context.WaitingCount--;

            context.IsRunning = false;

            if (context.WaitingCount <= 0)
            {
                context.ChangeState(IdleAutoScenarioLifecycleState.Instance);
            }
        }

        public override EN_SCENARIO_RESULT GetVisibleResult(AutoScenarioRuntimeContext context)
        {
            return EN_SCENARIO_RESULT.WAITING;
        }
    }

    internal sealed class RunningAutoScenarioLifecycleState : AutoScenarioLifecycleStateBase
    {
        public static readonly RunningAutoScenarioLifecycleState Instance = new RunningAutoScenarioLifecycleState();

        private RunningAutoScenarioLifecycleState() { }

        public override EN_AUTO_SCENARIO_STATE AutoState
        {
            get { return EN_AUTO_SCENARIO_STATE.RUNNING; }
        }

        public override void OnEnqueue(AutoScenarioRuntimeContext context)
        {
            context.WaitingCount++;
        }

        public override EN_SCENARIO_RESULT GetVisibleResult(AutoScenarioRuntimeContext context)
        {
            return EN_SCENARIO_RESULT.PROCEED;
        }
    }

    internal abstract class TerminalAutoScenarioLifecycleStateBase : AutoScenarioLifecycleStateBase
    {
        public override void OnEnqueue(AutoScenarioRuntimeContext context)
        {
            context.WaitingCount++;
            context.ChangeState(WaitingAutoScenarioLifecycleState.Instance);
        }

        public override void OnPrepareSucceeded(AutoScenarioRuntimeContext context)
        {
            if (context.WaitingCount > 0)
                context.WaitingCount--;

            context.IsRunning = true;
            context.ChangeState(RunningAutoScenarioLifecycleState.Instance);
        }

        public override void OnPrepareFailed(AutoScenarioRuntimeContext context)
        {
            if (context.WaitingCount > 0)
                context.WaitingCount--;

            context.IsRunning = false;

            if (context.WaitingCount <= 0)
            {
                context.ChangeState(IdleAutoScenarioLifecycleState.Instance);
            }
            else
            {
                context.ChangeState(WaitingAutoScenarioLifecycleState.Instance);
            }
        }
    }

    internal sealed class CompletedAutoScenarioLifecycleState : TerminalAutoScenarioLifecycleStateBase
    {
        public static readonly CompletedAutoScenarioLifecycleState Instance = new CompletedAutoScenarioLifecycleState();

        private CompletedAutoScenarioLifecycleState() { }

        public override EN_AUTO_SCENARIO_STATE AutoState
        {
            get { return EN_AUTO_SCENARIO_STATE.COMPLETED; }
        }

        public override EN_SCENARIO_RESULT GetVisibleResult(AutoScenarioRuntimeContext context)
        {
            if (context.WaitingCount > 0)
                return EN_SCENARIO_RESULT.WAITING;

            return EN_SCENARIO_RESULT.COMPLETED;
        }
    }

    internal sealed class ErrorAutoScenarioLifecycleState : TerminalAutoScenarioLifecycleStateBase
    {
        public static readonly ErrorAutoScenarioLifecycleState Instance = new ErrorAutoScenarioLifecycleState();

        private ErrorAutoScenarioLifecycleState() { }

        public override EN_AUTO_SCENARIO_STATE AutoState
        {
            get { return EN_AUTO_SCENARIO_STATE.ERROR; }
        }

        public override EN_SCENARIO_RESULT GetVisibleResult(AutoScenarioRuntimeContext context)
        {
            if (context.WaitingCount > 0)
                return EN_SCENARIO_RESULT.WAITING;

            return EN_SCENARIO_RESULT.ERROR;
        }
    }

    internal sealed class TimeoutAutoScenarioLifecycleState : TerminalAutoScenarioLifecycleStateBase
    {
        public static readonly TimeoutAutoScenarioLifecycleState Instance = new TimeoutAutoScenarioLifecycleState();

        private TimeoutAutoScenarioLifecycleState() { }

        public override EN_AUTO_SCENARIO_STATE AutoState
        {
            get { return EN_AUTO_SCENARIO_STATE.TIMEOUT_ERROR; }
        }

        public override EN_SCENARIO_RESULT GetVisibleResult(AutoScenarioRuntimeContext context)
        {
            if (context.WaitingCount > 0)
                return EN_SCENARIO_RESULT.WAITING;

            return EN_SCENARIO_RESULT.TIMEOUT_ERROR;
        }
    }
}