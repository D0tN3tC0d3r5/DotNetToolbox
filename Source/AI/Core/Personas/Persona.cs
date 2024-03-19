namespace DotNetToolbox.AI.Personas;

[method: JsonConstructor]
public class Persona() {

    public Persona(string name)
        : this() {
        Name = IsNotNull(name);
    }

    public string Name { get; set; } = "Agent";
    public string Description { get; set; } = "You are a helpful agent.";
    public string? Personality { get; set; }
    public List<string> Instructions { get; set; } = [];
    public List<Fact> Facts { get; set; } = [];
    public List<Tool> KnownTools { get; set; } = [];

    public override string ToString() {
        var builder = new StringBuilder();
        builder.AppendLine($"You name is {Name}.");
        builder.AppendLine(Description);
        if (!string.IsNullOrWhiteSpace(Personality)) builder.AppendLine(Personality);
        builder.AppendSection(KnownTools, "Known Tools");
        builder.AppendSection(Facts);
        builder.AppendSection(Instructions);
        return builder.ToString();
    }
}
