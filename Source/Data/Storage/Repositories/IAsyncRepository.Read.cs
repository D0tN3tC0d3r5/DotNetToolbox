namespace DotNetToolbox.Data.Repositories;
public partial interface IAsyncRepository<TItem> {
    Task<IReadOnlyList<TItem>> ToArrayAsync(CancellationToken ct = default);
    Task<IReadOnlyList<TResult>> ToArrayAsync<TResult>(Expression<Func<TItem, TResult>> mapping, CancellationToken ct = default);
    Task<IList<TItem>> ToListAsync(CancellationToken ct = default);
    Task<IList<TResult>> ToListAsync<TResult>(Expression<Func<TItem, TResult>> mapping, CancellationToken ct = default);
    Task<ISet<TItem>> ToHashSetAsync(CancellationToken ct = default);
    Task<ISet<TResult>> ToHashSetAsync<TResult>(Expression<Func<TItem, TResult>> mapping, CancellationToken ct = default);
    Task<TResultRepository> ToRepositoryAsync<TResultRepository, TResult>(Expression<Func<TItem, TResult>> mapping, CancellationToken ct = default)
        where TResultRepository : class, IRepository<TResult>;
    Task<IAsyncRepository<TResult>> ToRepositoryAsync<TResult>(Expression<Func<TItem, TResult>> mapping, CancellationToken ct = default);
    Task<IDictionary<TKey, TValue>> ToDictionaryAsync<TKey, TValue>(Func<TItem, TKey> selectKey, Func<TItem, TValue> selectValue, IEqualityComparer<TKey>? comparer = null, CancellationToken ct = default)
        where TKey : notnull;
    Task<TItem> FirstAsync(CancellationToken ct = default);
    Task<TItem> FirstAsync(Expression<Func<TItem, bool>> predicate, CancellationToken ct = default);
    Task<TItem?> FirstOrDefaultAsync(CancellationToken ct = default);
    Task<TItem> FirstOrDefaultAsync(TItem defaultValue, CancellationToken ct = default);
    Task<TItem?> FirstOrDefaultAsync(Expression<Func<TItem, bool>> predicate, CancellationToken ct = default);
    Task<TItem> FirstOrDefaultAsync(Expression<Func<TItem, bool>> predicate, TItem defaultValue, CancellationToken ct = default);
    Task<TItem> LastAsync(CancellationToken ct = default);
    Task<TItem> LastAsync(Expression<Func<TItem, bool>> predicate, CancellationToken ct = default);
    Task<TItem?> LastOrDefaultAsync(CancellationToken ct = default);
    Task<TItem> LastOrDefaultAsync(TItem defaultValue, CancellationToken ct = default);
    Task<TItem?> LastOrDefaultAsync(Expression<Func<TItem, bool>> predicate, CancellationToken ct = default);
    Task<TItem> LastOrDefaultAsync(Expression<Func<TItem, bool>> predicate, TItem defaultValue, CancellationToken ct = default);
    Task<TItem> SingleAsync(CancellationToken ct = default);
    Task<TItem> SingleAsync(Expression<Func<TItem, bool>> predicate, CancellationToken ct = default);
    Task<TItem?> SingleOrDefaultAsync(CancellationToken ct = default);
    Task<TItem> SingleOrDefaultAsync(TItem defaultValue, CancellationToken ct = default);
    Task<TItem?> SingleOrDefaultAsync(Expression<Func<TItem, bool>> predicate, CancellationToken ct = default);
    Task<TItem> SingleOrDefaultAsync(Expression<Func<TItem, bool>> predicate, TItem defaultValue, CancellationToken ct = default);
    Task<TItem> ElementAtAsync(int index, CancellationToken ct = default);
    Task<TItem> ElementAtAsync(Index index, CancellationToken ct = default);
    Task<TItem?> ElementAtOrDefault(int index, CancellationToken ct = default);
    Task<TItem?> ElementAtOrDefault(Index index, CancellationToken ct = default);
    Task<bool> ContainsAsync(TItem item, CancellationToken ct = default);
    Task<bool> ContainsAsync(TItem item, IEqualityComparer<TItem>? comparer, CancellationToken ct = default);
    Task<bool> SequenceEqualAsync(IEnumerable<TItem> source2, CancellationToken ct = default);
    Task<bool> SequenceEqualAsync(IEnumerable<TItem> source2, IEqualityComparer<TItem>? comparer, CancellationToken ct = default);
    Task<bool> AnyAsync(CancellationToken ct = default);
    Task<bool> AnyAsync(Expression<Func<TItem, bool>> predicate, CancellationToken ct = default);
    Task<bool> AllAsync(Expression<Func<TItem, bool>> predicate, CancellationToken ct = default);
    Task<int> CountAsync(CancellationToken ct = default);
    Task<int> CountAsync(Expression<Func<TItem, bool>> predicate, CancellationToken ct = default);
    Task<long> LongCountAsync(CancellationToken ct = default);
    Task<long> LongCountAsync(Expression<Func<TItem, bool>> predicate, CancellationToken ct = default);
    Task<TItem?> MinAsync(CancellationToken ct = default);
    Task<TResult?> MinAsync<TResult>(Expression<Func<TItem, TResult>> selector, CancellationToken ct = default);
    Task<TItem?> MinByAsync<TKey>(Expression<Func<TItem, TKey>> keySelector, CancellationToken ct = default);
    Task<TItem?> MinByAsync<TKey>(Expression<Func<TItem, TKey>> keySelector, IComparer<TItem>? comparer, CancellationToken ct = default);
    Task<TItem?> MaxAsync(CancellationToken ct = default);
    Task<TResult?> MaxAsync<TResult>(Expression<Func<TItem, TResult>> selector, CancellationToken ct = default);
    Task<TItem?> MaxByAsync<TKey>(Expression<Func<TItem, TKey>> keySelector, CancellationToken ct = default);
    Task<TItem?> MaxByAsync<TKey>(Expression<Func<TItem, TKey>> keySelector, IComparer<TItem>? comparer, CancellationToken ct = default);
    Task<int> SumAsync(Expression<Func<TItem, int>> selector, CancellationToken ct = default);
    Task<int?> SumAsync(Expression<Func<TItem, int?>> selector, CancellationToken ct = default);
    Task<long> SumAsync(Expression<Func<TItem, long>> selector, CancellationToken ct = default);
    Task<long?> SumAsync(Expression<Func<TItem, long?>> selector, CancellationToken ct = default);
    Task<float> SumAsync(Expression<Func<TItem, float>> selector, CancellationToken ct = default);
    Task<float?> SumAsync(Expression<Func<TItem, float?>> selector, CancellationToken ct = default);
    Task<double> SumAsync(Expression<Func<TItem, double>> selector, CancellationToken ct = default);
    Task<double?> SumAsync(Expression<Func<TItem, double?>> selector, CancellationToken ct = default);
    Task<decimal> SumAsync(Expression<Func<TItem, decimal>> selector, CancellationToken ct = default);
    Task<decimal?> SumAsync(Expression<Func<TItem, decimal?>> selector, CancellationToken ct = default);
    Task<double> AverageAsync(Expression<Func<TItem, int>> selector, CancellationToken ct = default);
    Task<double?> AverageAsync(Expression<Func<TItem, int?>> selector, CancellationToken ct = default);
    Task<float> AverageAsync(Expression<Func<TItem, float>> selector, CancellationToken ct = default);
    Task<float?> AverageAsync(Expression<Func<TItem, float?>> selector, CancellationToken ct = default);
    Task<double> AverageAsync(Expression<Func<TItem, long>> selector, CancellationToken ct = default);
    Task<double?> AverageAsync(Expression<Func<TItem, long?>> selector, CancellationToken ct = default);
    Task<double> AverageAsync(Expression<Func<TItem, double>> selector, CancellationToken ct = default);
    Task<double?> AverageAsync(Expression<Func<TItem, double?>> selector, CancellationToken ct = default);
    Task<decimal> AverageAsync(Expression<Func<TItem, decimal>> selector, CancellationToken ct = default);
    Task<decimal?> AverageAsync(Expression<Func<TItem, decimal?>> selector, CancellationToken ct = default);
    Task<TItem> AggregateAsync(Expression<Func<TItem, TItem, TItem>> func, CancellationToken ct = default);
    Task<TAccumulate> AggregateAsync<TAccumulate>(TAccumulate seed, Expression<Func<TAccumulate, TItem, TAccumulate>> func, CancellationToken ct = default);
    Task<TResult> AggregateAsync<TAccumulate, TResult>(TAccumulate seed, Expression<Func<TAccumulate, TItem, TAccumulate>> func, Expression<Func<TAccumulate, TResult>> selector, CancellationToken ct = default);
}
