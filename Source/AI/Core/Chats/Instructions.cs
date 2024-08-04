namespace DotNetToolbox.AI.Chats;

public class Instructions
    : IContextSection {
    public string Title { get; } = string.Empty;
    public List<string> Goals { get; set; } = [];
    public List<string> Scope { get; set; } = [];
    public List<string> Requirements { get; set; } = [];
    public List<string> Assumptions { get; set; } = [];
    public List<string> Constraints { get; set; } = [];
    public List<string> Examples { get; set; } = [];
    public List<string> Strategy { get; set; } = [];
    public List<string> Validation { get; set; } = [];

    public string GetIndentedText(string indent) {
        var builder = new StringBuilder();
        builder.AppendSection(indent, Goals, nameof(Goals));
        builder.AppendSection(indent, Scope, nameof(Scope));
        builder.AppendSection(indent, Assumptions, nameof(Assumptions));
        builder.AppendSection(indent, Requirements, nameof(Requirements));
        builder.AppendSection(indent, Constraints, nameof(Constraints));
        builder.AppendSection(indent, Examples, nameof(Examples));
        builder.AppendSection(indent, Strategy, nameof(Strategy));
        builder.AppendSection(indent, Validation, nameof(Validation));
        return builder.ToString();
    }
}
