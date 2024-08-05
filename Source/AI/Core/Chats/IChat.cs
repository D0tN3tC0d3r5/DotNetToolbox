namespace DotNetToolbox.AI.Chats;

public interface IChat {
    string Id { get; }
    List<Message> Messages { get; }
    uint TotalTokens { get; set; }
}
