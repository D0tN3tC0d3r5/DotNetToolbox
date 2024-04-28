// ReSharper disable once CheckNamespace - Intended to be in this namespace
namespace System.Linq.Async;

public static partial class AsyncEnumerableExtensions {
    public static ValueTask<TItem> MinAsync<TItem>(
            this IAsyncQueryable<TItem> source,
            CancellationToken cancellationToken = default)
        => source.MinAsync(x => x, cancellationToken);

    public static async ValueTask<TResult> MinAsync<TItem, TResult>(
            this IAsyncQueryable<TItem> source,
            Func<TItem, TResult> selector,
            CancellationToken cancellationToken = default)
        => await source.MinByAsync(selector, Comparer<TResult>.Default, selector, cancellationToken)
        ?? throw new InvalidOperationException("Collection contains no elements.");

    public static ValueTask<TItem> MinAsync<TItem>(
            this IAsyncQueryable<TItem> source,
            IComparer<TItem> itemComparer,
            CancellationToken cancellationToken = default)
        => source.MinAsync(x => x, itemComparer, cancellationToken);

    public static ValueTask<TResult> MinAsync<TItem, TResult>(
            this IAsyncQueryable<TItem> source,
            Func<TItem, TResult> selector,
            IComparer<TResult> valueComparer,
            CancellationToken cancellationToken = default)
        => source.MinByAsync(selector, valueComparer, selector, cancellationToken);
}
