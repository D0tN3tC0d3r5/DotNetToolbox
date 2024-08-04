namespace DotNetToolbox.AI.Common;

public class Tool
    : IValidatable, IContextSection {
    public string Title { get; } = string.Empty;
    public required int Id { get; init; }
    public required string Name { get; init; }
    public required string ReturnType { get; init; }
    public string? Description { get; set; }
    public List<Argument> Arguments { get; set; } = [];

    public string GetIndentedText(string indent) {
        var builder = new StringBuilder();
        builder.AppendIntoNewLine($"{indent}{Id}. {Name}:<{ReturnType}>");
        var newIndentation = indent + "    ";
        foreach (var arg in Arguments)
            builder.AppendSection(newIndentation, arg);
        builder.AppendIntoNewLine($"{indent}{Description}");
        return builder.ToString();
    }

    public Result Validate(IDictionary<string, object?>? context = null)
        => Result.Success();
}
