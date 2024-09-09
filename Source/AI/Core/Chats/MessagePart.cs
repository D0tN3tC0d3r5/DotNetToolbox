using static DotNetToolbox.AI.Chats.MessagePartContentType;

namespace DotNetToolbox.AI.Chats;
public class MessagePart(MessagePartContentType type, object? content = null) {
    public MessagePart(string text)
        : this(Text, text) {
    }
    public MessagePart(byte[] data)
        : this(MessagePartContentType.File, data) {
    }
    public MessagePart()
        : this(Text, string.Empty) {
    }

    public MessagePartContentType Type { get; } = type;
    public object? Content { get; set; } = content;
    public bool IsPartial { get; set; }

    public override string ToString()
        => Content is null ? string.Empty
            : Content is string text ? text
            : Encoding.ASCII.GetString((byte[])Content);

    public byte[] ToBytes()
        => Content is null ? []
            : Content is byte[] data ? data
            : Encoding.UTF8.GetBytes((string)Content);

    public static implicit operator MessagePart(string text) => new(text);
    public static implicit operator MessagePart(byte[] data) => new(data);
    public static implicit operator string(MessagePart part) => part.ToString();
}
