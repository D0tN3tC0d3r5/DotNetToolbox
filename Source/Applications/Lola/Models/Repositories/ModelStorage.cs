namespace Lola.Models.Repositories;

public class ModelStorage(IConfiguration configuration)
    : JsonFilePerTypeStorage<ModelEntity, uint>("models", configuration),
      IModelStorage {
    protected override uint FirstKey { get; } = 1;

    protected override bool TryGenerateNextKey(out uint next) {
        next = LastUsedKey == default ? FirstKey : ++LastUsedKey;
        return true;
    }
}
