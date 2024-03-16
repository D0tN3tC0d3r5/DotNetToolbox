namespace DotNetToolbox.AI.OpenAI.Chat;

public class ChatRequestMessage {
    public ChatRequestMessage(object content) {
        switch (content) {
            case string:
                Role = "system";
                Content = content;
                break;
            case Message c:
                Role = c.Role;
                Content = c.Parts.Length == 1
                              ? (string)c.Parts[0].Value
                              : c.Parts.ToArray(p => new MessageContent(p.Value));
                break;
            case ChatFunctionCallResult c:
                Role = "tool";
                Content = c.Value;
                ToolCallId = c.CallId;
                break;
            case ChatResponseToolRequest[] c:
                Role = "assistant";
                ToolCalls = c;
                break;
            default:
                throw new NotSupportedException();
        }
    }
    [JsonPropertyName("role")]
    public string Role { get; init; }
    [JsonPropertyName("content")]
    public object? Content { get; init; }
    [JsonPropertyName("name")]
    public string? Name { get; set; }
    [JsonPropertyName("tool_calls")]
    public ChatResponseToolRequest[]? ToolCalls { get; set; }
    [JsonPropertyName("tool_call_id")]
    public string? ToolCallId { get; set; }
}
