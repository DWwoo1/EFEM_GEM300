using EFEM.Defines.Job;
using EFEM.Jobs.Domain;

namespace EFEM.Jobs.Repository
{
    public sealed class ControlJobRecoveryDto
    {
        public string Id { get; set; }

        public ControlJobState State { get; set; }

        public ControlJobStartMode StartMode { get; set; }

        public string[] ProcessJobIds { get; set; }

        public string[] CurrentProcessJobIds { get; set; }

        public string DataCollectionPlan { get; set; }

        public string[] CarrierInputIds { get; set; }

        public ControlJobMaterialOutputSpec[] MaterialOutputSpecifications { get; set; }

        public ControlJobMaterialOutputByStatus[] MaterialOutputByStatus { get; set; }

        public uint[] PauseEventIds { get; set; }

        public ControlJobProcessJobStatusInfo[] ProcessJobStatus { get; set; }

        public ControlJobProcessingControlSpec[] ProcessingControlSpecifications { get; set; }

        public MaterialOrderMode ProcessOrderManagement { get; set; }

        public static ControlJobRecoveryDto FromDomain(ControlJob job)
        {
            if (job == null)
                return null;

            return new ControlJobRecoveryDto
            {
                Id = job.Id,
                State = job.State,
                StartMode = job.StartMode,
                ProcessJobIds = job.ProcessJobIds ?? new string[0],
                CurrentProcessJobIds = job.CurrentProcessJobIds ?? new string[0],
                DataCollectionPlan = job.DataCollectionPlan ?? string.Empty,
                CarrierInputIds = job.CarrierInputIds ?? new string[0],
                MaterialOutputSpecifications = job.MaterialOutputSpecifications ?? new ControlJobMaterialOutputSpec[0],
                MaterialOutputByStatus = job.MaterialOutputByStatus ?? new ControlJobMaterialOutputByStatus[0],
                PauseEventIds = job.PauseEventIds ?? new uint[0],
                ProcessJobStatus = job.ProcessJobStatus ?? new ControlJobProcessJobStatusInfo[0],
                ProcessingControlSpecifications = job.ProcessingControlSpecifications ?? new ControlJobProcessingControlSpec[0],
                ProcessOrderManagement = job.ProcessOrderManagement
            };
        }

        public ControlJob ToDomain()
        {
            return new ControlJob(
                Id,
                State,
                StartMode,
                ProcessJobIds ?? new string[0],
                CurrentProcessJobIds ?? new string[0],
                DataCollectionPlan ?? string.Empty,
                CarrierInputIds ?? new string[0],
                MaterialOutputSpecifications ?? new ControlJobMaterialOutputSpec[0],
                MaterialOutputByStatus ?? new ControlJobMaterialOutputByStatus[0],
                PauseEventIds ?? new uint[0],
                ProcessJobStatus ?? new ControlJobProcessJobStatusInfo[0],
                ProcessingControlSpecifications ?? new ControlJobProcessingControlSpec[0],
                ProcessOrderManagement);
        }
    }
}