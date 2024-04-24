namespace DotNetToolbox.Data.Repositories;

public abstract class Repository<TStrategy, TItem>
    : Repository<Repository<TStrategy, TItem>, TStrategy, TItem>
    where TStrategy : class, IRepositoryStrategy<TItem> {
    protected Repository(IStrategyFactory factory)
        : base(factory) {
    }
    protected Repository(TStrategy strategy)
        : base(strategy) {
    }
    protected Repository(IEnumerable<TItem> data, IStrategyFactory factory)
        : base(data, factory) {
    }
    protected Repository(IEnumerable<TItem> data, TStrategy strategy)
        : base(data, strategy) {
    }
}

public abstract class Repository<TRepository, TStrategy, TItem> : IRepository<TItem>, IEnumerable<TItem>
    where TRepository : Repository<TRepository, TStrategy, TItem>
    where TStrategy : class, IRepositoryStrategy<TItem> {

    protected Repository(TStrategy strategy)
        : this([], strategy) {
    }
    protected Repository(IStrategyFactory factory)
        : this([], factory) {
    }
    protected Repository(IEnumerable<TItem> data, IStrategyFactory factory)
        : this(data, IsNotNull(factory).GetRequiredStrategy<TItem, TStrategy>()) {
    }
    protected Repository(IEnumerable<TItem> data, TStrategy strategy) {
        Strategy = IsNotNull(strategy);
        Strategy.Seed(data.ToList());
    }

    protected TStrategy Strategy { get; init; }

    public Type ElementType => Strategy.ElementType;
    public Expression Expression => Strategy.Expression;
    public IQueryProvider Provider => Strategy.Provider;

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    public IEnumerator<TItem> GetEnumerator() => Strategy.GetEnumerator();

    public void Seed(IEnumerable<TItem> seed)
        => Strategy.Seed(seed);

    public void Add(TItem newItem)
        => Strategy.Add(newItem);
    public void Update(Expression<Func<TItem, bool>> predicate, TItem updatedItem)
        => Strategy.Update(predicate, updatedItem);
    public void Remove(Expression<Func<TItem, bool>> predicate)
        => Strategy.Remove(predicate);
}
