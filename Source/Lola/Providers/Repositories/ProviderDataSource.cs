using DotNetToolbox.Data.DataSources;

namespace Lola.Providers.Repositories;

public class ProviderDataSource(IProviderStorage strategy)
    : DataSource<IProviderStorage, ProviderEntity, uint>(strategy),
      IProviderDataSource;
