namespace DotNetToolbox.AI.Personas;

public interface IPersona {
    Profile Profile { get; set; }
    List<Skill> Skills { get; set; }
}
