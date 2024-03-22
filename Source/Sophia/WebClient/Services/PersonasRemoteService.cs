namespace Sophia.WebClient.Services;

public class PersonasRemoteService(HttpClient httpClient)
    : IPersonasRemoteService {

    private static readonly UrlEncoder _encoder = UrlEncoder.Create();

    public async Task<IReadOnlyList<PersonaData>> GetList(string? filter = null) {
        var query = filter is null ? string.Empty : $"?filter={_encoder.Encode(filter)}";
        var list = await httpClient.GetFromJsonAsync<PersonaData[]>($"api/personas{query}");
        return list!;
    }

    public async Task<PersonaData?> GetById(int id) {
        var persona = await httpClient.GetFromJsonAsync<PersonaData>($"api/personas/{id}");
        return persona;
    }

    public async Task Add(PersonaData persona) {
        var response = await httpClient.PostAsJsonAsync("api/personas", persona);
        response.EnsureSuccessStatusCode();
        var result = await response.Content.ReadFromJsonAsync<PersonaData>();
        persona.Id = result!.Id;
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
