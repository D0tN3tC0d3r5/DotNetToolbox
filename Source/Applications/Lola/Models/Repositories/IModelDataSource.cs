namespace Lola.Models.Repositories;

public interface IModelDataSource : IDataSource<ModelEntity, uint> {
    ModelEntity? GetSelected();
    ModelEntity[] GetAll(Expression<Func<ModelEntity, bool>>? predicate = null, bool includeProviders = true);
    ModelEntity? FindById(uint id, bool includeProvider = true);
}
