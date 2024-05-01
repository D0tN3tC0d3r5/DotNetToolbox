namespace DotNetToolbox.Data.Repositories;

public interface IRepository
    : IQueryableRepository
    , IUpdatableRepository;

public interface IRepository<TItem>
    : IQueryableRepository<TItem>
    , IUpdatableRepository {

    #region Blocking

    void Seed(IEnumerable<TItem> seed);

    #endregion

    #region Async

    Task SeedAsync(IEnumerable<TItem> seed, CancellationToken ct = default);

    #endregion
}
