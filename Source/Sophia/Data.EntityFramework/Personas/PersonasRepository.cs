namespace Sophia.Data.Personas;

public class PersonasRepository(DataContext dataContext, DbContext dbContext)
    : EntityFrameworkRepository<PersonaData, PersonaEntity>(dataContext, dbContext) {
    protected override Expression<Func<PersonaEntity, bool>> Translate(Expression<Func<PersonaData, bool>> predicate) {
        var parameter = Expression.Parameter(typeof(PersonaEntity), "entity");
        var body = Expression.Invoke(predicate, Expression.Property(parameter, "Data"));
        return Expression.Lambda<Func<PersonaEntity, bool>>(body, parameter);
    }

    protected override Expression<Func<PersonaEntity, PersonaData>> Project { get; } = input => new() {
        Id = input.Id,
        Name = input.Name,
        Description = input.Description,
        Characteristics = ToCharacteristicsData(input.Characteristics),
        Facts = input.Facts,
        KnownTools = input.Tools.ToList(ToToolData),
    };

    protected override Action<PersonaData, PersonaEntity> UpdateFrom { get; }
    = (input, target) => {
        target.Id = input.Id;
        target.Name = input.Name;
        target.Description = input.Description;
        target.Characteristics = ToCharacteristicsEntity(input.Characteristics);
        target.Facts = input.Facts;
        target.Tools = input.KnownTools.ToList(ToToolEntity);
    };

    private static CharacteristicsData ToCharacteristicsData(CharacteristicsEntity input)
        => new() {
            Cognition = input.Cognition,
            Disposition = input.Disposition,
            Interaction = input.Interaction,
            Attitude = input.Attitude,
        };

    private static CharacteristicsEntity ToCharacteristicsEntity(CharacteristicsData input)
        => new() {
            Cognition = input.Cognition,
            Disposition = input.Disposition,
            Interaction = input.Interaction,
            Attitude = input.Attitude,
        };

    private static ToolData ToToolData(ToolEntity input)
        => new() {
            Name = input.Name,
            Description = input.Description,
            Arguments = input.Arguments.AsIndexed().ToList(ToArgumentData),
        };

    private static ArgumentData ToArgumentData(Indexed<ArgumentEntity> input)
        => new() {
            Index = input.Index,
            Name = input.Value.Name,
            Description = input.Value.Description,
            Type = input.Value.Type,
            Choices = input.Value.Type != ArgumentType.Enum ? [] : [.. input.Value.Choices],
            IsRequired = input.Value.IsRequired,
        };

    private static ToolEntity ToToolEntity(ToolData input)
        => new() {
            Name = input.Name,
            Description = input.Description,
            Arguments = input.Arguments.AsIndexed().ToList(ToArgumentEntity),
        };

    private static ArgumentEntity ToArgumentEntity(Indexed<ArgumentData> input)
        => new() {
            Index = input.Index,
            Name = input.Value.Name,
            Description = input.Value.Description,
            Type = input.Value.Type,
            Choices = input.Value.Type != ArgumentType.Enum ? [] : [.. input.Value.Choices],
            IsRequired = input.Value.IsRequired,
        };
}
