namespace Sophia.Services;

public interface IChatsRemoteService : IChatsService;

public interface IChatsService {
    Task<IReadOnlyList<ChatData>> GetList(string? filter = null);
    Task<ChatData?> GetById(int id);
    Task Create(ChatData chat);
    Task Archive(int id);
    Task Unarchive(int id);
    Task Rename(int id, string newName);
    Task AddMessage(int id, MessageData message);
    Task Delete(int id);
}
