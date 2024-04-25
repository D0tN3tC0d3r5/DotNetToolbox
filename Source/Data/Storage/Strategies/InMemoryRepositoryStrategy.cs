namespace DotNetToolbox.Data.Strategies;

public class InMemoryRepositoryStrategy<TItem>
    : RepositoryStrategy<TItem> {

    public InMemoryRepositoryStrategy() { }
    public InMemoryRepositoryStrategy(IEnumerable<TItem> data)
        : base(data) { }

    public override void Seed(IEnumerable<TItem> seed) {
        OriginalData = seed.ToList();
        Query = new EnumerableQuery<TItem>(OriginalData);
    }

    public override void Add(TItem newItem) {
        UpdatableData.Add(newItem);
        Query = UpdatableData.AsQueryable();
    }

    public override void Update(Expression<Func<TItem, bool>> predicate, TItem updatedItem) {
        Remove(predicate);
        Add(updatedItem);
    }
    public override void Remove(Expression<Func<TItem, bool>> predicate) {
        var itemToRemove = Query.FirstOrDefault(predicate);
        if (itemToRemove is null)
            return;
        UpdatableData.Remove(itemToRemove);
        Query = UpdatableData.AsQueryable();
    }
}
