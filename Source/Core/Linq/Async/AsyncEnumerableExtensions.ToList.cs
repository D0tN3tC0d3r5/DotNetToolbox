namespace DotNetToolbox.Linq.Async;

public static partial class AsyncEnumerableExtensions {
    public static async ValueTask<List<TItem>> ToListAsync<TItem>(this IAsyncQueryable<TItem> source, CancellationToken ct = default) {
        var result = new List<TItem>();
        await foreach (var item in IsNotNull(source).WithCancellation(ct))
            result.Add(item);
        return result;
    }

    public static async ValueTask<List<TResult>> ToListAsync<TItem, TResult>(this IAsyncQueryable<TItem> source, Func<TItem, TResult> mapping, CancellationToken ct = default) {
        var result = new List<TResult>();
        await foreach (var item in IsNotNull(source).WithCancellation(ct))
            result.Add(mapping(item));
        return result;
    }

    public static async ValueTask<List<TResult>> ToListAsync<TItem, TResult>(this IAsyncQueryable<TItem> source, Func<TItem, int, TResult> mapping, CancellationToken ct = default) {
        var result = new List<TResult>();
        var index = 0;
        await foreach (var item in IsNotNull(source).WithCancellation(ct))
            result.Add(mapping(item, index++));
        return result;
    }
}
