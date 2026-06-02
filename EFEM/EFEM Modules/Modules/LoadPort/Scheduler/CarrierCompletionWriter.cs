using System;

using EFEM.Defines.LoadPort;
using EFEM.MaterialTracking;

namespace EFEM.Modules.LoadPort.Scheduler
{
    public interface ICarrierCompletionWriter
    {
        // CarrierCompleted 저장을 공통 처리한다.
        void SetCompletedIfNeeded(int portId);
    }

    public sealed class CarrierCompletionWriter : ICarrierCompletionWriter
    {
        private readonly CarrierManagementServer _carrierServer;

        public CarrierCompletionWriter()
            : this(CarrierManagementServer.Instance)
        {
        }

        private CarrierCompletionWriter(
            CarrierManagementServer carrierServer)
        {
            if (carrierServer == null)
                throw new ArgumentNullException(nameof(carrierServer));

            _carrierServer = carrierServer;
        }

        public void SetCompletedIfNeeded(int portId)
        {
            if (false == _carrierServer.HasCarrier(portId))
                return;

            CarrierAccessStates accessState =
                _carrierServer.GetCarrierAccessingStatus(portId);

            // 중복 저장 방지
            if (accessState == CarrierAccessStates.CarrierCompleted)
                return;

            _carrierServer.SetCarrierAccessingStatus(
                portId,
                CarrierAccessStates.CarrierCompleted);

            _carrierServer.SaveCarrierData(portId);
        }
    }
}
