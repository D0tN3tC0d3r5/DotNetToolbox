using System.Collections.Async;

namespace DotNetToolbox.Linq.Async;

public class AsyncEnumerableQuery;

public class AsyncEnumerableQuery<TItem>
    : AsyncEnumerableQuery
    , IAsyncQueryable<TItem>
    , IAsyncQueryProvider {
    private readonly Expression _expression;
    private readonly IAsyncQueryable<TItem>? _query;

    IAsyncQueryProvider IAsyncQueryable.Provider => this;
    Type IAsyncQueryable.ElementType => typeof(TItem);

    public AsyncEnumerableQuery(IEnumerable<TItem> enumerable) {
        _query = enumerable.AsAsyncQueryable();
        _expression = Expression.Constant(this);
    }

    public AsyncEnumerableQuery(Expression expression) {
        _expression = expression;
    }

    System.Collections.Async.Generic.IAsyncEnumerator<TItem> System.Collections.Async.Generic.IAsyncEnumerable<TItem>.GetAsyncEnumerator(CancellationToken ct)
        => throw new NotImplementedException();
    IAsyncEnumerator IAsyncEnumerable.GetAsyncEnumerator(CancellationToken ct)
        => throw new NotImplementedException();

    Expression IAsyncQueryable.Expression => _expression;

    IQueryable IAsyncQueryProvider.CreateQuery(Expression expression)
        => throw new NotImplementedException();
    IQueryable<TElement> IAsyncQueryProvider.CreateQuery<TElement>(Expression expression)
        => throw new NotImplementedException();

    Task<object?> IAsyncQueryProvider.ExecuteAsync(Expression expression)
        => throw new NotImplementedException();
    Task<TResult> IAsyncQueryProvider.ExecuteAsync<TResult>(Expression expression)
        => throw new NotImplementedException();
}
