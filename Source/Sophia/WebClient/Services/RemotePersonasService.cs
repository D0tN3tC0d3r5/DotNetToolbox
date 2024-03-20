namespace Sophia.WebClient.Services;

public class RemotePersonasService : IPersonasService {
    private readonly HttpClient _httpClient;

    public RemotePersonasService(HttpClient httpClient) {
        _httpClient = httpClient;
    }

    public async Task<IReadOnlyList<PersonaData>> GetList(string? filter = null) {
        var list = await _httpClient.GetFromJsonAsync<List<PersonaData>>("api/personas");
        return list!;
    }

    public async Task<PersonaData?> GetById(int id) {
        var persona = await _httpClient.GetFromJsonAsync<PersonaData>($"api/personas/{id}");
        return persona;
    }

    public async Task Add(PersonaData persona) {
        var response = await _httpClient.PostAsJsonAsync("api/personas", persona);
        response.EnsureSuccessStatusCode();
    }

    public async Task Update(PersonaData persona) {
        var response = await _httpClient.PutAsJsonAsync($"api/personas", persona);
        response.EnsureSuccessStatusCode();
    }

    public async Task Delete(int id) {
        var response = await _httpClient.DeleteAsync($"api/personas/{id}");
        response.EnsureSuccessStatusCode();
    }
}
