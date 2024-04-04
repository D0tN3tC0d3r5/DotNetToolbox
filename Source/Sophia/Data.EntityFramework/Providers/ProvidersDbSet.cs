namespace Sophia.Data.Providers;

public class ProvidersDbSet(ApplicationDbContext dbContext)
    : ProviderRepository {
    public override Task<bool> HaveAny(Expression<Func<ProviderData, bool>> predicate, CancellationToken ct = default) {
        var translator = new LambdaExpressionConversionVisitor<ProviderData, ProviderEntity>();
        var newExpression = translator.Translate<Expression<Func<ProviderEntity, bool>>>(predicate);
        return dbContext.Providers.AnyAsync(newExpression, ct);
    }

    public override async Task<IReadOnlyList<ProviderData>> ToArrayAsync(CancellationToken ct = default)
        => await dbContext.Providers
                          .AsNoTracking()
                          .Include(i => i.Models)
                          .Select(i => Mapper.ToProviderData(i, false))
                          .ToArrayAsync(ct);

    public override async Task<ProviderData?> FindByKey(int key, CancellationToken ct = default) {
        var entity = await dbContext.Providers
                                    .AsNoTracking()
                                    .Include(i => i.Models)
                                    .FirstOrDefaultAsync(i => i.Id == key, ct);
        return entity == null
            ? null
            : Mapper.ToProviderData(entity, includeModels: true);
    }

    public override async ValueTask Add(ProviderData input, CancellationToken ct = default) {
        var alreadyExists = await dbContext.Providers.AnyAsync(i => i.Id == input.Id, ct);
        if (alreadyExists)
            return;
        var entity = Mapper.ToProviderEntity(input);
        await dbContext.Providers.AddAsync(entity, ct);
        await dbContext.SaveChangesAsync(ct);
    }

    public override async Task Update(ProviderData input, CancellationToken ct = default) {
        var entity = await dbContext.Providers
                                    .Include(i => i.Models)
                                    .FirstOrDefaultAsync(i => i.Id == input.Id, ct);
        if (entity == null)
            return;
        Mapper.UpdateProviderEntity(input, entity);
        await dbContext.SaveChangesAsync(ct);
    }

    public override async Task Remove(int key, CancellationToken ct = default) {
        var entity = await dbContext.Providers
                                    .FirstOrDefaultAsync(i => i.Id == key, ct);
        if (entity == null)
            return;
        dbContext.Providers.Remove(entity);
        await dbContext.SaveChangesAsync(ct);
    }
}
