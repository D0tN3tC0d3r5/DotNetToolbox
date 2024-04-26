// ReSharper disable once CheckNamespace - Intended to be in this namespace
namespace System.Linq.Async;

public static partial class AsyncEnumerableExtensions {
    public static async ValueTask<TItem> AggregateAsync<TItem>(this IAsyncQueryable<TItem> source, Func<TItem, TItem, TItem> aggregate, CancellationToken cancellationToken = default) {
        IsNotNull(aggregate);
        var result = default(TItem)!;
        await foreach (var item in IsNotNull(source).WithCancellation(cancellationToken).ConfigureAwait(false)) {
            result = aggregate(result, item);
        }
        return result;
    }

    public static async ValueTask<TResult> AggregateAsync<TItem, TResult>(this IAsyncQueryable<TItem> source, TResult seed, Func<TResult, TItem, TResult> aggregate, CancellationToken cancellationToken = default) {
        IsNotNull(aggregate);
        var result = seed;
        await foreach (var item in IsNotNull(source).WithCancellation(cancellationToken).ConfigureAwait(false)) {
            result = aggregate(result, item);
        }
        return result;
    }

    public static async ValueTask<TResult> AggregateAsync<TItem, TAccumulate, TResult>(this IAsyncQueryable<TItem> source, TAccumulate seed, Func<TAccumulate, TItem, TAccumulate> aggregate, Func<TAccumulate, TResult> selector, CancellationToken cancellationToken = default) {
        IsNotNull(selector);
        IsNotNull(aggregate);
        var result = seed;
        await foreach (var item in IsNotNull(source).WithCancellation(cancellationToken).ConfigureAwait(false)) {
            result = aggregate(result, item);
        }
        return selector(result);
    }
}
