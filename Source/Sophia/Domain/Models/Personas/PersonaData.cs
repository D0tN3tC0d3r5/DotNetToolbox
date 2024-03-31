namespace Sophia.Models.Personas;

public class PersonaData
    : IEntity<int> {
    public int Id { get; set; }
    [Required]
    [MinLength(3, ErrorMessage = "Name must be at least 3 characters long.")]
    [MaxLength(100, ErrorMessage = "Name cannot exceed 100 characters.")]
    public string Name { get; set; } = "Agent";
    [Required]
    [MaxLength(1000, ErrorMessage = "Description cannot exceed 1000 characters.")]
    public string Description { get; set; } = "You are a helpful agent.";
    [MaxLength(1000, ErrorMessage = "Personality cannot exceed 1000 characters.")]
    public CharacteristicsData Characteristics { get; set; } = new();
    public List<string> Facts { get; set; } = [];
    public List<ToolData> KnownTools { get; set; } = [];

    public Persona ToModel() => new() {
        Name = Name,
        Description = Description,
        Characteristics = Characteristics.ToModel(),
        Facts = Facts,
        KnownTools = KnownTools.ToList(f => f.ToModel()),
    };
}
