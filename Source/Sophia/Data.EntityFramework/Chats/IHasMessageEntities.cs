
namespace Sophia.Data.Chats;

public interface IHasMessageEntities {
    List<MessageEntity> Messages { get; set; }
}

public interface IHasChatMessageEntities : IHasMessageEntities {
    Guid Id { get; set; }
}

public interface IHasChatAgentMessageEntities : IHasMessageEntities {
    Guid ChatId { get; set; }
    int Number { get; set; }
}
