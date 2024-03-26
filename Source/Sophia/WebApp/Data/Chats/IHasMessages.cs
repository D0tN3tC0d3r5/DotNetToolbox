
namespace Sophia.WebApp.Data.Chats;

public interface IHasMessages {
    List<MessageEntity> Messages { get; set; }
}

public interface IHasChatMessages : IHasMessages {
    string Id { get; set; }
}

public interface IHasAgentMessages : IHasMessages {
    string ChatId { get; set; }
    int Index { get; set; }
}
