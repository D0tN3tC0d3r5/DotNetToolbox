namespace Sophia.Models.Personas;

public class PersonaData {
    public int Id { get; set; }
    [Required]
    [MinLength(3, ErrorMessage = "Name must be at least 3 characters long.")]
    [MaxLength(100, ErrorMessage = "Name cannot exceed 100 characters.")]
    public string Name { get; set; } = "Agent";
    [Required]
    [MaxLength(1000, ErrorMessage = "Description cannot exceed 1000 characters.")]
    public string Description { get; set; } = "You are a helpful agent.";
    [MaxLength(1000, ErrorMessage = "Personality cannot exceed 1000 characters.")]
    public string? Personality { get; set; }
    public List<string> Instructions { get; set; } = [];
    public List<FactData> Facts { get; set; } = [];
    public List<ToolData> KnownTools { get; set; } = [];

    public string? ValidateInstructions()
        => Instructions.Any(string.IsNullOrWhiteSpace)
               ? "Instructions cannot contain empty or whitespace strings."
               : Instructions.Count != Instructions.Distinct().Count()
                   ? "Instructions cannot contain duplicated values."
                   : null;

    public Persona ToModel() => new() {
        Name = Name,
        Description = Description,
        Personality = Personality,
        Instructions = Instructions,
        Facts = Facts.ToList(f => f.ToModel()),
        KnownTools = KnownTools.ToList(f => f.ToModel()),
    };
}
