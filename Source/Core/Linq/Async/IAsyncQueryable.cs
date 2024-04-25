namespace DotNetToolbox.Linq.Async;

//[SuppressMessage("Design", "CA1010:Generic interface should also be implemented", Justification = "Does not apply.")]
//public interface IAsyncQueryable
//    : IAsyncEnumerable
//    , IQueryable {
//    IAsyncQueryProvider AsyncProvider { get; }
//}

public interface IAsyncQueryable<out TItem>
    : IAsyncEnumerable<TItem>
    , IQueryable<TItem> {
    IAsyncQueryProvider AsyncProvider { get; }
}
