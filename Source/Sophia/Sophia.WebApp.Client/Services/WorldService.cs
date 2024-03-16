﻿using System.Net.Http.Json;

namespace Sophia.WebClient.Services;

public class WorldService(HttpClient httpClient)
    : IWorldService {
    public Task<WorldData> GetWorld()
        => httpClient.GetFromJsonAsync<WorldData>("world")!;

    public async Task UpdateWorld(WorldData input) {
        var response = await httpClient.PutAsJsonAsync("world", input)!;
        response.EnsureSuccessStatusCode();
    }
}
