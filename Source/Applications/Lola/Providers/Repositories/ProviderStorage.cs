namespace Lola.Providers.Repositories;

public sealed class ProviderStorage(IConfiguration configuration)
    : JsonFilePerTypeStorage<ProviderEntity, uint>("providers", configuration),
      IProviderStorage {
    protected override uint FirstKey { get; } = 1;

    protected override bool TryGenerateNextKey(out uint next) {
        next = LastUsedKey == default ? FirstKey : ++LastUsedKey;
        return true;
    }
}
