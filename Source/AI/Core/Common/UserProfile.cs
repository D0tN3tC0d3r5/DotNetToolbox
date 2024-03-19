namespace DotNetToolbox.AI.Common;

public class UserProfile {
    public string? Name { get; set; }
    public string? Language { get; set; }

    public override string ToString() {
        var builder = new StringBuilder();
        if (!string.IsNullOrWhiteSpace(Name)) builder.Append($"The USER name is {Name}.");
        if (!string.IsNullOrWhiteSpace(Language)) builder.AppendIntoNewLine($"The USER preferred language is {Language}.");
        return builder.ToString();
    }
}
