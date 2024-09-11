namespace AI.Sample.Models.Handlers;

public class ModelHandler(IApplication application, IModelRepository repository, ILogger<ModelHandler> logger)
    : IModelHandler {
    private const string _applicationModelKey = "ApplicationModel";

    private ModelEntity? _selected;

    public ModelEntity? Internal {
        get => GetSelected();
        private set => SetSelected(IsNotNull(value));
    }

    private ModelEntity? GetSelected() {
        var cachedValue = application.Map.GetValueAs<ModelEntity>(_applicationModelKey);
        _selected = cachedValue ?? repository.GetSelected();
        if (_selected is null) return null;
        if (cachedValue is null) application.Map[_applicationModelKey] = _selected;
        return _selected; // Should only return null if the storage is empty or there is no selected model in the storage.
    }

    private void SetSelected(ModelEntity value) {
        if (value.Key == _selected?.Key) return;
        _selected = value;

        // Ensure record uniqueness in storage
        var oldSelectedModel = repository.FirstOrDefault(m => m.Selected);
        if (oldSelectedModel is not null && oldSelectedModel.Key != _selected.Key) {
            oldSelectedModel.Selected = false;
            repository.Update(oldSelectedModel);
        }
        _selected.Selected = true;
        repository.Update(_selected);

        // Update cached value
        application.Map[_applicationModelKey] = _selected;
    }

    public ModelEntity[] List()
        => [.. repository.GetAll().OrderBy(m => m.Name)];

    public ModelEntity? GetByKey(string key)
        => repository.FindByKey(key);

    public ModelEntity Create(Action<ModelEntity> setUp) {
        var model = new ModelEntity();
        setUp(model);
        return model;
    }

    public void Register(ModelEntity model) {
        if (repository.FindByKey(model.Key) is not null)
            throw new InvalidOperationException($"A model with the key '{model.Key}' already exists.");
        if (_selected is null) model.Selected = true;
        repository.Add(model);
        _selected = model;
        logger.LogInformation("Added new model: {ModelKey} => {ModelName}", model.Key, model.Name);
    }

    public void Update(ModelEntity model) {
        if (repository.FindByKey(model.Key) == null)
            throw new InvalidOperationException($"Settings with key '{model.Key}' not found.");
        repository.Update(model);
        logger.LogInformation("Updated model: {ModelKey} => {ModelName}", model.Key, model.Name);
    }

    public void Remove(string key) {
        var model = repository.FindByKey(key, false) ?? throw new InvalidOperationException($"Settings with key '{key}' not found.");

        repository.Remove(key);
        logger.LogInformation("Removed model: {ModelKey} => {ModelName}", model.Key, model.Name);
    }

    public ModelEntity[] ListByProvider(string provider) => repository.GetFromProvider(provider);

    public void RemoveByProvider(string provider) {
        foreach (var model in repository.GetFromProvider(provider)) {
            repository.Remove(model.Key);
            logger.LogInformation("Removed model associated with provider {Provider}: {ModelKey} => {ModelName}", provider, model.Key, model.Name);
        }
    }

    public void Select(string key) {
        var model = repository.FindByKey(key)
                 ?? throw new InvalidOperationException($"Settings '{key}' not found.");
        Internal = model;
        logger.LogInformation("Settings '{ModelKey} => {ModelName}' selected : ", model.Key, model.Name);
    }
}
