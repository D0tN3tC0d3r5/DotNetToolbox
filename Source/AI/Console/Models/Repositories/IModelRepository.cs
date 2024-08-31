namespace AI.Sample.Models.Repositories;

public interface IModelRepository : IRepository<ModelEntity, string> {
    ModelEntity[] GetByProviderKey(uint provider);
}
