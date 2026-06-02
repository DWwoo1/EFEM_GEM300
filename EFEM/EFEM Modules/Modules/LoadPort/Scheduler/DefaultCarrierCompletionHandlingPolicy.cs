using System;

using EFEM.Defines.LoadPort;
using EFEM.MaterialTracking;

namespace EFEM.Modules.LoadPort.Scheduler
{
    public sealed class DefaultCarrierCompletionHandlingPolicy
        : ICarrierCompletionHandlingPolicy
    {
        private readonly CarrierManagementServer _carrierServer;
        private readonly ICarrierCompletionWriter _completionWriter;

        private bool _completionRequested;

        public DefaultCarrierCompletionHandlingPolicy()
            : this(
                CarrierManagementServer.Instance,
                new CarrierCompletionWriter())
        {
        }

        private DefaultCarrierCompletionHandlingPolicy(
            CarrierManagementServer carrierServer,
            ICarrierCompletionWriter completionWriter)
        {
            if (carrierServer == null)
                throw new ArgumentNullException(nameof(carrierServer));

            if (completionWriter == null)
                throw new ArgumentNullException(nameof(completionWriter));

            _carrierServer = carrierServer;
            _completionWriter = completionWriter;
        }

        public void RequestCompletion(int portId)
        {
            if (false == _carrierServer.HasCarrier(portId))
            {
                _completionRequested = false;
                return;
            }

            // 즉시 완료 정책도 동일한 lifecycle을 사용한다.
            _completionRequested = true;
        }

        public void TryFinalizeCompletion(
            int portId,
            LoadPortStateInformation loadPortInformation)
        {
            if (false == _completionRequested)
                return;

            // Default 정책은 추가 조건 없이 즉시 확정한다.
            _completionWriter.SetCompletedIfNeeded(portId);

            _completionRequested = false;
        }

        public bool ShouldUnloadCarrier(int portId)
        {
            if (false == _carrierServer.HasCarrier(portId))
                return false;

            if (_completionRequested)
                return true;

            CarrierAccessStates accessState =
                _carrierServer.GetCarrierAccessingStatus(portId);

            return accessState == CarrierAccessStates.CarrierCompleted
                || accessState == CarrierAccessStates.CarrierStopped;
        }

        public void ResetCarrierCompletionRequest(int portId)
        {
            _completionRequested = false;
        }
    }
}