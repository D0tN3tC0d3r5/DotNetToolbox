namespace DotNetToolbox.Data.Strategies;

public abstract class UnitOfWorkRepositoryStrategy<TItem>
    : RepositoryStrategy<TItem>
    , IUnitOfWorkRepositoryStrategy<TItem> {
    public virtual void SaveChanges()
        => throw new NotImplementedException();
}
