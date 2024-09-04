// ReSharper disable once CheckNamespace - Intended to be in this namespace
namespace System.Linq.Async;

public static partial class QueryableAsyncExtensions {
    public static ValueTask<HashSet<TItem>> ToHashSetAsync<TItem>(this IQueryable<TItem> source, CancellationToken ct = default)
        => IsNotNull(source).MakeHashSetAsync(EqualityComparer<TItem>.Default, ct);

    public static ValueTask<HashSet<TItem>> ToHashSetAsync<TItem>(this IQueryable<TItem> source, IEqualityComparer<TItem> comparer, CancellationToken ct = default)
        => IsNotNull(source).MakeHashSetAsync(comparer, ct);

    public static ValueTask<HashSet<TResult>> ToHashSetAsync<TItem, TResult>(this IQueryable<TItem> source, Expression<Func<TItem, TResult>> mapping, CancellationToken ct = default)
        => IsNotNull(source).Select(mapping).MakeHashSetAsync(EqualityComparer<TResult>.Default, ct);

    public static ValueTask<HashSet<TResult>> ToHashSetAsync<TItem, TResult>(this IQueryable<TItem> source, Expression<Func<TItem, TResult>> mapping, IEqualityComparer<TResult> comparer, CancellationToken ct = default)
        => IsNotNull(source).Select(mapping).MakeHashSetAsync(comparer, ct);

    public static ValueTask<HashSet<TResult>> ToHashSetAsync<TItem, TResult>(this IQueryable<TItem> source, Expression<Func<TItem, int, TResult>> mapping, CancellationToken ct = default)
        => IsNotNull(source).Select(mapping).MakeHashSetAsync(EqualityComparer<TResult>.Default, ct);

    public static ValueTask<HashSet<TResult>> ToHashSetAsync<TItem, TResult>(this IQueryable<TItem> source, Expression<Func<TItem, int, TResult>> mapping, IEqualityComparer<TResult> comparer, CancellationToken ct = default)
        => IsNotNull(source).Select(mapping).MakeHashSetAsync(comparer, ct);

    private static async ValueTask<HashSet<TResult>> MakeHashSetAsync<TResult>(this IQueryable<TResult> source, IEqualityComparer<TResult> comparer, CancellationToken ct = default) {
        var result = new HashSet<TResult>(IsNotNull(comparer));
        await foreach (var item in IsNotNull(source).AsAsyncEnumerable(ct))
            result.Add(item);
        return result;
    }
}
