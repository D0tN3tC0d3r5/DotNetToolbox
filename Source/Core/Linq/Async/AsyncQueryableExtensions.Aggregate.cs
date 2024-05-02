// ReSharper disable once CheckNamespace - Intended to be in this namespace
namespace System.Linq.Async;

public static partial class AsyncQueryableExtensions {
    public static ValueTask<TItem> AggregateAsync<TItem>(this IQueryable<TItem> source, Func<TItem, TItem, TItem> aggregate, CancellationToken cancellationToken = default)
        => GetAggregatedValue(source, default, aggregate, x => x, cancellationToken);

    public static ValueTask<TResult> AggregateAsync<TItem, TResult>(this IQueryable<TItem> source, Func<TItem, TItem, TItem> aggregate, Func<TItem, TResult> resultSelector, CancellationToken cancellationToken = default)
        => GetAggregatedValue(source, default, aggregate, resultSelector, cancellationToken);

    public static ValueTask<TAccumulate> AggregateAsync<TItem, TAccumulate>(this IQueryable<TItem> source, TAccumulate seed, Func<TAccumulate, TItem, TAccumulate> aggregate, CancellationToken cancellationToken = default)
        => GetAggregatedValue(source, seed, aggregate, x => x, cancellationToken);

    public static ValueTask<TResult> AggregateAsync<TItem, TAccumulate, TResult>(this IQueryable<TItem> source, TAccumulate seed, Func<TAccumulate, TItem, TAccumulate> aggregate, Func<TAccumulate, TResult> resultSelector, CancellationToken cancellationToken = default)
        => GetAggregatedValue(source, seed, aggregate, resultSelector, cancellationToken);

    private static async ValueTask<TResult> GetAggregatedValue<TItem, TAccumulate, TResult>(
        IQueryable<TItem> source,
        TAccumulate? seed,
        Func<TAccumulate, TItem, TAccumulate> aggregate,
        Func<TAccumulate, TResult> resultSelector,
        CancellationToken cancellationToken) {
        IsNotNull(resultSelector);
        IsNotNull(aggregate);
        await using var enumerator = IsNotNull(source).GetAsyncEnumerator(cancellationToken);
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
