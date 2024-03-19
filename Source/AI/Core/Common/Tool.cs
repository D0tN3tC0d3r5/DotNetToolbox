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
        if (Arguments.Count > 0) {
            builder.AppendLine();
            builder.Append("Arguments:");
            for (var index = 0; index < Arguments.Count; index++) {
                builder.AppendLine();
                var argument = Arguments[index];
                builder.Append("   ");
                if (!argument.IsRequired) builder.Append('[');
                if (index > 0) builder.Append(", ");
                builder.Append($"{index}. {argument.Type} {argument.Name}");
                if (argument.Options?.Length > 0)
                    builder.Append($" in {{{string.Join('|', argument.Options)}}}");
                if (Description is not null) builder.Append(" '{argument.Description}'");
                if (!argument.IsRequired) builder.Append(']');
            }
        }
        builder.AppendLine(";");
        return builder.ToString();
    }

    public Result Validate(IDictionary<string, object?>? context = null)
        => Result.Success();
}
