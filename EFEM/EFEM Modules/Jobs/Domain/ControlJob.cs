using System;
using System.Collections.Generic;

//using FrameOfSystem3.SECSGEM.SecsGemSDK.Gem300;

using EFEM.Defines.Job;
using EFEM.Jobs.Repository;

namespace EFEM.Jobs.Domain
{
    public sealed class ControlJob : IEntity<string>
    {
        public string Id { get; private set; }

        public ControlJobState State { get; private set; }

        public ControlJobStartMode StartMode { get; private set; }

        public string[] ProcessJobIds { get; private set; }

        // CurrentPRJob
        // 현재 ControlJob에서 진행 중이거나 선택된 ProcessJob ID 목록.
        public string[] CurrentProcessJobIds { get; private set; }

        // DataCollectionPlan
        public string DataCollectionPlan { get; private set; }

        // CarrierInputSpec
        public string[] CarrierInputIds { get; private set; }

        // MtrlOutSpec
        public ControlJobMaterialOutputSpec[] MaterialOutputSpecifications { get; private set; }

        // MtrlOutByStatus
        public ControlJobMaterialOutputByStatus[] MaterialOutputByStatus { get; private set; }

        // PauseEvent
        public uint[] PauseEventIds { get; private set; }

        // PRJobStatusList
        public ControlJobProcessJobStatusInfo[] ProcessJobStatus { get; private set; }

        // ProcessingCtrlSpec
        public ControlJobProcessingControlSpec[] ProcessingControlSpecifications { get; private set; }

        // ProcessOrderMgmt
        public MaterialOrderMode ProcessOrderManagement { get; private set; }

        public ControlJob(
            string id,
            ControlJobState state,
            ControlJobStartMode startMode,
            string[] processJobIds)
            : this(
                id,
                state,
                startMode,
                processJobIds,
                new string[0],
                string.Empty,
                new string[0],
                new ControlJobMaterialOutputSpec[0],
                new ControlJobMaterialOutputByStatus[0],
                new uint[0],
                new ControlJobProcessJobStatusInfo[0],
                new ControlJobProcessingControlSpec[0],
                0)
        {
        }

        public ControlJob(
            string id,
            ControlJobState state,
            ControlJobStartMode startMode,
            string[] processJobIds,
            string[] currentProcessJobIds,
            string dataCollectionPlan,
            string[] carrierInputIds,
            ControlJobMaterialOutputSpec[] materialOutputSpecifications,
            ControlJobMaterialOutputByStatus[] materialOutputByStatus,
            uint[] pauseEventIds,
            ControlJobProcessJobStatusInfo[] processJobStatus,
            ControlJobProcessingControlSpec[] processingControlSpecifications,
            MaterialOrderMode processOrderManagement)
        {
            if (string.IsNullOrWhiteSpace(id))
                throw new ArgumentException("ControlJobId is invalid.", nameof(id));

            Id = id;
            State = state;
            StartMode = startMode;
            ProcessJobIds = processJobIds ?? new string[0];
            CurrentProcessJobIds = currentProcessJobIds ?? new string[0];
            DataCollectionPlan = dataCollectionPlan ?? string.Empty;
            CarrierInputIds = carrierInputIds ?? new string[0];
            MaterialOutputSpecifications = materialOutputSpecifications ?? new ControlJobMaterialOutputSpec[0];
            MaterialOutputByStatus = materialOutputByStatus ?? new ControlJobMaterialOutputByStatus[0];
            PauseEventIds = pauseEventIds ?? new uint[0];
            ProcessJobStatus = processJobStatus ?? new ControlJobProcessJobStatusInfo[0];
            ProcessingControlSpecifications = processingControlSpecifications ?? new ControlJobProcessingControlSpec[0];
            ProcessOrderManagement = processOrderManagement;
        }

        public void ChangeState(ControlJobState state)
        {
            State = state;
        }

        public void ChangeStartMode(ControlJobStartMode startMode)
        {
            StartMode = startMode;
        }

        public void ChangeProcessJobIds(string[] processJobIds)
        {
            ProcessJobIds = processJobIds ?? new string[0];
        }

        /// <summary>
        /// ControlJob이 참조하는 ProcessJob들의 상태 스냅샷을 갱신한다.
        ///
        /// 주의:
        /// ProcessJobStatus는 상태의 원천이 아니다.
        /// 원천은 ProcessJob.State이고,
        /// 이 값은 ControlJob 조회/보고/화면 표시를 위한 스냅샷이다.
        /// </summary>
        public void ChangeProcessJobStatus(
            string[] currentProcessJobIds,
            ControlJobProcessJobStatusInfo[] processJobStatus)
        {
            CurrentProcessJobIds = currentProcessJobIds ?? new string[0];
            ProcessJobStatus = processJobStatus ?? new ControlJobProcessJobStatusInfo[0];
        }

        public void ChangeAttributeInfo(
            ControlJobState state,
            ControlJobStartMode startMode,
            string[] processJobIds,
            string[] currentProcessJobIds,
            string dataCollectionPlan,
            string[] carrierInputIds,
            ControlJobMaterialOutputSpec[] materialOutputSpecifications,
            ControlJobMaterialOutputByStatus[] materialOutputByStatus,
            uint[] pauseEventIds,
            ControlJobProcessJobStatusInfo[] processJobStatus,
            ControlJobProcessingControlSpec[] processingControlSpecifications,
            MaterialOrderMode processOrderManagement)
        {
            State = state;
            StartMode = startMode;
            ProcessJobIds = processJobIds ?? new string[0];
            CurrentProcessJobIds = currentProcessJobIds ?? new string[0];
            DataCollectionPlan = dataCollectionPlan ?? string.Empty;
            CarrierInputIds = carrierInputIds ?? new string[0];
            MaterialOutputSpecifications = materialOutputSpecifications ?? new ControlJobMaterialOutputSpec[0];
            MaterialOutputByStatus = materialOutputByStatus ?? new ControlJobMaterialOutputByStatus[0];
            PauseEventIds = pauseEventIds ?? new uint[0];
            ProcessJobStatus = processJobStatus ?? new ControlJobProcessJobStatusInfo[0];
            ProcessingControlSpecifications = processingControlSpecifications ?? new ControlJobProcessingControlSpec[0];
            ProcessOrderManagement = processOrderManagement;
        }

        public void ChangeAttributeInfoExceptState(
            ControlJobStartMode startMode,
            string[] processJobIds,
            string[] currentProcessJobIds,
            string dataCollectionPlan,
            string[] carrierInputIds,
            ControlJobMaterialOutputSpec[] materialOutputSpecifications,
            ControlJobMaterialOutputByStatus[] materialOutputByStatus,
            uint[] pauseEventIds,
            ControlJobProcessJobStatusInfo[] processJobStatus,
            ControlJobProcessingControlSpec[] processingControlSpecifications,
            MaterialOrderMode processOrderManagement)
        {
            StartMode = startMode;
            ProcessJobIds = processJobIds ?? new string[0];
            CurrentProcessJobIds = currentProcessJobIds ?? new string[0];
            DataCollectionPlan = dataCollectionPlan ?? string.Empty;
            CarrierInputIds = carrierInputIds ?? new string[0];
            MaterialOutputSpecifications = materialOutputSpecifications ?? new ControlJobMaterialOutputSpec[0];
            MaterialOutputByStatus = materialOutputByStatus ?? new ControlJobMaterialOutputByStatus[0];
            PauseEventIds = pauseEventIds ?? new uint[0];
            ProcessJobStatus = processJobStatus ?? new ControlJobProcessJobStatusInfo[0];
            ProcessingControlSpecifications = processingControlSpecifications ?? new ControlJobProcessingControlSpec[0];
            ProcessOrderManagement = processOrderManagement;
        }

        public override string ToString()
        {
            return string.Format(
                "ControlJobId={0}, State={1}, StartMode={2}, ProcessJobIds=[{3}], CurrentProcessJobIds=[{4}], DataCollectionPlan={5}, CarrierInputIds=[{6}], PauseEventIds=[{7}], ProcessOrderManagement={8}",
                Id,
                State,
                StartMode,
                ProcessJobIds == null ? string.Empty : string.Join(",", ProcessJobIds),
                CurrentProcessJobIds == null ? string.Empty : string.Join(",", CurrentProcessJobIds),
                DataCollectionPlan,
                CarrierInputIds == null ? string.Empty : string.Join(",", CarrierInputIds),
                PauseEventIds == null ? string.Empty : string.Join(",", PauseEventIds),
                ProcessOrderManagement);
        }
    }
}