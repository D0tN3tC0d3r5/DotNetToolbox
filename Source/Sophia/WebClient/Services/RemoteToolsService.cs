﻿namespace Sophia.WebClient.Services;

public class RemoteToolsService(HttpClient httpClient)
    : IToolsService {
    public async Task<IReadOnlyList<ToolData>> GetList(string? filter = null) {
        var list = await httpClient.GetFromJsonAsync<ToolData[]>("tools");
        return list!;
    }

    public async Task<ToolData?> GetById(int id) {
        var tool = await httpClient.GetFromJsonAsync<ToolData>($"tools/{id}");
        return tool;
    }

    public async Task Add(ToolData input) {
        var response = await httpClient.PostAsJsonAsync("tools", input)!;
        response.EnsureSuccessStatusCode();
    }

    public async Task Update(ToolData input) {
        var response = await httpClient.PutAsJsonAsync("tools", input)!;
        response.EnsureSuccessStatusCode();
    }

    public async Task Delete(int id) {
        var response = await httpClient.DeleteAsync($"tools/{id}")!;
        response.EnsureSuccessStatusCode();
    }
}
