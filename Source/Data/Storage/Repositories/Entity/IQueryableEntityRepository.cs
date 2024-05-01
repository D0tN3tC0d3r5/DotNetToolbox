namespace DotNetToolbox.Data.Repositories.Entity;

public interface IQueryableEntityRepository<TItem, in TKey>
    : IQueryableRepository<TItem>
    where TItem : IEntity<TKey>
    where TKey : notnull {

#region Blocking

    TItem? FindByKey(TKey key);

#endregion

#region Async

    Task<TItem?> FindByKeyAsync(TKey key, CancellationToken ct = default);

#endregion
}
