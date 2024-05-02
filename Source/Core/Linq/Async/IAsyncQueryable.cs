// ReSharper disable once CheckNamespace - Intended to be in this namespace
namespace System.Linq.Async;

public interface IAsyncQueryable
    : IQueryable {
    IAsyncQueryProvider AsyncProvider { get; }
}

public interface IAsyncQueryable<out TItem>
    : IAsyncEnumerable<TItem>
    , IAsyncQueryable
    , IQueryable<TItem>;
