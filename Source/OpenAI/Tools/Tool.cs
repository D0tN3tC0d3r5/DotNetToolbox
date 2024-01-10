namespace DotNetToolbox.OpenAI.Tools;

public record Tool {
    public ToolType Type { get; init; } = ToolType.Function;
    public required Function Function { get; init; }
}
