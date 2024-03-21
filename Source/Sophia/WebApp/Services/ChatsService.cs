namespace Sophia.WebApp.Services;

public class ChatsService : IChatsService {
    public Task<IReadOnlyList<ChatData>> GetList(string? filter = null)
        => Task.FromResult<IReadOnlyList<ChatData>>(Array.Empty<ChatData>());

    public Task<ChatData?> GetById(int chatId)
        => Task.FromResult(new ChatData())!;

    public Task Create(ChatData chat)
        => Task.CompletedTask;

    public Task Archive(int chatId)
        => Task.CompletedTask;

    public Task AddMessage(int chatId, MessageData message)
        => Task.CompletedTask;

    public Task Delete(int chatId)
        => Task.CompletedTask;
}
