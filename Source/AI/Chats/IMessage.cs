namespace DotNetToolbox.AI.Chats;

public interface IMessage {
    string Role { get; }
    object? Content { get; }
}
