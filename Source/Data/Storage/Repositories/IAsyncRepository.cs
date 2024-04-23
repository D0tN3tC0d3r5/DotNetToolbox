namespace DotNetToolbox.Data.Repositories;

public partial interface IAsyncRepository;

public partial interface IAsyncRepository<TItem>
    : IAsyncRepository {
    Task SeedAsync(IEnumerable<TItem> seed);
}
