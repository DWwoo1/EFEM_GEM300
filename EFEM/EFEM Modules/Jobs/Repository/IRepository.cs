using System.Collections.Generic;

namespace EFEM.Jobs.Repository
{
    public interface IOrderedRepository<TEntity, TKey> : IRepository<TEntity, TKey>
        where TEntity : class, IEntity<TKey>
    {
        bool IsFirst(TKey id);

        bool MoveToFirst(TKey id);

        int IndexOf(TKey id);

        bool MoveToIndex(TKey id, int index);

        IReadOnlyList<TKey> GetOrderedIds();
    }
    public interface IRepository<TEntity, TKey> where TEntity : class, IEntity<TKey>
    {
        void Add(TEntity entity);

        void AddOrUpdate(TEntity entity);

        TEntity GetOrDefault(TKey id);

        bool Contains(TKey id);

        bool Remove(TKey id);

        IReadOnlyList<TEntity> GetAll();

        void Clear();
    }
}
