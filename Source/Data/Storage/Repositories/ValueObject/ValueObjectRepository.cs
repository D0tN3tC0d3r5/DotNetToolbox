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
    : Repository<TStrategy, TItem>
    , IValueObjectRepository<TItem>
    where TStrategy : class, IValueObjectRepositoryStrategy<TItem> {
    protected ValueObjectRepository(TStrategy strategy, IEnumerable<TItem>? data = null)
        : base(strategy, data) {
    }

    #region Blocking

    public TItem[] GetAll()
        => Strategy.GetAll();
    public TItem? Find(Expression<Func<TItem, bool>> predicate)
        => Strategy.Find(predicate);

    public TItem Create(Action<TItem> setItem)
        => Strategy.Create(setItem);
    public void Add(TItem newItem)
        => Strategy.Add(newItem);
    public void Patch(Expression<Func<TItem, bool>> predicate, Action<TItem> setItem)
        => Strategy.Patch(predicate, setItem);
    public void Update(Expression<Func<TItem, bool>> predicate, TItem updatedItem)
        => Strategy.Update(predicate, updatedItem);
    public void Remove(Expression<Func<TItem, bool>> predicate)
        => Strategy.Remove(predicate);

    #endregion

    #region Async

    public ValueTask<TItem[]> GetAllAsync(CancellationToken ct = default)
        => Strategy.GetAllAsync(ct);
    public ValueTask<TItem?> FindAsync(Expression<Func<TItem, bool>> predicate, CancellationToken ct = default)
        => Strategy.FindAsync(predicate, ct);

    public Task<TItem> CreateAsync(Func<TItem, CancellationToken, Task> setItem, CancellationToken ct = default)
        => Strategy.CreateAsync(setItem, ct);
    public Task AddAsync(TItem newItem, CancellationToken ct = default)
        => Strategy.AddAsync(newItem, ct);
    public Task PatchAsync(Expression<Func<TItem, bool>> predicate, Func<TItem, CancellationToken, Task> setItem, CancellationToken ct = default)
        => Strategy.PatchAsync(predicate, setItem, ct);
    public Task UpdateAsync(Expression<Func<TItem, bool>> predicate, TItem updatedItem, CancellationToken ct = default)
        => Strategy.UpdateAsync(predicate, updatedItem, ct);
    public Task RemoveAsync(Expression<Func<TItem, bool>> predicate, CancellationToken ct = default)
        => Strategy.RemoveAsync(predicate, ct);

    #endregion
}
