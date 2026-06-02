using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using EFEM.Modules;
using EFEM.MaterialTracking;
using EFEM.Defines.LoadPort;
using EFEM.Defines.MaterialTracking;
using EFEM.Defines.AtmRobot;
using EFEM.Modules.LoadPort.Scheduler;

namespace EFEM.CustomizedByProcessType.PWA500Common
{
    /// <summary>
    /// LoadPort의 자재가 모두 비워졌을 때 Carrier 완료로 판단하는 조건.
    /// </summary>
    public sealed class CarrierEmptiedCompletionCondition : ICarrierCompletionCondition
    {
        private readonly SubstrateManager _substrateManager;
        private readonly CarrierManagementServer _carrierServer;
        private readonly ProcessModuleGroup _processGroup;
        private readonly AtmRobotManager _robotManager;

        private List<Substrate> _processModuleSubstratesBuffer;
        private Dictionary<RobotArmTypes, Substrate> _robotSubstratesByArmBuffer;

        public CarrierEmptiedCompletionCondition()
            : this(
                SubstrateManager.Instance,
                CarrierManagementServer.Instance,
                ProcessModuleGroup.Instance,
                AtmRobotManager.Instance)
        {
        }

        private CarrierEmptiedCompletionCondition(
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

            // 자재가 뭐라도 있으면 완료된게 아님
            if (_substrateManager.HasAnySubstrateAtLoadPort(portId))
                return false;

            // TODO : 한계시간에 의한 조건을 추가해야 한다. -> 고객사에서 운용 시나리오가 아직 확립되지 않음

            return true;
        }
    }
}
