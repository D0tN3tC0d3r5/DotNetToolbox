namespace Lola.Models.Handlers;

public interface IModelHandler {
    ModelEntity[] List(uint providerKey = 0);
    ModelEntity? GetById(uint id);
    ModelEntity? GetByKey(string key);
    ModelEntity? GetByName(string name);
    void Add(ModelEntity model);
    void Update(ModelEntity model);
    void Remove(uint id);

    void Select(uint id);
    ModelEntity? Selected { get; }
}
