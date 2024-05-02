namespace DotNetToolbox.Data.Repositories;

public interface IRepository
    : IQueryableRepository
    , IUpdatableRepository;

public interface IRepository<TItem>
    : IRepository
    , IQueryableRepository<TItem> {

    #region Blocking

    void Seed(IEnumerable<TItem> seed);

    #endregion

    #region Async

    Task SeedAsync(IEnumerable<TItem> seed, CancellationToken ct = default);

    #endregion
}
