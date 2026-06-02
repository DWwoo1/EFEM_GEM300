using System.Collections.Generic;

namespace EFEM.Jobs.Binding
{
    public enum JobBindingValidationMode
    {
        // ControlJob 생성 전에도 ProcessJob 단독 바인딩 여부를 볼 수 있다.
        ProcessJobOnly = 0,

        // ControlJob 실행 전 검사에서는 ControlJobId까지 일치해야 한다.
        ProcessJobAndControlJob = 1
    }

    /// <summary>
    /// Substrate와 Job 정보를 연결하는 전용 인터페이스.
    ///
    /// 이 객체의 책임은 "바인딩" 하나다.
    /// - Job을 생성하지 않는다.
    /// - Carrier를 생성하지 않는다.
    /// - SlotMap을 검증하지 않는다.
    /// - Substrate를 생성하지 않는다.
    /// - ControlJob / ProcessJob 상태 전이를 수행하지 않는다.
    ///
    /// 단지 이미 존재하는 Job 정보와 이미 검증되어 생성된 Substrate를 찾아
    /// Substrate.ProcessJobId / ControlJobId / RecipeId에 기록한다.
    /// </summary>
    public interface ISubstrateJobBinder
    {
        /// <summary>
        /// ProcessJob 기준으로 현재 존재하는 Substrate에 Job 정보를 바인딩한다.
        ///
        /// 호출 권장 시점:
        /// - PRJobCreate 성공 직후
        /// - ProcessJobCreated callback 수신 후
        ///
        /// 재료가 아직 도착하지 않은 경우에는 아무 것도 기록하지 않고 종료한다.
        /// 이후 SlotMap 검증 시 BindByCarrierPort가 다시 호출되므로 문제 없다.
        /// </summary>
        void BindByProcessJob(string processJobId);

        /// <summary>
        /// ControlJob 기준으로 연결된 ProcessJob들을 찾고,
        /// 각 ProcessJob의 재료에 ControlJobId / ProcessJobId / RecipeId를 바인딩한다.
        ///
        /// 호출 권장 시점:
        /// - ControlJobCreate 성공 직후
        /// - ControlJobCreated callback 수신 후
        /// </summary>
        void BindByControlJob(string controlJobId);

        /// <summary>
        /// Carrier SlotMap 검증/반영 후 호출한다.
        ///
        /// 이 메서드는 "재료가 나중에 도착한 경우"를 처리한다.
        /// Job은 이미 생성되어 있고 Substrate가 없어서 바인딩하지 못했다가,
        /// SlotMap 반영으로 Substrate가 생성되면 여기서 다시 바인딩한다.
        /// </summary>
        void BindByCarrierPort(int portId);

        /// <summary>
        /// ControlJob을 EXECUTING으로 보내기 전에,
        /// 필요한 Substrate에 Job 정보가 모두 바인딩되었는지 확인한다.
        /// </summary>
        bool IsBoundForControlJob(string controlJobId);

        /// <summary>
        /// Substrate에 Control Job 정보가 모두 바인딩되었는지 확인한다.
        /// </summary>
        bool IsBoundForControlJob(
            string controlJobId,
            JobBindingValidationMode mode);

        /// <summary>
        /// Substrate에 Process Job 정보가 모두 바인딩되었는지 확인한다.
        /// </summary>
        bool IsBoundForProcessJob(
            string processJobId,
            JobBindingValidationMode mode);

        /// <summary>
        /// ProcessJob 삭제/취소/종료 시 Substrate에 남아 있는 ProcessJobId를 해제한다.
        /// </summary>
        void UnbindByProcessJob(string processJobId);

        /// <summary>
        /// ControlJob 삭제/취소/종료 시 Substrate에 남아 있는 ControlJobId와
        /// 연결된 ProcessJobId를 해제한다.
        /// </summary>
        void UnbindByControlJob(string controlJobId);

        /// <summary>
        /// UI 표시용 바인딩 상태 조회.
        /// 상태를 변경하지 않는 읽기 전용 메서드다.
        /// </summary>
        JobBindingSnapshot GetBindingSnapshot(
            string controlJobId,
            string processJobId);

        /// <summary>
        /// 유실/파손/제거된 자재를 Binder의 active binding target에서 제외한다.
        /// ProcessJob.MaterialInfo 원본은 변경하지 않는다.
        /// </summary>
        void RemoveBindingTarget(
            string processJobId,
            string carrierId,
            int slot,
            string reason);

        /// <summary>
        /// ProcessJob 제거 시 해당 Job에 저장된 removed binding target 목록을 정리한다.
        /// </summary>
        void ClearRemovedBindingTargets(string processJobId);

        /// <summary>
        /// CarrierId 기준으로 관련 ProcessJobId 목록을 조회한다.
        /// Carrier는 있지만 Slot 목록이 없는 Job도 포함한다.
        /// </summary>
        IReadOnlyList<string> GetProcessJobIdsByCarrier(string carrierId);

        /// <summary>
        /// CarrierId 기준으로 관련 ControlJobId 목록을 조회한다.
        /// </summary>
        IReadOnlyList<string> GetControlJobIdsByCarrier(string carrierId);

        /// <summary>
        /// 현재 Port에 있는 Carrier 기준으로 관련 ProcessJobId 목록을 조회한다.
        /// </summary>
        IReadOnlyList<string> GetProcessJobIdsByCarrierPort(int portId);

        /// <summary>
        /// 현재 Port에 있는 Carrier 기준으로 관련 ControlJobId 목록을 조회한다.
        /// </summary>
        IReadOnlyList<string> GetControlJobIdsByCarrierPort(int portId);

        /// <summary>
        /// 저장소에 복구된 removed binding target 목록을 Binder 내부 캐시에 다시 로드한다.
        /// </summary>
        void ReloadRemovedBindingTargets();
    }
}