// ReSharper disable once CheckNamespace - Intended to be in this namespace

namespace System.Linq.Async;

public static partial class AsyncQueryableExtensions {
    public static ValueTask<TItem?> FirstOrDefaultAsync<TItem>(this IQueryable<TItem> source, CancellationToken cancellationToken = default)
        => FindFirstOrDefault(source, _ => true, default, cancellationToken);

    public static ValueTask<TItem?> FirstOrDefaultAsync<TItem>(this IQueryable<TItem> source, Func<TItem, bool> predicate, CancellationToken cancellationToken = default)
        => FindFirstOrDefault(source, predicate, default, cancellationToken);

    public static ValueTask<TItem?> FirstOrDefaultAsync<TItem>(this IQueryable<TItem> source, TItem? defaultValue, CancellationToken cancellationToken = default)
        => FindFirstOrDefault(source, _ => true, defaultValue, cancellationToken)!;

    public static ValueTask<TItem?> FirstOrDefaultAsync<TItem>(this IQueryable<TItem> source, Func<TItem, bool> predicate, TItem? defaultValue, CancellationToken cancellationToken = default)
        => FindFirstOrDefault(source, predicate, defaultValue, cancellationToken);

    private static async ValueTask<TItem?> FindFirstOrDefault<TItem>(IQueryable<TItem> source, Func<TItem, bool> predicate, TItem? defaultValue, CancellationToken cancellationToken) {
        IsNotNull(predicate);
        await foreach (var item in source.AsAsyncQueryable().AsConfigured(cancellationToken)) {
            if (!predicate(item))
                continue;
            return item;
        }

        return defaultValue;
    }
}
