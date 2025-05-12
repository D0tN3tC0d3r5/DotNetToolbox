namespace DotNetToolbox.Data.DataSources;

public interface IUpdatableDataSource;

public interface IUpdatableDataSource<TItem, in TKey>
    : IUpdatableDataSource
    where TItem : IEntity<TKey>
    where TKey : notnull {
    Result<TItem> Create(Action<TItem>? setItem = null, IMap? validationContext = null);

    Result Seed(IEnumerable<TItem> seed, bool preserveContent = false, IMap? validationContext = null);

    Result Add(TItem newItem, IMap? validationContext = null);
    Result AddMany(IEnumerable<TItem> newItems, IMap? validationContext = null);

    Result Update(Expression<Func<TItem, bool>> predicate, TItem updatedItem, IMap? validationContext = null);
    Result UpdateMany(Expression<Func<TItem, bool>> predicate, IEnumerable<TItem> updatedItems, IMap? validationContext = null);

    Result AddOrUpdate(Expression<Func<TItem, bool>> predicate, TItem updatedItem, IMap? validationContext = null);
    Result AddOrUpdateMany(Expression<Func<TItem, bool>> predicate, IEnumerable<TItem> items, IMap? validationContext = null);

    Result Patch(Expression<Func<TItem, bool>> predicate, Action<TItem> setItem, IMap? validationContext = null);
    Result PatchMany(Expression<Func<TItem, bool>> predicate, Action<TItem> setItem, IMap? validationContext = null);

    Result Remove(Expression<Func<TItem, bool>> predicate);
    Result RemoveMany(Expression<Func<TItem, bool>> predicate);

    Result Clear();

    Result Update(TItem updatedItem, IMap? validationContext = null);
    Result UpdateMany(IEnumerable<TItem> updatedItems, IMap? validationContext = null);

    Result AddOrUpdate(TItem updatedItem, IMap? validationContext = null);
    Result AddOrUpdateMany(IEnumerable<TItem> updatedItems, IMap? validationContext = null);

    Result Patch(TKey key, Action<TItem> setItem, IMap? validationContext = null);
    Result PatchMany(IEnumerable<TKey> keys, Action<TItem> setItem, IMap? validationContext = null);

    Result Remove(TKey key);
    Result RemoveMany(IEnumerable<TKey> keys);
}
