using System.Collections.Generic;

namespace FrameOfSystem3.SECSGEM.Trace
{
    public interface ITraceRecoveryStore
    {
        bool TryReadTraceInfo(
            out IDictionary<string, long> info,
            out IDictionary<string, string> processOnly,
            out IDictionary<string, string> initialTraceValues);

        void WriteTraceInfo(
            IReadOnlyDictionary<string, long> info,
            IReadOnlyDictionary<string, string> processOnly);

        bool TryReadLastValues(ref Dictionary<long, string> values);

        void WriteLastValues(Dictionary<long, string> values);
    }
}