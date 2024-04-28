// ReSharper disable once CheckNamespace - Intended to be in this namespace
namespace System.Linq.Async;

public class AsyncEnumerableQuery;

public class AsyncEnumerableQuery<TItem>
    : AsyncEnumerableQuery
    , IAsyncQueryable<TItem>
    , IAsyncQueryProvider {
    private readonly Expression _expression;
    private readonly IEnumerable<TItem> _source;
    private readonly CancellationToken _ct;

    public AsyncEnumerableQuery(IEnumerable<TItem> source, Expression? expression = null, CancellationToken ct = default) {
        _source = source;
        _expression = expression ?? Expression.Constant(this);
        _ct = ct;
    }

    public AsyncEnumerableQuery(Expression expression, CancellationToken ct = default)
        : this([], expression, ct) {
    }

    IEnumerator IEnumerable.GetEnumerator()
        => _source.GetEnumerator();
    IEnumerator<TItem> IEnumerable<TItem>.GetEnumerator()
        => _source.GetEnumerator();
    IAsyncEnumerator<TItem> IAsyncEnumerable<TItem>.GetAsyncEnumerator(CancellationToken ct) {
        var cts = CancellationTokenSource.CreateLinkedTokenSource(ct, _ct);
        return new AsyncEnumerator<TItem>(_source.GetEnumerator(), cts.Token);
    }

    Expression IQueryable.Expression => _expression;
    IQueryProvider IQueryable.Provider => this;
    IAsyncQueryProvider IAsyncQueryable<TItem>.AsyncProvider => this;
    Type IQueryable.ElementType => typeof(TItem);

    IQueryable IQueryProvider.CreateQuery(Expression expression)
        => new EnumerableQuery<TItem>(expression);
    IQueryable<TElement> IQueryProvider.CreateQuery<TElement>(Expression expression)
        => new EnumerableQuery<TElement>(expression);
    IAsyncQueryable<TElement> IAsyncQueryProvider.CreateAsyncQuery<TElement>(Expression expression)
        => new AsyncEnumerableQuery<TElement>(expression);

    object IQueryProvider.Execute(Expression expression)
        => throw new NotImplementedException();
    TResult IQueryProvider.Execute<TResult>(Expression expression)
        => throw new NotImplementedException();
    Task<TResult> IAsyncQueryProvider.ExecuteAsync<TResult>(Expression expression, CancellationToken ct)
        => throw new NotImplementedException();
}
