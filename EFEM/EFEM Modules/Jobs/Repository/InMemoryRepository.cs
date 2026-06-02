using System;
using System.Collections.Generic;

namespace EFEM.Jobs.Repository
{
    public sealed class InMemoryRepository<TEntity, TKey> : IOrderedRepository<TEntity, TKey>
        where TEntity : class, IEntity<TKey>
    {
        private readonly object _lock = new object();

        private readonly Dictionary<TKey, TEntity> _items =
            new Dictionary<TKey, TEntity>();

        private readonly List<TKey> _order =
            new List<TKey>();

        public void Add(TEntity entity)
        {
            if (entity == null)
                throw new ArgumentNullException(nameof(entity));

            lock (_lock)
            {
                if (_items.ContainsKey(entity.Id))
                    throw new InvalidOperationException("Entity already exists. Id=" + entity.Id);

                _items.Add(entity.Id, entity);
                _order.Add(entity.Id);
            }

            WriteLine("Add", entity.Id, entity);
        }

        public void AddOrUpdate(TEntity entity)
        {
            if (entity == null)
                throw new ArgumentNullException(nameof(entity));

            lock (_lock)
            {
                if (!_items.ContainsKey(entity.Id))
                    _order.Add(entity.Id);

                _items[entity.Id] = entity;
            }

            WriteLine("AddOrUpdate", entity.Id, entity);
        }

        public TEntity GetOrDefault(TKey id)
        {
            lock (_lock)
            {
                TEntity entity;

                if (_items.TryGetValue(id, out entity))
                    return entity;

                return null;
            }
        }

        public bool Contains(TKey id)
        {
            lock (_lock)
            {
                return _items.ContainsKey(id);
            }
        }

        public bool Remove(TKey id)
        {
            bool removed;

            lock (_lock)
            {
                removed = _items.Remove(id);

                if (removed)
                    _order.Remove(id);
            }

            Console.WriteLine(
                "[{0}] InMemoryRepository<{1}> Remove Id={2}, Removed={3}",
                DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff"),
                typeof(TEntity).Name,
                id,
                removed);

            return removed;
        }

        public IReadOnlyList<TEntity> GetAll()
        {
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

        public IReadOnlyList<TKey> GetOrderedIds()
        {
            lock (_lock)
            {
                return new List<TKey>(_order);
            }
        }

        public bool IsFirst(TKey id)
        {
            lock (_lock)
            {
                if (_order.Count == 0)
                    return false;

                return EqualityComparer<TKey>.Default.Equals(_order[0], id);
            }
        }

        public bool MoveToFirst(TKey id)
        {
            return MoveToIndex(id, 0);
        }
        public int IndexOf(TKey id)
        {
            lock (_lock)
            {
                return _order.IndexOf(id);
            }
        }

        public bool MoveToIndex(TKey id, int index)
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
            }

            Console.WriteLine(
                "[{0}] InMemoryRepository<{1}> MoveToIndex Id={2}, Index={3}",
                DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff"),
                typeof(TEntity).Name,
                id,
                index);

            return true;
        }

        public void Clear()
        {
            lock (_lock)
            {
                _items.Clear();
                _order.Clear();
            }

            Console.WriteLine(
                "[{0}] InMemoryRepository<{1}> Clear",
                DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff"),
                typeof(TEntity).Name);
        }

        private static void WriteLine(string action, TKey id, TEntity entity)
        {
            Console.WriteLine(
                "[{0}] InMemoryRepository<{1}> {2} Id={3}, Data={4}",
                DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff"),
                typeof(TEntity).Name,
                action,
                id,
                entity);
        }
    }
}