namespace DotNetToolbox.AI.OpenAI.Chats;

public class ChatRequestMessage : IChatRequestMessage {
    [SetsRequiredMembers]
    public ChatRequestMessage(object content) {
        switch (content) {
            case string:
                Role = Roles.System;
                Content = content;
                break;
            case Message c:
                Role = c.Role;
                Content = c.Parts.Length == 1
                              ? (string)c.Parts[0].Value
                              : c.Parts.ToArray(p => new MessageContent(p.Value));
                break;
            case ChatFunctionCallResult c:
                Role = Roles.Tool;
                Content = c.Value;
                ToolCallId = c.CallId;
                break;
            case ChatResponseToolRequest[] c:
                Role = Roles.Assistant;
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
}
