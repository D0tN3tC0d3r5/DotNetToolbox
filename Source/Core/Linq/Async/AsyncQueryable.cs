namespace System.Linq.Async;

[Diagnostics.CodeAnalysis.SuppressMessage("Design", "CA1010:Generic interface should also be implemented", Justification = "Implemented below")]
[Diagnostics.CodeAnalysis.SuppressMessage("CodeQuality", "IDE0079:Remove unnecessary suppression", Justification = "<Pending>")]
public class AsyncQueryable
    : IAsyncQueryable {
    private readonly IAsyncQueryProvider _provider;
    protected IQueryable Source { get; }

    protected AsyncQueryable(IQueryable source) {
        Source = source;
        _provider = new AsyncQueryableProvider(Source);
    }

    public AsyncQueryable(IEnumerable source, Expression? expression = null)
        : this(expression == null
                   ? source.AsQueryable()
                   : source.AsQueryable()
                           .Provider.CreateQuery(expression)) { }

    Expression IQueryable.Expression => Source.Expression;
    Type IQueryable.ElementType => Source.ElementType;
    IQueryProvider IQueryable.Provider => Source.Provider;
    IAsyncQueryProvider IAsyncQueryable.AsyncProvider => _provider;

    IEnumerator IEnumerable.GetEnumerator() => Source.GetEnumerator();
}

public class AsyncQueryable<TItem>
    : AsyncQueryable,
    IAsyncQueryable<TItem> {
    protected AsyncQueryable(IQueryable source)
        : base(source) {
    }

    public AsyncQueryable(IEnumerable<TItem> source, Expression? expression = null)
        : base(source, expression) {
    }

    IEnumerator<TItem> IEnumerable<TItem>.GetEnumerator() => ((IQueryable<TItem>)Source).GetEnumerator();
    IAsyncEnumerator<TItem> IAsyncEnumerable<TItem>.GetAsyncEnumerator(CancellationToken ct)
        => new AsyncEnumerator<TItem>(((IQueryable<TItem>)Source).GetEnumerator(), ct);
}
