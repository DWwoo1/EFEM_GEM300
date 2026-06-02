using System;
using System.Collections.Generic;

using EFEM.MaterialTracking;
using EFEM.Modules.LoadPort.Scheduler;
using EFEM.CustomizedByProcessType.PWA500Common;

namespace FrameOfSystem3.SECSGEM.PortRole
{
    interface IPortRoleProfile
    {
        void UpdateConditionAndPolicy(
            int portId,
            SubstrateType substrateType,
            ref string lastConditionKey,
            ref ICarrierCompletionHandlingPolicy currentPolicy,
            ref ICarrierCompletionCondition currentCondition);
        bool IsJobRequiredForLoading(SubstrateType type);
        bool IsBinType(SubstrateType type);
        SubstrateType GetRequestTypeFromPMForBinOrEmptyType(bool isLoading);
        bool IsEmptyCarrierAtSimulation(SubstrateType substrateType);
        bool GetNextSlotInformationToPick(int lpIndex,
            SubstrateType type,
            string processJobId,
            out LoadPortLocation location,
            out string substrateKey);
        bool IsProcessingCompleted(int portId, SubstrateType substrateType, out List<string> processJobIds);
        CheckingCarrierCodeToUnload FindWellknownProtInfoBySubstrateType(
            Substrate substrate,
            SubstrateType subType,
            Func<int, SubstrateType> funcGetSubstrateTypeByLoadPortIndex,
            ref int portId,
            ref int slot,
            ref string description);
    }
}
