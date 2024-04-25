namespace DotNetToolbox.Data.Repositories;

public class AsyncUnitOfWorkRepository<TItem>(IAsyncUnitOfWorkRepositoryStrategy<TItem> strategy, IEnumerable<TItem>? data = null)
    : AsyncRepository<IAsyncUnitOfWorkRepositoryStrategy<TItem>, TItem>(IsNotNull(strategy), data ?? [])
    , IAsyncUnitOfWorkRepository<TItem> {
    public AsyncUnitOfWorkRepository(IEnumerable<TItem>? data = null)
        : this(new InMemoryAsyncUnitOfWorkRepositoryStrategy<TItem>(), data) { }
    public AsyncUnitOfWorkRepository(IRepositoryStrategyProvider provider, IEnumerable<TItem>? data = null)
        : this(IsNotNull(provider).GetRequiredAsyncUnitOfWorkStrategyFor<TItem>(), data) { }

    public virtual Task SaveChangesAsync(CancellationToken ct = default)
        => Strategy.SaveChangesAsync(ct);
}
