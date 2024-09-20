namespace Lola.Personas.Repositories;

public class PersonaEntity
    : Entity<PersonaEntity, uint> {
    public string Name { get; set; } = string.Empty;
    public string Role { get; set; } = string.Empty;
    public List<string> Goals { get; set; } = [];

    public List<Query> Questions { get; init; } = [];

    public string Expertise { get; set; } = string.Empty;
    public List<string> Characteristics { get; set; } = [];
    public List<string> Requirements { get; set; } = [];
    public List<string> Restrictions { get; set; } = [];
    public List<string> Traits { get; set; } = [];

    public override Result Validate(IMap? context = null) {
        var result = base.Validate(context);
        var action = IsNotNull(context).GetRequiredValueAs<EntityAction>(nameof(EntityAction));
        result += action == EntityAction.Insert
                      ? ValidateNewName(Name, context.GetRequiredValueAs<IPersonaHandler>(nameof(PersonaHandler)))
                      : ValidateName(Id, Name, context.GetRequiredValueAs<IPersonaHandler>(nameof(PersonaHandler)));
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
        => new(entity.Id) {
            Name = entity.Name,
            Role = entity.Role,
            Goals = entity.Goals,
            Expertise = entity.Expertise,
            Characteristics = entity.Characteristics,
            Requirements = entity.Requirements,
            Restrictions = entity.Restrictions,
            Traits = entity.Traits,
        };

    public static Result ValidateNewName(string? name, IPersonaHandler handler) {
        var result = Result.Success();
        if (string.IsNullOrWhiteSpace(name))
            result += new ValidationError("The name is required.", nameof(Name));
        else if (handler.Find(p => p.Name.Equals(name, StringComparison.OrdinalIgnoreCase)) is not null)
            result += new ValidationError("A persona with this name is already registered.", nameof(Name));
        return result;
    }

    public static Result ValidateName(uint id, string? name, IPersonaHandler handler) {
        var result = Result.Success();
        if (string.IsNullOrWhiteSpace(name))
            result += new ValidationError("The name is required.", nameof(Name));
        else if (handler.Find(p => p.Name.Equals(name, StringComparison.OrdinalIgnoreCase) && p.Id != id) is not null)
            result += new ValidationError("A persona with this name is already registered.", nameof(Name));
        return result;
    }

    public static Result ValidateRole(string? role) {
        var result = Result.Success();
        if (string.IsNullOrWhiteSpace(role))
            result += new ValidationError("The role is required.", nameof(Name));
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
        return goals.Aggregate(result, (current, goal) => current + ValidateGoal(goal));
    }

    public static Result ValidateCharacteristic(string? characteristic) {
        var result = Result.Success();
        if (string.IsNullOrWhiteSpace(characteristic))
            result += new ValidationError("The characteristic cannot be null or empty.", nameof(Characteristics));
        return result;
    }

    public static Result ValidateCharacteristics(List<string> characteristics) {
        var result = Result.Success();
        return characteristics.Aggregate(result, (current, characteristic) => current + ValidateCharacteristic(characteristic));
    }

    public static Result ValidateRequirement(string? requirement) {
        var result = Result.Success();
        if (string.IsNullOrWhiteSpace(requirement))
            result += new ValidationError("The requirement cannot be null or empty.", nameof(Requirements));
        return result;
    }

    public static Result ValidateRequirements(List<string> requirements) {
        var result = Result.Success();
        return requirements.Aggregate(result, (current, requirement) => current + ValidateRequirement(requirement));
    }

    public static Result ValidateRestriction(string? restriction) {
        var result = Result.Success();
        if (string.IsNullOrWhiteSpace(restriction))
            result += new ValidationError("The restriction cannot be null or empty.", nameof(Restrictions));
        return result;
    }

    public static Result ValidateRestrictions(List<string> restrictions) {
        var result = Result.Success();
        return restrictions.Aggregate(result, (current, restriction) => current + ValidateRestriction(restriction));
    }

    public static Result ValidateTrait(string? trait) {
        var result = Result.Success();
        if (string.IsNullOrWhiteSpace(trait))
            result += new ValidationError("The trait cannot be null or empty.", nameof(Characteristics));
        return result;
    }

    public static Result ValidateTraits(List<string> traits) {
        var result = Result.Success();
        return traits.Aggregate(result, (current, trait) => current + ValidateTrait(trait));
    }
}
