// ReSharper disable once CheckNamespace - Intended to be in this namespace
namespace System.Linq.Async;

public interface IAsyncOrderedQueryable<out TItem>
    : IAsyncQueryable<TItem>
    , IOrderedQueryable<TItem>;
