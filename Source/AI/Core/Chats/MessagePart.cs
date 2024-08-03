namespace DotNetToolbox.AI.Chats;

public class MessagePart(MessagePartContentType type, object content) {
    public MessagePart(string text)
        : this(MessagePartContentType.Text, text) {
    }

    public MessagePartContentType Type { get; } = type;
    public object Content { get; } = content;
    public string Text
        => Type == MessagePartContentType.Text
               ? (string)Content
               : $"[{Type}]";

    public static implicit operator MessagePart(string text) => new(text);
    public static implicit operator string(MessagePart part) => part.Text;
}
