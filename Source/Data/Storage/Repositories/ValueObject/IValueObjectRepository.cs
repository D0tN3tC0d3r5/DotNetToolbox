namespace DotNetToolbox.Data.Repositories.ValueObject;

public interface IValueObjectRepository<TItem>
    : IRepository<TItem>
    , IUpdatableValueObjectRepository<TItem> {

    #region Blocking

    IReadOnlyList<TItem> GetAll();
    TItem? Find(Expression<Func<TItem, bool>> predicate);

    #endregion

    #region Async

    Task<IReadOnlyList<TItem>> GetAllAsync(CancellationToken ct = default);
    Task<TItem?> FindAsync(Expression<Func<TItem, bool>> predicate, CancellationToken ct = default);

    #endregion
}
