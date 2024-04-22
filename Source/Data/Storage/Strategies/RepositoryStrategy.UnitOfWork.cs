namespace DotNetToolbox.Data.Strategies;

public abstract class UnitOfWorkRepositoryStrategy<TItem>
    : RepositoryStrategy<TItem>{
    public virtual Task<int> SaveChanges(CancellationToken ct = default)
        => throw new NotImplementedException();
}
