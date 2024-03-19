namespace DotNetToolbox.AI.Personas;

[method: JsonConstructor]
public class Persona(Profile? profile = null) {

    public Persona(string name, string? description = null, Profile? profile = null)
        : this(profile) {
        Name = IsNotNull(name);
        Description = description;
    }

    public string Name { get; set; } = "Agent";
    public string? Description { get; set; }
    public Profile Profile { get; set; } = profile ?? new();

    public override string ToString() {
        var builder = new StringBuilder();
        builder.AppendLine($"You are a helpful {Name}.");
        builder.AppendLine(Description);
        builder.AppendLine(Profile.ToString());
        return builder.ToString();
    }
}
