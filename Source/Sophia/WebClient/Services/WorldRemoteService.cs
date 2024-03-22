namespace Sophia.WebClient.Services;

public class WorldRemoteService(HttpClient httpClient)
    : IWorldRemoteService {

    public Task<WorldData> GetWorld()
        => httpClient.GetFromJsonAsync<WorldData>("api/world")!;

    public async Task UpdateWorld(WorldData input) {
        var response = await httpClient.PutAsJsonAsync("api/world", input)!;
        response.EnsureSuccessStatusCode();
    }
}
