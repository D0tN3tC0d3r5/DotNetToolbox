using DotNetToolbox.Data.Storages;

namespace Lola.Providers.Repositories;

public interface IProviderStorage
    : IStorage<ProviderEntity, uint>;
