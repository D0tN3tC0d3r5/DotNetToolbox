namespace DotNetToolbox.AI.Chats;

public class Message(string role, MessagePart[] parts) {
    private static readonly Func<StringBuilder, MessagePart, StringBuilder> _concatValues = (b, v) => b.AppendLine(v.AsText());

    public Message(string role, MessagePart part)
        : this(role, [part]) {
    }
    public Message(string role, string message)
        : this(role, new MessagePart("text", message)) {
    }

    public string Role { get; set; } = role;
    public MessagePart[] Parts { get; set; } = [..parts];
    public string AsText() => parts.Aggregate(new(), _concatValues).ToString();
}
