using static DotNetToolbox.AI.Chats.MessagePartContentType;

namespace DotNetToolbox.AI.Chats;
public class MessagePart(MessagePartContentType type, object? content = null) {
    public MessagePart(string text)
        : this(Text, text) {
    }
    public MessagePart(IEnumerable data)
        : this(MessagePartContentType.File, data) {
    }
    public MessagePart()
        : this(Text, string.Empty) {
    }

    public MessagePartContentType Type { get; } = type;
    public object? Content { get; set; } = content;
    public bool IsPartial { get; set; }

    public override string ToString()
        => Content switch {
            null => string.Empty,
            string text => text,
            _ => Encoding.ASCII.GetString((byte[])Content),
        };

    public byte[] ToBytes()
        => Content switch {
            null => [],
            byte[] data => data,
            _ => Encoding.UTF8.GetBytes((string)Content),
        };

    public static implicit operator MessagePart(string text) => new(text);
    public static implicit operator MessagePart(byte[] data) => new(data);
    public static implicit operator string(MessagePart part) => part.ToString();
}
