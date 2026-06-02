using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;

namespace EFEM.Jobs.Repository
{
    public sealed class JsonJobRelationRepository :
        IJobRelationRepository,
        IRemovedBindingTargetRepository,
        IDisposable
    {
        #region <Constructors>

        public JsonJobRelationRepository(string activePath, int maxParallelIO = 6)
        {
            if (string.IsNullOrWhiteSpace(activePath))
                throw new ArgumentNullException(nameof(activePath));

            if (maxParallelIO <= 0)
                throw new ArgumentOutOfRangeException(nameof(maxParallelIO));

            _activePath = activePath;
            Directory.CreateDirectory(_activePath);

            _storePath = Path.Combine(_activePath, "Relations.json");
            _ioThrottle = new SemaphoreSlim(maxParallelIO, maxParallelIO);

            LoadFromStorage();
        }

        #endregion </Constructors>

        #region <Fields>

        private readonly object _lock = new object();
        private readonly string _activePath;
        private readonly string _storePath;
        private readonly SemaphoreSlim _ioThrottle;

        private readonly Dictionary<string, List<string>> _controlToProcesses =
            new Dictionary<string, List<string>>(StringComparer.Ordinal);

        private readonly Dictionary<string, string> _processToControl =
            new Dictionary<string, string>(StringComparer.Ordinal);

        private readonly Dictionary<string, RemovedBindingTarget> _removedBindingTargets =
            new Dictionary<string, RemovedBindingTarget>(StringComparer.OrdinalIgnoreCase);

        private volatile bool _disposed;

        #endregion </Fields>

        #region <Methods>

        public void Link(string controlJobId, IEnumerable<string> processJobIds)
        {
            ThrowIfDisposed();

            if (string.IsNullOrWhiteSpace(controlJobId))
                throw new ArgumentException("ControlJobId is invalid.", nameof(controlJobId));

            if (processJobIds == null)
                throw new ArgumentNullException(nameof(processJobIds));

            var copiedProcessJobIds = CopyDistinctProcessJobIds(processJobIds);

            _ioThrottle.Wait();

            try
            {
                lock (_lock)
                {
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

                    SaveSnapshotUnderLock();
                }
            }
            finally
            {
                _ioThrottle.Release();
            }

            Console.WriteLine(
                "[{0}] JsonJobRelationRepository Link ControlJobId={1}, ProcessJobIds=[{2}]",
                DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff"),
                controlJobId,
                string.Join(",", copiedProcessJobIds));
        }

        public bool CanLink(string controlJobId, IEnumerable<string> processJobIds)
        {
            ThrowIfDisposed();

            if (string.IsNullOrWhiteSpace(controlJobId))
                return false;

            if (processJobIds == null)
                return false;

            bool result = true;
            var copiedProcessJobIds = new List<string>();

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
                "[{0}] JsonJobRelationRepository CanLink ControlJobId={1}, ProcessJobIds=[{2}], Result={3}",
                DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff"),
                controlJobId,
                string.Join(",", copiedProcessJobIds),
                result);

            return result;
        }

        public string[] GetProcessJobIds(string controlJobId)
        {
            ThrowIfDisposed();

            if (string.IsNullOrWhiteSpace(controlJobId))
                return new string[0];

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
            ThrowIfDisposed();

            if (string.IsNullOrWhiteSpace(processJobId))
                return null;

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
            ThrowIfDisposed();

            if (string.IsNullOrWhiteSpace(controlJobId))
                return false;

            lock (_lock)
            {
                return _controlToProcesses.ContainsKey(controlJobId);
            }
        }

        public bool ContainsProcessJob(string processJobId)
        {
            ThrowIfDisposed();

            if (string.IsNullOrWhiteSpace(processJobId))
                return false;

            lock (_lock)
            {
                return _processToControl.ContainsKey(processJobId);
            }
        }

        public bool HasLinkedProcessJobs(string controlJobId)
        {
            ThrowIfDisposed();

            if (string.IsNullOrWhiteSpace(controlJobId))
                return false;

            lock (_lock)
            {
                List<string> processJobIds;

                return _controlToProcesses.TryGetValue(controlJobId, out processJobIds)
                    && processJobIds.Count > 0;
            }
        }

        public void UnlinkControlJob(string controlJobId)
        {
            ThrowIfDisposed();

            if (string.IsNullOrWhiteSpace(controlJobId))
                return;

            bool existed = false;

            _ioThrottle.Wait();

            try
            {
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

                    SaveSnapshotUnderLock();
                }
            }
            finally
            {
                _ioThrottle.Release();
            }

            Console.WriteLine(
                "[{0}] JsonJobRelationRepository UnlinkControlJob ControlJobId={1}, Existed={2}",
                DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff"),
                controlJobId,
                existed);
        }

        public void UnlinkProcessJob(string processJobId)
        {
            ThrowIfDisposed();

            if (string.IsNullOrWhiteSpace(processJobId))
                return;

            string controlJobId = null;
            bool existed = false;

            _ioThrottle.Wait();

            try
            {
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

                    SaveSnapshotUnderLock();
                }
            }
            finally
            {
                _ioThrottle.Release();
            }

            Console.WriteLine(
                "[{0}] JsonJobRelationRepository UnlinkProcessJob ProcessJobId={1}, ControlJobId={2}, Existed={3}",
                DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff"),
                processJobId,
                controlJobId,
                existed);
        }

        public void Clear()
        {
            ThrowIfDisposed();

            _ioThrottle.Wait();

            try
            {
                lock (_lock)
                {
                    _controlToProcesses.Clear();
                    _processToControl.Clear();

                    SaveSnapshotUnderLock();
                }
            }
            finally
            {
                _ioThrottle.Release();
            }

            Console.WriteLine(
                "[{0}] JsonJobRelationRepository Clear",
                DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff"));
        }

        public void Dispose()
        {
            if (_disposed)
                return;

            _disposed = true;

            if (_ioThrottle != null)
                _ioThrottle.Dispose();
        }

        #endregion </Methods>

        #region <Internal>

        private void LoadFromStorage()
        {
            lock (_lock)
            {
                _controlToProcesses.Clear();
                _processToControl.Clear();
                _removedBindingTargets.Clear();

                if (!File.Exists(_storePath))
                    return;

                RelationSnapshot snapshot = null;

                try
                {
                    snapshot = JsonJobStorageFile.LoadOrBackup<RelationSnapshot>(_storePath);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(
                        "[{0}] JsonJobRelationRepository Load failed. Error={1}",
                        DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff"),
                        ex.Message);

                    return;
                }

                if (snapshot == null)
                    return;

                if (snapshot.ControlToProcesses != null)
                {
                    foreach (var item in snapshot.ControlToProcesses)
                    {
                        if (string.IsNullOrWhiteSpace(item.Key))
                            continue;

                        var copiedProcessJobIds = CopyDistinctProcessJobIds(item.Value);

                        _controlToProcesses[item.Key] = copiedProcessJobIds;

                        foreach (var processJobId in copiedProcessJobIds)
                        {
                            string existingControlJobId;

                            if (_processToControl.TryGetValue(processJobId, out existingControlJobId))
                            {
                                throw new InvalidDataException(
                                    "Invalid job relation storage. ProcessJob is linked to multiple ControlJobs. ProcessJobId="
                                    + processJobId
                                    + ", ControlJobId1="
                                    + existingControlJobId
                                    + ", ControlJobId2="
                                    + item.Key);
                            }

                            _processToControl[processJobId] = item.Key;
                        }
                    }
                }

                if (snapshot.RemovedBindingTargets != null)
                {
                    foreach (RemovedBindingTarget target in snapshot.RemovedBindingTargets)
                    {
                        if (!IsValidRemovedBindingTarget(target))
                            continue;

                        _removedBindingTargets[target.GetKey()] = target;
                    }
                }
            }
        }

        private void SaveSnapshotUnderLock()
        {
            var snapshot = new RelationSnapshot();

            snapshot.ControlToProcesses =
                new Dictionary<string, List<string>>(StringComparer.Ordinal);

            foreach (var item in _controlToProcesses)
            {
                snapshot.ControlToProcesses[item.Key] =
                    new List<string>(item.Value);
            }

            snapshot.RemovedBindingTargets =
                new List<RemovedBindingTarget>();

            foreach (RemovedBindingTarget target in _removedBindingTargets.Values)
            {
                if (!IsValidRemovedBindingTarget(target))
                    continue;

                snapshot.RemovedBindingTargets.Add(target);
            }

            JsonJobStorageFile.SaveAtomic(_storePath, snapshot);
        }

        private static List<string> CopyDistinctProcessJobIds(IEnumerable<string> processJobIds)
        {
            var result = new List<string>();

            if (processJobIds == null)
                return result;

            foreach (var processJobId in processJobIds)
            {
                if (string.IsNullOrWhiteSpace(processJobId))
                    continue;

                if (!result.Contains(processJobId))
                    result.Add(processJobId);
            }

            return result;
        }

        private void ThrowIfDisposed()
        {
            if (_disposed)
                throw new ObjectDisposedException(nameof(JsonJobRelationRepository));
        }

        private sealed class RelationSnapshot
        {
            public Dictionary<string, List<string>> ControlToProcesses { get; set; }

            /*
             * ProcessJob.MaterialInfo 원본은 유지하되,
             * Binder의 active binding target에서 제외할 Carrier/Slot 목록.
             */
            public List<RemovedBindingTarget> RemovedBindingTargets { get; set; }
        }
        private static bool IsValidRemovedBindingTarget(RemovedBindingTarget target)
        {
            if (target == null)
                return false;

            if (string.IsNullOrWhiteSpace(target.ProcessJobId))
                return false;

            if (string.IsNullOrWhiteSpace(target.CarrierId))
                return false;

            if (target.Slot <= 0)
                return false;

            return true;
        }
        #endregion </Internal>

        #region <IRemovedBindingTargetRepository>
        public void AddOrUpdate(RemovedBindingTarget target)
        {
            ThrowIfDisposed();

            if (!IsValidRemovedBindingTarget(target))
                return;

            _ioThrottle.Wait();

            try
            {
                lock (_lock)
                {
                    _removedBindingTargets[target.GetKey()] = target;
                    SaveSnapshotUnderLock();
                }
            }
            finally
            {
                _ioThrottle.Release();
            }

            Console.WriteLine(
                "[{0}] JsonJobRelationRepository AddOrUpdateRemovedBindingTarget ProcessJobId={1}, CarrierId={2}, Slot={3}, Reason={4}",
                DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff"),
                target.ProcessJobId,
                target.CarrierId,
                target.Slot,
                target.Reason);
        }

        public void Remove(
            string processJobId,
            string carrierId,
            int slot)
        {
            ThrowIfDisposed();

            if (string.IsNullOrWhiteSpace(processJobId))
                return;

            if (string.IsNullOrWhiteSpace(carrierId))
                return;

            if (slot <= 0)
                return;

            string key = RemovedBindingTarget.CreateKey(
                processJobId,
                carrierId,
                slot);

            bool removed = false;

            _ioThrottle.Wait();

            try
            {
                lock (_lock)
                {
                    removed = _removedBindingTargets.Remove(key);
                    SaveSnapshotUnderLock();
                }
            }
            finally
            {
                _ioThrottle.Release();
            }

            Console.WriteLine(
                "[{0}] JsonJobRelationRepository RemoveRemovedBindingTarget ProcessJobId={1}, CarrierId={2}, Slot={3}, Removed={4}",
                DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff"),
                processJobId,
                carrierId,
                slot,
                removed);
        }

        public void RemoveByProcessJob(string processJobId)
        {
            ThrowIfDisposed();

            if (string.IsNullOrWhiteSpace(processJobId))
                return;

            _ioThrottle.Wait();

            try
            {
                lock (_lock)
                {
                    var removeKeys = new List<string>();

                    foreach (KeyValuePair<string, RemovedBindingTarget> item in _removedBindingTargets)
                    {
                        if (item.Value == null)
                            continue;

                        if (string.Equals(
                            item.Value.ProcessJobId,
                            processJobId,
                            StringComparison.OrdinalIgnoreCase))
                        {
                            removeKeys.Add(item.Key);
                        }
                    }

                    foreach (string key in removeKeys)
                        _removedBindingTargets.Remove(key);

                    SaveSnapshotUnderLock();
                }
            }
            finally
            {
                _ioThrottle.Release();
            }

            Console.WriteLine(
                "[{0}] JsonJobRelationRepository RemoveRemovedBindingTargetsByProcessJob ProcessJobId={1}",
                DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff"),
                processJobId);
        }

        public IReadOnlyList<RemovedBindingTarget> GetAll()
        {
            ThrowIfDisposed();

            lock (_lock)
            {
                return new List<RemovedBindingTarget>(_removedBindingTargets.Values);
            }
        }
        void IRemovedBindingTargetRepository.Clear()
        {
            ThrowIfDisposed();

            _ioThrottle.Wait();

            try
            {
                lock (_lock)
                {
                    _removedBindingTargets.Clear();
                    SaveSnapshotUnderLock();
                }
            }
            finally
            {
                _ioThrottle.Release();
            }

            Console.WriteLine(
                "[{0}] JsonJobRelationRepository ClearRemovedBindingTargets",
                DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff"));
        }
        #endregion </IRemovedBindingTargetRepository>
    }
}