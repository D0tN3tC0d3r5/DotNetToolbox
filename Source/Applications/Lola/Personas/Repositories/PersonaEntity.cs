namespace Lola.Personas.Repositories;

public class PersonaEntity
    : Entity<PersonaEntity, uint> {
    public string Name { get; set; } = string.Empty;
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
        result += ValidateName(Name, IsNotNull(context).GetRequiredValueAs<IPersonaHandler>(nameof(PersonaHandler)));
        result += ValidateRole(Role);
        result += ValidateGoals(Goals);
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

    public static Result ValidateName(string? name, IPersonaHandler handler) {
        var result = Result.Success();
        if (string.IsNullOrWhiteSpace(name))
            result += new ValidationError("The name is required.", nameof(Name));
        else if (handler.GetByName(name) is not null)
            result += new ValidationError("A persona with this name is already registered.", nameof(Name));
        return result;
    }

    public static Result ValidateRole(string? name) {
        var result = Result.Success();
        if (string.IsNullOrWhiteSpace(name))
            result += new ValidationError("The name is required.", nameof(Name));
        return result;
    }

    public static Result ValidateGoal(string? goal) {
        var result = Result.Success();
        if (string.IsNullOrWhiteSpace(goal))
            result += new ValidationError("The goal cannot be null or empty.", nameof(Goals));
        return result;
    }

    public static Result ValidateGoals(List<string> goals) {
        var result = Result.Success();
        if (goals.Count == 0)
            result += new ValidationError("At least one goal is required.", nameof(Goals));
        foreach (var goal in goals)
            result += ValidateGoal(goal);
        return result;
    }
}
