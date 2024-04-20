namespace DotNetToolbox.Data.Repositories;

public class UnitOfWorkRepository<TStrategy, TItem>(IEnumerable<TItem> data, TStrategy strategy)
    : UnitOfWorkRepository<UnitOfWorkRepository<TStrategy, TItem>, TStrategy, TItem>(data, strategy)
    where TStrategy : class, IUnitOfWorkRepositoryStrategy<TItem> {
    // ReSharper disable PossibleMultipleEnumeration
    protected UnitOfWorkRepository(IEnumerable<TItem> data, IStrategyFactory factory)
        : this(data, IsNotNull(factory).GetRequiredStrategy<TStrategy, TItem>(data)) {
    }
    // ReSharper enable PossibleMultipleEnumeration
    protected UnitOfWorkRepository(IStrategyFactory factory)
        : this([], factory) {
    }
}

public class UnitOfWorkRepository<TRepository, TStrategy, TItem>(IEnumerable<TItem> data, TStrategy strategy)
    : Repository<TRepository, TStrategy, TItem>(data, strategy),
      IUnitOfWorkRepository<TItem>
    where TRepository : UnitOfWorkRepository<TRepository, TStrategy, TItem>
    where TStrategy : class, IUnitOfWorkRepositoryStrategy<TItem> {
    public virtual Task<int> SaveChanges(CancellationToken ct = default)
        => Strategy.SaveChanges(ct);
    // ReSharper disable PossibleMultipleEnumeration
    protected UnitOfWorkRepository(IEnumerable<TItem> data, IStrategyFactory factory)
        : this(data, IsNotNull(factory).GetRequiredStrategy<TStrategy, TItem>(data)) {
    }
    // ReSharper enable PossibleMultipleEnumeration
    protected UnitOfWorkRepository(IStrategyFactory factory)
        : this([], factory) {
    }
}
