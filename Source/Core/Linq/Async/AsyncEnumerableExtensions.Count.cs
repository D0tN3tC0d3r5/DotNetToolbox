// ReSharper disable once CheckNamespace - Intended to be in this namespace
namespace System.Linq.Async;

public static partial class AsyncEnumerableExtensions {
    public static ValueTask<int> CountAsync<TItem>(this IAsyncQueryable<TItem> source, CancellationToken cancellationToken = default)
        => source.CountAsync(_ => true, cancellationToken);

    public static async ValueTask<int> CountAsync<TItem>(this IAsyncQueryable<TItem> source, Func<TItem, bool> predicate, CancellationToken cancellationToken = default) {
        IsNotNull(predicate);
        var count = 0;
        await foreach (var item in source.AsConfigured(cancellationToken)) {
            if (predicate(item))
                count++;
        }

        return count;
    }
}
