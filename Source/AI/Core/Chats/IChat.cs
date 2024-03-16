namespace DotNetToolbox.AI.Chats;

public interface IChat {
    string Id { get; }
    Instructions Instructions { get; }
    List<Message> Messages { get; }
    uint TotalTokens { get; set; }
}
