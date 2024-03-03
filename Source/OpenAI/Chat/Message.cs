namespace DotNetToolbox.OpenAI.Agents;

public class Message(MessageType type) {
    [JsonPropertyName("role")]
    public virtual MessageType Type { get; } = type;
    public virtual string Content { get; set; } = string.Empty;
    public virtual string? Name { get; set; }
    public virtual ToolCall[]? ToolCalls { get; set; }
    public virtual string? ToolCallId { get; set; }
}
