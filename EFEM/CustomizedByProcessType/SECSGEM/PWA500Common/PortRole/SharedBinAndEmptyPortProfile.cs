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
    public sealed class SharedBinAndEmptyPortProfile : IPortRoleProfile
    {
        public SharedBinAndEmptyPortProfile(
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
            return substrateType.ToString();
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

            if (false == (currentCondition is DefaultCarrierCompletionCondition))
            {
                currentCondition = new DefaultCarrierCompletionCondition();
            }

            lastConditionKey = currentCompletionConditionKey;
        }
        public bool IsEmptyCarrierAtSimulation(SubstrateType substrateType)
        {
            return false;
        }
        public bool IsJobRequiredForLoading(SubstrateType type)
        {
            return true;
        }
        public SubstrateType GetRequestTypeFromPMForBinOrEmptyType(bool isLoading)
        {
            return SubstrateType.Bin1;
        }
        public bool IsBinType(SubstrateType type)
        {
            return type == SubstrateType.Bin1 ||
                type == SubstrateType.Bin2 ||
                type == SubstrateType.Bin3;

            // 개조 후 아래 주석 살려야함
            //return type == SubstrateType.Bin1 ||
            //       type == SubstrateType.Bin2 ||
            //       type == SubstrateType.Bin3;

            // 개조 후 아래 지워야함
            //return type == SubstrateType.Empty;
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

                //case SubstrateType.Bin1:
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

                //case SubstrateType.Bin2:
                //    break;
                //case SubstrateType.Bin3:
                //    break;
                default:
                    break;
            }


            return false;
        }
        public bool IsProcessingCompleted(int portId, SubstrateType substrateType, out List<string> processJobIds)
        {
            processJobIds = new List<string>();
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

            //switch (substrateType)
            //{
            //    case SubstrateType.Bin1:
            //    case SubstrateType.Core:
            //        {
            //            var carrierId = CarrierServer.GetCarrierId(portId);
            //            var jobs = SubstrateJobBindingService.Instance.GetProcessJobIdsByCarrier(carrierId);
            //            foreach (var item in jobs)
            //            {
            //                var processed = JobCompletionService.Instance.AreAllMaterialsProcessed(item);
            //                if (processed)
            //                {
            //                    processJobIds.Add(item);
            //                }
            //            }

            //            return processJobIds.Count > 0;
            //        }

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

            //default:
            //        return false;
            //}
        }

        public CheckingCarrierCodeToUnload FindWellknownProtInfoBySubstrateType(
            Substrate substrate,
            SubstrateType subType,
            Func<int, SubstrateType> funcGetSubstrateTypeByLoadPortIndex,
            ref int portId,
            ref int slot,
            ref string description)
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
    }
}
