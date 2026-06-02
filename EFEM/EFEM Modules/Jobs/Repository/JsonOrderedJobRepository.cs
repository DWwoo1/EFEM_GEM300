using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;

namespace EFEM.Jobs.Repository
{
    public sealed class JsonOrderedJobRepository<TEntity> :
        IOrderedRepository<TEntity, string>,
        IDisposable
        where TEntity : class, IEntity<string>
    {
        #region <Constructors>
        public JsonOrderedJobRepository(
            string activePath,
            IJobJsonEntityAdapter<TEntity> adapter,
            int maxParallelIO = 6)
        {
            if (string.IsNullOrWhiteSpace(activePath))
                throw new ArgumentNullException(nameof(activePath));

            if (adapter == null)
                throw new ArgumentNullException(nameof(adapter));

            if (maxParallelIO <= 0)
                throw new ArgumentOutOfRangeException(nameof(maxParallelIO));

            _activePath = activePath;
            _adapter = adapter;

            Directory.CreateDirectory(_activePath);

            _ioThrottle = new SemaphoreSlim(maxParallelIO, maxParallelIO);

            LoadFromStorage();
        }
        #endregion </Constructors>

        #region <Fields>

        private readonly object _lock = new object();
        private readonly string _activePath;
        private readonly SemaphoreSlim _ioThrottle;
        private readonly IJobJsonEntityAdapter<TEntity> _adapter;

        private readonly Dictionary<string, TEntity> _items =
            new Dictionary<string, TEntity>(StringComparer.Ordinal);

        private readonly List<string> _order =
            new List<string>();

        private volatile bool _disposed;

        #endregion </Fields>

        #region <Methods>

        public void Add(TEntity entity)
        {
            ThrowIfDisposed();

            if (entity == null)
                throw new ArgumentNullException(nameof(entity));

            ValidateKey(entity.Id);

            _ioThrottle.Wait();

            try
            {
                lock (_lock)
                {
                    if (_items.ContainsKey(entity.Id))
                        throw new InvalidOperationException("Entity already exists. Id=" + entity.Id);

                    _items.Add(entity.Id, entity);
                    _order.Add(entity.Id);

                    try
                    {
                        SaveEntityUnderLock(entity);
                        SaveOrderUnderLock();
                    }
                    catch
                    {
                        _items.Remove(entity.Id);
                        _order.Remove(entity.Id);

                        throw;
                    }
                }
            }
            finally
            {
                _ioThrottle.Release();
            }

            WriteLine("Add", entity.Id, entity);
        }

        public void AddOrUpdate(TEntity entity)
        {
            ThrowIfDisposed();

            if (entity == null)
                throw new ArgumentNullException(nameof(entity));

            ValidateKey(entity.Id);

            _ioThrottle.Wait();

            try
            {
                lock (_lock)
                {
                    TEntity oldEntity;
                    bool existed = _items.TryGetValue(entity.Id, out oldEntity);

                    if (!existed)
                        _order.Add(entity.Id);

                    _items[entity.Id] = entity;

                    try
                    {
                        SaveEntityUnderLock(entity);
                        SaveOrderUnderLock();
                    }
                    catch
                    {
                        if (existed)
                        {
                            _items[entity.Id] = oldEntity;
                        }
                        else
                        {
                            _items.Remove(entity.Id);
                            _order.Remove(entity.Id);
                        }

                        throw;
                    }
                }
            }
            finally
            {
                _ioThrottle.Release();
            }

            WriteLine("AddOrUpdate", entity.Id, entity);
        }

        public TEntity GetOrDefault(string id)
        {
            ThrowIfDisposed();

            if (string.IsNullOrWhiteSpace(id))
                return null;

            lock (_lock)
            {
                TEntity entity;

                if (_items.TryGetValue(id, out entity))
                    return entity;

                return null;
            }
        }

        public bool Contains(string id)
        {
            ThrowIfDisposed();

            if (string.IsNullOrWhiteSpace(id))
                return false;

            lock (_lock)
            {
                return _items.ContainsKey(id);
            }
        }

        public bool Remove(string id)
        {
            ThrowIfDisposed();

            if (string.IsNullOrWhiteSpace(id))
                return false;

            bool removed = false;

            _ioThrottle.Wait();

            try
            {
                lock (_lock)
                {
                    removed = _items.Remove(id);

                    if (removed)
                    {
                        _order.Remove(id);

                        DeleteEntityFilesUnderLock(id);
                        SaveOrderUnderLock();
                    }
                }
            }
            finally
            {
                _ioThrottle.Release();
            }

            Console.WriteLine(
                "[{0}] JsonOrderedJobRepository<{1}> Remove Id={2}, Removed={3}",
                DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff"),
                typeof(TEntity).Name,
                id,
                removed);

            return removed;
        }

        public IReadOnlyList<TEntity> GetAll()
        {
            ThrowIfDisposed();

            lock (_lock)
            {
                var result = new List<TEntity>();

                foreach (var id in _order)
                {
                    TEntity entity;

                    if (_items.TryGetValue(id, out entity))
                        result.Add(entity);
                }

                return result;
            }
        }

        public IReadOnlyList<string> GetOrderedIds()
        {
            ThrowIfDisposed();

            lock (_lock)
            {
                return new List<string>(_order);
            }
        }

        public bool IsFirst(string id)
        {
            ThrowIfDisposed();

            if (string.IsNullOrWhiteSpace(id))
                return false;

            lock (_lock)
            {
                if (_order.Count == 0)
                    return false;

                return string.Equals(_order[0], id, StringComparison.Ordinal);
            }
        }

        public bool MoveToFirst(string id)
        {
            return MoveToIndex(id, 0);
        }

        public int IndexOf(string id)
        {
            ThrowIfDisposed();

            if (string.IsNullOrWhiteSpace(id))
                return -1;

            lock (_lock)
            {
                return _order.IndexOf(id);
            }
        }

        public bool MoveToIndex(string id, int index)
        {
            ThrowIfDisposed();

            if (string.IsNullOrWhiteSpace(id))
                return false;

            bool moved = false;

            _ioThrottle.Wait();

            try
            {
                lock (_lock)
                {
                    if (!_items.ContainsKey(id))
                        return false;

                    int currentIndex = _order.IndexOf(id);

                    if (currentIndex < 0)
                        return false;

                    if (index < 0)
                        index = 0;

                    if (index >= _order.Count)
                        index = _order.Count - 1;

                    if (currentIndex == index)
                        return false;

                    _order.RemoveAt(currentIndex);

                    if (index > _order.Count)
                        index = _order.Count;

                    _order.Insert(index, id);

                    SaveOrderUnderLock();

                    moved = true;
                }
            }
            finally
            {
                _ioThrottle.Release();
            }

            if (moved)
            {
                Console.WriteLine(
                    "[{0}] JsonOrderedJobRepository<{1}> MoveToIndex Id={2}, Index={3}",
                    DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff"),
                    typeof(TEntity).Name,
                    id,
                    index);
            }

            return moved;
        }

        public void Clear()
        {
            ThrowIfDisposed();

            _ioThrottle.Wait();

            try
            {
                lock (_lock)
                {
                    _items.Clear();
                    _order.Clear();

                    DeleteAllStorageFilesUnderLock();
                }
            }
            finally
            {
                _ioThrottle.Release();
            }

            Console.WriteLine(
                "[{0}] JsonOrderedJobRepository<{1}> Clear",
                DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff"),
                typeof(TEntity).Name);
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
                _items.Clear();
                _order.Clear();

                string[] files = Directory.GetFiles(_activePath, "*.json");

                foreach (var file in files)
                {
                    string name = Path.GetFileName(file);

                    if (string.Equals(
                        name,
                        JsonJobStorageFile.OrderFileName,
                        StringComparison.OrdinalIgnoreCase))
                    {
                        continue;
                    }

                    try
                    {
                        var entity = _adapter.Load(file);

                        if (entity == null || string.IsNullOrWhiteSpace(entity.Id))
                            continue;

                        if (!_items.ContainsKey(entity.Id))
                            _items.Add(entity.Id, entity);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(
                            "[{0}] JsonOrderedJobRepository<{1}> Load failed. File={2}, Error={3}",
                            DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff"),
                            typeof(TEntity).Name,
                            file,
                            ex.ToString());
                    }
                }

                LoadOrderUnderLock();

                Console.WriteLine(
                    "[{0}] JsonOrderedJobRepository<{1}> Loaded. Directory={2}, ItemCount={3}, OrderCount={4}",
                    DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff"),
                    typeof(TEntity).Name,
                    _activePath,
                    _items.Count,
                    _order.Count);
            }
        }
        private void SaveEntityUnderLock(TEntity entity)
        {
            var path = JsonJobStorageFile.EntityPath(_activePath, entity.Id);
            _adapter.Save(path, entity);
        }

        private void SaveOrderUnderLock()
        {
            var path = JsonJobStorageFile.OrderPath(_activePath);
            JsonJobStorageFile.SaveAtomic(path, _order);
        }

        private void DeleteEntityFilesUnderLock(string id)
        {
            var path = JsonJobStorageFile.EntityPath(_activePath, id);
            var bak = path + ".bak";

            TryDelete(path);
            TryDelete(bak);
        }

        private void DeleteAllStorageFilesUnderLock()
        {
            string[] files = Directory.GetFiles(_activePath);

            foreach (var file in files)
            {
                TryDelete(file);
            }
        }

        private static void TryDelete(string path)
        {
            try
            {
                if (File.Exists(path))
                    File.Delete(path);
            }
            catch
            {
            }
        }

        private static void ValidateKey(string key)
        {
            if (string.IsNullOrWhiteSpace(key))
                throw new ArgumentException("Key is required.", nameof(key));
        }

        private void ThrowIfDisposed()
        {
            if (_disposed)
                throw new ObjectDisposedException(nameof(JsonOrderedJobRepository<TEntity>));
        }

        private static void WriteLine(string action, string id, TEntity entity)
        {
            Console.WriteLine(
                "[{0}] JsonOrderedJobRepository<{1}> {2} Id={3}, Data={4}",
                DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff"),
                typeof(TEntity).Name,
                action,
                id,
                entity);
        }
        private void LoadOrderUnderLock()
        {
            var orderPath = JsonJobStorageFile.OrderPath(_activePath);
            List<string> storedOrder = null;

            if (File.Exists(orderPath))
            {
                try
                {
                    storedOrder = JsonJobStorageFile.LoadOrBackup<List<string>>(orderPath);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(
                        "[{0}] JsonOrderedJobRepository<{1}> Order load failed. File={2}, Error={3}",
                        DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff"),
                        typeof(TEntity).Name,
                        orderPath,
                        ex.ToString());
                }
            }

            if (storedOrder != null)
            {
                foreach (var id in storedOrder)
                {
                    if (string.IsNullOrWhiteSpace(id))
                        continue;

                    if (!_items.ContainsKey(id))
                        continue;

                    if (!_order.Contains(id))
                        _order.Add(id);
                }
            }

            var missingIds = new List<string>();

            foreach (var item in _items)
            {
                if (!_order.Contains(item.Key))
                    missingIds.Add(item.Key);
            }

            missingIds.Sort(StringComparer.Ordinal);

            foreach (var id in missingIds)
                _order.Add(id);
        }
        #endregion </Internal>
    }
}