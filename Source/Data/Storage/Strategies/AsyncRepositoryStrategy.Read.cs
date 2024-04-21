namespace DotNetToolbox.Data.Strategies;

public abstract partial class AsyncRepositoryStrategy<TItem> {
    public virtual Task<IReadOnlyList<TItem>> ToArrayAsync(CancellationToken ct = default)
        => throw new NotImplementedException();

    public virtual Task<IReadOnlyList<TResult>> ToArrayAsync<TResult>(Expression<Func<TItem, TResult>> mapping, CancellationToken ct = default)
        => throw new NotImplementedException();

    public virtual Task<IList<TItem>> ToListAsync(CancellationToken ct = default)
        => throw new NotImplementedException();

    public virtual Task<IList<TResult>> ToListAsync<TResult>(Expression<Func<TItem, TResult>> mapping, CancellationToken ct = default)
        => throw new NotImplementedException();

    public virtual Task<ISet<TItem>> ToHashSetAsync(CancellationToken ct = default)
        => throw new NotImplementedException();

    public virtual Task<ISet<TResult>> ToHashSetAsync<TResult>(Expression<Func<TItem, TResult>> mapping, CancellationToken ct = default)
        => throw new NotImplementedException();

    public virtual Task<TResultRepository> ToRepositoryAsync<TResultRepository, TResult>(Expression<Func<TItem, TResult>> mapping, CancellationToken ct = default)
        where TResultRepository : class, IRepository<TResult>
        where TResult : class
        => throw new NotImplementedException();

    public virtual Task<IAsyncRepository<TResult>> ToRepositoryAsync<TResult>(Expression<Func<TItem, TResult>> mapping, CancellationToken ct = default)
        where TResult : class
        => throw new NotImplementedException();

    public virtual Task<IDictionary<TKey, TValue>> ToDictionaryAsync<TKey, TValue>(Func<TItem, TKey> selectKey, Func<TItem, TValue> selectValue, IEqualityComparer<TKey>? comparer = null, CancellationToken ct = default)
        where TKey : notnull
        => throw new NotImplementedException();

    public virtual Task<TItem> FirstAsync(CancellationToken ct = default)
        => throw new NotImplementedException();

    public virtual Task<TItem> FirstAsync(Expression<Func<TItem, bool>> predicate, CancellationToken ct = default)
        => throw new NotImplementedException();

    public virtual Task<TItem?> FirstOrDefaultAsync(CancellationToken ct = default)
        => throw new NotImplementedException();

    public virtual Task<TItem> FirstOrDefaultAsync(TItem defaultValue, CancellationToken ct = default)
        => throw new NotImplementedException();

    public virtual Task<TItem?> FirstOrDefaultAsync(Expression<Func<TItem, bool>> predicate, CancellationToken ct = default)
        => throw new NotImplementedException();

    public virtual Task<TItem> FirstOrDefaultAsync(Expression<Func<TItem, bool>> predicate, TItem defaultValue, CancellationToken ct = default)
        => throw new NotImplementedException();

    public virtual Task<TItem> LastAsync(CancellationToken ct = default)
        => throw new NotImplementedException();

    public virtual Task<TItem> LastAsync(Expression<Func<TItem, bool>> predicate, CancellationToken ct = default)
        => throw new NotImplementedException();

    public virtual Task<TItem?> LastOrDefaultAsync(CancellationToken ct = default)
        => throw new NotImplementedException();

    public virtual Task<TItem> LastOrDefaultAsync(TItem defaultValue, CancellationToken ct = default)
        => throw new NotImplementedException();

    public virtual Task<TItem?> LastOrDefaultAsync(Expression<Func<TItem, bool>> predicate, CancellationToken ct = default)
        => throw new NotImplementedException();

    public virtual Task<TItem> LastOrDefaultAsync(Expression<Func<TItem, bool>> predicate, TItem defaultValue, CancellationToken ct = default)
        => throw new NotImplementedException();

    public virtual Task<TItem> SingleAsync(CancellationToken ct = default)
        => throw new NotImplementedException();

    public virtual Task<TItem> SingleAsync(Expression<Func<TItem, bool>> predicate, CancellationToken ct = default)
        => throw new NotImplementedException();

    public virtual Task<TItem?> SingleOrDefaultAsync(CancellationToken ct = default)
        => throw new NotImplementedException();

    public virtual Task<TItem> SingleOrDefaultAsync(TItem defaultValue, CancellationToken ct = default)
        => throw new NotImplementedException();

    public virtual Task<TItem?> SingleOrDefaultAsync(Expression<Func<TItem, bool>> predicate, CancellationToken ct = default)
        => throw new NotImplementedException();

    public virtual Task<TItem> SingleOrDefaultAsync(Expression<Func<TItem, bool>> predicate, TItem defaultValue, CancellationToken ct = default)
        => throw new NotImplementedException();

    public virtual Task<TItem> ElementAtAsync(int index, CancellationToken ct = default)
        => throw new NotImplementedException();

    public virtual Task<TItem> ElementAtAsync(Index index, CancellationToken ct = default)
        => throw new NotImplementedException();

    public virtual Task<TItem?> ElementAtOrDefault(int index, CancellationToken ct = default)
        => throw new NotImplementedException();

    public virtual Task<TItem?> ElementAtOrDefault(Index index, CancellationToken ct = default)
        => throw new NotImplementedException();

    public virtual Task<bool> ContainsAsync(TItem item, CancellationToken ct = default)
        => throw new NotImplementedException();

    public virtual Task<bool> ContainsAsync(TItem item, IEqualityComparer<TItem>? comparer, CancellationToken ct = default)
        => throw new NotImplementedException();

    public virtual Task<bool> SequenceEqualAsync(IEnumerable<TItem> source2, CancellationToken ct = default)
        => throw new NotImplementedException();

    public virtual Task<bool> SequenceEqualAsync(IEnumerable<TItem> source2, IEqualityComparer<TItem>? comparer, CancellationToken ct = default)
        => throw new NotImplementedException();

    public virtual Task<bool> AnyAsync(CancellationToken ct = default)
        => throw new NotImplementedException();

    public virtual Task<bool> AnyAsync(Expression<Func<TItem, bool>> predicate, CancellationToken ct = default)
        => throw new NotImplementedException();

    public virtual Task<bool> AllAsync(Expression<Func<TItem, bool>> predicate, CancellationToken ct = default)
        => throw new NotImplementedException();

    public virtual Task<int> CountAsync(CancellationToken ct = default)
        => throw new NotImplementedException();

    public virtual Task<int> CountAsync(Expression<Func<TItem, bool>> predicate, CancellationToken ct = default)
        => throw new NotImplementedException();

    public virtual Task<long> LongCountAsync(CancellationToken ct = default)
        => throw new NotImplementedException();

    public virtual Task<long> LongCountAsync(Expression<Func<TItem, bool>> predicate, CancellationToken ct = default)
        => throw new NotImplementedException();

    public virtual Task<TItem?> MinAsync(CancellationToken ct = default)
        => throw new NotImplementedException();

    public virtual Task<TResult?> MinAsync<TResult>(Expression<Func<TItem, TResult>> selector, CancellationToken ct = default)
        => throw new NotImplementedException();

    public virtual Task<TItem?> MinByAsync<TKey>(Expression<Func<TItem, TKey>> keySelector, CancellationToken ct = default)
        => throw new NotImplementedException();

    public virtual Task<TItem?> MinByAsync<TKey>(Expression<Func<TItem, TKey>> keySelector, IComparer<TItem>? comparer, CancellationToken ct = default)
        => throw new NotImplementedException();

    public virtual Task<TItem?> MaxAsync(CancellationToken ct = default)
        => throw new NotImplementedException();

    public virtual Task<TResult?> MaxAsync<TResult>(Expression<Func<TItem, TResult>> selector, CancellationToken ct = default)
        => throw new NotImplementedException();

    public virtual Task<TItem?> MaxByAsync<TKey>(Expression<Func<TItem, TKey>> keySelector, CancellationToken ct = default)
        => throw new NotImplementedException();

    public virtual Task<TItem?> MaxByAsync<TKey>(Expression<Func<TItem, TKey>> keySelector, IComparer<TItem>? comparer, CancellationToken ct = default)
        => throw new NotImplementedException();

    public virtual Task<int> SumAsync(Expression<Func<TItem, int>> selector, CancellationToken ct = default)
        => throw new NotImplementedException();

    public virtual Task<int?> SumAsync(Expression<Func<TItem, int?>> selector, CancellationToken ct = default)
        => throw new NotImplementedException();

    public virtual Task<long> SumAsync(Expression<Func<TItem, long>> selector, CancellationToken ct = default)
        => throw new NotImplementedException();

    public virtual Task<long?> SumAsync(Expression<Func<TItem, long?>> selector, CancellationToken ct = default)
        => throw new NotImplementedException();

    public virtual Task<float> SumAsync(Expression<Func<TItem, float>> selector, CancellationToken ct = default)
        => throw new NotImplementedException();

    public virtual Task<float?> SumAsync(Expression<Func<TItem, float?>> selector, CancellationToken ct = default)
        => throw new NotImplementedException();

    public virtual Task<double> SumAsync(Expression<Func<TItem, double>> selector, CancellationToken ct = default)
        => throw new NotImplementedException();

    public virtual Task<double?> SumAsync(Expression<Func<TItem, double?>> selector, CancellationToken ct = default)
        => throw new NotImplementedException();

    public virtual Task<decimal> SumAsync(Expression<Func<TItem, decimal>> selector, CancellationToken ct = default)
        => throw new NotImplementedException();

    public virtual Task<decimal?> SumAsync(Expression<Func<TItem, decimal?>> selector, CancellationToken ct = default)
        => throw new NotImplementedException();

    public virtual Task<double> AverageAsync(Expression<Func<TItem, int>> selector, CancellationToken ct = default)
        => throw new NotImplementedException();

    public virtual Task<double?> AverageAsync(Expression<Func<TItem, int?>> selector, CancellationToken ct = default)
        => throw new NotImplementedException();

    public virtual Task<float> AverageAsync(Expression<Func<TItem, float>> selector, CancellationToken ct = default)
        => throw new NotImplementedException();

    public virtual Task<float?> AverageAsync(Expression<Func<TItem, float?>> selector, CancellationToken ct = default)
        => throw new NotImplementedException();

    public virtual Task<double> AverageAsync(Expression<Func<TItem, long>> selector, CancellationToken ct = default)
        => throw new NotImplementedException();

    public virtual Task<double?> AverageAsync(Expression<Func<TItem, long?>> selector, CancellationToken ct = default)
        => throw new NotImplementedException();

    public virtual Task<double> AverageAsync(Expression<Func<TItem, double>> selector, CancellationToken ct = default)
        => throw new NotImplementedException();

    public virtual Task<double?> AverageAsync(Expression<Func<TItem, double?>> selector, CancellationToken ct = default)
        => throw new NotImplementedException();

    public virtual Task<decimal> AverageAsync(Expression<Func<TItem, decimal>> selector, CancellationToken ct = default)
        => throw new NotImplementedException();

    public virtual Task<decimal?> AverageAsync(Expression<Func<TItem, decimal?>> selector, CancellationToken ct = default)
        => throw new NotImplementedException();

    public virtual Task<TItem> AggregateAsync(Expression<Func<TItem, TItem, TItem>> func, CancellationToken ct = default)
        => throw new NotImplementedException();

    public virtual Task<TAccumulate> AggregateAsync<TAccumulate>(TAccumulate seed, Expression<Func<TAccumulate, TItem, TAccumulate>> func, CancellationToken ct = default)
        => throw new NotImplementedException();

    public virtual Task<TResult> AggregateAsync<TAccumulate, TResult>(TAccumulate seed, Expression<Func<TAccumulate, TItem, TAccumulate>> func, Expression<Func<TAccumulate, TResult>> selector, CancellationToken ct = default)
        => throw new NotImplementedException();
}
