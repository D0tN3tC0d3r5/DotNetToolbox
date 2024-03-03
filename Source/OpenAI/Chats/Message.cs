namespace DotNetToolbox.OpenAI.Chats;

public class Message(MessageType type) {
    public virtual MessageType Type { get; } = type;
    public virtual string? Content { get; set; }
    public virtual string? Name { get; set; }
    public virtual ToolCall[]? ToolCalls { get; set; }
    public virtual string? ToolCallId { get; set; }
    public virtual string? FinishReason { get; set; }
}
