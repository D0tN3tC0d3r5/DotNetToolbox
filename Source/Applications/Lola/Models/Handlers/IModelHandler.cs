namespace Lola.Models.Handlers;

public interface IModelHandler {
    ModelEntity[] List(uint providerKey = 0);
    ModelEntity? GetByKey(string key);
    ModelEntity? GetByName(string name);
    void Add(ModelEntity model);
    void Update(ModelEntity model);
    void Remove(string key);

    void Select(string key);
    ModelEntity? Selected { get; }
}
