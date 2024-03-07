namespace DotNetToolbox.AI.OpenAI.Chats;

public class Message
    : IMessage {

    private Message(string role, object? content) {
        Role = role;
        Content = content;
    }

    public Message(string role, string content)
        : this(role, (object)content) { }

    public Message(string role, IEnumerable<Content> content)
        : this(role, (object)content.ToArray()) { }

    public Message(string role, IEnumerable<ToolCall> toolCalls)
        : this(role, (object?)null) {
        ToolCalls = toolCalls.ToArray();
    }

    [JsonPropertyName("role")]
    public string Role { get; init; }

    [JsonPropertyName("content")]
    public object? Content { get; init; }

    [JsonPropertyName("name")]
    public string? Name { get; set; }

    [JsonPropertyName("tool_calls")]
    public ToolCall[]? ToolCalls { get; set; }

    [JsonPropertyName("tool_call_id")]
    public string? ToolCallId { get; set; }

    [JsonPropertyName("finish_reason")]
    public string? FinishReason { get; set; }
}
