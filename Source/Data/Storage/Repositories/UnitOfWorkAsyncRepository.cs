namespace DotNetToolbox.Data.Repositories;

public class AsyncUnitOfWorkRepository<TItem>(IAsyncUnitOfWorkRepositoryStrategy<TItem> strategy, IEnumerable<TItem>? data = null)
    : AsyncRepository<IAsyncUnitOfWorkRepositoryStrategy<TItem>, TItem>(IsNotNull(strategy), data ?? [])
    , IAsyncUnitOfWorkRepository<TItem> {
    public AsyncUnitOfWorkRepository(IEnumerable<TItem>? data = null)
        : this(new InMemoryAsyncUnitOfWorkRepositoryStrategy<TItem>(), data) { }
    public AsyncUnitOfWorkRepository(IRepositoryStrategyProvider provider, IEnumerable<TItem>? data = null)
        : this((IAsyncUnitOfWorkRepositoryStrategy<TItem>)IsNotNull(provider).GetStrategyFor<TItem>(), data) { }

    public virtual void SaveChanges()
        => Strategy.SaveChanges();

    public virtual Task SaveChangesAsync(CancellationToken ct = default)
        => Strategy.SaveChangesAsync(ct);
}
