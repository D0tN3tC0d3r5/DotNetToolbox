namespace DotNetToolbox.AI.Shared;

public class Argument {
    public required string Name { get; set; }
    public required ArgumentType Type { get; set; }
    public string? Description { get; set; }
    public string[]? Options { get; set; } = [];
    public bool IsRequired { get; set; }
}
