namespace DotNetToolbox.Data.Repositories;

public class QueryableRepository<TRepository, TModel>
    : IQueryableRepository<TRepository, TModel>
    where TRepository : QueryableRepository<TRepository, TModel>
    where TModel : class, IEntity, new() {

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
}
