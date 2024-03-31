namespace Sophia.WebClient.Services;

public class ChatsRemoteService(HttpClient httpClient)
    : IChatsRemoteService {

    private static readonly UrlEncoder _encoder = UrlEncoder.Create();

    public async Task<IReadOnlyList<ChatData>> GetList(string? filter = null) {
        var query = filter is null ? string.Empty : $"?filter={_encoder.Encode(filter)}";
        var list = await httpClient.GetFromJsonAsync<ChatData[]>($"api/chats{query}");
        return list!;
    }

    public async Task<ChatData?> GetById(Guid id) {
        var chat = await httpClient.GetFromJsonAsync<ChatData>($"api/chats/{id}");
        return chat;
    }

    public async Task Create(ChatData chat) {
        try {
            var response = await httpClient.PostAsJsonAsync("api/chats", chat);
            response.EnsureSuccessStatusCode();
            var result = await response.Content.ReadFromJsonAsync<ChatData>();
            chat.Id = result!.Id;
        }
        catch (Exception ex) {
            Console.WriteLine(ex.Message);
            throw;
        }
    }

    public async Task Archive(Guid id) {
        var response = await httpClient.PatchAsJsonAsync($"api/chats/{id}/archive", string.Empty);
        response.EnsureSuccessStatusCode();
    }

    public async Task Unarchive(Guid id) {
        var response = await httpClient.PatchAsJsonAsync($"api/chats/{id}/unarchive", string.Empty);
        response.EnsureSuccessStatusCode();
    }

    public async Task Rename(Guid id, string newName) {
        var response = await httpClient.PatchAsJsonAsync($"api/chats/{id}/rename", newName);
        response.EnsureSuccessStatusCode();
    }

    public async Task AddMessage(Guid id, MessageData newMessage) {
        var response = await httpClient.PatchAsJsonAsync($"api/chats/{id}/add-message", newMessage);
        response.EnsureSuccessStatusCode();
    }

    public async Task Delete(Guid id) {
        var response = await httpClient.DeleteAsync($"api/chats/{id}");
        response.EnsureSuccessStatusCode();
    }
}
