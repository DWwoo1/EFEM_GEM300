using System;
using System.Collections.Concurrent;
using System.Collections.Generic;

using FrameOfSystem3.SECSGEM.DefineSecsGem;
using FrameOfSystem3.SECSGEM.Scenario;

namespace FrameOfSystem3.SECSGEM
{
    public sealed class AutoScenarioRuntimeManager
    {
        #region <Fields>
        private readonly ProcessingScenario _host;

        private readonly ConcurrentDictionary<EN_SCENARIO, AutoScenarioRuntimeContext> _autoScenarioContexts
            = new ConcurrentDictionary<EN_SCENARIO, AutoScenarioRuntimeContext>();
        private readonly ConcurrentQueue<AutoScenarioRequest> _autoScenarioQueue
            = new ConcurrentQueue<AutoScenarioRequest>();

        private readonly ConcurrentDictionary<SenderScenarioKey, AutoScenarioStoredResult> _pendingAutoScenarioResults
            = new ConcurrentDictionary<SenderScenarioKey, AutoScenarioStoredResult>();
        private readonly ConcurrentDictionary<SenderScenarioKey, AutoScenarioStoredResult> _lastAutoScenarioResults
            = new ConcurrentDictionary<SenderScenarioKey, AutoScenarioStoredResult>();

        private readonly ConcurrentDictionary<SenderScenarioKey, EN_SCENARIO_RESULT> _pendingAutoScenarioExecutionStates
            = new ConcurrentDictionary<SenderScenarioKey, EN_SCENARIO_RESULT>();
        private readonly ConcurrentDictionary<SenderScenarioKey, EN_SCENARIO_RESULT> _lastAutoScenarioExecutionStates
            = new ConcurrentDictionary<SenderScenarioKey, EN_SCENARIO_RESULT>();

        private AutoScenarioRequest _activeAutoScenarioRequest = null;
        #endregion </Fields>

        #region <Constructor>
        public AutoScenarioRuntimeManager(ProcessingScenario host)
        {
            _host = host ?? throw new ArgumentNullException(nameof(host));
        }
        #endregion </Constructor>

        #region <Public Methods>
        public bool IsAutoScenario(EN_SCENARIO scenario)
        {
            return _host.GetInstanceScenario(scenario) is AutoScenarioBase;
        }

        public bool EnqueueAutoScenarioByUpdate(
            string sender,
            bool useLogging,
            EN_SCENARIO scenario,
            Dictionary<string, string> scenarioParams,
            Dictionary<string, string> additionalParams = null)
        {
            if (false == IsAutoScenario(scenario))
                return false;

            AutoScenarioRequest request = CreateAutoScenarioRequest(
                sender,
                scenario,
                scenarioParams,
                additionalParams,
                null,
                useLogging);

            RegisterEnqueuedAutoScenarioRequest(request);
            return true;
        }

        public EN_SCENARIO_RESULT EnqueueAutoScenario(
            string sender,
            bool useLogging,
            EN_SCENARIO scenario,
            Dictionary<string, string> scenarioParams,
            deleAutoScenarioCompleted callback,
            Dictionary<string, string> additionalParams = null)
        {
            if (false == (_host.GetInstanceScenario(scenario) is AutoScenarioBase))
                return EN_SCENARIO_RESULT.ERROR;

            AutoScenarioRequest request = CreateAutoScenarioRequest(
                sender,
                scenario,
                scenarioParams,
                additionalParams,
                callback,
                useLogging);

            RegisterEnqueuedAutoScenarioRequest(request);
            return EN_SCENARIO_RESULT.WAITING;
        }

        public Dictionary<string, string> ConsumeScenarioResultData(
            string sender,
            EN_SCENARIO scenario)
        {
            if (false == IsAutoScenario(scenario))
                return null;

            SenderScenarioKey key = MakeSenderScenarioKey(sender, scenario);
            if (false == _pendingAutoScenarioResults.TryRemove(key, out AutoScenarioStoredResult stored))
                return null;

            RemovePendingSenderScopedExecutionState(sender, scenario);
            RemoveLastSenderScopedExecutionState(sender, scenario);

            return stored.ResultData == null
                ? new Dictionary<string, string>()
                : new Dictionary<string, string>(stored.ResultData);
        }

        public Dictionary<string, string> GetLastScenarioResultData(
            string sender,
            EN_SCENARIO scenario)
        {
            if (false == IsAutoScenario(scenario))
                return null;

            SenderScenarioKey key = MakeSenderScenarioKey(sender, scenario);
            if (false == _lastAutoScenarioResults.TryGetValue(key, out AutoScenarioStoredResult stored))
                return null;

            return stored.ResultData == null
                ? new Dictionary<string, string>()
                : new Dictionary<string, string>(stored.ResultData);
        }

        public EN_SCENARIO_RESULT GetAutoScenarioExecutionState(
            string sender,
            EN_SCENARIO scenario)
        {
            if (false == IsAutoScenario(scenario))
                return EN_SCENARIO_RESULT.ERROR;

            SenderScenarioKey key = MakeSenderScenarioKey(sender, scenario);

            if (_pendingAutoScenarioExecutionStates.TryGetValue(key, out EN_SCENARIO_RESULT currentState))
            {
                //WriteAutoScenarioLog(
                //    "GET_STATE",
                //    sender,
                //    scenario,
                //    $"state={currentState}");
                return currentState;
            }

            //WriteAutoScenarioLog(
            //    "GET_STATE",
            //    sender,
            //    scenario,
            //    "state=ERROR");

            // 해석 2:
            // pending이 비워졌으면 last execution state도 보지 않는다.
            return EN_SCENARIO_RESULT.ERROR;
        }

        public void ProcessAutoScenarioQueue()
        {
            if (_activeAutoScenarioRequest == null)
            {
                if (false == _autoScenarioQueue.TryDequeue(out AutoScenarioRequest head))
                    return;

                WriteAutoScenarioLog(
                    EN_AUTO_SCENARIO_LOG_PHASE.DEQUEUE,
                    head.Sender,
                    head.Scenario,
                    $"queue={_autoScenarioQueue.Count}");

                if (_host.IsScenarioRunning(head.Scenario))
                {
                    _autoScenarioQueue.Enqueue(head);

                    WriteAutoScenarioLog(
                        EN_AUTO_SCENARIO_LOG_PHASE.DEQUEUE_REQUEUE,
                        head.Sender,
                        head.Scenario,
                        $"queue={_autoScenarioQueue.Count}");

                    return;
                }

                if (false == TryPrepareAutoScenarioRequest(head))
                {
                    CompleteAutoScenarioRequest(head, EN_SCENARIO_RESULT.ERROR);
                    return;
                }

                _activeAutoScenarioRequest = head;

                WriteAutoScenarioLog(
                    EN_AUTO_SCENARIO_LOG_PHASE.START,
                    head.Sender,
                    head.Scenario);
            }

            ExecuteActiveAutoScenarioRequest();
        }
        #endregion </Public Methods>

        #region <Internal Methods>
        private AutoScenarioRuntimeContext GetOrCreateAutoScenarioContext(EN_SCENARIO scenario)
        {
            return _autoScenarioContexts.GetOrAdd(
                scenario,
                _ => new AutoScenarioRuntimeContext(scenario));
        }

        private void ApplyAutoScenarioState(EN_SCENARIO scenario)
        {
            AutoScenarioBase autoScenario = _host.GetInstanceScenario(scenario) as AutoScenarioBase;
            if (autoScenario == null)
                return;

            AutoScenarioRuntimeContext context = GetOrCreateAutoScenarioContext(scenario);
            lock (context.SyncRoot)
            {
                autoScenario.SetAutoState(context.CurrentState.AutoState);
            }
        }

        private AutoScenarioRequest CreateAutoScenarioRequest(
            string sender,
            EN_SCENARIO scenario,
            Dictionary<string, string> scenarioParams,
            Dictionary<string, string> additionalParams,
            deleAutoScenarioCompleted callback,
            bool useLogging)
        {
            return new AutoScenarioRequest
            {
                Sender = NormalizeAutoScenarioSender(sender),
                Scenario = scenario,
                ScenarioParams = scenarioParams == null
                    ? new Dictionary<string, string>()
                    : new Dictionary<string, string>(scenarioParams),
                AdditionalParams = additionalParams == null
                    ? new Dictionary<string, string>()
                    : new Dictionary<string, string>(additionalParams),
                Callback = callback,
                QueueState = EN_SCENARIO_RESULT.WAITING,
                UseLogging = useLogging
            };
        }

        private void RegisterEnqueuedAutoScenarioRequest(AutoScenarioRequest request)
        {
            if (request == null)
                return;

            AutoScenarioRuntimeContext context = GetOrCreateAutoScenarioContext(request.Scenario);
            lock (context.SyncRoot)
            {
                context.MarkEnqueued();
            }

            SetSenderScopedExecutionState(
                request.Sender,
                request.Scenario,
                EN_SCENARIO_RESULT.WAITING);

            ApplyAutoScenarioState(request.Scenario);
            _autoScenarioQueue.Enqueue(request);

            WriteAutoScenarioLog(
                EN_AUTO_SCENARIO_LOG_PHASE.ENQUEUE,
                request.Sender,
                request.Scenario,
                $"queue={GetPendingQueueCount()}");
        }

        private bool TryPrepareAutoScenarioRequest(AutoScenarioRequest request)
        {
            if (request == null)
                return false;

            ScenarioBaseClass scen = _host.GetInstanceScenario(request.Scenario);
            if (scen == null)
                return false;

            if (false == (scen is AutoScenarioBase))
                return false;

            // 같은 sender + scenario 새 실행 시작 시 pending / last 동시 초기화
            ResetSenderScopedResults(request.Sender, request.Scenario);
            //ResetSenderScopedExecutionState(request.Sender, request.Scenario);

            _host.SetScenarioActivation(request.Scenario, false);
            _host.InitScenarioResultData(request.Scenario);
            scen.InitPermission();

            if (false == _host.UpdateScenarioParams(request.Scenario.ToString(), request.ScenarioParams))
            {
                _host.SetScenarioActivation(request.Scenario, false);

                AutoScenarioRuntimeContext failedContext = GetOrCreateAutoScenarioContext(request.Scenario);
                lock (failedContext.SyncRoot)
                {
                    failedContext.MarkPrepareFailed();
                }

                SetSenderScopedExecutionState(
                    request.Sender,
                    request.Scenario,
                    EN_SCENARIO_RESULT.ERROR);

                ApplyAutoScenarioState(request.Scenario);

                WriteAutoScenarioLog(
                    EN_AUTO_SCENARIO_LOG_PHASE.PREPARE_FAIL,
                    request.Sender,
                    request.Scenario,
                    "result=ERROR",
                    includeStep: true);

                return false;
            }

            _host.SetScenarioActivation(request.Scenario, true);

            AutoScenarioRuntimeContext context = GetOrCreateAutoScenarioContext(request.Scenario);
            lock (context.SyncRoot)
            {
                context.MarkPrepareSucceeded();
            }

            SetSenderScopedExecutionState(
                request.Sender,
                request.Scenario,
                EN_SCENARIO_RESULT.PROCEED);

            ApplyAutoScenarioState(request.Scenario);
            return true;
        }

        private void ExecuteActiveAutoScenarioRequest()
        {
            if (_activeAutoScenarioRequest == null)
                return;

            EN_SCENARIO_RESULT result = _host.ExecuteScenario(_activeAutoScenarioRequest.Scenario);
            switch (result)
            {
                case EN_SCENARIO_RESULT.PROCEED:
                    return;

                case EN_SCENARIO_RESULT.COMPLETED:
                case EN_SCENARIO_RESULT.ERROR:
                case EN_SCENARIO_RESULT.TIMEOUT_ERROR:
                    CompleteAutoScenarioRequest(_activeAutoScenarioRequest, result);
                    _activeAutoScenarioRequest = null;
                    return;

                default:
                    CompleteAutoScenarioRequest(_activeAutoScenarioRequest, EN_SCENARIO_RESULT.ERROR);
                    _activeAutoScenarioRequest = null;
                    return;
            }
        }

        private void CompleteAutoScenarioRequest(
            AutoScenarioRequest request,
            EN_SCENARIO_RESULT result)
        {
            if (request == null)
                return;

            Dictionary<string, string> resultData = _host.GetScenarioResultData(request.Scenario)
                ?? new Dictionary<string, string>();

            AutoScenarioRuntimeContext context = GetOrCreateAutoScenarioContext(request.Scenario);
            lock (context.SyncRoot)
            {
                context.MarkTerminal(result, resultData);
            }

            ApplyAutoScenarioState(request.Scenario);

            // pending / last result 동시 저장
            SaveSenderScopedResults(request, resultData, result);

            // pending / last execution state 동시 저장
            SetSenderScopedExecutionState(
                request.Sender,
                request.Scenario,
                result);

            switch (result)
            {
                case EN_SCENARIO_RESULT.COMPLETED:
                    WriteAutoScenarioLog(
                        EN_AUTO_SCENARIO_LOG_PHASE.COMPLETE,
                        request.Sender,
                        request.Scenario,
                        "result=COMPLETED");
                    break;

                case EN_SCENARIO_RESULT.ERROR:
                    WriteAutoScenarioLog(
                        EN_AUTO_SCENARIO_LOG_PHASE.COMPLETE_ERROR,
                        request.Sender,
                        request.Scenario,
                        "result=ERROR",
                        includeStep: true);
                    break;

                case EN_SCENARIO_RESULT.TIMEOUT_ERROR:
                    WriteAutoScenarioLog(
                        EN_AUTO_SCENARIO_LOG_PHASE.COMPLETE_TIMEOUT,
                        request.Sender,
                        request.Scenario,
                        "result=TIMEOUT_ERROR",
                        includeStep: true);
                    break;
            }

            try
            {
                request.Callback?.Invoke(
                    NormalizeAutoScenarioSender(request.Sender),
                    request.Scenario,
                    new Dictionary<string, string>(request.ScenarioParams),
                    new Dictionary<string, string>(resultData),
                    result);
            }
            catch (Exception ex)
            {
                // callback 실패는 runtime 밖으로 전파하지 않고 로그만 남긴다.
                WriteAutoScenarioLog(
                    EN_AUTO_SCENARIO_LOG_PHASE.CALLBACK_ERROR,
                    request.Sender,
                    request.Scenario,
                    string.Format("type={0} ex={1}", ex.GetType().Name, ex.Message),
                    includeStep: true);
            }
            finally
            {
                if (request.Callback != null)
                {
                    RemovePendingSenderScopedResult(request.Sender, request.Scenario);
                    RemovePendingSenderScopedExecutionState(request.Sender, request.Scenario);
                    RemoveLastSenderScopedExecutionState(request.Sender, request.Scenario);
                }

                _host.RaiseAutoScenarioCompleted(request, result, resultData);
            }
        }

        private static string NormalizeAutoScenarioSender(string sender)
        {
            return string.IsNullOrWhiteSpace(sender) ? "Unknown" : sender.Trim();
        }

        private static SenderScenarioKey MakeSenderScenarioKey(string sender, EN_SCENARIO scenario)
        {
            return new SenderScenarioKey(NormalizeAutoScenarioSender(sender), scenario);
        }

        private void ResetSenderScopedResults(string sender, EN_SCENARIO scenario)
        {
            SenderScenarioKey key = MakeSenderScenarioKey(sender, scenario);

            _pendingAutoScenarioResults.TryRemove(key, out _);
            _lastAutoScenarioResults.TryRemove(key, out _);
        }

        private void SaveSenderScopedResults(
            AutoScenarioRequest request,
            Dictionary<string, string> resultData,
            EN_SCENARIO_RESULT result)
        {
            if (request == null)
                return;

            SenderScenarioKey key = MakeSenderScenarioKey(request.Sender, request.Scenario);

            AutoScenarioStoredResult stored = new AutoScenarioStoredResult
            {
                Sender = NormalizeAutoScenarioSender(request.Sender),
                Scenario = request.Scenario,
                ScenarioParams = request.ScenarioParams == null
                    ? new Dictionary<string, string>()
                    : new Dictionary<string, string>(request.ScenarioParams),
                ResultData = resultData == null
                    ? new Dictionary<string, string>()
                    : new Dictionary<string, string>(resultData),
                Result = result
            };

            _pendingAutoScenarioResults[key] = stored.Clone();
            _lastAutoScenarioResults[key] = stored.Clone();
        }

        private void RemovePendingSenderScopedResult(string sender, EN_SCENARIO scenario)
        {
            SenderScenarioKey key = MakeSenderScenarioKey(sender, scenario);
            _pendingAutoScenarioResults.TryRemove(key, out _);
        }

        private void ResetSenderScopedExecutionState(string sender, EN_SCENARIO scenario)
        {
            SenderScenarioKey key = MakeSenderScenarioKey(sender, scenario);

            _pendingAutoScenarioExecutionStates.TryRemove(key, out _);
            _lastAutoScenarioExecutionStates.TryRemove(key, out _);
        }

        private void SetSenderScopedExecutionState(
            string sender,
            EN_SCENARIO scenario,
            EN_SCENARIO_RESULT state)
        {
            SenderScenarioKey key = MakeSenderScenarioKey(sender, scenario);

            _pendingAutoScenarioExecutionStates[key] = state;
            _lastAutoScenarioExecutionStates[key] = state;
        }

        private void RemovePendingSenderScopedExecutionState(string sender, EN_SCENARIO scenario)
        {
            SenderScenarioKey key = MakeSenderScenarioKey(sender, scenario);
            _pendingAutoScenarioExecutionStates.TryRemove(key, out _);
        }

        private void RemoveLastSenderScopedExecutionState(string sender, EN_SCENARIO scenario)
        {
            SenderScenarioKey key = MakeSenderScenarioKey(sender, scenario);
            _lastAutoScenarioExecutionStates.TryRemove(key, out _);
        }
        private int GetScenarioStepSafe(EN_SCENARIO scenario)
        {
            ScenarioBaseClass scen = _host.GetInstanceScenario(scenario);
            if (scen == null)
                return DefineSecsGem.Contants.SCENARIO_STEP_END;

            return scen.Step;
        }
        private static string FormatAutoScenarioLogPhase(EN_AUTO_SCENARIO_LOG_PHASE phase)
        {
            string text;
            switch (phase)
            {
                case EN_AUTO_SCENARIO_LOG_PHASE.ENQUEUE:
                    text = "ENQUEUE";
                    break;

                case EN_AUTO_SCENARIO_LOG_PHASE.DEQUEUE:
                    text = "DEQUEUE";
                    break;

                case EN_AUTO_SCENARIO_LOG_PHASE.DEQUEUE_REQUEUE:
                    text = "DEQUEUE_REQUEUE";
                    break;

                case EN_AUTO_SCENARIO_LOG_PHASE.START:
                    text = "START";
                    break;

                case EN_AUTO_SCENARIO_LOG_PHASE.PREPARE_FAIL:
                    text = "PREPARE_FAIL";
                    break;

                case EN_AUTO_SCENARIO_LOG_PHASE.COMPLETE:
                    text = "COMPLETE";
                    break;

                case EN_AUTO_SCENARIO_LOG_PHASE.COMPLETE_ERROR:
                    text = "COMPLETE_ERROR";
                    break;

                case EN_AUTO_SCENARIO_LOG_PHASE.COMPLETE_TIMEOUT:
                    text = "COMPLETE_TIMEOUT";
                    break;

                case EN_AUTO_SCENARIO_LOG_PHASE.CALLBACK_ERROR:
                    text = "CALLBACK_ERROR";
                    break;

                default:
                    text = "NONE";
                    break;
            }

            return text.PadRight(16);
        }
        private void WriteAutoScenarioLog(
            EN_AUTO_SCENARIO_LOG_PHASE phase,
            string sender,
            EN_SCENARIO scenario,
            string message = null,
            bool includeStep = false)
        {
            string normalizedSender = NormalizeAutoScenarioSender(sender);
            string phaseText = FormatAutoScenarioLogPhase(phase);
            string senderText = (normalizedSender ?? string.Empty).PadRight(12);

            string log = $"[AUTO]\t[{phaseText}]\t[{senderText}] scenario={scenario}";

            if (includeStep)
            {
                log = $"{log} step={GetScenarioStepSafe(scenario)}";
            }

            if (false == string.IsNullOrWhiteSpace(message))
            {
                log = $"{log} {message}";
            }

            _host.WriteScenarioRuntimeLog(log);
        }

        private bool ShouldLogEnqueue(AutoScenarioRequest request)
        {
            if (request == null)
                return false;

            return request.UseLogging;
        }
        private int GetPendingQueueCount()
        {
            return _autoScenarioQueue.Count;
        }
        #endregion </Internal Methods>
    }
}