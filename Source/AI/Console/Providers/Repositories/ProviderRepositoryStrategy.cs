namespace AI.Sample.Providers.Repositories;

public class ProviderRepositoryStrategy(IConfigurationRoot configuration)
    : JsonFileRepositoryStrategy<IProviderRepository, ProviderEntity, uint>("providers", configuration),
      IProviderRepositoryStrategy {
    protected override uint FirstKey { get; } = 1;

    protected override bool TryGenerateNextKey(out uint next) {
        next = LastUsedKey == default ? FirstKey : ++LastUsedKey;
        return true;
    }
}
