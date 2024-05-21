namespace DotNetToolbox.AI.Common;

public class Tool
    : IValidatable {
    public required int Id { get; set; }
    public required string Name { get; set; }
    public string? Description { get; set; }
    public List<Argument> Arguments { get; set; } = [];

    public override string ToString() {
        var builder = new StringBuilder();
        builder.Append($"{Id}. {Name}: '{Description}'");
        builder.AppendSection("    ", Arguments);
        return builder.ToString();
    }

    public Result Validate(IDictionary<string, object?>? context = null)
        => Result.Success();
}
