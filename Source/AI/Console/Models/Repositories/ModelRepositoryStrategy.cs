using System.Diagnostics.CodeAnalysis;

namespace AI.Sample.Models.Repositories;

public class ModelRepositoryStrategy(IConfigurationRoot configuration)
    : JsonFilePerTypeRepositoryStrategy<IModelRepository, ModelEntity, string>("models", configuration),
      IModelRepositoryStrategy {
    protected override bool TryGenerateNextKey([MaybeNullWhen(false)] out string next) {
        next = default;
        return false;
    }
}
