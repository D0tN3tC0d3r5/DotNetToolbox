namespace DotNetToolbox.AI.Personas;

public class Argument(string name, string type, string[]? options = null, bool isRequired = false, string? description = null) {
    public string Name { get; set; } = name;
    public string Type { get; set; } = type;
    public string? Description { get; set; } = description;
    public string[]? Options { get; set; } = options;
    public bool IsRequired { get; set; } = isRequired;
}
