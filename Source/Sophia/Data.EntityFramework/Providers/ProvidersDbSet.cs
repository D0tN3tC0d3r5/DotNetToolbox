namespace Sophia.Data.Providers;

public class ProvidersDbSet(ApplicationDbContext dbContext) : ProviderRepository {
    public override Task<bool> HaveAny(Expression<Func<ProviderData, bool>> predicate, CancellationToken ct = default) {
        var translator = new ExpressionTranslator<ProviderData, ProviderEntity>();
        var newExpression = translator.Translate<Expression<Func<ProviderEntity, bool>>>(predicate);
        return dbContext.Providers.AnyAsync(newExpression, ct);
    }
}
