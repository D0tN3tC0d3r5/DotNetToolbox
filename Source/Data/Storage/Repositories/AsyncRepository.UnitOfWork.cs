namespace DotNetToolbox.Data.Repositories;

public class AsyncUnitOfWorkRepository<TStrategy, TItem>
    : AsyncUnitOfWorkRepository<AsyncUnitOfWorkRepository<TStrategy, TItem>, TStrategy, TItem>
    where TStrategy : class, IAsyncUnitOfWorkRepositoryStrategy<TItem>{
    public AsyncUnitOfWorkRepository(TStrategy strategy)
        : base(strategy) {
    }
    public AsyncUnitOfWorkRepository(IStrategyFactory factory)
        : base(factory) { }
    public AsyncUnitOfWorkRepository(IEnumerable<TItem> data, TStrategy strategy)
        : base(data, strategy) {
    }
    public AsyncUnitOfWorkRepository(IEnumerable<TItem> data, IStrategyFactory factory)
        : base(data, factory) { }
}

public class AsyncUnitOfWorkRepository<TRepository, TStrategy, TItem>
    : AsyncRepository<TRepository, TStrategy, TItem>,
      IAsyncUnitOfWorkRepository<TItem>
    where TRepository : AsyncUnitOfWorkRepository<TRepository, TStrategy, TItem>
    where TStrategy : class, IAsyncUnitOfWorkRepositoryStrategy<TItem>{
    public AsyncUnitOfWorkRepository(TStrategy strategy)
        : base(strategy) {
    }
    protected AsyncUnitOfWorkRepository(IStrategyFactory factory)
        : base(factory) {
    }
    public AsyncUnitOfWorkRepository(IEnumerable<TItem> data, TStrategy strategy)
        : base(data, strategy) {
    }
    protected AsyncUnitOfWorkRepository(IEnumerable<TItem> data, IStrategyFactory factory)
        : base(data, factory) {
    }

    public virtual Task<int> SaveChangesAsync(CancellationToken ct = default)
        => Strategy.SaveChangesAsync(ct);
}
