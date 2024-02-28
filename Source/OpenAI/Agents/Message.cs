namespace DotNetToolbox.OpenAI.Agents;

public class Message {
    [JsonPropertyName("role")]
    public virtual MessageType Type { get; init; } = MessageType.Assistant;

    public virtual string Content { get; set; } = string.Empty;
    public virtual string? Name { get; init; }
    public virtual ToolCall[]? ToolCalls { get; init; }
    public virtual string? ToolCallId { get; init; }
}
