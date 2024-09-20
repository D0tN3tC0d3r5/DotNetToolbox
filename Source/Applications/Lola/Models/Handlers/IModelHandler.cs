namespace Lola.Models.Handlers;

public interface IModelHandler {
    ModelEntity[] List(uint providerKey = 0);
    ModelEntity? GetById(uint id);
    ModelEntity? Find(Expression<Func<ModelEntity, bool>> predicate);
    void Add(ModelEntity model);
    void Update(ModelEntity model);
    void Remove(uint id);

    void Select(uint id);
    ModelEntity? Selected { get; }
}
