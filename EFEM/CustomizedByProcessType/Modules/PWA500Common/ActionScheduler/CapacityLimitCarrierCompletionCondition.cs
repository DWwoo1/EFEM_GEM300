using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using FrameOfSystem3.Recipe;

using EFEM.Modules;
using EFEM.MaterialTracking;
using EFEM.Defines.LoadPort;
using EFEM.Defines.MaterialTracking;
using EFEM.Defines.AtmRobot;
using EFEM.Modules.LoadPort.Scheduler;

namespace EFEM.CustomizedByProcessType.PWA500Common
{
    /// <summary>
    /// 슬롯 점유 상태와 Capacity Limit 기준으로 Carrier 완료 여부를 판단한다.
    /// </summary>
    internal sealed class CapacityLimitCarrierCompletionCondition : ICarrierCompletionCondition
    {
        private readonly LoadPortManager _loadPortManager;
        private readonly CarrierManagementServer _carrierServer;
        private readonly SubstrateManager _substrateManager;
        private readonly Recipe _recipe;

        private readonly int _loadPortIndex;
        private readonly string _paramUseCapacityLimit;
        private readonly string _paramUseAvailableCapacity;

        public bool UseCapacityLimit { get; private set; }
        public int MaxCapacity { get; private set; }
        public int AvailableCapacity { get; private set; }

        public CapacityLimitCarrierCompletionCondition(
            int loadPortIndex,
            string paramUseCapacityLimit,
            string paramUseAvailableCapacity)
            : this(
                loadPortIndex,
                paramUseCapacityLimit,
                paramUseAvailableCapacity,
                LoadPortManager.Instance,
                CarrierManagementServer.Instance,
                SubstrateManager.Instance,
                Recipe.GetInstance())
        {
        }

        private CapacityLimitCarrierCompletionCondition(
            int loadPortIndex,
            string paramUseCapacityLimit,
            string paramUseAvailableCapacity,
            LoadPortManager loadPortManager,
            CarrierManagementServer carrierServer,
            SubstrateManager substrateManager,
            Recipe recipe)
        {
            if (loadPortManager == null)
                throw new ArgumentNullException(nameof(loadPortManager));

            if (carrierServer == null)
                throw new ArgumentNullException(nameof(carrierServer));

            if (substrateManager == null)
                throw new ArgumentNullException(nameof(substrateManager));

            if (recipe == null)
                throw new ArgumentNullException(nameof(recipe));

            if (string.IsNullOrEmpty(paramUseCapacityLimit))
                throw new ArgumentException("Parameter name is null or empty.", nameof(paramUseCapacityLimit));

            if (string.IsNullOrEmpty(paramUseAvailableCapacity))
                throw new ArgumentException("Parameter name is null or empty.", nameof(paramUseAvailableCapacity));

            _loadPortIndex = loadPortIndex;
            _paramUseCapacityLimit = paramUseCapacityLimit;
            _paramUseAvailableCapacity = paramUseAvailableCapacity;

            _loadPortManager = loadPortManager;
            _carrierServer = carrierServer;
            _substrateManager = substrateManager;
            _recipe = recipe;
        }

        public bool IsCarrierCompleted(
            int portId,
            LoadPortTransferStates transferState)
        {
            if (transferState != LoadPortTransferStates.TransferBlocked)
                return false;

            if (false == _carrierServer.HasCarrier(portId))
                return false;

            int capacity = _carrierServer.GetCapacity(portId);
            if (capacity <= 0)
                return false;

            UseCapacityLimit = _recipe.GetValue(
                EN_RECIPE_TYPE.EQUIPMENT,
                _paramUseCapacityLimit,
                false);

            MaxCapacity = capacity;

            LoadPortLoadingMode loadingMode =
                _loadPortManager.GetCarrierLoadingType(_loadPortIndex);

            int completedCount = 0;

            for (int slot = 1; slot <= capacity; ++slot)
            {
                if (ShouldSkipSlot(slot, loadingMode))
                    continue;

                if (false == _substrateManager.HasSubstrateAtLoadPort(portId, slot))
                {
                    if (false == UseCapacityLimit)
                        return false;

                    continue;
                }

                if (IsSubstrateAtDestination(portId, slot))
                    ++completedCount;
            }

            if (UseCapacityLimit)
                return IsAvailableCapacitySatisfied(completedCount);

            return true;
        }

        private bool ShouldSkipSlot(
            int slot,
            LoadPortLoadingMode loadingMode)
        {
            // 기존 로직 유지:
            // Cassette / ClosedCassette 모드에서는 1번 슬롯을 작업 대상에서 제외한다.
            if (slot != 1)
                return false;

            return loadingMode == LoadPortLoadingMode.Cassette ||
                   loadingMode == LoadPortLoadingMode.ClosedCassette;
        }

        private bool IsSubstrateAtDestination(
            int portId,
            int slot)
        {
            TransportStates transferStatus = TransportStates.AtSource;

            if (false == _substrateManager.GetTransferStatusAtLoadPort(
                portId,
                slot,
                ref transferStatus))
            {
                return false;
            }

            return transferStatus == TransportStates.AtDestination;
        }

        private bool IsAvailableCapacitySatisfied(
            int completedCount)
        {
            AvailableCapacity = _recipe.GetValue(
                EN_RECIPE_TYPE.EQUIPMENT,
                _paramUseAvailableCapacity,
                MaxCapacity);

            return completedCount >= AvailableCapacity;
        }
    }
}
