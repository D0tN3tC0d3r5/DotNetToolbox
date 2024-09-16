namespace Lola.Providers.Repositories;

public sealed class ProviderDataSource(IProviderStorage storage)
    : DataSource<IProviderStorage, ProviderEntity, uint>(storage),
      IProviderDataSource;
