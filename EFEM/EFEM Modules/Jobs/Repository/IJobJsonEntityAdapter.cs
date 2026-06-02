namespace EFEM.Jobs.Repository
{
    public interface IJobJsonEntityAdapter<TEntity>
        where TEntity : class, IEntity<string>
    {
        TEntity Load(string path);

        void Save(string path, TEntity entity);
    }
}