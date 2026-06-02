using EFEM.Defines.LoadPort;

namespace EFEM.Modules.LoadPort.Scheduler
{
    public interface ICarrierCompletionHandlingPolicy
    {
        // 완료 조건이 만족되었음을 기록한다.
        void RequestCompletion(int portId);

        // 현재 LoadPort 상태를 기준으로 CarrierCompleted 확정을 시도한다.
        void TryFinalizeCompletion(
            int portId,
            LoadPortStateInformation loadPortInformation);

        // Unload action 진입 가능 여부를 판단한다.
        bool ShouldUnloadCarrier(int portId);

        // 완료 요청 pending 상태를 초기화한다.
        void ResetCarrierCompletionRequest(int portId);
    }
}
