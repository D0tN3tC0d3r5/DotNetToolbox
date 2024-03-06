namespace DotNetToolbox.AI.Tools;

public record Parameter {
    public required string Type { get; init; }
    public string? Description { get; init; }
    public string[]? Enum { get; init; }
}
