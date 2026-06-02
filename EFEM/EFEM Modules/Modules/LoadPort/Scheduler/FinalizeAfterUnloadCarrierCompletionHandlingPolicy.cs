using System;

using EFEM.Modules;
using EFEM.Defines.LoadPort;
using EFEM.MaterialTracking;

namespace EFEM.Modules.LoadPort.Scheduler
{
    public sealed class FinalizeAfterUnloadCarrierCompletionHandlingPolicy
        : ICarrierCompletionHandlingPolicy
    {
        private readonly CarrierManagementServer _carrierServer;
        private readonly ICarrierCompletionWriter _completionWriter;

        private bool _completionRequested;

        public FinalizeAfterUnloadCarrierCompletionHandlingPolicy()
            : this(
                CarrierManagementServer.Instance,
                new CarrierCompletionWriter())
        {
        }

        private FinalizeAfterUnloadCarrierCompletionHandlingPolicy(
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

            // Door close 전까지 완료 확정을 보류한다.
            _completionRequested = true;
        }

        public void TryFinalizeCompletion(
            int portId,
            LoadPortStateInformation loadPortInformation)
        {
            if (false == _completionRequested)
                return;

            if (false == _carrierServer.HasCarrier(portId))
            {
                _completionRequested = false;
                return;
            }

            // 정책 조건을 만족할 때만 CarrierCompleted로 확정한다.
            if (false == CanFinalizeCarrierCompletion(loadPortInformation))
                return;

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

            return accessState.Equals(CarrierAccessStates.CarrierCompleted)
                || accessState.Equals(CarrierAccessStates.CarrierStopped);
        }

        public void ResetCarrierCompletionRequest(int portId)
        {
            _completionRequested = false;
        }

        private bool CanFinalizeCarrierCompletion(
            LoadPortStateInformation loadPortInformation)
        {
            if (loadPortInformation == null)
                return false;

            // Door가 닫힌 뒤 CarrierCompleted 저장을 허용한다.
            return false == loadPortInformation.DoorState;
        }
    }
}