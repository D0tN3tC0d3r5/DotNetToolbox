using System.Text;

namespace DotNetToolbox.AI.OpenAI.Chats;

public class OpenAIChatRequestMessage {
    public OpenAIChatRequestMessage(object content) {
        switch (content) {
            case Message { Parts.Length: 1 } c:
                Role = c.Role;
                Content = (string)c.Parts[0].Value;
                break;
            case Message { Role: "system" } c:
                Role = c.Role;
                Content = c.Parts.Aggregate(new StringBuilder(), (s, p) => s.AppendLine((string)p.Value)).ToString();
                break;
            case Message c:
                Role = c.Role;
                Content = c.Parts.ToArray(p => new OpenAIChatRequestMessageContent(p.Value));
                break;
            case OpenAIChatRequestMessageToolCallResult c:
                Role = "tool";
                Content = c.Value;
                ToolCallId = c.CallId;
                break;
            case OpenAIChatResponseToolCall[] c:
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
    public OpenAIChatResponseToolCall[]? ToolCalls { get; set; }
    [JsonPropertyName("tool_call_id")]
    public string? ToolCallId { get; set; }
}
