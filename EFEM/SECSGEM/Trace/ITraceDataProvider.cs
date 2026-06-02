using System.Collections.Generic;

namespace FrameOfSystem3.SECSGEM.Trace
{
    public interface ITraceDataProvider
    {
        TraceDefinition BuildDefinition();

        bool Initialize(ITraceRecoveryStore recoveryStore);

        void SaveRecovery(ITraceRecoveryStore recoveryStore);

        void Refresh();

        bool TryGetSnapshot(out Dictionary<long, string> snapshot);

        IReadOnlyCollection<long> GetConfiguredVariableIds();
    }
}