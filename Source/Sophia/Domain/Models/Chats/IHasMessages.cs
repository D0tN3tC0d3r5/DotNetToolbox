namespace Sophia.Models.Chats;

public interface IHasMessages {
    List<MessageData> Messages { get; }
}
