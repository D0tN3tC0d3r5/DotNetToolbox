namespace Sophia.Data.Providers;

public class ProvidersRepository(DataContext dataContext, DbContext dbContext)
    : EntityFrameworkRepository<ProviderData, ProviderEntity>(dataContext, dbContext) {
    protected override Expression<Func<ProviderEntity, bool>> Translate(Expression<Func<ProviderData, bool>> predicate) {
        var parameter = Expression.Parameter(typeof(ProviderEntity), "entity");
        var body = Expression.Invoke(predicate, Expression.Property(parameter, "Data"));
        return Expression.Lambda<Func<ProviderEntity, bool>>(body, parameter);
    }

    protected override Expression<Func<ProviderEntity, ProviderData>> Project { get; } = input => new() {
        Id = input.Id,
        Name = input.Name,
        Models = input.Models.ToList(Mapper.ToModelData),
    };

    protected override Action<ProviderData, ProviderEntity> UpdateFrom { get; } =
        (input, target) => {
            target.Name = input.Name;
            target.Models = input.Models.ToList(Mapper.ToModelEntity);
        };
}
