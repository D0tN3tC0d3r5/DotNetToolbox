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

    public async Task Add(PersonaData input) {
        var response = await httpClient.PostAsJsonAsync("api/personas", input);
        response.EnsureSuccessStatusCode();
        var result = await response.Content.ReadFromJsonAsync<PersonaData>();
        input.Id = result!.Id;
    }

    public async Task Update(PersonaData input) {
        var response = await httpClient.PutAsJsonAsync("api/personas", input);
        response.EnsureSuccessStatusCode();
    }

    public async Task Delete(int id) {
        var response = await httpClient.DeleteAsync($"api/personas/{id}");
        response.EnsureSuccessStatusCode();
    }
}
