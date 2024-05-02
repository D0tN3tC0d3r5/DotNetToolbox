// ReSharper disable once CheckNamespace - Intended to be in this namespace
namespace System.Linq.Async;

public static partial class AsyncQueryableExtensions {
    public static ValueTask<TItem> MaxAsync<TItem>(
            this IQueryable<TItem> source,
            CancellationToken cancellationToken = default)
        => source.MaxAsync(x => x, cancellationToken);

    public static async ValueTask<TResult> MaxAsync<TItem, TResult>(
            this IQueryable<TItem> source,
            Func<TItem, TResult> selector,
            CancellationToken cancellationToken = default)
        => await source.MaxByAsync(selector, Comparer<TResult>.Default, selector, cancellationToken)
        ?? throw new InvalidOperationException("Collection contains no elements.");

    public static ValueTask<TItem> MaxAsync<TItem>(
            this IQueryable<TItem> source,
            IComparer<TItem> itemComparer,
            CancellationToken cancellationToken = default)
        => source.MaxAsync(x => x, itemComparer, cancellationToken);

    public static ValueTask<TResult> MaxAsync<TItem, TResult>(
            this IQueryable<TItem> source,
            Func<TItem, TResult> selector,
            IComparer<TResult> valueComparer,
            CancellationToken cancellationToken = default)
        => source.MaxByAsync(selector, valueComparer, selector, cancellationToken);
}
