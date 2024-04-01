namespace Sophia.Data.Providers;

public class ProvidersRepository(DataContext dataContext, ApplicationDbContext dbContext)
    : EntityFrameworkRepository<ProviderData, ProviderEntity, int>(dataContext, dbContext.Providers) {
    protected override Expression<Func<ProviderEntity, ProviderData>> Project { get; }
        = input => Mapper.ToProviderData(input, true);
    protected override Action<ProviderData, ProviderEntity> UpdateFrom { get; }
        = Mapper.UpdateProviderEntity;
    protected override Func<ProviderData, ProviderEntity> Create { get; }
        = Mapper.ToProviderEntity;

    public override async Task<bool> HaveAny(Expression<Func<ProviderData, bool>>? predicate, CancellationToken ct = default) {
        var test1 = await Set.AnyAsync(ct);
        var newExpression = SwitchSource(predicate);
        test1 = await Set.AnyAsync(newExpression, ct);
        return await base.HaveAny(predicate, ct);
    }
}
