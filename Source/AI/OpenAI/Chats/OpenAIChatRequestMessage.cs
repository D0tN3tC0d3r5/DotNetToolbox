namespace DotNetToolbox.AI.OpenAI.Chats;

public class OpenAIChatRequestMessage {
    public OpenAIChatRequestMessage(object content) {
        switch (content) {
            case string:
                Role = "system";
                Content = content;
                break;
            case Message c:
                Role = c.Role;
                Content = c.Parts.Length == 1
                              ? (string)c.Parts[0].Value
                              : c.Parts.ToArray(p => new OpenAIChatRequestMessageContent(p.Value));
                break;
            case OpenAIChatFunctionCallResult c:
                Role = "tool";
                Content = c.Value;
                ToolCallId = c.CallId;
                break;
            case OpenAIChatResponseToolRequest[] c:
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
    public OpenAIChatResponseToolRequest[]? ToolCalls { get; set; }
    [JsonPropertyName("tool_call_id")]
    public string? ToolCallId { get; set; }
}
