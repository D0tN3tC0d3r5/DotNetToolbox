namespace AI.Sample.Models.Repositories;

public interface IModelRepository : IRepository<ModelEntity, string> {
    ModelEntity? GetSelected();
    ModelEntity[] GetFromProvider(string provider);
    ModelEntity[] GetAll(Expression<Func<ModelEntity, bool>>? predicate = null, bool includeProviders = true);
    ModelEntity? FindByKey(string key, bool includeProvider = true);
}
