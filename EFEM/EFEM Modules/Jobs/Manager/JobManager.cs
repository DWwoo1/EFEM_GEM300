using System;
using System.Collections.Generic;
using System.Threading;

using FrameOfSystem3.SECSGEM;

using EFEM.Defines.Common;
using EFEM.Defines.Job;
using EFEM.Jobs.Domain;
using EFEM.Jobs.Repository;
using EFEM.Jobs.Binding;
using EFEM.Jobs.Policy;

namespace EFEM.Jobs.Manager
{
    public sealed class JobManager :
        IJobManager,
        IControlJobServiceCallback,
        IProcessJobServiceCallback
    {
        #region <Constructors>
        private JobManager(
            IGem300ScenarioService gem300Service,
            IOrderedRepository<ControlJob, string> controlJobRepository,
            IOrderedRepository<ProcessJob, string> processJobRepository,
            IJobRelationRepository relationRepository,
            ISecsGemResultEvaluator resultEvaluator,
            IProcessJobRemovalPolicy processJobRemovalPolicy)
        {
            _gem300Service = gem300Service ?? throw new ArgumentNullException(nameof(gem300Service));
            _controlJobRepository = controlJobRepository ?? throw new ArgumentNullException(nameof(controlJobRepository));
            _processJobRepository = processJobRepository ?? throw new ArgumentNullException(nameof(processJobRepository));
            _relationRepository = relationRepository ?? throw new ArgumentNullException(nameof(relationRepository));
            _resultEvaluator = resultEvaluator ?? throw new ArgumentNullException(nameof(resultEvaluator));

            // null이면 기존 동작 유지
            _processJobRemovalPolicy =
                processJobRemovalPolicy ?? new ImmediateProcessJobRemovalPolicy();
        }
        #endregion </Constructors>

        #region <Fields>
        private readonly IOrderedRepository<ControlJob, string> _controlJobRepository;
        private readonly IOrderedRepository<ProcessJob, string> _processJobRepository;
        private readonly IJobRelationRepository _relationRepository;
        private readonly ISecsGemResultEvaluator _resultEvaluator;
        private readonly IProcessJobRemovalPolicy _processJobRemovalPolicy;

        private static readonly object _configureLock = new object();
        private static IJobManager _instance;
        private readonly IGem300ScenarioService _gem300Service;

        private readonly object _autoRemoveLock = new object();

        private readonly HashSet<string> _autoRemovingControlJobIds = new HashSet<string>();

        private static readonly ControlJobState[] ActiveControlJobPriority =
        {
            ControlJobState.Executing,
            ControlJobState.Paused,
            ControlJobState.WaitingForStart,
            ControlJobState.Selected
        };
        private static readonly ProcessJobState[] ActiveProcessJobPriority =
        {
            ProcessJobState.Processing,
            ProcessJobState.Pausing,
            ProcessJobState.Paused,
            ProcessJobState.Stopping,
            ProcessJobState.Aborting,
            ProcessJobState.WaitingForStart,
            ProcessJobState.SettingUp
        };
        #endregion </Fields>

        #region <Properties>
        public static IJobManager Instance
        {
            get
            {
                var instance = Volatile.Read(ref _instance);

                if (instance == null)
                    throw new InvalidOperationException("JobManager is not configured.");

                return instance;
            }
        }
        #endregion </Properties>

        #region <Configuration>
        public static void ConfigureDeferred(
            IGem300ScenarioService gem300Service,
            IOrderedRepository<ControlJob, string> controlJobRepository,
            IOrderedRepository<ProcessJob, string> processJobRepository,
            IJobRelationRepository relationRepository,
            ISecsGemResultEvaluator resultEvaluator,
            IProcessJobRemovalPolicy processJobRemovalPolicy)
        {
            if (gem300Service == null)
                throw new ArgumentNullException(nameof(gem300Service));

            if (controlJobRepository == null)
                throw new ArgumentNullException(nameof(controlJobRepository));

            if (processJobRepository == null)
                throw new ArgumentNullException(nameof(processJobRepository));

            if (relationRepository == null)
                throw new ArgumentNullException(nameof(relationRepository));

            if (resultEvaluator == null)
                throw new ArgumentNullException(nameof(resultEvaluator));

            lock (_configureLock)
            {
                if (_instance != null)
                    throw new InvalidOperationException("JobManager is already configured.");

                var instance = new JobManager(
                    gem300Service,
                    controlJobRepository,
                    processJobRepository,
                    relationRepository,
                    resultEvaluator,
                    processJobRemovalPolicy);

                instance.RegisterCallbacks();

                Volatile.Write(ref _instance, instance);
            }
        }

        private void RegisterCallbacks()
        {
            _gem300Service.RegisterControlJobServiceCallback(this);
            _gem300Service.RegisterProcessJobServiceCallback(this);
        }
        #endregion </Configuration>

        #region <Create>
        public long CreateProcessJob(
            string processJobId,
            MaterialFormat materialFormat,
            ProcessStartMode startMode,
            MaterialOrderMode materialOrder,
            IReadOnlyDictionary<string, IReadOnlyList<int>> materialInfo,
            RecipeMethod recipeMethod,
            string recipeId,
            string[] recipeParameterNames,
            string[] recipeParameterValues)
        {
            ValidateMaterialInfoOrThrow(materialInfo);

            var processJob = new ProcessJob(
                processJobId,
                ProcessJobState.JobQueued,
                materialFormat,
                startMode,
                materialOrder,
                materialInfo,
                recipeMethod,
                recipeId,
                recipeParameterNames,
                recipeParameterValues);

            _processJobRepository.AddOrUpdate(processJob);

            var result = _gem300Service.ProcessJob.Create(
                processJobId,
                materialFormat,
                startMode,
                materialOrder,
                materialInfo,
                recipeMethod,
                recipeId,
                recipeParameterNames,
                recipeParameterValues);

            if (!_resultEvaluator.IsSuccess(result))
            {
                _processJobRepository.Remove(processJobId);
            }
            else
            {
                // Job이 먼저 생성되고 재료가 아직 도착하지 않았을 수 있다.
                // 이 경우 Binder는 아무 것도 하지 않고 종료한다.
                // 이후 Carrier SlotMap 검증 후 BindByCarrierPort에서 다시 바인딩된다.
                if (SubstrateJobBindingService.Instance != null)
                    SubstrateJobBindingService.Instance.BindByProcessJob(processJobId);
            }

            return result;
        }

        public long CreateProcessJobWithNumericRecipe(
            string processJobId,
            MaterialFormat materialFormat,
            ProcessStartMode startMode,
            MaterialOrderMode materialOrder,
            IReadOnlyDictionary<string, IReadOnlyList<int>> materialInfo,
            RecipeMethod recipeMethod,
            string recipeId,
            string[] recipeParameterNames,
            long[] recipeParameterValues)
        {
            ValidateMaterialInfoOrThrow(materialInfo);

            string[] convertedValues = ConvertNumericRecipeValues(recipeParameterValues);

            var processJob = new ProcessJob(
                processJobId,
                ProcessJobState.JobQueued,
                materialFormat,
                startMode,
                materialOrder,
                materialInfo,
                recipeMethod,
                recipeId,
                recipeParameterNames,
                convertedValues);

            _processJobRepository.AddOrUpdate(processJob);

            // SDK도 MaterialInfo를 받으므로 그대로 전달한다.
            var result = _gem300Service.ProcessJob.CreateWithNumericRecipe(
                processJobId,
                materialFormat,
                startMode,
                materialOrder,
                materialInfo,
                recipeMethod,
                recipeId,
                recipeParameterNames,
                recipeParameterValues);

            if (!_resultEvaluator.IsSuccess(result))
            {
                _processJobRepository.Remove(processJobId);
            }
            else
            {
                if (SubstrateJobBindingService.Instance != null)
                    SubstrateJobBindingService.Instance.BindByProcessJob(processJobId);
            }

            return result;
        }
        public long CreateControlJob(
            string controlJobId,
            ControlJobStartMode startMode,
            string[] processJobIds)
        {
            if (string.IsNullOrWhiteSpace(controlJobId))
                throw new ArgumentException("ControlJobId is invalid.", nameof(controlJobId));

            ValidateLinkedProcessJobsExist(processJobIds);

            if (!_relationRepository.CanLink(controlJobId, processJobIds))
            {
                throw new InvalidOperationException(
                    "One or more ProcessJobs are already linked to another ControlJob.");
            }

            var controlJob = new ControlJob(
                controlJobId,
                ControlJobState.Queued,
                startMode,
                processJobIds);

            _controlJobRepository.AddOrUpdate(controlJob);
            _relationRepository.Link(controlJobId, processJobIds);

            var result = _gem300Service.ControlJob.Create(
                controlJobId,
                startMode,
                processJobIds);

            if (!_resultEvaluator.IsSuccess(result))
            {
                _controlJobRepository.Remove(controlJobId);
                _relationRepository.UnlinkControlJob(controlJobId);
            }
            else
            {
                SynchronizeControlJobProcessJobStatus(controlJobId);

                if (SubstrateJobBindingService.Instance != null)
                    SubstrateJobBindingService.Instance.BindByControlJob(controlJobId);
            }

            return result;
        }
        #endregion </Create>

        #region <Recovery>
        public void RebindRecoveredJobs()
        {
            var binder = SubstrateJobBindingService.Instance;

            if (binder == null)
                return;

            // ProcessJob 먼저 바인딩한다.
            // ProcessJob에는 MaterialInfo / RecipeId가 있고,
            // Substrate에 ProcessJobId / RecipeId를 먼저 기록할 수 있다.
            var processJobs = _processJobRepository.GetAll();

            foreach (var processJob in processJobs)
            {
                if (processJob == null)
                    continue;

                binder.BindByProcessJob(processJob.Id);
            }

            // 그 다음 ControlJob을 바인딩한다.
            // 이유:
            // ControlJob은 relation을 통해 연결된 ProcessJob들을 기준으로
            // Substrate에 ControlJobId까지 보강한다.
            var controlJobs = _controlJobRepository.GetAll();

            foreach (var controlJob in controlJobs)
            {
                if (controlJob == null)
                    continue;

                binder.BindByControlJob(controlJob.Id);
            }
        }
        #endregion </Recovery>

        #region <Request>
        public long RequestControlJob(string controlJobId)
        {
            if (string.IsNullOrWhiteSpace(controlJobId))
                throw new ArgumentException("ControlJobId is invalid.", nameof(controlJobId));

            return _gem300Service.ControlJob.RequestJob(controlJobId);
        }
        public long RequestAllControlJobIds()
        {
            return _gem300Service.ControlJob.RequestAllJobIds();
        }
        public long RequestProcessJob(string processJobId)
        {
            if (string.IsNullOrWhiteSpace(processJobId))
                throw new ArgumentException("ProcessJobId is invalid.", nameof(processJobId));

            return _gem300Service.ProcessJob.RequestJob(processJobId);
        }
        public long RequestAllProcessJobIds()
        {
            return _gem300Service.ProcessJob.RequestAllJobIds();
        }
        public long RequestControlJobSelect(string controlJobId)
        {
            if (string.IsNullOrWhiteSpace(controlJobId))
                throw new ArgumentException("ControlJobId is invalid.", nameof(controlJobId));

            return _gem300Service.ControlJob.RequestSelect(controlJobId);
        }
        public long RequestControlJobHeadOfQueue(string controlJobId)
        {
            if (string.IsNullOrWhiteSpace(controlJobId))
                throw new ArgumentException("ControlJobId is invalid.", nameof(controlJobId));

            var job = _controlJobRepository.GetOrDefault(controlJobId);

            if (job == null)
            {
                throw new InvalidOperationException(
                    "ControlJob does not exist. ControlJobId=" + controlJobId);
            }

            if (!IsQueuedControlJob(job.State))
            {
                throw new InvalidOperationException(
                    "ControlJob is not queued. ControlJobId="
                    + controlJobId
                    + ", State="
                    + job.State);
            }

            int currentIndex = _controlJobRepository.IndexOf(controlJobId);
            int queueHeadIndex = GetControlJobQueueHeadIndex();

            if (currentIndex < 0 || queueHeadIndex < 0)
            {
                // TODO : 로그 필요
                Console.WriteLine(
                    "[{0}] JobManager HOQ request failed. ControlJobId={1}, CurrentIndex={2}, QueueHeadIndex={3}",
                    DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff"),
                    controlJobId,
                    currentIndex,
                    queueHeadIndex);
                //throw new InvalidOperationException(
                //    "ControlJob queue index is invalid. ControlJobId="
                //    + controlJobId
                //    + ", CurrentIndex="
                //    + currentIndex
                //    + ", QueueHeadIndex="
                //    + queueHeadIndex);
                return JobAcknowledgeResult.Failure;
            }

            if (currentIndex == queueHeadIndex)
                return JobAcknowledgeResult.Success;

            var result = _gem300Service.ControlJob.RequestHeadOfQueue(controlJobId);

            if (_resultEvaluator.IsSuccess(result))
                _controlJobRepository.MoveToIndex(controlJobId, queueHeadIndex);

            return result;
        }
        public long RequestControlJobHeadOfQueueInfo()
        {
            return _gem300Service.ControlJob.RequestHeadOfQueueInfo();
        }
        #endregion </Request>

        #region <Command>
        public long RequestControlJobCommand(
            string controlJobId,
            ControlJobCommand command,
            string commandParameterName,
            string commandParameterValue)
        {
            if (string.IsNullOrWhiteSpace(controlJobId))
                throw new ArgumentException("ControlJobId is invalid.", nameof(controlJobId));

            var job = _controlJobRepository.GetOrDefault(controlJobId);

            if (job == null)
                throw new InvalidOperationException(
                    "ControlJob does not exist. ControlJobId=" + controlJobId);

            var result = _gem300Service.ControlJob.RequestCommand(
                controlJobId,
                command,
                commandParameterName,
                commandParameterValue);

            return result;
        }
        public long RequestProcessJobCommand(
            string processJobId,
            ProcessJobCommand command)
        {
            if (string.IsNullOrWhiteSpace(processJobId))
                throw new ArgumentException("ProcessJobId is invalid.", nameof(processJobId));

            var job = _processJobRepository.GetOrDefault(processJobId);

            if (job == null)
                throw new InvalidOperationException(
                    "ProcessJob does not exist. ProcessJobId=" + processJobId);

            var result = _gem300Service.ProcessJob.RequestCommand(
                processJobId,
                command);

            return result;
        }
        #endregion </Command>

        #region <Set>
        public long SetControlJobInfo(
            string controlJobId,
            ControlJobState state,
            ControlJobStartMode startMode,
            string[] processJobIds)
        {
            if (string.IsNullOrWhiteSpace(controlJobId))
                throw new ArgumentException("ControlJobId is invalid.", nameof(controlJobId));

            if (processJobIds == null)
                throw new ArgumentNullException(nameof(processJobIds));

            ValidateLinkedProcessJobsExist(processJobIds);

            if (!_relationRepository.CanLink(controlJobId, processJobIds))
            {
                throw new InvalidOperationException(
                    "One or more ProcessJobs are already linked to another ControlJob.");
            }

            var result = _gem300Service.ControlJob.SetJobInfo(
                controlJobId,
                state,
                startMode,
                processJobIds);

            if (_resultEvaluator.IsSuccess(result))
            {
                var job = _controlJobRepository.GetOrDefault(controlJobId);

                if (job == null)
                {
                    job = new ControlJob(
                        controlJobId,
                        state,
                        startMode,
                        processJobIds);
                }
                else
                {
                    job.ChangeState(state);
                    job.ChangeStartMode(startMode);
                    job.ChangeProcessJobIds(processJobIds);
                }

                _controlJobRepository.AddOrUpdate(job);
                _relationRepository.Link(controlJobId, processJobIds);

                SynchronizeControlJobProcessJobStatus(controlJobId);

                if (SubstrateJobBindingService.Instance != null)
                    SubstrateJobBindingService.Instance.BindByControlJob(controlJobId);
            }

            return result;
        }
        public long SetProcessJobInfo(
            string processJobId,
            MaterialFormat materialFormat,
            ProcessStartMode startMode,
            MaterialOrderMode materialOrder,
            IReadOnlyDictionary<string, IReadOnlyList<int>> materialInfo,
            RecipeMethod recipeMethod,
            string recipeId,
            string[] recipeParameterNames,
            string[] recipeParameterValues)
        {
            ValidateMaterialInfoOrThrow(materialInfo);

            if (string.IsNullOrWhiteSpace(processJobId))
                throw new ArgumentException("ProcessJobId is invalid.", nameof(processJobId));

            var result = _gem300Service.ProcessJob.SetJobInfo(
                processJobId,
                materialFormat,
                startMode,
                materialOrder,
                materialInfo,
                recipeMethod,
                recipeId,
                recipeParameterNames,
                recipeParameterValues);

            if (_resultEvaluator.IsSuccess(result))
            {
                var job = _processJobRepository.GetOrDefault(processJobId);

                if (job == null)
                {
                    job = new ProcessJob(
                        processJobId,
                        ProcessJobState.JobQueued,
                        materialFormat,
                        startMode,
                        materialOrder,
                        materialInfo,
                        recipeMethod,
                        recipeId,
                        recipeParameterNames,
                        recipeParameterValues);
                }
                else
                {
                    /*
                     * 기존 MaterialInfo 기준으로 이미 Substrate에 바인딩된 정보가 있을 수 있다.
                     * MaterialInfo가 Carrier+Slot 있음 -> Carrier만 있음 / Empty 로 바뀌면
                     * 새 BindByProcessJob()만으로는 기존 바인딩을 지울 수 없다.
                     */
                    if (SubstrateJobBindingService.Instance != null)
                        SubstrateJobBindingService.Instance.UnbindByProcessJob(processJobId);

                    job.ChangeStartMode(startMode);
                    job.ChangeMaterialOrder(materialOrder);
                    job.ChangeMaterial(materialFormat, materialInfo);
                    job.ChangeRecipeInfo(
                        recipeMethod,
                        recipeId,
                        recipeParameterNames,
                        recipeParameterValues);
                }

                _processJobRepository.AddOrUpdate(job);

                if (SubstrateJobBindingService.Instance != null)
                    SubstrateJobBindingService.Instance.BindByProcessJob(processJobId);
            }

            return result;
        }
        public long SetProcessJobInfoWithNumericRecipe(
            string processJobId,
            MaterialFormat materialFormat,
            ProcessStartMode startMode,
            MaterialOrderMode materialOrder,
            IReadOnlyDictionary<string, IReadOnlyList<int>> materialInfo,
            RecipeMethod recipeMethod,
            string recipeId,
            string[] recipeParameterNames,
            long[] recipeParameterValues)
        {
            ValidateMaterialInfoOrThrow(materialInfo);

            if (string.IsNullOrWhiteSpace(processJobId))
                throw new ArgumentException("ProcessJobId is invalid.", nameof(processJobId));

            var result = _gem300Service.ProcessJob.SetJobInfoWithNumericRecipe(
                processJobId,
                materialFormat,
                startMode,
                materialOrder,
                materialInfo,
                recipeMethod,
                recipeId,
                recipeParameterNames,
                recipeParameterValues);

            if (_resultEvaluator.IsSuccess(result))
            {
                string[] convertedValues = ConvertNumericRecipeValues(recipeParameterValues);

                var job = _processJobRepository.GetOrDefault(processJobId);

                if (job == null)
                {
                    job = new ProcessJob(
                        processJobId,
                        ProcessJobState.JobQueued,
                        materialFormat,
                        startMode,
                        materialOrder,
                        materialInfo,
                        recipeMethod,
                        recipeId,
                        recipeParameterNames,
                        convertedValues);
                }
                else
                {
                    /*
                     * MaterialInfo 변경 전에 기존 Substrate 바인딩을 제거한다.
                     * 특히 Carrier+Slot 있음 -> Carrier만 있음 / Empty 로 바뀌는 경우
                     * 기존 Substrate에 남은 Job 정보를 제거하기 위함이다.
                     */
                    if (SubstrateJobBindingService.Instance != null)
                        SubstrateJobBindingService.Instance.UnbindByProcessJob(processJobId);

                    job.ChangeStartMode(startMode);
                    job.ChangeMaterialOrder(materialOrder);
                    job.ChangeMaterial(materialFormat, materialInfo);
                    job.ChangeRecipeInfo(
                        recipeMethod,
                        recipeId,
                        recipeParameterNames,
                        convertedValues);
                }

                _processJobRepository.AddOrUpdate(job);

                if (SubstrateJobBindingService.Instance != null)
                    SubstrateJobBindingService.Instance.BindByProcessJob(processJobId);
            }

            return result;
        }


        public long SetProcessJobState(
            string processJobId,
            ProcessJobState state)
        {
            if (string.IsNullOrWhiteSpace(processJobId))
                throw new ArgumentException("ProcessJobId is invalid.", nameof(processJobId));

            var job = _processJobRepository.GetOrDefault(processJobId);

            if (job == null)
                throw new InvalidOperationException(
                    "ProcessJob does not exist. ProcessJobId=" + processJobId);

            var result = _gem300Service.ProcessJob.SetState(
                processJobId,
                state);

            if (_resultEvaluator.IsSuccess(result))
            {
                job.ChangeState(state);
                _processJobRepository.AddOrUpdate(job);

                var controlJobId =
                    _relationRepository.GetControlJobIdOrDefault(processJobId);

                if (!string.IsNullOrWhiteSpace(controlJobId))
                {
                    /*
                     * ProcessJob 상태가 수동 Set으로 변경된 경우도
                     * ControlJob.ProcessJobStatus를 반드시 갱신한다.
                     */
                    SynchronizeControlJobProcessJobStatus(controlJobId);

                    RemoveControlJobIfAllProcessJobsTerminalOrRemoved(controlJobId);
                }
            }

            return result;
        }
        #endregion </Set>

        #region <Notify>
        public long NotifyProcessJobSettingUpStarted(string processJobId)
        {
            if (string.IsNullOrWhiteSpace(processJobId))
                throw new ArgumentException("ProcessJobId is invalid.", nameof(processJobId));

            var job = _processJobRepository.GetOrDefault(processJobId);

            if (job == null)
                throw new InvalidOperationException(
                    "ProcessJob does not exist. ProcessJobId=" + processJobId);

            var result = _gem300Service.ProcessJob.NotifySettingUpStarted(processJobId);
            if (_resultEvaluator.IsSuccess(result))
            {
                job.ChangeState(ProcessJobState.SettingUp);
                _processJobRepository.AddOrUpdate(job);

                /*
                 * Notify 계열에서 상태를 바꾼 경우도
                 * ControlJob 내부 스냅샷을 갱신한다.
                 */
                SynchronizeControlJobProcessJobStatusByProcessJobId(processJobId);
            }

            return result;
        }
        public long NotifyProcessJobSettingUpCompleted(string processJobId)
        {
            if (string.IsNullOrWhiteSpace(processJobId))
                throw new ArgumentException("ProcessJobId is invalid.", nameof(processJobId));

            var job = _processJobRepository.GetOrDefault(processJobId);

            if (job == null)
                throw new InvalidOperationException(
                    "ProcessJob does not exist. ProcessJobId=" + processJobId);

            var result = _gem300Service.ProcessJob.NotifySettingUpCompleted(processJobId);
            if (_resultEvaluator.IsSuccess(result))
            {
                job.ChangeState(ProcessJobState.WaitingForStart);
                _processJobRepository.AddOrUpdate(job);

                SynchronizeControlJobProcessJobStatusByProcessJobId(processJobId);
            }

            return result;
        }
        #endregion </Notify>

        #region <Remove>
        public long RemoveControlJob(
            string controlJobId,
            ControlJobRemoveMode removeMode)
        {
            if (removeMode == ControlJobRemoveMode.RejectIfProcessJobsExist)
            {
                if (_relationRepository.HasLinkedProcessJobs(controlJobId))
                {
                    throw new InvalidOperationException(
                        "ControlJob cannot be removed because linked ProcessJobs exist. ControlJobId="
                        + controlJobId);
                }

                return RemoveControlJobOnly(controlJobId);
            }

            if (removeMode == ControlJobRemoveMode.RemoveLinkedProcessJobs)
            {
                return RemoveLinkedProcessJobsThenControlJob(controlJobId);
            }

            throw new ArgumentOutOfRangeException(nameof(removeMode), removeMode, null);
        }
        private long RemoveControlJobOnly(string controlJobId)
        {
            var result = _gem300Service.ControlJob.Remove(controlJobId);

            if (_resultEvaluator.IsSuccess(result))
            {
                // 추가:
                // ControlJob Repository/Relation을 제거하기 전에 Unbind한다.
                // 이유:
                // UnbindByControlJob은 연결된 ProcessJob 정보를 조회해야 하기 때문이다.
                if (SubstrateJobBindingService.Instance != null)
                    SubstrateJobBindingService.Instance.UnbindByControlJob(controlJobId);

                _controlJobRepository.Remove(controlJobId);
                _relationRepository.UnlinkControlJob(controlJobId);
            }

            return result;
        }
        private long RemoveAllControlJobsWithLinkedProcessJobsOnly()
        {
            var controlJobs = _controlJobRepository.GetAll();

            foreach (var controlJob in controlJobs)
            {
                if (controlJob == null)
                    continue;

                var result = RemoveLinkedProcessJobsThenControlJob(controlJob.Id);

                if (!_resultEvaluator.IsSuccess(result))
                    return result;
            }

            return JobAcknowledgeResult.Success;
        }
        private long RemoveLinkedProcessJobsThenControlJob(string controlJobId)
        {
            var processJobIds = _relationRepository.GetProcessJobIds(controlJobId);

            if (processJobIds == null)
                processJobIds = new string[0];

            /*
             * 1단계:
             * 먼저 SDK 쪽 ProcessJob Remove 가능 여부를 처리한다.
             * 이 단계에서는 아직 로컬 Repository / Relation / Substrate Binding을 건드리지 않는다.
             */
            foreach (var processJobId in processJobIds)
            {
                if (string.IsNullOrWhiteSpace(processJobId))
                    continue;

                ProcessJob processJob =
                    _processJobRepository.GetOrDefault(processJobId);

                if (processJob == null)
                    continue;

                bool shouldRequestSdkRemove =
                    _processJobRemovalPolicy.ShouldRequestSdkRemoveOnControlJobRemoval(
                        processJob);

                if (!shouldRequestSdkRemove)
                    continue;

                long processJobRemoveResult =
                    _gem300Service.ProcessJob.Remove(processJobId);

                if (!_resultEvaluator.IsSuccess(processJobRemoveResult))
                {
                    bool ignoreFailure =
                        _processJobRemovalPolicy.ShouldIgnoreSdkRemoveFailureOnControlJobRemoval(
                            processJob,
                            processJobRemoveResult);

                    if (!ignoreFailure)
                        return processJobRemoveResult;
                }
            }

            /*
             * 2단계:
             * SDK ControlJob Remove를 수행한다.
             * ControlJob Remove가 실패하면 로컬 상태는 유지한다.
             */
            long controlJobRemoveResult =
                _gem300Service.ControlJob.Remove(controlJobId);

            if (!_resultEvaluator.IsSuccess(controlJobRemoveResult))
                return controlJobRemoveResult;

            /*
             * 3단계:
             * SDK 제거가 끝났으므로 이제 로컬 상태를 정리한다.
             */
            var binder = SubstrateJobBindingService.Instance;

            if (binder != null)
                binder.UnbindByControlJob(controlJobId);

            foreach (var processJobId in processJobIds)
            {
                if (string.IsNullOrWhiteSpace(processJobId))
                    continue;

                if (binder != null)
                {
                    binder.UnbindByProcessJob(processJobId);
                    binder.ClearRemovedBindingTargets(processJobId);
                }

                _processJobRepository.Remove(processJobId);
                _relationRepository.UnlinkProcessJob(processJobId);
            }

            _controlJobRepository.Remove(controlJobId);
            _relationRepository.UnlinkControlJob(controlJobId);

            return controlJobRemoveResult;
        }
        //private long RemoveLinkedProcessJobsThenControlJob(string controlJobId)
        //{
        //    var processJobIds = _relationRepository.GetProcessJobIds(controlJobId);
        //    var binder = SubstrateJobBinderLocator.Instance;

        //    // relation이 살아 있을 때 ControlJob 기준 unbind 먼저 수행
        //    if (binder != null)
        //        binder.UnbindByControlJob(controlJobId);

        //    foreach (var processJobId in processJobIds)
        //    {
        //        // ProcessJob repository가 살아 있을 때 ProcessJob 기준 unbind
        //        if (binder != null)
        //            binder.UnbindByProcessJob(processJobId);

        //        var processJobRemoveResult = _gem300Service.ProcessJob.Remove(processJobId);

        //        if (!_resultEvaluator.IsSuccess(processJobRemoveResult))
        //            return processJobRemoveResult;

        //        _processJobRepository.Remove(processJobId);
        //        _relationRepository.UnlinkProcessJob(processJobId);
        //    }

        //    var controlJobRemoveResult = _gem300Service.ControlJob.Remove(controlJobId);

        //    if (_resultEvaluator.IsSuccess(controlJobRemoveResult))
        //    {
        //        _controlJobRepository.Remove(controlJobId);
        //        _relationRepository.UnlinkControlJob(controlJobId);
        //    }

        //    return controlJobRemoveResult;
        //}
        public long RemoveProcessJob(string processJobId)
        {
            if (string.IsNullOrWhiteSpace(processJobId))
                throw new ArgumentException("ProcessJobId is invalid.", nameof(processJobId));

            var controlJobId = _relationRepository.GetControlJobIdOrDefault(processJobId);

            var result = _gem300Service.ProcessJob.Remove(processJobId);
            if (_resultEvaluator.IsSuccess(result))
            {
                // 추가:
                // Repository에서 ProcessJob을 제거하기 전에 Unbind한다.
                // Binder가 ProcessJob.MaterialInfo를 읽어야 Substrate 위치를 찾을 수 있기 때문이다.
                if (SubstrateJobBindingService.Instance != null)
                {
                    SubstrateJobBindingService.Instance.UnbindByProcessJob(processJobId);

                    /*
                     * ProcessJob 자체가 제거되면
                     * 해당 Job에 대해 저장된 removed binding target도 더 이상 의미가 없다.
                     */
                    SubstrateJobBindingService.Instance.ClearRemovedBindingTargets(processJobId);
                }


                _processJobRepository.Remove(processJobId);
                _relationRepository.UnlinkProcessJob(processJobId);

                if (!string.IsNullOrWhiteSpace(controlJobId))
                {
                    RefreshControlJobProcessJobIdsFromRelation(controlJobId);
                    SynchronizeControlJobProcessJobStatus(controlJobId);
                    RemoveControlJobIfAllProcessJobsTerminalOrRemoved(controlJobId);
                }
            }

            return result;
        }
        private void RemoveControlJobIfAllProcessJobsTerminalOrRemoved(string controlJobId)
        {
            if (string.IsNullOrWhiteSpace(controlJobId))
                return;

            var controlJob = _controlJobRepository.GetOrDefault(controlJobId);

            if (controlJob == null)
                return;

            if (!AreAllProcessJobsTerminalOrRemoved(controlJobId))
                return;

            lock (_autoRemoveLock)
            {
                if (_autoRemovingControlJobIds.Contains(controlJobId))
                    return;

                _autoRemovingControlJobIds.Add(controlJobId);
            }

            try
            {
                controlJob.ChangeState(ControlJobState.Completed);
                _controlJobRepository.AddOrUpdate(controlJob);

                RemoveControlJob(
                    controlJobId,
                    ControlJobRemoveMode.RemoveLinkedProcessJobs);
            }
            finally
            {
                lock (_autoRemoveLock)
                {
                    _autoRemovingControlJobIds.Remove(controlJobId);
                }
            }
        }
        public long RemoveAllControlJobs(ControlJobRemoveMode removeMode)
        {
            if (removeMode == ControlJobRemoveMode.RejectIfProcessJobsExist)
            {
                var controlJobs = _controlJobRepository.GetAll();

                foreach (var controlJob in controlJobs)
                {
                    if (controlJob == null)
                        continue;

                    if (_relationRepository.HasLinkedProcessJobs(controlJob.Id))
                    {
                        throw new InvalidOperationException(
                            "Cannot remove all ControlJobs because one or more ControlJobs have linked ProcessJobs.");
                    }
                }

                var result = _gem300Service.ControlJob.RemoveAll();

                if (_resultEvaluator.IsSuccess(result))
                {
                    _controlJobRepository.Clear();
                    _relationRepository.Clear();
                }

                return result;
            }

            if (removeMode == ControlJobRemoveMode.RemoveLinkedProcessJobs)
            {
                return RemoveAllControlJobsWithLinkedProcessJobsOnly();
            }

            throw new ArgumentOutOfRangeException(nameof(removeMode), removeMode, null);
        }
        public long RemoveAllProcessJobs()
        {
            var processRemoveResult = _gem300Service.ProcessJob.RemoveAll();

            if (!_resultEvaluator.IsSuccess(processRemoveResult))
                return processRemoveResult;

            var controlRemoveResult = _gem300Service.ControlJob.RemoveAll();

            if (!_resultEvaluator.IsSuccess(controlRemoveResult))
                return controlRemoveResult;

            var binder = SubstrateJobBindingService.Instance;

            if (binder != null)
            {
                var controlJobs = _controlJobRepository.GetAll();

                foreach (var controlJob in controlJobs)
                {
                    if (controlJob == null)
                        continue;

                    binder.UnbindByControlJob(controlJob.Id);
                }

                var processJobs = _processJobRepository.GetAll();

                foreach (var processJob in processJobs)
                {
                    if (processJob == null)
                        continue;

                    binder.UnbindByProcessJob(processJob.Id);
                }
            }

            if (binder != null)
            {
                var processJobs = _processJobRepository.GetAll();

                foreach (var processJob in processJobs)
                {
                    if (processJob == null)
                        continue;

                    binder.ClearRemovedBindingTargets(processJob.Id);
                }
            }

            _processJobRepository.Clear();
            _controlJobRepository.Clear();
            _relationRepository.Clear();

            return controlRemoveResult;
        }
        #endregion </Remove>

        #region <Query>
        public ControlJob GetControlJobOrDefault(string controlJobId)
        {
            return _controlJobRepository.GetOrDefault(controlJobId);
        }
        public string GetControlJobIdOrDefault(string processJobId)
        {
            return _relationRepository.GetControlJobIdOrDefault(processJobId);
        }
        public IReadOnlyList<ControlJob> GetAllControlJobs()
        {
            return _controlJobRepository.GetAll();
        }
        public ProcessJob GetProcessJobOrDefault(string processJobId)
        {
            return _processJobRepository.GetOrDefault(processJobId);
        }
        public string[] GetProcessJobIds(string controlJobId)
        {
            return _relationRepository.GetProcessJobIds(controlJobId);
        }
        public IReadOnlyList<ProcessJob> GetAllProcessJobs()
        {
            return _processJobRepository.GetAll();
        }
        public IReadOnlyList<ProcessJob> GetLinkedProcessJobs(string controlJobId)
        {
            var processJobs = new List<ProcessJob>();

            if (string.IsNullOrWhiteSpace(controlJobId))
                return processJobs;

            ControlJob controlJob =
                _controlJobRepository.GetOrDefault(controlJobId);

            string[] processJobIds;

            if (controlJob != null)
                processJobIds = GetProcessJobIdsByControlJobPolicy(controlJob);
            else
                processJobIds = _relationRepository.GetProcessJobIds(controlJobId) ?? new string[0];

            foreach (var processJobId in processJobIds)
            {
                if (string.IsNullOrWhiteSpace(processJobId))
                    continue;

                var processJob = _processJobRepository.GetOrDefault(processJobId);

                if (processJob != null)
                    processJobs.Add(processJob);
            }

            return processJobs;
        }
        public ControlJob GetActiveControlJobOrDefault()
        {
            var controlJobs = _controlJobRepository.GetAll();

            foreach (var state in ActiveControlJobPriority)
            {
                var job = FindFirstControlJobByState(controlJobs, state);

                if (job != null)
                    return job;
            }

            return null;
        }
        public ControlJob GetWorkingControlJobOrDefault()
        {
            var activeControlJob = GetActiveControlJobOrDefault();

            if (activeControlJob != null)
                return activeControlJob;

            return GetHeadOfQueueControlJobOrDefault();
        }
        public ControlJob GetHeadOfQueueControlJobOrDefault()
        {
            int queueHeadIndex = GetControlJobQueueHeadIndex();

            if (queueHeadIndex < 0)
                return null;

            var controlJobIds = _controlJobRepository.GetOrderedIds();

            if (queueHeadIndex >= controlJobIds.Count)
                return null;

            return _controlJobRepository.GetOrDefault(controlJobIds[queueHeadIndex]);
        }

        public bool IsHeadOfQueueControlJob(string controlJobId)
        {
            if (string.IsNullOrWhiteSpace(controlJobId))
                return false;

            var job = _controlJobRepository.GetOrDefault(controlJobId);

            if (job == null)
                return false;

            if (!IsQueuedControlJob(job.State))
                return false;

            int currentIndex = _controlJobRepository.IndexOf(controlJobId);
            int queueHeadIndex = GetControlJobQueueHeadIndex();

            return currentIndex >= 0 && currentIndex == queueHeadIndex;
        }
        public ProcessJob GetActiveProcessJobOrDefault()
        {
            var activeControlJob = GetActiveControlJobOrDefault();

            if (activeControlJob != null)
            {
                var linkedProcessJobs = GetLinkedProcessJobs(activeControlJob.Id);
                var linkedActiveProcessJob = FindFirstProcessJobByPriority(linkedProcessJobs);

                if (linkedActiveProcessJob != null)
                    return linkedActiveProcessJob;
            }

            var processJobs = _processJobRepository.GetAll();

            return FindFirstProcessJobByPriority(processJobs);
        }
        public ProcessJob GetWorkingProcessJobOrDefault()
        {
            var activeProcessJob = GetActiveProcessJobOrDefault();

            if (activeProcessJob != null)
                return activeProcessJob;

            var workingControlJob = GetWorkingControlJobOrDefault();

            if (workingControlJob == null)
                return null;

            var linkedProcessJobs = GetLinkedProcessJobs(workingControlJob.Id);

            foreach (var processJob in linkedProcessJobs)
            {
                if (processJob == null)
                    continue;

                if (processJob.State == ProcessJobState.JobQueued)
                    return processJob;
            }

            return null;
        }
        public ControlJob GetControlJobByCarrierInputIdOrDefault(string carrierId)
        {
            if (string.IsNullOrWhiteSpace(carrierId))
                return null;

            var controlJobs = _controlJobRepository.GetAll();

            foreach (var controlJob in controlJobs)
            {
                if (controlJob == null)
                    continue;

                if (ContainsCarrierInputId(controlJob, carrierId))
                    return controlJob;
            }

            return null;
        }

        public IReadOnlyList<ControlJob> GetControlJobsByCarrierInputId(string carrierId)
        {
            var result = new List<ControlJob>();

            if (string.IsNullOrWhiteSpace(carrierId))
                return result;

            var controlJobs = _controlJobRepository.GetAll();

            foreach (var controlJob in controlJobs)
            {
                if (controlJob == null)
                    continue;

                if (ContainsCarrierInputId(controlJob, carrierId))
                    result.Add(controlJob);
            }

            return result;
        }

        public ControlJob GetControlJobByCarrierOutputSpecValueOrDefault(string carrierId)
        {
            if (string.IsNullOrWhiteSpace(carrierId))
                return null;

            var controlJobs = _controlJobRepository.GetAll();

            foreach (var controlJob in controlJobs)
            {
                if (controlJob == null)
                    continue;

                if (ContainsCarrierOutputSpecValue(controlJob, carrierId))
                    return controlJob;
            }

            return null;
        }

        public IReadOnlyList<ControlJob> GetControlJobsByCarrierOutputSpecValue(string carrierId)
        {
            var result = new List<ControlJob>();

            if (string.IsNullOrWhiteSpace(carrierId))
                return result;

            var controlJobs = _controlJobRepository.GetAll();

            foreach (var controlJob in controlJobs)
            {
                if (controlJob == null)
                    continue;

                if (ContainsCarrierOutputSpecValue(controlJob, carrierId))
                    result.Add(controlJob);
            }

            return result;
        }
        public ProcessJob GetActiveProcessJobOrDefault(string controlJobId)
        {
            if (string.IsNullOrWhiteSpace(controlJobId))
                return null;

            var controlJob = _controlJobRepository.GetOrDefault(controlJobId);

            if (controlJob == null)
                return null;

            var linkedProcessJobs = GetLinkedProcessJobs(controlJobId);

            return FindFirstProcessJobByPriority(linkedProcessJobs);
        }
        public ProcessJob GetProcessingProcessJobOrDefault(string controlJobId)
        {
            if (string.IsNullOrWhiteSpace(controlJobId))
                return null;

            var linkedProcessJobs = GetLinkedProcessJobs(controlJobId);

            foreach (var processJob in linkedProcessJobs)
            {
                if (processJob == null)
                    continue;

                if (processJob.State == ProcessJobState.Processing)
                    return processJob;
            }

            return null;
        }
        #endregion </Query>

        #region <ControlJob Callback>
        public void OnCreated(ControlJobCreatedEventArgs e)
        {
            if (e == null || e.Job == null)
                return;

            var jobInfo = e.Job;

            if (string.IsNullOrWhiteSpace(jobInfo.ControlJobId))
                return;

            var job = _controlJobRepository.GetOrDefault(jobInfo.ControlJobId);
            if (job == null)
            {
                job = new ControlJob(
                    jobInfo.ControlJobId,
                    jobInfo.State,
                    jobInfo.StartMode,
                    jobInfo.ProcessJobIds,
                    jobInfo.CurrentProcessJobIds,
                    jobInfo.DataCollectionPlan,
                    jobInfo.CarrierInputIds,
                    jobInfo.MaterialOutputSpecifications,
                    jobInfo.MaterialOutputByStatus,
                    jobInfo.PauseEventIds,
                    jobInfo.ProcessJobStates,
                    jobInfo.ProcessingControlSpecifications,
                    jobInfo.ProcessOrderManagement);

                _controlJobRepository.AddOrUpdate(job);
            }
            else
            {
                job.ChangeAttributeInfoExceptState(
                    jobInfo.StartMode,
                    jobInfo.ProcessJobIds,
                    jobInfo.CurrentProcessJobIds,
                    jobInfo.DataCollectionPlan,
                    jobInfo.CarrierInputIds,
                    jobInfo.MaterialOutputSpecifications,
                    jobInfo.MaterialOutputByStatus,
                    jobInfo.PauseEventIds,
                    jobInfo.ProcessJobStates,
                    jobInfo.ProcessingControlSpecifications,
                    jobInfo.ProcessOrderManagement);

                _controlJobRepository.AddOrUpdate(job);
            }

            if (_relationRepository.CanLink(jobInfo.ControlJobId, jobInfo.ProcessJobIds))
            {
                _relationRepository.Link(jobInfo.ControlJobId, jobInfo.ProcessJobIds);

                SynchronizeControlJobProcessJobStatus(jobInfo.ControlJobId);

                if (SubstrateJobBindingService.Instance != null)
                    SubstrateJobBindingService.Instance.BindByControlJob(jobInfo.ControlJobId);
            }
            else
            {
                Console.WriteLine(
                    "[{0}] JobManager relation link skipped. ControlJobId={1}, ProcessJobIds=[{2}]",
                    DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff"),
                    jobInfo.ControlJobId,
                    jobInfo.ProcessJobIds == null ? string.Empty : string.Join(",", jobInfo.ProcessJobIds));
            }
        }
        public void OnStateChanged(ControlJobStateChangedEventArgs e)
        {
            if (e == null || string.IsNullOrWhiteSpace(e.ControlJobId))
                return;

            var job = _controlJobRepository.GetOrDefault(e.ControlJobId);

            if (job == null)
                return;

            job.ChangeState(e.State);
            _controlJobRepository.AddOrUpdate(job);
        }
        public void OnDeleted(ControlJobDeletedEventArgs e)
        {
            if (e == null || string.IsNullOrWhiteSpace(e.ControlJobId))
                return;

            string controlJobId = e.ControlJobId;

            var job = _controlJobRepository.GetOrDefault(controlJobId);

            if (job != null)
                job.ChangeState(ControlJobState.Deleted);

            string[] processJobIds =
                _relationRepository.GetProcessJobIds(controlJobId);

            var binder = SubstrateJobBindingService.Instance;

            if (binder != null)
                binder.UnbindByControlJob(controlJobId);

            // ControlJob 삭제 시점에 연결 ProcessJob도 로컬에서 정리한다.
            if (processJobIds != null)
            {
                foreach (var processJobId in processJobIds)
                {
                    if (string.IsNullOrWhiteSpace(processJobId))
                        continue;

                    if (binder != null)
                    {
                        binder.UnbindByProcessJob(processJobId);
                        binder.ClearRemovedBindingTargets(processJobId);
                    }

                    _processJobRepository.Remove(processJobId);
                    _relationRepository.UnlinkProcessJob(processJobId);
                }
            }

            _controlJobRepository.Remove(controlJobId);
            _relationRepository.UnlinkControlJob(controlJobId);
        }
        //public void OnDeleted(ControlJobDeletedEventArgs e)
        //{
        //    if (e == null || string.IsNullOrWhiteSpace(e.ControlJobId))
        //        return;

        //    var job = _controlJobRepository.GetOrDefault(e.ControlJobId);

        //    if (job != null)
        //        job.ChangeState(ControlJobState.Deleted);

        //    // Repository/Relation을 제거하기 전에 Unbind한다.
        //    // UnbindByControlJob은 연결된 ProcessJob 정보를 relation에서 조회해야 할 수 있다.
        //    if (SubstrateJobBinderLocator.Instance != null)
        //        SubstrateJobBinderLocator.Instance.UnbindByControlJob(e.ControlJobId);

        //    _controlJobRepository.Remove(e.ControlJobId);
        //    _relationRepository.UnlinkControlJob(e.ControlJobId);
        //}
        public void OnVerifyRequestedByHost(ControlJobVerifyRequestedEventArgs e)
        {
            if (e == null)
                return;

            if (string.IsNullOrWhiteSpace(e.ControlJobId))
            {
                _gem300Service.ControlJob.AcknowledgeVerify(
                    e.MessageId,
                    e.ControlJobId,
                    JobAcknowledgeResult.Failure,
                    CreateErrorCodes(JobAcknowledgeError.InvalidControlJobId),
                    CreateErrorTexts(JobAcknowledgeError.InvalidControlJobId));
                return;
            }

            if (_controlJobRepository.Contains(e.ControlJobId))
            {
                _gem300Service.ControlJob.AcknowledgeVerify(
                    e.MessageId,
                    e.ControlJobId,
                    JobAcknowledgeResult.Failure,
                    CreateErrorCodes(JobAcknowledgeError.ControlJobAlreadyExists),
                    CreateErrorTexts(JobAcknowledgeError.ControlJobAlreadyExists));
                return;
            }

            if (e.ProcessJobIds == null || e.ProcessJobIds.Length == 0)
            {
                _gem300Service.ControlJob.AcknowledgeVerify(
                    e.MessageId,
                    e.ControlJobId,
                    JobAcknowledgeResult.Failure,
                    CreateErrorCodes(JobAcknowledgeError.NoLinkedProcessJob),
                    CreateErrorTexts(JobAcknowledgeError.NoLinkedProcessJob));
                return;
            }

            if (!_relationRepository.CanLink(e.ControlJobId, e.ProcessJobIds))
            {
                _gem300Service.ControlJob.AcknowledgeVerify(
                    e.MessageId,
                    e.ControlJobId,
                    JobAcknowledgeResult.Failure,
                    CreateErrorCodes(JobAcknowledgeError.ProcessJobAlreadyLinked),
                    CreateErrorTexts(JobAcknowledgeError.ProcessJobAlreadyLinked));
                return;
            }

            foreach (var processJobId in e.ProcessJobIds)
            {
                if (string.IsNullOrWhiteSpace(processJobId))
                {
                    _gem300Service.ControlJob.AcknowledgeVerify(
                        e.MessageId,
                        e.ControlJobId,
                        JobAcknowledgeResult.Failure,
                        CreateErrorCodes(JobAcknowledgeError.InvalidProcessJobId),
                        CreateErrorTexts(JobAcknowledgeError.InvalidProcessJobId));
                    return;
                }

                if (!_processJobRepository.Contains(processJobId))
                {
                    _gem300Service.ControlJob.AcknowledgeVerify(
                        e.MessageId,
                        e.ControlJobId,
                        JobAcknowledgeResult.Failure,
                        CreateErrorCodes(JobAcknowledgeError.LinkedProcessJobNotFound),
                        CreateErrorTexts(JobAcknowledgeError.LinkedProcessJobNotFound));
                    return;
                }
            }

            var controlJob = new ControlJob(
                e.ControlJobId,
                ControlJobState.Queued,
                e.StartMode,
                e.ProcessJobIds);

            _controlJobRepository.AddOrUpdate(controlJob);
            _relationRepository.Link(e.ControlJobId, e.ProcessJobIds);

            SynchronizeControlJobProcessJobStatus(e.ControlJobId);

            if (SubstrateJobBindingService.Instance != null)
                SubstrateJobBindingService.Instance.BindByControlJob(e.ControlJobId);

            _gem300Service.ControlJob.AcknowledgeVerify(
                e.MessageId,
                e.ControlJobId,
                JobAcknowledgeResult.Success,
                new long[0],
                new string[0]);
        }
        public void OnCommandRequestedByHost(ControlJobCommandRequestedEventArgs e)
        {
            if (e == null)
                return;

            if (string.IsNullOrWhiteSpace(e.ControlJobId))
            {
                _gem300Service.ControlJob.AcknowledgeCommand(
                    e.MessageId,
                    e.ControlJobId,
                    e.Command,
                    JobAcknowledgeResult.Failure,
                    CreateErrorCodes(JobAcknowledgeError.InvalidControlJobId),
                    CreateErrorTexts(JobAcknowledgeError.InvalidControlJobId));
                return;
            }

            var job = _controlJobRepository.GetOrDefault(e.ControlJobId);
            if (job == null)
            {
                _gem300Service.ControlJob.AcknowledgeCommand(
                    e.MessageId,
                    e.ControlJobId,
                    e.Command,
                    JobAcknowledgeResult.Failure,
                    CreateErrorCodes(JobAcknowledgeError.ControlJobNotFound),
                    CreateErrorTexts(JobAcknowledgeError.ControlJobNotFound));
                return;
            }

            // 지금은 존재 여부만 확인하고 허용.
            // 단, Start 계열 command라면 Substrate에 Job 정보가 모두 바인딩되었는지 확인한다.
            //
            // 주의:
            // 이 검사는 상태 전이가 아니라 "실행 허가 전 검증"이다.
            // 실제 상태 전이는 기존 ControlJob 흐름에서 처리한다.
            if (IsStartCommand(e.Command))
            {
                bool bound =
                    SubstrateJobBindingService.Instance == null ||
                    SubstrateJobBindingService.Instance.IsBoundForControlJob(e.ControlJobId);

                if (!bound)
                {
                    _gem300Service.ControlJob.AcknowledgeCommand(
                        e.MessageId,
                        e.ControlJobId,
                        e.Command,
                        JobAcknowledgeResult.Failure,
                        CreateErrorCodes(JobAcknowledgeError.InvalidMaterial),
                        CreateErrorTexts(JobAcknowledgeError.InvalidMaterial));

                    return;
                }
            }

            // 지금은 존재 여부만 확인하고 허용.
            // 나중에 command별 상태 전이 정책 추가.
            _gem300Service.ControlJob.AcknowledgeCommand(
                e.MessageId,
                e.ControlJobId,
                e.Command,
                JobAcknowledgeResult.Success,
                new long[0],
                new string[0]);
        }
        public void OnManualStartRequired(ControlJobManualStartEventArgs e)
        {
            if (e == null || string.IsNullOrWhiteSpace(e.ControlJobId))
                return;

            var job = _controlJobRepository.GetOrDefault(e.ControlJobId);

            if (job == null)
                return;

            job.ChangeState(ControlJobState.WaitingForStart);
            _controlJobRepository.AddOrUpdate(job);
        }
        public void OnManualStartRequired(ProcessJobManualStartEventArgs e)
        {
            if (e == null || string.IsNullOrWhiteSpace(e.ProcessJobId))
                return;

            ChangeProcessJobStateLocal(
                e.ProcessJobId,
                ProcessJobState.WaitingForStart);
        }
        public void OnHeadOfQueueChanged(ControlJobHoqChangedEventArgs e)
        {
            if (e == null || string.IsNullOrWhiteSpace(e.ControlJobId))
                return;

            var job = _controlJobRepository.GetOrDefault(e.ControlJobId);

            if (job == null)
                return;

            if (!IsQueuedControlJob(job.State))
                return;

            int currentIndex = _controlJobRepository.IndexOf(e.ControlJobId);
            int targetIndex = GetControlJobQueueHeadIndex();

            if (currentIndex < 0 || targetIndex < 0)
                return;

            if (currentIndex >= 0 && currentIndex != targetIndex)
                _controlJobRepository.MoveToIndex(e.ControlJobId, targetIndex);

            //job.ChangeState(ControlJobState.HeadOfQueue);
            //_controlJobRepository.AddOrUpdate(job);

            Console.WriteLine(
                "[{0}] JobManager HOQ changed. ControlJobId={1}, CurrentIndex={2}, TargetIndex={3}",
                DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff"),
                e.ControlJobId,
                currentIndex,
                targetIndex);
        }
        #endregion </ControlJob Callback>

        #region <ProcessJob Callback>
        public void OnCreated(ProcessJobCreatedEventArgs e)
        {
            if (e == null || e.Job == null)
                return;

            var jobInfo = e.Job;

            if (string.IsNullOrWhiteSpace(jobInfo.ProcessJobId))
                return;

            if (!IsValidMaterialInfo(jobInfo.MaterialInfo))
            {
                Console.WriteLine(
                    "[{0}] JobManager ProcessJob created callback ignored. Invalid MaterialInfo. ProcessJobId={1}",
                    DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff"),
                    jobInfo.ProcessJobId);

                return;
            }

            var parameterNames = ToRecipeParameterNames(jobInfo.RecipeParameters);
            var parameterValues = ToRecipeParameterValues(jobInfo.RecipeParameters);

            var job = _processJobRepository.GetOrDefault(jobInfo.ProcessJobId);

            if (job == null)
            {
                job = new ProcessJob(
                    jobInfo.ProcessJobId,
                    ProcessJobState.JobQueued,
                    jobInfo.MaterialFormat,
                    jobInfo.StartMode,
                    jobInfo.MaterialOrder,
                    jobInfo.MaterialInfo,
                    jobInfo.RecipeMethod,
                    jobInfo.RecipeId,
                    parameterNames,
                    parameterValues,
                    jobInfo.PauseEventIds);

                _processJobRepository.AddOrUpdate(job);
            }
            else
            {
                /*
                 * 기존 ProcessJob이 이미 Substrate에 바인딩되어 있을 수 있으므로,
                 * MaterialInfo를 새 값으로 덮어쓰기 전에 기존 바인딩을 먼저 제거한다.
                 */
                if (SubstrateJobBindingService.Instance != null)
                    SubstrateJobBindingService.Instance.UnbindByProcessJob(jobInfo.ProcessJobId);

                job.ChangeStartMode(jobInfo.StartMode);
                job.ChangeMaterialOrder(jobInfo.MaterialOrder);
                job.ChangeMaterial(
                    jobInfo.MaterialFormat,
                    jobInfo.MaterialInfo);
                job.ChangeRecipeInfo(
                    jobInfo.RecipeMethod,
                    jobInfo.RecipeId,
                    parameterNames,
                    parameterValues);
                job.ChangePauseEventIds(jobInfo.PauseEventIds);

                _processJobRepository.AddOrUpdate(job);
            }

            if (SubstrateJobBindingService.Instance != null)
                SubstrateJobBindingService.Instance.BindByProcessJob(jobInfo.ProcessJobId);
        }
        public void OnStateChanged(ProcessJobStateChangedEventArgs e)
        {
            if (e == null || string.IsNullOrWhiteSpace(e.ProcessJobId))
                return;

            var job = _processJobRepository.GetOrDefault(e.ProcessJobId);

            if (job == null)
                return;

            /*
             * 1. ProcessJob 상태 갱신.
             * ProcessJob.State가 상태의 원천이다.
             */
            job.ChangeState(e.State);
            _processJobRepository.AddOrUpdate(job);

            var controlJobId =
                _relationRepository.GetControlJobIdOrDefault(e.ProcessJobId);

            if (string.IsNullOrWhiteSpace(controlJobId))
                return;

            var controlJob =
                _controlJobRepository.GetOrDefault(controlJobId);

            if (controlJob == null)
                return;

            /*
             * 2. 중요:
             * ProcessJob 상태가 바뀌었으므로,
             * ControlJob 내부의 PRJobStatusList 성격인 ProcessJobStatus도 재계산한다.
             */
            SynchronizeControlJobProcessJobStatus(controlJobId);

            /*
             * 3. 기존 상태별 후처리 유지.
             */
            switch (e.State)
            {
                case ProcessJobState.SettingUp:
                    {
                        /*
                         * TODO:
                         * 실제 장비 SettingUp 완료 시점이 따로 있다면
                         * 여기서 즉시 완료 처리하지 않는 것이 더 정확하다.
                         */
                        //NotifyProcessJobSettingUpCompleted(e.ProcessJobId);
                    }
                    break;

                default:
                    break;
            }

            /*
             * 4. 모든 ProcessJob이 terminal 상태라면 ControlJob 완료/삭제 처리.
             */
            RemoveControlJobIfAllProcessJobsTerminalOrRemoved(controlJobId);
        }
        public void OnDeleted(ProcessJobDeletedEventArgs e)
        {
            if (e == null || string.IsNullOrWhiteSpace(e.ProcessJobId))
                return;

            string controlJobId =
                _relationRepository.GetControlJobIdOrDefault(e.ProcessJobId);

            ProcessJob processJob =
                _processJobRepository.GetOrDefault(e.ProcessJobId);

            bool shouldRemoveLocal =
                _processJobRemovalPolicy.ShouldRemoveLocalOnSdkDeleted(
                    processJob,
                    controlJobId);

            if (!shouldRemoveLocal)
            {
                // SDK에서는 삭제됐지만, 로컬에서는 ControlJob 제거 시점까지 유지한다.
                if (!string.IsNullOrWhiteSpace(controlJobId))
                {
                    SynchronizeControlJobProcessJobStatus(controlJobId);
                    RemoveControlJobIfAllProcessJobsTerminalOrRemoved(controlJobId);
                }

                return;
            }

            // 기존 정책 또는 ControlJob에 연결되지 않은 ProcessJob은 로컬에도 삭제 반영
            if (SubstrateJobBindingService.Instance != null)
            {
                SubstrateJobBindingService.Instance.UnbindByProcessJob(e.ProcessJobId);
                SubstrateJobBindingService.Instance.ClearRemovedBindingTargets(e.ProcessJobId);
            }

            if (_processJobRepository.Contains(e.ProcessJobId))
                _processJobRepository.Remove(e.ProcessJobId);

            _relationRepository.UnlinkProcessJob(e.ProcessJobId);

            if (!string.IsNullOrWhiteSpace(controlJobId))
                RemoveControlJobIfAllProcessJobsTerminalOrRemoved(controlJobId);
        }

        /*
         * Verify 이후 SDK가 ProcessJobCreated callback을 발생시킨다.
         * 실제 Substrate binding은 OnCreated(ProcessJobCreatedEventArgs)에서 수행한다.
         */
        public void OnVerifyRequestedByHost(ProcessJobVerifyRequestedEventArgs e)
        {
            if (e == null)
                return;

            if (e.Jobs == null || e.Jobs.Count == 0)
            {
                _gem300Service.ProcessJob.AcknowledgeVerify(
                    e.MessageId,
                    new string[0],
                    JobAcknowledgeResult.Failure,
                    CreateErrorCodes(JobAcknowledgeError.InvalidProcessJobId),
                    CreateErrorTexts(JobAcknowledgeError.InvalidProcessJobId));
                return;
            }

            var processJobIds = new string[e.Jobs.Count];

            for (int i = 0; i < e.Jobs.Count; ++i)
            {
                var jobInfo = e.Jobs[i];

                if (jobInfo == null || string.IsNullOrWhiteSpace(jobInfo.ProcessJobId))
                {
                    _gem300Service.ProcessJob.AcknowledgeVerify(
                        e.MessageId,
                        processJobIds,
                        JobAcknowledgeResult.Failure,
                        CreateErrorCodes(JobAcknowledgeError.InvalidProcessJobId),
                        CreateErrorTexts(JobAcknowledgeError.InvalidProcessJobId));
                    return;
                }

                processJobIds[i] = jobInfo.ProcessJobId;

                if (_processJobRepository.Contains(jobInfo.ProcessJobId))
                {
                    _gem300Service.ProcessJob.AcknowledgeVerify(
                        e.MessageId,
                        processJobIds,
                        JobAcknowledgeResult.Failure,
                        CreateErrorCodes(JobAcknowledgeError.ProcessJobAlreadyExists),
                        CreateErrorTexts(JobAcknowledgeError.ProcessJobAlreadyExists));
                    return;
                }

                if (string.IsNullOrWhiteSpace(jobInfo.RecipeId))
                {
                    _gem300Service.ProcessJob.AcknowledgeVerify(
                        e.MessageId,
                        processJobIds,
                        JobAcknowledgeResult.Failure,
                        CreateErrorCodes(JobAcknowledgeError.InvalidRecipeId),
                        CreateErrorTexts(JobAcknowledgeError.InvalidRecipeId));
                    return;
                }

                if (!IsValidMaterialInfo(jobInfo.MaterialInfo))
                {
                    _gem300Service.ProcessJob.AcknowledgeVerify(
                        e.MessageId,
                        processJobIds,
                        JobAcknowledgeResult.Failure,
                        CreateErrorCodes(JobAcknowledgeError.InvalidMaterial),
                        CreateErrorTexts(JobAcknowledgeError.InvalidMaterial));
                    return;
                }
            }

            for (int i = 0; i < e.Jobs.Count; ++i)
            {
                var jobInfo = e.Jobs[i];

                var processJob = new ProcessJob(
                    jobInfo.ProcessJobId,
                    ProcessJobState.JobQueued,
                    jobInfo.MaterialFormat,
                    jobInfo.StartMode,
                    jobInfo.MaterialOrder,
                    jobInfo.MaterialInfo,
                    jobInfo.RecipeMethod,
                    jobInfo.RecipeId,
                    ToRecipeParameterNames(jobInfo.RecipeParameters),
                    ToRecipeParameterValues(jobInfo.RecipeParameters));

                _processJobRepository.AddOrUpdate(processJob);
            }

            _gem300Service.ProcessJob.AcknowledgeVerify(
                e.MessageId,
                processJobIds,
                JobAcknowledgeResult.Success,
                new long[0],
                new string[0]);
        }
        public void OnCommandRequestedByHost(ProcessJobCommandRequestedEventArgs e)
        {
            if (e == null)
                return;

            if (string.IsNullOrWhiteSpace(e.ProcessJobId))
            {
                _gem300Service.ProcessJob.AcknowledgeCommand(
                    e.MessageId,
                    e.Command,
                    e.ProcessJobId,
                    JobAcknowledgeResult.Failure,
                    CreateErrorCodes(JobAcknowledgeError.InvalidProcessJobId),
                    CreateErrorTexts(JobAcknowledgeError.InvalidProcessJobId));
                return;
            }

            var job = _processJobRepository.GetOrDefault(e.ProcessJobId);

            if (job == null)
            {
                _gem300Service.ProcessJob.AcknowledgeCommand(
                    e.MessageId,
                    e.Command,
                    e.ProcessJobId,
                    JobAcknowledgeResult.Failure,
                    CreateErrorCodes(JobAcknowledgeError.ProcessJobNotFound),
                    CreateErrorTexts(JobAcknowledgeError.ProcessJobNotFound));
                return;
            }

            // 지금은 존재 여부만 확인하고 허용.
            // 나중에 command별 상태 전이 정책 추가.
            _gem300Service.ProcessJob.AcknowledgeCommand(
                e.MessageId,
                e.Command,
                e.ProcessJobId,
                JobAcknowledgeResult.Success,
                new long[0],
                new string[0]);
        }
        public void OnRecipeVariablesRequestedByHost(ProcessJobRecipeVariableRequestedEventArgs e)
        {
            if (e == null)
                return;

            if (string.IsNullOrWhiteSpace(e.ProcessJobId))
            {
                _gem300Service.ProcessJob.AcknowledgeRecipeVariables(
                    e.MessageId,
                    e.ProcessJobId,
                    JobAcknowledgeResult.Failure,
                    CreateErrorCodes(JobAcknowledgeError.InvalidProcessJobId),
                    CreateErrorTexts(JobAcknowledgeError.InvalidProcessJobId));
                return;
            }

            var job = _processJobRepository.GetOrDefault(e.ProcessJobId);

            if (job == null)
            {
                _gem300Service.ProcessJob.AcknowledgeRecipeVariables(
                    e.MessageId,
                    e.ProcessJobId,
                    JobAcknowledgeResult.Failure,
                    CreateErrorCodes(JobAcknowledgeError.ProcessJobNotFound),
                    CreateErrorTexts(JobAcknowledgeError.ProcessJobNotFound));
                return;
            }

            job.ChangeRecipeParameters(
                ToRecipeParameterNames(e.RecipeParameters),
                ToRecipeParameterValues(e.RecipeParameters));

            _processJobRepository.AddOrUpdate(job);

            _gem300Service.ProcessJob.AcknowledgeRecipeVariables(
                e.MessageId,
                e.ProcessJobId,
                JobAcknowledgeResult.Success,
                new long[0],
                new string[0]);
        }
        public void OnStartMethodRequestedByHost(ProcessJobStartMethodRequestedEventArgs e)
        {
            if (e == null)
                return;

            if (e.ProcessJobIds == null || e.ProcessJobIds.Length == 0)
            {
                _gem300Service.ProcessJob.AcknowledgeStartMethod(
                    e.MessageId,
                    new string[0],
                    JobAcknowledgeResult.Failure,
                    CreateErrorCodes(JobAcknowledgeError.InvalidProcessJobId),
                    CreateErrorTexts(JobAcknowledgeError.InvalidProcessJobId));
                return;
            }

            foreach (var processJobId in e.ProcessJobIds)
            {
                if (string.IsNullOrWhiteSpace(processJobId))
                {
                    _gem300Service.ProcessJob.AcknowledgeStartMethod(
                        e.MessageId,
                        e.ProcessJobIds,
                        JobAcknowledgeResult.Failure,
                        CreateErrorCodes(JobAcknowledgeError.InvalidProcessJobId),
                        CreateErrorTexts(JobAcknowledgeError.InvalidProcessJobId));
                    return;
                }

                if (!_processJobRepository.Contains(processJobId))
                {
                    _gem300Service.ProcessJob.AcknowledgeStartMethod(
                        e.MessageId,
                        e.ProcessJobIds,
                        JobAcknowledgeResult.Failure,
                        CreateErrorCodes(JobAcknowledgeError.ProcessJobNotFound),
                        CreateErrorTexts(JobAcknowledgeError.ProcessJobNotFound));
                    return;
                }
            }

            _gem300Service.ProcessJob.AcknowledgeStartMethod(
                e.MessageId,
                e.ProcessJobIds,
                JobAcknowledgeResult.Success,
                new long[0],
                new string[0]);
        }
        public void OnMaterialOrderRequestedByHost(ProcessJobMaterialOrderRequestedEventArgs e)
        {
            if (e == null)
                return;

            // 지금은 기본 허용.
            // 나중에 e.MaterialOrder 값과 장비 상태를 기준으로 검증 추가.
            _gem300Service.ProcessJob.AcknowledgeMaterialOrder(
                e.MessageId,
                JobAcknowledgeResult.Success);
        }
        public void OnSettingUpRequested(ProcessJobSettingUpEventArgs e)
        {
            if (e == null || string.IsNullOrWhiteSpace(e.ProcessJobId))
                return;

            var job = _processJobRepository.GetOrDefault(e.ProcessJobId);
            if (job == null)
                return;

            //// TODO : 수정필요한데..
            //var result = _gem300Service.ProcessJob.NotifySettingUpStarted(e.ProcessJobId);
            //if (!_resultEvaluator.IsSuccess(result))
            //    return;

            //job.ChangeState(ProcessJobState.SettingUp);
            //_processJobRepository.AddOrUpdate(job);
            ///*
            // * SettingUp 요청으로 로컬 상태를 바꿨으므로
            // * ControlJob.ProcessJobStatus도 같이 맞춘다.
            // */
            //SynchronizeControlJobProcessJobStatusByProcessJobId(e.ProcessJobId);
        }
        #endregion </ProcessJob Callback>

        #region <Validation>
        private bool AreAllProcessJobsTerminalOrRemoved(string controlJobId)
        {
            var processJobIds = _relationRepository.GetProcessJobIds(controlJobId);

            if (processJobIds == null || processJobIds.Length == 0)
                return true;

            foreach (var processJobId in processJobIds)
            {
                var processJob = _processJobRepository.GetOrDefault(processJobId);

                if (processJob == null)
                    continue;

                if (!IsTerminalProcessJobState(processJob.State))
                    return false;
            }

            return true;
        }
        private void ValidateLinkedProcessJobsExist(string[] processJobIds)
        {
            if (processJobIds == null || processJobIds.Length == 0)
                throw new ArgumentException("ProcessJobIds are empty.", nameof(processJobIds));

            foreach (var processJobId in processJobIds)
            {
                if (string.IsNullOrWhiteSpace(processJobId))
                    throw new ArgumentException("ProcessJobId is invalid.", nameof(processJobIds));

                if (!_processJobRepository.Contains(processJobId))
                {
                    throw new InvalidOperationException(
                        "Linked ProcessJob does not exist. ProcessJobId=" + processJobId);
                }
            }
        }
        #endregion </Validation>

        #region <State Policy>
        private void RefreshControlJobProcessJobIdsFromRelation(string controlJobId)
        {
            if (string.IsNullOrWhiteSpace(controlJobId))
                return;

            ControlJob controlJob =
                _controlJobRepository.GetOrDefault(controlJobId);

            if (controlJob == null)
                return;

            string[] processJobIds =
                _relationRepository.GetProcessJobIds(controlJobId);

            controlJob.ChangeProcessJobIds(processJobIds ?? new string[0]);
            _controlJobRepository.AddOrUpdate(controlJob);
        }
        private void SynchronizeControlJobProcessJobStatusByProcessJobId(string processJobId)
        {
            if (string.IsNullOrWhiteSpace(processJobId))
                return;

            string controlJobId =
                _relationRepository.GetControlJobIdOrDefault(processJobId);

            if (string.IsNullOrWhiteSpace(controlJobId))
                return;

            SynchronizeControlJobProcessJobStatus(controlJobId);
        }

        private void SynchronizeControlJobProcessJobStatus(
            string controlJobId)
        {
            if (string.IsNullOrWhiteSpace(controlJobId))
                return;

            ControlJob controlJob =
                _controlJobRepository.GetOrDefault(controlJobId);

            if (controlJob == null)
                return;

            /*
             * ProcessJobStatus는 ControlJob이 참조하는 ProcessJobIds 순서로 만든다.
             *
             * 1순위: controlJob.ProcessJobIds
             * 2순위: relationRepository.GetProcessJobIds(controlJobId)
             *
             * 이유:
             * - ControlJob.ProcessJobIds는 ControlJob이 원래 참조하는 ProcessJob 목록이다.
             * - RelationRepository는 삭제/Unlink 시점에 목록이 줄어들 수 있다.
             * - PRJobStatusList 성격이라면 ControlJob이 가진 ProcessJobIds 기준이 더 안정적이다.
             */
            string[] processJobIds = controlJob.ProcessJobIds;

            if (processJobIds == null || processJobIds.Length == 0)
                processJobIds = _relationRepository.GetProcessJobIds(controlJobId);

            if (processJobIds == null)
                processJobIds = new string[0];

            var currentProcessJobIds = new List<string>();
            var processJobStatus = new List<ControlJobProcessJobStatusInfo>();

            for (int i = 0; i < processJobIds.Length; ++i)
            {
                string processJobId = processJobIds[i];

                if (string.IsNullOrWhiteSpace(processJobId))
                    continue;

                ProcessJob processJob =
                    _processJobRepository.GetOrDefault(processJobId);

                ProcessJobState state;

                if (processJob == null)
                {
                    /*
                     * ProcessJob이 이미 삭제된 경우.
                     *
                     * 정책:
                     * - ControlJob.ProcessJobIds의 순서를 유지하기 위해 StatusList에는 남긴다.
                     * - 상태는 JobComplete로 표시한다.
                     *
                     * 삭제된 ProcessJob을 StatusList에서 아예 빼고 싶다면
                     * 이 부분을 continue; 로 바꾸면 된다.
                     */
                    state = ProcessJobState.JobComplete;
                }
                else
                {
                    state = processJob.State;
                }

                processJobStatus.Add(
                    CreateProcessJobStatusInfo(
                        processJobId,
                        state));

                if (IsCurrentProcessJobState(state))
                    currentProcessJobIds.Add(processJobId);
            }

            controlJob.ChangeProcessJobStatus(
                currentProcessJobIds.ToArray(),
                processJobStatus.ToArray());

            _controlJobRepository.AddOrUpdate(controlJob);
        }
        private static ControlJobProcessJobStatusInfo CreateProcessJobStatusInfo(
            string processJobId,
            ProcessJobState state)
        {
            return new ControlJobProcessJobStatusInfo(
                processJobId,
                state);
        }
        private static bool IsCurrentProcessJobState(ProcessJobState state)
        {
            return state == ProcessJobState.SettingUp
                || state == ProcessJobState.WaitingForStart
                || state == ProcessJobState.Processing
                || state == ProcessJobState.Pausing
                || state == ProcessJobState.Paused
                || state == ProcessJobState.Stopping
                || state == ProcessJobState.Aborting;
        }
        private static bool IsValidMaterialInfo(
            IReadOnlyDictionary<string, IReadOnlyList<int>> materialInfo)
        {
            /*
             * MaterialInfo 유효성 정책:
             *
             * 1. 캐리어 + 슬롯 있음
             *    - 정상
             *    - Substrate 바인딩 대상
             *
             * 2. 캐리어 + 슬롯 없음
             *    - 정상
             *    - Carrier 정보는 있지만 특정 Substrate Slot 대상은 없음
             *
             * 3. 캐리어 없음 + 슬롯 없음
             *    - 정상
             *    - 재료/바인딩 대상 없음
             *
             * 4. 캐리어 없음 + 슬롯 있음
             *    - 비정상
             *    - Slot이 어느 Carrier의 Slot인지 알 수 없기 때문
             */
            if (materialInfo == null || materialInfo.Count == 0)
                return true;

            foreach (KeyValuePair<string, IReadOnlyList<int>> item in materialInfo)
            {
                bool hasCarrier = !string.IsNullOrWhiteSpace(item.Key);
                bool hasSlots = item.Value != null && item.Value.Count > 0;

                /*
                 * 캐리어 없음 + 슬롯 있음은 비정상.
                 */
                if (!hasCarrier && hasSlots)
                    return false;

                /*
                 * 캐리어 없음 + 슬롯 없음은 정상.
                 * 의미상 "바인딩 대상 없음"으로 본다.
                 */
                if (!hasCarrier && !hasSlots)
                    continue;

                /*
                 * 캐리어 있음 + 슬롯 없음은 정상.
                 * Carrier 정보는 있지만 Substrate Slot 바인딩 대상은 없다.
                 */
                if (hasCarrier && !hasSlots)
                    continue;

                /*
                 * 캐리어 있음 + 슬롯 있음.
                 * Slot 번호만 검증한다.
                 */
                foreach (int slot in item.Value)
                {
                    if (slot <= 0)
                        return false;
                }
            }

            return true;
        }

        private static bool IsStartCommand(ControlJobCommand command)
        {
            return command == ControlJobCommand.Start;
        }
        private static ControlJob FindFirstControlJobByState(
            IReadOnlyList<ControlJob> controlJobs,
            ControlJobState state)
        {
            if (controlJobs == null || controlJobs.Count == 0)
                return null;

            foreach (var controlJob in controlJobs)
            {
                if (controlJob == null)
                    continue;

                if (controlJob.State == state)
                    return controlJob;
            }

            return null;
        }
        private static ProcessJob FindFirstProcessJobByPriority(
            IReadOnlyList<ProcessJob> processJobs)
        {
            if (processJobs == null || processJobs.Count == 0)
                return null;

            foreach (var state in ActiveProcessJobPriority)
            {
                foreach (var processJob in processJobs)
                {
                    if (processJob == null)
                        continue;

                    if (processJob.State == state)
                        return processJob;
                }
            }

            return null;
        }
        private static bool IsTerminalProcessJobState(ProcessJobState state)
        {
            return state == ProcessJobState.ProcessComplete
                || state == ProcessJobState.Stopped
                || state == ProcessJobState.Aborted
                || state == ProcessJobState.JobCanceled
                || state == ProcessJobState.JobComplete;
        }
        private static bool IsQueuedControlJob(ControlJobState state)
        {
            return state == ControlJobState.Queued;
        }
        private int GetControlJobQueueHeadIndex()
        {
            var controlJobIds = _controlJobRepository.GetOrderedIds();

            for (int i = 0; i < controlJobIds.Count; ++i)
            {
                var controlJob = _controlJobRepository.GetOrDefault(controlJobIds[i]);

                if (controlJob == null)
                    continue;

                if (IsQueuedControlJob(controlJob.State))
                    return i;
            }

            return -1;
        }
        private void ChangeProcessJobStateLocal(
            string processJobId,
            ProcessJobState state)
        {
            if (string.IsNullOrWhiteSpace(processJobId))
                return;

            ProcessJob processJob =
                _processJobRepository.GetOrDefault(processJobId);

            if (processJob == null)
                return;

            processJob.ChangeState(state);
            _processJobRepository.AddOrUpdate(processJob);

            SynchronizeControlJobProcessJobStatusByProcessJobId(processJobId);
        }
        private static void ValidateMaterialInfoOrThrow(
            IReadOnlyDictionary<string, IReadOnlyList<int>> materialInfo)
        {
            if (IsValidMaterialInfo(materialInfo))
                return;

            throw new ArgumentException(
                "MaterialInfo is invalid. Slot information cannot exist without CarrierId.",
                nameof(materialInfo));
        }
        #endregion </State Policy>

        #region <ControlJob Search Policy>
        private static bool ContainsCarrierInputId(
            ControlJob controlJob,
            string carrierId)
        {
            if (controlJob == null)
                return false;

            if (string.IsNullOrWhiteSpace(carrierId))
                return false;

            var carrierInputIds = controlJob.CarrierInputIds;

            if (carrierInputIds == null || carrierInputIds.Length == 0)
                return false;

            foreach (var carrierInputId in carrierInputIds)
            {
                if (string.Equals(
                    carrierInputId,
                    carrierId,
                    StringComparison.Ordinal))
                {
                    return true;
                }
            }

            return false;
        }

        private static bool ContainsCarrierOutputSpecValue(
            ControlJob controlJob,
            string carrierId)
        {
            if (controlJob == null)
                return false;

            if (string.IsNullOrWhiteSpace(carrierId))
                return false;

            var outputSpecifications = controlJob.MaterialOutputSpecifications;

            // OutSpec이 비어 있으면 source carrier로 간다.
            if (outputSpecifications == null || outputSpecifications.Length == 0)
                return ContainsCarrierInputId(controlJob, carrierId);

            bool hasOutputSpecValue = false;

            foreach (var outputSpec in outputSpecifications)
            {
                if (outputSpec == null)
                    continue;

                if (string.IsNullOrWhiteSpace(outputSpec.Value))
                    continue;

                hasOutputSpecValue = true;

                if (string.Equals(
                    outputSpec.Value,
                    carrierId,
                    StringComparison.OrdinalIgnoreCase))
                {
                    return true;
                }
            }

            // 배열은 있지만 값이 전부 비어 있으면 OutSpec 없음으로 본다.
            if (false == hasOutputSpecValue)
                return ContainsCarrierInputId(controlJob, carrierId);

            return false;
        }
        private static bool UsesProcessJobArrayOrder(ControlJob controlJob)
        {
            if (controlJob == null)
                return false;

            return controlJob.ProcessOrderManagement == MaterialOrderMode.Arrival ||
                controlJob.ProcessOrderManagement == MaterialOrderMode.List;
        }

        private string[] GetProcessJobIdsByControlJobPolicy(ControlJob controlJob)
        {
            if (controlJob == null)
                return new string[0];

            if (UsesProcessJobArrayOrder(controlJob) &&
                controlJob.ProcessJobIds != null &&
                controlJob.ProcessJobIds.Length > 0)
            {
                return controlJob.ProcessJobIds;
            }

            return _relationRepository.GetProcessJobIds(controlJob.Id) ?? new string[0];
        }
        #endregion </ControlJob Search Policy>

        #region <Conversion>
        private static string[] ConvertNumericRecipeValues(long[] values)
        {
            if (values == null || values.Length == 0)
                return new string[0];

            var result = new string[values.Length];

            for (int i = 0; i < values.Length; ++i)
                result[i] = values[i].ToString();

            return result;
        }
        private static string[] ToRecipeParameterNames(ProcessRecipeParameter[] parameters)
        {
            if (parameters == null || parameters.Length == 0)
                return new string[0];

            var names = new string[parameters.Length];

            for (int i = 0; i < parameters.Length; ++i)
                names[i] = parameters[i] == null ? string.Empty : parameters[i].Name;

            return names;
        }

        private static string[] ToRecipeParameterValues(ProcessRecipeParameter[] parameters)
        {
            if (parameters == null || parameters.Length == 0)
                return new string[0];

            var values = new string[parameters.Length];

            for (int i = 0; i < parameters.Length; ++i)
                values[i] = parameters[i] == null ? string.Empty : parameters[i].Value;

            return values;
        }
        #endregion </Conversion>

        #region <Acknowledge Error>

        private static long[] CreateErrorCodes(params JobAcknowledgeError[] errors)
        {
            if (errors == null || errors.Length == 0)
                return new long[0];

            var result = new long[errors.Length];

            for (int i = 0; i < errors.Length; ++i)
                result[i] = (long)errors[i];

            return result;
        }
        private static string[] CreateErrorTexts(params JobAcknowledgeError[] errors)
        {
            if (errors == null || errors.Length == 0)
                return new string[0];

            var result = new string[errors.Length];

            for (int i = 0; i < errors.Length; ++i)
                result[i] = errors[i].GetDescription();

            return result;
        }
        #endregion </Acknowledge Error>
    }
}
