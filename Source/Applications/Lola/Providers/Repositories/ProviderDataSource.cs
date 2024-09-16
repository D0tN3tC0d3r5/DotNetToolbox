namespace Lola.Providers.Repositories;

public class ProviderDataSource(IProviderStorage storage)
    : DataSource<IProviderStorage, ProviderEntity, uint>(storage),
      IProviderDataSource;
