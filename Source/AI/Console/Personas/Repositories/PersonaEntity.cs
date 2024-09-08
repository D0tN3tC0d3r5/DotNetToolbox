namespace AI.Sample.Personas.Repositories;

public class PersonaEntity
    : Entity<PersonaEntity, uint> {
    public string Name { get; init; } = string.Empty;
    public string Role { get; init; } = string.Empty;
    public List<string> Goals { get; init; } = [];
    public string Expertise { get; init; } = string.Empty;
    public List<Query> Questions { get; init; } = [];
    public List<string> Traits { get; init; } = [];
    public List<string> Important { get; init; } = [];
    public List<string> Negative { get; init; } = [];
    public List<string> Other { get; init; } = [];

    public override Result Validate(IContext? context = null) {
        var result = base.Validate(context);
        if (string.IsNullOrWhiteSpace(Name)) result += new ValidationError("The name is required.", nameof(Name));
        if (string.IsNullOrWhiteSpace(Role)) result += new ValidationError("The primary role is required.", nameof(Role));
        if (Goals.Count == 0) result += new ValidationError("At least one goal is required.", nameof(Goals));
        return result;
    }

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
