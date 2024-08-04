namespace DotNetToolbox.AI.OpenAI.Tools;

public class ToolParameter(string name, string type, string[]? options = null, bool isRequired = false, string? description = null) {
    public required string Name { get; init; } = name;
    public required string Type { get; init; } = type;
    public string? Description { get; set; } = description;
    public string[] Options { get; init; } = options ?? [];
    public bool IsRequired { get; init; } = isRequired;

    public string Signature => $"{Name}:{Type}";
}
