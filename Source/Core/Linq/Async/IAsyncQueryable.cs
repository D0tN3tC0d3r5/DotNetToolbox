// ReSharper disable once CheckNamespace - Intended to be in this namespace
namespace System.Linq.Async;

public interface IAsyncQueryable<out TItem>
    : IAsyncEnumerable<TItem>
    , IQueryable<TItem> {
    IAsyncQueryProvider AsyncProvider { get; }
}
