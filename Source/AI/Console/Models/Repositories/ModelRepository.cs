namespace AI.Sample.Models.Repositories;

public class ModelRepository(IModelRepositoryStrategy strategy, Lazy<IProviderRepository> providers)
    : Repository<IModelRepositoryStrategy, ModelEntity, string>(strategy),
      IModelRepository {
    public ModelEntity[] GetAll(Expression<Func<ModelEntity, bool>>? predicate = null, bool includeProviders = true) {
        var models = base.GetAll(predicate);
        if (!includeProviders) return models;
        var providers1 = providers.Value.GetAll();
        foreach (var model in models)
            model.Provider = providers1.FirstOrDefault(p => p.Key == model.ProviderKey);
        return models;
    }

    public ModelEntity? GetSelected() {
        var model = Find(m => m.Selected);
        if (model is null) return null;
        model.Provider = providers.Value.FindByKey(model.ProviderKey);
        return model;
    }

    public ModelEntity? FindByKey(string key, bool includeProvider = true) {
        var model = base.FindByKey(key);
        if (model is null) return null;
        if (!includeProvider) return model;
        model.Provider = providers.Value.FindByKey(model.ProviderKey);
        return model;
    }
}
