namespace DotNetToolbox.Data.Repositories;

internal class RepositoryFactory
    : IRepositoryFactory {
    public IRepository<TItem> CreateRepository<TItem>(IEnumerable<TItem>? data = null) {
        var repository = InstanceFactory.Create<InMemoryRepository<TItem>>();
        if (data is not null) repository.Seed(data);
        return repository;
    }

    public IRepository<TItem> CreateRepository<TItem, TStrategy>(IEnumerable<TItem>? data = null)
        where TStrategy : class, IRepositoryStrategy<TItem>, new() {
        var repository = InstanceFactory.Create<Repository<TStrategy, TItem>>();
        if (data is not null) repository.Seed(data);
        return repository;
    }
}
