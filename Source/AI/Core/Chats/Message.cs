namespace DotNetToolbox.AI.Chats;

public class Message(string role, MessagePart[] parts) {
    public Message(string role, string message)
        : this(role, [new MessagePart("text", message)]) {
    }

    public string Role { get; set; } = role;
    public MessagePart[] Parts { get; set; } = parts;
}
