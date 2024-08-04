namespace DotNetToolbox.AI.Personas;

public class Characteristics
    : IContextSection {
    public string Title { get; } = string.Empty;
    public List<string> Cognition { get; set; } = [];
    public List<string> Disposition { get; set; } = [];
    public List<string> Interaction { get; set; } = [];
    public List<string> Attitude { get; set; } = [];

    public string GetIndentedText(string indent) {
        var builder = new StringBuilder();
        builder.AppendSection(indent, Cognition);
        builder.AppendSection(indent, Disposition);
        builder.AppendSection(indent, Interaction);
        builder.AppendSection(indent, Attitude);
        return builder.ToString();
    }
}
