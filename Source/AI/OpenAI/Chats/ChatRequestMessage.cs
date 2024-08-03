namespace DotNetToolbox.AI.OpenAI.Chats;

public class ChatRequestMessage : IChatRequestMessage {
    [SetsRequiredMembers]
    public ChatRequestMessage(object content) {
        switch (content) {
            case string:
                Role = RoleToString(MessageRole.System);
                Content = content;
                break;
            case Message c:
                Role = RoleToString(c.Role);
                Content = c.Parts.Count == 1
                              ? (string)c.Parts[0].Content
                              : c.Parts.ToArray(p => new MessageContent(p.Content));
                break;
            case ChatFunctionCallResult c:
                Role = RoleToString(MessageRole.Tool);
                Content = c.Value;
                ToolCallId = c.CallId;
                break;
            case ChatResponseToolRequest[] c:
                Role = RoleToString(MessageRole.Assistant);
                ToolCalls = c;
                break;
            default:
                throw new NotSupportedException();
        }
    }
    [JsonPropertyName("role")]
    public required string Role { get; init; }
    [JsonPropertyName("content")]
    public object? Content { get; set; }
    [JsonPropertyName("name")]
    public string? Name { get; set; }
    [JsonPropertyName("tool_calls")]
    public ChatResponseToolRequest[]? ToolCalls { get; set; }
    [JsonPropertyName("tool_call_id")]
    public string? ToolCallId { get; set; }

    private static string RoleToString(MessageRole role) => role switch {
        MessageRole.User => "user",
        MessageRole.Assistant => "assistant",
        MessageRole.Tool => "tool",
        _ => "system",
    };
}
