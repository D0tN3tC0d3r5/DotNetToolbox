namespace DotNetToolbox.Data.Repositories;

public interface IQueryableRepository;

public interface IQueryableRepository<TItem>
    : IQueryableRepository {

    #region Blocking

    void Load();

    TItem[] GetAll();
    TItem? Find(Expression<Func<TItem, bool>> predicate);

    #endregion

    #region Async

    Task LoadAsync(CancellationToken ct = default);

    ValueTask<TItem[]> GetAllAsync(CancellationToken ct = default);
    ValueTask<TItem?> FindAsync(Expression<Func<TItem, bool>> predicate, CancellationToken ct = default);

    #endregion
}

