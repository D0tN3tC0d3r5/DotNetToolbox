namespace DotNetToolbox.AI.Chats;

public interface IChat {
    string Id { get; }
    Message System { get; }
    List<Message> Messages { get; }
}
