namespace DotNetToolbox.Data.Repositories;

public class AsyncOrderedRepository<TItem>
    : AsyncRepository<TItem>,
      IAsyncOrderedQueryableRepository<TItem>
    where TItem : class {
    protected AsyncOrderedRepository(IStrategyFactory? provider = null)
        : base([], provider) {
    }

    protected AsyncOrderedRepository(IEnumerable<TItem> source, IStrategyFactory? provider = null)
        : base(source, provider) {
    }

    public AsyncOrderedRepository<TItem> ThenBy<TKey>(Expression<Func<TItem, TKey>> keySelector)
        => Strategy.ThenBy(keySelector);

    public AsyncOrderedRepository<TItem> ThenBy<TKey>(Expression<Func<TItem, TKey>> keySelector, IComparer<TKey>? comparer)
        => Strategy.ThenBy(keySelector, comparer);

    public AsyncOrderedRepository<TItem> ThenByDescending<TKey>(Expression<Func<TItem, TKey>> keySelector)
        => Strategy.ThenByDescending(keySelector);

    public AsyncOrderedRepository<TItem> ThenByDescending<TKey>(Expression<Func<TItem, TKey>> keySelector, IComparer<TKey>? comparer)
        => Strategy.ThenByDescending(keySelector, comparer);
}
