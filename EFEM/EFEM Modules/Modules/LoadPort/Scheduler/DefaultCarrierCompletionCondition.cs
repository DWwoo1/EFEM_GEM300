using System;
using System.Collections.Generic;

using EFEM.Modules;
using EFEM.MaterialTracking;
using EFEM.Defines.LoadPort;
using EFEM.Defines.MaterialTracking;
using EFEM.Defines.AtmRobot;

namespace EFEM.Modules.LoadPort.Scheduler
{
    public interface ICarrierCompletionCondition
    {
        bool IsCarrierCompleted(int portId, LoadPortTransferStates transferState);
    }

    /// <summary>
    /// 기본 Carrier 완료 판정 조건.
    /// BaseLoadPortActionScheduler.IsCarrierCompleted 로직과 동일한 판정 규칙을 사용한다.
    /// </summary>
    public sealed class DefaultCarrierCompletionCondition : ICarrierCompletionCondition
    {
        private readonly SubstrateManager _substrateManager;
        private readonly CarrierManagementServer _carrierServer;
        private readonly ProcessModuleGroup _processGroup;
        private readonly AtmRobotManager _robotManager;

        private List<Substrate> _processModuleSubstratesBuffer;
        private Dictionary<RobotArmTypes, Substrate> _robotSubstratesByArmBuffer;

        public DefaultCarrierCompletionCondition()
            : this(
                SubstrateManager.Instance,
                CarrierManagementServer.Instance,
                ProcessModuleGroup.Instance,
                AtmRobotManager.Instance)
        {
        }

        private DefaultCarrierCompletionCondition(
            SubstrateManager substrateManager,
            CarrierManagementServer carrierServer,
            ProcessModuleGroup processGroup,
            AtmRobotManager robotManager)
        {
            if (substrateManager == null)
                throw new ArgumentNullException(nameof(substrateManager));

            if (carrierServer == null)
                throw new ArgumentNullException(nameof(carrierServer));

            if (processGroup == null)
                throw new ArgumentNullException(nameof(processGroup));

            if (robotManager == null)
                throw new ArgumentNullException(nameof(robotManager));

            _substrateManager = substrateManager;
            _carrierServer = carrierServer;
            _processGroup = processGroup;
            _robotManager = robotManager;

            _processModuleSubstratesBuffer = new List<Substrate>();
            _robotSubstratesByArmBuffer = new Dictionary<RobotArmTypes, Substrate>();
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

            // BaseLoadPortActionScheduler.IsSubstrateAtDestination(portId)와 동일한 의미.
            if (false == AreSourcePortSubstratesCompleted(portId))
                return false;

            // BaseLoadPortActionScheduler.IsCarrierCompleted()의 마지막 LP 전체 검사와 동일한 의미.
            if (false == AreAllLoadPortSubstratesCompleted(portId))
                return false;

            return true;
        }
        private bool AreSourcePortSubstratesCompleted(int portId)
        {
            if (false == AreSourceLoadPortSubstratesCompleted(portId))
                return false;

            if (false == AreSourceProcessModuleSubstratesCompleted(portId))
                return false;

            if (false == AreSourceRobotSubstratesCompleted(portId))
                return false;

            return true;
        }
        private bool AreSourceLoadPortSubstratesCompleted(int portId)
        {
            var substratesAtLoadPort = _substrateManager.GetSubstratesAtLoadPort(portId);

            if (substratesAtLoadPort == null)
                return false;

            foreach (var item in substratesAtLoadPort)
            {
                Substrate substrate = item.Value;

                if (IsSubstrateFromPortIncomplete(substrate, portId))
                    return false;
            }

            return true;
        }
        private bool AreSourceProcessModuleSubstratesCompleted(int portId)
        {
            for (int i = 0; i < _processGroup.Count; ++i)
            {
                string processModuleName = _processGroup.GetProcessModuleName(i);

                _processModuleSubstratesBuffer.Clear();

                bool hasSubstrates = _substrateManager.GetSubstratesAtProcessModule(
                    processModuleName,
                    ref _processModuleSubstratesBuffer);

                if (hasSubstrates == false)
                    continue;

                foreach (Substrate substrate in _processModuleSubstratesBuffer)
                {
                    if (IsSubstrateFromPortIncomplete(substrate, portId))
                        return false;
                }
            }

            return true;
        }
        private bool AreSourceRobotSubstratesCompleted(int portId)
        {
            for (int i = 0; i < _robotManager.Count; ++i)
            {
                string robotName = _robotManager.GetRobotName(i);

                if (_robotSubstratesByArmBuffer == null)
                    _robotSubstratesByArmBuffer =
                        new Dictionary<RobotArmTypes, Substrate>();
                else
                    _robotSubstratesByArmBuffer.Clear();

                bool hasSubstrates = _substrateManager.GetSubstratesAtRobotAll(
                    robotName,
                    ref _robotSubstratesByArmBuffer);

                if (hasSubstrates == false)
                    continue;

                if (_robotSubstratesByArmBuffer == null)
                    continue;

                foreach (var item in _robotSubstratesByArmBuffer)
                {
                    Substrate substrate = item.Value;

                    if (IsSubstrateFromPortIncomplete(substrate, portId))
                        return false;
                }
            }

            return true;
        }

        private bool AreAllLoadPortSubstratesCompleted(int portId)
        {
            var substratesAtLoadPort = _substrateManager.GetSubstratesAtLoadPort(portId);

            if (substratesAtLoadPort == null)
                return false;

            foreach (var item in substratesAtLoadPort)
            {
                Substrate substrate = item.Value;

                if (substrate == null)
                    continue;

                if (false == IsProcessingCompleted(
                    substrate.TransportStatus,
                    substrate.ProcessingStatus))
                {
                    return false;
                }
            }

            return true;
        }

        private bool IsSubstrateFromPortIncomplete(
            Substrate substrate,
            int portId)
        {
            if (substrate == null)
                return false;

            if (substrate.SourcePortId.Equals(portId) == false)
                return false;

            return IsProcessingCompleted(
                substrate.TransportStatus,
                substrate.ProcessingStatus) == false;
        }

        private bool IsProcessingCompleted(
            TransportStates transferStatus,
            ProcessingStates processingStatus)
        {
            switch (processingStatus)
            {
                case ProcessingStates.Rejected:
                case ProcessingStates.Stopped:
                case ProcessingStates.Aborted:
                case ProcessingStates.Skipped:
                case ProcessingStates.Lost:
                    break;

                default:
                    return transferStatus.Equals(TransportStates.AtDestination);
            }

            return true;
        }

        private static string FormatSubstrate(Substrate substrate)
        {
            if (substrate == null)
                return "Substrate=null";

            return string.Format(
                "Key={0}, SourcePort={1}, SourceSlot={2}, Transport={3}, Processing={4}",
                substrate.UniqueKey,
                substrate.SourcePortId,
                substrate.SourceSlot,
                substrate.TransportStatus,
                substrate.ProcessingStatus);
        }
    }
}