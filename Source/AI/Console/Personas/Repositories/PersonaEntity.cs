namespace AI.Sample.Personas.Repositories;

public class PersonaEntity
    : Entity<PersonaEntity, uint> {
    public string Name { get; set; } = string.Empty;
    public string PrimaryRole { get; set; } = string.Empty;
    public string IntendedUse { get; set; } = string.Empty;
    public List<Query> AdditionalInformation { get; set; } = [];
    public string Prompt { get; set; } = string.Empty;

    public override Result Validate(IContext? context = null) {
        var result = base.Validate(context);
        if (string.IsNullOrWhiteSpace(Name))
            result += new ValidationError("The name is required.", nameof(Name));
        if (string.IsNullOrWhiteSpace(PrimaryRole))
            result += new ValidationError("The primary role is required.", nameof(PrimaryRole));
        if (string.IsNullOrWhiteSpace(IntendedUse))
            result += new ValidationError("The intended use is required.", nameof(PrimaryRole));
        return result;
    }
}
