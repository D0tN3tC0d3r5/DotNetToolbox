namespace Sophia.WebClient.Services;

public class ToolsRemoteService(HttpClient httpClient)
    : IToolsRemoteService {

    private static readonly UrlEncoder _encoder = UrlEncoder.Create();

    public async Task<IReadOnlyList<ToolData>> GetList(string? filter = null) {
        var query = filter is null ? string.Empty : $"?filter={_encoder.Encode(filter)}";
        var list = await httpClient.GetFromJsonAsync<ToolData[]>($"api/tools{query}");
        return list!;
    }

    public async Task<ToolData?> GetById(int id) {
        var tool = await httpClient.GetFromJsonAsync<ToolData>($"api/tools/{id}");
        return tool;
    }

    public async Task Add(ToolData tool) {
        var response = await httpClient.PostAsJsonAsync("api/tools", tool)!;
        response.EnsureSuccessStatusCode();
        var result = await response.Content.ReadFromJsonAsync<ToolData>();
        tool.Id = result!.Id;
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
