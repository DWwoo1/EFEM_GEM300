using System.Collections.Generic;

namespace EFEM.Jobs.Repository
{
    public interface IJobRelationRepository
    {
        // 지정한 ControlJob과 ProcessJob 목록의 관계를 등록한다.
        // 현재 정책상 ProcessJob 하나는 하나의 ControlJob에만 연결될 수 있다.
        void Link(string controlJobId, IEnumerable<string> processJobIds);

        // 지정한 ProcessJob 목록이 해당 ControlJob에 연결 가능한지 확인한다.
        // 다른 ControlJob에 이미 연결된 ProcessJob이 있으면 false를 반환한다.
        // 같은 ControlJob에 이미 연결된 ProcessJob은 재설정 용도로 허용할 수 있다.
        bool CanLink(string controlJobId, IEnumerable<string> processJobIds);

        // 지정한 ControlJob에 연결된 ProcessJob ID 목록을 반환한다.
        // 반환 순서는 Link 시 전달된 ProcessJob 순서를 유지하는 것이 원칙이다.
        string[] GetProcessJobIds(string controlJobId);

        // 지정한 ProcessJob이 연결된 ControlJob ID를 반환한다.
        // 연결된 ControlJob이 없으면 null을 반환한다.
        string GetControlJobIdOrDefault(string processJobId);

        // 지정한 ControlJob에 대한 관계 정보가 저장되어 있는지 확인한다.
        bool ContainsControlJob(string controlJobId);

        // 지정한 ProcessJob이 어떤 ControlJob과 연결되어 있는지 확인한다.
        bool ContainsProcessJob(string processJobId);

        // 지정한 ControlJob에 연결된 ProcessJob이 하나 이상 있는지 확인한다.
        bool HasLinkedProcessJobs(string controlJobId);

        // 지정한 ControlJob과 연결된 모든 ProcessJob 관계를 제거한다.
        void UnlinkControlJob(string controlJobId);

        // 지정한 ProcessJob과 ControlJob 간의 관계를 제거한다.
        void UnlinkProcessJob(string processJobId);

        // 모든 ControlJob-ProcessJob 관계를 제거한다.
        void Clear();

        // 현재 정책:
        // ProcessJob 하나는 하나의 ControlJob에만 연결 가능.
        //
        // 향후 확장 후보:
        // IReadOnlyList<string> GetControlJobIds(string processJobId);
        // void LinkManyToMany(string controlJobId, IEnumerable<string> processJobIds);
    }
}