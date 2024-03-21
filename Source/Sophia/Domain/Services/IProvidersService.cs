namespace Sophia.Services;

public interface IProvidersRemoteService : IProvidersService;

public interface IProvidersService {
    Task<IReadOnlyList<ProviderData>> GetList(string? filter = null);
    Task<ProviderData?> GetById(int id);
    Task Add(ProviderData provider);
    Task Update(ProviderData provider);
    Task Delete(int id);
}
