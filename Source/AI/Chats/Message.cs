namespace DotNetToolbox.AI.Chats;

public class Message(string source, MessagePart[] parts) {
    public string Role { get; set; } = source;
    public MessagePart[] Parts { get; set; } = parts;
}
