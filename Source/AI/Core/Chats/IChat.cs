namespace DotNetToolbox.AI.Chats;

public interface IChat {
    string Id { get; }
    List<Message> Messages { get; }
    uint InputTokens { get; set; }
    uint OutputTokens { get; set; }
}
