using Sophia.Models.Chats;

namespace Sophia.Services;

public interface IChatsRemoteService : IChatsService;

public interface IChatsService {
    Task<IReadOnlyList<ChatData>> GetList(string? filter = null);
    Task Start(ChatData newChat);
    Task<ChatData?> Resume(int chatId);
    Task Archive(int chatId);
    Task Delete(int chatId);
}
