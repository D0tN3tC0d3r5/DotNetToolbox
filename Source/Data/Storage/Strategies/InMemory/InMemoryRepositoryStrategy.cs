namespace DotNetToolbox.Data.Strategies.InMemory;

public class InMemoryRepositoryStrategy<TItem>(IEnumerable<TItem>? data = null)
    : AsyncRepositoryStrategy<TItem>(data ?? []) {
    public override void Seed(IEnumerable<TItem> seed) {
        OriginalData = seed.ToList();
        Query = new EnumerableQuery<TItem>(OriginalData);
    }

    public override void Add(TItem newItem) {
        UpdatableData.Add(newItem);
        Query = UpdatableData.AsQueryable();
    }

    public override void Update(Expression<Func<TItem, bool>> predicate, TItem updatedItem) {
        if (!TryRemove(predicate))
            return;
        Add(updatedItem);
    }

    public override void Remove(Expression<Func<TItem, bool>> predicate)
        => TryRemove(predicate);

    private bool TryRemove(Expression<Func<TItem, bool>> predicate) {
        var itemToRemove = Query.FirstOrDefault(predicate);
        if (itemToRemove is null)
            return false;
        UpdatableData.Remove(itemToRemove);
        Query = UpdatableData.AsQueryable();
        return true;
    }
}
