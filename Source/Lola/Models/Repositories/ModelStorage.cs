namespace Lola.Models.Repositories;

public class ModelStorage(IConfiguration configuration)
    : JsonFilePerTypeStorage<ModelEntity, string>("models", configuration),
      IModelStorage {
    protected override bool TryGenerateNextKey([MaybeNullWhen(false)] out string next) {
        next = default;
        return false;
    }
}
