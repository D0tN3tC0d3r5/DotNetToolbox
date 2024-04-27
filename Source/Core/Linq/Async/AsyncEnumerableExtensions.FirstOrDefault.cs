// ReSharper disable once CheckNamespace - Intended to be in this namespace

namespace System.Linq.Async;

public static partial class AsyncEnumerableExtensions {
    public static ValueTask<TItem?> FirstOrDefaultAsync<TItem>(this IAsyncQueryable<TItem> source, CancellationToken cancellationToken = default)
        => FindFirstOrDefault(source, _ => true, default, cancellationToken);

    public static ValueTask<TItem?> FirstOrDefaultAsync<TItem>(this IAsyncQueryable<TItem> source, Func<TItem, bool> predicate, CancellationToken cancellationToken = default)
        => FindFirstOrDefault(source, predicate, default, cancellationToken);

    public static ValueTask<TItem?> FirstOrDefaultAsync<TItem>(this IAsyncQueryable<TItem> source, TItem? defaultValue, CancellationToken cancellationToken = default)
        => FindFirstOrDefault(source, _ => true, defaultValue, cancellationToken)!;

    public static ValueTask<TItem?> FirstOrDefaultAsync<TItem>(this IAsyncQueryable<TItem> source, Func<TItem, bool> predicate, TItem? defaultValue, CancellationToken cancellationToken = default)
        => FindFirstOrDefault(source, predicate, defaultValue, cancellationToken);

    private static async ValueTask<TItem?> FindFirstOrDefault<TItem>(IAsyncQueryable<TItem> source, Func<TItem, bool> predicate, TItem? defaultValue, CancellationToken cancellationToken) {
        IsNotNull(predicate);
        await foreach (var item in IsNotNull(source).WithCancellation(cancellationToken).ConfigureAwait(false)) {
            if (!predicate(item)) continue;
            return item;
        }

        return defaultValue;
    }
}
