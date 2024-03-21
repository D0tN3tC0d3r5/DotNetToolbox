namespace Sophia.WebClient.Services;

public class PersonasRemoteService(HttpClient httpClient)
    : IPersonasRemoteService {
    public async Task<IReadOnlyList<PersonaData>> GetList(string? filter = null) {
        var list = await httpClient.GetFromJsonAsync<PersonaData[]>("api/personas");
        return list!;
    }

    public async Task<PersonaData?> GetById(int id) {
        var persona = await httpClient.GetFromJsonAsync<PersonaData>($"api/personas/{id}");
        return persona;
    }

    public async Task Add(PersonaData persona) {
        var response = await httpClient.PostAsJsonAsync("api/personas", persona);
        response.EnsureSuccessStatusCode();
    }

    public async Task Update(PersonaData persona) {
        var response = await httpClient.PutAsJsonAsync("api/personas", persona);
        response.EnsureSuccessStatusCode();
    }

    public async Task Delete(int id) {
        var response = await httpClient.DeleteAsync($"api/personas/{id}");
        response.EnsureSuccessStatusCode();
    }
}
