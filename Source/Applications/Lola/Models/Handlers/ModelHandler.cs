namespace Lola.Models.Handlers;

public class ModelHandler(IApplication application, IModelDataSource dataSource, ILogger<ModelHandler> logger)
    : IModelHandler {
    private const string _applicationModelKey = "ApplicationModel";

    private ModelEntity? _selected;

    public ModelEntity? Selected {
        get => GetSelected();
        private set => SetSelected(IsNotNull(value));
    }

    private ModelEntity? GetSelected() {
        var cachedValue = application.Context.GetValueAs<ModelEntity>(_applicationModelKey);
        _selected = cachedValue ?? dataSource.GetSelected();
        if (_selected is null) return null;
        if (cachedValue is null) application.Context[_applicationModelKey] = _selected;
        return _selected; // Should only return null if the storage is empty or there is no selected model in the storage.
    }

    private void SetSelected(ModelEntity value) {
        if (value.Key == _selected?.Key) return;
        _selected = value;

        // Ensure record uniqueness in storage
        var oldSelectedModel = dataSource.FirstOrDefault(m => m.Selected);
        if (oldSelectedModel is not null && oldSelectedModel.Key != _selected.Key) {
            oldSelectedModel.Selected = false;
            dataSource.Update(oldSelectedModel);
        }
        _selected.Selected = true;
        dataSource.Update(_selected);

        // Update cached value
        application.Context[_applicationModelKey] = _selected;
    }

    public ModelEntity[] List(uint providerKey = 0)
        => [.. dataSource.GetAll(m => m.ProviderKey == 0 || m.ProviderKey == providerKey).OrderBy(m => m.Name)];

    public ModelEntity? GetByKey(string key)
        => dataSource.FindByKey(key);

    public ModelEntity? GetByName(string name)
        => dataSource.Find(i => i.Name == name);

    public void Add(ModelEntity model) {
        if (dataSource.FindByKey(model.Key) is not null)
            throw new InvalidOperationException($"A model with the key '{model.Key}' already exists.");
        if (_selected is null) model.Selected = true;
        dataSource.Add(model);
        _selected = model;
        logger.LogInformation("Added new model: {ModelKey} => {ModelName}", model.Key, model.Name);
    }

    public void Update(ModelEntity model) {
        if (dataSource.FindByKey(model.Key) == null)
            throw new InvalidOperationException($"Settings with key '{model.Key}' not found.");
        dataSource.Update(model);
        logger.LogInformation("Updated model: {ModelKey} => {ModelName}", model.Key, model.Name);
    }

    public void Remove(string key) {
        var model = dataSource.FindByKey(key, false) ?? throw new InvalidOperationException($"Settings with key '{key}' not found.");

        dataSource.Remove(key);
        logger.LogInformation("Removed model: {ModelKey} => {ModelName}", model.Key, model.Name);
    }

    public ModelEntity[] ListByProvider(uint providerKey) => dataSource.GetAll(m => m.ProviderKey == providerKey);

    public void Select(string key) {
        var model = dataSource.FindByKey(key)
                 ?? throw new InvalidOperationException($"Settings '{key}' not found.");
        Selected = model;
        logger.LogInformation("Settings '{ModelKey} => {ModelName}' selected : ", model.Key, model.Name);
    }
}
