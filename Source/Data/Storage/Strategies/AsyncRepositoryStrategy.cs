namespace DotNetToolbox.Data.Strategies;

public abstract class AsyncRepositoryStrategy<TItem>
    : RepositoryStrategy<TItem>
    , IAsyncRepositoryStrategy<TItem> {

    protected AsyncRepositoryStrategy()
        : this([]) {
    }

    protected AsyncRepositoryStrategy(IEnumerable<TItem> data) {
        OriginalData = data.ToList();
        Query = OriginalData.AsQueryable();
        AsyncQuery = OriginalData.AsAsyncQueryable();
    }

    //IAsyncEnumerator IAsyncEnumerable.GetAsyncEnumerator(CancellationToken ct)
    //    => GetAsyncEnumerator(ct);
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Naming", "CA1725:Parameter names should match base declaration", Justification = "<Pending>")]
    public IAsyncEnumerator<TItem> GetAsyncEnumerator(CancellationToken ct = default)
        => AsyncQuery.GetAsyncEnumerator(ct);

    public IAsyncQueryProvider AsyncProvider => AsyncQuery.AsyncProvider;

    protected IAsyncQueryable<TItem> AsyncQuery { get; set; }

    public virtual Task SeedAsync(IAsyncEnumerable<TItem> seed, CancellationToken ct = default)
        => throw new NotImplementedException();

    public virtual Task AddAsync(TItem newItem, CancellationToken ct = default)
        => throw new NotImplementedException();

    public virtual Task UpdateAsync(Expression<Func<TItem, bool>> predicate, TItem updatedItem, CancellationToken ct = default)
        => throw new NotImplementedException();

    public virtual Task RemoveAsync(Expression<Func<TItem, bool>> predicate, CancellationToken ct = default)
        => throw new NotImplementedException();
}
