namespace DotNetToolbox.AI.Chats;

public interface IChatRequestMessage {
    string Role { get; }
    object? Content { get; }
}
