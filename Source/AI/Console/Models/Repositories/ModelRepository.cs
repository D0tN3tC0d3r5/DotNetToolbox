namespace AI.Sample.Models.Repositories;

public class ModelRepository(IModelRepositoryStrategy strategy)
    : Repository<IModelRepositoryStrategy, ModelEntity, string>(strategy),
      IModelRepository {
    public ModelEntity[] GetByProviderKey(uint providerKey) {
        var allModels = GetAll();
        return allModels.Where(m => m.ProviderKey == providerKey).ToArray();
    }
}
