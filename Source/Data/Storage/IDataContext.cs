namespace DotNetToolbox.Data;

public interface IDataContext {
    #region Blocking

    void EnsureExists();
    void Seed();
    void EnsureIsUpToDate();
    void SaveChanges();

    #endregion

    #region Async

    Task EnsureExistsAsync();
    Task SeedAsync(CancellationToken ct = default);
    Task EnsureIsUpToDateAsync(CancellationToken ct = default);
    Task SaveChangesAsync(CancellationToken ct = default);

    #endregion
}
