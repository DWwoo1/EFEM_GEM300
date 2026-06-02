namespace EFEM.Jobs.Repository
{
    public interface IEntity<TKey>
    {
        TKey Id { get; }
    }
}
