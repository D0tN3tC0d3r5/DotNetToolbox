namespace DotNetToolbox.AI.Tools;

public record Tool(Function Function) {
    public ToolType Type { get; init; } = ToolType.Function;
}
