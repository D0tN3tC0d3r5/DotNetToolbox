﻿namespace DotNetToolbox.AI.Personas;

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

    public override string ToString() {
        var builder = new StringBuilder();
        builder.AppendLine($"Your name is {Name}.");
        builder.AppendLine(Description);
        builder.AppendLine(Characteristics.ToString());
        builder.AppendSection(KnownTools, "Known Tools");
        builder.AppendSection(Facts);
        return builder.ToString();
    }
}
