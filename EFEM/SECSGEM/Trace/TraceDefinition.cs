using System.Collections.Generic;

namespace FrameOfSystem3.SECSGEM.Trace
{
    public sealed class TraceDefinition
    {
        public bool IsEnabled { get; set; }
        public uint IntervalMs { get; set; }
    }
}