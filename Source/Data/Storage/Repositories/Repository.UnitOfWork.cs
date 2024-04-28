namespace DotNetToolbox.Data.Repositories;

public class UnitOfWorkRepository<TItem>(IUnitOfWorkRepositoryStrategy<TItem> strategy, IEnumerable<TItem>? data = null)
    : Repository<IUnitOfWorkRepositoryStrategy<TItem>, TItem>(IsNotNull(strategy), data ?? [])
    , IUnitOfWorkRepository<TItem> {
    public UnitOfWorkRepository(IEnumerable<TItem>? data = null)
        : this(new InMemoryUnitOfWorkRepositoryStrategy<TItem>(), data) { }
    public UnitOfWorkRepository(IRepositoryStrategyProvider provider, IEnumerable<TItem>? data = null)
        : this(IsNotNull(provider).GetRequiredUnitOfWorkStrategyFor<TItem>(), data) { }

    public virtual void SaveChanges()
        => Strategy.SaveChanges();
}
