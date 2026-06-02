using System;

using FrameOfSystem3.SECSGEM.DefineSecsGem;

namespace EFEM.Defines.Job
{
    public interface IControlJobService
    {
        bool IsDriverAttached { get; }
        void AttachDriver(IControlJobDriver driver);
        void DetachDriver();
        void RegisterCallback(IControlJobServiceCallback callback);
        void UnregisterCallback(IControlJobServiceCallback callback);
        long Create(string controlJobId, ControlJobStartMode startMode, string[] processJobIds);
        long RequestJob(string controlJobId);
        long RequestAllJobIds();
        long RequestSelect(string controlJobId);
        long RequestHeadOfQueue(string controlJobId);
        long RequestHeadOfQueueInfo();
        long RequestCommand(string controlJobId, ControlJobCommand command, string commandParameterName, string commandParameterValue);
        long AcknowledgeVerify(long messageId, string controlJobId, long result, long[] errorCodes, string[] errorTexts);
        long AcknowledgeCommand(long messageId, string controlJobId, ControlJobCommand command, long result, long[] errorCodes, string[] errorTexts);
        long SetJobInfo(string controlJobId, ControlJobState state, ControlJobStartMode startMode, string[] processJobIds);
        long Remove(string controlJobId);
        long RemoveAll();
    }
    public sealed class ControlJobProcessJobStatusInfo
    {
        public string ProcessJobId { get; private set; }

        public ProcessJobState State { get; private set; }

        public ControlJobProcessJobStatusInfo(
            string processJobId,
            ProcessJobState state)
        {
            ProcessJobId = processJobId ?? string.Empty;
            State = state;
        }
    }
    public sealed class ControlJobMaterialOutputSpec
    {
        public string AttributeId { get; private set; }

        public byte[] SourceSlotNumbers { get; private set; }

        public string Value { get; private set; }

        public byte[] DestinationSlotNumbers { get; private set; }

        public ControlJobMaterialOutputSpec(
            string attributeId,
            byte[] sourceSlotNumbers,
            string value,
            byte[] destinationSlotNumbers)
        {
            AttributeId = attributeId ?? string.Empty;
            SourceSlotNumbers = sourceSlotNumbers ?? new byte[0];
            Value = value ?? string.Empty;
            DestinationSlotNumbers = destinationSlotNumbers ?? new byte[0];
        }
    }
    public sealed class ControlJobMaterialOutputByStatus
    {
        public byte MaterialStatus { get; private set; }

        public string Value { get; private set; }

        public byte[] SlotNumbers { get; private set; }

        public ControlJobMaterialOutputByStatus(
            byte materialStatus,
            string value,
            byte[] slotNumbers)
        {
            MaterialStatus = materialStatus;
            Value = value ?? string.Empty;
            SlotNumbers = slotNumbers ?? new byte[0];
        }
    }
    public sealed class ControlJobProcessingControlSpec
    {
        public string ProcessJobId { get; private set; }

        public string[] RuleNames { get; private set; }

        public string[] RuleValues { get; private set; }

        public byte[] OutputRuleStatus { get; private set; }

        public string[] OutputRuleValues { get; private set; }

        public ControlJobProcessingControlSpec(
            string processJobId,
            string[] ruleNames,
            string[] ruleValues,
            byte[] outputRuleStatus,
            string[] outputRuleValues)
        {
            ProcessJobId = processJobId ?? string.Empty;
            RuleNames = ruleNames ?? new string[0];
            RuleValues = ruleValues ?? new string[0];
            OutputRuleStatus = outputRuleStatus ?? new byte[0];
            OutputRuleValues = outputRuleValues ?? new string[0];
        }
    }
    public sealed class ControlJobInfo
    {
        public string ControlJobId { get; private set; }

        public ControlJobState State { get; private set; }

        public ControlJobStartMode StartMode { get; private set; }

        public string[] ProcessJobIds { get; private set; }

        public string[] CurrentProcessJobIds { get; private set; }

        public string DataCollectionPlan { get; private set; }

        public string[] CarrierInputIds { get; private set; }

        public ControlJobMaterialOutputSpec[] MaterialOutputSpecifications { get; private set; }

        public ControlJobMaterialOutputByStatus[] MaterialOutputByStatus { get; private set; }

        public uint[] PauseEventIds { get; private set; }

        public ControlJobProcessJobStatusInfo[] ProcessJobStates { get; private set; }

        public ControlJobProcessingControlSpec[] ProcessingControlSpecifications { get; private set; }

        public MaterialOrderMode ProcessOrderManagement { get; private set; }

        public ControlJobInfo(
            string controlJobId,
            ControlJobStartMode startMode,
            string[] processJobIds)
            : this(
                controlJobId,
                ControlJobState.Queued,
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

        public ControlJobInfo(
            string controlJobId,
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
            ControlJobId = controlJobId;
            State = state;
            StartMode = startMode;
            ProcessJobIds = processJobIds ?? new string[0];
            CurrentProcessJobIds = currentProcessJobIds ?? new string[0];
            DataCollectionPlan = dataCollectionPlan ?? string.Empty;
            CarrierInputIds = carrierInputIds ?? new string[0];
            MaterialOutputSpecifications = materialOutputSpecifications ?? new ControlJobMaterialOutputSpec[0];
            MaterialOutputByStatus = materialOutputByStatus ?? new ControlJobMaterialOutputByStatus[0];
            PauseEventIds = pauseEventIds ?? new uint[0];
            ProcessJobStates = processJobStatus ?? new ControlJobProcessJobStatusInfo[0];
            ProcessingControlSpecifications = processingControlSpecifications ?? new ControlJobProcessingControlSpec[0];
            ProcessOrderManagement = processOrderManagement;
        }
    }
    public sealed class ControlJobCreatedEventArgs : EventArgs
    {
        public ControlJobCreatedEventArgs(ControlJobInfo job) 
        {
            Job = job; 
        }
        public ControlJobInfo Job { get; }
    }
    public sealed class ControlJobStateChangedEventArgs : EventArgs
    {
        public ControlJobStateChangedEventArgs(string controlJobId, ControlJobState state) 
        {
            ControlJobId = controlJobId;
            State = state; 
        }
        public string ControlJobId { get; }
        public ControlJobState State { get; }
    }
    public sealed class ControlJobDeletedEventArgs : EventArgs
    {
        public ControlJobDeletedEventArgs(string controlJobId) 
        {
            ControlJobId = controlJobId; 
        }
        public string ControlJobId { get; }
    }
    public sealed class ControlJobVerifyRequestedEventArgs : EventArgs
    {
        public ControlJobVerifyRequestedEventArgs(
            long messageId,
            string controlJobId, 
            string[] carrierIds,
            string[] processJobIds,
            MaterialOrderMode processOrderManagement, 
            ControlJobStartMode startMode)
        {
            MessageId = messageId;
            ControlJobId = controlJobId;
            CarrierIds = carrierIds ?? Array.Empty<string>();
            ProcessJobIds = processJobIds ?? Array.Empty<string>();
            ProcessOrderManagement = processOrderManagement;
            StartMode = startMode;
        }
        public long MessageId { get; }
        public string ControlJobId { get; }
        public string[] CarrierIds { get; }
        public string[] ProcessJobIds { get; }
        public MaterialOrderMode ProcessOrderManagement { get; }
        public ControlJobStartMode StartMode { get; }
    }
    public sealed class ControlJobCommandRequestedEventArgs : EventArgs
    {
        public ControlJobCommandRequestedEventArgs(
            long messageId, 
            string controlJobId, 
            ControlJobCommand command) 
        {
            MessageId = messageId; 
            ControlJobId = controlJobId; 
            Command = command; 
        }
        public long MessageId { get; }
        public string ControlJobId { get; }
        public ControlJobCommand Command { get; }
    }
    public sealed class ControlJobManualStartEventArgs : EventArgs
    {
        public ControlJobManualStartEventArgs(string controlJobId) 
        {
            ControlJobId = controlJobId; 
        }
        public string ControlJobId { get; }
    }
    public sealed class ControlJobHoqChangedEventArgs : EventArgs
    {
        public ControlJobHoqChangedEventArgs(string controlJobId) 
        {
            ControlJobId = controlJobId; 
        }
        public string ControlJobId { get; }
    }
}
