namespace Sophia.Data.Providers;

public class ProvidersRepository(DataContext dataContext, DbContext dbContext)
    : EntityFrameworkRepository<ProviderData, int, ProviderEntity, int>(dataContext, dbContext) {
    //protected override Expression<Func<ProviderEntity, ProviderData>> Project { get; }
    //    = input => Mapper.ToProviderData(input);

    //protected override Action<ProviderData, ProviderEntity> UpdateFrom { get; }
    //    = Mapper.UpdateProviderEntity;
}
