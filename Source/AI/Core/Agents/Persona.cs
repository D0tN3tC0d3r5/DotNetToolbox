namespace DotNetToolbox.AI.Agents;

[method: JsonConstructor]
public class Persona() {
    public Persona(string name)
        : this() {
        Name = IsNotNull(name);
    }

    public string Name { get; set; } = "Agent";
    public string Description { get; set; } = "You are a helpful ASSISTANT.";
    public List<string> Cognition { get; set; } = [];
    public List<string> Disposition { get; set; } = [];
    public List<string> Interaction { get; set; } = [];
    public List<string> Attitude { get; set; } = [];
    public List<string> Facts { get; set; } = [];
    public List<Tool> KnownTools { get; set; } = [];

    public string GetIndentedText(string indent) {
        var builder = new StringBuilder();
        builder.AppendLine($"{indent}Your name is {Name}.");
        builder.AppendLine($"{indent}{Description}");
        builder.Append(indent, Cognition);
        builder.Append(indent, Disposition);
        builder.Append(indent, Interaction);
        builder.Append(indent, Attitude);
        builder.Append(indent, Facts);
        if (KnownTools.Count > 0) {
            builder.AppendIntoNewLine($"{indent}Known Tools");
            foreach (var tool in KnownTools)
                builder.Append(indent, tool);
        }
        return builder.ToString();
    }
}
