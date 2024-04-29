// ReSharper disable once CheckNamespace - Intended to be in this namespace
namespace System.Linq.Async;

public static partial class AsyncQueryableExtensions {
    public static ValueTask<HashSet<TItem>> ToHashSetAsync<TItem>(this IAsyncQueryable<TItem> source, CancellationToken ct = default)
        => source.ToHashSetAsync((x, _) => x, EqualityComparer<TItem>.Default, ct);

    public static ValueTask<HashSet<TItem>> ToHashSetAsync<TItem>(this IAsyncQueryable<TItem> source, IEqualityComparer<TItem> comparer, CancellationToken ct = default)
        => source.ToHashSetAsync((x, _) => x, comparer, ct);

    public static ValueTask<HashSet<TResult>> ToHashSetAsync<TItem, TResult>(this IAsyncQueryable<TItem> source, Func<TItem, TResult> mapping, CancellationToken ct = default)
        => source.ToHashSetAsync((x, _) => mapping(x), EqualityComparer<TResult>.Default, ct);

    public static ValueTask<HashSet<TResult>> ToHashSetAsync<TItem, TResult>(this IAsyncQueryable<TItem> source, Func<TItem, TResult> mapping, IEqualityComparer<TResult> comparer, CancellationToken ct = default)
        => source.ToHashSetAsync((x, _) => mapping(x), comparer, ct);

    public static ValueTask<HashSet<TResult>> ToHashSetAsync<TItem, TResult>(this IAsyncQueryable<TItem> source, Func<TItem, int, TResult> mapping, CancellationToken ct = default)
        => source.ToHashSetAsync(mapping, EqualityComparer<TResult>.Default, ct);

    public static async ValueTask<HashSet<TResult>> ToHashSetAsync<TItem, TResult>(this IAsyncQueryable<TItem> source, Func<TItem, int, TResult> mapping, IEqualityComparer<TResult> comparer, CancellationToken ct = default) {
        var result = new HashSet<TResult>(IsNotNull(comparer));
        var index = 0;
        await foreach (var item in IsNotNull(source).WithCancellation(ct).ConfigureAwait(false))
            result.Add(mapping(item, index++));
        return result;
    }
}
