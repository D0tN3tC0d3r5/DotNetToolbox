namespace DotNetToolbox.Data.Repositories;

public interface IRepository
    : IQueryableRepository
    , IUpdatableRepository;

public interface IRepository<TItem>
    : IRepository
    , IQueryableRepository<TItem> {

    string Name { get; }

    #region Blocking

    void Seed(IEnumerable<TItem> seed);
    void Load();

    #endregion

    #region Async

    Task SeedAsync(IEnumerable<TItem> seed, CancellationToken ct = default);
    Task LoadAsync(CancellationToken ct = default);

    #endregion
}
