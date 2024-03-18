namespace Sophia.WebClient.Services;

public class SkillsService(HttpClient httpClient)
    : ISkillsService {
    public async Task<IReadOnlyList<SkillData>> GetList(string? filter = null) {
        var list = await httpClient.GetFromJsonAsync<SkillData[]>("skills");
        return list!;
    }

    public async Task<SkillData?> GetById(int id) {
        var skill = await httpClient.GetFromJsonAsync<SkillData>($"skills/{id}");
        return skill;
    }

    public async Task Add(SkillData input) {
        var response = await httpClient.PostAsJsonAsync("skills", input)!;
        response.EnsureSuccessStatusCode();
    }

    public async Task Update(SkillData input) {
        var response = await httpClient.PutAsJsonAsync("skills", input)!;
        response.EnsureSuccessStatusCode();
    }

    public async Task Delete(int id) {
        var response = await httpClient.DeleteAsync($"skills/{id}")!;
        response.EnsureSuccessStatusCode();
    }
}
