namespace DotNetToolbox.Data.Repositories;

public class OrderedRepository<TRepository, TItem>
    : Repository<TRepository, TItem>
    , IOrderedRepository<TItem>
    where TRepository : OrderedRepository<TRepository, TItem>
    where TItem : class {

    public OrderedRepository(IStrategyFactory? provider = null)
        : base([], provider) {
    }

    public OrderedRepository(IEnumerable<TItem> data, IStrategyFactory? provider = null)
        : base(data, provider) {
    }

    public IOrderedRepository<TItem> ThenBy<TKey>(Expression<Func<TItem, TKey>> keySelector)
        => Strategy.ThenBy(keySelector);

    public IOrderedRepository<TItem> ThenBy<TKey>(Expression<Func<TItem, TKey>> keySelector, IComparer<TKey>? comparer)
        => Strategy.ThenBy(keySelector, comparer);

    public IOrderedRepository<TItem> ThenByDescending<TKey>(Expression<Func<TItem, TKey>> keySelector)
        => Strategy.ThenByDescending(keySelector);

    public IOrderedRepository<TItem> ThenByDescending<TKey>(Expression<Func<TItem, TKey>> keySelector, IComparer<TKey>? comparer)
        => Strategy.ThenByDescending(keySelector, comparer);
}
