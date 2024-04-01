namespace Sophia.Data.Providers;

public class ProvidersRepository(DataContext dataContext, ApplicationDbContext dbContext)
    : EntityFrameworkRepository<ProviderData, ProviderEntity, int>(dataContext, dbContext.Providers) {
    protected override Expression<Func<ProviderEntity, ProviderData>> ProjectTo { get; }
        = input => Mapper.ToProviderData(input, true);
    protected override Action<ProviderData, ProviderEntity> UpdateFrom { get; }
        = Mapper.UpdateProviderEntity;
    protected override Func<ProviderData, ProviderEntity> CreateFrom { get; }
        = Mapper.ToProviderEntity;
}
