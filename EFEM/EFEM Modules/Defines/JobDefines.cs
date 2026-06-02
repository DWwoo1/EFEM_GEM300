using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Reflection;

namespace EFEM.Defines.Job
{
    public enum ControlJobStartMode
    {
        UserStart = 0,
        AutoStart = 1
    }
    public enum ControlJobCommand
    {
        Start = 1,
        Pause = 2,
        Resume = 3,
        Cancel = 4,
        Deselect = 5,
        Stop = 6,
        Abort = 7,
        HeadOfQueue = 8
    }
    public enum ControlJobState
    {
        Queued = 0,
        Selected = 1,
        WaitingForStart = 2,
        Executing = 3,
        Paused = 4,
        Completed = 5,
        Canceled = 6,
        Deselected = 7,
        Stopped = 8,
        Aborted = 9,
        Deleted = 10,
        HeadOfQueue = 11,
        Created = 12,
        Resume = 13
    }

    public enum ProcessStartMode
    {
        UserStart = 0,
        AutoStart = 1
    }
    public enum MaterialOrderMode
    {
        Arrival = 1,
        Optimize = 2,
        List = 3
    }
    public enum RecipeMethod
    {
        RecipeOnly = 1,
        RecipeWithVariableTuning = 2
    }
    public enum ProcessJobCommand
    {
        Start = 1,
        Pause = 2,
        Resume = 3,
        Stop = 4,
        Abort = 5,
        Cancel = 6
    }
    public enum ProcessJobState
    {
        JobQueued = 0,
        SettingUp = 1,
        WaitingForStart = 2,
        Processing = 3,
        ProcessComplete = 4,
        Pausing = 6,
        Paused = 7,
        Stopping = 8,
        Aborting = 9,
        Stopped = 10,
        Aborted = 11,
        JobCanceled = 12,
        JobComplete = 17
    }
    public enum JobBindingStatus
    {
        NoTarget = 0,

        // 바인딩 대상은 있지만 아직 Carrier / Slot / Substrate / JobBinding이 준비되지 않음
        Pending = 1,

        // 현재 LoadPort Slot 기준으로 정상 바인딩됨
        Bound = 2,

        // 이미 JobBinding된 Substrate가 있으나 현재 LoadPort Slot에는 없음
        // 작업을 위해 장비 내부로 이동한 상태를 표현한다.
        Transferred = 3,

        // 다른 Job 또는 다른 ControlJob에 바인딩된 충돌 상태
        Invalid = 4
    }
    public enum JobAcknowledgeError : long
    {
        [Description("Unknown error.")]
        Unknown = 900000,

        [Description("ControlJobId is empty or invalid.")]
        InvalidControlJobId = 900001,

        [Description("ProcessJobId is empty or invalid.")]
        InvalidProcessJobId = 900002,

        [Description("ControlJob already exists.")]
        ControlJobAlreadyExists = 900003,

        [Description("ProcessJob already exists.")]
        ProcessJobAlreadyExists = 900004,

        [Description("ControlJob does not exist.")]
        ControlJobNotFound = 900005,

        [Description("ProcessJob does not exist.")]
        ProcessJobNotFound = 900006,

        [Description("ProcessJob is already linked to another ControlJob.")]
        ProcessJobAlreadyLinked = 900007,

        [Description("ControlJob has no linked ProcessJob.")]
        NoLinkedProcessJob = 900008,

        [Description("Linked ProcessJob does not exist.")]
        LinkedProcessJobNotFound = 900009,

        [Description("Command is not allowed in the current state.")]
        InvalidStateForCommand = 900010,

        [Description("RecipeId is empty or invalid.")]
        InvalidRecipeId = 900011,

        [Description("Material information is empty or invalid.")]
        InvalidMaterial = 900012,

        [Description("Slot information is empty or invalid.")]
        InvalidSlot = 900013,

        [Description("Recipe parameter is invalid.")]
        InvalidRecipeParameter = 900014,

        [Description("Material order is invalid.")]
        InvalidMaterialOrder = 900015,

        [Description("Start method is invalid.")]
        InvalidStartMethod = 900016
    }

    public static class JobAcknowledgeResult
    {
        public const long Success = 0;
        public const long Failure = 1;
    }
    public static class JobAcknowledgeErrorExtensions
    {
        public static string GetDescription(this JobAcknowledgeError error)
        {
            var type = typeof(JobAcknowledgeError);
            var name = Enum.GetName(type, error);

            if (name == null)
                return error.ToString();

            var field = type.GetField(name);

            if (field == null)
                return error.ToString();

            var attribute = field.GetCustomAttribute<DescriptionAttribute>();

            if (attribute == null)
                return error.ToString();

            return attribute.Description;
        }
    }
    /// <summary>
    /// UI 트리 표시용 중간 모델.
    /// WinForms TreeNode에 직접 의존하지 않는다.
    /// </summary>
    public sealed class JobTreeNode
    {
        public string Text { get; set; }
        public string Detail { get; set; }
        public string NodeType { get; set; }
        public string SourceId { get; set; }

        public List<JobTreeNode> Children { get; private set; }

        public JobTreeNode()
        {
            Text = string.Empty;
            Detail = string.Empty;
            NodeType = string.Empty;
            SourceId = string.Empty;
            Children = new List<JobTreeNode>();
        }
    }
}
