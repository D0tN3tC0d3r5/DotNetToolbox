namespace Sophia.Services;

public interface IChatsRemoteService : IChatsService;

public interface IChatsService {
    Task<IReadOnlyList<ChatData>> GetList(string? filter = null);
    Task<ChatData?> GetById(Guid id);
    Task Create(ChatData input);
    Task Archive(Guid id);
    Task Unarchive(Guid id);
    Task Rename(Guid id, string newName);
    Task AddMessage(Guid id, MessageData newMessage);
    Task Delete(Guid id);
}
