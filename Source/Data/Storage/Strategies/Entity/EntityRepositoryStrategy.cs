using DotNetToolbox.Data.Strategies.Key;

namespace DotNetToolbox.Data.Strategies.Entity;

public abstract class EntityRepositoryStrategy<TItem, TKey, TKeyHandler>
    : RepositoryStrategy<TItem>
    , IEntityRepositoryStrategy<TItem, TKey, TKeyHandler>
    where TItem : IEntity<TKey>
    where TKeyHandler : class, IKeyHandler<TKey>, IHasDefault<TKeyHandler>
    where TKey : notnull {

    public IEntityRepository<TItem, TKey, TKeyHandler> Repository { get; set; } = default!;
    public TKeyHandler KeyHandler { get; set; } = TKeyHandler.Default;

    #region Blocking

    public virtual TItem? FindByKey(TKey key) => throw new NotImplementedException();

    public virtual void Update(TItem updatedItem) => throw new NotImplementedException();
    public virtual void Patch(TKey key, Action<TItem> setItem) => throw new NotImplementedException();
    public virtual void Remove(TKey key) => throw new NotImplementedException();

    #endregion

    #region Async
    public virtual ValueTask<TItem?> FindByKeyAsync(TKey key, CancellationToken ct = default) => throw new NotImplementedException();

    public virtual Task UpdateAsync(TItem updatedItem, CancellationToken ct = default) => throw new NotImplementedException();
    public virtual Task PatchAsync(TKey key, Func<TItem, CancellationToken, Task> setItem, CancellationToken ct = default) => throw new NotImplementedException();
    public virtual Task RemoveAsync(TKey key, CancellationToken ct = default) => throw new NotImplementedException();

    #endregion
}
