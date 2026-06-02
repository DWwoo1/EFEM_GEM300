using System.Collections.Generic;

namespace EFEM.Jobs.Binding
{
    /// <summary>
    /// Port + Carrier 기준으로 관련 ProcessJob을 빠르게 찾기 위한 조회 인덱스.
    /// 저장소가 아니라 재구성 가능한 read model이다.
    /// </summary>
    public interface IJobBindingTargetIndex
    {
        void Clear();

        void AddOrUpdateProcessJob(
            string processJobId,
            IReadOnlyList<JobBindingTarget> targets);

        // Carrier는 있지만 Slot 목록이 없는 Job도 조회할 수 있도록 별도 등록한다.
        void AddOrUpdateProcessJobCarrierReferences(
            string processJobId,
            IReadOnlyList<string> carrierIds);

        void RemoveProcessJob(string processJobId);

        void UpdateCarrierPort(
            int sourcePortId,
            string carrierId);

        /// <summary>
        /// CarrierId만으로 관련 ProcessJobId 목록을 조회한다.
        /// PortId를 아직 모르는 시점에서 후보 Job을 찾을 때 사용한다.
        /// </summary>
        IReadOnlyList<string> GetProcessJobIdsByCarrier(
            string carrierId);

        IReadOnlyList<string> GetProcessJobIdsByPortCarrier(
            int sourcePortId,
            string carrierId);

        IReadOnlyList<JobBindingTarget> GetTargetsByProcessJobId(
            string processJobId);
    }
}