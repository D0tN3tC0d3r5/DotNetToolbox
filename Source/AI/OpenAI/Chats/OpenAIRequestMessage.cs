namespace DotNetToolbox.AI.OpenAI.Chats;

public class OpenAIRequestMessage {
    public OpenAIRequestMessage(object content) {
        switch (content) {
            case Message { Parts.Length: 1 } c:
                Role = c.Role;
                Content = (string)c.Parts[0].Value;
                break;
            case Message c:
                Role = c.Role;
                Content = c.Parts.ToArray(p => new OpenAIMessageContent(p.Value));
                break;
            case OpenAIToolResult c:
                Role = "tool";
                Content = c.Value;
                ToolCallId = c.CallId;
                break;
            case OpenAIToolCall[] c:
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
    public OpenAIToolCall[]? ToolCalls { get; set; }
    [JsonPropertyName("tool_call_id")]
    public string? ToolCallId { get; set; }
}
