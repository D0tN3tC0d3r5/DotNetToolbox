namespace Sophia.Data.Providers;

public class ProvidersRepository(ApplicationDbContext dbContext)
    : EntityFrameworkRepository<ProviderData, ProviderEntity, int>(dbContext.Providers) {
    protected override Expression<Func<ProviderEntity, ProviderData>> ProjectTo { get; }
        = input => Mapper.ToProviderData(input, true);
    protected override Action<ProviderData, ProviderEntity> UpdateFrom { get; }
        = Mapper.UpdateProviderEntity;
    protected override Func<ProviderData, ProviderEntity> CreateFrom { get; }
        = Mapper.ToProviderEntity;
}
