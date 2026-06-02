using System;
using System.Collections.Generic;

namespace EFEM.Jobs.Repository
{
    public sealed class InMemoryJobRelationRepository : IJobRelationRepository
    {
        private readonly object _lock = new object();

        private readonly Dictionary<string, List<string>> _controlToProcesses =
            new Dictionary<string, List<string>>();

        private readonly Dictionary<string, string> _processToControl =
            new Dictionary<string, string>();

        public void Link(string controlJobId, IEnumerable<string> processJobIds)
        {
            if (string.IsNullOrWhiteSpace(controlJobId))
                throw new ArgumentException("ControlJobId is invalid.", nameof(controlJobId));

            if (processJobIds == null)
                throw new ArgumentNullException(nameof(processJobIds));

            List<string> copiedProcessJobIds;

            lock (_lock)
            {
                copiedProcessJobIds = new List<string>();

                foreach (var processJobId in processJobIds)
                {
                    if (string.IsNullOrWhiteSpace(processJobId))
                        continue;

                    if (!copiedProcessJobIds.Contains(processJobId))
                        copiedProcessJobIds.Add(processJobId);
                }

                foreach (var processJobId in copiedProcessJobIds)
                {
                    string existingControlJobId;

                    if (_processToControl.TryGetValue(processJobId, out existingControlJobId))
                    {
                        if (!string.Equals(existingControlJobId, controlJobId, StringComparison.Ordinal))
                        {
                            throw new InvalidOperationException(
                                "ProcessJob is already linked to another ControlJob. ProcessJobId="
                                + processJobId
                                + ", ExistingControlJobId="
                                + existingControlJobId
                                + ", NewControlJobId="
                                + controlJobId);
                        }
                    }
                }

                List<string> oldProcessJobIds;

                if (_controlToProcesses.TryGetValue(controlJobId, out oldProcessJobIds))
                {
                    foreach (var oldProcessJobId in oldProcessJobIds)
                        _processToControl.Remove(oldProcessJobId);
                }

                _controlToProcesses[controlJobId] = copiedProcessJobIds;

                foreach (var processJobId in copiedProcessJobIds)
                    _processToControl[processJobId] = controlJobId;
            }

            Console.WriteLine(
                "[{0}] InMemoryJobRelationRepository Link ControlJobId={1}, ProcessJobIds=[{2}]",
                DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff"),
                controlJobId,
                string.Join(",", copiedProcessJobIds));
        }

        public bool CanLink(string controlJobId, IEnumerable<string> processJobIds)
        {
            if (string.IsNullOrWhiteSpace(controlJobId))
                return false;

            if (processJobIds == null)
                return false;

            bool result = true;
            List<string> copiedProcessJobIds = new List<string>();

            lock (_lock)
            {
                foreach (var processJobId in processJobIds)
                {
                    if (string.IsNullOrWhiteSpace(processJobId))
                        continue;

                    copiedProcessJobIds.Add(processJobId);

                    string existingControlJobId;

                    if (_processToControl.TryGetValue(processJobId, out existingControlJobId))
                    {
                        if (!string.Equals(existingControlJobId, controlJobId, StringComparison.Ordinal))
                        {
                            result = false;
                            break;
                        }
                    }
                }
            }

            Console.WriteLine(
                "[{0}] InMemoryJobRelationRepository CanLink ControlJobId={1}, ProcessJobIds=[{2}], Result={3}",
                DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff"),
                controlJobId,
                string.Join(",", copiedProcessJobIds),
                result);

            return result;
        }

        public string[] GetProcessJobIds(string controlJobId)
        {
            lock (_lock)
            {
                List<string> processJobIds;

                if (!_controlToProcesses.TryGetValue(controlJobId, out processJobIds))
                    return new string[0];

                return processJobIds.ToArray();
            }
        }

        public string GetControlJobIdOrDefault(string processJobId)
        {
            lock (_lock)
            {
                string controlJobId;

                if (_processToControl.TryGetValue(processJobId, out controlJobId))
                    return controlJobId;

                return null;
            }
        }

        public bool ContainsControlJob(string controlJobId)
        {
            lock (_lock)
            {
                return _controlToProcesses.ContainsKey(controlJobId);
            }
        }

        public bool ContainsProcessJob(string processJobId)
        {
            lock (_lock)
            {
                return _processToControl.ContainsKey(processJobId);
            }
        }

        public bool HasLinkedProcessJobs(string controlJobId)
        {
            lock (_lock)
            {
                List<string> processJobIds;

                return _controlToProcesses.TryGetValue(controlJobId, out processJobIds)
                    && processJobIds.Count > 0;
            }
        }

        public void UnlinkControlJob(string controlJobId)
        {
            bool existed;

            lock (_lock)
            {
                List<string> processJobIds;

                existed = _controlToProcesses.TryGetValue(controlJobId, out processJobIds);

                if (existed)
                {
                    foreach (var processJobId in processJobIds)
                        _processToControl.Remove(processJobId);
                }

                _controlToProcesses.Remove(controlJobId);
            }

            Console.WriteLine(
                "[{0}] InMemoryJobRelationRepository UnlinkControlJob ControlJobId={1}, Existed={2}",
                DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff"),
                controlJobId,
                existed);
        }

        public void UnlinkProcessJob(string processJobId)
        {
            string controlJobId = null;
            bool existed = false;

            lock (_lock)
            {
                if (_processToControl.TryGetValue(processJobId, out controlJobId))
                {
                    existed = true;
                    _processToControl.Remove(processJobId);

                    List<string> processJobIds;

                    if (_controlToProcesses.TryGetValue(controlJobId, out processJobIds))
                    {
                        processJobIds.Remove(processJobId);

                        if (processJobIds.Count == 0)
                            _controlToProcesses.Remove(controlJobId);
                    }
                }
            }

            Console.WriteLine(
                "[{0}] InMemoryJobRelationRepository UnlinkProcessJob ProcessJobId={1}, ControlJobId={2}, Existed={3}",
                DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff"),
                processJobId,
                controlJobId,
                existed);
        }

        public void Clear()
        {
            lock (_lock)
            {
                _controlToProcesses.Clear();
                _processToControl.Clear();
            }

            Console.WriteLine(
                "[{0}] InMemoryJobRelationRepository Clear",
                DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff"));
        }
    }
}