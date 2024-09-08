namespace DotNetToolbox.AI.Anthropic.Chats;

public class ChatRequestMessage
    : IChatRequestMessage {
    [SetsRequiredMembers]
    public ChatRequestMessage(object content) {
        switch (content) {
            case Message c:
                Role = RoleToString(c.Role);
                Content = c.Count == 1
                              ? (string)c[0].Content
                              : c.ToArray(p => new MessageContent(p.Content));
                break;
            default:
                throw new NotSupportedException();
        }
    }
    [JsonPropertyName("role")]
    public required string Role { get; init; }
    [JsonPropertyName("content")]
    public object? Content { get; set; }

    private static string RoleToString(MessageRole role) => role switch {
        MessageRole.User => "user",
        MessageRole.Assistant => "assistant",
        MessageRole.Tool => "tool",
        _ => "system",
    };
}
