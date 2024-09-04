// ReSharper disable once CheckNamespace - Intended to be in this namespace
namespace System.Linq.Async;

public static partial class QueryableAsyncExtensions {
    public static ValueTask<List<TItem>> ToListAsync<TItem>(this IQueryable<TItem> source, CancellationToken ct = default)
        => IsNotNull(source).MakeListAsync(ct);

    public static ValueTask<List<TResult>> ToListAsync<TItem, TResult>(this IQueryable<TItem> source, Expression<Func<TItem, TResult>> mapping, CancellationToken ct = default)
        => IsNotNull(source).Select(mapping).MakeListAsync(ct);

    public static ValueTask<List<TResult>> ToListAsync<TItem, TResult>(this IQueryable<TItem> source, Expression<Func<TItem, int, TResult>> mapping, CancellationToken ct = default)
        => IsNotNull(source).Select(mapping).MakeListAsync(ct);

    private static async ValueTask<List<TResult>> MakeListAsync<TResult>(this IQueryable<TResult> source, CancellationToken ct = default) {
        var result = new List<TResult>();
        await foreach (var item in IsNotNull(source).AsAsyncEnumerable(ct))
            result.Add(item);
        return result;
    }
}
