namespace System.Linq.Async;

public abstract class AsyncQueryable {
    public static EmptyAsyncQueryable<TItem> Empty<TItem>() => new();
}

public class AsyncQueryable<TItem>
    : AsyncQueryable
    , IAsyncQueryable<TItem>
    , IAsyncQueryProvider {
    private readonly Expression _expression;
    private readonly IQueryable<TItem> _source;

    public AsyncQueryable(IEnumerable<TItem> source, Expression? expression = null) {
        _source = source.AsQueryable();
        _expression = expression ?? Expression.Constant(_source);
    }

    IEnumerator IEnumerable.GetEnumerator()
        => _source.GetEnumerator();
    IEnumerator<TItem> IEnumerable<TItem>.GetEnumerator()
        => _source.GetEnumerator();
    IAsyncEnumerator<TItem> IAsyncEnumerable<TItem>.GetAsyncEnumerator(CancellationToken ct)
        => new AsyncEnumerator<TItem>(_source.GetEnumerator(), ct);

    Expression IQueryable.Expression => _expression;
    IQueryProvider IQueryable.Provider => _source.Provider;
    IAsyncQueryProvider IAsyncQueryable.AsyncProvider => this;
    Type IQueryable.ElementType => typeof(TItem);

    IAsyncQueryable<TElement> IAsyncQueryProvider.CreateAsyncQuery<TElement>(Expression expression)
        => new AsyncQueryable<TElement>([], expression);

    Task<TResult> IAsyncQueryProvider.ExecuteAsync<TResult>(Expression expression, CancellationToken ct)
        => throw new NotImplementedException();
}
