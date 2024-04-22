namespace DotNetToolbox.Data.Repositories;

public class UnitOfWorkRepository<TStrategy, TItem>(TStrategy strategy)
    : UnitOfWorkRepository<UnitOfWorkRepository<TStrategy, TItem>, TStrategy, TItem>(strategy)
    where TStrategy : class, IUnitOfWorkRepositoryStrategy<TItem>{
    // ReSharper disable PossibleMultipleEnumeration
    protected UnitOfWorkRepository(IEnumerable<TItem> data, IStrategyFactory factory)
        : this(IsNotNull(factory).GetRequiredStrategy<TStrategy, TItem>(data)) {
    }
    // ReSharper enable PossibleMultipleEnumeration
    protected UnitOfWorkRepository(IStrategyFactory factory)
        : this([], factory) {
    }
}

public class UnitOfWorkRepository<TRepository, TStrategy, TItem>(TStrategy strategy)
    : Repository<TRepository, TStrategy, TItem>(strategy),
      IUnitOfWorkRepository<TItem>
    where TRepository : UnitOfWorkRepository<TRepository, TStrategy, TItem>
    where TStrategy : class, IUnitOfWorkRepositoryStrategy<TItem>{
    public virtual Task<int> SaveChanges(CancellationToken ct = default)
        => Strategy.SaveChanges(ct);
    // ReSharper disable PossibleMultipleEnumeration
    protected UnitOfWorkRepository(IEnumerable<TItem> data, IStrategyFactory factory)
        : this(IsNotNull(factory).GetRequiredStrategy<TStrategy, TItem>(data)) {
    }
    // ReSharper enable PossibleMultipleEnumeration
    protected UnitOfWorkRepository(IStrategyFactory factory)
        : this([], factory) {
    }
}
