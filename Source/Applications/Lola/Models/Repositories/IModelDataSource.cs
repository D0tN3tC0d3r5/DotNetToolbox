using DotNetToolbox.Data.DataSources;

namespace Lola.Models.Repositories;

public interface IModelDataSource : IDataSource<ModelEntity, string> {
    ModelEntity? GetSelected();
    ModelEntity[] GetAll(Expression<Func<ModelEntity, bool>>? predicate = null, bool includeProviders = true);
    ModelEntity? FindByKey(string key, bool includeProvider = true);
}
