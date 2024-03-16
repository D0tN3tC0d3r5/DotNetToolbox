namespace DotNetToolbox.AI.Personas;

[method: JsonConstructor]
public class Persona(Profile? profile = null, List<Skill>? skills = null) {

    public Persona(string name, string? description = null, Profile? profile = null, List<Skill>? skills = null)
        : this(profile, skills) {
        Name = IsNotNull(name);
        Description = description;
    }

    public string Name { get; set; } = "Agent";
    public string? Description { get; set; }
    public Profile Profile { get; set; } = profile ?? new();
    public List<Skill> Skills { get; set; } = skills ?? [];
}
