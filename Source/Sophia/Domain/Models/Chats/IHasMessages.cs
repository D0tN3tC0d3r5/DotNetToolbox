namespace Sophia.Models.Chats;

public interface IHasMessages {
    List<MessageData> Messages { get; set; }
}

public interface IHasChatMessages : IHasMessages {
    Guid Id { get; set; }
}

public interface IHasChatAgentMessages : IHasMessages {
    Guid ChatId { get; set; }
    int Number { get; set; }
}
