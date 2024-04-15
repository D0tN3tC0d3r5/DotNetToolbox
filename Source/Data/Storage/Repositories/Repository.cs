namespace DotNetToolbox.Data.Repositories;

public class Repository<TItem>(IEnumerable<TItem> source, IStrategyProvider? provider = null)
    : ReadOnlyRepository<TItem>(source, provider),
      IRepository<TItem>
    where TItem : class, new() {
    public Repository(IStrategyProvider? provider = null)
        : this([], provider) {
    }

    public void Add(TItem newItem)
        => Strategy.Add(newItem);
    public void Update(Expression<Func<TItem, bool>> predicate, TItem updatedItem)
        => Strategy.Update(predicate, updatedItem);
    public void Remove(Expression<Func<TItem, bool>> predicate)
        => Strategy.Remove(predicate);
}
