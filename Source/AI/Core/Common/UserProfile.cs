namespace DotNetToolbox.AI.Common;

public class UserProfile {
    public string Name { get; set; } = "User";
    public string Language { get; set; } = "English";

    public override string ToString() {
        var builder = new StringBuilder();
        if (!string.IsNullOrWhiteSpace(Name)) builder.Append($"The USER is called {Name}.");
        if (!string.IsNullOrWhiteSpace(Language)) builder.AppendIntoNewLine($"The USER's preferred language is {Language}.");
        return builder.ToString();
    }
}
