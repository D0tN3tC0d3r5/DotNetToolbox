namespace DotNetToolbox.AI.Anthropic;

public class ClaudeMessage(MessageType type)
    : Message {
    public virtual MessageType Type { get; } = type;
    public virtual string? Content { get; set; }
    public virtual string? FinishReason { get; set; }
}