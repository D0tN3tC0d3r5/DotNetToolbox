namespace Sophia.WebClient.Services;

public class ChatsRemoteService(HttpClient httpClient)
    : IChatsRemoteService {
    public async Task<IReadOnlyList<ChatData>> GetList(string? filter = null) {
        var list = await httpClient.GetFromJsonAsync<ChatData[]>("api/chats");
        return list!;
    }

    public async Task<ChatData?> Resume(int id) {
        var chat = await httpClient.GetFromJsonAsync<ChatData>($"api/chats/{id}");
        return chat;
    }

    public async Task Start(ChatData chat) {
        var response = await httpClient.PostAsJsonAsync("api/chats", chat);
        response.EnsureSuccessStatusCode();
    }

    public async Task Archive(int id) {
        var response = await httpClient.PatchAsJsonAsync($"api/chats/{id}", "archive");
        response.EnsureSuccessStatusCode();
    }

    public async Task Delete(int id) {
        var response = await httpClient.DeleteAsync($"api/chats/{id}");
        response.EnsureSuccessStatusCode();
    }
}
