namespace DotNetToolbox.AI.OpenAI.Tools;

public class Tool(string name, string returnType, Dictionary<string, ToolParameter>? parameters = null, string? description = null) {
    public required string Name { get; init; } = name;
    public required string ReturnType { get; init; } = returnType;
    public string? Description { get; init; } = description;
    public Dictionary<string, ToolParameter> Parameters { get; init; } = parameters ?? [];

    public string Signature => $"{Name}({string.Join(",", Parameters.Values.Select(p => p.Signature))})->{ReturnType}";
}
