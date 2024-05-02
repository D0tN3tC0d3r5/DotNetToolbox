namespace DotNetToolbox.Data.Strategies.Entity;

public abstract class EntityRepositoryStrategy<TItem, TKey>
    : ValueObjectRepositoryStrategy<TItem>
    , IEntityRepositoryStrategy<TItem, TKey>
    where TItem : IEntity<TKey>
    where TKey : notnull {

    #region Blocking

    public virtual TItem FindByKey(TKey key) => throw new NotImplementedException();

    public virtual TKey GetNextKey(IReadOnlyDictionary<object, object?>? keyContext = null) => throw new NotImplementedException();
    public virtual void Update(TItem updatedItem) => throw new NotImplementedException();
    public virtual void Patch(TKey key, Action<TItem> setItem) => throw new NotImplementedException();
    public virtual void Remove(TKey key) => throw new NotImplementedException();

    #endregion

    #region Async
    public virtual Task<TItem?> FindByKeyAsync(TKey key, CancellationToken ct = default) => throw new NotImplementedException();

    public virtual Task<TKey> GetNextKeyAsync(IReadOnlyDictionary<object, object?>? keyContext = null, CancellationToken ct = default) => throw new NotImplementedException();
    public virtual Task UpdateAsync(TItem updatedItem, CancellationToken ct = default) => throw new NotImplementedException();
    public virtual Task PatchAsync(TKey key, Func<TItem, CancellationToken, Task> setItem, CancellationToken ct = default) => throw new NotImplementedException();
    public virtual Task RemoveAsync(TKey key, CancellationToken ct = default) => throw new NotImplementedException();

    #endregion

}
