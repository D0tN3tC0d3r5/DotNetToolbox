namespace DotNetToolbox.Data.Repositories;

public abstract class InMemoryRepositoryBase<TRepository, TItem, TKey>
    : Repository<InMemoryRepositoryStrategy<TRepository, TItem, TKey>, TItem, TKey>
    where TRepository : InMemoryRepositoryBase<TRepository, TItem, TKey>
    where TItem : class, IEntity<TKey>, new()
    where TKey : notnull {
    protected InMemoryRepositoryBase(IEnumerable<TItem>? data = null)
        : base(data) {
        var lazyRepository = new Lazy<TRepository>(() => (TRepository)this);
        Strategy = new InMemoryRepositoryStrategy<TRepository, TItem, TKey>() {
            Repository = this,
        };
    }
}

public abstract class InMemoryRepositoryBase<TRepository, TItem>
    : Repository<InMemoryRepositoryStrategy<TRepository, TItem>, TItem>
    where TRepository : InMemoryRepositoryBase<TRepository, TItem> {
    protected InMemoryRepositoryBase(IEnumerable<TItem>? data = null)
        : base(data) {
        Strategy = new InMemoryRepositoryStrategy<TRepository, TItem>() {
            Repository = this,
        };
    }
}
