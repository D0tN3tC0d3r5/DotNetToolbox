using System.Diagnostics.CodeAnalysis;

namespace AI.Sample.Models.Repositories;

public class ModelRepositoryStrategy(IConfigurationRoot configuration, Lazy<IModelRepository> repository)
    : JsonFileRepositoryStrategy<IModelRepository, ModelEntity, string>("models", configuration, repository),
      IModelRepositoryStrategy {
    protected override bool TryGenerateNextKey([MaybeNullWhen(false)] out string next) {
        next = default;
        return false;
    }
}
