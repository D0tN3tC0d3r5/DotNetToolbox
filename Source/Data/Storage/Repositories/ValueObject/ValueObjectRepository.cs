namespace DotNetToolbox.Data.Repositories.ValueObject;

public class ValueObjectRepository<TItem>
    : ValueObjectRepository<IValueObjectRepositoryStrategy<TItem>, TItem> {
    public ValueObjectRepository(IEnumerable<TItem>? data = null)
        : base(new InMemoryValueObjectRepositoryStrategy<TItem>(), data) { }

    public ValueObjectRepository(IRepositoryStrategyProvider provider, IEnumerable<TItem>? data = null)
        : base((IValueObjectRepositoryStrategy<TItem>)IsNotNull(provider).GetStrategy<TItem>(), data) { }

    public ValueObjectRepository(IValueObjectRepositoryStrategy<TItem> strategy, IEnumerable<TItem>? data = null)
        : base(strategy, data ?? []) { }
}

public abstract class ValueObjectRepository<TStrategy, TItem>
    : IValueObjectRepository<TItem>
    where TStrategy : class, IValueObjectRepositoryStrategy<TItem> {
    protected ValueObjectRepository(TStrategy strategy, IEnumerable<TItem>? data = null) {
        Strategy = IsNotNull(strategy);
        if (data is null)
            return;
        var list = data as List<TItem> ?? data.ToList();
        Strategy.Seed(list);
    }

    protected TStrategy Strategy { get; init; }
    public Type ElementType => Strategy.ElementType;
    public Expression Expression => Strategy.Expression;

    #region Blocking

    IEnumerator IEnumerable.GetEnumerator() => Strategy.GetEnumerator();
    public IEnumerator<TItem> GetEnumerator() => Strategy.GetEnumerator();
    public IQueryProvider Provider => Strategy.Provider;

    public void Seed(IEnumerable<TItem> seed)
        => Strategy.Seed(seed);

    public TItem Find(Expression<Func<TItem, bool>> predicate) => throw new NotImplementedException();

    public void Create(Action<TItem> setNewItem) => throw new NotImplementedException();

    public void Add(TItem newItem) {
        Debug.Assert(true);
        Strategy.Add(newItem);
        Debug.Assert(true);
    }

    public void Patch(Expression<Func<TItem, bool>> predicate, Action<TItem> setItem) => throw new NotImplementedException();
    public void Update(Expression<Func<TItem, bool>> predicate, TItem updatedItem)
        => Strategy.Update(predicate, updatedItem);
    public void Remove(Expression<Func<TItem, bool>> predicate)
        => Strategy.Remove(predicate);

    #endregion

    #region Async

    public IAsyncEnumerator<TItem> GetAsyncEnumerator(CancellationToken ct = default)
        => Strategy.GetAsyncEnumerator(ct);

    public IAsyncQueryProvider AsyncProvider => Strategy.AsyncProvider;

    public Task SeedAsync(IEnumerable<TItem> seed, CancellationToken ct = default)
        => Strategy.SeedAsync(seed, ct);

    public Task<TItem?> FindAsync(Expression<Func<TItem, bool>> predicate, CancellationToken ct = default) => throw new NotImplementedException();

    public Task CreateAsync(Func<TItem, CancellationToken, Task> setNewItem, CancellationToken ct = default) => throw new NotImplementedException();

    public Task AddAsync(TItem newItem, CancellationToken ct = default)
        => Strategy.AddAsync(newItem, ct);

    public Task PatchAsync(Expression<Func<TItem, bool>> predicate, Func<TItem, CancellationToken, Task> setItem, CancellationToken ct = default) => throw new NotImplementedException();

    public Task UpdateAsync(Expression<Func<TItem, bool>> predicate, TItem updatedItem, CancellationToken ct = default)
        => Strategy.UpdateAsync(predicate, updatedItem, ct);

    public Task RemoveAsync(Expression<Func<TItem, bool>> predicate, CancellationToken ct = default)
        => Strategy.RemoveAsync(predicate, ct);

    #endregion
}
