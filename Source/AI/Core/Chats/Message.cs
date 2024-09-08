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
    public string Text => this.Aggregate(new StringBuilder(), (b, v) => b.Append(v.Text)).ToString();

    public static implicit operator string(Message message) => message.Text;
}
