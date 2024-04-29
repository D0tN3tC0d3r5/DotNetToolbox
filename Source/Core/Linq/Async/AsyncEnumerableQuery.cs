// ReSharper disable once CheckNamespace - Intended to be in this namespace

namespace System.Linq.Async;

public class AsyncEnumerableQuery;

public class AsyncEnumerableQuery<TItem>
    : AsyncEnumerableQuery
    , IAsyncQueryable<TItem>
    , IAsyncQueryProvider {
    private readonly Expression _expression;
    private readonly IQueryable<TItem> _source;

    public AsyncEnumerableQuery(IEnumerable<TItem> source, Expression? expression = null) {
        _source = source.AsQueryable();
        _expression = expression ?? Expression.Constant(_source);
    }

    public AsyncEnumerableQuery(Expression expression)
        : this([], expression) {
    }

    IEnumerator IEnumerable.GetEnumerator()
        => _source.GetEnumerator();
    IEnumerator<TItem> IEnumerable<TItem>.GetEnumerator()
        => _source.GetEnumerator();
    IAsyncEnumerator<TItem> IAsyncEnumerable<TItem>.GetAsyncEnumerator(CancellationToken ct)
        => new AsyncEnumerator<TItem>(_source.GetEnumerator(), ct);

    Expression IQueryable.Expression => _expression;
    IQueryProvider IQueryable.Provider => _source.Provider;
    IAsyncQueryProvider IAsyncQueryable<TItem>.AsyncProvider => this;
    Type IQueryable.ElementType => typeof(TItem);

    IAsyncQueryable<TElement> IAsyncQueryProvider.CreateAsyncQuery<TElement>(Expression expression)
        => new AsyncEnumerableQuery<TElement>(expression);

    Task<TResult> IAsyncQueryProvider.ExecuteAsync<TResult>(Expression expression, CancellationToken ct)
        => throw new NotImplementedException();
}
