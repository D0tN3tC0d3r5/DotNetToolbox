namespace DotNetToolbox.Data.Strategies;

public abstract class AsyncUnitOfWorkRepositoryStrategy<TItem>
    : AsyncRepositoryStrategy<TItem>
    where TItem : class {
    public virtual Task<int> SaveChangesAsync(CancellationToken ct = default)
        => throw new NotImplementedException();
}
