namespace DotNetToolbox.AI.Common;

public class UserProfile {
    public string Name { get; set; } = string.Empty;
    public string Language { get; set; } = "English";

    public override string ToString() {
        var builder = new StringBuilder();
        if (!string.IsNullOrWhiteSpace(Name)) builder.Append($"The USER is name is {Name}.");
        if (!string.IsNullOrWhiteSpace(Language)) builder.AppendIntoNewLine($"The USER's preferred language is {Language}.");
        return builder.ToString();
    }
}
