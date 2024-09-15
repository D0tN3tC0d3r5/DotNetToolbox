using DotNetToolbox.Data.DataSources;

namespace Lola.Models.Repositories;

public class ModelDataSource(IModelStorage storage, Lazy<IProviderDataSource> providers)
    : DataSource<IModelStorage, ModelEntity, string>(storage),
      IModelDataSource {
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
