namespace AI.Sample.Providers.Repositories;

public class ProviderRepositoryStrategy(IConfigurationRoot configuration,
                                        Lazy<IProviderRepository> repository)
    : JsonFileRepositoryStrategy<IProviderRepository, ProviderEntity, uint, INumericSequencer>("providers", configuration, repository),
      IProviderRepositoryStrategy {
    protected override uint FirstKey { get; } = 1;

    protected override Result<uint> GenerateNextKey() {
        if (LastUsedKey == default) LastUsedKey = FirstKey;
        else ++LastUsedKey;
        return Result.Success(LastUsedKey);
    }
}
