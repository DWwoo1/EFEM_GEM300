using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EFEM.Defines.AtmRobot;
using EFEM.MaterialTracking;
using EFEM.Defines.MaterialTracking;
using EFEM.Defines.LoadPort;
using EFEM.Defines.Common;
using EFEM.ActionScheduler.RobotActionSchedulers;
using EFEM.CustomizedByProcessType.PWA500Common;

using FrameOfSystem3.SECSGEM;

namespace EFEM.CustomizedByProcessType.PWA500W
{
    public class PWA500WRobotActionScheduler : BaseRobotActionScheduler
    {
        #region <Constructors>
        public PWA500WRobotActionScheduler(int index) : base(index)
        {
            _requestedLoadingLocation = new List<string>();
            _requestedUnloadingLocation = new List<string>();

            _functionsForPWA500 = FunctionsForPWA500W_NRD.Instance;

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

        #region <Fields>
        private const int ProcessModuleIndex = 0;
        private const string CoreToString = "Core";
        private const string InputToString = "Input";
        private const string Inch8ToString = "8";
        private const string Inch12ToString = "12";
        private List<string> _requestedLoadingLocation;
        private List<string> _requestedUnloadingLocation;

        private static FunctionsForPWA500W_NRD _functionsForPWA500 = null;

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
            bool isPmEmpty = IsProcessModuleEmpty();
        
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

                // PM이 비어있고, Core 요청이면(=코어 첫 투입 방지) 패스
                if (isPmEmpty && substrateType == SubstrateType.Core)
                    continue;

                //if (IsProcessModuleEmpty() && IsTypeEqualsCoreByRequestedLocation(pmKind))
                //    continue;

                // 요청지 이름을 통해 SubstrateType을 가져온다. -> 위에서 받아오니 패스
                //MapRequestToEnums(pmKind, out SubstrateType subType);

                // 투입 가능한 LP 번호를 가져오고, 없으면 패스
                int lpIndex = -1;
                if (false == FindLoadPortForLoading(substrateType, size, isPmEmpty, ref lpIndex))
                    continue;

                // 픽업할 슬롯 정보를 가져와서 가능하면 픽 진행
                if (GetNextSlotInformationToPick(lpIndex, out var lpLoc, out var key))
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
                    SetWorkingInfoToWork(arm, substrateOnArm.UniqueKey, destLoc.Id, ModuleType.LoadPort);
                    action = RobotScheduleType.Place;
                    priority = ArmDecisionPriority.PlaceToLP;
                    return true;
                }

                // TODO : 목적지 슬롯이 막히면..? 추후 알람 처리 필요
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
        private bool AreAllSubstratesNeedProcessing(int portId)
        {
            var subs = _substrateManager.GetSubstratesAtLoadPort(portId);

            foreach (var item in subs)
            {
                // 다른놈이 있는거다.
                if (item.Value.TransportStatus != TransportStates.AtDestination)
                    return false;
            }

            return true;
        }
        private bool IsLoadPortTransferStatusBlocked(int lpIndex)
        {
            if (false == LoadPortInformations.ContainsKey(lpIndex))
                return false;

            return (LoadPortInformations[lpIndex].TransferState == LoadPortTransferStates.TransferBlocked && LoadPortInformations[lpIndex].DoorState);
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
                substrateType = isLoading ? SubstrateType.Empty : SubstrateType.Bin1;
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
        private bool GetNextSlotInformationToPick(int lpIndex, out LoadPortLocation location, out string substrateKey)
        {
            location = null;
            substrateKey = string.Empty;

            int portId = _loadPortManager.GetLoadPortPortId(lpIndex);

            // 로드포트의 슬롯 별 자재를 가져온다.
            Dictionary<int, Substrate> substrates = _substrateManager.GetSubstratesAtLoadPort(portId);
            if (substrates == null ||
                substrates.Count <= 0)
                return false;

            foreach (var item in substrates)
            {
                if (item.Value == null)
                    continue;

                var transf = item.Value.TransportStatus;
                var proc = item.Value.ProcessingStatus;
                if (transf == TransportStates.AtSource &&
                    proc == ProcessingStates.NeedsProcessing)
                {
                    var locId = item.Value.LocationId;
                    if (false == LocationServer.FindLocationById(locId, out var loc))
                        continue;

                    location = loc as LoadPortLocation;
                    substrateKey = item.Value.UniqueKey;
                    if (location != null)
                        return true;
                }
            }

            return false;
        }
        private bool GetNextSlotInformationToPlace(int lpIndex, ref int slot)
        {
            int portId = _loadPortManager.GetLoadPortPortId(lpIndex);
            if (false == _carrierServer.HasCarrier(portId))
                return false;

            //if (false == _substrateManager.HasAnySubstrateInLoadPort(portId))
            //    return false;

            slot = -1;
            bool notAvailableSlotFirst = (_loadPortManager.GetCarrierLoadingType(lpIndex) == LoadPortLoadingMode.Cassette || _loadPortManager.GetCarrierLoadingType(lpIndex) == LoadPortLoadingMode.ClosedCassette);
            int capacity = _carrierServer.GetCapacity(portId);
            for (int i = 1; i <= capacity; ++i)
            {
                if (notAvailableSlotFirst && i == 1)
                    continue;

                if (false == _substrateManager.HasSubstrateAtLoadPort(portId, i))
                {
                    slot = i;
                    break;
                }
            }

            return (slot >= 0);
        }

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

        private bool FindLoadPortForLoading(SubstrateType type, SubstrateSize size, bool isPmEmpty, ref int lpIndex)
        {
            SubstrateType curType = SubstrateType.Core;
            SubstrateSize curSize = SubstrateSize.Inch_8;

            if (type.Equals(SubstrateType.Core))
            {
                // 1. Access된 Carrier가 있는지 먼저 검색
                int inAccessedCarrierIndex = -1;
                for (int i = 0; i < _loadPortManager.Count; ++i)
                {
                    if (false == _loadPortManager.IsLoadPortEnabled(i))
                        continue;

                    if (false == _functionsForPWA500.GetSubstrateSpecByLoadPortIndex(i, ref curType, ref curSize))
                        continue;

                    if (type != curType || size != curSize)
                        continue;

                    int portId = _loadPortManager.GetLoadPortPortId(i);
                    if (_carrierServer.GetCarrierAccessingStatus(portId).Equals(CarrierAccessStates.InAccessed))
                    {
                        inAccessedCarrierIndex = i;
                        break;
                    }
                }

                if (inAccessedCarrierIndex >= 0)
                {
                    // 1-1. Access된 캐리어가 있으면 작업이 가능한 상태인지 검사
                    int portId = _loadPortManager.GetLoadPortPortId(inAccessedCarrierIndex);
                    if (_carrierServer.HasCarrier(portId) &&
                        IsLoadPortTransferStatusBlocked(inAccessedCarrierIndex) &&
                        false == _loadPortManager.IsLoadPortBusy(inAccessedCarrierIndex))
                    {
                        lpIndex = inAccessedCarrierIndex;
                        return true;
                    }
                }
                else
                {
                    // 1-2. Access된 캐리어가 없으면, 작업 가능한 것 중 아무거나 선택
                    for (int i = 0; i < _loadPortManager.Count; ++i)
                    {
                        if (false == _loadPortManager.IsLoadPortEnabled(i))
                            continue;

                        if (false == _functionsForPWA500.GetSubstrateSpecByLoadPortIndex(i, ref curType, ref curSize))
                            continue;

                        if (type != curType || size != curSize)
                            continue;

                        int portId = _loadPortManager.GetLoadPortPortId(i);

                        if (false == _carrierServer.HasCarrier(portId))
                            continue;

                        // 완료되거나 정지된 놈은 스킵
                        if (_carrierServer.GetCarrierAccessingStatus(portId).Equals(CarrierAccessStates.CarrierCompleted) ||
                            _carrierServer.GetCarrierAccessingStatus(portId).Equals(CarrierAccessStates.CarrierStopped))
                            continue;

                        // 동작 중이면 스킵
                        if (false == IsLoadPortTransferStatusBlocked(i) ||
                            _loadPortManager.IsLoadPortBusy(i))
                            continue;

                        // 모든 자재가 NeedProcessing 상태면 -> TrackIn 해야하는 상황에 공정 설비에 공테이프가 없으면 투입하지 말아야한다.
                        if (AreAllSubstratesNeedProcessing(portId) && isPmEmpty)
                            continue;

                        //if (_substrateManager.AreAllSubstratesNeedProcessing(portId))
                        //{
                        //    if (false == _substrateManager.GetSubstratesAtProcessModule(processModuleName, ref _substratesAtProcessModule) ||
                        //        _substratesAtProcessModule.Count <= 0)
                        //        continue;
                        //}

                        lpIndex = i;

                        return true;
                    }
                }

                return false;
            }
            else
            {
                for (int i = 0; i < _loadPortManager.Count; ++i)
                {
                    SubstrateType convertedSubType = _functionsForPWA500.GetSubstrateTypeByLoadPortIndex(i);
                    if (convertedSubType.Equals(SubstrateType.Core))
                        continue;

                    int portId = _loadPortManager.GetLoadPortPortId(i);
                    if (false == _carrierServer.HasCarrier(portId) ||
                        false == IsLoadPortTransferStatusBlocked(i)
                        || _loadPortManager.IsLoadPortBusy(i))
                        continue;

                    // TODO : 빈소터와의 운영상 차이점 -> W는 소팅포트가 한 개여서 아래는 필요없을듯함.
                    lpIndex = i;

                    return true;

                    #region 
                    //string lotId = _carrierServer.GetCarrierLotId(portId);
                    //if (_substrateManager.GetSubstratesAtProcessModule(processModuleName, ref _substratesAtProcessModule))
                    //{
                    //    for (int subs = 0; subs < _substratesAtProcessModule.Count; ++subs)
                    //    {
                    //        string subType = _substratesAtProcessModule[subs].GetAttribute(PWA500WSubstrateAttributes.SubstrateType);
                    //        if (false == Enum.TryParse(subType, out SubstrateType substrateTypeAtProcessModule))
                    //            continue;

                    //        if (substrateTypeAtProcessModule.Equals(SubstrateType.Core))
                    //            continue;


                    //        //if (_substratesAtProcessModule[subs].GetAttribute(PWA500WSubstrateAttributes.ParentLotId) != null &&
                    //        //    false == _substratesAtProcessModule[subs].GetAttribute(PWA500WSubstrateAttributes.ParentLotId).Equals(lotId))
                    //        //{
                    //        //    return false;
                    //        //}
                    //    }

                    //    lpIndex = i;
                    //    return true;
                    //}
                    //else
                    //{
                    //    lpIndex = i;
                    //    return true;
                    //}
                    #endregion
                }

                return false;
            }
        }
        #endregion </LoadPort 관련>

        #endregion </Methods>
    }
}