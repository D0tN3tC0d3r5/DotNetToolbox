namespace Sophia.Data.World;

public class WorldRepository(DataContext dataContext, DbContext dbContext)
    : EntityFrameworkRepository<WorldData, WorldEntity>(dataContext, dbContext) {
    protected override Expression<Func<WorldEntity, bool>> Translate(Expression<Func<WorldData, bool>> predicate) {
        var parameter = Expression.Parameter(typeof(WorldEntity), "entity");
        var body = Expression.Invoke(predicate, Expression.Property(parameter, "Data"));
        return Expression.Lambda<Func<WorldEntity, bool>>(body, parameter);
    }

    protected override Expression<Func<WorldEntity, WorldData>> Project { get; } =
        input => new() { Facts = input.Facts };

    protected override Action<WorldData, WorldEntity> UpdateFrom { get; } =
        (input, target) => target.Facts = input.Facts;
}
