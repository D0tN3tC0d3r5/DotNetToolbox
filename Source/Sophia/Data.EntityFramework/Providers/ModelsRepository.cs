namespace Sophia.Data.Providers;

public class ModelsRepository(DataContext dataContext, DbContext dbContext)
    : EntityFrameworkRepository<ModelData, ModelEntity>(dataContext, dbContext) {
    protected override Expression<Func<ModelEntity, bool>> Translate(Expression<Func<ModelData, bool>> predicate) {
        var parameter = Expression.Parameter(typeof(ModelEntity), "entity");
        var body = Expression.Invoke(predicate, Expression.Property(parameter, "Data"));
        return Expression.Lambda<Func<ModelEntity, bool>>(body, parameter);
    }

    protected override Expression<Func<ModelEntity, ModelData>> Project { get; } =
        input => new() {
            Id = input.ModelId,
            Name = input.Name,
        };

    protected override Action<ModelData, ModelEntity> UpdateFrom { get; } =
        (input, target) => target.Name = input.Name;
}
