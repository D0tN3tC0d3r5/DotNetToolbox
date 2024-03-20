namespace Sophia.WebClient.Services;

public class ToolsRemoteService(HttpClient httpClient)
    : IToolsRemoteService {
    public async Task<IReadOnlyList<ToolData>> GetList(string? filter = null) {
        var list = await httpClient.GetFromJsonAsync<ToolData[]>("api/tools");
        return list!;
    }

    public async Task<ToolData?> GetById(int id) {
        var tool = await httpClient.GetFromJsonAsync<ToolData>($"api/tools/{id}");
        return tool;
    }

    public async Task Add(ToolData input) {
        var response = await httpClient.PostAsJsonAsync("api/tools", input)!;
        response.EnsureSuccessStatusCode();
    }

    public async Task Update(ToolData input) {
        var response = await httpClient.PutAsJsonAsync("api/tools", input)!;
        response.EnsureSuccessStatusCode();
    }

    public async Task Delete(int id) {
        var response = await httpClient.DeleteAsync($"api/tools/{id}")!;
        response.EnsureSuccessStatusCode();
    }
}
