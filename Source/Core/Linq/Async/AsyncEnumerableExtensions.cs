namespace DotNetToolbox.Linq.Async;

public static partial class AsyncEnumerableExtensions {
    //    public static ValueTask<bool> SequenceEqualAsync<TItem>(this IAsyncQueryable<TItem> source, IEnumerable<TItem> source2, CancellationToken ct = default)
    //        => throw new NotImplementedException();

    //    public static ValueTask<bool> SequenceEqualAsync<TItem>(this IAsyncQueryable<TItem> source, IEnumerable<TItem> source2, IEqualityComparer<TItem>? comparer, CancellationToken ct = default)
    //        => throw new NotImplementedException();

    //    public static ValueTask<bool> AllAsync<TItem>(this IAsyncQueryable<TItem> source, Expression<Func<TItem, bool>> predicate, CancellationToken ct = default)
    //        => throw new NotImplementedException();

    //    public static ValueTask<TItem?> MinAsync<TItem>(this IAsyncQueryable<TItem> source, CancellationToken ct = default)
    //        => throw new NotImplementedException();

    //    public static ValueTask<TResult?> MinAsync<TItem, TResult>(this IAsyncQueryable<TItem> source, Expression<Func<TItem, TResult>> selector, CancellationToken ct = default)
    //        => throw new NotImplementedException();

    //    public static ValueTask<TItem?> MinByAsync<TItem, TKey>(this IAsyncQueryable<TItem> source, Expression<Func<TItem, TKey>> keySelector, CancellationToken ct = default)
    //        => throw new NotImplementedException();

    //    public static ValueTask<TItem?> MinByAsync<TItem, TKey>(this IAsyncQueryable<TItem> source, Expression<Func<TItem, TKey>> keySelector, IComparer<TItem>? comparer, CancellationToken ct = default)
    //        => throw new NotImplementedException();

    //    public static ValueTask<TItem?> MaxAsync<TItem>(this IAsyncQueryable<TItem> source, CancellationToken ct = default)
    //        => throw new NotImplementedException();

    //    public static ValueTask<TResult?> MaxAsync<TItem, TResult>(this IAsyncQueryable<TItem> source, Expression<Func<TItem, TResult>> selector, CancellationToken ct = default)
    //        => throw new NotImplementedException();

    //    public static ValueTask<TItem?> MaxByAsync<TItem, TKey>(this IAsyncQueryable<TItem> source, Expression<Func<TItem, TKey>> keySelector, CancellationToken ct = default)
    //        => throw new NotImplementedException();

    //    public static ValueTask<TItem?> MaxByAsync<TItem, TKey>(this IAsyncQueryable<TItem> source, Expression<Func<TItem, TKey>> keySelector, IComparer<TItem>? comparer, CancellationToken ct = default)
    //        => throw new NotImplementedException();

    //    public static ValueTask<int> SumAsync<TItem>(this IAsyncQueryable<TItem> source, Expression<Func<TItem, int>> selector, CancellationToken ct = default)
    //        => throw new NotImplementedException();

    //    public static ValueTask<int?> SumAsync<TItem>(this IAsyncQueryable<TItem> source, Expression<Func<TItem, int?>> selector, CancellationToken ct = default)
    //        => throw new NotImplementedException();

    //    public static ValueTask<long> SumAsync<TItem>(this IAsyncQueryable<TItem> source, Expression<Func<TItem, long>> selector, CancellationToken ct = default)
    //        => throw new NotImplementedException();

    //    public static ValueTask<long?> SumAsync<TItem>(this IAsyncQueryable<TItem> source, Expression<Func<TItem, long?>> selector, CancellationToken ct = default)
    //        => throw new NotImplementedException();

    //    public static ValueTask<float> SumAsync<TItem>(this IAsyncQueryable<TItem> source, Expression<Func<TItem, float>> selector, CancellationToken ct = default)
    //        => throw new NotImplementedException();

    //    public static ValueTask<float?> SumAsync<TItem>(this IAsyncQueryable<TItem> source, Expression<Func<TItem, float?>> selector, CancellationToken ct = default)
    //        => throw new NotImplementedException();

    //    public static ValueTask<double> SumAsync<TItem>(this IAsyncQueryable<TItem> source, Expression<Func<TItem, double>> selector, CancellationToken ct = default)
    //        => throw new NotImplementedException();

    //    public static ValueTask<double?> SumAsync<TItem>(this IAsyncQueryable<TItem> source, Expression<Func<TItem, double?>> selector, CancellationToken ct = default)
    //        => throw new NotImplementedException();

    //    public static ValueTask<decimal> SumAsync<TItem>(this IAsyncQueryable<TItem> source, Expression<Func<TItem, decimal>> selector, CancellationToken ct = default)
    //        => throw new NotImplementedException();

    //    public static ValueTask<decimal?> SumAsync<TItem>(this IAsyncQueryable<TItem> source, Expression<Func<TItem, decimal?>> selector, CancellationToken ct = default)
    //        => throw new NotImplementedException();

    //    public static ValueTask<double> AverageAsync<TItem>(this IAsyncQueryable<TItem> source, Expression<Func<TItem, int>> selector, CancellationToken ct = default)
    //        => throw new NotImplementedException();

    //    public static ValueTask<double?> AverageAsync<TItem>(this IAsyncQueryable<TItem> source, Expression<Func<TItem, int?>> selector, CancellationToken ct = default)
    //        => throw new NotImplementedException();

    //    public static ValueTask<float> AverageAsync<TItem>(this IAsyncQueryable<TItem> source, Expression<Func<TItem, float>> selector, CancellationToken ct = default)
    //        => throw new NotImplementedException();

    //    public static ValueTask<float?> AverageAsync<TItem>(this IAsyncQueryable<TItem> source, Expression<Func<TItem, float?>> selector, CancellationToken ct = default)
    //        => throw new NotImplementedException();

    //    public static ValueTask<double> AverageAsync<TItem>(this IAsyncQueryable<TItem> source, Expression<Func<TItem, long>> selector, CancellationToken ct = default)
    //        => throw new NotImplementedException();

    //    public static ValueTask<double?> AverageAsync<TItem>(this IAsyncQueryable<TItem> source, Expression<Func<TItem, long?>> selector, CancellationToken ct = default)
    //        => throw new NotImplementedException();

    //    public static ValueTask<double> AverageAsync<TItem>(this IAsyncQueryable<TItem> source, Expression<Func<TItem, double>> selector, CancellationToken ct = default)
    //        => throw new NotImplementedException();

    //    public static ValueTask<double?> AverageAsync<TItem>(this IAsyncQueryable<TItem> source, Expression<Func<TItem, double?>> selector, CancellationToken ct = default)
    //        => throw new NotImplementedException();

    //    public static ValueTask<decimal> AverageAsync<TItem>(this IAsyncQueryable<TItem> source, Expression<Func<TItem, decimal>> selector, CancellationToken ct = default)
    //        => throw new NotImplementedException();

    //    public static ValueTask<decimal?> AverageAsync<TItem>(this IAsyncQueryable<TItem> source, Expression<Func<TItem, decimal?>> selector, CancellationToken ct = default)
    //        => throw new NotImplementedException();

    //    public static ValueTask<TItem> AggregateAsync<TItem>(this IAsyncQueryable<TItem> source, Expression<Func<TItem, TItem, TItem>> func, CancellationToken ct = default)
    //        => throw new NotImplementedException();

    //    public static ValueTask<TAccumulate> AggregateAsync<TItem, TAccumulate>(this IAsyncQueryable<TItem> source, TAccumulate seed, Expression<Func<TAccumulate, TItem, TAccumulate>> func, CancellationToken ct = default)
    //        => throw new NotImplementedException();

    //    public static ValueTask<TResult> AggregateAsync<TItem, TAccumulate, TResult>(this IAsyncQueryable<TItem> source, TAccumulate seed, Expression<Func<TAccumulate, TItem, TAccumulate>> func, Expression<Func<TAccumulate, TResult>> selector, CancellationToken ct = default)
    //        => throw new NotImplementedException();
}
