namespace DotNetToolbox.Linq.Async;

public static class AsyncEnumerableExtensions {
    public static IEnumerable<TSource> ToEnumerable<TSource>(this IAsyncQueryable<TSource> source) {
        var enumerable = source.GetAsyncEnumerator();
        try {
            while (!enumerable.MoveNextAsync().GetResult()) {
                yield return enumerable.Current;
            }
        }
        finally {
            enumerable.DisposeAsync().Wait();
        }
    }

    public static async ValueTask<TItem[]> ToArrayAsync<TItem>(this IAsyncQueryable<TItem> source, CancellationToken ct = default) {
        var capacity = 4;
        var result = new TItem[capacity];
        var length = 0;
        await using var enumerator = IsNotNull(source).GetAsyncEnumerator(ct);
        while (await enumerator.MoveNextAsync() && length < Array.MaxLength) {
            result[length++] = enumerator.Current;
            if (length < capacity) continue;
            capacity <<= 1;
            if (capacity >= Array.MaxLength) capacity = Array.MaxLength;
            Array.Resize(ref result, capacity);
        }
        return result;
    }

    public static async ValueTask<TResult[]> ToArrayAsync<TItem, TResult>(this IAsyncQueryable<TItem> source, Func<TItem, TResult> mapping, CancellationToken ct = default) {
        var capacity = 4;
        var result = new TResult[capacity];
        var length = 0;
        await using var enumerator = IsNotNull(source).GetAsyncEnumerator(ct);
        while (await enumerator.MoveNextAsync() && length < Array.MaxLength) {
            result[length++] = mapping(enumerator.Current);
            if (length < capacity) continue;
            capacity <<= 1;
            if (capacity >= Array.MaxLength)
                capacity = Array.MaxLength;
            Array.Resize(ref result, capacity);
        }
        return result;
    }

    public static async ValueTask<List<TItem>> ToListAsync<TItem>(this IAsyncQueryable<TItem> source, CancellationToken ct = default) {
        var result = new List<TItem>();
        await foreach (var item in IsNotNull(source).WithCancellation(ct))
            result.Add(item);
        return result;
    }

    public static async ValueTask<List<TResult>> ToListAsync<TItem, TResult>(this IAsyncQueryable<TItem> source, Func<TItem, TResult> mapping, CancellationToken ct = default) {
        var result = new List<TResult>();
        await foreach (var item in IsNotNull(source).WithCancellation(ct))
            result.Add(mapping(item));
        return result;
    }

    public static async ValueTask<HashSet<TItem>> ToHashSetAsync<TItem>(this IAsyncQueryable<TItem> source, CancellationToken ct = default) {
        var result = new HashSet<TItem>();
        await foreach (var item in IsNotNull(source).WithCancellation(ct))
            result.Add(item);
        return result;
    }

    public static async ValueTask<HashSet<TItem>> ToHashSetAsync<TItem>(this IAsyncQueryable<TItem> source, IEqualityComparer<TItem> comparer, CancellationToken ct = default) {
        var result = new HashSet<TItem>(IsNotNull(comparer));
        await foreach (var item in IsNotNull(source).WithCancellation(ct))
            result.Add(item);
        return result;
    }

    public static async ValueTask<HashSet<TResult>> ToHashSetAsync<TItem, TResult>(this IAsyncQueryable<TItem> source, Func<TItem, TResult> mapping, CancellationToken ct = default) {
        var result = new HashSet<TResult>();
        await foreach (var item in IsNotNull(source).WithCancellation(ct))
            result.Add(mapping(item));
        return result;
    }

    public static async ValueTask<HashSet<TResult>> ToHashSetAsync<TItem, TResult>(this IAsyncQueryable<TItem> source, Func<TItem, TResult> mapping, IEqualityComparer<TResult> comparer, CancellationToken ct = default) {
        var result = new HashSet<TResult>(IsNotNull(comparer));
        await foreach (var item in IsNotNull(source).WithCancellation(ct))
            result.Add(mapping(item));
        return result;
    }

    public static ValueTask<Dictionary<TKey, TValue>> ToDictionaryAsync<TItem, TKey, TValue>(this IAsyncQueryable<TItem> source, Func<TItem, TKey> selectKey, Func<TItem, TValue> selectValue, IEqualityComparer<TKey>? comparer = null, CancellationToken ct = default)
        where TKey : notnull
        => throw new NotImplementedException();

    public static async ValueTask<bool> AnyAsync<TItem>(this IAsyncQueryable<TItem> source, CancellationToken cancellationToken = default) {
        await using var enumerator = IsNotNull(source).GetAsyncEnumerator(cancellationToken);
        return await enumerator.MoveNextAsync();
    }

    public static async ValueTask<bool> AnyAsync<TItem>(this IAsyncQueryable<TItem> source, Func<TItem, bool> predicate, CancellationToken cancellationToken = default) {
        IsNotNull(predicate);
        await foreach (var item in IsNotNull(source).WithCancellation(cancellationToken)) {
            if (predicate(item))
                return true;
        }

        return false;
    }

    public static async Task<int> CountAsync<TItem>(this IAsyncQueryable<TItem> source, CancellationToken cancellationToken = default) {
        await using var enumerator = IsNotNull(source).GetAsyncEnumerator(cancellationToken);
        var count = 0;
        while (await enumerator.MoveNextAsync()) count++;
        return count;
    }

    public static async Task<int> CountAsync<TItem>(this IAsyncQueryable<TItem> source, Func<TItem, bool> predicate, CancellationToken cancellationToken = default) {
        IsNotNull(predicate);
        await using var enumerator = IsNotNull(source).GetAsyncEnumerator(cancellationToken);
        var count = 0;
        while (await enumerator.MoveNextAsync())
            if (predicate(enumerator.Current)) count++;
        return count;
    }

    public static async Task<TItem?> FirstOrDefaultAsync<TItem>(this IAsyncQueryable<TItem> source, CancellationToken cancellationToken = default) {
        await using var enumerator = IsNotNull(source).GetAsyncEnumerator(cancellationToken);
        return await enumerator.MoveNextAsync()
                   ? enumerator.Current
                   : default;
    }

    public static async Task<TItem?> FirstOrDefaultAsync<TItem>(this IAsyncQueryable<TItem> source, Func<TItem, bool> predicate, CancellationToken cancellationToken = default) {
        IsNotNull(predicate);
        await foreach (var item in IsNotNull(source).WithCancellation(cancellationToken)) {
            if (predicate(item)) return item;
        }

        return default;
    }

    //    public static Task<TItem[]> ToArrayAsync<TItem>(this IAsyncQueryable<TItem> source, CancellationToken ct = default)
    //        => throw new NotImplementedException();

    //    public static Task<TResult[]> ToArrayAsync<TItem, TResult>(this IAsyncQueryable<TItem> source, Expression<Func<TItem, TResult>> mapping, CancellationToken ct = default)
    //        => throw new NotImplementedException();

    //    public static Task<List<TItem>> ToListAsync<TItem>(this IAsyncQueryable<TItem> source, CancellationToken ct = default)
    //        => throw new NotImplementedException();

    //    public static Task<List<TResult>> ToListAsync<TItem, TResult>(this IAsyncQueryable<TItem> source, Expression<Func<TItem, TResult>> mapping, CancellationToken ct = default)
    //        => throw new NotImplementedException();

    //    public static Task<HashSet<TItem>> ToHashSetAsync<TItem>(this IAsyncQueryable<TItem> source, CancellationToken ct = default)
    //        => throw new NotImplementedException();

    //    public static Task<HashSet<TResult>> ToHashSetAsync<TItem, TResult>(this IAsyncQueryable<TItem> source, Expression<Func<TItem, TResult>> mapping, CancellationToken ct = default)
    //        => throw new NotImplementedException();

    //    public static Task<Dictionary<TKey, TValue>> ToDictionaryAsync<TItem, TKey, TValue>(this IAsyncQueryable<TItem> source, Func<TItem, TKey> selectKey, Func<TItem, TValue> selectValue, IEqualityComparer<TKey>? comparer = null, CancellationToken ct = default)
    //        where TKey : notnull
    //        => throw new NotImplementedException();

    //    public static Task<TItem> FirstAsync<TItem>(this IAsyncQueryable<TItem> source, CancellationToken ct = default)
    //        => throw new NotImplementedException();

    //    public static Task<TItem> FirstAsync<TItem>(this IAsyncQueryable<TItem> source, Expression<Func<TItem, bool>> predicate, CancellationToken ct = default)
    //        => throw new NotImplementedException();

    //    public static Task<TItem?> FirstOrDefaultAsync<TItem>(this IAsyncQueryable<TItem> source, CancellationToken ct = default)
    //        => throw new NotImplementedException();

    //    public static Task<TItem> FirstOrDefaultAsync<TItem>(this IAsyncQueryable<TItem> source, TItem defaultValue, CancellationToken ct = default)
    //        => throw new NotImplementedException();

    //    public static Task<TItem?> FirstOrDefaultAsync<TItem>(this IAsyncQueryable<TItem> source, Expression<Func<TItem, bool>> predicate, CancellationToken ct = default)
    //        => throw new NotImplementedException();

    //    public static Task<TItem> FirstOrDefaultAsync<TItem>(this IAsyncQueryable<TItem> source, Expression<Func<TItem, bool>> predicate, TItem defaultValue, CancellationToken ct = default)
    //        => throw new NotImplementedException();

    //    public static Task<TItem> LastAsync<TItem>(this IAsyncQueryable<TItem> source, CancellationToken ct = default)
    //        => throw new NotImplementedException();

    //    public static Task<TItem> LastAsync<TItem>(this IAsyncQueryable<TItem> source, Expression<Func<TItem, bool>> predicate, CancellationToken ct = default)
    //        => throw new NotImplementedException();

    //    public static Task<TItem?> LastOrDefaultAsync<TItem>(this IAsyncQueryable<TItem> source, CancellationToken ct = default)
    //        => throw new NotImplementedException();

    //    public static Task<TItem> LastOrDefaultAsync<TItem>(this IAsyncQueryable<TItem> source, TItem defaultValue, CancellationToken ct = default)
    //        => throw new NotImplementedException();

    //    public static Task<TItem?> LastOrDefaultAsync<TItem>(this IAsyncQueryable<TItem> source, Expression<Func<TItem, bool>> predicate, CancellationToken ct = default)
    //        => throw new NotImplementedException();

    //    public static Task<TItem> LastOrDefaultAsync<TItem>(this IAsyncQueryable<TItem> source, Expression<Func<TItem, bool>> predicate, TItem defaultValue, CancellationToken ct = default)
    //        => throw new NotImplementedException();

    //    public static Task<TItem> SingleAsync<TItem>(this IAsyncQueryable<TItem> source, CancellationToken ct = default)
    //        => throw new NotImplementedException();

    //    public static Task<TItem> SingleAsync<TItem>(this IAsyncQueryable<TItem> source, Expression<Func<TItem, bool>> predicate, CancellationToken ct = default)
    //        => throw new NotImplementedException();

    //    public static Task<TItem?> SingleOrDefaultAsync<TItem>(this IAsyncQueryable<TItem> source, CancellationToken ct = default)
    //        => throw new NotImplementedException();

    //    public static Task<TItem> SingleOrDefaultAsync<TItem>(this IAsyncQueryable<TItem> source, TItem defaultValue, CancellationToken ct = default)
    //        => throw new NotImplementedException();

    //    public static Task<TItem?> SingleOrDefaultAsync<TItem>(this IAsyncQueryable<TItem> source, Expression<Func<TItem, bool>> predicate, CancellationToken ct = default)
    //        => throw new NotImplementedException();

    //    public static Task<TItem> SingleOrDefaultAsync<TItem>(this IAsyncQueryable<TItem> source, Expression<Func<TItem, bool>> predicate, TItem defaultValue, CancellationToken ct = default)
    //        => throw new NotImplementedException();

    //    public static Task<TItem> ElementAtAsync<TItem>(this IAsyncQueryable<TItem> source, int index, CancellationToken ct = default)
    //        => throw new NotImplementedException();

    //    public static Task<TItem> ElementAtAsync<TItem>(this IAsyncQueryable<TItem> source, Index index, CancellationToken ct = default)
    //        => throw new NotImplementedException();

    //    public static Task<TItem?> ElementAtOrDefault<TItem>(this IAsyncQueryable<TItem> source, int index, CancellationToken ct = default)
    //        => throw new NotImplementedException();

    //    public static Task<TItem?> ElementAtOrDefault<TItem>(this IAsyncQueryable<TItem> source, Index index, CancellationToken ct = default)
    //        => throw new NotImplementedException();

    //    public static Task<bool> ContainsAsync<TItem>(this IAsyncQueryable<TItem> source, TItem item, CancellationToken ct = default)
    //        => throw new NotImplementedException();

    //    public static Task<bool> ContainsAsync<TItem>(this IAsyncQueryable<TItem> source, TItem item, IEqualityComparer<TItem>? comparer, CancellationToken ct = default)
    //        => throw new NotImplementedException();

    //    public static Task<bool> SequenceEqualAsync<TItem>(this IAsyncQueryable<TItem> source, IEnumerable<TItem> source2, CancellationToken ct = default)
    //        => throw new NotImplementedException();

    //    public static Task<bool> SequenceEqualAsync<TItem>(this IAsyncQueryable<TItem> source, IEnumerable<TItem> source2, IEqualityComparer<TItem>? comparer, CancellationToken ct = default)
    //        => throw new NotImplementedException();

    //    public static Task<bool> AnyAsync<TItem>(this IAsyncQueryable<TItem> source, CancellationToken ct = default)
    //        => throw new NotImplementedException();

    //    public static Task<bool> AnyAsync<TItem>(this IAsyncQueryable<TItem> source, Expression<Func<TItem, bool>> predicate, CancellationToken ct = default)
    //        => throw new NotImplementedException();

    //    public static Task<bool> AllAsync<TItem>(this IAsyncQueryable<TItem> source, Expression<Func<TItem, bool>> predicate, CancellationToken ct = default)
    //        => throw new NotImplementedException();

    //    public static Task<int> CountAsync<TItem>(this IAsyncQueryable<TItem> source, CancellationToken ct = default)
    //        => throw new NotImplementedException();

    //    public static Task<int> CountAsync<TItem>(this IAsyncQueryable<TItem> source, Expression<Func<TItem, bool>> predicate, CancellationToken ct = default)
    //        => throw new NotImplementedException();

    //    public static Task<long> LongCountAsync<TItem>(this IAsyncQueryable<TItem> source, CancellationToken ct = default)
    //        => throw new NotImplementedException();

    //    public static Task<long> LongCountAsync<TItem>(this IAsyncQueryable<TItem> source, Expression<Func<TItem, bool>> predicate, CancellationToken ct = default)
    //        => throw new NotImplementedException();

    //    public static Task<TItem?> MinAsync<TItem>(this IAsyncQueryable<TItem> source, CancellationToken ct = default)
    //        => throw new NotImplementedException();

    //    public static Task<TResult?> MinAsync<TItem, TResult>(this IAsyncQueryable<TItem> source, Expression<Func<TItem, TResult>> selector, CancellationToken ct = default)
    //        => throw new NotImplementedException();

    //    public static Task<TItem?> MinByAsync<TItem, TKey>(this IAsyncQueryable<TItem> source, Expression<Func<TItem, TKey>> keySelector, CancellationToken ct = default)
    //        => throw new NotImplementedException();

    //    public static Task<TItem?> MinByAsync<TItem, TKey>(this IAsyncQueryable<TItem> source, Expression<Func<TItem, TKey>> keySelector, IComparer<TItem>? comparer, CancellationToken ct = default)
    //        => throw new NotImplementedException();

    //    public static Task<TItem?> MaxAsync<TItem>(this IAsyncQueryable<TItem> source, CancellationToken ct = default)
    //        => throw new NotImplementedException();

    //    public static Task<TResult?> MaxAsync<TItem, TResult>(this IAsyncQueryable<TItem> source, Expression<Func<TItem, TResult>> selector, CancellationToken ct = default)
    //        => throw new NotImplementedException();

    //    public static Task<TItem?> MaxByAsync<TItem, TKey>(this IAsyncQueryable<TItem> source, Expression<Func<TItem, TKey>> keySelector, CancellationToken ct = default)
    //        => throw new NotImplementedException();

    //    public static Task<TItem?> MaxByAsync<TItem, TKey>(this IAsyncQueryable<TItem> source, Expression<Func<TItem, TKey>> keySelector, IComparer<TItem>? comparer, CancellationToken ct = default)
    //        => throw new NotImplementedException();

    //    public static Task<int> SumAsync<TItem>(this IAsyncQueryable<TItem> source, Expression<Func<TItem, int>> selector, CancellationToken ct = default)
    //        => throw new NotImplementedException();

    //    public static Task<int?> SumAsync<TItem>(this IAsyncQueryable<TItem> source, Expression<Func<TItem, int?>> selector, CancellationToken ct = default)
    //        => throw new NotImplementedException();

    //    public static Task<long> SumAsync<TItem>(this IAsyncQueryable<TItem> source, Expression<Func<TItem, long>> selector, CancellationToken ct = default)
    //        => throw new NotImplementedException();

    //    public static Task<long?> SumAsync<TItem>(this IAsyncQueryable<TItem> source, Expression<Func<TItem, long?>> selector, CancellationToken ct = default)
    //        => throw new NotImplementedException();

    //    public static Task<float> SumAsync<TItem>(this IAsyncQueryable<TItem> source, Expression<Func<TItem, float>> selector, CancellationToken ct = default)
    //        => throw new NotImplementedException();

    //    public static Task<float?> SumAsync<TItem>(this IAsyncQueryable<TItem> source, Expression<Func<TItem, float?>> selector, CancellationToken ct = default)
    //        => throw new NotImplementedException();

    //    public static Task<double> SumAsync<TItem>(this IAsyncQueryable<TItem> source, Expression<Func<TItem, double>> selector, CancellationToken ct = default)
    //        => throw new NotImplementedException();

    //    public static Task<double?> SumAsync<TItem>(this IAsyncQueryable<TItem> source, Expression<Func<TItem, double?>> selector, CancellationToken ct = default)
    //        => throw new NotImplementedException();

    //    public static Task<decimal> SumAsync<TItem>(this IAsyncQueryable<TItem> source, Expression<Func<TItem, decimal>> selector, CancellationToken ct = default)
    //        => throw new NotImplementedException();

    //    public static Task<decimal?> SumAsync<TItem>(this IAsyncQueryable<TItem> source, Expression<Func<TItem, decimal?>> selector, CancellationToken ct = default)
    //        => throw new NotImplementedException();

    //    public static Task<double> AverageAsync<TItem>(this IAsyncQueryable<TItem> source, Expression<Func<TItem, int>> selector, CancellationToken ct = default)
    //        => throw new NotImplementedException();

    //    public static Task<double?> AverageAsync<TItem>(this IAsyncQueryable<TItem> source, Expression<Func<TItem, int?>> selector, CancellationToken ct = default)
    //        => throw new NotImplementedException();

    //    public static Task<float> AverageAsync<TItem>(this IAsyncQueryable<TItem> source, Expression<Func<TItem, float>> selector, CancellationToken ct = default)
    //        => throw new NotImplementedException();

    //    public static Task<float?> AverageAsync<TItem>(this IAsyncQueryable<TItem> source, Expression<Func<TItem, float?>> selector, CancellationToken ct = default)
    //        => throw new NotImplementedException();

    //    public static Task<double> AverageAsync<TItem>(this IAsyncQueryable<TItem> source, Expression<Func<TItem, long>> selector, CancellationToken ct = default)
    //        => throw new NotImplementedException();

    //    public static Task<double?> AverageAsync<TItem>(this IAsyncQueryable<TItem> source, Expression<Func<TItem, long?>> selector, CancellationToken ct = default)
    //        => throw new NotImplementedException();

    //    public static Task<double> AverageAsync<TItem>(this IAsyncQueryable<TItem> source, Expression<Func<TItem, double>> selector, CancellationToken ct = default)
    //        => throw new NotImplementedException();

    //    public static Task<double?> AverageAsync<TItem>(this IAsyncQueryable<TItem> source, Expression<Func<TItem, double?>> selector, CancellationToken ct = default)
    //        => throw new NotImplementedException();

    //    public static Task<decimal> AverageAsync<TItem>(this IAsyncQueryable<TItem> source, Expression<Func<TItem, decimal>> selector, CancellationToken ct = default)
    //        => throw new NotImplementedException();

    //    public static Task<decimal?> AverageAsync<TItem>(this IAsyncQueryable<TItem> source, Expression<Func<TItem, decimal?>> selector, CancellationToken ct = default)
    //        => throw new NotImplementedException();

    //    public static Task<TItem> AggregateAsync<TItem>(this IAsyncQueryable<TItem> source, Expression<Func<TItem, TItem, TItem>> func, CancellationToken ct = default)
    //        => throw new NotImplementedException();

    //    public static Task<TAccumulate> AggregateAsync<TItem, TAccumulate>(this IAsyncQueryable<TItem> source, TAccumulate seed, Expression<Func<TAccumulate, TItem, TAccumulate>> func, CancellationToken ct = default)
    //        => throw new NotImplementedException();

    //    public static Task<TResult> AggregateAsync<TItem, TAccumulate, TResult>(this IAsyncQueryable<TItem> source, TAccumulate seed, Expression<Func<TAccumulate, TItem, TAccumulate>> func, Expression<Func<TAccumulate, TResult>> selector, CancellationToken ct = default)
    //        => throw new NotImplementedException();
}
