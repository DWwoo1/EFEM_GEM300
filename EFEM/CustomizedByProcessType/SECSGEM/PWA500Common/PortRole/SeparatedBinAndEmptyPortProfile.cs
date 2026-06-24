using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using FrameOfSystem3.Recipe;

using EFEM.Modules;
using EFEM.Defines.LoadPort;
using EFEM.Defines.MaterialTracking;
using EFEM.MaterialTracking;
using EFEM.Jobs.Manager;
using EFEM.Jobs.Binding;
using EFEM.Jobs.Completion;
using EFEM.MaterialTracking;
using EFEM.Modules.LoadPort.Scheduler;
using EFEM.CustomizedByProcessType.PWA500Common;

namespace FrameOfSystem3.SECSGEM.PortRole
{
    public sealed class SeparatedBinAndEmptyPortProfile : IPortRoleProfile
    {
        public SeparatedBinAndEmptyPortProfile(
            Recipe.Recipe recipe,
            LoadPortManager loadPortManager,
            CarrierManagementServer carrierServer,
            SubstrateManager substrateManager)
        {
            Recipe = recipe;
            LoadPortManager = loadPortManager;
            CarrierServer = carrierServer;
            SubstrateManager = substrateManager;
        }

        private Recipe.Recipe Recipe { get; set; }
        private LoadPortManager LoadPortManager { get; set; }
        private CarrierManagementServer CarrierServer { get; set; }
        private SubstrateManager SubstrateManager { get; set; }

        private string MakeCompletionConditionKey(SubstrateType substrateType)
        {
            switch (substrateType)
            {
                case SubstrateType.Bin1:
                case SubstrateType.Bin2:
                case SubstrateType.Bin3:
                    {
                        int offsetIndex = substrateType - SubstrateType.Bin1;

                        Recipe.PARAM_EQUIPMENT paramUseCapacity =
                            PARAM_EQUIPMENT.UseCapacityLimitBin1 + offsetIndex;

                        Recipe.PARAM_EQUIPMENT paramCapacityLimit =
                            PARAM_EQUIPMENT.AvailableCarrierCapacityBin1 + offsetIndex;

                        bool useCapacityLimit =
                            Recipe.GetValue(
                                EN_RECIPE_TYPE.EQUIPMENT,
                                paramUseCapacity.ToString(),
                                false);

                        int capacityLimit =
                            Recipe.GetValue(
                                EN_RECIPE_TYPE.EQUIPMENT,
                                paramCapacityLimit.ToString(),
                                0);

                        return string.Format(
                            "{0}|{1}|{2}|{3}|{4}",
                            substrateType,
                            paramUseCapacity,
                            useCapacityLimit,
                            paramCapacityLimit,
                            capacityLimit);
                    }

                default:
                    return substrateType.ToString();
            }
        }

        public void UpdateConditionAndPolicy(
            int portId,
            SubstrateType substrateType,
            ref string lastConditionKey,
            ref ICarrierCompletionHandlingPolicy currentPolicy,
            ref ICarrierCompletionCondition currentCondition)
        {
            if (false == (currentPolicy is ICarrierCompletionHandlingPolicy))
            {
                currentPolicy = new FinalizeAfterUnloadCarrierCompletionHandlingPolicy();
            }

            string currentCompletionConditionKey = MakeCompletionConditionKey(substrateType);

            bool isCompletionConditionChanged =
                lastConditionKey != null &&
                false == string.Equals(
                    lastConditionKey,
                    currentCompletionConditionKey,
                    StringComparison.Ordinal);

            if (isCompletionConditionChanged)
            {
                currentPolicy.ResetCarrierCompletionRequest(portId);
                currentCondition = null;
            }

            switch (substrateType)
            {
                case SubstrateType.Empty:
                    if (false == (currentCondition is CarrierEmptiedCompletionCondition))
                    {
                        currentCondition = new CarrierEmptiedCompletionCondition();
                    }
                    break;

                case SubstrateType.Bin1:
                case SubstrateType.Bin2:
                case SubstrateType.Bin3:
                    if (false == (currentCondition is CapacityLimitCarrierCompletionCondition))
                    {
                        int offsetIndex = substrateType - SubstrateType.Bin1;

                        PARAM_EQUIPMENT paramUseCapacity =
                            PARAM_EQUIPMENT.UseCapacityLimitBin1 + offsetIndex;

                        PARAM_EQUIPMENT paramCapacityLimit =
                            PARAM_EQUIPMENT.AvailableCarrierCapacityBin1 + offsetIndex;
                        
                        int index = LoadPortManager.GetLoadPortIndexByPortId(portId);
                        currentCondition = new CapacityLimitCarrierCompletionCondition(
                            index,
                            paramUseCapacity.ToString(),
                            paramCapacityLimit.ToString());
                    }
                    break;

                default:
                    if (false == (currentCondition is DefaultCarrierCompletionCondition))
                    {
                        currentCondition = new DefaultCarrierCompletionCondition();
                    }
                    break;
            }

            lastConditionKey = currentCompletionConditionKey;
        }
        public bool IsEmptyCarrierAtSimulation(SubstrateType substrateType)
        {
            switch (substrateType)
            {
                case SubstrateType.Bin1:
                case SubstrateType.Bin2:
                case SubstrateType.Bin3:
                    return true;
                default:
                    return false;
            }
        }
        public bool IsJobRequiredForLoading(SubstrateType type)
        {
            return type == SubstrateType.Core || type == SubstrateType.Empty;
        }
        public bool IsBinType(SubstrateType type)
        {
            return type == SubstrateType.Bin1 ||
                   type == SubstrateType.Bin2 ||
                   type == SubstrateType.Bin3;
        }
        public SubstrateType GetRequestTypeFromPMForBinOrEmptyType(bool isLoading)
        {
            // TODO : Bin1 밖에 없어서 수정해야함
            return isLoading ? SubstrateType.Empty : SubstrateType.Bin1;
        }
        public bool GetNextSlotInformationToPick(int lpIndex,
            SubstrateType type,
            string processJobId,
            out LoadPortLocation location,
            out string substrateKey)
        {
            location = null;
            substrateKey = string.Empty;
            int portId = LoadPortManager.GetLoadPortPortId(lpIndex);
            // 로드포트의 슬롯 별 자재를 가져온다.
            var substrates = SubstrateManager.GetSubstratesAtLoadPort(portId);
            if (substrates == null ||
                substrates.Count <= 0)
                return false;

            switch (type)
            {
                case SubstrateType.Core:
                case SubstrateType.Empty:
                    {
                        var carrierId = CarrierServer.GetCarrierId(portId);
                        IJobManager manager = JobManager.Instance;
                        var job = manager.GetProcessJobOrDefault(processJobId);
                        if (job == null)
                            return false;

                        foreach (var item in job.MaterialInfo)
                        {
                            if (false == string.Equals(carrierId, item.Key, StringComparison.OrdinalIgnoreCase))
                                continue;

                            foreach (var slot in item.Value)
                            {
                                if (false == substrates.TryGetValue(slot, out var s))
                                    continue;

                                if (false == LocationServer.GetLoadPortLocation(portId, slot, out var loc))
                                    continue;

                                if (s.ProcessingStatus != ProcessingStates.NeedsProcessing)
                                    continue;

                                location = loc;
                                substrateKey = s.UniqueKey;

                                return true;
                            }
                        }
                    }
                    break;
                
                //case SubstrateType.Empty:
                //    {
                //        foreach (var item in substrates)
                //        {
                //            if (item.Value == null)
                //                continue;

                //            var transf = item.Value.TransportStatus;
                //            var proc = item.Value.ProcessingStatus;
                //            if (transf == TransportStates.AtSource &&
                //                proc == ProcessingStates.NeedsProcessing)
                //            {
                //                var locId = item.Value.LocationId;
                //                if (false == LocationServer.FindLocationById(locId, out var loc))
                //                    continue;

                //                location = loc as LoadPortLocation;
                //                substrateKey = item.Value.UniqueKey;
                //                if (location != null)
                //                {
                //                    return true;
                //                }
                //            }
                //        }
                //    }
                //    break;

                case SubstrateType.Bin1:
                    break;
                case SubstrateType.Bin2:
                    break;
                case SubstrateType.Bin3:
                    break;
                default:
                    break;
            }


            return false;
        }
        public bool IsProcessingCompleted(int portId, SubstrateType substrateType, out List<string> processJobIds)
        {
            processJobIds = new List<string>();
            switch (substrateType)
            {
                case SubstrateType.Core:
                case SubstrateType.Empty:
                    {
                        var carrierId = CarrierServer.GetCarrierId(portId);
                        var jobs = SubstrateJobBindingService.Instance.GetProcessJobIdsByCarrier(carrierId);
                        foreach (var item in jobs)
                        {
                            var processed = JobCompletionService.Instance.AreAllMaterialsProcessed(item);
                            if (processed)
                            {
                                processJobIds.Add(item);
                            }
                        }

                        return processJobIds.Count > 0;
                    }

                //case SubstrateType.Empty:
                //    return false;

                //case SubstrateType.Bin1:
                //case SubstrateType.Bin2:
                //case SubstrateType.Bin3:
                //    {
                //        int offsetIndex = substrateType - SubstrateType.Bin1;
                //        // Capacity 확인 필요
                //        PARAM_EQUIPMENT paramUseCapacity =
                //            PARAM_EQUIPMENT.UseCapacityLimitBin1 + offsetIndex;

                //        PARAM_EQUIPMENT paramCapacityLimit =
                //            PARAM_EQUIPMENT.AvailableCarrierCapacityBin1 + offsetIndex;

                //        if (Recipe.GetValue(EN_RECIPE_TYPE.EQUIPMENT, paramUseCapacity.ToString(), false))
                //        {
                //            var capacity = Recipe.GetValue(EN_RECIPE_TYPE.EQUIPMENT, paramCapacityLimit.ToString(), 12);
                //            var substrates = SubstrateManager.GetSubstratesAtLoadPort(portId);
                //            int count = 0;
                //            foreach (var item in substrates)
                //            {
                //                if (item.Value.ProcessingStatus == ProcessingStates.Processed)
                //                {
                //                    ++count;
                //                }
                //            }

                //            if (capacity <= count)
                //            {
                //                // 완료 처리
                //                var carrierId = CarrierServer.GetCarrierId(portId);
                //                var jobs = SubstrateJobBindingService.Instance.GetProcessJobIdsByCarrier(carrierId);
                //                processJobIds = new List<string>(jobs);
                //                return true;
                //            }
                //        }

                //        return false;
                //    }

                default:
                    return false;
            }
        }

        public CheckingCarrierCodeToUnload FindWellknownProtInfoBySubstrateType(
            Substrate substrate,
            SubstrateType subType,
            Func<int, SubstrateType> funcGetSubstrateTypeByLoadPortIndex,
            ref int portId,
            ref int slot,
            ref string description)
        {
            // 개조 후 : 작업 완료된 자재는 Bin 전용 포트로 찾아 넣는다.
            description = string.Empty;
            portId = -1; slot = -1;
            switch (subType)
            {
                case SubstrateType.Bin1:
                case SubstrateType.Bin2:
                case SubstrateType.Bin3:
                    {
                        int lpIndex = -1;
                        for (int i = 0; i < LoadPortManager.Count; ++i)
                        {
                            SubstrateType convertedSubType = funcGetSubstrateTypeByLoadPortIndex(i);
                            if (false == subType.Equals(convertedSubType))
                                continue;

                            lpIndex = i;
                            break;
                        }

                        // 비정상 포트
                        portId = LoadPortManager.GetLoadPortPortId(lpIndex);
                        if (lpIndex < 0 || portId <= 0)
                        {
                            description = ErrorDescriptionsForMaterialHanding.ErrorDescriptionForInvalidSubstratePortInfo;
                            return CheckingCarrierCodeToUnload.InvalidPortInfo;
                        }

                        // 포트 미사용
                        if (false == LoadPortManager.IsLoadPortEnabled(lpIndex))
                        {
                            description = ErrorDescriptionsForMaterialHanding.ErrorDescriptionForLoadPortNotEnabled;
                            return CheckingCarrierCodeToUnload.PortNotEnabled;
                            //return false;
                        }

                        // 포트상태 비정상
                        if (false == CarrierServer.HasCarrier(portId) ||
                            CarrierServer.GetCarrierAccessingStatus(portId).Equals(CarrierAccessStates.CarrierCompleted) ||
                            CarrierServer.GetCarrierAccessingStatus(portId).Equals(CarrierAccessStates.CarrierStopped))
                        {
                            description = ErrorDescriptionsForMaterialHanding.ErrorDescriptionForDoesntHaveCarrier;

                            return CheckingCarrierCodeToUnload.DoesNotHaveToAccessCarrier;
                        }

                        // 문이 닫힘
                        if (false == LoadPortManager.GetDoorState(lpIndex))
                        {
                            description = ErrorDescriptionsForMaterialHanding.ErrorDescriptionForDoorIsNotOpened;
                            return CheckingCarrierCodeToUnload.DoorIsNotOpened;
                            //return false;
                        }

                        // 포트에서 순차탐색한다.
                        int capacity = CarrierServer.GetCapacity(portId);
                        var substrates = SubstrateManager.GetSubstratesAtLoadPort(portId);
                        var loadingMode = LoadPortManager.GetCarrierLoadingType(lpIndex);
                        for (int i = 1; i <= capacity; ++i)
                        {
                            if (i == 1 &&
                                (loadingMode == LoadPortLoadingMode.Cassette ||
                                loadingMode == LoadPortLoadingMode.ClosedCassette))
                                continue;

                            if (false == substrates.ContainsKey(i))
                            {
                                slot = i;
                                return CheckingCarrierCodeToUnload.Ok;
                            }
                        }

                        description = ErrorDescriptionsForMaterialHanding.ErrorDescriptionForSlotIsFull;
                        return CheckingCarrierCodeToUnload.SlotsIsFull;
                        //return false;
                    }

                default:
                    {
                        portId = substrate.SourcePortId;
                        slot = substrate.SourceSlot;
                        var lpIndex = LoadPortManager.GetLoadPortIndexByPortId(portId);

                        if (false == LoadPortManager.IsLoadPortEnabled(lpIndex))
                        {
                            description = ErrorDescriptionsForMaterialHanding.ErrorDescriptionForLoadPortNotEnabled;
                            return CheckingCarrierCodeToUnload.PortNotEnabled;
                            //return false;
                        }

                        if (false == CarrierServer.HasCarrier(portId) ||
                            CarrierServer.GetCarrierAccessingStatus(portId).Equals(CarrierAccessStates.CarrierCompleted) ||
                            CarrierServer.GetCarrierAccessingStatus(portId).Equals(CarrierAccessStates.CarrierStopped))
                        {
                            description = ErrorDescriptionsForMaterialHanding.ErrorDescriptionForDoesntHaveCarrier;

                            return CheckingCarrierCodeToUnload.DoesNotHaveToAccessCarrier;
                        }

                        if (false == LoadPortManager.GetDoorState(lpIndex))
                        {
                            description = ErrorDescriptionsForMaterialHanding.ErrorDescriptionForDoorIsNotOpened;
                            return CheckingCarrierCodeToUnload.DoorIsNotOpened;
                            //return false;
                        }

                        return CheckingCarrierCodeToUnload.Ok;
                    }
                    //return true;
            }
        }
    }
}
