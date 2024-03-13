namespace DotNetToolbox.AI.Personas;

public interface IPersona {
    string Name { get; }
    string? Description { get; }
    Profile Profile { get; }
    List<Skill> Skills { get; }
}
