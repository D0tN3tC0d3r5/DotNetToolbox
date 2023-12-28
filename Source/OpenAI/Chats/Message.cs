namespace DotNetToolbox.OpenAI.Chats;

public record Message {
    [JsonPropertyName("role")]
    public virtual MessageType Type { get; init; }
    public virtual string? Name { get; init; }
    public virtual JsonElement? Content { get; init; } // string or ContentPart[]
    public virtual ToolCall[]? ToolCalls { get; init; }
    public virtual string? ToolCallId { get; init; }
}
