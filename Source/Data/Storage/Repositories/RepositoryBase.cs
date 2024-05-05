namespace DotNetToolbox.Data.Repositories;

public class RepositoryBase<TStrategy, TItem, TKey>
    : RepositoryBase<TStrategy, TItem>
    , IRepository<TItem, TKey>
    where TStrategy : class, IRepositoryStrategy<TItem, TKey>, new()
    where TItem : IEntity<TKey>
    where TKey : notnull {

    public RepositoryBase(IEnumerable<TItem>? data = null)
        : base(data) {
    }
    public RepositoryBase(string name, IEnumerable<TItem>? data = null)
        : base(name, data) {
    }

    public void SetKeyHandler(IKeyHandler<TKey> keyHandler)
        => Strategy.SetKeyHandler(keyHandler);

#region Blocking

    public TItem? FindByKey(TKey key)
        => Strategy.FindByKey(key);
    public void Update(TItem updatedItem)
        => Strategy.Update(updatedItem);
    public void Patch(TKey key, Action<TItem> setItem)
        => Strategy.Patch(key, setItem);
    public void Remove(TKey key)
        => Strategy.Remove(key);

    #endregion

    #region Async

    public ValueTask<TItem?> FindByKeyAsync(TKey key, CancellationToken ct = default)
        => Strategy.FindByKeyAsync(key, ct);
    public Task UpdateAsync(TItem updatedItem, CancellationToken ct = default)
        => Strategy.UpdateAsync(updatedItem, ct);
    public Task PatchAsync(TKey key, Func<TItem, CancellationToken, Task> setItem, CancellationToken ct = default)
        => Strategy.PatchAsync(key, setItem, ct);
    public Task RemoveAsync(TKey key, CancellationToken ct = default)
        => Strategy.RemoveAsync(key, ct);

    #endregion
}

public class RepositoryBase<TStrategy, TItem>
    : RepositoryBase<TItem>
    , IRepository<TItem>
    where TStrategy : class, IRepositoryStrategy<TItem>, new() {

    public RepositoryBase(IEnumerable<TItem>? data = null)
        : this(DefaultName, data) {
    }
    public RepositoryBase(string name, IEnumerable<TItem>? data = null)
        : base(name, data){
        Strategy = new();
        Strategy.SetRepository(this);
    }

    protected TStrategy Strategy { get; }

    #region Blocking

    public void Load()
        => Strategy.Load();
    public void Seed(IEnumerable<TItem> seed)
        => Strategy.Seed(seed);

    public TItem[] GetAll()
        => Strategy.GetAll();

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

public abstract class RepositoryBase<TItem>
    : IRepositoryBase<TItem> {
    protected static string DefaultName => $"|>Repository[{typeof(TItem).Name}]_{Guid.NewGuid():N}<|";

    protected RepositoryBase(IEnumerable<TItem>? data = null)
        : this(DefaultName, data) {
    }
    protected RepositoryBase(string name, IEnumerable<TItem>? data = null) {
        Name = IsNotNull(name);
        // ReSharper disable PossibleMultipleEnumeration
        if (data?.Any() ?? false) Data = data.ToList();
        // ReSharper enable PossibleMultipleEnumeration
    }

    public string Name { get; }

    internal List<TItem> Data { get; set; } = [];
    public Type ElementType => Query.ElementType;
    public Expression Expression => Query.Expression;

    #region Blocking

    internal IQueryable<TItem> Query => Data.AsQueryable();
    public IQueryProvider Provider => Query.Provider;
    IEnumerator IEnumerable.GetEnumerator() => Query.GetEnumerator();
    public IEnumerator<TItem> GetEnumerator() => Query.GetEnumerator();

    public IPagedRepository<TItem>? AsPaged() => this as IPagedRepository<TItem>;
    public IChunkedRepository<TItem>? AsChunked() => this as IChunkedRepository<TItem>;

    #endregion

    #region Async

    internal IAsyncQueryable<TItem> AsyncQuery => Data.AsAsyncQueryable();
    public IAsyncQueryProvider AsyncProvider => AsyncQuery.AsyncProvider;
    public IAsyncEnumerator<TItem> GetAsyncEnumerator(CancellationToken ct = default)
        => AsyncQuery.GetAsyncEnumerator(ct);

    #endregion
}
