namespace DotNetToolbox.AI.Common;

public class User {
    public string Name { get; set; } = string.Empty;
    public string Language { get; set; } = "English";
    public List<string> Facts { get; set; } = [];

    public override string ToString() {
        var builder = new StringBuilder();
        if (!string.IsNullOrWhiteSpace(Name)) builder.Append($"The USER is name is {Name}.");
        if (!string.IsNullOrWhiteSpace(Language)) builder.AppendIntoNewLine($"The USER's preferred language is {Language}.");
        builder.AppendSection(Facts, "User Facts");
        return builder.ToString();
    }
}
