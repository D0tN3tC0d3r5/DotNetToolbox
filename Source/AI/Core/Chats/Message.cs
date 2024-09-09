namespace DotNetToolbox.AI.Chats;

public class Message(MessageRole role, IEnumerable<MessagePart> parts)
    : List<MessagePart>([.. parts]) {
    public Message(MessageRole role, MessagePart part)
        : this(role, [part]) {
    }
    public Message(MessageRole role, string message)
        : this(role, new MessagePart(message)) {
    }
    public Message(MessageRole role)
        : this(role, []) {
    }

    public MessageRole Role { get; } = role;
    public bool IsPartial { get; set; }
    public override string ToString()
        => this.Aggregate(new StringBuilder(), (b, v) => v.IsPartial ? b.Append(v.ToString()) : b.AppendLine(v.ToString())).ToString();

    public static implicit operator string(Message message) => message.ToString();
}
