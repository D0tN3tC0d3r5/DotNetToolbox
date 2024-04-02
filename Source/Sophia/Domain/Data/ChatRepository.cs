namespace Sophia.Data;

public abstract class ChatRepository
    : SimpleKeyEntityRepository<ChatRepository, ChatData, Guid>;
