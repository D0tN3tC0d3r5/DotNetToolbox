namespace DotNetToolbox.AI.Personas;

[method: JsonConstructor]
public class Persona() {
    public Persona(string name)
        : this() {
        Name = IsNotNull(name);
    }

    public string Name { get; set; } = "Agent";
    public string Description { get; set; } = "You are a helpful ASSISTANT.";
    public Characteristics Characteristics { get; set; } = new();
    public List<string> Facts { get; set; } = [];
    public List<Tool> KnownTools { get; set; } = [];

    public string GetIndentedText(string indent) {
        var builder = new StringBuilder();
        builder.AppendLine($"{indent}Your name is {Name}.");
        builder.AppendLine($"{indent}{Description}");
        builder.AppendSection(indent, Characteristics);
        builder.AppendSection(indent, Facts);
        if (KnownTools.Count > 0) {
            builder.AppendIntoNewLine($"{indent}Known Tools");
            foreach (var tool in KnownTools)
                builder.AppendSection(indent, tool);
        }
        return builder.ToString();
    }
}
