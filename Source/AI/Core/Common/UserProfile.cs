namespace DotNetToolbox.AI.Common;

public class UserProfile :
    IContextSection {
    public string Title { get; } = "User Information";
    public virtual string Id { get; set; } = default!;
    public virtual string Name { get; set; } = default!;
    public virtual string Language { get; set; } = "English";
    public List<string> Facts { get; set; } = [];

    public virtual string GetIndentedText(string indent) {
        var builder = new StringBuilder();
        if (!string.IsNullOrWhiteSpace(Name))
            builder.Append($"{indent}The USER is name is {Name}.");
        if (!string.IsNullOrWhiteSpace(Language))
            builder.AppendIntoNewLine($"{indent}The USER's preferred language is {Language}.");
        builder.AppendSection(indent, Facts, "Other facts about the User");
        return builder.ToString();
    }
}
