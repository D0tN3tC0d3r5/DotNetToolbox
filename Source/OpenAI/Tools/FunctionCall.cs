namespace DotNetToolbox.OpenAI.Tools;

public record FunctionCall {
    public required string Name { get; init; }
    public string? Arguments { get; init; }
}
