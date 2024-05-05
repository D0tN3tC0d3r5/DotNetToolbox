namespace DotNetToolbox.Data.Repositories;

internal class RepositoryFactory
    : IRepositoryFactory {
    public IRepository<TItem> CreateRepository<TItem>(IEnumerable<TItem>? data = null)
        => InstanceFactory.Create<Repository<TItem>>(data);

    public IRepository<TItem> CreateRepository<TItem>(string name, IEnumerable<TItem>? data = null)
        => InstanceFactory.Create<Repository<TItem>>(name, data);

    public IRepository<TItem> CreateRepository<TItem, TStrategy>(IEnumerable<TItem>? data = null)
        where TStrategy : class, IRepositoryStrategy<TItem>, new()
        => InstanceFactory.Create<RepositoryBase<TStrategy, TItem>>(data);

    public IRepository<TItem> CreateRepository<TItem, TStrategy>(string name, IEnumerable<TItem>? data = null)
        where TStrategy : class, IRepositoryStrategy<TItem>, new()
        => InstanceFactory.Create<RepositoryBase<TStrategy, TItem>>(name, data);
}
