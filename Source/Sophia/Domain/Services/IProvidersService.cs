namespace Sophia.Services;

public interface IProvidersService {
    Task<IReadOnlyList<ProviderData>> GetList(string? filter = null);
    Task<ProviderData?> GetById(int id);
    Task Add(ProviderData input);
    Task Update(ProviderData input);
    Task Delete(int id);
}
