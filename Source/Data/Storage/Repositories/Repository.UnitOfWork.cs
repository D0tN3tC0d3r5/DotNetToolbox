namespace DotNetToolbox.Data.Repositories;

public class UnitOfWorkRepository<TStrategy, TItem>
    : UnitOfWorkRepository<UnitOfWorkRepository<TStrategy, TItem>, TStrategy, TItem>
    where TStrategy : class, IUnitOfWorkRepositoryStrategy<TItem>{
    public UnitOfWorkRepository(TStrategy strategy)
        : base(strategy) {
    }
    public UnitOfWorkRepository(IStrategyFactory factory)
        : base(factory) {
    }
    public UnitOfWorkRepository(IEnumerable<TItem> data, TStrategy strategy)
        : base(data, strategy) {
    }
    public UnitOfWorkRepository(IEnumerable<TItem> data, IStrategyFactory factory)
        : base(data, factory) {
    }
}

public class UnitOfWorkRepository<TRepository, TStrategy, TItem>
    : Repository<TRepository, TStrategy, TItem>,
      IUnitOfWorkRepository<TItem>
    where TRepository : UnitOfWorkRepository<TRepository, TStrategy, TItem>
    where TStrategy : class, IUnitOfWorkRepositoryStrategy<TItem>{

    public UnitOfWorkRepository(TStrategy strategy)
        : base(strategy) {
    }
    protected UnitOfWorkRepository(IStrategyFactory factory)
        : base(factory) {
    }
    protected UnitOfWorkRepository(IEnumerable<TItem> data, TStrategy strategy)
        : base(data, strategy) {
    }
    protected UnitOfWorkRepository(IEnumerable<TItem> data, IStrategyFactory factory)
        : base(data, factory) {
    }

    public virtual Task<int> SaveChanges(CancellationToken ct = default)
        => Strategy.SaveChanges(ct);
}
