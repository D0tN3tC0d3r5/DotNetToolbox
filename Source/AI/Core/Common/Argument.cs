namespace DotNetToolbox.AI.Common;

public class Argument {
    public required uint Index { get; set; }
    public required string Name { get; set; }
    public required ArgumentType Type { get; set; }
    public string? Description { get; set; }
    public string[]? Options { get; set; } = [];
    public bool IsRequired { get; set; }
}
