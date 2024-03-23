namespace Sophia.Services;

public interface IChatsRemoteService : IChatsService;

public interface IChatsService {
    Task<IReadOnlyList<ChatData>> GetList(string? filter = null);
    Task<ChatData?> GetById(string id);
    Task Create(ChatData chat);
    Task Archive(string id);
    Task Unarchive(string id);
    Task Rename(string id, string newName);
    Task AddMessage(string id, MessageData message);
    Task Delete(string id);
}
