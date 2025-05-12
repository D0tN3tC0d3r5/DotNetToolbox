// ReSharper disable once CheckNamespace - Intended to be in this namespace
namespace System.Linq.Async;

public static partial class AsyncQueryableExtensions {
    public static ValueTask<TItem> AggregateAsync<TItem>(this IAsyncQueryable<TItem> source, Func<TItem, TItem, TItem> aggregate, CancellationToken ct = default)
        => GetAggregatedValue(source, default, aggregate, static x => x, ct);

    public static ValueTask<TResult> AggregateAsync<TItem, TResult>(this IAsyncQueryable<TItem> source, Func<TItem, TItem, TItem> aggregate, Func<TItem, TResult> resultSelector, CancellationToken ct = default)
        => GetAggregatedValue(source, default, aggregate, resultSelector, ct);

    public static ValueTask<TAccumulate> AggregateAsync<TItem, TAccumulate>(this IAsyncQueryable<TItem> source, TAccumulate seed, Func<TAccumulate, TItem, TAccumulate> aggregate, CancellationToken ct = default)
        => GetAggregatedValue(source, seed, aggregate, static x => x, ct);

    public static ValueTask<TResult> AggregateAsync<TItem, TAccumulate, TResult>(this IAsyncQueryable<TItem> source, TAccumulate seed, Func<TAccumulate, TItem, TAccumulate> aggregate, Func<TAccumulate, TResult> resultSelector, CancellationToken ct = default)
        => GetAggregatedValue(source, seed, aggregate, resultSelector, ct);

    private static async ValueTask<TResult> GetAggregatedValue<TItem, TAccumulate, TResult>(
        IAsyncQueryable<TItem> source,
        TAccumulate? seed,
        Func<TAccumulate, TItem, TAccumulate> aggregate,
        Func<TAccumulate, TResult> resultSelector,
        CancellationToken ct) {
        IsNotNull(resultSelector);
        IsNotNull(aggregate);
        await using var enumerator = IsNotNull(source).GetAsyncEnumerator(ct);
        if (!await enumerator.MoveNextAsync().ConfigureAwait(false))
            throw new InvalidOperationException("Collection contains no elements.");
        var result = seed is not null
            ? aggregate(seed, enumerator.Current)
            : (TAccumulate)(object)enumerator.Current!;
        while (await enumerator.MoveNextAsync().ConfigureAwait(false)) {
            result = aggregate(result, enumerator.Current);
        }
        return resultSelector(result);
    }
}
