namespace DotNetToolbox.AI.Chats;

public interface IChat {
    string Id { get; }
    Instructions Instructions { get; }
    List<Message> Messages { get; }
    int TotalNumberOfTokens { get; set; }
}
