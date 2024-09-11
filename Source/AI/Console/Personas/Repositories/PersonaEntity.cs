using DotNetToolbox.AI.Personas;

namespace AI.Sample.Personas.Repositories;

public class PersonaEntity
    : Entity<PersonaEntity, uint> {
    public string Name { get; init; } = string.Empty;
    public string Role { get; set; } = string.Empty;
    public List<string> Goals { get; set; } = [];

    public List<Query> Questions { get; init; } = [];

    public string Expertise { get; set; } = string.Empty;
    public List<string> Traits { get; set; } = [];
    public List<string> Important { get; set; } = [];
    public List<string> Negative { get; set; } = [];
    public List<string> Other { get; set; } = [];

    public override Result Validate(IMap? context = null) {
        var result = base.Validate(context);
        if (string.IsNullOrWhiteSpace(Name)) result += new ValidationError("The name is required.", nameof(Name));
        if (string.IsNullOrWhiteSpace(Role)) result += new ValidationError("The primary role is required.", nameof(Role));
        if (Goals.Count == 0) result += new ValidationError("At least one goal is required.", nameof(Goals));
        return result;
    }

    public static implicit operator Map(PersonaEntity entity)
        => new() {
            ["Name"] = entity.Name,
            ["Role"] = entity.Role,
            ["Goals"] = entity.Goals,
            ["Questions"] = entity.Questions,
        };

    public static implicit operator Persona(PersonaEntity entity)
        => new(entity.Key) {
            Name = entity.Name,
            Role = entity.Role,
            Goals = entity.Goals,
            Expertise = entity.Expertise,
            Traits = entity.Traits,
            Important = entity.Important,
            Negative = entity.Negative,
            Other = entity.Other,
        };
}
