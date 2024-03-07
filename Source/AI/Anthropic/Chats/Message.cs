namespace DotNetToolbox.AI.Anthropic.Chats;

public class Message
    : IMessage {
    private Message(string role, object content) {
        Role = role;
        Content = content;
    }

    public Message(string role, string message)
        : this(role, new Content(message)) { }

    public Message(string role, IEnumerable<Content> content)
        : this(role, (object)content.ToArray()) { }

    [JsonPropertyName("role")]
    public string Role { get; init; }

    [JsonPropertyName("content")]
    public object Content { get; init; }
}
