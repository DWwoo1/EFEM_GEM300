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
using EFEM.CustomizedByProcessType.PWA500Common;
using EFEM.ActionScheduler.RobotActionSchedulers;

using FrameOfSystem3.SECSGEM;
using FrameOfSystem3.SECSGEM.Scenario;

namespace EFEM.CustomizedByProcessType.PWA500BIN
{
    public class PWA500BinSorterRobotActionScheduler : BaseRobotActionScheduler
    {
        #region <Constructors>
        public PWA500BinSorterRobotActionScheduler(int index) : base(index)
        {
            _requestedLoadingLocation = new List<string>();
            _requestedUnloadingLocation = new List<string>();

            _functionsForPWA500 = FunctionsForPWA500BIN_TP.Instance;

            CoreLoadPortIndex = new List<int>
            {
                (int)LoadPortType.Core_1,
                (int)LoadPortType.Core_2
            };

            //EmptyTapeLoadPortIndex = (int)LoadPortType.EmptyTape;

            BinLoadPortIndex = new List<int>
            {
                (int)LoadPortType.Bin_1,
                (int)LoadPortType.Bin_2,
                (int)LoadPortType.Bin_3
            };

            _seqNum = 0;

            LoadPortPorts = new Dictionary<int, int>();
            var ports = (LoadPortType[])Enum.GetValues(typeof(LoadPortType));
            foreach (var item in ports)
            {
                int lpIndex = (int)item;
                int port = _loadPortManager.GetLoadPortPortId(lpIndex);

                LoadPortPorts[lpIndex] = port;
            }

            WorkingInfosToPlace = new Dictionary<RobotArmTypes, RobotWorkingInfo>();
            LocationTypesToPlace = new Dictionary<RobotArmTypes, ModuleType>();
            ProcessModuleName = _processGroup.GetProcessModuleName(ProcessModuleIndex);

            _substratesAtProcessModule = new List<Substrate>();
        }
        #endregion </Constructors>

        #region <Fields>
        private const int ProcessModuleIndex = 0;
        private bool _turnLoad = false;
        private bool _turnUnload = false;

        private List<string> _requestedLoadingLocation;
        private List<string> _requestedUnloadingLocation;

        private readonly List<int> CoreLoadPortIndex = null;
        private readonly List<int> BinLoadPortIndex = null;

        private readonly Dictionary<int, int> LoadPortPorts = null;
        private readonly Dictionary<RobotArmTypes, ModuleType> LocationTypesToPlace = null;
        private readonly Dictionary<RobotArmTypes, RobotWorkingInfo> WorkingInfosToPlace = null;

        private readonly string ProcessModuleName;
        private static FunctionsForPWA500BIN_TP _functionsForPWA500 = null;

        private List<Substrate> _substratesAtProcessModule = null;
        #endregion </Fields>

        #region <Enum>
        //enum SubstrateType
        //{
        //    Core,
        //    Empty,
        //    Bin
        //}

        enum SchedulerStep
        {
            Start = 0,
            CollectData,
            SetupWorkInfoToPlace,
            SetupWorkInfoToPick,
            CheckRequestFromProcessModule,
            CheckAvailableArm,
            CheckLoadPortCondition,
            UpdateWorkingInfo,
            End = 1000
        }
        #endregion </Enum>

        #region <Properties>
        #endregion </Properties>

        #region <Methods>
        public override void InitScheduler()
        {
            base.InitScheduler();
        }
        private bool AreAllSubstratesNeedProcessing(int portId)
        {
            var subs = _substrateManager.GetSubstratesAtLoadPort(portId);

            foreach (var item in subs)
            {
                //if (item.Value.TransportStatus != SubstrateTransferStates.AtDestination)
                // 다른놈이 있는거다.
                if (item.Value.ProcessingStatus != ProcessingStates.NeedsProcessing)
                    return false;
            }

            return true;
        }
        private bool IsLoadPortTransferStatusBlocked(int lpIndex)
        {
            if (false == LoadPortInformations.ContainsKey(lpIndex))
                return false;

            return (LoadPortInformations[lpIndex].TransferState.Equals(LoadPortTransferStates.TransferBlocked) && LoadPortInformations[lpIndex].DoorState);
        }
        private bool HasCarriers(SubstrateType type, bool loading, ref int lpIndex)
        {
            switch (type)
            {
                case SubstrateType.Core:
                    {
                        List<int> preparedIndex = new List<int>();

                        // 1. Access된 Carrier가 있는지 먼저 검색
                        int inAccessedCarrierIndex = -1;
                        for (int i = 0; i < CoreLoadPortIndex.Count; ++i)
                        {
                            var index = CoreLoadPortIndex[i];
                            int portId = _loadPortManager.GetLoadPortPortId(index);

                            if (false == _carrierServer.HasCarrier(portId))
                                continue;

                            preparedIndex.Add(index);

                            if (_carrierServer.GetCarrierAccessingStatus(portId) == CarrierAccessStates.InAccessed)
                            {
                                inAccessedCarrierIndex = index;
                                break;
                            }
                        }

                        if (inAccessedCarrierIndex >= 0)
                        {
                            // Access 된 상태인데, 이 캐리어의 랏과 공정 설비에 코어가 이미 있고 그 코어의 랏과 다르면..? 여기도 인터락을 걸어야하나..
                            // 1-1. Access된 캐리어가 있으면 작업이 가능한 상태인지 검사
                            int portId = _loadPortManager.GetLoadPortPortId(inAccessedCarrierIndex);
                            if (_carrierServer.HasCarrier(portId) && IsLoadPortTransferStatusBlocked(inAccessedCarrierIndex) &&
                                false == _loadPortManager.IsLoadPortBusy(inAccessedCarrierIndex))
                            {
                                lpIndex = inAccessedCarrierIndex;
                                return true;
                            }
                        }
                        else
                        {
                            // 아예 준비된 포트가 없으면 스킵
                            if (preparedIndex.Count == 0)
                                return false;

                            string processModuleName = _processGroup.GetProcessModuleName(ProcessModuleIndex);

                            // 1-2. Access된 캐리어가 없으면, 작업 가능한 것 중 아무거나 선택
                            //  1) 공정설비에 자재가 있는지 검사(없으면 스킵)
                            if (false == _substrateManager.GetSubstratesAtProcessModule(processModuleName, ref _substratesAtProcessModule) ||
                                _substratesAtProcessModule.Count <= 0)
                            {
                                return false;
                            }

                            //  2) 공정설비에 코어타입이 아닌 자개가 있는지 먼저 검사 -> 코어 투입 전 공테이프가 최소 한개 투입되어 있어야 한다.(삼성 요청)
                            List<Substrate> cores = new List<Substrate>();
                            List<Substrate> bins = new List<Substrate>();
                            foreach (var item in _substratesAtProcessModule)
                            {
                                var substrateTypeString = item.GetAttribute(PWA500SubstrateAttributes.SubstrateType);
                                if (false == Enum.TryParse(substrateTypeString, out SubstrateType substrateType))
                                    continue;

                                if (substrateType == SubstrateType.Core)
                                {
                                    cores.Add(item);
                                }
                                else
                                {
                                    // 코어가 아닌 무언가가 있다.
                                    bins.Add(item);
                                }
                            }

                            // 코어가 한개 이상 있으면, 랏 비교 후 투입한다.
                            if (cores.Count > 0)
                            {
                                foreach (var item in cores)
                                {
                                    var parentLotId = item.GetAttribute(PWA500SubstrateAttributes.ParentLotId);
                                    foreach (var idx in preparedIndex)
                                    {
                                        var portId = _loadPortManager.GetLoadPortPortId(idx);
                                        var lotIdFromCarrier = _carrierServer.GetCarrierLotId(portId);
                                        if (string.Equals(parentLotId, lotIdFromCarrier, StringComparison.OrdinalIgnoreCase))
                                        {
                                            lpIndex = idx;
                                            return true;
                                        }
                                    }
                                }

                                return false;
                            }
                            else
                            {
                                // 코어가 없으면, 공테이프가 있는 경우만 투입한다.
                                if (bins.Count > 0)
                                {
                                    //  3) 있으면, 코어 포트 중 캐리어가 있는 임의의 포트를 선택
                                    lpIndex = preparedIndex.First();
                                    var portId = _loadPortManager.GetLoadPortPortId(lpIndex);
                                    if (false == _carrierServer.HasCarrier(portId) ||
                                        false == IsLoadPortTransferStatusBlocked(lpIndex) ||
                                        _loadPortManager.IsLoadPortBusy(lpIndex))
                                    {
                                        return false;
                                    }

                                    return true;
                                }
                                else
                                {
                                    return false;
                                }
                            }

                            #region <기존 코드>
                            //// 1-2. Access된 캐리어가 없으면, 작업 가능한 것 중 아무거나 선택

                            //for (int i = 0; i < CoreLoadPortIndex.Count; ++i)
                            //{
                            //    int portId = _loadPortManager.GetLoadPortPortId(CoreLoadPortIndex[i]);
                            //    if (false == _carrierServer.HasCarrier(portId) ||
                            //        false == IsLoadPortTransferStatusBlocked(CoreLoadPortIndex[i]) ||
                            //        _loadPortManager.IsLoadPortBusy(CoreLoadPortIndex[i]))
                            //        continue;

                            //    // 모든 자재가 NeedProcessing 상태면 -> TrackIn 해야하는 상황에 공정 설비에 공테이프가 없으면 투입하지 말아야한다.
                            //    if (AreAllSubstratesNeedProcessing(portId))
                            //    {
                            //        if (false == _substrateManager.GetSubstratesAtProcessModule(processModuleName, ref _substratesAtProcessModule) ||
                            //            _substratesAtProcessModule.Count <= 0)
                            //            continue;
                            //    }

                            //    lpIndex = CoreLoadPortIndex[i];

                            //    return true;
                            //}
                            #endregion </기존 코드>
                        }

                        return false;

                        #region <Orioginal>
                        //for (int i = 0; i < CoreLoadPortIndex.Count; ++i)
                        //{
                        //    int portId = _loadPortManager.GetLoadPortPortId(CoreLoadPortIndex[i]);
                        //    if (_carrierServer.HasCarrier(portId) && IsLoadPortTransferStatusBlocked(CoreLoadPortIndex[i]) &&
                        //        _carrierServer.GetCarrierAccessingStatus(portId).Equals(CarrierAccessStates.InAccessed) &&
                        //        false == _loadPortManager.IsLoadPortBusy(CoreLoadPortIndex[i]))
                        //    {
                        //        lpIndex = CoreLoadPortIndex[i];
                        //        return true;
                        //    }
                        //}

                        //for (int i = 0; i < CoreLoadPortIndex.Count; ++i)
                        //{
                        //    int portId = _loadPortManager.GetLoadPortPortId(CoreLoadPortIndex[i]);
                        //    if (_carrierServer.HasCarrier(portId) && IsLoadPortTransferStatusBlocked(CoreLoadPortIndex[i]) &&
                        //        false == _loadPortManager.IsLoadPortBusy(CoreLoadPortIndex[i]))
                        //    {
                        //        lpIndex = CoreLoadPortIndex[i];
                        //        return true;
                        //    }
                        //}
                        #endregion </Orioginal>
                    }

                case SubstrateType.Empty:
                    {
                        if (loading)
                        {
                            string processModuleName = _processGroup.GetProcessModuleName(ProcessModuleIndex);

                            for (int i = 0; i < _loadPortManager.Count; ++i)
                            {
                                SubstrateType convertedSubType = _functionsForPWA500.GetSubstrateTypeByLoadPortIndex(i);
                                if (false == convertedSubType.Equals(SubstrateType.Empty))
                                    continue;

                                int portId = _loadPortManager.GetLoadPortPortId(i);
                                if (false == _carrierServer.HasCarrier(portId) || false == IsLoadPortTransferStatusBlocked(i)
                                    || _loadPortManager.IsLoadPortBusy(i))
                                    continue;

                                if (_substrateManager.GetSubstratesAtProcessModule(processModuleName, ref _substratesAtProcessModule))
                                {
                                    // 공테이프의 모랏을 가져온다.
                                    string lotId = _carrierServer.GetCarrierLotId(portId);

                                    for (int subs = 0; subs < _substratesAtProcessModule.Count; ++subs)
                                    {
                                        string subType = _substratesAtProcessModule[subs].GetAttribute(PWA500SubstrateAttributes.SubstrateType);
                                        if (false == Enum.TryParse(subType, out SubstrateType substrateTypeAtProcessModule))
                                        {
                                            substrateTypeAtProcessModule = SubstrateType.Empty;
                                        }

                                        // 2025.03.06. jhlim [MOD] 코어인 경우만 체크하지 않도록 한다.
                                        if (substrateTypeAtProcessModule == SubstrateType.Core)
                                            continue;

                                        if (_substratesAtProcessModule[subs].GetAttribute(PWA500SubstrateAttributes.ParentLotId) != null &&
                                            false == _substratesAtProcessModule[subs].GetAttribute(PWA500SubstrateAttributes.ParentLotId).Equals(lotId))
                                        {
                                            return false;
                                        }
                                    }

                                    lpIndex = i;
                                    return true;
                                }
                                else
                                {
                                    lpIndex = i;
                                    return true;
                                }
                            }

                            #region <OriginalCode>                           
                            // 이 부분 모든 코드 확인 필요 - 작업이 이상하게됨
                            //for (int i = 0; i < _loadPortManager.Count; ++i)
                            //{
                            //    // 2024.09.03. jhlim [MOD] SubType을 UI에는 Center/Left/Right로 지정되도록 변경
                            //    //var paramName = FrameOfSystem3.Recipe.PARAM_EQUIPMENT.LoadPortType1 + i;
                            //    //string subTypeByRecipe = FrameOfSystem3.Recipe.Recipe.GetInstance().GetValue(FrameOfSystem3.Recipe.EN_RECIPE_TYPE.EQUIPMENT,
                            //    //    paramName.ToString(),
                            //    //    SubstrateType.Empty.ToString());
                            //    //if (false == subTypeByRecipe.Equals(SubstrateType.Empty.ToString()))
                            //    //    continue;

                            //    SubstrateType convertedSubType = _functionsForPWA500.GetSubstrateTypeByLoadPortIndex(i);
                            //    if (false == convertedSubType.Equals(SubstrateType.Empty))
                            //        continue;
                            //    // 2024.09.03. jhlim [END]

                            //    int portId = _loadPortManager.GetLoadPortPortId(i);
                            //    // 2024.12.04. jhlim [DEL]
                            //    //if (_carrierServer.HasCarrier(portId) && IsLoadPortTransferStatusBlocked(i))
                            //    // 2024.12.04. jhlim [END]
                            //    {
                            //        lpIndex = i;
                            //        return true;
                            //    }
                            //}
                            #endregion </OriginalCode>
                        }

                        return false;
                    }

                case SubstrateType.Bin1:
                case SubstrateType.Bin2:
                case SubstrateType.Bin3:
                    {
                        if (loading)
                        {
                            for (int i = 0; i < BinLoadPortIndex.Count; ++i)
                            {
                                int portId = _loadPortManager.GetLoadPortPortId(BinLoadPortIndex[i]);
                                if (_carrierServer.HasCarrier(portId) && IsLoadPortTransferStatusBlocked(BinLoadPortIndex[i]) &&
                                    _loadPortManager.IsLoadPortBusy(BinLoadPortIndex[i]))
                                {
                                    lpIndex = BinLoadPortIndex[i];
                                    return true;
                                }
                            }
                        }

                        return false;
                    }
                default:
                    return false;
            }
        }
        private bool GetSubstrateTypeByLoadingLocation(string locationName, ref SubstrateType substrateType)
        {
            if (locationName.Contains(Constants.CoreName))
            {
                substrateType = SubstrateType.Core;
                return true;
            }
            else if (locationName.Contains(Constants.SortName))
            {
                substrateType = SubstrateType.Empty;
                return true;
            }

            return false;
        }

        private bool GetSubstrateTypeByUnloadingLocation(string locationName, ref SubstrateType substrateType)
        {
            if (locationName.Contains(Constants.CoreName))
            {
                substrateType = SubstrateType.Core;
                return true;
            }
            else if (locationName.Contains(Constants.SortName))
            {
                if (FrameOfSystem3.Recipe.Recipe.GetInstance().GetValue(FrameOfSystem3.Recipe.EN_RECIPE_TYPE.COMMON, FrameOfSystem3.Recipe.PARAM_COMMON.UseCycleMode.ToString(),
                    false))
                {
                    substrateType = SubstrateType.Empty;
                }
                else
                {
                    substrateType = SubstrateType.Empty;
                    // Bin은 메시지 받기 전까지는 알 수 없으니 True 리턴한다.
                    return true;
                }
                return true;
            }

            return false;
        }

        // LoadPort 혹은 Carrier의 PortId로 PM의 입구를 찾는다.
        private string GetProcessModuleLoadingLocationByPortId(int portId)
        {
            bool find = false;
            LoadPortType lpType = LoadPortType.Core_1;
            foreach (var item in LoadPortPorts)
            {
                if (item.Value == portId)
                {
                    find = true;
                    lpType = (LoadPortType)item.Key;
                    break;
                }
            }

            if (find)
            {
                switch (lpType)
                {
                    // Bin으로 들어올 수가 있나??
                    //case LoadPortType.Bin_3:
                    //case LoadPortType.Bin_2:
                    //case LoadPortType.Bin_1:
                    //    return string.Empty;

                    case LoadPortType.EmptyTape:
                        return Constants.ProcessModuleSortInputName;

                    case LoadPortType.Core_2:
                    case LoadPortType.Core_1:
                        return Constants.ProcessModuleCoreInputName;

                    default:
                        break;
                }
            }

            return string.Empty;
        }

        private string GetProcessModuleUnloadingLocationByPortId(int portId)
        {
            bool find = false;
            LoadPortType lpType = LoadPortType.Core_1;
            foreach (var item in LoadPortPorts)
            {
                if (item.Value == portId)
                {
                    find = true;
                    lpType = (LoadPortType)item.Key;
                    break;
                }
            }

            if (find)
            {
                switch (lpType)
                {
                    case LoadPortType.Bin_3:
                    case LoadPortType.Bin_2:
                    case LoadPortType.Bin_1:
                        return Constants.ProcessModuleSortOutputName;

                    case LoadPortType.EmptyTape:
                        return Constants.ProcessModuleSortOutputName;

                    case LoadPortType.Core_2:
                    case LoadPortType.Core_1:
                        return Constants.ProcessModuleCoreOutputName;

                    default:
                        break;
                }
            }

            return string.Empty;
        }

        // 작업할 정보를 설정한다.
        private void SetWorkingInfoToWork(RobotArmTypes arm, string key, string locationId, ModuleType locationType)
        {
            _workingInfo.ActionArm = arm;
            _workingInfo.SubstrateKey = key;
            _workingInfo.LocationId = locationId;
            _workingInfo.LocationType = locationType;
        }

        // 자재 정보를 이용하여 내려놓을 장소를 찾는다.
        private bool GetWorkingInfoToPlace(Substrate substrate, out string locationId, out ModuleType locationType)
        {
            locationId = string.Empty;
            locationType = ModuleType.Unknown;
            switch (substrate.ProcessingStatus)
            {
                case ProcessingStates.NeedsProcessing:      // TO PM
                    {
                        // TODO 1. 라우트 레시피가 있는 경우 다음 공정 스텝으로 위치를 받아와야한다.
                        string subType = substrate.GetAttribute(PWA500SubstrateAttributes.SubstrateType);
                        if (false == Enum.TryParse(subType, out SubstrateType substrateType))
                        {

                        }
                        else
                        {
                            switch (substrateType)
                            {
                                case SubstrateType.Core:
                                    {
                                        locationId = Constants.ProcessModuleCoreInputName;
                                    }
                                    break;
                                case SubstrateType.Empty:
                                    {
                                        locationId = Constants.ProcessModuleSortInputName;
                                    }
                                    break;
                                default:
                                    // 여기에 Place 할 일이 없다.
                                    break;
                            }

                            //string processModuleName = _processGroup.GetProcessModuleName(ProcessModuleIndex);
                            //ProcessModuleLocation location = new ProcessModuleLocation(processModuleName, locationName);
                            //_locationServer.GetProcessModuleLocation(processModuleName, locationName, ref location);
                            //targetLocation = location;
                            locationType = ModuleType.ProcessModule;
                        }

                        return true;
                    }

                //case ProcessingStates.InProcess:
                //    break;
                //case ProcessingStates.Processed:
                //    break;
                case ProcessingStates.Rejected:     // TO LP
                case ProcessingStates.Stopped:
                case ProcessingStates.Aborted:
                case ProcessingStates.Skipped:
                    {
                        if (false == LocationServer.GetLoadPortLocation(substrate.SourcePortId, substrate.SourceSlot, out var location))
                            return false;

                        locationId = location.Id;
                        locationType = ModuleType.LoadPort;

                        //int lpIndex = _loadPortManager.GetLoadPortIndexByPortId(substrate.SourcePortId);
                        //string lpName = _loadPortManager.GetLoadPortName(lpIndex);
                        //targetLocation = lpName;
                        //targetSlot = substrate.SourceSlot;
                        //locationType = ModuleType.LoadPort;
                        return true;
                    }

                case ProcessingStates.Lost:
                    return false;

                default:    // TO LP
                    {
                        int lpIndex = _loadPortManager.GetLoadPortIndexByPortId(substrate.DestinationPortId);
                        int targetSlot = 0;
                        if (lpIndex == (int)LoadPortType.Bin_1 ||
                            lpIndex == (int)LoadPortType.Bin_2 ||
                            lpIndex == (int)LoadPortType.Bin_3)
                        {
                            if (false == GetNextSlotInformationToPlace(lpIndex, ref targetSlot))
                                return false;
                        }
                        else
                        {
                            targetSlot = substrate.SourceSlot;
                        }

                        if (false == LocationServer.GetLoadPortLocation(substrate.DestinationPortId, targetSlot, out var location))
                            return false;

                        locationId = location.Id;
                        locationType = ModuleType.LoadPort;

                        return true;
                    }
            }
        }

        private bool HasSubstratateToLoadAtProcessModule(string targetLocation)
        {
            for (int i = 0; i < _requestedLoadingLocation.Count; ++i)
            {
                if (_requestedLoadingLocation[i].Equals(targetLocation))
                    return true;
            }

            return false;
        }

        private bool HasSubstratateToUnloadAtProcessModule(string targetLocation)
        {
            for (int i = 0; i < _requestedUnloadingLocation.Count; ++i)
            {
                if (_requestedLoadingLocation[i].Equals(targetLocation))
                    return true;
            }

            return false;
        }

        private bool GetNextSlotInformationToPick(int lpIndex, out LoadPortLocation location, out string key)
        {
            location = null;
            key = string.Empty;

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
                    key = item.Value.UniqueKey;
                    if (location != null)
                        return true;
                }
            }

            return false;
            //location = null;
            //key = string.Empty;

            //int portId = _loadPortManager.GetLoadPortPortId(lpIndex);

            //if (false == _substrateManager.HasAnySubstrateAtLoadPort(portId))
            //    return false;

            //int capacity = _carrierServer.GetCapacity(portId);
            //for (int i = 0; i < capacity; ++i)
            //{
            //    if (LocationServer.GetLoadPortLocation(portId, i, out var lpLocation))
            //    {
            //        SubstrateTransferStates transferStatus = SubstrateTransferStates.AtSource;
            //        ProcessingStates processingStatus = ProcessingStates.NeedsProcessing;
            //        if (_substrateManager.GetTransferStatusAtLoadPort(portId, i, ref transferStatus) &&
            //            _substrateManager.GetProcessingStatusAtLoadPort(portId, i, ref processingStatus))
            //        {
            //            if (transferStatus.Equals(SubstrateTransferStates.AtSource) && 
            //                processingStatus.Equals(ProcessingStates.NeedsProcessing))
            //            {                                
            //                LocationServer.GetLoadPortLocation(portId, i, out location);
            //                key = _substrateManager.GetSubstrateKeyAtLoadPort(portId, i);

            //                return true;
            //            }
            //        }
            //    }               
            //}

            //return false;
        }

        private bool GetNextSlotInformationToPlace(int lpIndex, ref int slot)
        {
            int portId = _loadPortManager.GetLoadPortPortId(lpIndex);
            if (false == _carrierServer.HasCarrier(portId))
                return false;

            // 문이 열려있지 않으면 리턴
            if (false == _loadPortManager.GetDoorState(lpIndex))
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

        private RobotScheduleType GetNotCompletedStatus(int newStep = (int)SchedulerStep.Start)
        {
            _seqNum = newStep;
            return RobotScheduleType.Selection;
        }

        protected override RobotScheduleType DecideNextAction()
        {
            // 1. 공정 설비의 요청 수집
            bool needLoading = _processGroup.IsLoadingRequested(ProcessModuleIndex, ref _requestedLoadingLocation);
            bool needUnloading = _processGroup.IsUnloadingRequested(ProcessModuleIndex, ref _requestedUnloadingLocation);

            switch (_seqNum)
            {
                case (int)SchedulerStep.Start:
                    InitWorkingInfo();
                    _seqNum = (int)SchedulerStep.CollectData;
                    break;

                case (int)SchedulerStep.CollectData:
                    {
                        // 2. 암이 자재를 갖고 있는지 수집
                        bool hasAnySubstrate = false;
                        var robotName = _robotManager.GetRobotName(Index);
                        _substrateManager.GetSubstratesAtRobotAll(robotName, ref _substrates);
                        foreach (var item in _substrates)
                        {
                            if (item.Value == null)
                                continue;

                            bool targetLocationPrepared = false;
                            if (false == GetWorkingInfoToPlace(item.Value, out var locationId, out var locationType))
                            {
                                continue;
                            }

                            switch (locationType)
                            {
                                case ModuleType.LoadPort:
                                    {
                                        LocationServer.GetLocationById(locationId, out var location);
                                        if (location is LoadPortLocation)
                                        {
                                            var lpLoc = location as LoadPortLocation;
                                            if (lpLoc.PortId > 0)
                                            {
                                                int lpIndex = _loadPortManager.GetLoadPortIndexByPortId(lpLoc.PortId);
                                                targetLocationPrepared = (_carrierServer.HasCarrier(lpLoc.PortId) && LoadPortInformations[lpIndex].DoorState);
                                            }
                                        }
                                    }
                                    break;
                                case ModuleType.ProcessModule:
                                    {
                                        //ProcessModuleLocation location = targetLocation as ProcessModuleLocation;
                                        targetLocationPrepared = HasSubstratateToLoadAtProcessModule(locationId);
                                    }
                                    break;
                                default:
                                    break;
                            }

                            hasAnySubstrate |= targetLocationPrepared;
                        }
                        //foreach (var item in _substrates)
                        //{
                        //    hasAnySubstrate |= (item.Value != null);

                        // 결과 1 -> 요청이 전부 없고, 들고 있는 자재도 없으면 할게 없으니 초기 단계로 리턴
                        if (false == needLoading && false == needUnloading && false == hasAnySubstrate)
                        {
                            return GetNotCompletedStatus();
                        }

                        if (hasAnySubstrate)
                        {
                            // 결과 2 -> 자재가 있으면 일단 플레이스를 하려한다.
                            return GetNotCompletedStatus((int)SchedulerStep.SetupWorkInfoToPlace);
                        }
                        else
                        {
                            // 결과 3 -> 자재가 없으면 픽업을 시도한다.
                            return GetNotCompletedStatus((int)SchedulerStep.SetupWorkInfoToPick);
                        }
                    }

                #region <Setup workinginfo to pick>
                case (int)SchedulerStep.SetupWorkInfoToPick:
                    {
                        // 픽하기 전 로드포트 준비상태를 체크한다.
                        // 로딩 요청인 경우
                        if (_requestedLoadingLocation.Count > 0)
                        {
                            if (false == _turnLoad)
                            {
                                _requestedLoadingLocation.Reverse();
                            }
                            _turnLoad = !_turnLoad;
                        }

                        for (int i = 0; i < _requestedLoadingLocation.Count; ++i)
                        {
                            SubstrateType substrateType = SubstrateType.Core;
                            if (GetSubstrateTypeByLoadingLocation(_requestedLoadingLocation[i], ref substrateType))
                            {
                                int lpIndex = 0;/*, slot = 0;*/
                                bool hasCarrier = HasCarriers(substrateType, true, ref lpIndex);
                                if (hasCarrier)
                                {
                                    List<RobotArmTypes> arms = new List<RobotArmTypes>();
                                    if (false == _robotManager.GetAvailableArm(Index, true, ref arms))
                                        return GetNotCompletedStatus();

                                    RobotArmTypes armToWork = arms.First();
                                    switch (substrateType)
                                    {
                                        case SubstrateType.Core:
                                        case SubstrateType.Empty:
                                            {
                                                // Sub 정보를 가져온다.
                                                // 작업할 자재가 있을 때에만 작업하도록 수정
                                                if (GetNextSlotInformationToPick(lpIndex, out var targetLocation, out var key))
                                                {
                                                    //ProcessModuleLocation targetLocation = new ProcessModuleLocation("", "");
                                                    //string processModuleName = _processGroup.GetProcessModuleName(ProcessModuleIndex);
                                                    SetWorkingInfoToWork(armToWork, key, targetLocation.Id, ModuleType.LoadPort);

                                                    return RobotScheduleType.Pick;
                                                }
                                            }
                                            break;

                                        default:
                                            return GetNotCompletedStatus();
                                    }
                                }
                            }
                        }

                        if (_requestedUnloadingLocation.Count > 0)
                        {
                            if (false == _turnUnload)
                            {
                                _requestedUnloadingLocation.Reverse();
                            }
                            _turnUnload = !_turnUnload;
                        }

                        for (int i = 0; i < _requestedUnloadingLocation.Count; ++i)
                        {
                            SubstrateType substrateType = SubstrateType.Core;
                            if (GetSubstrateTypeByUnloadingLocation(_requestedUnloadingLocation[i], ref substrateType))
                            {
                                int lpIndex = 0;/*, slot = 0;*/
                                bool hasCarrier = HasCarriers(substrateType, true, ref lpIndex);
                                if (false == substrateType.Equals(SubstrateType.Empty))
                                {
                                    if (false == hasCarrier)
                                        continue;
                                }

                                int portId = _loadPortManager.GetLoadPortPortId(lpIndex);
                                string locationName = GetProcessModuleUnloadingLocationByPortId(portId);
                                if (string.IsNullOrEmpty(locationName))
                                    return GetNotCompletedStatus();

                                //Substrate substrate = new Substrate();
                                //if (false == _processGroup.GetSubstrateInEntryWay(ProcessModuleIndex, locationName, ref substrate))
                                //    return GetNotCompletedStatus();

                                string key = string.Empty;// substrate.Name;
                                List<RobotArmTypes> arms = new List<RobotArmTypes>();
                                if (false == _robotManager.GetAvailableArm(Index, true, ref arms))
                                    return GetNotCompletedStatus();

                                RobotArmTypes armToWork = arms.First();
                                string processModuleName = _processGroup.GetProcessModuleName(ProcessModuleIndex);

                                SetWorkingInfoToWork(armToWork, key, _requestedUnloadingLocation[i], ModuleType.ProcessModule);

                                return RobotScheduleType.Pick;
                            }
                        }

                        return GetNotCompletedStatus();
                    }
                #endregion </Setup workinginfo to pick>

                #region <Setup workinginfo to place>
                case (int)SchedulerStep.SetupWorkInfoToPlace:
                    {
                        LocationTypesToPlace.Clear();
                        WorkingInfosToPlace.Clear();
                        bool needLoadingToProcessModule = false;

                        // 로봇이 갖고 있는 자재정보를 받아온다.

                        #region <Get substrate informations in robot>
                        foreach (var item in _substrates)
                        {
                            if (item.Value == null)
                                continue;

                            //Location targetLocation = new Location("");
                            if (false == GetWorkingInfoToPlace(item.Value, out var locationId, out var locationType))
                            {
                                continue;
                                //return GetNotCompletedStatus();
                            }

                            RobotWorkingInfo info = new RobotWorkingInfo
                            {
                                ActionArm = item.Key,
                                SubstrateKey = item.Value.UniqueKey,
                                LocationId = locationId,
                                LocationType = locationType
                            };

                            // Target Location 을 이용해 PM으로 보낼 자재 중 요청받은 것이 있는지 여부를 확인한다.
                            needLoadingToProcessModule |= HasSubstratateToLoadAtProcessModule(locationId);

                            LocationTypesToPlace.Add(item.Key, locationType);
                            WorkingInfosToPlace.Add(item.Key, info);
                        }
                        #endregion </Get substrate informations in robot>

                        // 작업할 Arm을 첫 인덱스로 초기화
                        RobotArmTypes armToWork = WorkingInfosToPlace.First().Key;

                        // 작업할 위치 유형을 찾는다.
                        ModuleType targetLocationType = ModuleType.LoadPort;
                        if (needLoadingToProcessModule)
                        {
                            targetLocationType = ModuleType.ProcessModule;
                        }

                        foreach (var item in LocationTypesToPlace)
                        {
                            if (item.Value.Equals(targetLocationType))
                            {
                                armToWork = item.Key;
                                break;
                            }
                        }

                        _workingInfo = WorkingInfosToPlace[armToWork];

                        return RobotScheduleType.Place;
                    }
                #endregion </Setup workinginfo to place>

                case (int)SchedulerStep.CheckLoadPortCondition:
                    break;

                default:
                    break;
            }

            return RobotScheduleType.Selection;
        }

        #endregion </Methods>
    }
}