namespace DotNetToolbox.AI.Chats;

public class Message(string source, MessagePart[] parts) {
    public Message(string source, string message)
        : this(source, [new MessagePart("text", message)]) {
    }

    public string Role { get; set; } = source;
    public MessagePart[] Parts { get; set; } = parts;
}
