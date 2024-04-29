// ReSharper disable once CheckNamespace - Intended to be in this namespace

namespace System.Linq.Async;

public static partial class AsyncQueryableExtensions {
    public static ValueTask<TItem?> LastOrDefaultAsync<TItem>(this IAsyncQueryable<TItem> source, CancellationToken cancellationToken = default)
        => FindLastOrDefault(source, _ => true, default, cancellationToken);

    public static ValueTask<TItem?> LastOrDefaultAsync<TItem>(this IAsyncQueryable<TItem> source, Func<TItem, bool> predicate, CancellationToken cancellationToken = default)
        => FindLastOrDefault(source, predicate, default, cancellationToken);

    public static ValueTask<TItem?> LastOrDefaultAsync<TItem>(this IAsyncQueryable<TItem> source, TItem? defaultValue, CancellationToken cancellationToken = default)
        => FindLastOrDefault(source, _ => true, defaultValue, cancellationToken);

    public static ValueTask<TItem?> LastOrDefaultAsync<TItem>(this IAsyncQueryable<TItem> source, Func<TItem, bool> predicate, TItem? defaultValue, CancellationToken cancellationToken = default)
        => FindLastOrDefault(source, predicate, defaultValue, cancellationToken);

    private static async ValueTask<TItem?> FindLastOrDefault<TItem>(IAsyncQueryable<TItem> source, Func<TItem, bool> predicate, TItem? defaultValue, CancellationToken cancellationToken) {
        IsNotNull(predicate);
        var result = defaultValue;
        await foreach (var item in source.AsConfigured(cancellationToken)) {
            if (!predicate(item))
                continue;
            result = item;
        }

        return result;
    }
}
