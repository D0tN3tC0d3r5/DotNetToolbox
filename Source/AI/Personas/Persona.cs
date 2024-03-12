namespace DotNetToolbox.AI.Personas;

public class Persona(Profile? profile = null, List<Skill>? skills = null)
    : IPersona {
    public Profile Profile { get; set; } = profile ?? new();
    public List<Skill> Skills { get; set; } = skills ?? [];
}
