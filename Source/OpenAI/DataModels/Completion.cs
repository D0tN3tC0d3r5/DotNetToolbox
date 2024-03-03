namespace DotNetToolbox.OpenAI.DataModels;

public class Completion {
    public virtual Role Role { get; set; }
    public virtual string? Content { get; set; }
    public virtual string? Name { get; set; }
    public virtual ToolCall[]? ToolCalls { get; set; }
    public virtual string? ToolCallId { get; set; }
    public virtual string? FinishReason { get; set; }
}
