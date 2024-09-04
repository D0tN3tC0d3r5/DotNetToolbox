namespace AI.Sample.Models.Repositories;

public class ModelRepository(IModelRepositoryStrategy strategy, Lazy<IProviderRepository> providers)
    : Repository<IModelRepositoryStrategy, ModelEntity, string>(strategy),
      IModelRepository {
    private readonly Lazy<IProviderRepository> _providers = providers;

    public ModelEntity[] GetFromProvider(string provider) {
        if (uint.TryParse(provider, out var key))
            return GetAll(m => m.ProviderKey == key);

        var entity = _providers.Value.Find(p => p.Name == provider);
        return entity is null
            ? []
            : GetAll(m => m.ProviderKey == entity.Key);
    }

    public ModelEntity[] GetAll(Expression<Func<ModelEntity, bool>>? predicate = null, bool includeProviders = true) {
        var models = GetAll(predicate);
        if (!includeProviders) return models;
        var providers = _providers.Value.GetAll();
        foreach (var model in models)
            model.Provider = providers.FirstOrDefault(p => p.Key == model.ProviderKey);
        return models;
    }

    public ModelEntity? FindByKey(string key, bool includeProvider = true) {
        var model = FindByKey(key);
        if (model is null) return null;
        if (!includeProvider) return model;
        model.Provider = _providers.Value.FindByKey(model.ProviderKey);
        return model;
    }
}
