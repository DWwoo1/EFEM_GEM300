using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using EFEM.Defines.Common;
using EFEM.Defines.AtmRobot;
using EFEM.Defines.LoadPort;
using EFEM.Defines.MaterialTracking;
using EFEM.MaterialTracking;
using EFEM.CustomizedByProcessType.PWA500Common;
using EFEM.ActionScheduler.RobotActionSchedulers;
using EFEM.Jobs.Manager;
using EFEM.Jobs.Domain;
using EFEM.Defines.Job;
using EFEM.Jobs.Binding;

using FrameOfSystem3.SECSGEM;

namespace EFEM.CustomizedByProcessType.PWA500W
{
    public class PWA500WRobotActionSchedulerGEM300 : BaseRobotActionScheduler
    {
        #region <Constructors>
        public PWA500WRobotActionSchedulerGEM300(int index) : base(index)
        {
            _requestedLoadingLocation = new List<string>();
            _requestedUnloadingLocation = new List<string>();

            _functionsForPWA500 = FunctionsForPWA500W_NRD_300.Instance;

            _seqNum = 0;
            _substratesAtProcessModule = new List<Substrate>();
        }
        #endregion </Constructors>

        #region <Enum>
        private enum ArmDecisionPriority : int
        {
            None = int.MaxValue,
            PlaceToPM = 1,          // PM 투입 -> 프로세스를 위해 자재를 공정설비에 안착
            PlaceToLP = 2,          // 목적지 안착 -> 프로세스 완료된 자재를 로드포트에 안착
            ReturnToLP = 3,         // 원위치 복귀 -> 암을 비워야하는 상황
        }
        #endregion </Enum>

        #region <Types>
        private sealed class LoadingJobCandidate
        {
            public LoadingJobCandidate(
                int loadPortIndex,
                int portId,
                SubstrateType substrateType,
                SubstrateSize substrateSize,
                ControlJob controlJob)
            {
                LoadPortIndex = loadPortIndex;
                PortId = portId;
                SubstrateType = substrateType;
                SubstrateSize = substrateSize;
                ControlJob = controlJob;
            }

            public int LoadPortIndex { get; private set; }
            public int PortId { get; private set; }
            public SubstrateType SubstrateType { get; private set; }
            public SubstrateSize SubstrateSize { get; private set; }

            // Empty는 Job 없이 투입 가능하므로 null 허용
            public ControlJob ControlJob { get; private set; }
        }
        #endregion </Types>

        #region <Fields>
        private const int ProcessModuleIndex = 0;
        private const string CoreToString = "Core";
        private const string InputToString = "Input";
        private const string Inch8ToString = "8";
        private const string Inch12ToString = "12";
        private List<string> _requestedLoadingLocation;
        private List<string> _requestedUnloadingLocation;

        private static FunctionsForPWA500W_NRD_300 _functionsForPWA500 = null;

        private List<Substrate> _substratesAtProcessModule = null;
        private static readonly List<string> EmptyRequests = new List<string>(0);
        #endregion </Fields>

        #region <Properties>
        #endregion </Properties>

        #region <Methods>
        public override void InitScheduler()
        {
            base.InitScheduler();
        }
        protected override RobotScheduleType DecideNextAction()
        {
            // 0. PM으로부터 요청상태 갱신
            var (loadingReqs, unloadingReqs) = GetRequestPairs();
            //bool isPmEmpty = IsProcessModuleEmpty();

            #region <1. 암에 있는 자재를 이용해서 뭘 할지 결정>
            RobotScheduleType actionToUpper = RobotScheduleType.Selection, actionToLower = RobotScheduleType.Selection;
            ArmDecisionPriority priorityUpper = ArmDecisionPriority.None, priorityLower = ArmDecisionPriority.None;

            // 각각의 정보와 실행해야할 액션을 받아온다.
            bool hasUpper = GetSubstrateOnArm(RobotArmTypes.UpperArm, out Substrate substrateOnUpper);
            bool hasLower = GetSubstrateOnArm(RobotArmTypes.LowerArm, out Substrate substrateOnLower);
            bool canUpper = hasUpper && DecideActionForSubstrateOnArm(RobotArmTypes.UpperArm, substrateOnUpper, loadingReqs, unloadingReqs, out actionToUpper, out priorityUpper);
            bool canLower = hasLower && DecideActionForSubstrateOnArm(RobotArmTypes.LowerArm, substrateOnLower, loadingReqs, unloadingReqs, out actionToLower, out priorityLower);

            // 1) 두 팔 모두 가능
            if (canUpper && canLower)
            {
                return ((int)priorityUpper <= (int)priorityLower)
                    ? actionToUpper
                    : actionToLower;
            }
            else if (canUpper) // 2) Upper 만 가능
            {
                return actionToUpper;
            }
            else if (canLower) // 3) Lower 만 가능
            {
                return actionToLower;
            }
            #endregion </1. 암에 있는 자재를 이용해서 뭘 할지 결정>

            #region <2. 언로딩 요청에 대한 처리>
            // 언로딩 요청을 보고 -> PM에서 픽까지
            foreach (var item in unloadingReqs)
            {
                if (false == ClassifySpecsToHandlingByRequestedLocation(item, out _, out var size, out var isLoading) || isLoading)
                    continue;

                var arm = GetTargetArmBySize(size);
                if (false == IsArmAvailable(arm)) continue;

                // Pick from PM(Output) : 이후 Place 단계에서 LP로
                SetWorkingInfoToWork(arm, string.Empty, item, ModuleType.ProcessModule);
                return RobotScheduleType.Pick;
            }
            #endregion </2. 언로딩 요청에 대한 처리>

            #region <3. 로딩 요청에 대한 처리>
            // 로딩 요청을 보고 -> 로드포트에서 픽까지
            foreach (var item in loadingReqs)
            {
                if (false == ClassifySpecsToHandlingByRequestedLocation(item, out var substrateType, out var size, out var isLoading) ||
                    false == isLoading)
                    continue;

                var arm = GetTargetArmBySize(size);
                if (false == IsArmAvailable(arm)) continue;

                //ProcessModuleLocation pmInputLoc = new ProcessModuleLocation(string.Empty, string.Empty);
                //if (false == GetProcessModuleLocation(item, ref pmInputLoc))
                //    continue;

                // GEM300에서는 공테이프 선투입 로직 제거 필요
                // PM이 비어있고, Core 요청이면(=코어 첫 투입 방지) 패스
                //if (isPmEmpty && substrateType == SubstrateType.Core)
                //    continue;

                //if (IsProcessModuleEmpty() && IsTypeEqualsCoreByRequestedLocation(pmKind))
                //    continue;

                // 요청지 이름을 통해 SubstrateType을 가져온다. -> 위에서 받아오니 패스
                //MapRequestToEnums(pmKind, out SubstrateType subType);

                // 투입 가능한 LP 번호를 가져오고, 없으면 패스
                if (false == FindLoadPortForLoading(substrateType, 
                    size,
                    out var lpIndex,
                    out var processJobId))
                    continue;

                // 픽업할 슬롯 정보를 가져와서 가능하면 픽 진행
                if (_functionsForPWA500.GetNextSlotInformationToPick(
                    lpIndex, 
                    substrateType,
                    processJobId,
                    out var lpLoc,
                    out var key))
                {
                    SetWorkingInfoToWork(arm, key, lpLoc.Id, ModuleType.LoadPort);
                    return RobotScheduleType.Pick;
                }
            }
            #endregion </3. 로딩 요청에 대한 처리>

            // 4. 결정 불가
            return RobotScheduleType.Selection;
        }

        // 암에 있는 자재정보에 따라 실행할 행동을 결정(우선순위 값이 낮은 것을 먼저 수행)
        private bool DecideActionForSubstrateOnArm(
            RobotArmTypes arm,
            Substrate substrateOnArm,
            List<string> loadingReqs,
            List<string> unloadingReqs,
            out RobotScheduleType action,
            out ArmDecisionPriority priority)
        {
            action = RobotScheduleType.Selection;
            priority = ArmDecisionPriority.None;
            if (substrateOnArm == null)
                return false;

            if (substrateOnArm.ProcessingStatus == ProcessingStates.NeedsProcessing)
            {
                // 1) NeedsProcessing + PM 로딩요청이면 PM에 안착(PlaceToPM)
                if (HasLoadingRequestToPlace(substrateOnArm, loadingReqs, out var entryway))
                {
                    SetWorkingInfoToWork(arm, substrateOnArm.UniqueKey, entryway, ModuleType.ProcessModule);
                    action = RobotScheduleType.Place;
                    priority = ArmDecisionPriority.PlaceToPM;
                    return true;
                }

            }

            if (substrateOnArm.ProcessingStatus != ProcessingStates.NeedsProcessing)
            {
                // 2) NeedsProcessing 가 아니면 LP에 안착(PlaceToLP)
                if (GetLoadPortLocationToPlace(substrateOnArm, false, out LoadPortLocation destLoc) &&
                    IsSlotEmpty(destLoc))
                {
                    var subSizeString = substrateOnArm.GetAttribute(PWA500SubstrateAttributes.SubstrateSize);
                    var subTypeString = substrateOnArm.GetAttribute(PWA500SubstrateAttributes.SubstrateType);

                    if (Enum.TryParse(subSizeString, out SubstrateSize substrateSize) &&
                        Enum.TryParse(subTypeString, out SubstrateType onArmType))
                    {
                        bool canPlaceToLoadPort = false;

                        if (onArmType == SubstrateType.Core)
                        {
                            // Core 완료품은 Job 선택 없이 기존 정보 그대로 LP 안착 가능
                            canPlaceToLoadPort = true;
                        }
                        else if (IsBinType(onArmType))
                        {
                            // Bin1/2/3 완료품은 안착 전에 Output Job 선택 필요
                            // 로딩 직후 잡이 생성되고 실행되겠지만, 혹시 모를 상황에 대비해 남겨둔다.
                            // 정상적이라면 기판 속성에 잡만 설정, 비정상적이면 잡 실행 후 기판 속성에 잡을 설정
                            if (TryFindLoadPortForPlacingByPortId(
                                destLoc.PortId,
                                onArmType,
                                substrateSize,
                                out _,
                                out var pjId) &&
                                false == string.IsNullOrWhiteSpace(pjId))
                            {
                                _substrateManager.SetProcessJobIdByKey(
                                    substrateOnArm.UniqueKey,
                                    pjId);

                                _substrateManager.SaveDataByKey(
                                    substrateOnArm.UniqueKey);

                                canPlaceToLoadPort = true;
                            }
                        }
                        else if (onArmType == SubstrateType.Empty)
                        {
                            // 정책: Empty 완료품은 LP로 갈 수 없음
                            // return false 하지 않고 이 분기를 그냥 통과시킨다.
                            canPlaceToLoadPort = false;
                        }

                        if (canPlaceToLoadPort)
                        {
                            SetWorkingInfoToWork(
                                arm,
                                substrateOnArm.UniqueKey,
                                destLoc.Id,
                                ModuleType.LoadPort);

                            action = RobotScheduleType.Place;
                            priority = ArmDecisionPriority.PlaceToLP;
                            return true;
                        }
                    }
                }
            }

            if (substrateOnArm.ProcessingStatus == ProcessingStates.NeedsProcessing)
            {
                // 3) NeedsProcessing + 로딩요청 없는데(위에서 처리됐을 것) 내 크기의 다른 요청이 있으면 LP로 회수(ReturnToLP)
                var subSizeString = substrateOnArm.GetAttribute(PWA500SubstrateAttributes.SubstrateSize);
                var subTypeString = substrateOnArm.GetAttribute(PWA500SubstrateAttributes.SubstrateType);
                if (Enum.TryParse(subSizeString, out SubstrateSize substrateSize) && Enum.TryParse(subTypeString, out SubstrateType onArmType))
                {
                    SubstrateType reqType;
                    if (GetLoadPortLocationToPlace(substrateOnArm, true, out LoadPortLocation backLoc))
                    {
                        if (HasUnloadingRequestBySubstrateSize(substrateSize, unloadingReqs, out _) ||
                            (HasLoadingRequestBySubstrateSize(substrateSize, loadingReqs, out reqType) && onArmType != reqType))
                        {
                            if (IsSlotEmpty(backLoc))
                            {
                                SetWorkingInfoToWork(arm, substrateOnArm.UniqueKey, backLoc.Id, ModuleType.LoadPort);
                                action = RobotScheduleType.Place;
                                priority = ArmDecisionPriority.ReturnToLP;
                                return true;
                            }

                            // TODO : 목적지 슬롯이 막히면..? 추후 알람 처리 필요
                        }
                    }
                }
                //if (Enum.TryParse(subTypeString, out SubstrateSize substrateSize) &&
                //    HasUnloadingRequestBySubstrateSize(substrateSize, unloadingReqs) &&
                //    GetLoadPortLocationToPlace(substrateOnArm, true, out LoadPortLocation backLoc))
                //{
                //    if (IsSlotEmpty(backLoc))
                //    {
                //        SetWorkingInfoToWork(arm, substrateOnArm.GetName(), backLoc);
                //        action = RobotScheduleType.Place;
                //        priority = ArmDecisionPriority.ReturnToLP;
                //        return true;
                //    }

                //    // TODO : 목적지 슬롯이 막히면..? 추후 알람 처리 필요
                //}
            }

            return false;
        }

        private List<LoadingJobCandidate> CreateLoadingJobCandidates(
            IJobManager manager,
            Dictionary<int, Tuple<SubstrateType, SubstrateSize>> loadPortInfo)
        {
            var result = new List<LoadingJobCandidate>();

            if (loadPortInfo == null || loadPortInfo.Count == 0)
                return result;

            foreach (var item in loadPortInfo)
            {
                int loadPortIndex = item.Key;
                int portId = _loadPortManager.GetLoadPortPortId(loadPortIndex);

                SubstrateType substrateType = item.Value.Item1;
                SubstrateSize substrateSize = item.Value.Item2;

                ControlJob controlJob = null;

                if (_functionsForPWA500.IsJobRequiredForLoading(substrateType))
                {
                    AddLoadingJobCandidatesByCarrierPort(
                        manager,
                        result,
                        loadPortIndex,
                        portId,
                        substrateType,
                        substrateSize);

                    continue;
                }

                result.Add(new LoadingJobCandidate(
                    loadPortIndex,
                    portId,
                    substrateType,
                    substrateSize,
                    null));
            }

            return result;
        }
        private void AddLoadingJobCandidatesByCarrierPort(
            IJobManager manager,
            List<LoadingJobCandidate> result,
            int loadPortIndex,
            int portId,
            SubstrateType substrateType,
            SubstrateSize substrateSize)
        {
            if (manager == null || result == null)
                return;

            var binder = SubstrateJobBindingService.Instance;
            if (binder == null)
                return;

            var controlJobIds = binder.GetControlJobIdsByCarrierPort(portId);
            if (controlJobIds == null || controlJobIds.Count == 0)
                return;

            foreach (var controlJobId in controlJobIds)
            {
                if (string.IsNullOrWhiteSpace(controlJobId))
                    continue;

                var controlJob = manager.GetControlJobOrDefault(controlJobId);
                if (controlJob == null)
                    continue;

                if (false == IsControlJobBound(controlJob))
                    continue;

                result.Add(new LoadingJobCandidate(
                    loadPortIndex,
                    portId,
                    substrateType,
                    substrateSize,
                    controlJob));
            }
        }
        private List<LoadingJobCandidate> CreatePlacingJobCandidates(
            IJobManager manager,
            Dictionary<int, Tuple<SubstrateType, SubstrateSize>> loadPortInfo)
        {
            var result = new List<LoadingJobCandidate>();

            if (loadPortInfo == null || loadPortInfo.Count == 0)
                return result;

            foreach (var item in loadPortInfo)
            {
                int loadPortIndex = item.Key;
                int portId = _loadPortManager.GetLoadPortPortId(loadPortIndex);

                SubstrateType substrateType = item.Value.Item1;
                SubstrateSize substrateSize = item.Value.Item2;

                ControlJob controlJob = null;

                if (IsJobRequiredForPlacing(substrateType))
                {
                    AddPlacingJobCandidatesByDestinationPolicy(
                        manager,
                        result,
                        loadPortIndex,
                        portId,
                        substrateType,
                        substrateSize);

                    continue;
                }

                result.Add(new LoadingJobCandidate(
                    loadPortIndex,
                    portId,
                    substrateType,
                    substrateSize,
                    controlJob));
            }

            return result;
        }
        private void AddPlacingJobCandidatesByDestinationPolicy(
            IJobManager manager,
            List<LoadingJobCandidate> result,
            int loadPortIndex,
            int portId,
            SubstrateType substrateType,
            SubstrateSize substrateSize)
        {
            if (manager == null || result == null)
                return;

            var controlJobs = manager.GetAllControlJobs();
            if (controlJobs == null || controlJobs.Count == 0)
                return;

            string carrierId = _carrierServer.GetCarrierId(portId);
            if (string.IsNullOrWhiteSpace(carrierId))
                return;

            foreach (var controlJob in controlJobs)
            {
                if (controlJob == null)
                    continue;

                if (false == IsControlJobBound(controlJob))
                    continue;

                if (false == IsControlJobDestinationCarrier(
                    controlJob,
                    portId,
                    carrierId))
                {
                    continue;
                }

                result.Add(new LoadingJobCandidate(
                    loadPortIndex,
                    portId,
                    substrateType,
                    substrateSize,
                    controlJob));
            }
        }
        private bool IsControlJobDestinationCarrier(
            ControlJob controlJob,
            int portId,
            string carrierId)
        {
            if (controlJob == null)
                return false;

            if (string.IsNullOrWhiteSpace(carrierId))
                return false;

            // 1. OutSpec이 있으면 OutSpec.Value를 destination carrier로 본다.
            if (HasMaterialOutputSpecification(controlJob))
            {
                return ContainsMaterialOutputSpecificationValue(
                    controlJob,
                    carrierId);
            }

            // 2. OutSpec이 없으면 source carrier, 즉 InSpec / MaterialInfo 쪽으로 간다.
            return ContainsControlJobByBinderCarrierPort(
                controlJob,
                portId);
        }
        private static bool HasMaterialOutputSpecification(ControlJob controlJob)
        {
            if (controlJob == null)
                return false;

            var outputSpecifications = controlJob.MaterialOutputSpecifications;
            if (outputSpecifications == null || outputSpecifications.Length == 0)
                return false;

            foreach (var outputSpec in outputSpecifications)
            {
                if (outputSpec == null)
                    continue;

                if (false == string.IsNullOrWhiteSpace(outputSpec.Value))
                    return true;
            }

            return false;
        }

        private static bool ContainsMaterialOutputSpecificationValue(
            ControlJob controlJob,
            string carrierId)
        {
            if (controlJob == null)
                return false;

            if (string.IsNullOrWhiteSpace(carrierId))
                return false;

            var outputSpecifications = controlJob.MaterialOutputSpecifications;
            if (outputSpecifications == null || outputSpecifications.Length == 0)
                return false;

            foreach (var outputSpec in outputSpecifications)
            {
                if (outputSpec == null)
                    continue;

                if (string.Equals(
                    outputSpec.Value,
                    carrierId,
                    StringComparison.OrdinalIgnoreCase))
                {
                    return true;
                }
            }

            return false;
        }

        private bool ContainsControlJobByBinderCarrierPort(
            ControlJob controlJob,
            int portId)
        {
            if (controlJob == null)
                return false;

            var binder = SubstrateJobBindingService.Instance;
            if (binder == null)
                return false;

            var controlJobIds = binder.GetControlJobIdsByCarrierPort(portId);
            if (controlJobIds == null || controlJobIds.Count == 0)
                return false;

            foreach (var controlJobId in controlJobIds)
            {
                if (string.Equals(
                    controlJobId,
                    controlJob.Id,
                    StringComparison.OrdinalIgnoreCase))
                {
                    return true;
                }
            }

            return false;
        }
        private static bool IsBinType(SubstrateType type)
        {
            return type == SubstrateType.Bin1 ||
                   type == SubstrateType.Bin2 ||
                   type == SubstrateType.Bin3;
        }

        private static bool IsJobRequiredForPlacing(SubstrateType type)
        {
            return IsBinType(type);
        }
        private static bool IsControlJobBound(ControlJob controlJob)
        {
            if (controlJob == null)
                return false;

            if (string.IsNullOrWhiteSpace(controlJob.Id))
                return false;

            return SubstrateJobBindingService.Instance == null ||
                   SubstrateJobBindingService.Instance.IsBoundForControlJob(controlJob.Id);
        }

        private LoadingJobCandidate FindFirstCandidateByControlJobOrder(
            IJobManager manager,
            List<LoadingJobCandidate> candidates,
            Predicate<LoadingJobCandidate> predicate)
        {
            if (manager == null || candidates == null || candidates.Count == 0)
                return null;

            var orderedControlJobs = manager.GetAllControlJobs();
            if (orderedControlJobs == null || orderedControlJobs.Count == 0)
                return null;

            foreach (var cj in orderedControlJobs)
            {
                if (cj == null)
                    continue;

                foreach (var candidate in candidates)
                {
                    if (candidate == null || candidate.ControlJob == null)
                        continue;

                    if (false == string.Equals(
                        candidate.ControlJob.Id,
                        cj.Id,
                        StringComparison.Ordinal))
                    {
                        continue;
                    }

                    if (predicate == null || predicate(candidate))
                        return candidate;
                }
            }

            return null;
        }
        private bool TryAdvanceActivatedControlJob(
            IJobManager manager,
            LoadingJobCandidate candidate,
            out int lpIndex,
            out string processJobId)
        {
            lpIndex = -1;
            processJobId = string.Empty;

            if (manager == null || candidate == null || candidate.ControlJob == null)
                return false;

            var cj = candidate.ControlJob;

            if (cj.State == ControlJobState.Selected ||
                cj.State == ControlJobState.WaitingForStart)
            {
                return false;
            }

            if (cj.State != ControlJobState.Executing)
                return false;

            var pjs = manager.GetLinkedProcessJobs(cj.Id);
            if (pjs == null || pjs.Count == 0)
                return false;

            foreach (var pj in pjs)
            {
                if (pj == null)
                    continue;

                if (pj.State == ProcessJobState.JobQueued)
                {
                    manager.NotifyProcessJobSettingUpStarted(pj.Id);
                    return false;
                }

                if (pj.State == ProcessJobState.SettingUp)
                {
                    manager.NotifyProcessJobSettingUpCompleted(pj.Id);
                    return false;
                }

                if (false == IsProcessJobAvailableForMaterialMove(pj.State))
                    continue;

                lpIndex = candidate.LoadPortIndex;
                processJobId = pj.Id;
                return true;
            }

            return false;
        }
        private static bool IsProcessJobAvailableForMaterialMove(ProcessJobState state)
        {
            return state == ProcessJobState.WaitingForStart ||
                   state == ProcessJobState.Processing;
        }
        private bool TrySelectLoadingJobFromCandidate(
            IJobManager manager,
            LoadingJobCandidate target,
            List<LoadingJobCandidate> candidates,
            out string processJobId)
        {
            processJobId = string.Empty;

            if (target == null)
                return false;

            // 개조 후 아래 주석 살려야함
            // 개조 전도 BIN1로 설정하므로 있어도 무방하다.
            // Loading 정책: Empty는 Job 없이 투입 가능
            if (target.SubstrateType == SubstrateType.Empty)
                return true;

            // 개조 후 아래 주석 살려야함
            // 코어를 반환하는 메서드이므로 활용 가능(개조전은 true반환하므로 통과, 개조후는 조건 검사)
            // Loading 정책: Core만 GEM300 Job 필요
            //if (target.SubstrateType != SubstrateType.Core)
            //    return false;
            if (false == _functionsForPWA500.IsJobRequiredForLoading(target.SubstrateType))
                return false;

            return TrySelectJobFromCandidate(
                manager,
                target,
                candidates,
                out processJobId);
        }
        private bool TrySelectPlacingJobFromCandidate(
            IJobManager manager,
            LoadingJobCandidate target,
            List<LoadingJobCandidate> candidates,
            out string processJobId)
        {
            processJobId = string.Empty;

            if (target == null)
                return false;

            if (false == IsBinType(target.SubstrateType))
                return false;

            return TrySelectJobFromCandidate(
                manager,
                target,
                candidates,
                out processJobId);
        }
        private bool TrySelectJobFromCandidate(
            IJobManager manager,
            LoadingJobCandidate target,
            List<LoadingJobCandidate> candidates,
            out string processJobId)
        {
            processJobId = string.Empty;

            if (manager == null || target == null || target.ControlJob == null)
                return false;

            if (false == IsControlJobBound(target.ControlJob))
                return false;

            // 1. 이미 active인 같은 종류 CJ가 있으면 그것이 최우선.
            //    active CJ가 target이 아니면 현재 target은 skip.
            var activeSameKind = FindFirstCandidateByControlJobOrder(
                manager,
                candidates,
                delegate (LoadingJobCandidate candidate)
                {
                    return candidate.ControlJob != null &&
                           IsSameJobKind(candidate.SubstrateType, target.SubstrateType) &&
                           IsControlJobActive(candidate.ControlJob.State);
                });

            if (activeSameKind != null)
            {
                if (false == IsSameControlJob(activeSameKind, target))
                    return false;

                return TryAdvanceActivatedControlJob(
                    manager,
                    target,
                    out _,
                    out processJobId);
            }

            // 2. Queued 상태인 같은 종류 중 가장 앞선 CJ를 찾는다.
            //    Size는 여기서 비교하지 않는다.
            var firstSameKindQueued = FindFirstCandidateByControlJobOrder(
                manager,
                candidates,
                delegate (LoadingJobCandidate candidate)
                {
                    return candidate.ControlJob != null &&
                           IsSameJobKind(candidate.SubstrateType, target.SubstrateType) &&
                           candidate.ControlJob.State == ControlJobState.Queued;
                });

            if (firstSameKindQueued == null)
                return false;

            // 3. 같은 종류 중 선순위 CJ가 target이 아니면 skip.
            if (false == IsSameControlJob(firstSameKindQueued, target))
                return false;

            // 4. target이 같은 종류 중 최우선인데 HOQ가 아니면,
            //    앞에는 다른 종류 CJ가 있다는 뜻이므로 HOQ 요청.
            if (false == manager.IsHeadOfQueueControlJob(target.ControlJob.Id))
            {
                manager.RequestControlJobHeadOfQueue(target.ControlJob.Id);
                return false;
            }

            // 5. HOQ이면 Select 요청.
            manager.RequestControlJobSelect(target.ControlJob.Id);
            return false;
        }
        private static bool IsSameControlJob(
            LoadingJobCandidate left,
            LoadingJobCandidate right)
        {
            if (left == null || right == null)
                return false;

            if (left.ControlJob == null || right.ControlJob == null)
                return false;

            return string.Equals(
                left.ControlJob.Id,
                right.ControlJob.Id,
                StringComparison.Ordinal);
        }
        private static bool IsSameJobKind(
            SubstrateType left,
            SubstrateType right)
        {
            // 요구사항:
            // SubstrateType이 같으면 같은 종류의 잡이다.
            // Size는 잡 종류 판단에 사용하지 않는다.
            return left == right;
        }

        private static bool IsControlJobActive(ControlJobState state)
        {
            return state == ControlJobState.Executing ||
                   state == ControlJobState.WaitingForStart ||
                   state == ControlJobState.Selected;
        }

        private bool IsLoadPortPrepared(int lpIndex)
        {
            if (false == LoadPortInformations.ContainsKey(lpIndex))
                return false;

            return (
                LoadPortInformations[lpIndex].TransferState == LoadPortTransferStates.TransferBlocked && 
                LoadPortInformations[lpIndex].DoorState &&
                LoadPortInformations[lpIndex].CarrierIdVerificationState == CarrierIdVerificationStates.VerificationOk &&
                LoadPortInformations[lpIndex].CarrierSlotMapVerificationState == CarrierSlotMapVerificationStates.VerificationOk);
        }

        #region <Arm 관련>
        private void SetWorkingInfoToWork(RobotArmTypes arm, string key, string locationId, ModuleType locationType)
        {
            _workingInfo.ActionArm = arm;
            _workingInfo.SubstrateKey = key;
            _workingInfo.LocationId = locationId;
            _workingInfo.LocationType = locationType;
        }

        // Arm Role
        private RobotArmTypes GetTargetArmBySize(SubstrateSize waferSize) => waferSize == SubstrateSize.Inch_12 ? RobotArmTypes.UpperArm : RobotArmTypes.LowerArm;

        // 암이 사용 가능한지
        private bool IsArmAvailable(RobotArmTypes arm)
        {
            List<RobotArmTypes> arms = new List<RobotArmTypes>();
            if (false == _robotManager.GetAvailableArm(Index, true, ref arms))
                return false;

            return arms.Contains(arm);
        }
        // 타겟 암의 자재만 조회
        private bool GetSubstrateOnArm(RobotArmTypes targetArm, out Substrate s)
        {
            s = null;
            var robotName = _robotManager.GetRobotName(Index);

            return _substrateManager.GetSubstrateAtRobot(robotName, targetArm, out s);
        }
        #endregion </Arm 관련>

        #region <ProcessModule 관련>
        private string GetProcessModuleName()
        {
            return _processGroup.GetProcessModuleName(ProcessModuleIndex);
        }
        private bool GetProcessModuleLocation(string targetLocationName, ref ProcessModuleLocation location)
        {
            var pmName = GetProcessModuleName();
            return LocationServer.GetProcessModuleLocation(pmName, targetLocationName, out location);
        }
        // PM 내부가 비었는지
        private bool IsProcessModuleEmpty()
        {
            var pmName = GetProcessModuleName();

            if (false == _substrateManager.GetSubstratesAtProcessModule(pmName, ref _substratesAtProcessModule))
                return true;

            return _substratesAtProcessModule == null || _substratesAtProcessModule.Count == 0;
        }
        // 요청을 튜플로 반환
        private (List<string> loading, List<string> unloading) GetRequestPairs()
        {
            _processGroup.IsLoadingRequested(ProcessModuleIndex, ref _requestedLoadingLocation);
            _processGroup.IsUnloadingRequested(ProcessModuleIndex, ref _requestedUnloadingLocation);

            return (_requestedLoadingLocation ?? EmptyRequests, _requestedUnloadingLocation ?? EmptyRequests);
        }
        // 요청 이름을 Type, Size, Loading/Unloading 여부 등을 분류
        private bool ClassifySpecsToHandlingByRequestedLocation(string req, out SubstrateType substrateType, out SubstrateSize size, out bool isLoading)
        {
            isLoading = false;
            substrateType = SubstrateType.Core;
            size = SubstrateSize.Inch_12;
            if (string.IsNullOrWhiteSpace(req)) return false;

            var parts = req.Split(new[] { '.' }, 2);
            if (parts.Length != 2) return false;

            var tail = parts[1].Split('_'); // e.g. ["Core","12","Input"]
            if (tail.Length != 3) return false;

            isLoading = tail[2].Equals(InputToString, StringComparison.OrdinalIgnoreCase);
            var requestedLocationType = tail[0].Trim();

            if (requestedLocationType == CoreToString)
            {
                substrateType = SubstrateType.Core;
            }
            else
            {
                substrateType = _functionsForPWA500.GetRequestTypeFromPMForBinOrEmptyType(isLoading);
            }

            if (tail[1].Equals(Inch8ToString, StringComparison.OrdinalIgnoreCase))
            {
                size = SubstrateSize.Inch_8;
                return true;
            }
            else if (tail[1].Equals(Inch12ToString, StringComparison.OrdinalIgnoreCase))
            {
                size = SubstrateSize.Inch_12;
                return true;
            }
            else
            {
                return false;
            }
        }

        // 로딩 요청 중 주어진 웨이퍼 사이즈와 일치하는 요청이 하나라도 있는가?
        private bool HasLoadingRequestBySubstrateSize(SubstrateSize substrateSize, List<string> loadingReqs, out SubstrateType substrateType)
        {
            substrateType = SubstrateType.Core;
            if (loadingReqs == null || loadingReqs.Count == 0) return false;

            foreach (var item in loadingReqs)
            {
                if (false == ClassifySpecsToHandlingByRequestedLocation(item, out substrateType, out var reqSizeInch, out var isInput))
                    continue;

                if (false == isInput) continue;      // 언로딩은 패스

                if (reqSizeInch == substrateSize)   // 사이즈가 다르면 패스
                    return true;
            }
            return false;
        }

        // 언로딩 요청 중 주어진 웨이퍼 사이즈와 일치하는 요청이 하나라도 있는가?
        private bool HasUnloadingRequestBySubstrateSize(SubstrateSize substrateSize, List<string> unLoadingReqs, out SubstrateType substrateType)
        {
            substrateType = SubstrateType.Core;
            if (unLoadingReqs == null || unLoadingReqs.Count == 0) return false;

            foreach (var item in unLoadingReqs)
            {
                if (false == ClassifySpecsToHandlingByRequestedLocation(item, out substrateType, out var reqSizeInch, out var isInput))
                    continue;

                if (isInput) continue;      // 로딩은 패스

                if (reqSizeInch == substrateSize)   // 사이즈가 다르면 패스
                    return true;
            }
            return false;
        }

        // 로딩 요청 중 들고 있는 웨이퍼(Type/Size)를 받아줄 PM 의 요청이 있는지 확인한다.
        private bool HasLoadingRequestToPlace(Substrate substrate, List<string> loadingReqs, out string pmInputLoc)
        {
            pmInputLoc = string.Empty;
            var subTypeString = substrate.GetAttribute(PWA500SubstrateAttributes.SubstrateType);
            var subSizeString = substrate.GetAttribute(PWA500SubstrateAttributes.SubstrateSize);
            if (false == Enum.TryParse(subTypeString, out SubstrateType subType) ||
                false == Enum.TryParse(subSizeString, out SubstrateSize subSize))
                return false;

            foreach (var item in loadingReqs)
            {
                if (false == ClassifySpecsToHandlingByRequestedLocation(item, out var requestedLocationType, out var reqSize, out var isLoading)) continue;
                if (false == isLoading) continue;

                // 타입이 같은지 확인 : Core 는 동일한 Core 여야하고, 그 외는 Core 가 아니어야 같은 타입이다.
                bool isEquals =
                    (subType == SubstrateType.Core && requestedLocationType == SubstrateType.Core) ||
                    (subType != SubstrateType.Core && requestedLocationType != SubstrateType.Core);

                if (false == isEquals)
                    continue;

                if (reqSize != subSize)
                    continue;

                pmInputLoc = item;
                return true;
            }

            return false;
        }
        #endregion </ProcessModule 관련>

        #region <LoadPort 관련>
        private bool GetLoadPortLocationToPlace(Substrate substrate, bool isReturn, out LoadPortLocation lpLoc)
        {
            lpLoc = null;

            int port, slot;
            if (isReturn)
            {
                slot = substrate.SourceSlot;
                port = substrate.SourcePortId;
            }
            else
            {
                slot = substrate.DestinationSlot;
                port = substrate.DestinationPortId;
            }

            if (port <= 0 || slot < 0)
                return false;

            return LocationServer.GetLoadPortLocation(port, slot, out lpLoc);
        }

        // 슬롯 비어있는지 여부 확인
        private bool IsSlotEmpty(LoadPortLocation targetLoc)
        {
            return (false == _substrateManager.HasSubstrateAtLoadPort(targetLoc.PortId, targetLoc.Slot));
        }

        private Dictionary<int, Tuple<SubstrateType, SubstrateSize>> CreatePreparedLoadPortInfo()
        {
            var loadPortInfo = new Dictionary<int, Tuple<SubstrateType, SubstrateSize>>();

            for (int i = 0; i < _loadPortManager.Count; ++i)
            {
                if (false == _loadPortManager.IsLoadPortEnabled(i))
                    continue;

                int portId = _loadPortManager.GetLoadPortPortId(i);
                if (false == _carrierServer.HasCarrier(portId) ||
                    false == IsLoadPortPrepared(i) ||
                    _loadPortManager.IsLoadPortBusy(i))
                {
                    continue;
                }

                SubstrateType portType = SubstrateType.Core;
                SubstrateSize portSize = SubstrateSize.Inch_8;
                if (false == _functionsForPWA500.GetSubstrateSpecByLoadPortIndex(
                    i,
                    ref portType,
                    ref portSize))
                {
                    continue;
                }

                loadPortInfo[i] = Tuple.Create(portType, portSize);
            }

            return loadPortInfo;
        }
        private bool FindLoadPortForLoading(
            SubstrateType type,
            SubstrateSize size,
            out int lpIndex,
            out string processJobId)
        {
            lpIndex = -1;
            processJobId = string.Empty;

            IJobManager manager = JobManager.Instance;

            var loadPortInfo = CreatePreparedLoadPortInfo();
            if (loadPortInfo.Count == 0)
                return false;

            var candidates = CreateLoadingJobCandidates(
                manager,
                loadPortInfo);

            if (candidates.Count == 0)
                return false;

            foreach (var candidate in candidates)
            {
                if (candidate == null)
                    continue;

                if (candidate.SubstrateType != type)
                    continue;

                if (candidate.SubstrateSize != size)
                    continue;

                if (false == TrySelectLoadingJobFromCandidate(
                    manager,
                    candidate,
                    candidates,
                    out processJobId))
                {
                    continue;
                }

                lpIndex = candidate.LoadPortIndex;
                return true;
            }

            return false;
        }

        private bool TryFindLoadPortForPlacingByPortId(
            int portId,
            SubstrateType type,
            SubstrateSize size,
            out int lpIndex,
            out string processJobId)
        {
            lpIndex = -1;
            processJobId = string.Empty;

            if (portId <= 0)
                return false;

            // Place 전용 Job 선택은 Bin 완료품에만 적용한다.
            // Core 완료품은 DecideActionForSubstrateOnArm에서 바로 Place 처리하고,
            // Empty 완료품은 LP 안착 금지 정책이다.
            if (false == IsBinType(type))
                return false;

            IJobManager manager = JobManager.Instance;

            var loadPortInfo = CreatePreparedLoadPortInfo();
            if (loadPortInfo.Count == 0)
                return false;

            var candidates = CreatePlacingJobCandidates(
                manager,
                loadPortInfo);

            if (candidates.Count == 0)
                return false;

            LoadingJobCandidate target = null;

            foreach (var candidate in candidates)
            {
                if (candidate == null)
                    continue;

                if (candidate.PortId != portId)
                    continue;

                if (candidate.SubstrateType != type)
                    continue;

                if (candidate.SubstrateSize != size)
                    continue;

                target = candidate;
                break;
            }

            if (target == null)
                return false;

            if (false == TrySelectPlacingJobFromCandidate(
                manager,
                target,
                candidates,
                out processJobId))
            {
                return false;
            }

            lpIndex = target.LoadPortIndex;
            return true;
        }
        #endregion </LoadPort 관련>

        #endregion </Methods>
    }
}
