// ReSharper disable once CheckNamespace - Intended to be in this namespace
namespace System.Linq;

public static class EnumerableExtensions {
    public static IAsyncQueryable<TItem> AsAsyncQueryable<TItem>(this IEnumerable<TItem> source)
        => source as IAsyncQueryable<TItem> ?? new AsyncQueryable<TItem>(source);
}
