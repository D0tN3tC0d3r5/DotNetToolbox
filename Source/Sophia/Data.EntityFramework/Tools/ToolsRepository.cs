namespace Sophia.Data.Tools;

public class ToolsRepository(DataContext dataContext, DbContext dbContext)
    : EntityFrameworkRepository<ToolData, ToolEntity>(dataContext, dbContext) {
    protected override Expression<Func<ToolEntity, bool>> Translate(Expression<Func<ToolData, bool>> predicate) {
        var parameter = Expression.Parameter(typeof(ToolEntity), "entity");
        var body = Expression.Invoke(predicate, Expression.Property(parameter, "Data"));
        return Expression.Lambda<Func<ToolEntity, bool>>(body, parameter);
    }

    protected override Expression<Func<ToolEntity, ToolData>> Project { get; } =
        input => new() {
            Id = input.Id,
            Name = input.Name,
            Description = input.Description,
            Arguments = input.Arguments.ToList(ToArgumentData),
        };

    protected override Action<ToolData, ToolEntity> UpdateFrom { get; } =
        (input, target) => {
            target.Name = input.Name;
            target.Description = input.Description;
            target.Arguments = input.Arguments.AsIndexed().ToList(ToArgumentEntity);
        };

    private static ArgumentData ToArgumentData(ArgumentEntity input)
        => new() {
            Name = input.Name,
            Description = input.Description,
            Type = input.Type,
            Choices = [.. input.Choices],
            IsRequired = input.IsRequired,
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
