namespace DotNetToolbox.AI.Personas;

public class Characteristics {
    public List<string> Cognition { get; set; } = [];
    public List<string> Disposition { get; set; } = [];
    public List<string> Interaction { get; set; } = [];
    public List<string> Attitude { get; set; } = [];

    public override string ToString() {
        var builder = new StringBuilder();
        builder.AppendSection(Cognition);
        builder.AppendSection(Disposition);
        builder.AppendSection(Interaction);
        builder.AppendSection(Attitude);
        return builder.ToString();
    }
}
