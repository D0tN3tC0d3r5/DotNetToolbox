namespace DotNetToolbox.Linq.Async;

public static partial class AsyncEnumerableExtensions {
    public static async ValueTask<HashSet<TItem>> ToHashSetAsync<TItem>(this IAsyncQueryable<TItem> source, CancellationToken ct = default) {
        var result = new HashSet<TItem>();
        await foreach (var item in IsNotNull(source).WithCancellation(ct))
            result.Add(item);
        return result;
    }

    public static async ValueTask<HashSet<TItem>> ToHashSetAsync<TItem>(this IAsyncQueryable<TItem> source, IEqualityComparer<TItem> comparer, CancellationToken ct = default) {
        var result = new HashSet<TItem>(IsNotNull(comparer));
        await foreach (var item in IsNotNull(source).WithCancellation(ct))
            result.Add(item);
        return result;
    }

    public static async ValueTask<HashSet<TResult>> ToHashSetAsync<TItem, TResult>(this IAsyncQueryable<TItem> source, Func<TItem, TResult> mapping, CancellationToken ct = default) {
        var result = new HashSet<TResult>();
        await foreach (var item in IsNotNull(source).WithCancellation(ct))
            result.Add(mapping(item));
        return result;
    }

    public static async ValueTask<HashSet<TResult>> ToHashSetAsync<TItem, TResult>(this IAsyncQueryable<TItem> source, Func<TItem, TResult> mapping, IEqualityComparer<TResult> comparer, CancellationToken ct = default) {
        var result = new HashSet<TResult>(IsNotNull(comparer));
        await foreach (var item in IsNotNull(source).WithCancellation(ct))
            result.Add(mapping(item));
        return result;
    }

    public static async ValueTask<HashSet<TResult>> ToHashSetAsync<TItem, TResult>(this IAsyncQueryable<TItem> source, Func<TItem, int, TResult> mapping, CancellationToken ct = default) {
        var result = new HashSet<TResult>();
        var index = 0;
        await foreach (var item in IsNotNull(source).WithCancellation(ct))
            result.Add(mapping(item, index++));
        return result;
    }

    public static async ValueTask<HashSet<TResult>> ToHashSetAsync<TItem, TResult>(this IAsyncQueryable<TItem> source, Func<TItem, int, TResult> mapping, IEqualityComparer<TResult> comparer, CancellationToken ct = default) {
        var result = new HashSet<TResult>(IsNotNull(comparer));
        var index = 0;
        await foreach (var item in IsNotNull(source).WithCancellation(ct))
            result.Add(mapping(item, index++));
        return result;
    }
}
