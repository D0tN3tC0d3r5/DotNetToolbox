namespace AI.Sample.Providers.Repositories;

public class ProviderRepository(IProviderRepositoryStrategy strategy)
    : Repository<IProviderRepositoryStrategy, ProviderEntity, uint>(strategy),
      IProviderRepository;
