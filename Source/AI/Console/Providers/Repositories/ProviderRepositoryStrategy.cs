namespace AI.Sample.Providers.Repositories;

public class ProviderRepositoryStrategy(IConfigurationRoot configuration,
                                        Lazy<IProviderRepository> repository)
    : JsonFileRepositoryStrategy<IProviderRepository, ProviderEntity, uint>("providers", configuration, repository),
      IProviderRepositoryStrategy {
    protected override uint FirstKey { get; } = 1;

    protected override bool TryGenerateNextKey(out uint next) {
        next = LastUsedKey == default ? FirstKey : ++LastUsedKey;
        return true;
    }
}
