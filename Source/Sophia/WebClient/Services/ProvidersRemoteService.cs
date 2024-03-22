namespace Sophia.WebClient.Services;

public class ProvidersRemoteService(HttpClient httpClient)
    : IProvidersRemoteService {

    private static readonly UrlEncoder _encoder = UrlEncoder.Create();

    public async Task<IReadOnlyList<ProviderData>> GetList(string? filter = null) {
        var query = filter is null ? string.Empty : $"?filter={_encoder.Encode(filter)}";
        var list = await httpClient.GetFromJsonAsync<ProviderData[]>($"api/providers{query}");
        return list!;
    }

    public async Task<ProviderData?> GetById(int id) {
        var provider = await httpClient.GetFromJsonAsync<ProviderData>($"api/providers/{id}");
        return provider;
    }

    public async Task Add(ProviderData provider) {
        var response = await httpClient.PostAsJsonAsync("api/providers", provider)!;
        response.EnsureSuccessStatusCode();
        var result = await response.Content.ReadFromJsonAsync<ProviderData>();
        provider.Id = result!.Id;
    }

    public async Task Update(ProviderData provider) {
        var response = await httpClient.PutAsJsonAsync("api/providers", provider)!;
        response.EnsureSuccessStatusCode();
    }

    public async Task Delete(int id) {
        var response = await httpClient.DeleteAsync($"api/providers/{id}")!;
        response.EnsureSuccessStatusCode();
    }
}
