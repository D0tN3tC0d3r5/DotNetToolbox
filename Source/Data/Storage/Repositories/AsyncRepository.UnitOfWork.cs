namespace DotNetToolbox.Data.Repositories;

public class AsyncUnitOfWorkRepository<TStrategy, TItem>(IEnumerable<TItem> data, TStrategy strategy)
    : AsyncUnitOfWorkRepository<AsyncUnitOfWorkRepository<TStrategy, TItem>, TStrategy, TItem>(data, strategy)
    where TStrategy : class, IAsyncUnitOfWorkRepositoryStrategy<TItem>{
    // ReSharper disable PossibleMultipleEnumeration
    public AsyncUnitOfWorkRepository(IEnumerable<TItem> data, IStrategyFactory factory)
        : this(data, IsNotNull(factory).GetRequiredAsyncStrategy<TStrategy, TItem>(data)) { }
    // ReSharper enable PossibleMultipleEnumeration
    public AsyncUnitOfWorkRepository(IStrategyFactory factory)
        : this([], factory) { }
}

public class AsyncUnitOfWorkRepository<TRepository, TStrategy, TItem>(IEnumerable<TItem> data, TStrategy strategy)
    : AsyncRepository<TRepository, TStrategy, TItem>(data, strategy),
      IAsyncUnitOfWorkRepository<TItem>
    where TRepository : AsyncUnitOfWorkRepository<TRepository, TStrategy, TItem>
    where TStrategy : class, IAsyncUnitOfWorkRepositoryStrategy<TItem>{
    public virtual Task<int> SaveChangesAsync(CancellationToken ct = default)
        => Strategy.SaveChangesAsync(ct);
    // ReSharper disable PossibleMultipleEnumeration
    protected AsyncUnitOfWorkRepository(IEnumerable<TItem> data, IStrategyFactory factory)
        : this(data, IsNotNull(factory).GetRequiredAsyncStrategy<TStrategy, TItem>(data)) {
    }
    // ReSharper enable PossibleMultipleEnumeration
    protected AsyncUnitOfWorkRepository(IStrategyFactory factory)
        : this([], factory) {
    }
}
