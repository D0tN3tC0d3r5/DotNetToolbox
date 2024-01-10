namespace DotNetToolbox.OpenAI.Tools;

public record ToolCall {
    public required string Id { get; init; }
    public required ToolType Type { get; init; }
    public required FunctionCall Function { get; init; }
}
