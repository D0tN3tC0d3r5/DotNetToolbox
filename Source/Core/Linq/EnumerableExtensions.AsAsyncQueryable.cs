// ReSharper disable once CheckNamespace - Intended to be in this namespace
namespace System.Linq;

public static partial class EnumerableExtensions {
    public static IAsyncQueryable<TItem> ToAsyncQueryable<TItem>(this IEnumerable<TItem> source)
        => new AsyncEnumerableQuery<TItem>(source);
}
