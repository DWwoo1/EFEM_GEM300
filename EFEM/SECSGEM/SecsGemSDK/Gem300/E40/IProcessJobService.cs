using System;
using System.Collections.Generic;
using System.Linq;

using EFEM.Defines.Common;

using FrameOfSystem3.SECSGEM.DefineSecsGem;

namespace EFEM.Defines.Job
{
    public interface IProcessJobService
    {
        bool IsDriverAttached { get; }
        void AttachDriver(IProcessJobDriver driver);
        void DetachDriver();
        void RegisterCallback(IProcessJobServiceCallback callback);
        void UnregisterCallback(IProcessJobServiceCallback callback);

        //void SetHostRequestHandler(IProcessJobHostRequestHandler handler);
        //void ClearHostRequestHandler(IProcessJobHostRequestHandler handler);

        long Create(string processJobId, MaterialFormat materialFormat, ProcessStartMode startMode, MaterialOrderMode materialOrder, IReadOnlyDictionary<string, IReadOnlyList<int>> materialInfo, RecipeMethod recipeMethod, string recipeId, string[] recipeParameterNames, string[] recipeParameterValues);
        long CreateWithNumericRecipe(string processJobId, MaterialFormat materialFormat, ProcessStartMode startMode, MaterialOrderMode materialOrder, IReadOnlyDictionary<string, IReadOnlyList<int>> materialInfo, RecipeMethod recipeMethod, string recipeId, string[] recipeParameterNames, long[] recipeParameterValues);
        long RequestJob(string processJobId);
        long RequestAllJobIds();
        long RequestCommand(string processJobId, ProcessJobCommand command);
        long AcknowledgeVerify(long messageId, string[] processJobIds, long result, long[] errorCodes, string[] errorTexts);
        long AcknowledgeCommand(long messageId, ProcessJobCommand command, string processJobId, long result, long[] errorCodes, string[] errorTexts);
        long AcknowledgeRecipeVariables(long messageId, string processJobId, long result, long[] errorCodes, string[] errorTexts);
        long AcknowledgeStartMethod(long messageId, string[] processJobIds, long result, long[] errorCodes, string[] errorTexts);
        long AcknowledgeMaterialOrder(long messageId, long result);
        long SetJobInfo(string processJobId, MaterialFormat materialFormat, ProcessStartMode startMode, MaterialOrderMode materialOrder, IReadOnlyDictionary<string, IReadOnlyList<int>> materialInfo, RecipeMethod recipeMethod, string recipeId, string[] recipeParameterNames, string[] recipeParameterValues);
        long SetJobInfoWithNumericRecipe(string processJobId, MaterialFormat materialFormat, ProcessStartMode startMode, MaterialOrderMode materialOrder, IReadOnlyDictionary<string, IReadOnlyList<int>> materialInfo, RecipeMethod recipeMethod, string recipeId, string[] recipeParameterNames, long[] recipeParameterValues);
        long SetState(string processJobId, ProcessJobState state);
        long NotifySettingUpStarted(string processJobId);
        long NotifySettingUpCompleted(string processJobId);
        long Remove(string processJobId);
        long RemoveAll();
    }

    public sealed class ProcessRecipeParameter
    {
        public ProcessRecipeParameter(string name, string value) 
        {
            Name = name;
            Value = value; 
        }
        public string Name { get; }
        public string Value { get; }
    }

    public sealed class NumericProcessRecipeParameter
    {
        public NumericProcessRecipeParameter(string name, long value) 
        {
            Name = name;
            Value = value; 
        }
        public string Name { get; }
        public long Value { get; }
    }

    public sealed class ProcessJobInfo
    {
        public string ProcessJobId { get; private set; }
        public MaterialFormat MaterialFormat { get; private set; }
        public ProcessJobState State { get; private set; }
        public ProcessStartMode StartMode { get; private set; }
        public MaterialOrderMode MaterialOrder { get; private set; }
        public IReadOnlyDictionary<string, IReadOnlyList<int>> MaterialInfo { get; private set; }
        public RecipeMethod RecipeMethod { get; private set; }
        public string RecipeId { get; private set; }
        public ProcessRecipeParameter[] RecipeParameters { get; private set; }
        public uint[] PauseEventIds { get; private set; }
        
        public ProcessJobInfo(
            string processJobId,
            ProcessJobState state,
            MaterialFormat materialFormat,
            ProcessStartMode startMode,
            MaterialOrderMode materialOrder,
            IReadOnlyDictionary<string, IReadOnlyList<int>> materialInfo,
            RecipeMethod recipeMethod,
            string recipeId,
            ProcessRecipeParameter[] recipeParameters,
            uint[] pauseEventIds)
        {
            ProcessJobId = processJobId;
            State = state;
            MaterialFormat = materialFormat;
            StartMode = startMode;
            MaterialOrder = materialOrder;
            MaterialInfo = materialInfo;
            RecipeMethod = recipeMethod;
            RecipeId = recipeId;
            RecipeParameters = recipeParameters ?? new ProcessRecipeParameter[0];
            PauseEventIds = pauseEventIds ?? new uint[0];
        }
    }

    public sealed class ProcessJobCreatedEventArgs : EventArgs
    {
        public ProcessJobCreatedEventArgs(ProcessJobInfo job)
        {
            Job = job; 
        }
        public ProcessJobInfo Job { get; }
    }

    public sealed class ProcessJobStateChangedEventArgs : EventArgs
    {
        public ProcessJobStateChangedEventArgs(string processJobId, ProcessJobState state) 
        {
            ProcessJobId = processJobId; 
            State = state; 
        }
        public string ProcessJobId { get; }
        public ProcessJobState State { get; }
    }

    public sealed class ProcessJobDeletedEventArgs : EventArgs
    {
        public ProcessJobDeletedEventArgs(string processJobId)
        {
            ProcessJobId = processJobId; 
        }
        public string ProcessJobId { get; }
    }

    public sealed class ProcessJobVerifyRequestedEventArgs : EventArgs
    {
        public ProcessJobVerifyRequestedEventArgs(long messageId, IList<ProcessJobInfo> jobs) 
        {
            MessageId = messageId; 
            Jobs = jobs; 
        }
        public long MessageId { get; }
        public IList<ProcessJobInfo> Jobs { get; }
    }

    public sealed class ProcessJobCommandRequestedEventArgs : EventArgs
    {
        public ProcessJobCommandRequestedEventArgs(
            long messageId,
            string processJobId, 
            ProcessJobCommand command) 
        {
            MessageId = messageId; 
            ProcessJobId = processJobId; 
            Command = command; 
        }
        public long MessageId { get; }
        public string ProcessJobId { get; }
        public ProcessJobCommand Command { get; }
    }

    public sealed class ProcessJobRecipeVariableRequestedEventArgs : EventArgs
    {
        public ProcessJobRecipeVariableRequestedEventArgs(
            long messageId, 
            string processJobId, 
            ProcessRecipeParameter[] recipeParameters)
        {
            MessageId = messageId; 
            ProcessJobId = processJobId; 
            RecipeParameters = recipeParameters ?? Array.Empty<ProcessRecipeParameter>(); 
        }
        public long MessageId { get; }
        public string ProcessJobId { get; }
        public ProcessRecipeParameter[] RecipeParameters { get; }
    }

    public sealed class ProcessJobStartMethodRequestedEventArgs : EventArgs
    {
        public ProcessJobStartMethodRequestedEventArgs(
            long messageId, 
            string[] processJobIds, 
            long processStart) 
        {
            MessageId = messageId; 
            ProcessJobIds = processJobIds ?? Array.Empty<string>(); 
            ProcessStart = processStart; 
        }
        public long MessageId { get; }
        public string[] ProcessJobIds { get; }
        public long ProcessStart { get; }
    }

    public sealed class ProcessJobMaterialOrderRequestedEventArgs : EventArgs
    {
        public ProcessJobMaterialOrderRequestedEventArgs(long messageId, MaterialOrderMode materialOrder)
        {
            MessageId = messageId; 
            MaterialOrder = materialOrder; 
        }
        public long MessageId { get; }
        public MaterialOrderMode MaterialOrder { get; }
    }

    public sealed class ProcessJobManualStartEventArgs : EventArgs
    {
        public ProcessJobManualStartEventArgs(string processJobId) 
        {
            ProcessJobId = processJobId; 
        }
        public string ProcessJobId { get; }
    }

    public sealed class ProcessJobSettingUpEventArgs : EventArgs
    {
        public ProcessJobSettingUpEventArgs(string processJobId) 
        {
            ProcessJobId = processJobId;
        }
        public string ProcessJobId { get; }
    }
}
