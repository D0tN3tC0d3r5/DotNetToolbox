namespace DotNetToolbox.Data.Strategies;

public abstract class AsyncUnitOfWorkRepositoryStrategy<TItem>
    : AsyncRepositoryStrategy<TItem> {
    public virtual Task<int> SaveChangesAsync(CancellationToken ct = default)
        => throw new NotImplementedException();
}
