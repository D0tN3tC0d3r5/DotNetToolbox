namespace Sophia.WebClient.Services;

public class ProvidersRemoteService(HttpClient httpClient)
    : IProvidersRemoteService {
    public async Task<IReadOnlyList<ProviderData>> GetList(string? filter = null) {
        var list = await httpClient.GetFromJsonAsync<ProviderData[]>("api/providers");
        return list!;
    }

    public async Task<ProviderData?> GetById(int id) {
        var provider = await httpClient.GetFromJsonAsync<ProviderData>($"api/providers/{id}");
        return provider;
    }

    public async Task Add(ProviderData input) {
        var response = await httpClient.PostAsJsonAsync("api/providers", input)!;
        response.EnsureSuccessStatusCode();
    }

    public async Task Update(ProviderData input) {
        var response = await httpClient.PutAsJsonAsync("api/providers", input)!;
        response.EnsureSuccessStatusCode();
    }

    public async Task Delete(int id) {
        var response = await httpClient.DeleteAsync($"api/providers/{id}")!;
        response.EnsureSuccessStatusCode();
    }
}
