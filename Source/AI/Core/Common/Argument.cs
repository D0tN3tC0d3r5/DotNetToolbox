namespace DotNetToolbox.AI.Common;

public class Argument {
    public required int Index { get; set; }
    public required string Name { get; set; }
    public required ArgumentType Type { get; set; }
    public string? Description { get; set; }
    public string[]? Options { get; set; } = [];
    public bool IsRequired { get; set; }

    public override string ToString() {
        var builder = new StringBuilder();
        builder.Append($"{Index}: ");
        if (!IsRequired)
            builder.Append('[');
        builder.Append($"{Name}: <{Type}>");
        if (Options?.Length > 0)
            builder.Append($" in {{{string.Join('|', Options)}}}");
        if (!IsRequired)
            builder.Append(']');
        if (Description is not null)
            builder.Append($" '{Description}'");
        return builder.ToString();
    }
}
