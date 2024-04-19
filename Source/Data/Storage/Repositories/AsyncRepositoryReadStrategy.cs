namespace DotNetToolbox.Data.Repositories;

public abstract class AsyncRepositoryReadStrategy<TItem>
    : AsyncRepositoryQueryStrategy<TItem>,
      IAsyncReadOnlyRepository<TItem>
    where TItem : class {
    public virtual Task<IReadOnlyList<TItem>> ToArray(CancellationToken ct = default)
        => throw new NotImplementedException();

    public virtual Task<IReadOnlyList<TItem>> ToArray<TResult>(Expression<Func<TItem, TResult>> mapping, CancellationToken ct = default)
        => throw new NotImplementedException();

    public virtual Task<IList<TItem>> ToList(CancellationToken ct = default)
        => throw new NotImplementedException();

    public virtual Task<IList<TResult>> ToList<TResult>(Expression<Func<TItem, TResult>> mapping, CancellationToken ct = default)
        => throw new NotImplementedException();

    public virtual Task<ISet<TItem>> ToHashSet(CancellationToken ct = default)
        => throw new NotImplementedException();

    public virtual Task<ISet<TResult>> ToHashSet<TResult>(Expression<Func<TItem, TResult>> mapping, CancellationToken ct = default)
        => throw new NotImplementedException();

    public virtual Task<IRepository<TResult>> ToRepository<TResult>(Expression<Func<TItem, TResult>> mapping, CancellationToken ct = default)
        where TResult : class
        => throw new NotImplementedException();

    public virtual Task<IDictionary<TKey, TValue>> ToDictionary<TKey, TValue>(Expression<Func<TItem, TKey>> selectKey, Expression<Func<TItem, TValue>> selectValue, CancellationToken ct = default)
        where TKey : notnull
        => throw new NotImplementedException();

    public virtual Task<TItem> First(CancellationToken ct = default)
        => throw new NotImplementedException();

    public virtual Task<TItem> First(Expression<Func<TItem, bool>> predicate, CancellationToken ct = default)
        => throw new NotImplementedException();

    public virtual Task<TItem> FirstOrDefault(CancellationToken ct = default)
        => throw new NotImplementedException();

    public virtual Task<TItem> FirstOrDefault(TItem defaultValue, CancellationToken ct = default)
        => throw new NotImplementedException();

    public virtual Task<TItem> FirstOrDefault(Expression<Func<TItem, bool>> predicate, CancellationToken ct = default)
        => throw new NotImplementedException();

    public virtual Task<TItem> FirstOrDefault(Expression<Func<TItem, bool>> predicate, TItem defaultValue, CancellationToken ct = default)
        => throw new NotImplementedException();

    public virtual Task<TItem> Last(CancellationToken ct = default)
        => throw new NotImplementedException();

    public virtual Task<TItem> Last(Expression<Func<TItem, bool>> predicate, CancellationToken ct = default)
        => throw new NotImplementedException();

    public virtual Task<TItem> LastOrDefault(CancellationToken ct = default)
        => throw new NotImplementedException();

    public virtual Task<TItem> LastOrDefault(TItem defaultValue, CancellationToken ct = default)
        => throw new NotImplementedException();

    public virtual Task<TItem> LastOrDefault(Expression<Func<TItem, bool>> predicate, CancellationToken ct = default)
        => throw new NotImplementedException();

    public virtual Task<TItem> LastOrDefault(Expression<Func<TItem, bool>> predicate, TItem defaultValue, CancellationToken ct = default)
        => throw new NotImplementedException();

    public virtual Task<TItem> Single(CancellationToken ct = default)
        => throw new NotImplementedException();

    public virtual Task<TItem> Single(Expression<Func<TItem, bool>> predicate, CancellationToken ct = default)
        => throw new NotImplementedException();

    public virtual Task<TItem> SingleOrDefault(CancellationToken ct = default)
        => throw new NotImplementedException();

    public virtual Task<TItem> SingleOrDefault(TItem defaultValue, CancellationToken ct = default)
        => throw new NotImplementedException();

    public virtual Task<TItem> SingleOrDefault(Expression<Func<TItem, bool>> predicate, CancellationToken ct = default)
        => throw new NotImplementedException();

    public virtual Task<TItem> SingleOrDefault(Expression<Func<TItem, bool>> predicate, TItem defaultValue, CancellationToken ct = default)
        => throw new NotImplementedException();

    public virtual Task<TItem> ElementAt(int index, CancellationToken ct = default)
        => throw new NotImplementedException();

    public virtual Task<TItem> ElementAt(Index index, CancellationToken ct = default)
        => throw new NotImplementedException();

    public virtual Task<TItem> ElementAtOrDefault(int index, CancellationToken ct = default)
        => throw new NotImplementedException();

    public virtual Task<TItem> ElementAtOrDefault(Index index, CancellationToken ct = default)
        => throw new NotImplementedException();

    public virtual Task<bool> Contains(TItem item, CancellationToken ct = default)
        => throw new NotImplementedException();

    public virtual Task<bool> Contains(TItem item, IEqualityComparer<TItem>? comparer, CancellationToken ct = default)
        => throw new NotImplementedException();

    public virtual Task<bool> SequenceEqual(IEnumerable<TItem> source2, CancellationToken ct = default)
        => throw new NotImplementedException();

    public virtual Task<bool> SequenceEqual(IEnumerable<TItem> source2, IEqualityComparer<TItem>? comparer, CancellationToken ct = default)
        => throw new NotImplementedException();

    public virtual Task<bool> Any(CancellationToken ct = default)
        => throw new NotImplementedException();

    public virtual Task<bool> Any(Expression<Func<TItem, bool>> predicate, CancellationToken ct = default)
        => throw new NotImplementedException();

    public virtual Task<bool> All(Expression<Func<TItem, bool>> predicate, CancellationToken ct = default)
        => throw new NotImplementedException();

    public virtual Task<int?> Count(CancellationToken ct = default)
        => throw new NotImplementedException();

    public virtual Task<int?> Count(Expression<Func<TItem, bool>> predicate, CancellationToken ct = default)
        => throw new NotImplementedException();

    public virtual Task<long?> LongCount(CancellationToken ct = default)
        => throw new NotImplementedException();

    public virtual Task<long?> LongCount(Expression<Func<TItem, bool>> predicate, CancellationToken ct = default)
        => throw new NotImplementedException();

    public virtual Task<TItem> Min(CancellationToken ct = default)
        => throw new NotImplementedException();

    public virtual Task<TResult> Min<TResult>(Expression<Func<TItem, TResult>> selector, CancellationToken ct = default)
        => throw new NotImplementedException();

    public virtual Task<TItem> MinBy<TKey>(Expression<Func<TItem, TKey>> keySelector, CancellationToken ct = default)
        => throw new NotImplementedException();

    public virtual Task<TItem> MinBy<TKey>(Expression<Func<TItem, TKey>> keySelector, IComparer<TItem>? comparer, CancellationToken ct = default)
        => throw new NotImplementedException();

    public virtual Task<TItem> Max(CancellationToken ct = default)
        => throw new NotImplementedException();

    public virtual Task<TResult> Max<TResult>(Expression<Func<TItem, TResult>> selector, CancellationToken ct = default)
        => throw new NotImplementedException();

    public virtual Task<TItem> MaxBy<TKey>(Expression<Func<TItem, TKey>> keySelector, CancellationToken ct = default)
        => throw new NotImplementedException();

    public virtual Task<TItem> MaxBy<TKey>(Expression<Func<TItem, TKey>> keySelector, IComparer<TItem>? comparer, CancellationToken ct = default)
        => throw new NotImplementedException();

    public virtual Task<int?> Sum(Expression<Func<TItem, int>> selector, CancellationToken ct = default)
        => throw new NotImplementedException();

    public virtual Task<int?> Sum(Expression<Func<TItem, int?>> selector, CancellationToken ct = default)
        => throw new NotImplementedException();

    public virtual Task<long?> Sum(Expression<Func<TItem, long>> selector, CancellationToken ct = default)
        => throw new NotImplementedException();

    public virtual Task<long?> Sum(Expression<Func<TItem, long?>> selector, CancellationToken ct = default)
        => throw new NotImplementedException();

    public virtual Task<float> Sum(Expression<Func<TItem, float>> selector, CancellationToken ct = default)
        => throw new NotImplementedException();

    public virtual Task<float?> Sum(Expression<Func<TItem, float?>> selector, CancellationToken ct = default)
        => throw new NotImplementedException();

    public virtual Task<double> Sum(Expression<Func<TItem, double>> selector, CancellationToken ct = default)
        => throw new NotImplementedException();

    public virtual Task<double?> Sum(Expression<Func<TItem, double?>> selector, CancellationToken ct = default)
        => throw new NotImplementedException();

    public virtual Task<decimal> Sum(Expression<Func<TItem, decimal>> selector, CancellationToken ct = default)
        => throw new NotImplementedException();

    public virtual Task<decimal?> Sum(Expression<Func<TItem, decimal?>> selector, CancellationToken ct = default)
        => throw new NotImplementedException();

    public virtual Task<double> Average(Expression<Func<TItem, int>> selector, CancellationToken ct = default)
        => throw new NotImplementedException();

    public virtual Task<double?> Average(Expression<Func<TItem, int?>> selector, CancellationToken ct = default)
        => throw new NotImplementedException();

    public virtual Task<float> Average(Expression<Func<TItem, float>> selector, CancellationToken ct = default)
        => throw new NotImplementedException();

    public virtual Task<float?> Average(Expression<Func<TItem, float?>> selector, CancellationToken ct = default)
        => throw new NotImplementedException();

    public virtual Task<double> Average(Expression<Func<TItem, long>> selector, CancellationToken ct = default)
        => throw new NotImplementedException();

    public virtual Task<double?> Average(Expression<Func<TItem, long?>> selector, CancellationToken ct = default)
        => throw new NotImplementedException();

    public virtual Task<double> Average(Expression<Func<TItem, double>> selector, CancellationToken ct = default)
        => throw new NotImplementedException();

    public virtual Task<double?> Average(Expression<Func<TItem, double?>> selector, CancellationToken ct = default)
        => throw new NotImplementedException();

    public virtual Task<decimal> Average(Expression<Func<TItem, decimal>> selector, CancellationToken ct = default)
        => throw new NotImplementedException();

    public virtual Task<decimal?> Average(Expression<Func<TItem, decimal?>> selector, CancellationToken ct = default)
        => throw new NotImplementedException();

    public virtual Task<TItem> Aggregate(Expression<Func<TItem, TItem, TItem>> func, CancellationToken ct = default)
        => throw new NotImplementedException();

    public virtual Task<TAccumulate> Aggregate<TAccumulate>(TAccumulate seed, Expression<Func<TAccumulate, TItem, TAccumulate>> func, CancellationToken ct = default)
        => throw new NotImplementedException();

    public virtual Task<TResult> Aggregate<TAccumulate, TResult>(TAccumulate seed, Expression<Func<TAccumulate, TItem, TAccumulate>> func, Expression<Func<TAccumulate, TResult>> selector, CancellationToken ct = default)
        => throw new NotImplementedException();
}
