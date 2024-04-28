// ReSharper disable once CheckNamespace - Intended to be in this namespace
namespace System.Linq.Async;

public static partial class AsyncEnumerableExtensions {
    public static ValueTask<long> LongCountAsync<TItem>(this IAsyncQueryable<TItem> source, CancellationToken cancellationToken = default)
        => source.LongCountAsync(_ => true, cancellationToken);

    public static async ValueTask<long> LongCountAsync<TItem>(this IAsyncQueryable<TItem> source, Func<TItem, bool> predicate, CancellationToken cancellationToken = default) {
        IsNotNull(predicate);
        var count = 0L;
        await foreach (var item in source.AsConfigured(cancellationToken)) {
            if (!predicate(item)) continue;
            count++;
        }
        return count;
    }
}
