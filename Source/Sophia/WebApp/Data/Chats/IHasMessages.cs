
namespace Sophia.WebApp.Data.Chats;

public interface IHasMessages {
    List<MessageEntity> Messages { get; set; }
}

public interface IHasChatMessages : IHasMessages {
    Guid Id { get; set; }
}

public interface IHasAgentMessages : IHasMessages {
    Guid ChatId { get; set; }
    int Number { get; set; }
}
