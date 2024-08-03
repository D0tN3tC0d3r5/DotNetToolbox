namespace DotNetToolbox.AI.Chats;

public class Message(MessageRole role, MessagePart[] parts) {
    public Message(MessageRole role, MessagePart part)
        : this(role, [part]) {
    }
    public Message(MessageRole role, string message)
        : this(role, new MessagePart(message)) {
    }
    public Message(MessageRole role)
        : this(role, []) {
    }

    public MessageRole Role { get; set; } = role;
    public List<MessagePart> Parts { get; set; } = [.. parts];
    public bool IsComplete { get; set; }
    public string Text => parts.Aggregate(new StringBuilder(), (b, v) => b.AppendLine(v.Text)).ToString();

    public static implicit operator string(Message message) => message.Text;
}
