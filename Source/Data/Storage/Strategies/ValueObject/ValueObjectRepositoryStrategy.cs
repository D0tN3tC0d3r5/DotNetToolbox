namespace DotNetToolbox.Data.Strategies.ValueObject;

public abstract class ValueObjectRepositoryStrategy
    : IValueObjectRepositoryStrategy {
    public abstract Type ElementType { get; }
    public abstract Expression Expression { get; }
    public abstract IQueryProvider Provider { get; }

    public abstract IEnumerator GetEnumerator();
}

public abstract class ValueObjectRepositoryStrategy<TItem>
    : ValueObjectRepositoryStrategy,
    IValueObjectRepositoryStrategy<TItem> {

    public override Type ElementType => Query.ElementType;
    public override Expression Expression => Query.Expression;
    protected List<TItem> OriginalData { get; private set; } = [];

    #region Blocking

    protected IQueryable<TItem> Query => OriginalData.AsQueryable();
    public override IQueryProvider Provider => Query.Provider;

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    public override IEnumerator<TItem> GetEnumerator() => Query.GetEnumerator();

    public virtual void Seed(IEnumerable<TItem> seed)
        => OriginalData = seed as List<TItem> ?? IsNotNull(seed).ToList();

    public void Create(Action<TItem> setNewItem) => throw new NotImplementedException();

    public virtual void Add(TItem newItem) => throw new NotImplementedException();
    public virtual void Patch(Expression<Func<TItem, bool>> predicate, Action<TItem> setItem) => throw new NotImplementedException();
    public virtual void Update(TItem updatedItem) => throw new NotImplementedException();
    public virtual void Update(Expression<Func<TItem, bool>> predicate, TItem updatedItem) => throw new NotImplementedException();
    public virtual void Remove(Expression<Func<TItem, bool>> predicate) => throw new NotImplementedException();

    #endregion

    #region Async

    protected IAsyncQueryable<TItem> AsyncQuery => OriginalData.AsAsyncQueryable();
    public IAsyncQueryProvider AsyncProvider => AsyncQuery.AsyncProvider;

    public IAsyncEnumerator<TItem> GetAsyncEnumerator(CancellationToken ct = default) => AsyncQuery.GetAsyncEnumerator(ct);

    public virtual Task SeedAsync(IEnumerable<TItem> seed, CancellationToken ct = default) {
        Seed(seed);
        return Task.CompletedTask;
    }

    public virtual Task CreateAsync(Func<TItem, CancellationToken, Task> setNewItem, CancellationToken ct = default) => throw new NotImplementedException();

    public virtual Task AddAsync(TItem newItem, CancellationToken ct = default) => throw new NotImplementedException();
    public virtual Task PatchAsync(Expression<Func<TItem, bool>> predicate, Func<TItem, CancellationToken, Task> setItem, CancellationToken ct = default) => throw new NotImplementedException();
    public virtual Task UpdateAsync(TItem updatedItem, CancellationToken ct = default) => throw new NotImplementedException();
    public virtual Task UpdateAsync(Expression<Func<TItem, bool>> predicate, TItem updatedItem, CancellationToken ct = default) => throw new NotImplementedException();
    public virtual Task RemoveAsync(Expression<Func<TItem, bool>> predicate, CancellationToken ct = default) => throw new NotImplementedException();

    #endregion
}
