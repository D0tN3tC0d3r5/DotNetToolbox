namespace System.Linq.Async;

[Diagnostics.CodeAnalysis.SuppressMessage("Design", "CA1010:Generic interface should also be implemented", Justification = "Implemented below")]
public interface IAsyncQueryable
    : IQueryable {
    IAsyncQueryProvider AsyncProvider { get; }
}

public interface IAsyncQueryable<out TItem>
    : IAsyncEnumerable<TItem>
    , IAsyncQueryable
    , IQueryable<TItem>;
