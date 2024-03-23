namespace Sophia.WebClient.Services;

public class ChatsRemoteService(HttpClient httpClient)
    : IChatsRemoteService {

    private static readonly UrlEncoder _encoder = UrlEncoder.Create();

    public async Task<IReadOnlyList<ChatData>> GetList(string? filter = null) {
        var query = filter is null ? string.Empty : $"?filter={_encoder.Encode(filter)}";
        var list = await httpClient.GetFromJsonAsync<ChatData[]>($"api/chats{query}");
        return list!;
    }

    public async Task<ChatData?> GetById(string id) {
        var chat = await httpClient.GetFromJsonAsync<ChatData>($"api/chats/{id}");
        return chat;
    }

    public async Task Create(ChatData chat) {
        var response = await httpClient.PostAsJsonAsync("api/chats", chat);
        response.EnsureSuccessStatusCode();
        var result = await response.Content.ReadFromJsonAsync<ChatData>();
        chat.Id = result!.Id;
    }

    public async Task Archive(string id) {
        var response = await httpClient.PatchAsJsonAsync($"api/chats/{id}/archive", string.Empty);
        response.EnsureSuccessStatusCode();
    }

    public async Task Unarchive(string id) {
        var response = await httpClient.PatchAsJsonAsync($"api/chats/{id}/unarchive", string.Empty);
        response.EnsureSuccessStatusCode();
    }

    public async Task Rename(string id, string newName) {
        var response = await httpClient.PatchAsJsonAsync($"api/chats/{id}/rename", newName);
        response.EnsureSuccessStatusCode();
    }

    public async Task AddMessage(string id, MessageData message) {
        var response = await httpClient.PatchAsJsonAsync($"api/chats/{id}/add-message", message);
        response.EnsureSuccessStatusCode();
    }

    public async Task Delete(string id) {
        var response = await httpClient.DeleteAsync($"api/chats/{id}");
        response.EnsureSuccessStatusCode();
    }
}
