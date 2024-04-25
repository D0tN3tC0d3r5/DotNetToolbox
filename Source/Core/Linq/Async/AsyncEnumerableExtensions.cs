namespace DotNetToolbox.Linq.Async;

public static class AsyncEnumerableExtensions {
    public static async ValueTask<List<TItem>> ToListAsync<TItem>(this IAsyncEnumerable<TItem> source, CancellationToken cancellationToken = default) {
        var result = new List<TItem>();
        await foreach (var item in source.WithCancellation<TItem>(cancellationToken)) {
            result.Add(item);
        }

        return result;
    }

    public static async ValueTask<bool> AnyAsync<TItem>(this IAsyncEnumerable<TItem> source, CancellationToken cancellationToken = default) {
        source = IsNotNull(source);
        await using var enumerator = source.GetAsyncEnumerator(cancellationToken);
        return await enumerator.MoveNextAsync();
    }

    public static async ValueTask<bool> AnyAsync<TItem>(this System.Collections.Generic.IAsyncEnumerable<TItem> source, Func<TItem, bool> predicate, CancellationToken cancellationToken = default) {
        source = IsNotNull(source);
        predicate= IsNotNull(predicate);
        await foreach (var item in source.WithCancellation(cancellationToken)) {
            if (predicate(item))
                return true;
        }

        return false;
    }

    public static async Task<TItem?> FirstOrDefaultAsync<TItem>(this IAsyncEnumerable<TItem> source, Func<TItem, bool> predicate, CancellationToken cancellationToken = default) {
        await foreach (var item in source.WithCancellation<TItem>(cancellationToken)) {
            if (predicate(item)) return item;
        }

        return default;
    }

    //    public static Task<TItem[]> ToArrayAsync<TItem>(this IAsyncQueryable<TItem> query, CancellationToken ct = default)
    //        => throw new NotImplementedException();

    //    public static Task<TResult[]> ToArrayAsync<TItem, TResult>(this IAsyncQueryable<TItem> query, Expression<Func<TItem, TResult>> mapping, CancellationToken ct = default)
    //        => throw new NotImplementedException();

    //    public static Task<List<TItem>> ToListAsync<TItem>(this IAsyncQueryable<TItem> query, CancellationToken ct = default)
    //        => throw new NotImplementedException();

    //    public static Task<List<TResult>> ToListAsync<TItem, TResult>(this IAsyncQueryable<TItem> query, Expression<Func<TItem, TResult>> mapping, CancellationToken ct = default)
    //        => throw new NotImplementedException();

    //    public static Task<HashSet<TItem>> ToHashSetAsync<TItem>(this IAsyncQueryable<TItem> query, CancellationToken ct = default)
    //        => throw new NotImplementedException();

    //    public static Task<HashSet<TResult>> ToHashSetAsync<TItem, TResult>(this IAsyncQueryable<TItem> query, Expression<Func<TItem, TResult>> mapping, CancellationToken ct = default)
    //        => throw new NotImplementedException();

    //    public static Task<Dictionary<TKey, TValue>> ToDictionaryAsync<TItem, TKey, TValue>(this IAsyncQueryable<TItem> query, Func<TItem, TKey> selectKey, Func<TItem, TValue> selectValue, IEqualityComparer<TKey>? comparer = null, CancellationToken ct = default)
    //        where TKey : notnull
    //        => throw new NotImplementedException();

    //    public static Task<TItem> FirstAsync<TItem>(this IAsyncQueryable<TItem> query, CancellationToken ct = default)
    //        => throw new NotImplementedException();

    //    public static Task<TItem> FirstAsync<TItem>(this IAsyncQueryable<TItem> query, Expression<Func<TItem, bool>> predicate, CancellationToken ct = default)
    //        => throw new NotImplementedException();

    //    public static Task<TItem?> FirstOrDefaultAsync<TItem>(this IAsyncQueryable<TItem> query, CancellationToken ct = default)
    //        => throw new NotImplementedException();

    //    public static Task<TItem> FirstOrDefaultAsync<TItem>(this IAsyncQueryable<TItem> query, TItem defaultValue, CancellationToken ct = default)
    //        => throw new NotImplementedException();

    //    public static Task<TItem?> FirstOrDefaultAsync<TItem>(this IAsyncQueryable<TItem> query, Expression<Func<TItem, bool>> predicate, CancellationToken ct = default)
    //        => throw new NotImplementedException();

    //    public static Task<TItem> FirstOrDefaultAsync<TItem>(this IAsyncQueryable<TItem> query, Expression<Func<TItem, bool>> predicate, TItem defaultValue, CancellationToken ct = default)
    //        => throw new NotImplementedException();

    //    public static Task<TItem> LastAsync<TItem>(this IAsyncQueryable<TItem> query, CancellationToken ct = default)
    //        => throw new NotImplementedException();

    //    public static Task<TItem> LastAsync<TItem>(this IAsyncQueryable<TItem> query, Expression<Func<TItem, bool>> predicate, CancellationToken ct = default)
    //        => throw new NotImplementedException();

    //    public static Task<TItem?> LastOrDefaultAsync<TItem>(this IAsyncQueryable<TItem> query, CancellationToken ct = default)
    //        => throw new NotImplementedException();

    //    public static Task<TItem> LastOrDefaultAsync<TItem>(this IAsyncQueryable<TItem> query, TItem defaultValue, CancellationToken ct = default)
    //        => throw new NotImplementedException();

    //    public static Task<TItem?> LastOrDefaultAsync<TItem>(this IAsyncQueryable<TItem> query, Expression<Func<TItem, bool>> predicate, CancellationToken ct = default)
    //        => throw new NotImplementedException();

    //    public static Task<TItem> LastOrDefaultAsync<TItem>(this IAsyncQueryable<TItem> query, Expression<Func<TItem, bool>> predicate, TItem defaultValue, CancellationToken ct = default)
    //        => throw new NotImplementedException();

    //    public static Task<TItem> SingleAsync<TItem>(this IAsyncQueryable<TItem> query, CancellationToken ct = default)
    //        => throw new NotImplementedException();

    //    public static Task<TItem> SingleAsync<TItem>(this IAsyncQueryable<TItem> query, Expression<Func<TItem, bool>> predicate, CancellationToken ct = default)
    //        => throw new NotImplementedException();

    //    public static Task<TItem?> SingleOrDefaultAsync<TItem>(this IAsyncQueryable<TItem> query, CancellationToken ct = default)
    //        => throw new NotImplementedException();

    //    public static Task<TItem> SingleOrDefaultAsync<TItem>(this IAsyncQueryable<TItem> query, TItem defaultValue, CancellationToken ct = default)
    //        => throw new NotImplementedException();

    //    public static Task<TItem?> SingleOrDefaultAsync<TItem>(this IAsyncQueryable<TItem> query, Expression<Func<TItem, bool>> predicate, CancellationToken ct = default)
    //        => throw new NotImplementedException();

    //    public static Task<TItem> SingleOrDefaultAsync<TItem>(this IAsyncQueryable<TItem> query, Expression<Func<TItem, bool>> predicate, TItem defaultValue, CancellationToken ct = default)
    //        => throw new NotImplementedException();

    //    public static Task<TItem> ElementAtAsync<TItem>(this IAsyncQueryable<TItem> query, int index, CancellationToken ct = default)
    //        => throw new NotImplementedException();

    //    public static Task<TItem> ElementAtAsync<TItem>(this IAsyncQueryable<TItem> query, Index index, CancellationToken ct = default)
    //        => throw new NotImplementedException();

    //    public static Task<TItem?> ElementAtOrDefault<TItem>(this IAsyncQueryable<TItem> query, int index, CancellationToken ct = default)
    //        => throw new NotImplementedException();

    //    public static Task<TItem?> ElementAtOrDefault<TItem>(this IAsyncQueryable<TItem> query, Index index, CancellationToken ct = default)
    //        => throw new NotImplementedException();

    //    public static Task<bool> ContainsAsync<TItem>(this IAsyncQueryable<TItem> query, TItem item, CancellationToken ct = default)
    //        => throw new NotImplementedException();

    //    public static Task<bool> ContainsAsync<TItem>(this IAsyncQueryable<TItem> query, TItem item, IEqualityComparer<TItem>? comparer, CancellationToken ct = default)
    //        => throw new NotImplementedException();

    //    public static Task<bool> SequenceEqualAsync<TItem>(this IAsyncQueryable<TItem> query, IEnumerable<TItem> source2, CancellationToken ct = default)
    //        => throw new NotImplementedException();

    //    public static Task<bool> SequenceEqualAsync<TItem>(this IAsyncQueryable<TItem> query, IEnumerable<TItem> source2, IEqualityComparer<TItem>? comparer, CancellationToken ct = default)
    //        => throw new NotImplementedException();

    //    public static Task<bool> AnyAsync<TItem>(this IAsyncQueryable<TItem> query, CancellationToken ct = default)
    //        => throw new NotImplementedException();

    //    public static Task<bool> AnyAsync<TItem>(this IAsyncQueryable<TItem> query, Expression<Func<TItem, bool>> predicate, CancellationToken ct = default)
    //        => throw new NotImplementedException();

    //    public static Task<bool> AllAsync<TItem>(this IAsyncQueryable<TItem> query, Expression<Func<TItem, bool>> predicate, CancellationToken ct = default)
    //        => throw new NotImplementedException();

    //    public static Task<int> CountAsync<TItem>(this IAsyncQueryable<TItem> query, CancellationToken ct = default)
    //        => throw new NotImplementedException();

    //    public static Task<int> CountAsync<TItem>(this IAsyncQueryable<TItem> query, Expression<Func<TItem, bool>> predicate, CancellationToken ct = default)
    //        => throw new NotImplementedException();

    //    public static Task<long> LongCountAsync<TItem>(this IAsyncQueryable<TItem> query, CancellationToken ct = default)
    //        => throw new NotImplementedException();

    //    public static Task<long> LongCountAsync<TItem>(this IAsyncQueryable<TItem> query, Expression<Func<TItem, bool>> predicate, CancellationToken ct = default)
    //        => throw new NotImplementedException();

    //    public static Task<TItem?> MinAsync<TItem>(this IAsyncQueryable<TItem> query, CancellationToken ct = default)
    //        => throw new NotImplementedException();

    //    public static Task<TResult?> MinAsync<TItem, TResult>(this IAsyncQueryable<TItem> query, Expression<Func<TItem, TResult>> selector, CancellationToken ct = default)
    //        => throw new NotImplementedException();

    //    public static Task<TItem?> MinByAsync<TItem, TKey>(this IAsyncQueryable<TItem> query, Expression<Func<TItem, TKey>> keySelector, CancellationToken ct = default)
    //        => throw new NotImplementedException();

    //    public static Task<TItem?> MinByAsync<TItem, TKey>(this IAsyncQueryable<TItem> query, Expression<Func<TItem, TKey>> keySelector, IComparer<TItem>? comparer, CancellationToken ct = default)
    //        => throw new NotImplementedException();

    //    public static Task<TItem?> MaxAsync<TItem>(this IAsyncQueryable<TItem> query, CancellationToken ct = default)
    //        => throw new NotImplementedException();

    //    public static Task<TResult?> MaxAsync<TItem, TResult>(this IAsyncQueryable<TItem> query, Expression<Func<TItem, TResult>> selector, CancellationToken ct = default)
    //        => throw new NotImplementedException();

    //    public static Task<TItem?> MaxByAsync<TItem, TKey>(this IAsyncQueryable<TItem> query, Expression<Func<TItem, TKey>> keySelector, CancellationToken ct = default)
    //        => throw new NotImplementedException();

    //    public static Task<TItem?> MaxByAsync<TItem, TKey>(this IAsyncQueryable<TItem> query, Expression<Func<TItem, TKey>> keySelector, IComparer<TItem>? comparer, CancellationToken ct = default)
    //        => throw new NotImplementedException();

    //    public static Task<int> SumAsync<TItem>(this IAsyncQueryable<TItem> query, Expression<Func<TItem, int>> selector, CancellationToken ct = default)
    //        => throw new NotImplementedException();

    //    public static Task<int?> SumAsync<TItem>(this IAsyncQueryable<TItem> query, Expression<Func<TItem, int?>> selector, CancellationToken ct = default)
    //        => throw new NotImplementedException();

    //    public static Task<long> SumAsync<TItem>(this IAsyncQueryable<TItem> query, Expression<Func<TItem, long>> selector, CancellationToken ct = default)
    //        => throw new NotImplementedException();

    //    public static Task<long?> SumAsync<TItem>(this IAsyncQueryable<TItem> query, Expression<Func<TItem, long?>> selector, CancellationToken ct = default)
    //        => throw new NotImplementedException();

    //    public static Task<float> SumAsync<TItem>(this IAsyncQueryable<TItem> query, Expression<Func<TItem, float>> selector, CancellationToken ct = default)
    //        => throw new NotImplementedException();

    //    public static Task<float?> SumAsync<TItem>(this IAsyncQueryable<TItem> query, Expression<Func<TItem, float?>> selector, CancellationToken ct = default)
    //        => throw new NotImplementedException();

    //    public static Task<double> SumAsync<TItem>(this IAsyncQueryable<TItem> query, Expression<Func<TItem, double>> selector, CancellationToken ct = default)
    //        => throw new NotImplementedException();

    //    public static Task<double?> SumAsync<TItem>(this IAsyncQueryable<TItem> query, Expression<Func<TItem, double?>> selector, CancellationToken ct = default)
    //        => throw new NotImplementedException();

    //    public static Task<decimal> SumAsync<TItem>(this IAsyncQueryable<TItem> query, Expression<Func<TItem, decimal>> selector, CancellationToken ct = default)
    //        => throw new NotImplementedException();

    //    public static Task<decimal?> SumAsync<TItem>(this IAsyncQueryable<TItem> query, Expression<Func<TItem, decimal?>> selector, CancellationToken ct = default)
    //        => throw new NotImplementedException();

    //    public static Task<double> AverageAsync<TItem>(this IAsyncQueryable<TItem> query, Expression<Func<TItem, int>> selector, CancellationToken ct = default)
    //        => throw new NotImplementedException();

    //    public static Task<double?> AverageAsync<TItem>(this IAsyncQueryable<TItem> query, Expression<Func<TItem, int?>> selector, CancellationToken ct = default)
    //        => throw new NotImplementedException();

    //    public static Task<float> AverageAsync<TItem>(this IAsyncQueryable<TItem> query, Expression<Func<TItem, float>> selector, CancellationToken ct = default)
    //        => throw new NotImplementedException();

    //    public static Task<float?> AverageAsync<TItem>(this IAsyncQueryable<TItem> query, Expression<Func<TItem, float?>> selector, CancellationToken ct = default)
    //        => throw new NotImplementedException();

    //    public static Task<double> AverageAsync<TItem>(this IAsyncQueryable<TItem> query, Expression<Func<TItem, long>> selector, CancellationToken ct = default)
    //        => throw new NotImplementedException();

    //    public static Task<double?> AverageAsync<TItem>(this IAsyncQueryable<TItem> query, Expression<Func<TItem, long?>> selector, CancellationToken ct = default)
    //        => throw new NotImplementedException();

    //    public static Task<double> AverageAsync<TItem>(this IAsyncQueryable<TItem> query, Expression<Func<TItem, double>> selector, CancellationToken ct = default)
    //        => throw new NotImplementedException();

    //    public static Task<double?> AverageAsync<TItem>(this IAsyncQueryable<TItem> query, Expression<Func<TItem, double?>> selector, CancellationToken ct = default)
    //        => throw new NotImplementedException();

    //    public static Task<decimal> AverageAsync<TItem>(this IAsyncQueryable<TItem> query, Expression<Func<TItem, decimal>> selector, CancellationToken ct = default)
    //        => throw new NotImplementedException();

    //    public static Task<decimal?> AverageAsync<TItem>(this IAsyncQueryable<TItem> query, Expression<Func<TItem, decimal?>> selector, CancellationToken ct = default)
    //        => throw new NotImplementedException();

    //    public static Task<TItem> AggregateAsync<TItem>(this IAsyncQueryable<TItem> query, Expression<Func<TItem, TItem, TItem>> func, CancellationToken ct = default)
    //        => throw new NotImplementedException();

    //    public static Task<TAccumulate> AggregateAsync<TItem, TAccumulate>(this IAsyncQueryable<TItem> query, TAccumulate seed, Expression<Func<TAccumulate, TItem, TAccumulate>> func, CancellationToken ct = default)
    //        => throw new NotImplementedException();

    //    public static Task<TResult> AggregateAsync<TItem, TAccumulate, TResult>(this IAsyncQueryable<TItem> query, TAccumulate seed, Expression<Func<TAccumulate, TItem, TAccumulate>> func, Expression<Func<TAccumulate, TResult>> selector, CancellationToken ct = default)
    //        => throw new NotImplementedException();
}
