namespace DotNetToolbox.Linq.Async;

public interface IOrderedAsyncQueryable
    : IAsyncQueryable;

public interface IOrderedAsyncQueryable<out TItem>
    : IAsyncQueryable<TItem>
    , IOrderedAsyncQueryable {
}
