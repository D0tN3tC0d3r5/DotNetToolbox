namespace DotNetToolbox.AI.OpenAI;

public class OpenAIMessage(MessageType type)
    : Message {
    public virtual MessageType Type { get; } = type;
    public virtual string? Content { get; set; }
    public virtual string? Name { get; set; }
    public virtual ToolCall[]? ToolCalls { get; set; }
    public virtual string? ToolCallId { get; set; }
    public virtual string? FinishReason { get; set; }
}
