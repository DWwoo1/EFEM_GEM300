using System.Collections.Generic;

using EFEM.Defines.Job;
using EFEM.Defines.Common;
using EFEM.Jobs.Domain;

namespace EFEM.Jobs.Manager
{
    #region <Result Evaluator>

    public interface ISecsGemResultEvaluator
    {
        // SECS/GEM SDK 호출 결과가 성공인지 판단한다.
        bool IsSuccess(long result);
    }

    public class SecsGemResultEvaluator : ISecsGemResultEvaluator
    {
        public bool IsSuccess(long result)
        {
            return result == JobAcknowledgeResult.Success;
        }
    }

    #endregion </Result Evaluator>


    #region <Enums>

    public enum ControlJobRemoveMode
    {
        // 연결된 ProcessJob이 남아 있으면 ControlJob 삭제를 거부한다.
        RejectIfProcessJobsExist = 0,

        // 연결된 ProcessJob을 먼저 제거한 뒤 ControlJob을 제거한다.
        RemoveLinkedProcessJobs = 1
    }

    #endregion </Enums>


    #region <Interfaces>

    public interface IJobManager
    {
        #region <Create>

        // ProcessJob을 생성하고 내부 저장소에 등록한다.
        // SDK 생성 요청이 실패하면 내부 저장소 등록도 되돌린다.
        long CreateProcessJob(
            string processJobId,
            MaterialFormat materialFormat,
            ProcessStartMode startMode,
            MaterialOrderMode materialOrder,
            IReadOnlyDictionary<string, IReadOnlyList<int>> materialInfo,
            RecipeMethod recipeMethod,
            string recipeId,
            string[] recipeParameterNames,
            string[] recipeParameterValues);

        // 숫자형 Recipe Parameter를 사용하는 ProcessJob을 생성한다.
        // 내부 도메인에는 문자열 값으로 변환하여 저장한다.
        long CreateProcessJobWithNumericRecipe(
            string processJobId,
            MaterialFormat materialFormat,
            ProcessStartMode startMode,
            MaterialOrderMode materialOrder,
            IReadOnlyDictionary<string, IReadOnlyList<int>> materialInfo,
            RecipeMethod recipeMethod,
            string recipeId,
            string[] recipeParameterNames,
            long[] recipeParameterValues);

        // ControlJob을 생성하고 지정된 ProcessJob들과 관계를 등록한다.
        // 연결 대상 ProcessJob은 미리 존재해야 하며, 다른 ControlJob에 연결되어 있으면 실패한다.
        long CreateControlJob(
            string controlJobId,
            ControlJobStartMode startMode,
            string[] processJobIds);

        #endregion </Create>

        #region <Recovery>
        // 리커버리 전용. 재시작 후 잡을 자재와 연결한다.
        void RebindRecoveredJobs();
        #endregion </Recovery>

        #region <Request>

        // SDK/Host 측에 특정 ControlJob 정보 요청을 보낸다.
        // 실제 정보 반영은 이후 callback을 통해 내부 저장소에 동기화된다.
        long RequestControlJob(string controlJobId);

        // SDK/Host 측에 전체 ControlJob ID 목록 요청을 보낸다.
        long RequestAllControlJobIds();

        // SDK/Host 측에 특정 ProcessJob 정보 요청을 보낸다.
        // 실제 정보 반영은 이후 callback을 통해 내부 저장소에 동기화된다.
        long RequestProcessJob(string processJobId);

        // SDK/Host 측에 전체 ProcessJob ID 목록 요청을 보낸다.
        long RequestAllProcessJobIds();

        // 지정한 ControlJob을 SELECT 대상으로 요청한다.
        long RequestControlJobSelect(string controlJobId);

        // 지정한 ControlJob을 QUEUED 대기열의 선두로 이동하도록 요청한다.
        // 이미 대기열 선두이면 SDK 요청 없이 성공으로 처리한다.
        long RequestControlJobHeadOfQueue(string controlJobId);

        // SDK/Host 측에 현재 Head Of Queue 정보를 요청한다.
        long RequestControlJobHeadOfQueueInfo();

        #endregion </Request>


        #region <Command>

        // 지정한 ControlJob에 ControlJob command를 요청한다.
        // 현재는 command 전송 전 ControlJob 존재 여부를 확인한다.
        long RequestControlJobCommand(
            string controlJobId,
            ControlJobCommand command,
            string commandParameterName,
            string commandParameterValue);

        // 지정한 ProcessJob에 ProcessJob command를 요청한다.
        // 현재는 command 전송 전 ProcessJob 존재 여부를 확인한다.
        long RequestProcessJobCommand(
            string processJobId,
            ProcessJobCommand command);
        #endregion </Command>


        #region <Set>
        // ControlJob 정보를 SDK에 설정하고, 성공 시 내부 저장소와 CJ-PJ 관계를 갱신한다.
        long SetControlJobInfo(
            string controlJobId,
            ControlJobState state,
            ControlJobStartMode startMode,
            string[] processJobIds);

        // ProcessJob 정보를 SDK에 설정하고, 성공 시 내부 저장소를 갱신한다.
        long SetProcessJobInfo(
            string processJobId,
            MaterialFormat materialFormat,
            ProcessStartMode startMode,
            MaterialOrderMode materialOrder,
            IReadOnlyDictionary<string, IReadOnlyList<int>> materialInfo,
            RecipeMethod recipeMethod,
            string recipeId,
            string[] recipeParameterNames,
            string[] recipeParameterValues);

        // 숫자형 Recipe Parameter를 사용하는 ProcessJob 정보를 SDK에 설정한다.
        // 성공 시 내부 도메인에는 문자열 값으로 변환하여 저장한다.
        long SetProcessJobInfoWithNumericRecipe(
            string processJobId,
            MaterialFormat materialFormat,
            ProcessStartMode startMode,
            MaterialOrderMode materialOrder,
            IReadOnlyDictionary<string, IReadOnlyList<int>> materialInfo,
            RecipeMethod recipeMethod,
            string recipeId,
            string[] recipeParameterNames,
            long[] recipeParameterValues);

        // ProcessJob 상태를 SDK에 설정하고, 성공 시 내부 저장소 상태를 갱신한다.
        // 상태가 terminal이면 연결된 ControlJob 자동 완료/삭제 여부를 확인한다.
        long SetProcessJobState(
            string processJobId,
            ProcessJobState state);
        #endregion </Set>


        #region <Notify>

        // ProcessJob SettingUp 시작을 SDK/Host 측에 통지하고, 성공 시 내부 상태를 SettingUp으로 갱신한다.
        long NotifyProcessJobSettingUpStarted(string processJobId);

        // ProcessJob SettingUp 완료를 SDK/Host 측에 통지하고, 성공 시 내부 상태를 WaitingForStart로 갱신한다.
        long NotifyProcessJobSettingUpCompleted(string processJobId);

        #endregion </Notify>


        #region <Remove>

        // ControlJob을 제거한다.
        // removeMode에 따라 연결 ProcessJob이 남아 있으면 거부하거나, 연결 ProcessJob 제거 후 ControlJob을 제거한다.
        long RemoveControlJob(
            string controlJobId,
            ControlJobRemoveMode removeMode);

        // ProcessJob을 제거한다.
        // 연결된 ControlJob이 있으면 제거 후 남은 ProcessJob 상태에 따라 ControlJob도 자동 제거될 수 있다.
        long RemoveProcessJob(string processJobId);

        // 모든 ControlJob을 제거한다.
        // removeMode에 따라 연결 ProcessJob 존재 시 거부하거나, 각 ControlJob의 연결 ProcessJob을 함께 제거한다.
        long RemoveAllControlJobs(ControlJobRemoveMode removeMode);

        // 모든 ProcessJob을 제거한다.
        // ProcessJob이 모두 제거되면 ControlJob은 유효한 연결을 유지할 수 없으므로 ControlJob도 함께 제거한다.
        long RemoveAllProcessJobs();

        #endregion </Remove>


        #region <Query>

        // 내부 저장소에서 ControlJob을 조회한다.
        // 없으면 null을 반환한다.
        ControlJob GetControlJobOrDefault(string controlJobId);

        // 내부 저장소에서 ProcessJob을 조회한다.
        // 없으면 null을 반환한다.
        ProcessJob GetProcessJobOrDefault(string processJobId);

        // 지정한 ControlJob에 연결된 ProcessJob ID 목록을 반환한다.
        // 반환 순서는 관계 저장소에 등록된 순서를 따른다.
        string[] GetProcessJobIds(string controlJobId);

        // 지정한 ProcessJob이 연결된 ControlJob ID를 반환한다.
        // 연결된 ControlJob이 없으면 null을 반환한다.
        string GetControlJobIdOrDefault(string processJobId);

        // 내부 저장소의 전체 ControlJob 목록을 반환한다.
        // 반환 순서는 ControlJob 저장소의 순서를 따른다.
        IReadOnlyList<ControlJob> GetAllControlJobs();

        // 내부 저장소의 전체 ProcessJob 목록을 반환한다.
        // 반환 순서는 ProcessJob 저장소의 순서를 따른다.
        IReadOnlyList<ProcessJob> GetAllProcessJobs();

        // 지정한 ControlJob에 연결된 ProcessJob 객체 목록을 반환한다.
        // 삭제되었거나 저장소에 없는 ProcessJob은 결과에서 제외된다.
        IReadOnlyList<ProcessJob> GetLinkedProcessJobs(string controlJobId);

        // 지정한 ControlJob이 현재 QUEUED 대기열의 선두인지 확인한다.
        // SELECTED, EXECUTING, PAUSED 등 대기열 밖 상태는 HOQ로 보지 않는다.
        bool IsHeadOfQueueControlJob(string controlJobId);
        
        // 상태전이 모델상 ACTIVE 상태인 ControlJob을 우선순위 기준으로 반환한다.
        // 우선순위는 Executing, Paused, WaitingForStart, Selected 순서이다.
        // ACTIVE ControlJob이 없으면 null을 반환한다.
        ControlJob GetActiveControlJobOrDefault();

        // 상태전이 모델상 ACTIVE 상태인 ProcessJob을 우선순위 기준으로 반환한다.
        // Active ControlJob에 연결된 ProcessJob을 우선 탐색하고,
        // 없으면 전체 ProcessJob 저장소에서 탐색한다.
        // ACTIVE ProcessJob이 없으면 null을 반환한다.
        ProcessJob GetActiveProcessJobOrDefault();

        // 현재 QUEUED 대기열의 선두 ControlJob을 반환한다.
        // 대기 중인 ControlJob이 없으면 null을 반환한다.
        ControlJob GetHeadOfQueueControlJobOrDefault();

        // 현재 장비가 작업 대상으로 봐야 하는 ControlJob을 반환한다.
        // ACTIVE 상태의 ControlJob이 있으면 상태 우선순위 기준으로 반환하고,
        // 없으면 QUEUED 대기열의 선두 ControlJob을 반환한다.
        ControlJob GetWorkingControlJobOrDefault();

        // 이미 Active PJ가 있으면 그걸 작업 대상으로 본다.
        // 없으면 Working CJ를 찾는다.
        // Working CJ에 연결된 PJ 중 JobQueued인 첫 번째 PJ를 반환한다.
        ProcessJob GetWorkingProcessJobOrDefault();

        // InSpec에서 입력받은 Carrier ID가 포함된 첫 번째 ControlJob을 반환한다.
        // 없으면 null을 반환한다.
        ControlJob GetControlJobByCarrierInputIdOrDefault(string carrierId);

        // InSpec에서 입력받은 Carrier ID가 포함된 모든 ControlJob을 반환한다.
        IReadOnlyList<ControlJob> GetControlJobsByCarrierInputId(string carrierId);

        // OutSpec에서 입력받은 Carrier ID가 포함된 첫 번째 ControlJob을 반환한다.
        // 없으면 null을 반환한다.
        ControlJob GetControlJobByCarrierOutputSpecValueOrDefault(string carrierId);

        // OutSpec에서 입력받은 Carrier ID가 포함된 모든 ControlJob을 반환한다.
        IReadOnlyList<ControlJob> GetControlJobsByCarrierOutputSpecValue(string carrierId);

        // 지정한 ControlJob에 연결된 ProcessJob 중 ACTIVE 상태인 ProcessJob을 우선순위 기준으로 반환한다.
        // ACTIVE ProcessJob이 여러 개면 Processing, Pausing, Paused, Stopping, Aborting, WaitingForStart, SettingUp 순서로 반환한다.
        // 없으면 null을 반환한다.
        ProcessJob GetActiveProcessJobOrDefault(string controlJobId);

        // 지정한 ControlJob에 연결된 ProcessJob 중 Processing 상태인 ProcessJob을 반환한다.
        // 없으면 null을 반환한다.
        ProcessJob GetProcessingProcessJobOrDefault(string controlJobId);
        #endregion </Query>
    }

    #endregion </Interfaces>
}