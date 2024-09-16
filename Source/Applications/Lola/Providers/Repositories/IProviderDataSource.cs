using DotNetToolbox.Data.DataSources;

namespace Lola.Providers.Repositories;

public interface IProviderDataSource
    : IDataSource<ProviderEntity, uint>;
