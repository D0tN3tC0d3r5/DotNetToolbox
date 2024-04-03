namespace Sophia.Data.Providers;

public class ProvidersDbSet(ApplicationDbContext dbContext) : ProviderRepository {
    public override Task<bool> HaveAny(Expression<Func<ProviderData, bool>> predicate, CancellationToken ct = default) {
        return dbContext.Providers.AnyAsync(i => i.Name == "OpenAI", ct);
        //var translator = new ExpressionTranslator<ProviderData, ProviderEntity>();
        //var newExpression = translator.Translate<Expression<Func<ProviderEntity, bool>>>(predicate);
        //return dbContext.Providers.AnyAsync(newExpression, ct);
    }
}
