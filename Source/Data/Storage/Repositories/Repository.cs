namespace DotNetToolbox.Data.Repositories;

public class Repository<TItem>(IRepositoryStrategy<TItem> strategy, IEnumerable<TItem>? data = null)
    : Repository<IRepositoryStrategy<TItem>, TItem>(IsNotNull(strategy), data ?? []) {
    public Repository(IEnumerable<TItem>? data = null)
        : this(new InMemoryRepositoryStrategy<TItem>(), data ?? []) { }
    public Repository(IRepositoryStrategyProvider provider, IEnumerable<TItem>? data = null)
        : this(IsNotNull(provider).GetRequiredStrategyFor<TItem>(), data) { }
}

public abstract class Repository<TStrategy, TItem>
    : IRepository<TItem>
    where TStrategy : class, IRepositoryStrategy<TItem> {
    protected Repository(TStrategy strategy, IEnumerable<TItem> data) {
        Strategy = IsNotNull(strategy);
        var list = DefaultIfNotOfType(data, data.ToList());
        if (list.Count == 0)
            return;
        Strategy.Seed(list);
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
