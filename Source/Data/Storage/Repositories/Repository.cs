namespace DotNetToolbox.Data.Repositories;

public class Repository<TItem>
    : Repository<IRepositoryStrategy<TItem>, TItem> {
    public Repository(IEnumerable<TItem>? data = null)
        : base(DefaultName, new InMemoryRepositoryStrategy<TItem>(), data) {
    }
    public Repository(IRepositoryStrategy<TItem> strategy, IEnumerable<TItem>? data = null)
        : base(strategy, data) {
    }
    public Repository(string name, IEnumerable<TItem>? data = null)
        : base(name, new InMemoryRepositoryStrategy<TItem>(), data) {
    }
    public Repository(string name, IRepositoryStrategy<TItem> strategy, IEnumerable<TItem>? data = null)
        : base(name, strategy, data) {
    }
}

public class Repository<TStrategy, TItem>
    : IRepository<TItem>
    where TStrategy : class, IRepositoryStrategy<TItem> {
    protected static string DefaultName => $"|>Repository[{typeof(TItem).Name}]_{Guid.NewGuid():N}<|";

    public Repository(TStrategy strategy, IEnumerable<TItem>? data = null)
        : this(DefaultName, strategy, data) {
    }
    public Repository(string name, TStrategy strategy, IEnumerable<TItem>? data = null) {
        Name = IsNotNull(name);
        Strategy = IsNotNull(strategy);
        Strategy.SetRepository(this);
        // ReSharper disable PossibleMultipleEnumeration
        if (data?.Any() ?? false) Strategy.Seed(data.ToList());
        // ReSharper enable PossibleMultipleEnumeration
    }

    public string Name { get; }

    protected TStrategy Strategy { get; init; }
    internal List<TItem> Data { get; set; } = [];
    public Type ElementType => Query.ElementType;
    public Expression Expression => Query.Expression;

    #region Blocking

    internal IQueryable<TItem> Query => Data.AsQueryable();
    public IQueryProvider Provider => Query.Provider;
    IEnumerator IEnumerable.GetEnumerator() => Query.GetEnumerator();
    public IEnumerator<TItem> GetEnumerator() => Query.GetEnumerator();

    public void Load()
        => Strategy.Load();
    public void Seed(IEnumerable<TItem> seed)
        => Strategy.Seed(seed);

    public TItem[] GetAll()
        => Strategy.GetAll();

    public IPagedQueryableRepository<TItem>? AsPaged() => this as IPagedQueryableRepository<TItem>;
    public IChunkedQueryableRepository<TItem>? AsChunked() => this as IChunkedQueryableRepository<TItem>;

    public TItem? Find(Expression<Func<TItem, bool>> predicate)
        => Strategy.Find(predicate);

    public TItem Create(Action<TItem> setItem)
        => Strategy.Create(setItem);
    public void Add(TItem newItem)
        => Strategy.Add(newItem);

    public void Update(Expression<Func<TItem, bool>> predicate, TItem updatedItem)
        => Strategy.Update(predicate, updatedItem);
    public void Patch(Expression<Func<TItem, bool>> predicate, Action<TItem> setItem)
        => Strategy.Patch(predicate, setItem);

    public void Remove(Expression<Func<TItem, bool>> predicate)
        => Strategy.Remove(predicate);

    #endregion

    #region Async

    internal IAsyncQueryable<TItem> AsyncQuery => Data.AsAsyncQueryable();
    public IAsyncQueryProvider AsyncProvider => AsyncQuery.AsyncProvider;
    public IAsyncEnumerator<TItem> GetAsyncEnumerator(CancellationToken ct = default)
        => AsyncQuery.GetAsyncEnumerator(ct);

    public Task SeedAsync(IEnumerable<TItem> seed, CancellationToken ct = default)
        => Strategy.SeedAsync(seed, ct);
    public Task LoadAsync(CancellationToken ct = default)
        => Strategy.LoadAsync(ct);

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
