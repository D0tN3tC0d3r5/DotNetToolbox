namespace DotNetToolbox.Data.Repositories.Entity;

public interface IQueryableEntityRepository<TItem, TKey>
    : IQueryableRepository<TItem>
    where TItem : IEntity<TKey>
    where TKey : notnull {

#region Blocking

    TItem? FindByKey(TKey key);

#endregion

#region Async

    ValueTask<TItem?> FindByKeyAsync(TKey key, CancellationToken ct = default);

#endregion
}
