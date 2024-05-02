namespace DotNetToolbox.Data.Repositories.ValueObject;

public interface IValueObjectRepository<TItem>
    : IRepository<TItem>
    , IUpdatableValueObjectRepository<TItem> {

    #region Blocking

    TItem[] GetAll();
    TItem? Find(Expression<Func<TItem, bool>> predicate);

    #endregion

    #region Async

    ValueTask<TItem[]> GetAllAsync(CancellationToken ct = default);
    Task<TItem?> FindAsync(Expression<Func<TItem, bool>> predicate, CancellationToken ct = default);

    #endregion
}
