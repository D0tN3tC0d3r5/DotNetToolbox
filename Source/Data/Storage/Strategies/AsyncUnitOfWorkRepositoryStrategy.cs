namespace DotNetToolbox.Data.Strategies;

public abstract class AsyncUnitOfWorkRepositoryStrategy<TItem>
    : AsyncRepositoryStrategy<TItem>
    , IAsyncUnitOfWorkRepositoryStrategy<TItem> {
    public virtual void SaveChanges()
        => throw new NotImplementedException();
    public virtual Task SaveChangesAsync(CancellationToken ct = default)
        => throw new NotImplementedException();
}
