using System.Collections.Generic;
using TickCounter_;

namespace FrameOfSystem3.SECSGEM.Trace
{
    public sealed class TraceRuntimeManager
    {
        private readonly ITraceDataProvider _provider;
        private readonly ITraceRecoveryStore _recoveryStore;

        private bool _isEnabled;
        private uint _intervalMs;
        private readonly TickCounter _tick = new TickCounter();
        private readonly Dictionary<long, string> _previous = new Dictionary<long, string>();

        public TraceRuntimeManager(
            ITraceDataProvider provider,
            ITraceRecoveryStore recoveryStore)
        {
            _provider = provider;
            _recoveryStore = recoveryStore;
        }

        public bool Initialize()
        {
            if (_provider == null)
                return false;

            TraceDefinition definition = _provider.BuildDefinition();
            if (definition == null)
                return false;

            _intervalMs = definition.IntervalMs;

            if (false == definition.IsEnabled)
                return false;

            if (false == _provider.Initialize(_recoveryStore))
                return false;

            IReadOnlyCollection<long> configuredVariableIds = _provider.GetConfiguredVariableIds();
            _isEnabled = configuredVariableIds != null && configuredVariableIds.Count > 0;

            if (false == _isEnabled)
                return false;

            _tick.SetTickCount(_intervalMs);
            return true;
        }

        public void SaveRecovery()
        {
            if (false == _isEnabled)
                return;

            _provider.SaveRecovery(_recoveryStore);
        }

        public bool TryGetDelta(out Dictionary<long, string> changedValues)
        {
            changedValues = new Dictionary<long, string>();

            if (false == _isEnabled)
                return false;

            if (false == _tick.IsTickOver(true))
                return false;

            _provider.Refresh();

            Dictionary<long, string> snapshot;
            if (false == _provider.TryGetSnapshot(out snapshot))
                return false;

            _tick.SetTickCount(_intervalMs);

            foreach (KeyValuePair<long, string> item in snapshot)
            {
                string current = item.Value ?? string.Empty;

                if (false == _previous.TryGetValue(item.Key, out string previous)
                    || false == string.Equals(previous, current))
                {
                    changedValues[item.Key] = current;
                    _previous[item.Key] = current;
                }
            }

            return changedValues.Count > 0;
        }

        public bool TryGetCurrentSnapshot(out Dictionary<long, string> snapshot)
        {
            snapshot = new Dictionary<long, string>();

            if (_provider == null)
                return false;

            return _provider.TryGetSnapshot(out snapshot);
        }
    }
}