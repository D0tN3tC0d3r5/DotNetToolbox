namespace DotNetToolbox.OpenAI.Tools;

public record Tool {
    public required ToolType Type { get; init; }
    public required Function Function { get; init; }
}
