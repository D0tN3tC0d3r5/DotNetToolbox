namespace Sophia.Data;

public class ReadOnlyRepository<[DynamicallyAccessedMembers(IEntity.AccessedMembers)] TModel, TKey>
    : IReadOnlyRepository<TModel, TKey>
    where TModel : class, IEntity<TKey>, new()
    where TKey : notnull {

    private async IAsyncEnumerable<TModel> ToAsyncEnumerable() {
        foreach (var item in Local) {
            yield return item;
            await Task.Yield();
        }
    }

    protected List<TModel> Local { get; } = [];
    public IQueryable<TModel> AsQueryable() => Local.AsQueryable();

    public IAsyncEnumerator<TModel> GetAsyncEnumerator(CancellationToken cancellationToken = default)
        => ToAsyncEnumerable().GetAsyncEnumerator(cancellationToken);
    public IEnumerator<TModel> GetEnumerator() => Local.GetEnumerator();
    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    public Type ElementType { get; } = typeof(TModel);
    public Expression Expression => AsQueryable().Expression;
    public IQueryProvider Provider => AsQueryable().Provider;

    public virtual Task<IReadOnlyList<TModel>> ToArrayAsync(CancellationToken ct = default)
        => throw new NotImplementedException();
    public Task<bool> HaveAny(CancellationToken ct = default)
        => ExecuteHaveAny(default, ct);
    public Task<bool> HaveAny(Expression<Func<TModel, bool>> predicate, CancellationToken ct = default)
        => ExecuteHaveAny(predicate, ct);
    public Task<TModel?> FindByKey(TKey key, CancellationToken ct = default)
        => ExecuteFindFirst(i => i.Id.Equals(key), ct);
    public Task<TModel?> FindFirst(CancellationToken ct = default)
        => ExecuteFindFirst(default, ct);
    public Task<TModel?> FindFirst(Expression<Func<TModel, bool>> predicate, CancellationToken ct = default)
        => ExecuteFindFirst(predicate, ct);

    protected virtual Task<bool> ExecuteHaveAny(Expression<Func<TModel, bool>>? predicate, CancellationToken ct = default)
        => throw new NotImplementedException();
    protected virtual Task<TModel?> ExecuteFindFirst(Expression<Func<TModel, bool>>? predicate, CancellationToken ct = default)
        => throw new NotImplementedException();
}
